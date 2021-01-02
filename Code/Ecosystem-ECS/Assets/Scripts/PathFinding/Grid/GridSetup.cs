using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Physics;
using Unity.Entities;
using Unity.Mathematics;
using RaycastHit = Unity.Physics.RaycastHit;
using Unity.Physics.Systems;

public class GridSetup : MonoBehaviour
{
    public static GridSetup Instance; //reference to self - singleton
    EntityManager entityManager;

    [Header("Debugging")]
    [SerializeField] private bool displayGridGizmos; //used in debugging, shows walkable and non walkable nodes in scene

    [Header("Grid Data")]
    public float2 gridWorldSize;//A vector2 to store the width and height of the graph in world units.
    public int2 gridSize;//Size of the Grid in Array units.
    public int GridMaxSize { get { return gridSize.x * gridSize.y; } }

    [Header("Node Properties")]
    [SerializeField] private float gridNodeRadius;//This stores how big each square on the graph will be
    [SerializeField] private float distanceBetweenGridNodes;//The distance that the gizmo squares will spawn from each other.
    public float gridNodeDiameter;//Twice the amount of the radius (Set in the start function)
    public GridNode[,] grid;//The array of nodes that the A Star algorithm uses.

    [Header("LayerMask Data")]
    //[SerializeField] LayerMask walkableMask;
    //[SerializeField] private LayerMask unwalkableMask;//This is the mask that the program will look for when trying to find obstructions to the path.
    [SerializeField] private int unwalkableProximityPenalty;//Penalty for going near to unwalkable nodes
    private int penaltyMin = int.MaxValue;
    private int penaltyMax = int.MinValue;
    //[SerializeField] private TerrainType[] walkableRegions;
    //private Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    private void Awake()
    {
        Instance = this;
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        gridNodeDiameter = gridNodeRadius * 2;//Double the radius to get diameter
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(CreateGrid());
    }

    private void Update()
    {

    }

    public bool CreateGrid()
    {
        //yield return new WaitForEndOfFrame(); // wait till the end of frame so tile entities have been made
        gridWorldSize = SimulationManager.worldSize;
        gridSize.x = (int)math.round(gridWorldSize.x / gridNodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        gridSize.y = (int)math.round(gridWorldSize.y / gridNodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        grid = new GridNode[gridSize.x, gridSize.y]; // create array of grid nodes

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                float3 worldPoint = SimulationManager.worldBottomLeft + Vector3.right * (x * gridNodeDiameter + gridNodeRadius) + Vector3.forward * (y * gridNodeDiameter + gridNodeRadius);//Get the world co ordinates of the node from the bottom left of the graph

                bool isWalkable = false;

                int movementPenalty = 0;//penalty for walking over node
                float3 tempUp = worldPoint + new float3(0, 100000, 0);
                float3 tempDown = worldPoint + new float3(0, -100000, 0);
                CollisionFilter tempTileFilter = new CollisionFilter { BelongsTo = ~0u, CollidesWith = 1 >> 0, GroupIndex = 0 }; //filter to only collide with tiles
                //raycast from really high point to under map, colliding with only tiles
                Entity collidedEntity = UtilTools.PhysicsTools.GetEntityFromRaycast(tempUp, tempDown, tempTileFilter);

                if (entityManager.HasComponent<TerrainTypeData>(collidedEntity))
                {
                    movementPenalty = entityManager.GetComponentData<TerrainTypeData>(collidedEntity).terrainPenalty;
                    isWalkable = entityManager.GetComponentData<TerrainTypeData>(collidedEntity).isWalkable;
                }
                else return false; // if it collides with something that doesnt have terraintype then terrain hasnt loaded

                if (!isWalkable)
                {
                    movementPenalty += unwalkableProximityPenalty;
                }

                grid[x, y] = new GridNode(isWalkable, worldPoint, x, y, movementPenalty);//Create a new node in the array.
            }
        }

        BlurMovementPenaltyMap(3);//blur the map with a kernel extent of 3 (5*5)
        return true;
    }

    //Function that draws the wireframe, and the nodes
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

