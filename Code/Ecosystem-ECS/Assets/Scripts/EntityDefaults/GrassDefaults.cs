/* Use this class to define all the default values for components related to Grass Entities */

using Components;

namespace EntityDefaults
{
    public static class GrassDefaults
    {
        //time periods in hours
        private const int YEAR = 8766;
        private const int MONTH = 730;
        private const int WEEK = 168;
        private const int DAY = 24;
        private const int HOUR = 1;

        //Edible
        public static float nutritionalValue = 25f;
        public static bool canBeEaten = true;
        public static float nutritionalValueMultiplier = 1f;
        public static EdibleData.FoodType foodType = EdibleData.FoodType.Plant;


        //Size
        public static float sizeMultiplier = 1f;
        public static float scale = 5f;


        //State
        public static StateData.FlagStates flagState = StateData.FlagStates.None;
        public static StateData.FlagStates previousFlagState = StateData.FlagStates.None;
        public static StateData.DeathReason deathReason = StateData.DeathReason.Eaten;
        public static bool beenEaten = false;


        //ColliderTypeData
        public static ColliderTypeData.ColliderType GrassColliderType = ColliderTypeData.ColliderType.Grass;
    }
}
