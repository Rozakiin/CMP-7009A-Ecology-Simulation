using Components;
using MonoBehaviourTools.Grid;
using MonoBehaviourTools.UI;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Systems
{
    public class PathFindingSystem : SystemBase
    {
        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 14; //14 used as approximate of sqrt(2)*10 for diagonals

        private EndSimulationEntityCommandBufferSystem _ecbSystem;
        private NativeArray<PathNode> _pathNodeArray;


        protected override void OnCreate()
        {
            base.OnCreate();
            _ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        /*
         * calculates the shortest path of grid nodes between two points
         * using A-Star algorithm
         */
        protected override void OnUpdate()
        {
            //catch to not run if paused
            if (UITimeControl.Instance.GetPause()) return;

            var ecb = _ecbSystem.CreateCommandBuffer().ToConcurrent();

            if (!_pathNodeArray.IsCreated)
                _pathNodeArray = CreatePathNodeArray();
            var tempArray = new NativeArray<PathNode>(_pathNodeArray, Allocator.TempJob);

            var worldSize = SimulationManager.WorldSize;
            var gridSize = GridManager.Instance.GridSize;

            // goes through each entity requesting a path and finds a path
            Entities.ForEach((
                Entity entity,
                int entityInQueryIndex,
                ref DynamicBuffer<PathPositionData> pathPositionDataBuffer,
                ref PathFindingRequestData pathFindingRequestData,
                ref PathFollowData pathFollowData,
                in TargetData targetData
            ) =>
            {
                var tmpPathNodeArray = new NativeArray<PathNode>(tempArray, Allocator.Temp);

                //Find a path
                var findPathJob = new FindPathJob
                {
                    GridSize = gridSize,
                    WorldSize = worldSize,
                    PathNodeArray = tmpPathNodeArray,
                    StartPosition = pathFindingRequestData.StartPosition,
                    EndPosition = pathFindingRequestData.EndPosition,
                    IterationLimit = 10 * (int) targetData.SightRadius, //arbitrary decision should have better way to determine how long a path should be
                    PathFollowData = pathFollowData,
                    PathPositionDataBuffer = pathPositionDataBuffer
                };
                findPathJob.Execute(); //execute the find path job

                //update the edited component data from findPathJob
                pathFollowData = findPathJob.PathFollowData;

                ecb.RemoveComponent<PathFindingRequestData>(entityInQueryIndex, entity); //remove the PathFindingRequestData from entity
            }).WithDeallocateOnJobCompletion(tempArray).ScheduleParallel();

            // Make sure that the ECB system knows about our job
            _ecbSystem.AddJobHandleForProducer(Dependency);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_pathNodeArray.IsCreated)
                _pathNodeArray.Dispose();
        }

        //Creates a PathNode NativeArray for use in pathfinding from the GridNode array
        private NativeArray<PathNode> CreatePathNodeArray()
        {
            var grid = GridManager.Instance.Grid;
            var gridSize = GridManager.Instance.GridSize;
            var gridMaxSize = GridManager.Instance.GridMaxSize;
            var pathNodeArray = new NativeArray<PathNode>(gridMaxSize, Allocator.Persistent);

            for (var x = 0; x < gridSize.x; x++)
            for (var y = 0; y < gridSize.y; y++)
            {
                var pathNode = new PathNode
                {
                    X = x,
                    Y = y,
                    Index = CalculateIndex(x, y, gridSize.x),

                    GCost = int.MaxValue,

                    IsWalkable = grid[x, y].IsWalkable,
                    Penalty = grid[x, y].MovementPenalty,
                    Position = grid[x, y].WorldPosition,

                    CameFromNodeIndex = -1
                };

                pathNodeArray[pathNode.Index] = pathNode;
            }

            return pathNodeArray;
        }

        //Gets the closest node to the given world position.
        private static PathNode NodeFromWorldPoint(in float3 worldPos, in float2 worldSize, in int2 gridSize,
            in NativeArray<PathNode> pathNodeArray)
        {
            // how far along the grid the position is (left 0, middle 0.5, right 1)
            var percentX = worldPos.x / worldSize.x + 0.5f; // optimisation of maths
            var percentY = worldPos.z / worldSize.y + 0.5f;

            //clamp percent between 0 and 1
            percentX = math.clamp(percentX, 0, 1);
            percentY = math.clamp(percentY, 0, 1);

            // calc x,y position in the node array for the world position
            var x = Mathf.FloorToInt(math.min(gridSize.x * percentX, gridSize.x - 1));
            var y = Mathf.FloorToInt(math.min(gridSize.y * percentY, gridSize.y - 1));

            return pathNodeArray[CalculateIndex(x, y, gridSize.x)]; // position of closest node in array
        }

        //calc the index in the 1d array from the 2d array
        private static int CalculateIndex(in int x, in int y, in int gridSizeX)
        {
            return x + y * gridSizeX;
        }

        // if using diagonals
        private static int GetDistance(in PathNode nodeA, in PathNode nodeB)
        {
            var x = math.abs(nodeA.X - nodeB.X); //x1-x2
            var y = math.abs(nodeA.Y - nodeB.Y); //y1-y2
            if (x > y) return MoveDiagonalCost * y + MoveStraightCost * (x - y);
            return MoveDiagonalCost * x + MoveStraightCost * (y - x);
        }

        private static bool IsPositionInsideGrid(in int2 gridPosition, in int2 gridSize)
        {
            return
                gridPosition.x >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
        }

        private static int GetLowestCostFNodeIndex(in NativeList<int> openList, in NativeArray<PathNode> pathNodeArray)
        {
            var lowestCostPathNode = pathNodeArray[openList[0]]; //set to first in array
            //compare all in array to find lowest FCost with lowest hCost
            for (var i = 1; i < openList.Length; i++)
            {
                var testPathNode = pathNodeArray[openList[i]]; //pathnode to compare to
                if (testPathNode.FCost < lowestCostPathNode.FCost)
                    lowestCostPathNode = testPathNode;
                else if (testPathNode.FCost == lowestCostPathNode.FCost) //Tiebreaker
                    //Compare hcosts
                    if (testPathNode.HCost < lowestCostPathNode.HCost)
                        lowestCostPathNode = testPathNode;
            }

            return lowestCostPathNode.Index;
        }

        [BurstCompile]
        private struct FindPathJob : IJob
        {
            [ReadOnly] public int2 GridSize;
            [ReadOnly] public float2 WorldSize;

            public NativeArray<PathNode> PathNodeArray;
            [ReadOnly] public float3 StartPosition;
            public float3 EndPosition;

            [ReadOnly]public int IterationLimit;

            public PathFollowData PathFollowData;
            public DynamicBuffer<PathPositionData> PathPositionDataBuffer;
            public void Execute()
            {
                
                FindPath(StartPosition, ref EndPosition, ref PathNodeArray, IterationLimit, GridSize, WorldSize);

                var endNode = NodeFromWorldPoint(EndPosition, WorldSize, GridSize, PathNodeArray);
                SetBufferPath(PathNodeArray, endNode, ref PathFollowData, ref PathPositionDataBuffer);
            }

            private static void FindPath(in float3 startPosition, ref float3 endPosition, ref NativeArray<PathNode> pathNodeArray, in int iterationLimit, in int2 gridSize, in float2 worldSize)
            {
                var iterations = iterationLimit;
                var startNode = NodeFromWorldPoint(startPosition, worldSize, gridSize, pathNodeArray);
                var targetNode = NodeFromWorldPoint(endPosition, worldSize, gridSize, pathNodeArray);
                //Only path find if start and end are walkable
                if (startNode.IsWalkable && targetNode.IsWalkable)
                {
                    var neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp)
                    {
                        [0] = new int2(-1, 0), // Left
                        [1] = new int2(+1, 0), // Right
                        [2] = new int2(0, +1), // Up
                        [3] = new int2(0, -1), // Down
                        [4] = new int2(-1, -1), // Left Down
                        [5] = new int2(-1, +1), // Left Up
                        [6] = new int2(+1, -1), // Right Down
                        [7] = new int2(+1, +1) // Right Up
                    };

                    //open and closed list of indexes of nodes 
                    var openList = new NativeList<int>(Allocator.Temp); 
                    var closedList = new NativeList<int>(Allocator.Temp); 

                    //Add the starting node to the open list
                    openList.Add(startNode.Index); 

                    var closestNode = startNode;

                    //Whilst there is something in the open list
                    while (openList.Length > 0) 
                    {
                        //set current node to item with lowest F cost in open list
                        var currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray); 
                        //get currentnode as pathnode
                        var currentNode = pathNodeArray[currentNodeIndex]; 

                        //If the current node is the same as the target node, Found the Path!
                        if (currentNode.Index == targetNode.Index)
                            break;

                        // calc the closest node visited to the Targetnode, used if run out of iterations
                        if ((GetDistance(currentNode, targetNode) < GetDistance(closestNode, targetNode)) && currentNode.IsWalkable)
                            closestNode = currentNode;
                        // Path might still be obtainable but we've run out of allowed iterations
                        if (iterations == 0)
                        {
                            // Return the best result we've found so far
                            // Need to update goal so we can reconstruct the shorter path
                            endPosition = closestNode.Position;
                            break;
                        }

                        // Remove current node from Open List
                        for (var i = 0; i < openList.Length; i++)
                            if (openList[i] == currentNodeIndex)
                            {
                                //remove from openlist (swapback more performant)
                                openList.RemoveAtSwapBack(i); 
                                break;
                            }

                        //Add it to the closed List
                        closedList.Add(currentNodeIndex); 


                        //Loop through each neighbor of the current node
                        // ReSharper disable once ForCanBeConvertedToForeach
                        for (var i = 0; i < neighbourOffsetArray.Length; i++)
                        {
                            var neighbourOffset = neighbourOffsetArray[i];
                            var neighbourGridPosition =
                                new int2(currentNode.X + neighbourOffset.x, currentNode.Y + neighbourOffset.y);
                            
                            // Neighbour not valid position
                            if (!IsPositionInsideGrid(neighbourGridPosition, gridSize))
                                continue;

                            var neighbourNodeIndex = CalculateIndex(neighbourGridPosition.x, neighbourGridPosition.y, gridSize.x);

                            //If the neighbor has already been checked, Skip it
                            if (closedList.Contains(neighbourNodeIndex)) continue; 

                            //Get neighbour node as pathnode
                            var neighbourNode = pathNodeArray[neighbourNodeIndex]; 

                            //if the neighbour is unwalkable, Skip it
                            if (!neighbourNode.IsWalkable) continue; //Skip
                            
                            //Get the total cost of that neighbor
                            var moveCost = currentNode.GCost + GetDistance(currentNode, neighbourNode) + neighbourNode.Penalty; 

                            //If the total cost is greater than the g cost or it is not in the open list
                            if (moveCost < neighbourNode.GCost || !openList.Contains(neighbourNodeIndex))
                            {
                                //Set the g cost to the total cost
                                neighbourNode.GCost = moveCost; 
                                //Set the h cost
                                neighbourNode.HCost = GetDistance(neighbourNode, targetNode); 
                                //Set the parent of the node for retracing steps
                                neighbourNode.CameFromNodeIndex = currentNodeIndex; 
                                //save back to original neighbour node(neighbourNode is a copy, not reference)
                                pathNodeArray[neighbourNodeIndex] = neighbourNode; 

                                //If the neighbor is not in the openList, Add it to the list
                                if (!openList.Contains(neighbourNodeIndex))
                                    openList.Add(neighbourNodeIndex);
                            }
                        }

                        iterations--;
                    }


                    //Dispose of arrays
                    neighbourOffsetArray.Dispose();
                    openList.Dispose();
                    closedList.Dispose();
                }
            }

            private static void SetBufferPath(in NativeArray<PathNode> pathNodeArray, in PathNode endNode, ref PathFollowData pathFollowData, ref DynamicBuffer<PathPositionData> pathPositionDataBuffer)
            {
                pathPositionDataBuffer.Clear();

                if (endNode.CameFromNodeIndex == -1)
                {
                    // Didn't find a path!
                    pathFollowData.PathIndex = -1 ;
                }
                else
                {
                    // Found a path
                    CalculatePath(pathNodeArray, endNode, ref pathPositionDataBuffer);
                    pathFollowData.PathIndex = pathPositionDataBuffer.Length - 1;
                }
            }
            
            //Retrace path through the nodes(reversed) could maybe simplify path?
            private static void CalculatePath(in NativeArray<PathNode> pathNodeArray, in PathNode endNode, ref DynamicBuffer<PathPositionData> pathPositionDataBuffer)
            {
                // Found a path
                pathPositionDataBuffer.Add(new PathPositionData { Position = endNode.Position });

                var currentNode = endNode;
                while (currentNode.CameFromNodeIndex != -1)
                {
                    var cameFromNode = pathNodeArray[currentNode.CameFromNodeIndex];
                    pathPositionDataBuffer.Add(new PathPositionData { Position = cameFromNode.Position });
                    currentNode = cameFromNode;
                }
            }
        }

        private struct PathNode
        {
            // X,Y position in Node Array
            public int X; 
            public int Y;

            //index in Node Array
            public int Index; 

            // Cost to move to next node
            public int GCost; 
            // Distance to end from this node
            public int HCost; 
            //Total Cost of the Node;
            public int FCost => GCost + HCost; 

            // Is node obstructed
            public bool IsWalkable; 
            //penalty for walking the node
            public int Penalty; 
            // World position of node
            public float3 Position; 

            // For A* algo, store previous node came from to trace path
            public int CameFromNodeIndex; 

            public int CompareTo(in PathNode nodeToCompare)
            {
                var compare = FCost.CompareTo(nodeToCompare.FCost);
                if (compare == 0) //tiebreaker
                    compare = HCost.CompareTo(nodeToCompare.HCost);
                return -compare; //higher priority is lower cost
            }
        }
    }
}