        if (grid != null && displayGridGizmos)//If the grid is not empty and displayGridGizmos is true
        {
            foreach (GridNode n in grid)//Loop through every node in the grid
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));//pick color on gradient between white and black depending on where penalty lies between min and max
                if (!n.isWalkable)//If the current node is not walkable
                {
                    Gizmos.color = Color.red;//node colour red
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (gridNodeDiameter - distanceBetweenGridNodes));//Draw the node at the position of the node.
            }
        }
    }

    //Blurs a map of the movement penalty for each node using a box blur,
    //this is used to smooth the penalty weights for more natural pathfinding
    private void BlurMovementPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1; //must be odd number
        int kernelExtents = blurSize; // number of squares between centre and edge of kernel

        int[,] penaltiesHorizontal = new int[gridSize.x, gridSize.y]; //temp array to store horizontal pass over the penalty map
        int[,] penaltiesVertical = new int[gridSize.x, gridSize.y]; //temp array to store vertical pass over the penalty map

        //horizontal pass
        //fill the penaltiesHorizontal array with the sum of movement penalties stored in nodeArray covered by the kernel
        for (int y = 0; y < gridSize.y; y++)
        {
            //loop through nodes in kernel and sum them up
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents); //clamp so take value from first node rather than out of bounds
                penaltiesHorizontal[0, y] += grid[sampleX, y].movementPenalty;//add the node penalty value to penaltiesHorizonal
            }

            //loop over all remaining columns in the row
            for (int x = 1; x < gridSize.x; x++)
            {
                int indexToRemove = Mathf.Clamp(x - kernelExtents - 1, 0, gridSize.x);//calc index of node that is no longer inside kernel after kernel moved along 1
                int indexToAdd = Mathf.Clamp(x + kernelExtents, 0, gridSize.x - 1);//calc index of node that is now inside kernel after kernel moved along 1
                penaltiesHorizontal[x, y] = penaltiesHorizontal[x - 1, y] - grid[indexToRemove, y].movementPenalty + grid[indexToAdd, y].movementPenalty;//equal to previous - penalty at indexToRemove + penalty at indexToAdd
            }
        }

        //vertical pass
        //fill the penaltiesVertical array with the sum of movement penalties stored in nodeArray covered by the kernel
        for (int x = 0; x < gridSize.x; x++)
        {
            //loop through nodes in kernel and sum them up
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents); //clamp so take value from first node rather than out of bounds
                penaltiesVertical[x, 0] += penaltiesHorizontal[x, sampleY];//sample the penalty from the horizontal pass array
            }

            //loop over all remaining rows in the column
            for (int y = 1; y < gridSize.y; y++)
            {
                int indexToRemove = Mathf.Clamp(y - kernelExtents - 1, 0, gridSize.y);//calc index of node that is no longer inside kernel after kernel moved along 1
                int indexToAdd = Mathf.Clamp(y + kernelExtents, 0, gridSize.y - 1);//calc index of node that is now inside kernel after kernel moved along 1
                penaltiesVertical[x, y] = penaltiesVertical[x, y - 1] - penaltiesHorizontal[x, indexToRemove] + penaltiesHorizontal[x, indexToAdd];//equal to previous - penalty at indexToRemove + penalty at indexToAdd
                int blurredPenalty = Mathf.RoundToInt((float)penaltiesVertical[x, y] / (kernelSize * kernelSize));//average the penalty and round to nearest int
                grid[x, y].movementPenalty = blurredPenalty;//set the penalty in the nodeArray to the new blurred penalty

                //update penalty min and max
                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }


    //Function that gets the neighboring nodes of the given node.
    public List<GridNode> GetNeighboringNodes(GridNode _neighbourNode)
    {
        List<GridNode> neighbourList = new List<GridNode>();//Make a new list of all available neighbors.
        int checkX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int checkY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        checkX = _neighbourNode.x + 1;
        checkY = _neighbourNode.y;
        if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)//If the XPosition and YPosition is in range of the array
        {
            neighbourList.Add(grid[checkX, checkY]);//Add the grid to the available neighbors list
        }
        //Check the Left side of the current node.
        checkX = _neighbourNode.x - 1;
        checkY = _neighbourNode.y;
        if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)//If the XPosition and YPosition is in range of the array
        {
            neighbourList.Add(grid[checkX, checkY]);//Add the grid to the available neighbors list
        }
        //Check the Top side of the current node.
        checkX = _neighbourNode.x;
        checkY = _neighbourNode.y + 1;
        if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)//If the XPosition and YPosition is in range of the array
        {
            neighbourList.Add(grid[checkX, checkY]);//Add the grid to the available neighbors list
        }
        //Check the Bottom side of the current node.
        checkX = _neighbourNode.x;
        checkY = _neighbourNode.y - 1;
        if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)//If the XPosition and YPosition is in range of the array
        {
            neighbourList.Add(grid[checkX, checkY]);//Add the grid to the available neighbors list
        }

        return neighbourList;//Return the neighbours list.
    }


    //Gets the closest node to the given world position.
    public GridNode NodeFromWorldPoint(float3 _worldPos)
    {
        // how far along the grid the position is (left 0, middle 0.5, right 1)
        float percentX = _worldPos.x / gridWorldSize.x + 0.5f; // optimisation of maths
        float percentY = _worldPos.z / gridWorldSize.y + 0.5f;

        percentX = Mathf.Clamp01(percentX);// clamped between 0 and 1
        percentY = Mathf.Clamp01(percentY);// clamped between 0 and 1

        // calc x,y position in the node array for the world position
        int x = Mathf.FloorToInt(Mathf.Min(gridSize.x * percentX, gridSize.x - 1));
        int y = Mathf.FloorToInt(Mathf.Min(gridSize.y * percentY, gridSize.y - 1));

        return grid[x, y];// position of closest node in array
    }


    // returns if the given world point is in a walkable tile
    public bool IsWorldPointWalkable(float3 _worldPos)
    {
        return NodeFromWorldPoint(_worldPos).isWalkable;
    }


    // static version combines NodeFromWorldPoint and IsWorldPointWalkable
    public static bool IsWorldPointWalkableFromGrid(float3 _worldPos, float2 _worldSize, int2 _gridSize, GridNode[,] grid)
    {
        // how far along the grid the position is (left 0, middle 0.5, right 1)
        float percentX = _worldPos.x / _worldSize.x + 0.5f; // optimisation of maths
        float percentY = _worldPos.z / _worldSize.y + 0.5f;

        //clamp percent between 0 and 1
        percentX = math.clamp(percentX, 0, 1);
        percentY = math.clamp(percentY, 0, 1);

        // calc x,y position in the node array for the world position
        int x = Mathf.FloorToInt(math.min(_gridSize.x * percentX, _gridSize.x - 1));
        int y = Mathf.FloorToInt(math.min(_gridSize.y * percentY, _gridSize.y - 1));

        return grid[x, y].isWalkable;
    }
}
