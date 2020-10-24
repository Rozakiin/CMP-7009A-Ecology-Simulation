using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour, Edible
{
    public Simulation scene;
    private int currentHeight; // current growth height of the grass
    private static int maxHeight = 4; // max height to be shared by all grass
    //private static int baseEnergy = 20; // base energy to be shared by all grass
    private float xPos, zPos; // x and z position
    private float leftLimit, upLimit, rightLimit, downLimit; // limits of where the grass can be

    //Edible Interface
    public int baseNutritionalValue { get; set; } = 20;
    public bool canBeEaten { get; set; } = true;
    public int NutritionalValue()
    {
        return baseNutritionalValue * currentHeight;
    }

    // Energy is a property that multiplies baseEnergy with grass height
    // to give the energy when eaten
/*    public int Energy
    {
        get
        {
            return baseEnergy * currentHeight;
        }
        set
        {
            baseEnergy = value / currentHeight;
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.position.x;
        zPos = transform.position.z;
        GetLimits();
        transform.localScale = new Vector3(10f, 10f, 10f);
        currentHeight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetLimits()
    {
        leftLimit = scene.GetLeftLimit();
        upLimit = scene.GetUpLimit();
        rightLimit = scene.GetRightLimit();
        downLimit = scene.GetDownLimit();
    }

    // Setter for Max Height of grass
    void SetMaxHeight(int height)
    {
        maxHeight = height;
    }
}
