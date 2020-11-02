using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Edible
{
    private int currentHeight; // current growth height of the grass
    private static int maxHeight = 4; // max height to be shared by all grass
    private float xPos, zPos; // x and z position
    private float leftLimit, upLimit, rightLimit, downLimit; // limits of where the grass can be

    public int NutritionalValue()
    {
        return baseNutritionalValue * currentHeight;
    }

    void Awake()
    {
        scene = GameObject.FindWithTag("GameController").GetComponent<Simulation>();
    }

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

    public override int GetNutritionalValue()
    {
        return nutritionalValue;
    }

    public override void SetNutritionalValue()
    {
        nutritionalValue = baseNutritionalValue * currentHeight;
        //throw new System.NotImplementedException();
    }
}
