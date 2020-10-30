using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathFinderController : MonoBehaviour
{
    public bool displayGridGizmos; //used in debugging, shows walkable and non walkable nodes in scene
    public LayerMask unwalkableMask;//This is the mask that the program will look for when trying to find obstructions to the path.
    public Vector2 gridWorldSize;//A vector2 to store the width and height of the graph in world units.
    public float nodeRadius;//This stores how big each square on the graph will be
    public float distanceBetweenNodes;//The distance that the squares will spawn from eachother.
    public TerrainType[] walkableRegions;
    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
    Node[,] nodeArray;//The array of nodes that the A Star algorithm uses.

    float nodeDiameter;//Twice the amount of the radius (Set in the start function)
    int gridSizeX, gridSizeY;//Size of the Grid in Array units.

    public int MaxSize{ get{ return gridSizeX * gridSizeY; } }


    private void Awake()//Ran once the program starts
    {
        nodeDiameter = nodeRadius * 2;//Double the radius to get diameter
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        
        foreach(TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value;//use Bitwise OR to add together
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);//store in dictionary, key calc from power to raise 2 by
        }
        
        CreateGrid();//Draw the grid
    }

    void CreateGrid()
    {
        nodeArray = new Node[gridSizeX, gridSizeY];//Declare the array of nodes.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < gridSizeX; x++)//Loop through the array of nodes.
        {
            for (int y = 0; y < gridSizeY; y++)//Loop through the array of nodes
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);//Get the world co ordinates of the bottom left of the graph

                //If the node is not being obstructed
                //Quick collision check against the current node and anything in the world at its position. If it is colliding with an object with a unwalkableMask,
                bool isWalkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);

                int movementPenalty = 0;//penalty for walking over node
                
                if(isWalkable)
                {
                    Ray ray = new Ray(worldPoint+Vector3.up*50, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, walkableMask)) 
                    {
                        walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }

                nodeArray[x, y] = new Node(isWalkable, worldPoint, x, y, movementPenalty);//Create a new node in the array.
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
                if (n.isWalkable)//If the current node is a walkable
                {
                    Gizmos.color = Color.white;//node colour white
                }
                else
                {
                    Gizmos.color = Color.red;//node colour red
                }
                Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - distanceBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }

    [System.Serializable]//show in inspector
    public class TerrainType 
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}