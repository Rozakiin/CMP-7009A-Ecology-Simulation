using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct BasicNeedsData : IComponentData
    {
        //Thirst
        public float Thirst;
        public float ThirstyThreshold;
        public float ThirstMax;
        public float ThirstIncrease;
        public float DrinkingSpeed;

        //Hunger
        public float Hunger;
        public float HungryThreshold;
        public float HungerMax;
        public float HungerIncrease;
        public float PregnancyHungerIncrease;
        public float YoungHungerIncrease;
        public float AdultHungerIncrease;
        public float OldHungerIncrease;
        public float EatingSpeed;

        public enum DietType
        {
            Carnivore = EdibleData.FoodTypes.Meat,
            Herbivore = EdibleData.FoodTypes.Plant | EdibleData.FoodTypes.Fungi,
            Omnivore = EdibleData.FoodTypes.Meat | EdibleData.FoodTypes.Plant | EdibleData.FoodTypes.Fungi,
        }
        public DietType Diet;
    }
}
