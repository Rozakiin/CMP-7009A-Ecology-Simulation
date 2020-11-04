using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Edible
{
    private int currentHeight; // current growth height of the grass
    private static int maxHeight = 4; // max height to be shared by all grass

    void Awake()
    {
        scene = GameObject.FindWithTag("GameController").GetComponent<Simulation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(10f, 10f, 10f);
        currentHeight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
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
