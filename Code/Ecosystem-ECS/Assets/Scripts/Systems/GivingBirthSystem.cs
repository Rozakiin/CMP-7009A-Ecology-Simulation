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
        var rabbitAtTarget = true;
        var rabbitSightRadius = RabbitDefaults.sightRadius;
        var rabbitTouchRadius = RabbitDefaults.touchRadius;
        var rabbitMateRadius = RabbitDefaults.mateRadius;

        //Path follow data
        var rabbitPathIndex = -1;

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

        var rabbitPregnant = RabbitDefaults.pregnant;
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

        NativeArray<int> rabbitsBirthed = new NativeArray<int>(1, Allocator.TempJob);
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
                    Entity newEntity = ecb.Instantiate(entityInQueryIndex, entity);

                    #region Setting New Entity's Components
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

                            pregnant = rabbitPregnant,
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
                    #endregion

                    reproductiveData.birthStartTime = bioStatsData.age;
                    reproductiveData.babiesBorn++;
                    rabbitsBirthed[0]++;
                }

                if (reproductiveData.babiesBorn >= reproductiveData.currentLitterSize)
                {
                    //reproductiveData.babiesBorn = 0;
                    reproductiveData.pregnant = false;
                }
            }
        }).ScheduleParallel();
        this.CompleteDependency();
        SimulationManager.Instance.rabbitPopulation += rabbitsBirthed[0];
        rabbitsBirthed.Dispose();
    }
}
