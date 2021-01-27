/* Use this class to define all the default values for components related to Grass Entities */

using Components;

namespace EntityDefaults
{
    public class GrassDefaults : DefaultsBase
    {
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