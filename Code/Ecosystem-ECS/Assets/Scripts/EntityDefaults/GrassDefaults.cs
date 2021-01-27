/* Use this class to define all the default values for components related to Grass Entities */

using Components;

namespace EntityDefaults
{
    public static class GrassDefaults
    {
        //time periods in hours
        private const int Year = 8766;
        private const int Month = 730;
        private const int Week = 168;
        private const int Day = 24;
        private const int Hour = 1;

        //Edible
        public static float NutritionalValue = 25f;
        public static bool CanBeEaten = true;
        public static float NutritionalValueMultiplier = 1f;
        public static EdibleData.FoodTypes FoodType = EdibleData.FoodTypes.Plant;


        //Size
        public static float SizeMultiplier = 1f;
        public static float Scale = 5f;


        //State
        public static StateData.FlagStates FlagState = StateData.FlagStates.None;
        public static StateData.FlagStates PreviousFlagState = StateData.FlagStates.None;
        public static StateData.DeathReasons DeathReason = StateData.DeathReasons.Eaten;
        public static bool BeenEaten = false;


        //ColliderTypeData
        public static ColliderTypeData.ColliderType Collider = ColliderTypeData.ColliderType.Grass;
    }
}
