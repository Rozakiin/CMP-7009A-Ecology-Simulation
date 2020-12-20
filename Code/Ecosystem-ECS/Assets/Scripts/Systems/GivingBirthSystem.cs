using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class GivingBirthSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {

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
            
                if(stateData.state == StateData.States.GivingBirth)
                {
                    if(bioStatsData.age - reproductiveData.birthStartTime >= reproductiveData.birthDuration &&
                    reproductiveData.babiesBorn < reproductiveData.currentLitterSize)
                    {
                        ////give birth
                        //Entity newEntity = e;
                        //EntityManager.Instantiate(e);

                        
                        //ArchetypeChunkEntityType archetype =  this.GetArchetypeChunkEntityType();
                        Entity newEntity = ecb.Instantiate(entityInQueryIndex, e);
                        ecb.SetComponent(entityInQueryIndex, newEntity, 
                            new StateData
                            {
                                state = RabbitDefaults.state,
                                previousState = RabbitDefaults.previousState
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new TargetData
                            {
                                atTarget = true,
                                currentTarget = translation.Value,
                                oldTarget = translation.Value
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
                                hunger = RabbitDefaults.hunger,
                                hungerIncrease = RabbitDefaults.hungerIncrease,
                                entityToEat = RabbitDefaults.entityToEat,
                                diet = RabbitDefaults.diet,
                                thirst = RabbitDefaults.thirst,
                                thirstIncrease = RabbitDefaults.thirstIncrease,
                                drinkingSpeed = RabbitDefaults.drinkingSpeed,
                                entityToDrink = RabbitDefaults.entityToDrink
                            }
                        );

                        BioStatsData.Gender randGender = UnityEngine.Random.Range(0, 2) == 1 ? randGender = BioStatsData.Gender.Female : randGender = BioStatsData.Gender.Male;

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new BioStatsData
                            {
                                age = RabbitDefaults.age,
                                ageGroup = RabbitDefaults.ageGroup,
                                gender = randGender
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new ReproductiveData
                            {
                                mateStartTime = RabbitDefaults.mateStartTime,
                                reproductiveUrge = RabbitDefaults.reproductiveUrge,
                                reproductiveUrgeIncrease = (randGender == BioStatsData.Gender.Female ? RabbitDefaults.reproductiveUrgeIncreaseFemale : RabbitDefaults.reproductiveUrgeIncreaseMale),
                                defaultRepoductiveIncrease = (randGender == BioStatsData.Gender.Female ? RabbitDefaults.reproductiveUrgeIncreaseFemale : RabbitDefaults.reproductiveUrgeIncreaseMale),
                                entityToMate = RabbitDefaults.entityToMate,

                                pregnant = RabbitDefaults.pregnant,
                                babiesBorn = RabbitDefaults.babiesBorn,
                                birthStartTime = RabbitDefaults.birthStartTime,
                                currentLitterSize = RabbitDefaults.currentLitterSize,
                                pregnancyStartTime = RabbitDefaults.pregnancyStartTime
                            }
                        );

                        ecb.SetComponent(entityInQueryIndex, newEntity,
                            new SizeData
                            {
                                size = (randGender == BioStatsData.Gender.Female ? RabbitDefaults.scaleFemale : RabbitDefaults.scaleMale),
                                sizeMultiplier = RabbitDefaults.sizeMultiplier,
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
                }
        }).Schedule();
    }
}
