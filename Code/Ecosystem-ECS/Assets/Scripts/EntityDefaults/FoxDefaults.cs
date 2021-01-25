using Components;
using Unity.Entities;

/* Use this class to define all the default values for components related to Fox Entities */
namespace EntityDefaults
{
    public static class FoxDefaults
    {
        //time periods in hours
        private const int YEAR = 8766;
        private const int MONTH = 730;
        private const int WEEK = 168;
        private const int DAY = 24;
        private const int HOUR = 1;


        //Age
        public static float age = 0f;
        public static float ageIncrease = 1f;
        public static float ageMax = 7*YEAR;
        public static BioStatsData.AgeGroup ageGroup = BioStatsData.AgeGroup.Young;
        public static float adultEntryTimer = 1*WEEK;
        public static float oldEntryTimer = 5*YEAR;


        //Edible
        public static float nutritionalValue = 100f;
        public static bool canBeEaten = false;
        public static float nutritionalValueMultiplier = 1f;
        public static EdibleData.FoodType foodType = EdibleData.FoodType.Meat;


        //Hunger
        public static float hunger = 0f;
        public static float hungerMax = 2*WEEK;
        public static float hungryThreshold = 3*DAY;
        public static float hungerIncrease = 1f;
        public static float pregnancyHungerIncrease = 0.7f;
        public static float youngHungerIncrease = 0.3f;
        public static float adultHungerIncrease = 1f;
        public static float oldHungerIncrease = 0.5f;
        public static float eatingSpeed = 1f;
        public static BasicNeedsData.Diet diet = BasicNeedsData.Diet.Carnivore;


        //Thirst
        public static float thirst = 0f;
        public static float thirstMax = 3*DAY;
        public static float thirstyThreshold = 0.5f*DAY;
        public static float thirstIncrease = 1f;
        public static float drinkingSpeed = 2f;


        //Mate
        public static float mateStartTime = 0f;
        public static float matingDuration = 1*HOUR;
        public static float reproductiveUrge = 0f;
        public static float reproductiveUrgeIncreaseMale = 1f;
        public static float reproductiveUrgeIncreaseFemale = 0f;
        public static float matingThreshold = 1*DAY;


        //Pregnancy
        public static float pregnancyStartTime = 0f;
        public static int babiesBorn = 0;
        public static float birthStartTime = 0f;
        public static int currentLitterSize = 0;
        public static float pregnancyLengthModifier = 1f;
        public static float pregnancyLength = 2*DAY;
        public static float birthDuration = 1*HOUR;
        public static int litterSizeMin = 1;
        public static int litterSizeMax = 14;
        public static int litterSizeAve = 5;


        //Movement
        public static float moveSpeed = 35f;
        public static float rotationSpeed = 10f;
        public static float moveMultiplier = 1f;
        public static float pregnancyMoveMultiplier = 0.5f;
        public static float originalMoveMultiplier = 1f;
        public static float youngMoveMultiplier = 0.4f;
        public static float adultMoveMultiplier = 1f;
        public static float oldMoveMultiplier = 0.4f;


        //Size
        public static float sizeMultiplier = 1f;
        public static float scaleMale = 3f;
        public static float scaleFemale = 2f;
        public static float youngSizeMultiplier = 0.6f;
        public static float adultSizeMultiplier = 1f;
        public static float oldSizeMultiplier = 0.75f;
        public static float ageSizeMultiplier = youngSizeMultiplier;


        //State
        public static StateData.FlagStates flagState = StateData.FlagStates.Wandering;
        public static StateData.FlagStates previousFlagState = StateData.FlagStates.Wandering;
        public static StateData.DeathReason deathReason = StateData.DeathReason.Eaten;
        public static bool beenEaten = false;


        //Target
        public static float touchRadius = 1f;
        public static float sightRadius = 50f;
        public static float mateRadius = 5f;

        //lookingEntityData
        public static Entity predatorEntity = Entity.Null;
        public static Entity entityToEat = Entity.Null;
        public static Entity entityToDrink = Entity.Null;
        public static Entity entityToMate = Entity.Null;
        public static float shortestToEdibleDistance = float.PositiveInfinity;
        public static float shortestToWaterDistance = float.PositiveInfinity;
        public static float shortestToPredatorDistance = float.PositiveInfinity;
        public static float shortestToMateDistance = float.PositiveInfinity;


        //ColliderTypeData
        public static ColliderTypeData.ColliderType colliderType = ColliderTypeData.ColliderType.Fox;
    }
}
