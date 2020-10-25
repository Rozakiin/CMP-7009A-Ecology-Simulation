using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    // To be used as main script for the sim

    // GameObjects
    public GameObject grassTile;
    public GameObject lightGrassTile;
    public GameObject waterTile;
    public GameObject tileContainer;
    public GameObject rabbit;
    public GameObject rabbitContainer;
    public GameObject grass;
    public GameObject grassContainer;
    public GameObject plane; //used for spawning rabbit on mouseclick

    // GameObject Counters
    private int grassTileCount;
    private int waterTileCount;
    private int rabbitCount;
    private int grassCount;

    // Grid Data
    private int gridWidth;
    private int gridHeight;
    private float tileSize;
    private float leftLimit, upLimit, rightLimit, downLimit;

    private System.Random rnd;
    private int numberOfTurns;

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

        rnd = new System.Random();

        CreateMap("Assets/Scripts/Map/MapExample.txt");
        SetLimits();
        for (int i = 0; i < 5; i++)
        {
            CreateRabbit();
            CreateGrass();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

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

    void CreateMap(string path)
    {
        List<List<MapReader.TerrainCost>> mapList = new List<List<MapReader.TerrainCost>>();
        MapReader.ReadInMap(path, ref mapList);
        CreateTilesFromMapList(ref mapList);
    }

    void CreateTilesFromMapList(ref List<List<MapReader.TerrainCost>> mapList)
    {
        gridWidth = mapList.Count;
        gridHeight = mapList[0].Count;

        for (int i = 0; i < mapList.Count; i++)
        {
            for (int j = 0; j < mapList[i].Count; j++)
            {
                tileSize = grassTile.GetComponent<Renderer>().bounds.size.x;                   //Get the width of the tile
                float xPos = i * tileSize;                                                     //Get the tile's x position
                float yPos = grassTile.transform.position.y;                                //Get the tile's y position (always the same)
                float zPos = j * tileSize;                                                     //Get the tile's z position
                GameObject tileClone;
                //Debug.Log(mapList[i][j].ToString());
                switch (mapList[i][j])
                {
                    case MapReader.TerrainCost.Water:
                        tileClone = Instantiate(waterTile, new Vector3(xPos, yPos, zPos), waterTile.transform.rotation);  //Place the water tile
                        tileClone.transform.parent = tileContainer.transform;
                        tileClone.name += i + "" + (j + 1);
                        break;
                    case MapReader.TerrainCost.Grass:
                        tileClone = Instantiate(grassTile, new Vector3(xPos, yPos, zPos), grassTile.transform.rotation);  //Place the grass tile
                        tileClone.transform.parent = tileContainer.transform;
                        tileClone.name += i + "" + (j + 1);
                        break;
                    case MapReader.TerrainCost.Sand:
                        //tileClone = Instantiate(sandTile, new Vector3(xPos, yPos, zPos), sandTile.transform.rotation);  //Place the sand tile
                        //tileClone.transform.parent = tileContainer.transform;
                        //tileClone.name += i + "" + (j + 1);
                        break;
                    case MapReader.TerrainCost.Rock:
                        //tileClone = Instantiate(rockTile, new Vector3(xPos, yPos, zPos), rockTile.transform.rotation);  //Place the rock tile
                        //tileClone.transform.parent = tileContainer.transform;
                        //tileClone name += i + "" + (j + 1);
                        break;
                    default:
                        tileClone = Instantiate(lightGrassTile, new Vector3(xPos, yPos, zPos), lightGrassTile.transform.rotation);
                        tileClone.transform.parent = tileContainer.transform;
                        tileClone.name += i + "" + (j + 1);
                        throw new System.InvalidOperationException("Unknown TerrainCost value"); //TODO check correct Exception thrown
                }
            }
        }
    }

    void CreateTiles()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                tileSize = grassTile.GetComponent<Renderer>().bounds.size.x;                   //Get the width of the tile
                float xPos = i * tileSize;                                                     //Get the tile's x position
                float yPos = grassTile.transform.position.y;                                //Get the tile's y position (always the same)
                float zPos = j * tileSize;                                                     //Get the tile's z position
                GameObject tileClone;
                if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                {
                    tileClone = Instantiate(lightGrassTile, new Vector3(xPos, yPos, zPos), lightGrassTile.transform.rotation);  //Place the light green tile
                }
                else
                {
                    tileClone = Instantiate(grassTile, new Vector3(xPos, yPos, zPos), grassTile.transform.rotation);             //Place the green tile
                }
                tileClone.transform.parent = tileContainer.transform;
                tileClone.name += i + "" + (j + 1);
            }
        }
    }

    //Unfinished
    // void CreateGameObjectAtPos(ref GameObject object, ref GameObject objectContainer, int iterator, ref Vector3 position)
    // {
    //     GameObject objClone = Instantiate(object, position, obj.transform.rotation) as GameObject;
    //     objectClone.transform.parent = objectContainer.transform;
    //     objectClone.name = iterator;
    // }

    void CreateRabbitAtPos(ref Vector3 position)
    {
        GameObject rabbitClone = Instantiate(rabbit, position, rabbit.transform.rotation) as GameObject;
        rabbitCount++;
        rabbitClone.transform.parent = rabbitContainer.transform;
        rabbitClone.name = "RabbitClone" + rabbitCount;
    }

    void CreateRabbit() 
    {
        int rnd1 = rnd.Next(0, (int)gridWidth);
        int rnd2 = rnd.Next(0, (int)gridHeight);
        float rabXPos = rnd1 * tileSize;
        float rabZPos = rnd2 * tileSize;
        GameObject rabbitClone = Instantiate(rabbit, new Vector3(rabXPos, 0, rabZPos), rabbit.transform.rotation) as GameObject;
        rabbitCount++;
        rabbitClone.transform.parent = rabbitContainer.transform;
        rabbitClone.name = "RabbitClone" + rabbitCount;
    }

    void CreateGrass()
    {
        int randWidth = rnd.Next(0, (int)gridWidth);
        int randHeight = rnd.Next(0, (int)gridHeight);
        float grassXPos = randWidth * tileSize;
        float grassZPos = randHeight * tileSize;
        GameObject grassClone = Instantiate(grass, new Vector3(grassXPos, 0, grassZPos), grass.transform.rotation) as GameObject;
        grassCount++;
        grassClone.transform.parent = grassContainer.transform;
        grassClone.name = "GrassClone" + grassCount;
    }

    void SetLimits()
    {
        upLimit = (float)(gridHeight - 1) * tileSize;
        leftLimit = 0;
        rightLimit = (float)(gridWidth - 1) * tileSize;
        downLimit = 0;
    }

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
}

