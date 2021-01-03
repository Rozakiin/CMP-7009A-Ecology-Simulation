using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct BasicNeedsData : IComponentData
{
    //Thirst
    public float thirst;
    public float thirstyThreshold;
    public float thirstMax;
    public float thirstIncrease;
    public float drinkingSpeed;

    //Hunger
    public float hunger;
    public float hungryThreshold;
    public float hungerMax;
    public float hungerIncrease;
    public float pregnancyHungerIncrease;
    public float youngHungerIncrease;
    public float adultHungerIncrease;
    public float oldHungerIncrease;
    public float eatingSpeed;

    public enum Diet
    {
        Carnivore = EdibleData.FoodType.Meat,
        Herbivore = EdibleData.FoodType.Plant | EdibleData.FoodType.Fungi,
        Omnivore = EdibleData.FoodType.Meat | EdibleData.FoodType.Plant | EdibleData.FoodType.Fungi,
    }
    public Diet diet;
}
