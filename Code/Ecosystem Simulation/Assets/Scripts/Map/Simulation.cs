using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    // To be used as main script for the sim

    #region GameObjects
    [Header("GameObjects")]
    [SerializeField] public GameObject grassTile;
    [SerializeField] public GameObject lightGrassTile;
    [SerializeField] public GameObject waterTile;
    [SerializeField] public GameObject sandTile;
    [SerializeField] public GameObject rockTile;
    [SerializeField] public GameObject tileContainer;
    [SerializeField] public GameObject waterContainer;
    [SerializeField] public GameObject rabbit;
    [SerializeField] public GameObject rabbitContainer;
    [SerializeField] public GameObject grass;
    [SerializeField] public GameObject grassContainer;
    #endregion

    #region GameObject Counters
    [Header("GameObject Counters")]
    [SerializeField] private int grassTileCount;
    [SerializeField] private int waterTileCount;
    [SerializeField] private int rabbitCount;
    [SerializeField] private int grassCount;
    #endregion

    #region Grid Data
    [Header("Grid Data")]
    [SerializeField] public int gridWidth;
    [SerializeField] public int gridHeight;
    [SerializeField] public Vector2 worldSize;
    [SerializeField] public Vector3 worldBottomLeft;
    [SerializeField] private float tileSize;
    [SerializeField] private float leftLimit;
    [SerializeField] private float upLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float downLimit;
    #endregion

    #region Other
    [Header("Other")]
    [SerializeField] private Random rnd;
    [SerializeField] private int numberOfTurns;
    [SerializeField] private List<Edible> rabbitList;
    [SerializeField] private List<Edible> grassList;
    #endregion

    #region Initialisation
    // Start is called before the first frame update
    void Awake()
    {
        // Initiate property values
        // Object Counts
        grassTileCount = 0;
        waterTileCount = 0;
        rabbitCount = 0;
        grassCount = 0;
        // Grid data
        gridWidth = 0;
        gridHeight = 0;
        tileSize = 0;
        leftLimit = 0;
        upLimit = 0;
        rightLimit = 0;
        downLimit = 0;

        rabbitList = new List<Edible>();
        grassList = new List<Edible>();
        rnd = new Random();

        CreateMap("Assets/Scripts/Map/MapExample.txt");
        for (int i = 0; i < 15; i++)
        {
            CreateRabbit();
            
        }
        for(int i=0; i<40; i++)
        {
            CreateGrass();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        // checks for click of the mouse, sends ray out from camera, creates rabbit where it hits
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){
                Vector3 targetPosition = hit.point;
                CreateRabbitAtPos(ref targetPosition);
            }
        }
    }

    #region Map Creation
    void CreateMap(string path)
    {
        List<List<MapReader.TerrainCost>> mapList = new List<List<MapReader.TerrainCost>>();
        MapReader.ReadInMap(path, ref mapList);
        CreateTilesFromMapList(ref mapList);
        SetLimits();
    }


    void CreateTilesFromMapList(ref List<List<MapReader.TerrainCost>> mapList)
    {
        gridWidth = mapList[0].Count;
        gridHeight = mapList.Count;
        tileSize = grassTile.GetComponent<Renderer>().bounds.size.x;                   //Get the width of the tile
        worldSize.x = gridWidth*tileSize;
        worldSize.y = gridHeight*tileSize;
        worldBottomLeft = transform.position - Vector3.right * worldSize.x / 2 - Vector3.forward * worldSize.y / 2;//Get the real world position of the bottom left of the grid.
        
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * tileSize + tileSize/2) + Vector3.forward * (y * tileSize + tileSize/2);//Get the world co ordinates of the tile from the bottom left of the graph
                GameObject tileClone;

                switch (mapList[y][x])
                {
                    case MapReader.TerrainCost.Water:
                        tileClone = Instantiate(waterTile, worldPoint, waterTile.transform.rotation);  //Place the water tile
                        tileClone.transform.parent = waterContainer.transform;
                        tileClone.name += y + "" + x;
                        tileClone.layer = 8; //set layer to unwalkable
                        break;
                    case MapReader.TerrainCost.Grass:
                        tileClone = Instantiate(grassTile, worldPoint, grassTile.transform.rotation);  //Place the grass tile
                        tileClone.transform.parent = tileContainer.transform;
                        tileClone.name += y + "" + x;
                        tileClone.layer = 9; //set layer to grass
                        break;
                    case MapReader.TerrainCost.Sand:
                        tileClone = Instantiate(sandTile, worldPoint, sandTile.transform.rotation);  //Place the sand tile
                        tileClone.transform.parent = tileContainer.transform;
                        tileClone.name += y + "" + x;
                        tileClone.layer = 10; //set layer to grass
                        break;
                    case MapReader.TerrainCost.Rock:
                        tileClone = Instantiate(rockTile, worldPoint, rockTile.transform.rotation);  //Place the rock tile
                        tileClone.transform.parent = tileContainer.transform;
                        tileClone.name += y + "" + x;
                        tileClone.layer = 11; //set layer to grass
                        break;
                    default:
                        tileClone = Instantiate(lightGrassTile, worldPoint, lightGrassTile.transform.rotation);
                        tileClone.transform.parent = tileContainer.transform;
                        tileClone.name += y + "" + x;
                        throw new System.InvalidOperationException("Unknown TerrainCost value"); //TODO check correct Exception thrown
                }
            }
        }
    }

    void SetLimits()
    {
        upLimit = (float)(gridHeight - 1) * tileSize;
        leftLimit = 0;
        rightLimit = (float)(gridWidth - 1) * tileSize;
        downLimit = 0;
    }
    #endregion

    #region Object Spawning
    public void CreateRabbitAtPos(ref Vector3 position)
    {
        GameObject rabbitClone = Instantiate(rabbit, position, rabbit.transform.rotation) as GameObject;
        rabbitCount++;
        rabbitClone.GetComponent<Rabbit>().scene = this;
        rabbitList.Add(rabbitClone.GetComponent<Rabbit>());
        rabbitClone.transform.parent = rabbitContainer.transform;
        rabbitClone.name = "RabbitClone" + rabbitCount;
    }
    
    public void CreateRabbit()
    {
        int randWidth = Random.Range(0, (int)gridWidth-1);
        int randHeight = Random.Range(0, (int)gridHeight-1);
        Vector3 worldPoint = worldBottomLeft + Vector3.right * (randWidth * tileSize + tileSize/2) + Vector3.forward * (randHeight * tileSize + tileSize/2);//Get the world co ordinates of the rabbit from the bottom left of the graph
        
        GameObject rabbitClone = Instantiate(rabbit, worldPoint, rabbit.transform.rotation) as GameObject;
        rabbitCount++;
        rabbitClone.GetComponent<Rabbit>().scene = this;
        rabbitList.Add(rabbitClone.GetComponent<Rabbit>());
        rabbitClone.transform.parent = rabbitContainer.transform;
        rabbitClone.name = "RabbitClone" + rabbitCount;
    }

    public void CreateGrass()
    {
        int randWidth = Random.Range(0, (int)gridWidth-1);
        int randHeight = Random.Range(0, (int)gridHeight-1);
        Vector3 worldPoint = worldBottomLeft + Vector3.right * (randWidth * tileSize + tileSize/2) + Vector3.forward * (randHeight * tileSize + tileSize/2);//Get the world co ordinates of the rabbit from the bottom left of the graph

        GameObject grassClone = Instantiate(grass, worldPoint, grass.transform.rotation) as GameObject;
        grassCount++;
        //grassClone.GetComponent<Grass>().scene = this;
        grassList.Add(grassClone.GetComponent<Grass>());
        grassClone.transform.parent = grassContainer.transform;
        grassClone.name = "GrassClone" + grassCount;
    }
    #endregion

    #region Getters
    public int GetGridWidth()
    {
        return gridWidth;
    }

    public int GetGridHeight()
    {
        return gridHeight;
    }

    public float GetTileSize()
    {
        return tileSize;
    }

    public float GetLeftLimit()
    {
        return leftLimit;
    }

    public float GetUpLimit()
    {
        return upLimit;
    }

    public float GetRightLimit()
    {
        return rightLimit;
    }

    public float GetDownLimit()
    {
        return downLimit;
    }

    public int GetNumberOfTurns()
    {
        return numberOfTurns;
    }

    public List<Edible> GetGrassList()
    {
        return grassList;
    }

    public List<Edible> GetRabbitList()
    {
        return rabbitList;
    }
    #endregion

    public void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void RemoveFromList(Edible edible, List<Edible> edibleList)
    {
        for(int i=0; i<edibleList.Count; i++)
        {
            if(edibleList[i] == edible)
            {
                edibleList.RemoveAt(i);
            }
        }
    }
}

