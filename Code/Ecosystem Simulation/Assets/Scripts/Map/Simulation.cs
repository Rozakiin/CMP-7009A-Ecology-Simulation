using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    // To be used as main script for the sim

    // GameObjects
    public GameObject grassTile;
    public GameObject lightGrassTile;
    public GameObject tileContainer;
    public GameObject rabbit;
    public GameObject rabbitContainer;
    public GameObject grass;
    public GameObject grassContainer;

    private int gridWidth = 10;
    private int gridHeight = 10;
    private float tileSize;
    private float leftLimit, upLimit, rightLimit, downLimit;

    private System.Random rnd;
    private int numberOfTurns;

    // Start is called before the first frame update
    void Awake()
    {
        rnd = new System.Random();
        CreateTiles();
        SetLimits();
        CreateMap("Assets/Scripts/Map/MapExample.txt");
        for (int i = 0; i < 5; i++)
        {
            CreateRabbit(i);
            CreateGrass(i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateMap(string path)
    {
        List<List<MapReader.TerrainCost>> mapList = new List<List<MapReader.TerrainCost>>();
        MapReader.ReadInMap(path, ref mapList);
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

    void CreateRabbit(int iterator)
    {
        int rnd1 = rnd.Next(0, (int)gridWidth);
        int rnd2 = rnd.Next(0, (int)gridHeight);
        float rabXPos = rnd1 * tileSize;
        float rabZPos = rnd2 * tileSize;
        GameObject rabbitClone = Instantiate(rabbit, new Vector3(rabXPos, 0, rabZPos), rabbit.transform.rotation) as GameObject;
        rabbitClone.transform.parent = rabbitContainer.transform;
        rabbitClone.name = "RabbitClone" + (iterator + 1);
    }

    void CreateGrass(int iterator)
    {
        int randWidth = rnd.Next(0, (int)gridWidth);
        int randHeight = rnd.Next(0, (int)gridHeight);
        float grassXPos = randWidth * tileSize;
        float grassZPos = randHeight * tileSize;
        GameObject grassClone = Instantiate(grass, new Vector3(grassXPos, 0, grassZPos), grass.transform.rotation) as GameObject;
        grassClone.transform.parent = grassContainer.transform;
        grassClone.name = "GrassClone" + (iterator + 1);
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

