using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class GivingBirthSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem ecbSystem;
    public static GivingBirthSystem Instance; // public reference to self (singleton)


    protected override void OnCreate()
    {
        base.OnCreate();
        Instance = this;
        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        float time = UnityEngine.Time.time;
        float timeSeed = time * System.DateTimeOffset.Now.Millisecond;

        #region Default Rabbit stats
        //Edible
        var rabbitNutritionalValue = RabbitDefaults.nutritionalValue;
        var rabbitCanBeEaten = RabbitDefaults.canBeEaten;
        var rabbitNutritionalValueMultiplier = RabbitDefaults.nutritionalValueMultiplier;
        EdibleData.FoodType rabbitFoodType = RabbitDefaults.foodType;

        //Movement
        var rabbitRotationSpeed = RabbitDefaults.rotationSpeed;
        var rabbitMoveSpeedBase = RabbitDefaults.moveSpeed;
        var rabbitMoveMultiplier = RabbitDefaults.moveMultiplier;
        var rabbitPregnancyMoveMultiplier = RabbitDefaults.pregnancyMoveMultiplier;
        var rabbitOriginalMoveMultiplier = RabbitDefaults.originalMoveMultiplier;
        var rabbitYoungMoveMultiplier = RabbitDefaults.youngMoveMultiplier;
        var rabbitAdultMoveMultiplier = RabbitDefaults.adultMoveMultiplier;
        var rabbitOldMoveMultiplier = RabbitDefaults.oldMoveMultiplier;

        //States
        StateData.FlagStates rabbitFlagState = RabbitDefaults.flagState;
        StateData.FlagStates rabbitPreviousFlagState = RabbitDefaults.previousFlagState;
        StateData.DeathReason rabbitDeathReason = RabbitDefaults.deathReason;
        var rabbitBeenEaten = RabbitDefaults.beenEaten;

        //Target data
        var rabbitSightRadius = RabbitDefaults.sightRadius;
        var rabbitTouchRadius = RabbitDefaults.touchRadius;
        var rabbitMateRadius = RabbitDefaults.mateRadius;

        //Basic needs data
        var rabbitHunger = RabbitDefaults.hunger;
        var rabbitHungryThreshold = RabbitDefaults.hungryThreshold;
        var rabbitHungerMax = RabbitDefaults.hungerMax;
        var rabbitHungerIncrease = RabbitDefaults.hungerIncrease;
        var rabbitPregnancyHungerIncrease = RabbitDefaults.pregnancyHungerIncrease;
        var rabbitYoungHungerIncrease = RabbitDefaults.youngHungerIncrease;
        var rabbitAdultHungerIncrease = RabbitDefaults.adultHungerIncrease;
        var rabbitOldHungerIncrease = RabbitDefaults.oldHungerIncrease;
        var rabbitEatingSpeed = RabbitDefaults.eatingSpeed;
        var rabbitEntityToEat = RabbitDefaults.entityToEat;
        var rabbitDiet = RabbitDefaults.diet;
        var rabbitThirst = RabbitDefaults.thirst;
        var rabbitThirstyThreshold = RabbitDefaults.thirstyThreshold;
        var rabbitThirstMax = RabbitDefaults.thirstMax;
        var rabbitThirstIncrease = RabbitDefaults.thirstIncrease;
        var rabbitDrinkingSpeed = RabbitDefaults.drinkingSpeed;
        var rabbitEntityToDrink = RabbitDefaults.entityToDrink;

        //Bio stats data
        var rabbitAge = RabbitDefaults.age;
        var rabbitAgeIncrease = RabbitDefaults.ageIncrease;
        var rabbitAgeMax = RabbitDefaults.ageMax;
        var rabbitAgeGroup = RabbitDefaults.ageGroup;
        var rabbitAdultEntryTimer = RabbitDefaults.adultEntryTimer;
        var rabbitOldEntryTimer = RabbitDefaults.oldEntryTimer;

        //Reproducite data
        var rabbitMatingDuration = RabbitDefaults.matingDuration;
        var rabbitMateStartTime = RabbitDefaults.mateStartTime;
        var rabbitReproductiveUrge = RabbitDefaults.reproductiveUrge;
        var rabbitMatingThreshold = RabbitDefaults.matingThreshold;
        var rabbitEntityToMate = RabbitDefaults.entityToMate;

        var rabbitBirthDuration = RabbitDefaults.birthDuration;
        var rabbitBabiesBorn = RabbitDefaults.babiesBorn;
        var rabbitBirthStartTime = RabbitDefaults.birthStartTime;
        var rabbitCurrentLitterSize = RabbitDefaults.currentLitterSize;
        var rabbitLitterSizeMin = RabbitDefaults.litterSizeMin;
        var rabbitLitterSizeMax = RabbitDefaults.litterSizeMax;
        var rabbitLitterSizeAve = RabbitDefaults.litterSizeAve;
        var rabbitPregnancyLengthBase = RabbitDefaults.pregnancyLength;
        var rabbitPregnancyLengthModifier = RabbitDefaults.pregnancyLengthModifier;
        var rabbitPregnancyStartTime = RabbitDefaults.pregnancyStartTime;

        var rabbitReproductiveUrgeIncreaseFemale = RabbitDefaults.reproductiveUrgeIncreaseFemale;
        var rabbitReproductiveUrgeIncreaseMale = RabbitDefaults.reproductiveUrgeIncreaseMale;

        //Size data
        var rabbitSizeMultiplier = RabbitDefaults.sizeMultiplier;
        var rabbitAgeSizeMultiplier = RabbitDefaults.ageSizeMultiplier;
        var rabbitYoungSizeMultiplier = RabbitDefaults.youngSizeMultiplier;
        var rabbitAdultSizeMultiplier = RabbitDefaults.adultSizeMultiplier;
        var rabbitOldSizeMultiplier = RabbitDefaults.oldSizeMultiplier;

        var rabbitScaleFemale = RabbitDefaults.scaleFemale;
        var rabbitScaleMale = RabbitDefaults.scaleMale;
        #endregion

        #region Default Fox stats
        //Edible
        var foxNutritionalValue = FoxDefaults.nutritionalValue;
        var foxCanBeEaten = FoxDefaults.canBeEaten;
        var foxNutritionalValueMultiplier = FoxDefaults.nutritionalValueMultiplier;
        EdibleData.FoodType foxFoodType = FoxDefaults.foodType;

        //Movement
        var foxRotationSpeed = FoxDefaults.rotationSpeed;
        var foxMoveSpeedBase = FoxDefaults.moveSpeed;
        var foxMoveMultiplier = FoxDefaults.moveMultiplier;
        var foxPregnancyMoveMultiplier = FoxDefaults.pregnancyMoveMultiplier;
        var foxOriginalMoveMultiplier = FoxDefaults.originalMoveMultiplier;
        var foxYoungMoveMultiplier = FoxDefaults.youngMoveMultiplier;
        var foxAdultMoveMultiplier = FoxDefaults.adultMoveMultiplier;
        var foxOldMoveMultiplier = FoxDefaults.oldMoveMultiplier;

        //States
        StateData.FlagStates foxFlagState = FoxDefaults.flagState;
        StateData.FlagStates foxPreviousFlagState = FoxDefaults.previousFlagState;
        StateData.DeathReason foxDeathReason = FoxDefaults.deathReason;
        var foxBeenEaten = FoxDefaults.beenEaten;

        //Target data
        var foxSightRadius = FoxDefaults.sightRadius;
        var foxTouchRadius = FoxDefaults.touchRadius;
        var foxMateRadius = FoxDefaults.mateRadius;

        //Basic needs data
        var foxHunger = FoxDefaults.hunger;
        var foxHungryThreshold = FoxDefaults.hungryThreshold;
        var foxHungerMax = FoxDefaults.hungerMax;
        var foxHungerIncrease = FoxDefaults.hungerIncrease;
        var foxPregnancyHungerIncrease = FoxDefaults.pregnancyHungerIncrease;
        var foxYoungHungerIncrease = FoxDefaults.youngHungerIncrease;
        var foxAdultHungerIncrease = FoxDefaults.adultHungerIncrease;
        var foxOldHungerIncrease = FoxDefaults.oldHungerIncrease;
        var foxEatingSpeed = FoxDefaults.eatingSpeed;
        var foxEntityToEat = FoxDefaults.entityToEat;
        var foxDiet = FoxDefaults.diet;
        var foxThirst = FoxDefaults.thirst;
        var foxThirstyThreshold = FoxDefaults.thirstyThreshold;
        var foxThirstMax = FoxDefaults.thirstMax;
        var foxThirstIncrease = FoxDefaults.thirstIncrease;
        var foxDrinkingSpeed = FoxDefaults.drinkingSpeed;
        var foxEntityToDrink = FoxDefaults.entityToDrink;

        //Bio stats data
        var foxAge = FoxDefaults.age;
        var foxAgeIncrease = FoxDefaults.ageIncrease;
        var foxAgeMax = FoxDefaults.ageMax;
        var foxAgeGroup = FoxDefaults.ageGroup;
        var foxAdultEntryTimer = FoxDefaults.adultEntryTimer;
        var foxOldEntryTimer = FoxDefaults.oldEntryTimer;

        //Reproductive data
        var foxMatingDuration = FoxDefaults.matingDuration;
        var foxMateStartTime = FoxDefaults.mateStartTime;
        var foxReproductiveUrge = FoxDefaults.reproductiveUrge;
        var foxMatingThreshold = FoxDefaults.matingThreshold;
        var foxEntityToMate = FoxDefaults.entityToMate;

        var foxBirthDuration = FoxDefaults.birthDuration;
        var foxBabiesBorn = FoxDefaults.babiesBorn;
        var foxBirthStartTime = FoxDefaults.birthStartTime;
        var foxCurrentLitterSize = FoxDefaults.currentLitterSize;
        var foxLitterSizeMin = FoxDefaults.litterSizeMin;
        var foxLitterSizeMax = FoxDefaults.litterSizeMax;
        var foxLitterSizeAve = FoxDefaults.litterSizeAve;
        var foxPregnancyLengthBase = FoxDefaults.pregnancyLength;
        var foxPregnancyLengthModifier = FoxDefaults.pregnancyLengthModifier;
        var foxPregnancyStartTime = FoxDefaults.pregnancyStartTime;

        var foxReproductiveUrgeIncreaseFemale = FoxDefaults.reproductiveUrgeIncreaseFemale;
        var foxReproductiveUrgeIncreaseMale = FoxDefaults.reproductiveUrgeIncreaseMale;

        //Size data
        var foxSizeMultiplier = FoxDefaults.sizeMultiplier;
        var foxAgeSizeMultiplier = FoxDefaults.ageSizeMultiplier;
        var foxYoungSizeMultiplier = FoxDefaults.youngSizeMultiplier;
        var foxAdultSizeMultiplier = FoxDefaults.adultSizeMultiplier;
        var foxOldSizeMultiplier = FoxDefaults.oldSizeMultiplier;

        var foxScaleFemale = FoxDefaults.scaleFemale;
        var foxScaleMale = FoxDefaults.scaleMale;
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
            if (stateData.isGivingBirth)
            {
                if ((bioStatsData.age - reproductiveData.birthStartTime >= reproductiveData.birthDuration) &&
                reproductiveData.babiesBorn < reproductiveData.currentLitterSize)
                {
                    ////give birth
                    #region Setting New Entity's Components
                    // determine if rabbit or fox giving birth - not very scalable
                    if (HasComponent<IsRabbitTag>(entity))
                    {
                        Entity newEntity = ecb.Instantiate(entityInQueryIndex, entity);

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new MovementData
                            {
                                rotationSpeed = rabbitRotationSpeed,
                                moveSpeedBase = rabbitMoveSpeedBase,
                                moveMultiplier = rabbitMoveMultiplier,
                                pregnancyMoveMultiplier = rabbitPregnancyMoveMultiplier,
                                originalMoveMultiplier = rabbitOriginalMoveMultiplier,
                                youngMoveMultiplier = rabbitYoungMoveMultiplier,
                                adultMoveMultiplier = rabbitAdultMoveMultiplier,
                                oldMoveMultiplier = rabbitOldMoveMultiplier
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new StateData
                            {
                                flagState = rabbitFlagState,
                                previousFlagState = rabbitPreviousFlagState,
                                deathReason = rabbitDeathReason,
                                beenEaten = rabbitBeenEaten
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new TargetData
                            {
                                atTarget = true,
                                currentTarget = translation.Value,
                                oldTarget = translation.Value,

                                sightRadius = rabbitSightRadius,
                                touchRadius = rabbitTouchRadius,
                                mateRadius = rabbitMateRadius
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new PathFollowData
                            {
                                pathIndex = -1
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new BasicNeedsData
                            {
                                hunger = rabbitHunger,
                                hungryThreshold = rabbitHungryThreshold,
                                hungerMax = rabbitHungerMax,
                                hungerIncrease = rabbitHungerIncrease,
                                pregnancyHungerIncrease = rabbitPregnancyHungerIncrease,
                                youngHungerIncrease = rabbitYoungHungerIncrease,
                                adultHungerIncrease = rabbitAdultHungerIncrease,
                                oldHungerIncrease = rabbitOldHungerIncrease,
                                eatingSpeed = rabbitEatingSpeed,
                                diet = rabbitDiet,
                                thirst = rabbitThirst,
                                thirstyThreshold = rabbitThirstyThreshold,
                                thirstMax = rabbitThirstMax,
                                thirstIncrease = rabbitThirstIncrease,
                                drinkingSpeed = rabbitDrinkingSpeed,
                            }
                        );

                        float seed = timeSeed * (translation.Value.x * translation.Value.z) + entity.Index;
                        Random randomGen = new Random((uint)seed + 2);
                        BioStatsData.Gender randGender = randomGen.NextInt(0, 2) == 1 ? randGender = BioStatsData.Gender.Female : randGender = BioStatsData.Gender.Male;

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new BioStatsData
                            {
                                age = rabbitAge,
                                ageIncrease = rabbitAgeIncrease,
                                ageMax = rabbitAgeMax,
                                ageGroup = rabbitAgeGroup,
                                adultEntryTimer = rabbitAdultEntryTimer,
                                oldEntryTimer = rabbitOldEntryTimer,
                                gender = randGender
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new ReproductiveData
                            {
                                matingDuration = rabbitMatingDuration,
                                mateStartTime = rabbitMateStartTime,
                                reproductiveUrge = rabbitReproductiveUrge,
                                reproductiveUrgeIncrease = (randGender == BioStatsData.Gender.Female ? rabbitReproductiveUrgeIncreaseFemale : rabbitReproductiveUrgeIncreaseMale),
                                defaultRepoductiveIncrease = (randGender == BioStatsData.Gender.Female ? rabbitReproductiveUrgeIncreaseFemale : rabbitReproductiveUrgeIncreaseMale),
                                matingThreshold = rabbitMatingThreshold,

                                birthDuration = rabbitBirthDuration,
                                babiesBorn = rabbitBabiesBorn,
                                birthStartTime = rabbitBirthStartTime,
                                currentLitterSize = rabbitCurrentLitterSize,
                                litterSizeMin = rabbitLitterSizeMin,
                                litterSizeMax = rabbitLitterSizeMax,
                                litterSizeAve = rabbitLitterSizeAve,
                                pregnancyLengthBase = rabbitPregnancyLengthBase,
                                pregnancyLengthModifier = rabbitPregnancyLengthModifier,
                                pregnancyStartTime = rabbitPregnancyStartTime
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new SizeData
                            {
                                size = (randGender == BioStatsData.Gender.Female ? rabbitScaleFemale : rabbitScaleMale),
                                sizeMultiplier = rabbitSizeMultiplier,
                                ageSizeMultiplier = rabbitAgeSizeMultiplier,
                                youngSizeMultiplier = rabbitYoungSizeMultiplier,
                                adultSizeMultiplier = rabbitAdultSizeMultiplier,
                                oldSizeMultiplier = rabbitOldSizeMultiplier

                            }
                        );
                    }
                    else if (HasComponent<IsFoxTag>(entity))
                    {
                        Entity newEntity = ecb.Instantiate(entityInQueryIndex, entity);

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new MovementData
                            {
                                rotationSpeed = foxRotationSpeed,
                                moveSpeedBase = foxMoveSpeedBase,
                                moveMultiplier = foxMoveMultiplier,
                                pregnancyMoveMultiplier = foxPregnancyMoveMultiplier,
                                originalMoveMultiplier = foxOriginalMoveMultiplier,
                                youngMoveMultiplier = foxYoungMoveMultiplier,
                                adultMoveMultiplier = foxAdultMoveMultiplier,
                                oldMoveMultiplier = foxOldMoveMultiplier
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new StateData
                            {
                                flagState = foxFlagState,
                                previousFlagState = foxPreviousFlagState,
                                deathReason = foxDeathReason,
                                beenEaten = foxBeenEaten
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new TargetData
                            {
                                atTarget = true,
                                currentTarget = translation.Value,
                                oldTarget = translation.Value,

                                sightRadius = foxSightRadius,
                                touchRadius = foxTouchRadius,
                                mateRadius = foxMateRadius
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new PathFollowData
                            {
                                pathIndex = -1
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new BasicNeedsData
                            {
                                hunger = foxHunger,
                                hungryThreshold = foxHungryThreshold,
                                hungerMax = foxHungerMax,
                                hungerIncrease = foxHungerIncrease,
                                pregnancyHungerIncrease = foxPregnancyHungerIncrease,
                                youngHungerIncrease = foxYoungHungerIncrease,
                                adultHungerIncrease = foxAdultHungerIncrease,
                                oldHungerIncrease = foxOldHungerIncrease,
                                eatingSpeed = foxEatingSpeed,
                                diet = foxDiet,
                                thirst = foxThirst,
                                thirstyThreshold = foxThirstyThreshold,
                                thirstMax = foxThirstMax,
                                thirstIncrease = foxThirstIncrease,
                                drinkingSpeed = foxDrinkingSpeed,
                            }
                        );

                        float seed = timeSeed * (translation.Value.x * translation.Value.z) + entity.Index;
                        Random randomGen = new Random((uint)seed + 2);
                        BioStatsData.Gender randGender = randomGen.NextInt(0, 2) == 1 ? randGender = BioStatsData.Gender.Female : randGender = BioStatsData.Gender.Male;

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new BioStatsData
                            {
                                age = foxAge,
                                ageIncrease = foxAgeIncrease,
                                ageMax = foxAgeMax,
                                ageGroup = foxAgeGroup,
                                adultEntryTimer = foxAdultEntryTimer,
                                oldEntryTimer = foxOldEntryTimer,
                                gender = randGender
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new ReproductiveData
                            {
                                matingDuration = foxMatingDuration,
                                mateStartTime = foxMateStartTime,
                                reproductiveUrge = foxReproductiveUrge,
                                reproductiveUrgeIncrease = (randGender == BioStatsData.Gender.Female ? foxReproductiveUrgeIncreaseFemale : foxReproductiveUrgeIncreaseMale),
                                defaultRepoductiveIncrease = (randGender == BioStatsData.Gender.Female ? foxReproductiveUrgeIncreaseFemale : foxReproductiveUrgeIncreaseMale),
                                matingThreshold = foxMatingThreshold,

                                birthDuration = foxBirthDuration,
                                babiesBorn = foxBabiesBorn,
                                birthStartTime = foxBirthStartTime,
                                currentLitterSize = foxCurrentLitterSize,
                                litterSizeMin = foxLitterSizeMin,
                                litterSizeMax = foxLitterSizeMax,
                                litterSizeAve = foxLitterSizeAve,
                                pregnancyLengthBase = foxPregnancyLengthBase,
                                pregnancyLengthModifier = foxPregnancyLengthModifier,
                                pregnancyStartTime = foxPregnancyStartTime
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new SizeData
                            {
                                size = (randGender == BioStatsData.Gender.Female ? foxScaleFemale : foxScaleMale),
                                sizeMultiplier = foxSizeMultiplier,
                                ageSizeMultiplier = foxAgeSizeMultiplier,
                                youngSizeMultiplier = foxYoungSizeMultiplier,
                                adultSizeMultiplier = foxAdultSizeMultiplier,
                                oldSizeMultiplier = foxOldSizeMultiplier

                            }
                        );
                    }
                    #endregion

                    reproductiveData.birthStartTime = bioStatsData.age;
                    reproductiveData.babiesBorn++;
                }
            }
        }).ScheduleParallel();
        this.CompleteDependency();
    }
}
