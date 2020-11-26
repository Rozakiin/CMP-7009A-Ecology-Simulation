using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct HungerData : IComponentData
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

    public float hunger;
    public float hungryThreshold;
    public float hungerMax;
    public float hungerIncrease;
    public float eatingSpeed;
    public Entity entityToEat;
    public Entity entityPredator;

    public enum Diet
    {
        Carnivore = EdibleData.FoodType.Meat,
        Herbivore = EdibleData.FoodType.Plant | EdibleData.FoodType.Fungi,
        Omnivore = EdibleData.FoodType.Meat | EdibleData.FoodType.Plant | EdibleData.FoodType.Fungi,
    }
    public Diet diet;
}
