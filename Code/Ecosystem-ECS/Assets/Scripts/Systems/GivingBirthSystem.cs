using Components;
using EntityDefaults;
using Unity.Entities;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace Systems
{
    public class GivingBirthSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem _ecbSystem;


        protected override void OnCreate()
        {
            base.OnCreate();
            _ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        /*
         * Gives birth (spawns) to a child entity of the same type as the entity giving birth
         * sets some inherited values from the parent
         */
        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer().ToConcurrent();

            float time = UnityEngine.Time.time;
            float timeSeed = time * System.DateTimeOffset.Now.Millisecond;

            #region Default Rabbit stats

            //Edible
            var rabbitNutritionalValue = RabbitDefaults.NutritionalValue;
            var rabbitCanBeEaten = RabbitDefaults.CanBeEaten;
            var rabbitNutritionalValueMultiplier = RabbitDefaults.NutritionalValueMultiplier;
            EdibleData.FoodTypes rabbitFoodType = RabbitDefaults.FoodType;

            //Movement
            var rabbitRotationSpeed = RabbitDefaults.RotationSpeed;
            var rabbitMoveSpeedBase = RabbitDefaults.MoveSpeed;
            var rabbitMoveMultiplier = RabbitDefaults.MoveMultiplier;
            var rabbitPregnancyMoveMultiplier = RabbitDefaults.PregnancyMoveMultiplier;
            var rabbitOriginalMoveMultiplier = RabbitDefaults.OriginalMoveMultiplier;
            var rabbitYoungMoveMultiplier = RabbitDefaults.YoungMoveMultiplier;
            var rabbitAdultMoveMultiplier = RabbitDefaults.AdultMoveMultiplier;
            var rabbitOldMoveMultiplier = RabbitDefaults.OldMoveMultiplier;

            //States
            StateData.FlagStates rabbitFlagState = RabbitDefaults.FlagState;
            StateData.FlagStates rabbitPreviousFlagState = RabbitDefaults.FlagStatePrevious;
            StateData.DeathReasons rabbitDeathReason = RabbitDefaults.DeathReason;
            var rabbitBeenEaten = RabbitDefaults.BeenEaten;

            //Target data
            var rabbitSightRadius = RabbitDefaults.SightRadius;
            var rabbitTouchRadius = RabbitDefaults.TouchRadius;
            var rabbitMateRadius = RabbitDefaults.MateRadius;

            //Basic needs data
            var rabbitHunger = RabbitDefaults.Hunger;
            var rabbitHungryThreshold = RabbitDefaults.HungryThreshold;
            var rabbitHungerMax = RabbitDefaults.HungerMax;
            var rabbitHungerIncrease = RabbitDefaults.HungerIncrease;
            var rabbitPregnancyHungerIncrease = RabbitDefaults.PregnancyHungerIncrease;
            var rabbitYoungHungerIncrease = RabbitDefaults.YoungHungerIncrease;
            var rabbitAdultHungerIncrease = RabbitDefaults.AdultHungerIncrease;
            var rabbitOldHungerIncrease = RabbitDefaults.OldHungerIncrease;
            var rabbitEatingSpeed = RabbitDefaults.EatingSpeed;
            var rabbitEntityToEat = RabbitDefaults.EntityToEat;
            var rabbitDiet = RabbitDefaults.Diet;
            var rabbitThirst = RabbitDefaults.Thirst;
            var rabbitThirstyThreshold = RabbitDefaults.ThirstyThreshold;
            var rabbitThirstMax = RabbitDefaults.ThirstMax;
            var rabbitThirstIncrease = RabbitDefaults.ThirstIncrease;
            var rabbitDrinkingSpeed = RabbitDefaults.DrinkingSpeed;
            var rabbitEntityToDrink = RabbitDefaults.EntityToDrink;

            //Bio stats data
            var rabbitAge = RabbitDefaults.Age;
            var rabbitAgeIncrease = RabbitDefaults.AgeIncrease;
            var rabbitAgeMax = RabbitDefaults.AgeMax;
            var rabbitAgeGroup = RabbitDefaults.AgeGroup;
            var rabbitAdultEntryTimer = RabbitDefaults.AdultEntryTimer;
            var rabbitOldEntryTimer = RabbitDefaults.OldEntryTimer;

            //Reproducite data
            var rabbitMatingDuration = RabbitDefaults.MatingDuration;
            var rabbitMateStartTime = RabbitDefaults.MateStartTime;
            var rabbitReproductiveUrge = RabbitDefaults.ReproductiveUrge;
            var rabbitMatingThreshold = RabbitDefaults.MatingThreshold;
            var rabbitEntityToMate = RabbitDefaults.EntityToMate;

            var rabbitBirthDuration = RabbitDefaults.BirthDuration;
            var rabbitBabiesBorn = RabbitDefaults.BabiesBorn;
            var rabbitBirthStartTime = RabbitDefaults.BirthStartTime;
            var rabbitCurrentLitterSize = RabbitDefaults.CurrentLitterSize;
            var rabbitLitterSizeMin = RabbitDefaults.LitterSizeMin;
            var rabbitLitterSizeMax = RabbitDefaults.LitterSizeMax;
            var rabbitLitterSizeAve = RabbitDefaults.LitterSizeAve;
            var rabbitPregnancyLengthBase = RabbitDefaults.PregnancyLength;
            var rabbitPregnancyLengthModifier = RabbitDefaults.PregnancyLengthModifier;
            var rabbitPregnancyStartTime = RabbitDefaults.PregnancyStartTime;

            var rabbitReproductiveUrgeIncreaseFemale = RabbitDefaults.ReproductiveUrgeIncreaseFemale;
            var rabbitReproductiveUrgeIncreaseMale = RabbitDefaults.ReproductiveUrgeIncreaseMale;

            //Size data
            var rabbitSizeMultiplier = RabbitDefaults.SizeMultiplier;
            var rabbitAgeSizeMultiplier = RabbitDefaults.AgeSizeMultiplier;
            var rabbitYoungSizeMultiplier = RabbitDefaults.YoungSizeMultiplier;
            var rabbitAdultSizeMultiplier = RabbitDefaults.AdultSizeMultiplier;
            var rabbitOldSizeMultiplier = RabbitDefaults.OldSizeMultiplier;

            var rabbitScaleFemale = RabbitDefaults.ScaleFemale;
            var rabbitScaleMale = RabbitDefaults.ScaleMale;

            #endregion

            #region Default Fox stats

            //Edible
            var foxNutritionalValue = FoxDefaults.NutritionalValue;
            var foxCanBeEaten = FoxDefaults.CanBeEaten;
            var foxNutritionalValueMultiplier = FoxDefaults.NutritionalValueMultiplier;
            EdibleData.FoodTypes foxFoodType = FoxDefaults.FoodType;

            //Movement
            var foxRotationSpeed = FoxDefaults.RotationSpeed;
            var foxMoveSpeedBase = FoxDefaults.MoveSpeed;
            var foxMoveMultiplier = FoxDefaults.MoveMultiplier;
            var foxPregnancyMoveMultiplier = FoxDefaults.PregnancyMoveMultiplier;
            var foxOriginalMoveMultiplier = FoxDefaults.OriginalMoveMultiplier;
            var foxYoungMoveMultiplier = FoxDefaults.YoungMoveMultiplier;
            var foxAdultMoveMultiplier = FoxDefaults.AdultMoveMultiplier;
            var foxOldMoveMultiplier = FoxDefaults.OldMoveMultiplier;

            //States
            StateData.FlagStates foxFlagState = FoxDefaults.FlagState;
            StateData.FlagStates foxPreviousFlagState = FoxDefaults.PreviousFlagState;
            StateData.DeathReasons foxDeathReason = FoxDefaults.DeathReason;
            var foxBeenEaten = FoxDefaults.BeenEaten;

            //Target data
            var foxSightRadius = FoxDefaults.SightRadius;
            var foxTouchRadius = FoxDefaults.TouchRadius;
            var foxMateRadius = FoxDefaults.MateRadius;

            //Basic needs data
            var foxHunger = FoxDefaults.Hunger;
            var foxHungryThreshold = FoxDefaults.HungryThreshold;
            var foxHungerMax = FoxDefaults.HungerMax;
            var foxHungerIncrease = FoxDefaults.HungerIncrease;
            var foxPregnancyHungerIncrease = FoxDefaults.PregnancyHungerIncrease;
            var foxYoungHungerIncrease = FoxDefaults.YoungHungerIncrease;
            var foxAdultHungerIncrease = FoxDefaults.AdultHungerIncrease;
            var foxOldHungerIncrease = FoxDefaults.OldHungerIncrease;
            var foxEatingSpeed = FoxDefaults.EatingSpeed;
            var foxEntityToEat = FoxDefaults.EntityToEat;
            var foxDiet = FoxDefaults.Diet;
            var foxThirst = FoxDefaults.Thirst;
            var foxThirstyThreshold = FoxDefaults.ThirstyThreshold;
            var foxThirstMax = FoxDefaults.ThirstMax;
            var foxThirstIncrease = FoxDefaults.ThirstIncrease;
            var foxDrinkingSpeed = FoxDefaults.DrinkingSpeed;
            var foxEntityToDrink = FoxDefaults.EntityToDrink;

            //Bio stats data
            var foxAge = FoxDefaults.Age;
            var foxAgeIncrease = FoxDefaults.AgeIncrease;
            var foxAgeMax = FoxDefaults.AgeMax;
            var foxAgeGroup = FoxDefaults.AgeGroup;
            var foxAdultEntryTimer = FoxDefaults.AdultEntryTimer;
            var foxOldEntryTimer = FoxDefaults.OldEntryTimer;

            //Reproductive data
            var foxMatingDuration = FoxDefaults.MatingDuration;
            var foxMateStartTime = FoxDefaults.MateStartTime;
            var foxReproductiveUrge = FoxDefaults.ReproductiveUrge;
            var foxMatingThreshold = FoxDefaults.MatingThreshold;
            var foxEntityToMate = FoxDefaults.EntityToMate;

            var foxBirthDuration = FoxDefaults.BirthDuration;
            var foxBabiesBorn = FoxDefaults.BabiesBorn;
            var foxBirthStartTime = FoxDefaults.BirthStartTime;
            var foxCurrentLitterSize = FoxDefaults.CurrentLitterSize;
            var foxLitterSizeMin = FoxDefaults.LitterSizeMin;
            var foxLitterSizeMax = FoxDefaults.LitterSizeMax;
            var foxLitterSizeAve = FoxDefaults.LitterSizeAve;
            var foxPregnancyLengthBase = FoxDefaults.PregnancyLength;
            var foxPregnancyLengthModifier = FoxDefaults.PregnancyLengthModifier;
            var foxPregnancyStartTime = FoxDefaults.PregnancyStartTime;

            var foxReproductiveUrgeIncreaseFemale = FoxDefaults.ReproductiveUrgeIncreaseFemale;
            var foxReproductiveUrgeIncreaseMale = FoxDefaults.ReproductiveUrgeIncreaseMale;

            //Size data
            var foxSizeMultiplier = FoxDefaults.SizeMultiplier;
            var foxAgeSizeMultiplier = FoxDefaults.AgeSizeMultiplier;
            var foxYoungSizeMultiplier = FoxDefaults.YoungSizeMultiplier;
            var foxAdultSizeMultiplier = FoxDefaults.AdultSizeMultiplier;
            var foxOldSizeMultiplier = FoxDefaults.OldSizeMultiplier;

            var foxScaleFemale = FoxDefaults.ScaleFemale;
            var foxScaleMale = FoxDefaults.ScaleMale;

            #endregion

            Entities.ForEach((
                Entity entity,
                int entityInQueryIndex,
                ref ReproductiveData reproductiveData,
                in StateData stateData,
                in BioStatsData bioStatsData,
                in Translation translation
            ) =>
            {
                if (stateData.IsGivingBirth)
                {
                    //Determine if enough time has passed since the last baby was born and if there are still babies to be born
                    if ((bioStatsData.Age - reproductiveData.BirthStartTime >= reproductiveData.BirthDuration) &&
                        reproductiveData.BabiesBorn < reproductiveData.CurrentLitterSize)
                    {
                        //give birth
                        #region Setting New Entity's Components
                        // determine if rabbit or fox giving birth - not very scalable
                        if (HasComponent<IsRabbitTag>(entity))
                        {
                            Entity newEntity = ecb.Instantiate(entityInQueryIndex, entity);

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new MovementData
                                {
                                    RotationSpeed = rabbitRotationSpeed,
                                    MoveSpeedBase = rabbitMoveSpeedBase,
                                    MoveMultiplier = rabbitMoveMultiplier,
                                    PregnancyMoveMultiplier = rabbitPregnancyMoveMultiplier,
                                    OriginalMoveMultiplier = rabbitOriginalMoveMultiplier,
                                    YoungMoveMultiplier = rabbitYoungMoveMultiplier,
                                    AdultMoveMultiplier = rabbitAdultMoveMultiplier,
                                    OldMoveMultiplier = rabbitOldMoveMultiplier
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new StateData
                                {
                                    FlagStateCurrent = rabbitFlagState,
                                    FlagStatePrevious = rabbitPreviousFlagState,
                                    DeathReason = rabbitDeathReason,
                                    BeenEaten = rabbitBeenEaten
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new TargetData
                                {
                                    AtTarget = true,
                                    Target = translation.Value,
                                    TargetOld = translation.Value,

                                    SightRadius = rabbitSightRadius,
                                    TouchRadius = rabbitTouchRadius,
                                    MateRadius = rabbitMateRadius
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new PathFollowData
                                {
                                    PathIndex = -1
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new BasicNeedsData
                                {
                                    Hunger = rabbitHunger,
                                    HungryThreshold = rabbitHungryThreshold,
                                    HungerMax = rabbitHungerMax,
                                    HungerIncrease = rabbitHungerIncrease,
                                    PregnancyHungerIncrease = rabbitPregnancyHungerIncrease,
                                    YoungHungerIncrease = rabbitYoungHungerIncrease,
                                    AdultHungerIncrease = rabbitAdultHungerIncrease,
                                    OldHungerIncrease = rabbitOldHungerIncrease,
                                    EatingSpeed = rabbitEatingSpeed,
                                    Diet = rabbitDiet,
                                    Thirst = rabbitThirst,
                                    ThirstyThreshold = rabbitThirstyThreshold,
                                    ThirstMax = rabbitThirstMax,
                                    ThirstIncrease = rabbitThirstIncrease,
                                    DrinkingSpeed = rabbitDrinkingSpeed,
                                }
                            );

                            float seed = timeSeed * (translation.Value.x * translation.Value.z) + entity.Index;
                            Random randomGen = new Random((uint)seed + 2);
                            BioStatsData.Genders randGender = randomGen.NextInt(0, 2) == 1
                                ? randGender = BioStatsData.Genders.Female
                                : randGender = BioStatsData.Genders.Male;

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new BioStatsData
                                {
                                    Age = rabbitAge,
                                    AgeIncrease = rabbitAgeIncrease,
                                    AgeMax = rabbitAgeMax,
                                    AgeGroup = rabbitAgeGroup,
                                    AdultEntryTimer = rabbitAdultEntryTimer,
                                    OldEntryTimer = rabbitOldEntryTimer,
                                    Gender = randGender
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new ReproductiveData
                                {
                                    MatingDuration = rabbitMatingDuration,
                                    MateStartTime = rabbitMateStartTime,
                                    ReproductiveUrge = rabbitReproductiveUrge,
                                    ReproductiveUrgeIncrease = (randGender == BioStatsData.Genders.Female
                                        ? rabbitReproductiveUrgeIncreaseFemale
                                        : rabbitReproductiveUrgeIncreaseMale),
                                    DefaultReproductiveIncrease = (randGender == BioStatsData.Genders.Female
                                        ? rabbitReproductiveUrgeIncreaseFemale
                                        : rabbitReproductiveUrgeIncreaseMale),
                                    MatingThreshold = rabbitMatingThreshold,

                                    BirthDuration = rabbitBirthDuration,
                                    BabiesBorn = rabbitBabiesBorn,
                                    BirthStartTime = rabbitBirthStartTime,
                                    CurrentLitterSize = rabbitCurrentLitterSize,
                                    LitterSizeMin = rabbitLitterSizeMin,
                                    LitterSizeMax = rabbitLitterSizeMax,
                                    LitterSizeAve = rabbitLitterSizeAve,
                                    PregnancyLengthBase = rabbitPregnancyLengthBase,
                                    PregnancyLengthModifier = rabbitPregnancyLengthModifier,
                                    PregnancyStartTime = rabbitPregnancyStartTime
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new SizeData
                                {
                                    size = (randGender == BioStatsData.Genders.Female
                                        ? rabbitScaleFemale
                                        : rabbitScaleMale),
                                    SizeMultiplier = rabbitSizeMultiplier,
                                    AgeSizeMultiplier = rabbitAgeSizeMultiplier,
                                    YoungSizeMultiplier = rabbitYoungSizeMultiplier,
                                    AdultSizeMultiplier = rabbitAdultSizeMultiplier,
                                    OldSizeMultiplier = rabbitOldSizeMultiplier

                                }
                            );
                        }
                        else if (HasComponent<IsFoxTag>(entity))
                        {
                            Entity newEntity = ecb.Instantiate(entityInQueryIndex, entity);

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new MovementData
                                {
                                    RotationSpeed = foxRotationSpeed,
                                    MoveSpeedBase = foxMoveSpeedBase,
                                    MoveMultiplier = foxMoveMultiplier,
                                    PregnancyMoveMultiplier = foxPregnancyMoveMultiplier,
                                    OriginalMoveMultiplier = foxOriginalMoveMultiplier,
                                    YoungMoveMultiplier = foxYoungMoveMultiplier,
                                    AdultMoveMultiplier = foxAdultMoveMultiplier,
                                    OldMoveMultiplier = foxOldMoveMultiplier
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new StateData
                                {
                                    FlagStateCurrent = foxFlagState,
                                    FlagStatePrevious = foxPreviousFlagState,
                                    DeathReason = foxDeathReason,
                                    BeenEaten = foxBeenEaten
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new TargetData
                                {
                                    AtTarget = true,
                                    Target = translation.Value,
                                    TargetOld = translation.Value,

                                    SightRadius = foxSightRadius,
                                    TouchRadius = foxTouchRadius,
                                    MateRadius = foxMateRadius
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new PathFollowData
                                {
                                    PathIndex = -1
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new BasicNeedsData
                                {
                                    Hunger = foxHunger,
                                    HungryThreshold = foxHungryThreshold,
                                    HungerMax = foxHungerMax,
                                    HungerIncrease = foxHungerIncrease,
                                    PregnancyHungerIncrease = foxPregnancyHungerIncrease,
                                    YoungHungerIncrease = foxYoungHungerIncrease,
                                    AdultHungerIncrease = foxAdultHungerIncrease,
                                    OldHungerIncrease = foxOldHungerIncrease,
                                    EatingSpeed = foxEatingSpeed,
                                    Diet = foxDiet,
                                    Thirst = foxThirst,
                                    ThirstyThreshold = foxThirstyThreshold,
                                    ThirstMax = foxThirstMax,
                                    ThirstIncrease = foxThirstIncrease,
                                    DrinkingSpeed = foxDrinkingSpeed,
                                }
                            );

                            float seed = timeSeed * (translation.Value.x * translation.Value.z) + entity.Index;
                            Random randomGen = new Random((uint)seed + 2);
                            BioStatsData.Genders randGender = randomGen.NextInt(0, 2) == 1
                                ? randGender = BioStatsData.Genders.Female
                                : randGender = BioStatsData.Genders.Male;

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new BioStatsData
                                {
                                    Age = foxAge,
                                    AgeIncrease = foxAgeIncrease,
                                    AgeMax = foxAgeMax,
                                    AgeGroup = foxAgeGroup,
                                    AdultEntryTimer = foxAdultEntryTimer,
                                    OldEntryTimer = foxOldEntryTimer,
                                    Gender = randGender
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new ReproductiveData
                                {
                                    MatingDuration = foxMatingDuration,
                                    MateStartTime = foxMateStartTime,
                                    ReproductiveUrge = foxReproductiveUrge,
                                    ReproductiveUrgeIncrease = (randGender == BioStatsData.Genders.Female
                                        ? foxReproductiveUrgeIncreaseFemale
                                        : foxReproductiveUrgeIncreaseMale),
                                    DefaultReproductiveIncrease = (randGender == BioStatsData.Genders.Female
                                        ? foxReproductiveUrgeIncreaseFemale
                                        : foxReproductiveUrgeIncreaseMale),
                                    MatingThreshold = foxMatingThreshold,

                                    BirthDuration = foxBirthDuration,
                                    BabiesBorn = foxBabiesBorn,
                                    BirthStartTime = foxBirthStartTime,
                                    CurrentLitterSize = foxCurrentLitterSize,
                                    LitterSizeMin = foxLitterSizeMin,
                                    LitterSizeMax = foxLitterSizeMax,
                                    LitterSizeAve = foxLitterSizeAve,
                                    PregnancyLengthBase = foxPregnancyLengthBase,
                                    PregnancyLengthModifier = foxPregnancyLengthModifier,
                                    PregnancyStartTime = foxPregnancyStartTime
                                }
                            );

                            ecb.SetComponent(entityInQueryIndex, newEntity,
                                new SizeData
                                {
                                    size = (randGender == BioStatsData.Genders.Female ? foxScaleFemale : foxScaleMale),
                                    SizeMultiplier = foxSizeMultiplier,
                                    AgeSizeMultiplier = foxAgeSizeMultiplier,
                                    YoungSizeMultiplier = foxYoungSizeMultiplier,
                                    AdultSizeMultiplier = foxAdultSizeMultiplier,
                                    OldSizeMultiplier = foxOldSizeMultiplier

                                }
                            );
                        }

                        #endregion

                        //Set the birthStartTime to the current age, so that it can count time towards the next baby
                        reproductiveData.BirthStartTime = bioStatsData.Age;
                        reproductiveData.BabiesBorn++;
                    }
                }
            }).ScheduleParallel();
            this.CompleteDependency();
        }
    }
}