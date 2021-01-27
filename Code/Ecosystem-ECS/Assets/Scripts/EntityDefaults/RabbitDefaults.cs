using Components;
using Unity.Entities;

/* Use this class to define all the default values for components related to Rabbit Entities */
namespace EntityDefaults
{
    public class RabbitDefaults : DefaultsBase
    {
        //Age
        public static float Age = 0f;
        public static float AgeIncrease = 1f;
        public static float AgeMax = 10 * Year;
        public static BioStatsData.AgeGroups AgeGroup = BioStatsData.AgeGroups.Young;
        public static float AdultEntryTimer = 1 * Month;
        public static float OldEntryTimer = 6 * Year;


        //Edible
        public static float NutritionalValue = 50f;
        public static bool CanBeEaten = true;
        public static float NutritionalValueMultiplier = 1f;
        public static EdibleData.FoodTypes FoodType = EdibleData.FoodTypes.Meat;


        //Hunger
        public static float Hunger = 0f;
        public static float HungerMax = 4 * Day;
        public static float HungryThreshold = 1 * Day;
        public static float HungerIncrease = 1f;
        public static float PregnancyHungerIncrease = 0.7f;
        public static float YoungHungerIncrease = 0.3f;
        public static float AdultHungerIncrease = 1f;
        public static float OldHungerIncrease = 0.5f;
        public static float EatingSpeed = 1f;
        public static BasicNeedsData.DietType Diet = BasicNeedsData.DietType.Herbivore;


        //Thirst
        public static float Thirst = 0f;
        public static float ThirstMax = 100f;
        public static float ThirstyThreshold = 0.5f * Day;
        public static float ThirstIncrease = 1f;
        public static float DrinkingSpeed = 2f;


        //Mate
        public static float MateStartTime = 0f;
        public static float MatingDuration = 1 * Hour;
        public static float ReproductiveUrge = 0f;
        public static float ReproductiveUrgeIncreaseMale = 1f;
        public static float ReproductiveUrgeIncreaseFemale = 0f;
        public static float MatingThreshold = 1 * Day;


        //Pregnancy
        public static float PregnancyStartTime = 0f;
        public static int BabiesBorn = 0;
        public static float BirthStartTime = 0f;
        public static int CurrentLitterSize = 0;
        public static float PregnancyLengthModifier = 1f;
        public static float PregnancyLength = 1 * Month;
        public static float BirthDuration = 1 * Hour;
        public static int LitterSizeMin = 1;
        public static int LitterSizeMax = 13;
        public static int LitterSizeAve = 7;


        //Movement
        public static float MoveSpeed = 25f;
        public static float RotationSpeed = 10f;
        public static float MoveMultiplier = 1f;
        public static float PregnancyMoveMultiplier = 0.5f;
        public static float OriginalMoveMultiplier = 1f;
        public static float YoungMoveMultiplier = 0.4f;
        public static float AdultMoveMultiplier = 1f;
        public static float OldMoveMultiplier = 0.4f;


        //Size
        public static float SizeMultiplier = 1f;
        public static float ScaleMale = 2f;
        public static float ScaleFemale = 3f;
        public static float YoungSizeMultiplier = 0.6f;
        public static float AdultSizeMultiplier = 1f;
        public static float OldSizeMultiplier = 0.75f;
        public static float AgeSizeMultiplier = YoungSizeMultiplier;


        //State
        public static StateData.FlagStates FlagState = StateData.FlagStates.Wandering;
        public static StateData.FlagStates FlagStatePrevious = StateData.FlagStates.Wandering;
        public static StateData.DeathReasons DeathReason = StateData.DeathReasons.Eaten;
        public static bool BeenEaten = false;


        //Target
        public static float TouchRadius = 1f;
        public static float SightRadius = 20f;
        public static float MateRadius = 5f;


        //lookingEntityData
        public static Entity PredatorEntity = Entity.Null;
        public static Entity EntityToEat = Entity.Null;
        public static Entity EntityToDrink = Entity.Null;
        public static Entity EntityToMate = Entity.Null;
        public static float ShortestToEdibleDistance = float.PositiveInfinity;
        public static float ShortestToWaterDistance = float.PositiveInfinity;
        public static float ShortestToPredatorDistance = float.PositiveInfinity;
        public static float ShortestToMateDistance = float.PositiveInfinity;


        //ColliderTypeData
        public static ColliderTypeData.ColliderType Collider = ColliderTypeData.ColliderType.Rabbit;
    }
}