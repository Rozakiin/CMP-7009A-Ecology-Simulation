using Components;
using MonoBehaviourTools.Grid;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    public class PathFindingSystem : SystemBase
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;//14 used as approximate of sqrt(2)*10 for diagonals

        private EndSimulationEntityCommandBufferSystem ecbSystem;
        private NativeArray<PathNode> pathNodeArray;


        protected override void OnCreate()
        {
            base.OnCreate();
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        /*
         * calculates the shortest path of grid nodes between two points
         * using A-Star algorithm
         */
        protected override void OnUpdate()
        {
            //catch to not run if paused
            if (MonoBehaviourTools.UI.UITimeControl.Instance.GetPause())
            {
                return;
            }

            var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

            if (!pathNodeArray.IsCreated)
                pathNodeArray = CreatePathNodeArray();
            NativeArray<PathNode> tempArray = new NativeArray<PathNode>(pathNodeArray, Allocator.TempJob);

            float2 gridWorldSize = GridSetup.Instance.gridWorldSize;
            int2 gridSize = GridSetup.Instance.gridSize;

            // goes through each entity requesting a path and finds a path
            Entities.ForEach((
                Entity entity,
                int entityInQueryIndex,
                DynamicBuffer<PathPositionData> pathPositionDataBuffer,
                ref PathFindingRequestData pathFindingRequestData,
                ref PathFollowData pathFollowData,
                in TargetData targetData
            ) =>
            {
                NativeArray<PathNode> tmpPathNodeArray = new NativeArray<PathNode>(tempArray, Allocator.Temp);

                //Find a path
                FindPathJob findPathJob = new FindPathJob
                {
                    gridSize = gridSize,
                    pathNodeArray = tmpPathNodeArray,
                    startNode = NodeFromWorldPoint(pathFindingRequestData.startPosition, gridWorldSize, gridSize, tmpPathNodeArray),
                    targetNode = NodeFromWorldPoint(pathFindingRequestData.endPosition, gridWorldSize, gridSize, tmpPathNodeArray),
                    entity = entity,
                    iterationLimit = 10 * (int)targetData.sightRadius,//arbitary decision should have better way to determine how long a path should be
                };
                findPathJob.Execute();//execute the find path job

                //sets the buffer in the entity to follow the path
                SetBufferPathJob setBufferPathJob = new SetBufferPathJob
                {
                    entity = entity,
                    worldSize = gridWorldSize,
                    gridSize = gridSize,
                    pathNodeArray = findPathJob.pathNodeArray,
                    pathfindingRequestData = pathFindingRequestData,
                    pathFollowData = pathFollowData,
                    pathPositionDataBuffer = pathPositionDataBuffer,
                };
                setBufferPathJob.Execute();//execute the set buffer path job

                //update the edited component data from setbufferpathjob
                pathFollowData = setBufferPathJob.pathFollowData;
                pathPositionDataBuffer = setBufferPathJob.pathPositionDataBuffer;

                ecb.RemoveComponent<PathFindingRequestData>(entityInQueryIndex, entity);//remove the requestpathdata from entity
            }).WithDeallocateOnJobCompletion(tempArray).ScheduleParallel();

            // Make sure that the ECB system knows about our job
            ecbSystem.AddJobHandleForProducer(this.Dependency);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (pathNodeArray.IsCreated)
                pathNodeArray.Dispose();
        }

        [BurstCompile]
        private struct FindPathJob
        {
            public int2 gridSize;

            public NativeArray<PathNode> pathNodeArray;
            public PathNode startNode;
            public PathNode targetNode;

            public Entity entity;

            public int iterationLimit;
            public void Execute()
            {
                //Only path find if start and end are walkable
                if (startNode.isWalkable && targetNode.isWalkable)
                {
                    NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
                    neighbourOffsetArray[0] = new int2(-1, 0); // Left
                    neighbourOffsetArray[1] = new int2(+1, 0); // Right
                    neighbourOffsetArray[2] = new int2(0, +1); // Up
                    neighbourOffsetArray[3] = new int2(0, -1); // Down
                    neighbourOffsetArray[4] = new int2(-1, -1); // Left Down
                    neighbourOffsetArray[5] = new int2(-1, +1); // Left Up
                    neighbourOffsetArray[6] = new int2(+1, -1); // Right Down
                    neighbourOffsetArray[7] = new int2(+1, +1); // Right Up

                    NativeList<int> openList = new NativeList<int>(Allocator.Temp);//openlist of indexes of nodes 
                    NativeList<int> closedList = new NativeList<int>(Allocator.Temp);//closedlist of indexes of nodes 

                    openList.Add(startNode.index);//Add the starting node to the open list to begin the program

                    while (openList.Length > 0 && iterationLimit > 0)//Whilst there is something in the open list
                    {
                        int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);//set current node to item with lowest F cost in open list
                        PathNode currentNode = pathNodeArray[currentNodeIndex]; //get currentnode as pathnode

                        //If the current node is the same as the target node
                        if (currentNode.index == targetNode.index)
                        {
                            //Found the Path!
                            break;
                        }

                        // Remove current node from Open List
                        for (int i = 0; i < openList.Length; i++)
                        {
                            if (openList[i] == currentNodeIndex)
                            {
                                openList.RemoveAtSwapBack(i); //remove from openlist (swapback more performant)
                                break;
                            }
                        }

                        closedList.Add(currentNodeIndex);//Add it to the closed List


                        //Loop through each neighbor of the current node
                        for (int i = 0; i < neighbourOffsetArray.Length; i++)
                        {
                            int2 neighbourOffset = neighbourOffsetArray[i];
                            int2 neighbourGridPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                            if (!IsPositionInsideGrid(neighbourGridPosition, gridSize))
                            {
                                // Neighbour not valid position
                                continue;
                            }

                            int neighbourNodeIndex = CalculateIndex(neighbourGridPosition.x, neighbourGridPosition.y, gridSize.x);

                            //If the neighbor has already been checked
                            if (closedList.Contains(neighbourNodeIndex))
                            {
                                continue;//Skip it
                            }

                            PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];//Get neighbour node as pathnode

                            //if the neighbour is unwalkable
                            if (!neighbourNode.isWalkable)
                            {
                                continue;//Skip
                            }

                            int moveCost = currentNode.gCost + GetDistance(currentNode, neighbourNode) + neighbourNode.penalty;//Get the total cost of that neighbor

                            //If the total cost is greater than the g cost or it is not in the open list
                            if (moveCost < neighbourNode.gCost || !openList.Contains(neighbourNodeIndex))
                            {
                                neighbourNode.gCost = moveCost;//Set the g cost to the total cost
                                neighbourNode.hCost = GetDistance(neighbourNode, targetNode);//Set the h cost
                                neighbourNode.cameFromNodeIndex = currentNodeIndex;//Set the parent of the node for retracing steps
                                pathNodeArray[neighbourNodeIndex] = neighbourNode; //save back to original neighbour node(neighbourNode is a copy, not reference)

                                //If the neighbor is not in the openList
                                if (!openList.Contains(neighbourNodeIndex))
                                {
                                    openList.Add(neighbourNodeIndex);//Add it to the list
                                }
                            }
                        }
                        iterationLimit--;
                    }
                    //Dispose of arrays
                    neighbourOffsetArray.Dispose();
                    openList.Dispose();
                    closedList.Dispose();
                }
            }
        }

        [BurstCompile]
        private struct SetBufferPathJob
        {
            public float2 worldSize;
            public int2 gridSize;

            [DeallocateOnJobCompletion]
            public NativeArray<PathNode> pathNodeArray;

            public Entity entity;

            public PathFindingRequestData pathfindingRequestData;
            public PathFollowData pathFollowData;
            public DynamicBuffer<PathPositionData> pathPositionDataBuffer;
            public void Execute()
            {
                pathPositionDataBuffer.Clear();

                PathNode endNode = NodeFromWorldPoint(pathfindingRequestData.endPosition, worldSize, gridSize, pathNodeArray);
                if (endNode.cameFromNodeIndex == -1)
                {
                    // Didn't find a path!
                    pathFollowData = new PathFollowData { pathIndex = -1 };
                }
                else
                {
                    // Found a path
                    CalculatePath(pathNodeArray, endNode, pathPositionDataBuffer);
                    pathFollowData = new PathFollowData { pathIndex = pathPositionDataBuffer.Length - 1 };
                }
            }
        }

        //Retrace path through the nodes(reversed) could maybe use simplifypath?
        private static void CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode, DynamicBuffer<PathPositionData> pathPositionDataBuffer)
        {
            if (endNode.cameFromNodeIndex == -1)
            {
                // Couldn't find a path!
            }
            else
            {
                // Found a path
                pathPositionDataBuffer.Add(new PathPositionData { position = endNode.position });

                PathNode currentNode = endNode;
                while (currentNode.cameFromNodeIndex != -1)
                {
                    PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                    pathPositionDataBuffer.Add(new PathPositionData { position = cameFromNode.position });
                    currentNode = cameFromNode;
                }
            }
        }

        //Creates a PathNode NativeArray for use in pathfinding from the GridNode array
        private NativeArray<PathNode> CreatePathNodeArray()
        {
            GridNode[,] grid = GridSetup.Instance.grid;
            int2 gridSize = GridSetup.Instance.gridSize;
            int gridMaxSize = GridSetup.Instance.GridMaxSize;
            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridMaxSize, Allocator.Persistent);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    PathNode pathNode = new PathNode
                    {
                        x = x,
                        y = y,
                        index = CalculateIndex(x, y, gridSize.x),

                        gCost = int.MaxValue,

                        isWalkable = grid[x, y].isWalkable,
                        penalty = grid[x, y].movementPenalty,
                        position = grid[x, y].worldPosition,

                        cameFromNodeIndex = -1
                    };

                    pathNodeArray[pathNode.index] = pathNode;
                }
            }

            return pathNodeArray;
        }

        //Gets the closest node to the given world position.
        private static PathNode NodeFromWorldPoint(float3 _worldPos, float2 gridWorldSize, int2 gridSize, NativeArray<PathNode> pathNodeArray)
        {
            // how far along the grid the position is (left 0, middle 0.5, right 1)
            float percentX = _worldPos.x / gridWorldSize.x + 0.5f; // optimisation of maths
            float percentY = _worldPos.z / gridWorldSize.y + 0.5f;

            //clamp percent between 0 and 1
            percentX = math.clamp(percentX, 0, 1);
            percentY = math.clamp(percentY, 0, 1);

            // calc x,y position in the node array for the world position
            int x = UnityEngine.Mathf.FloorToInt(math.min(gridSize.x * percentX, gridSize.x - 1));
            int y = UnityEngine.Mathf.FloorToInt(math.min(gridSize.y * percentY, gridSize.y - 1));

            return pathNodeArray[CalculateIndex(x, y, gridSize.x)];// position of closest node in array
        }

        // if using diagonals
        private static int GetDistance(PathNode _nodeA, PathNode _nodeB)
        {
            int x = math.abs(_nodeA.x - _nodeB.x);//x1-x2
            int y = math.abs(_nodeA.y - _nodeB.y);//y1-y2
            if (x > y)
            {
                return MOVE_DIAGONAL_COST * y + MOVE_STRAIGHT_COST * (x - y);
            }
            return MOVE_DIAGONAL_COST * x + MOVE_STRAIGHT_COST * (y - x);
        }

        //calc the index in the 1d array from the 2d array
        private static int CalculateIndex(int x, int y, int gridSizeX)
        {
            return x + y * gridSizeX;
        }

        private static int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
        {
            PathNode lowestCostPathNode = pathNodeArray[openList[0]]; //set to first in array
                                                                      //compare all in array to find lowest FCost with lowest hCost
            for (int i = 1; i < openList.Length; i++)
            {
                PathNode testPathNode = pathNodeArray[openList[i]];//pathnode to compare to
                if (testPathNode.FCost < lowestCostPathNode.FCost)
                {
                    lowestCostPathNode = testPathNode;
                }
                else if (testPathNode.FCost == lowestCostPathNode.FCost)//Tiebreaker
                {
                    //Compare hcosts
                    if (testPathNode.hCost < lowestCostPathNode.hCost)
                    {
                        lowestCostPathNode = testPathNode;
                    }
                }
            }
            return lowestCostPathNode.index;
        }

        private static bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
        {
            return
                gridPosition.x >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
        }

        private struct PathNode
        {
            public int x;// X position in Node Array
            public int y;// Y position in Node Array

            public int index;//index in Node Array

            public int gCost;// Cost to move to next node
            public int hCost;// Distance to end from this node
            public int FCost { get { return gCost + hCost; } }//Total Cost of the Node;

            public bool isWalkable;// Is node obstructed
            public int penalty; //penaty for walking the node
            public float3 position;// World position of node

            public int cameFromNodeIndex;// For A* algo, store previous node came from to trace path

            public int CompareTo(PathNode nodeToCompare)
            {
                int compare = FCost.CompareTo(nodeToCompare.FCost);
                if (compare == 0) //tiebreaker
                {
                    compare = hCost.CompareTo(nodeToCompare.hCost);
                }
                return -compare;//higher priority is lower cost
            }
        }
    }
}
