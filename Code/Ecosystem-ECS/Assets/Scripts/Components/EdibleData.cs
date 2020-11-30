using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct EdibleData : IComponentData
{
    // Add fields to your component here. Remember that:
    //
    // * A component itself is for storing data and doesn't 'do' anything.
    //
    // * To act on the data, you will need a System.
    //
    // * Data in a component must be blittable, which means a component can
    //   only contain fields which are primitive types or other blittable
    //   structs; they cannot contain references to classes.
    //
    // * You should focus on the data structure that makes the most sense
    //   for runtime use here. Authoring Components will be used for 
    //   authoring the data in the Editor.

    public bool canBeEaten;
    public float nutritionalValueBase;
    public float nutritionalValueMultiplier;
    public float NutritionalValue
    {
        get { return nutritionalValueBase * nutritionalValueMultiplier; }
    }
    //Not ideal groupings but will work when only one type of predator
    //Allows FoodTypes to be OR bitwise to have multiple preferences
    [Flags]
    public enum FoodType
    {
        Plant = 0b_0000_0001,//1
        Fungi = 0b_0000_0010, //2
        Meat = 0b_0000_0100,//4
    }
    public FoodType foodType;

}
