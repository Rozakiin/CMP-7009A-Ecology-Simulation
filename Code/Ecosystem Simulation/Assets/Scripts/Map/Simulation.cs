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

    public int gridWidth;
    public int gridHeight;
    public Vector2 worldSize;
    public Vector3 worldBottomLeft;
    private float tileSize;
    private float leftLimit, upLimit, rightLimit, downLimit;

    private System.Random rnd;
    private int numberOfTurns;

    // Start is called before the first frame update
    void Awake()
    {
        rnd = new System.Random();
        //CreateTiles();
        CreateMap("Assets/Scripts/Map/MapExample.txt");
        SetLimits();
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
        CreateTilesFromMapList(ref mapList);
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
                        tileClone.transform.parent = tileContainer.transform;
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
                        //tileClone = Instantiate(sandTile, worldPoint, sandTile.transform.rotation);  //Place the sand tile
                        //tileClone.transform.parent = tileContainer.transform;
                        //tileClone.name += y + "" + x;
                        //tileClone.layer = 10; //set layer to grass
                        break;
                    case MapReader.TerrainCost.Rock:
                        //tileClone = Instantiate(rockTile, worldPoint, rockTile.transform.rotation);  //Place the rock tile
                        //tileClone.transform.parent = tileContainer.transform;
                        //tileClone name += y + "" + x;
                        //tileClone.layer = 11; //set layer to grass
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

    void CreateRabbit(int iterator)
    {
        int randWidth = rnd.Next(0, (int)gridWidth-1);
        int randHeight = rnd.Next(0, (int)gridHeight-1);
        Vector3 worldPoint = worldBottomLeft + Vector3.right * (randWidth * tileSize + tileSize/2) + Vector3.forward * (randHeight * tileSize + tileSize/2);//Get the world co ordinates of the rabbit from the bottom left of the graph
        
        GameObject rabbitClone = Instantiate(rabbit, worldPoint, rabbit.transform.rotation) as GameObject;
        rabbitClone.transform.parent = rabbitContainer.transform;
        rabbitClone.name = "RabbitClone" + (iterator + 1);
    }

    void CreateGrass(int iterator)
    {
        int randWidth = rnd.Next(0, (int)gridWidth-1);
        int randHeight = rnd.Next(0, (int)gridHeight-1);
        Vector3 worldPoint = worldBottomLeft + Vector3.right * (randWidth * tileSize + tileSize/2) + Vector3.forward * (randHeight * tileSize + tileSize/2);//Get the world co ordinates of the rabbit from the bottom left of the graph

        GameObject grassClone = Instantiate(grass, worldPoint, grass.transform.rotation) as GameObject;
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

