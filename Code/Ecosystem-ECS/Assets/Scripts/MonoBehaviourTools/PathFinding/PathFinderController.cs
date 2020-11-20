using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathFinderController : MonoBehaviour
{
    public static PathFinderController Instance;

    #region Properties
    [Header("Debugging")]
    [SerializeField] private bool displayGridGizmos; //used in debugging, shows walkable and non walkable nodes in scene

    [Header("Grid Data")]
    private Vector2 gridWorldSize;//A vector2 to store the width and height of the graph in world units.
    private int gridSizeX, gridSizeY;//Size of the Grid in Array units.
    public int MaxSize { get { return gridSizeX * gridSizeY; } }

    [Header("Node Properties")]
    [SerializeField] private float nodeRadius;//This stores how big each square on the graph will be
    [SerializeField] private float distanceBetweenNodes;//The distance that the squares will spawn from each other.
    private float nodeDiameter;//Twice the amount of the radius (Set in the start function)
    private Node[,] nodeArray;//The array of nodes that the A Star algorithm uses.

    [Header("LayerMask Data")]
    [SerializeField] LayerMask walkableMask;
    [SerializeField] private LayerMask unwalkableMask;//This is the mask that the program will look for when trying to find obstructions to the path.
    [SerializeField] private int unwalkableProximityPenalty = 10;//Penalty for going near to unwalkable nodes
    private int penaltyMin = int.MaxValue;
    private int penaltyMax = int.MinValue;
    [SerializeField] private TerrainType[] walkableRegions;
    private Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    #endregion

    #region Initialisation
    void Awake()//Ran once the program starts
    {
        Instance = this;
        nodeDiameter = nodeRadius * 2;//Double the radius to get diameter
    }

    void Start()
    {
        gridWorldSize = SimulationManager.worldSize;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.

        foreach (TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value;//use Bitwise OR to add together
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);//store in dictionary, key calc from power to raise 2 by
        }

        CreateGrid();//Draw the grid
    }
    #endregion


    private void CreateGrid()
    {
        nodeArray = new Node[gridSizeX, gridSizeY];//Declare the array of nodes.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < gridSizeX; x++)//Loop through the array of nodes.
        {
            for (int y = 0; y < gridSizeY; y++)//Loop through the array of nodes
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);//Get the world co ordinates of the node from the bottom left of the graph
                //Vector3 worldPoint = Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);//Get the world co ordinates of the bottom left of the graph
                //If the node is not being obstructed
                //Quick collision check against the current node and anything in the world at its position. If it is colliding with an object with a unwalkableMask,
                bool isWalkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);

                int movementPenalty = 0;//penalty for walking over node

                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, walkableMask))
                {
                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }

                if (!isWalkable)
                {
                    movementPenalty += unwalkableProximityPenalty;
                }

                nodeArray[x, y] = new Node(isWalkable, worldPoint, x, y, movementPenalty);//Create a new node in the array.
            }
        }

        BlurMovementPenaltyMap(3);//blur the map with a kernel extent of 3 (5*5)

    }

    //Blurs a map of the movement penalty for each node using a box blur,
    //this is used to smooth the penalty weights for more natural pathfinding
    private void BlurMovementPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1; //must be odd number
        int kernelExtents = blurSize; // number of squares between centre and edge of kernel

        int[,] penaltiesHorizontal = new int[gridSizeX, gridSizeY]; //temp array to store horizontal pass over the penalty map
        int[,] penaltiesVertical = new int[gridSizeX, gridSizeY]; //temp array to store vertical pass over the penalty map

        //horizontal pass
        //fill the penaltiesHorizontal array with the sum of movement penalties stored in nodeArray covered by the kernel
        for (int y = 0; y < gridSizeY; y++)
        {
            //loop through nodes in kernel and sum them up
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents); //clamp so take value from first node rather than out of bounds
                penaltiesHorizontal[0, y] += nodeArray[sampleX, y].penalty;//add the node penalty value to penaltiesHorizonal
            }

            //loop over all remaining columns in the row
            for (int x = 1; x < gridSizeX; x++)
            {
                int indexToRemove = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);//calc index of node that is no longer inside kernel after kernel moved along 1
                int indexToAdd = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);//calc index of node that is now inside kernel after kernel moved along 1
                penaltiesHorizontal[x, y] = penaltiesHorizontal[x - 1, y] - nodeArray[indexToRemove, y].penalty + nodeArray[indexToAdd, y].penalty;//equal to previous - penalty at indexToRemove + penalty at indexToAdd
            }
        }

        //vertical pass
        //fill the penaltiesVertical array with the sum of movement penalties stored in nodeArray covered by the kernel
        for (int x = 0; x < gridSizeX; x++)
        {
            //loop through nodes in kernel and sum them up
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents); //clamp so take value from first node rather than out of bounds
                penaltiesVertical[x, 0] += penaltiesHorizontal[x, sampleY];//sample the penalty from the horizontal pass array
            }

            //loop over all remaining rows in the column
            for (int y = 1; y < gridSizeY; y++)
            {
                int indexToRemove = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);//calc index of node that is no longer inside kernel after kernel moved along 1
                int indexToAdd = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);//calc index of node that is now inside kernel after kernel moved along 1
                penaltiesVertical[x, y] = penaltiesVertical[x, y - 1] - penaltiesHorizontal[x, indexToRemove] + penaltiesHorizontal[x, indexToAdd];//equal to previous - penalty at indexToRemove + penalty at indexToAdd
                int blurredPenalty = Mathf.RoundToInt((float)penaltiesVertical[x, y] / (kernelSize * kernelSize));//average the penalty and round to nearest int
                nodeArray[x, y].penalty = blurredPenalty;//set the penalty in the nodeArray to the new blurred penalty

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
    public List<Node> GetNeighboringNodes(Node _neighbourNode)
    {
        List<Node> neighbourList = new List<Node>();//Make a new list of all available neighbors.
        int checkX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int checkY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        checkX = _neighbourNode.gridX + 1;
        checkY = _neighbourNode.gridY;
        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)//If the XPosition and YPosition is in range of the array
        {
            neighbourList.Add(nodeArray[checkX, checkY]);//Add the grid to the available neighbors list
        }
        //Check the Left side of the current node.
        checkX = _neighbourNode.gridX - 1;
        checkY = _neighbourNode.gridY;
        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)//If the XPosition and YPosition is in range of the array
        {
            neighbourList.Add(nodeArray[checkX, checkY]);//Add the grid to the available neighbors list
        }
        //Check the Top side of the current node.
        checkX = _neighbourNode.gridX;
        checkY = _neighbourNode.gridY + 1;
        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)//If the XPosition and YPosition is in range of the array
        {
            neighbourList.Add(nodeArray[checkX, checkY]);//Add the grid to the available neighbors list
        }
        //Check the Bottom side of the current node.
        checkX = _neighbourNode.gridX;
        checkY = _neighbourNode.gridY - 1;
        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)//If the XPosition and YPosition is in range of the array
        {
            neighbourList.Add(nodeArray[checkX, checkY]);//Add the grid to the available neighbors list
        }

        return neighbourList;//Return the neighbours list.
    }


    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 _worldPos)
    {
        //float percentX = (_worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x; // how far along the grid the position is (left 0, middle 0.5, right 1)
        //float percentY = (_worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        float percentX = _worldPos.x / gridWorldSize.x + 0.5f; // optimisation of maths
        float percentY = _worldPos.z / gridWorldSize.y + 0.5f;

        percentX = Mathf.Clamp01(percentX);// clamped between 0 and 1
        percentY = Mathf.Clamp01(percentY);// clamped between 0 and 1

        //int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);// calc x position in the node array for the world position
        //int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);// calc y position in the node array for the world position
        int x = Mathf.FloorToInt(Mathf.Min(gridSizeX * percentX, gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Min(gridSizeY * percentY, gridSizeY - 1));


        return nodeArray[x, y];// position of closest node in array
    }


    //Function that draws the wireframe, and the nodes
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

        if (nodeArray != null && displayGridGizmos)//If the grid is not empty and displayGridGizmos is true
        {
            foreach (Node n in nodeArray)//Loop through every node in the grid
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.penalty));//pick color on gradient between white and black depending on where penalty lies between min and max
                if (!n.isWalkable)//If the current node is not walkable
                {
                    Gizmos.color = Color.red;//node colour red
                }
                Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - distanceBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }
}