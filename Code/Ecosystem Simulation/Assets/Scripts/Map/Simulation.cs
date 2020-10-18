using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    // To be used as main script for the sim

    public GameObject grassTile;
    public GameObject lightGrassTile;
    public GameObject rabbit;
    public GameObject grass;
    private int gridWidth = 10;
    private int gridHeight = 10;
    private float tileSize;
    private float leftLimit, upLimit, rightLimit, downLimit;
    private System.Random rnd;

    // Start is called before the first frame update
    void Awake()
    {
        rnd = new System.Random();
        CreateTiles();
        SetLimits();
        CreateRabbits();
        CreateGrass();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                {
                    GameObject lightTileClone = Instantiate(lightGrassTile, new Vector3(xPos, yPos, zPos), lightGrassTile.transform.rotation);  //Place the light green tile
                }
                else
                {
                    GameObject grassTileClone = Instantiate(grassTile, new Vector3(xPos, yPos, zPos), grassTile.transform.rotation);             //Place the green tile
                }
            }
        }
    }

    void CreateRabbits()
    {
        int rnd1 = rnd.Next(0, (int)gridWidth);
        int rnd2 = rnd.Next(0, (int)gridHeight);
        float rabXPos = rnd1 * tileSize;
        float rabZPos = rnd2 * tileSize;
        Instantiate(rabbit, new Vector3(rabXPos, 0, rabZPos), rabbit.transform.rotation);
        rabbit.transform.localScale = new Vector3(3f, 3f, 3f);
    }

    void CreateGrass()
    {
        
    }
    void SetLimits()
    {
        upLimit = (float)(gridHeight - 1) * tileSize;
        leftLimit = 0;
        rightLimit = (gridWidth - 1) * tileSize;
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
}

