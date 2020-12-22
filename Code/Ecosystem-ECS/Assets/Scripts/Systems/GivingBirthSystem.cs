using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

public class GivingBirthSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
    public static GivingBirthSystem Instance; // public reference to self (singleton)


    protected override void OnCreate()
    {
        base.OnCreate();
        Instance = this;
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {

        float time = UnityEngine.Time.time;
        float timeSeed = time * System.DateTimeOffset.Now.Millisecond;

        #region Default Rabbit stats
        //Edible
        float rabbitNutritionalValue = RabbitDefaults.nutritionalValue;
        bool rabbitCanBeEaten = RabbitDefaults.canBeEaten;
        float rabbitNutritionalValueMultiplier = RabbitDefaults.nutritionalValueMultiplier;
        EdibleData.FoodType rabbitFoodType = RabbitDefaults.foodType;

        //Movement
        float rabbitRotationSpeed = RabbitDefaults.rotationSpeed;
        float rabbitMoveSpeedBase = RabbitDefaults.moveSpeed;
        float rabbitMoveMultiplier = RabbitDefaults.moveMultiplier;
        float rabbitPregnancyMoveMultiplier = RabbitDefaults.pregnancyMoveMultiplier;
        float rabbitOriginalMoveMultiplier = RabbitDefaults.originalMoveMultiplier;
        float rabbitYoungMoveMultiplier = RabbitDefaults.youngMoveMultiplier;
        float rabbitAdultMoveMultiplier = RabbitDefaults.adultMoveMultiplier;
        float rabbitOldMoveMultiplier = RabbitDefaults.oldMoveMultiplier;

        //States
        StateData.States rabbitState = RabbitDefaults.state;
        StateData.States rabbitPreviousState = RabbitDefaults.previousState;
        StateData.DeathReason rabbitDeathReason = RabbitDefaults.deathReason;
        bool rabbitBeenEaten = RabbitDefaults.beenEaten;

        //Target data
        bool rabbitAtTarget = true;

        float rabbitSightRadius = RabbitDefaults.sightRadius;
        float rabbitTouchRadius = RabbitDefaults.touchRadius;

        //Path follow data
        int rabbitPathIndex = -1;

        //Basic needs data
        float rabbitHunger = RabbitDefaults.hunger;
        float rabbitHungryThreshold = RabbitDefaults.hungryThreshold;
        float rabbitHungerMax = RabbitDefaults.hungerMax;
        float rabbitHungerIncrease = RabbitDefaults.hungerIncrease;
        float rabbitPregnancyHungerIncrease = RabbitDefaults.pregnancyHungerIncrease;
        float rabbitYoungHungerIncrease = RabbitDefaults.youngHungerIncrease;
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

        var rabbitReproductiveUrgeIncreaseFemale =  RabbitDefaults.reproductiveUrgeIncreaseFemale;
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

        var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();
        Entities.ForEach((
            Entity e,
            int entityInQueryIndex,
            ref ReproductiveData reproductiveData,
            ref StateData stateData,
            ref MovementData movementData,
            in BioStatsData bioStatsData,
            in Translation translation
            ) => {
            
                //If you want to test uncomment this and comment out the if below
                //if(bioStatsData.age > 10 && bioStatsData.age < 12)
                //{
                    if(bioStatsData.age - reproductiveData.birthStartTime >= reproductiveData.birthDuration &&
                    reproductiveData.babiesBorn < reproductiveData.currentLitterSize)
                    {
                        ////give birth
                        //Entity newEntity = e;
                        //EntityManager.Instantiate(e);

                        
                        //ArchetypeChunkEntityType archetype =  this.GetArchetypeChunkEntityType();
                        Entity newEntity = ecb.Instantiate(entityInQueryIndex, e);

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
                                state = rabbitState,
                                previousState = rabbitPreviousState,
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
                                touchRadius = rabbitTouchRadius
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
                                entityToEat = rabbitEntityToEat,
                                diet = rabbitDiet,
                                thirst = rabbitThirst,
                                thirstyThreshold = rabbitThirstyThreshold,
                                thirstMax = rabbitThirstMax,
                                thirstIncrease = rabbitThirstIncrease,
                                drinkingSpeed = rabbitDrinkingSpeed,
                                entityToDrink = rabbitEntityToDrink
                            }
                        );

                        float seed = timeSeed * (translation.Value.x * translation.Value.z) + e.Index;
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
                                entityToMate = rabbitEntityToMate,

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

                        reproductiveData.birthStartTime = bioStatsData.age;
                        reproductiveData.babiesBorn++;
                    }
                    if(reproductiveData.babiesBorn >= reproductiveData.currentLitterSize)
                    {
                        reproductiveData.pregnant = false;
                        movementData.moveMultiplier = movementData.originalMoveMultiplier;
                        stateData.state = StateData.States.Wandering;
                    }
                //}
        }).ScheduleParallel();

        this.CompleteDependency();
    }
}
