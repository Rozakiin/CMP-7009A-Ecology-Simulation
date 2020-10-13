using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public GameObject grassTile;
    public GameObject lightGrassTile;
    private float gridWidth = 10f;
    private float gridHeight = 10f;
    private float sizeX;
    private float sizeZ;


    // Start is called before the first frame update
    void Start()
    {
        //GameObject tileClone = Instantiate();
        for(int i=0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                sizeX = grassTile.GetComponent<Renderer>().bounds.size.x;                   //Get the width of the tile
                sizeZ = grassTile.GetComponent<Renderer>().bounds.size.z;                   //Get the length of the tile
                float xPos = i * sizeX;                                                     //Get the tile's x position
                float yPos = grassTile.transform.position.y;                                //Get the tile's y position (always the same)
                float zPos = j * sizeZ;                                                     //Get the tile's z position
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

    public float GetWidth()
    {
        return gridWidth;
    }

    public float GetHeight()
    {
        return gridHeight;
    }

    public float GetSizeX()
    {
        return sizeX;
    }

    public float GetSizeZ()
    {
        return sizeZ;
    }
}
