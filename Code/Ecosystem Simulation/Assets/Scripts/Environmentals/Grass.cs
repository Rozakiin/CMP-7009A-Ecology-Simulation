using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private int currentHeight;
    private static int maxHeight = 4; // max height to be shared by all grass
    private static int baseEnergy = 20; // base energy to be shared by all grass

    // Default Grass constructor
    Grass()
    {
        currentHeight = 0;
    }

    // Grass constructor with specifed current height
    Grass(int height)
    {
        currentHeight = height;
    }

    // Setter for Max Height of grass
    void SetMaxHeight(int height)
    {
        maxHeight = height;
    }

    // Energy is a property that multiplies baseEnergy with grass height
    // to give the energy when eaten
    public int Energy
    {
        get
        {
            return baseEnergy * currentHeight;
        }
        set
        {
            baseEnergy = value / currentHeight;
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
}
