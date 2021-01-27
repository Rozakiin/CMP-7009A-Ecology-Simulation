using Components;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace MonoBehaviourTools.Grid
{
    public class GridSetup : MonoBehaviour
    {
        public static GridSetup Instance; //reference to self - singleton
        EntityManager _entityManager;

        [Header("Debugging")]
        [SerializeField] private bool _displayGridGizmos; //used in debugging, shows walkable and non walkable nodes in scene

        [Header("Grid Data")]
        public float2 GridWorldSize;//A vector2 to store the width and height of the graph in world units.
        public int2 GridSize;//Size of the Grid in Array units.
        public int GridMaxSize { get { return GridSize.x * GridSize.y; } }

        [Header("Node Properties")]
        [SerializeField] private float _gridNodeRadius;//This stores how big each square on the graph will be
        [SerializeField] private float _distanceBetweenGridNodes;//The distance that the gizmo squares will spawn from each other.
        public float GridNodeDiameter;//Twice the amount of the radius (Set in the start function)
        public GridNode[,] Grid;//The array of nodes that the A Star algorithm uses.

        [Header("LayerMask Data")]
        [SerializeField] private int _unwalkableProximityPenalty;//Penalty for going near to unwalkable nodes
        private int _penaltyMin = int.MaxValue;
        private int _penaltyMax = int.MinValue;


        private void Awake()
        {
            Instance = this;
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            GridNodeDiameter = _gridNodeRadius * 2;//Double the radius to get diameter
        }

        /*Methods to create the pathfinding grid from the map, asigns a movement penalty based on if is walkable and terrainpenalty*/
        public bool CreateGrid()
        {
            GridWorldSize = SimulationManager.WorldSize;
            GridSize.x = (int)math.round(GridWorldSize.x / GridNodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
            GridSize.y = (int)math.round(GridWorldSize.y / GridNodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
            Grid = new GridNode[GridSize.x, GridSize.y]; // create array of grid nodes

            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    float3 worldPoint = SimulationManager.WorldBottomLeft + Vector3.right * (x * GridNodeDiameter + _gridNodeRadius) + Vector3.forward * (y * GridNodeDiameter + _gridNodeRadius);//Get the world co ordinates of the node from the bottom left of the graph

                    bool isWalkable;
                    int movementPenalty;//penalty for walking over node

                    float3 tempUp = worldPoint + new float3(0, 100000, 0);
                    float3 tempDown = worldPoint + new float3(0, -100000, 0);
                    CollisionFilter tempTileFilter = new CollisionFilter { BelongsTo = ~0u, CollidesWith = 1 >> 0, GroupIndex = 0 }; //filter to only collide with tiles
                    //raycast from really high point to under map, colliding with only tiles
                    Entity collidedEntity = UtilTools.PhysicsTools.GetEntityFromRaycast(tempUp, tempDown, tempTileFilter);

                    if (_entityManager.HasComponent<TerrainTypeData>(collidedEntity))
                    {
                        movementPenalty = _entityManager.GetComponentData<TerrainTypeData>(collidedEntity).TerrainPenalty;
                        isWalkable = _entityManager.GetComponentData<TerrainTypeData>(collidedEntity).IsWalkable;
                    }
                    else return false; // if it collides with something that doesnt have terraintype then terrain hasnt loaded

                    if (!isWalkable)
                    {
                        movementPenalty += _unwalkableProximityPenalty;
                    }

                    Grid[x, y] = new GridNode(isWalkable, worldPoint, x, y, movementPenalty);//Create a new node in the array.
                }
            }

            BlurMovementPenaltyMap(3);//blur the map with a kernel extent of 3 (5*5)
            return true;
        }

        /*Function that draws the wireframe, and the nodes*/
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

            if (Grid != null && _displayGridGizmos)//If the grid is not empty and displayGridGizmos is true
            {
                foreach (GridNode n in Grid)//Loop through every node in the grid
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(_penaltyMin, _penaltyMax, n.MovementPenalty));//pick color on gradient between white and black depending on where penalty lies between min and max
                    if (!n.IsWalkable)//If the current node is not walkable
                    {
                        Gizmos.color = Color.red;//node colour red
                    }
                    Gizmos.DrawCube(n.WorldPosition, Vector3.one * (GridNodeDiameter - _distanceBetweenGridNodes));//Draw the node at the position of the node.
                }
            }
        }

        /*
         * Blurs a map of the movement penalty for each node using a box blur,
         * this is used to smooth the penalty weights for more natural pathfinding
         */
        private void BlurMovementPenaltyMap(int blurSize)
        {
            int kernelSize = blurSize * 2 + 1; //must be odd number
            int kernelExtents = blurSize; // number of squares between centre and edge of kernel

            int[,] penaltiesHorizontal = new int[GridSize.x, GridSize.y]; //temp array to store horizontal pass over the penalty map
            int[,] penaltiesVertical = new int[GridSize.x, GridSize.y]; //temp array to store vertical pass over the penalty map

            //horizontal pass
            //fill the penaltiesHorizontal array with the sum of movement penalties stored in nodeArray covered by the kernel
            for (int y = 0; y < GridSize.y; y++)
            {
                //loop through nodes in kernel and sum them up
                for (int x = -kernelExtents; x <= kernelExtents; x++)
                {
                    int sampleX = Mathf.Clamp(x, 0, kernelExtents); //clamp so take value from first node rather than out of bounds
                    penaltiesHorizontal[0, y] += Grid[sampleX, y].MovementPenalty;//add the node penalty value to penaltiesHorizonal
                }

                //loop over all remaining columns in the row
                for (int x = 1; x < GridSize.x; x++)
                {
                    int indexToRemove = Mathf.Clamp(x - kernelExtents - 1, 0, GridSize.x);//calc index of node that is no longer inside kernel after kernel moved along 1
                    int indexToAdd = Mathf.Clamp(x + kernelExtents, 0, GridSize.x - 1);//calc index of node that is now inside kernel after kernel moved along 1
                    penaltiesHorizontal[x, y] = penaltiesHorizontal[x - 1, y] - Grid[indexToRemove, y].MovementPenalty + Grid[indexToAdd, y].MovementPenalty;//equal to previous - penalty at indexToRemove + penalty at indexToAdd
                }
            }

            //vertical pass
            //fill the penaltiesVertical array with the sum of movement penalties stored in nodeArray covered by the kernel
            for (int x = 0; x < GridSize.x; x++)
            {
                //loop through nodes in kernel and sum them up
                for (int y = -kernelExtents; y <= kernelExtents; y++)
                {
                    int sampleY = Mathf.Clamp(y, 0, kernelExtents); //clamp so take value from first node rather than out of bounds
                    penaltiesVertical[x, 0] += penaltiesHorizontal[x, sampleY];//sample the penalty from the horizontal pass array
                }

                //blur bottom row
                int blurredPenalty = Mathf.RoundToInt((float)penaltiesVertical[x, 0] / (kernelSize * kernelSize));//average the penalty and round to nearest int
                Grid[x, 0].MovementPenalty = blurredPenalty;//set the penalty in the nodeArray to the new blurred penalty

                //loop over all remaining rows in the column
                for (int y = 1; y < GridSize.y; y++)
                {
                    int indexToRemove = Mathf.Clamp(y - kernelExtents - 1, 0, GridSize.y);//calc index of node that is no longer inside kernel after kernel moved along 1
                    int indexToAdd = Mathf.Clamp(y + kernelExtents, 0, GridSize.y - 1);//calc index of node that is now inside kernel after kernel moved along 1
                    penaltiesVertical[x, y] = penaltiesVertical[x, y - 1] - penaltiesHorizontal[x, indexToRemove] + penaltiesHorizontal[x, indexToAdd];//equal to previous - penalty at indexToRemove + penalty at indexToAdd
                    blurredPenalty = Mathf.RoundToInt((float)penaltiesVertical[x, y] / (kernelSize * kernelSize));//average the penalty and round to nearest int
                    Grid[x, y].MovementPenalty = blurredPenalty;//set the penalty in the nodeArray to the new blurred penalty

                    //update penalty min and max
                    if (blurredPenalty > _penaltyMax)
                    {
                        _penaltyMax = blurredPenalty;
                    }
                    if (blurredPenalty < _penaltyMin)
                    {
                        _penaltyMin = blurredPenalty;
                    }
                }
            }
        }


        /*Function that gets the neighboring nodes of the given node.*/
        public List<GridNode> GetNeighboringNodes(GridNode neighbourNode)
        {
            List<GridNode> neighbourList = new List<GridNode>();//Make a new list of all available neighbors.
            int checkX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
            int checkY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

            //Check the right side of the current node.
            checkX = neighbourNode.X + 1;
            checkY = neighbourNode.Y;
            if (checkX >= 0 && checkX < GridSize.x && checkY >= 0 && checkY < GridSize.y)//If the XPosition and YPosition is in range of the array
            {
                neighbourList.Add(Grid[checkX, checkY]);//Add the grid to the available neighbors list
            }
            //Check the Left side of the current node.
            checkX = neighbourNode.X - 1;
            checkY = neighbourNode.Y;
            if (checkX >= 0 && checkX < GridSize.x && checkY >= 0 && checkY < GridSize.y)//If the XPosition and YPosition is in range of the array
            {
                neighbourList.Add(Grid[checkX, checkY]);//Add the grid to the available neighbors list
            }
            //Check the Top side of the current node.
            checkX = neighbourNode.X;
            checkY = neighbourNode.Y + 1;
            if (checkX >= 0 && checkX < GridSize.x && checkY >= 0 && checkY < GridSize.y)//If the XPosition and YPosition is in range of the array
            {
                neighbourList.Add(Grid[checkX, checkY]);//Add the grid to the available neighbors list
            }
            //Check the Bottom side of the current node.
            checkX = neighbourNode.X;
            checkY = neighbourNode.Y - 1;
            if (checkX >= 0 && checkX < GridSize.x && checkY >= 0 && checkY < GridSize.y)//If the XPosition and YPosition is in range of the array
            {
                neighbourList.Add(Grid[checkX, checkY]);//Add the grid to the available neighbors list
            }

            return neighbourList;//Return the neighbours list.
        }


        /*Gets the closest node to the given world position.*/
        public GridNode NodeFromWorldPoint(float3 worldPos)
        {
            // how far along the grid the position is (left 0, middle 0.5, right 1)
            float percentX = worldPos.x / GridWorldSize.x + 0.5f; // optimisation of maths
            float percentY = worldPos.z / GridWorldSize.y + 0.5f;

            percentX = Mathf.Clamp01(percentX);// clamped between 0 and 1
            percentY = Mathf.Clamp01(percentY);// clamped between 0 and 1

            // calc x,y position in the node array for the world position
            int x = Mathf.FloorToInt(Mathf.Min(GridSize.x * percentX, GridSize.x - 1));
            int y = Mathf.FloorToInt(Mathf.Min(GridSize.y * percentY, GridSize.y - 1));

            return Grid[x, y];// position of closest node in array
        }


        /* returns if the given world point is in a walkable tile*/
        public bool IsWorldPointWalkable(float3 worldPos)
        {
            return NodeFromWorldPoint(worldPos).IsWalkable;
        }


        /* static version combines NodeFromWorldPoint and IsWorldPointWalkable*/
        public static bool IsWorldPointWalkableFromGrid(float3 worldPos, float2 worldSize, int2 gridSize, GridNode[,] grid)
        {
            // how far along the grid the position is (left 0, middle 0.5, right 1)
            float percentX = worldPos.x / worldSize.x + 0.5f; // optimisation of maths
            float percentY = worldPos.z / worldSize.y + 0.5f;

            //clamp percent between 0 and 1
            percentX = math.clamp(percentX, 0, 1);
            percentY = math.clamp(percentY, 0, 1);

            // calc x,y position in the node array for the world position
            int x = Mathf.FloorToInt(math.min(gridSize.x * percentX, gridSize.x - 1));
            int y = Mathf.FloorToInt(math.min(gridSize.y * percentY, gridSize.y - 1));

            return grid[x, y].IsWalkable;
        }
    }
}
