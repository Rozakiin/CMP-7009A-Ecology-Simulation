using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateRabbits : MonoBehaviour
{

    public CreateGrid createGrid;
    public GameObject rabbit;


    // Start is called before the first frame update
    void Start()
    {
        float gridWidth = createGrid.GetWidth();
        float gridHeight = createGrid.GetHeight();
        int rnd1 = new System.Random().Next(0, (int)gridWidth);
        int rnd2 = new System.Random().Next(0, (int)gridHeight);
        float sizeX = createGrid.GetSizeX();
        float sizeZ = createGrid.GetSizeZ();
        Instantiate(rabbit, new Vector3(rnd1, 0, rnd2), rabbit.transform.rotation);
    }

}
