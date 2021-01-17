using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct EdibleData : IComponentData
    {
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
}
