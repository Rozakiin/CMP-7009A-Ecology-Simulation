using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct EdibleData : IComponentData
    {
        public bool CanBeEaten;
        public float NutritionalValueBase;
        public float NutritionalValueMultiplier;
        public float NutritionalValue => NutritionalValueBase * NutritionalValueMultiplier;

        //Not ideal groupings but will work when only one type of predator
        //Allows FoodTypes to be OR bitwise to have multiple preferences
        [Flags]
        public enum FoodTypes
        {
            Plant = 0b_0000_0001, //1
            Fungi = 0b_0000_0010, //2
            Meat = 0b_0000_0100 //4
        }

        public FoodTypes FoodType;
    }
}