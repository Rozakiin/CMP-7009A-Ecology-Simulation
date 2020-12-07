using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(SimulationSystemGroup))]//Not sure if working
public class StateSystem : SystemBase
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
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     float deltaTime = Time.DeltaTime;

        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.

        // Acquire an ECB and convert it to a concurrent one to be able
        // to use it from a parallel job.
        //var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();

        Entities.ForEach((
            ref StateData stateData,
            ref PregnancyData pregnancyData,
            ref BasicNeedsData basicNeedsData,
            ref MovementData movementData,
            in BioStatsData bioStatsData,
            //in AgeData ageData,
            //in HungerData hungerData,
            //in ThirstData thirstData,
            in MateData mateData,
            //in GenderData genderData,
            in VisionData visionData,
            in Translation translation
            
            
            //ref MovementData movementData
            )=> {
                // Implement the work to perform for each entity here.
                // You should only access data that is local or that is a
                // field on this job. Note that the 'rotation' parameter is
                // marked as 'in', which means it cannot be modified,
                // but allows this job to run in parallel with other jobs
                // that want to read Rotation component data.
                // For example,
                //     translation.Value += math.mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;

                //Priorities: Eaten>Thirst>Hunger>Age
                if (stateData.beenEaten)
                {
                    stateData.previousState = stateData.state;
                    stateData.state = StateData.States.Dead;
                    stateData.deathReason = StateData.DeathReason.Eaten;
                }
                else if (basicNeedsData.thirst >= basicNeedsData.thirstMax)
                {
                    stateData.previousState = stateData.state;
                    stateData.state = StateData.States.Dead;
                    stateData.deathReason = StateData.DeathReason.Thirst;
                }
                else if (basicNeedsData.hunger >= basicNeedsData.hungerMax)
                {
                    stateData.previousState = stateData.state;
                    stateData.state = StateData.States.Dead;
                    stateData.deathReason = StateData.DeathReason.Hunger;
                }
                else if (bioStatsData.age >= bioStatsData.ageMax)
                {
                    stateData.previousState = stateData.state;
                    stateData.state = StateData.States.Dead;
                    stateData.deathReason = StateData.DeathReason.Age;
                }

                if(pregnancyData.pregnant)
                {
                    basicNeedsData.hungerIncrease = basicNeedsData.pregnancyHungerIncrease;
                    movementData.moveMultiplier = movementData.pregnancyMoveMultiplier;
                    if(bioStatsData.age - pregnancyData.pregnancyStartTime >= pregnancyData.PregnancyLength)
                    {
                        stateData.previousState = stateData.state;
                        stateData.state = StateData.States.GivingBirth;
                        pregnancyData.birthStartTime = bioStatsData.age;
                        pregnancyData.pregnant = false;
                    }
                }

                if (!pregnancyData.pregnant)
                {
                    if(bioStatsData.ageGroup == BioStatsData.AgeGroup.Young)
                    {
                        basicNeedsData.hungerIncrease = basicNeedsData.youngHungerIncrease;
                        movementData.moveMultiplier = movementData.youngMoveMultiplier;
                    }
                    else if(bioStatsData.ageGroup == BioStatsData.AgeGroup.Adult)
                    {
                        basicNeedsData.hungerIncrease = basicNeedsData.adultHungerIncrease;
                        movementData.moveMultiplier = movementData.adultMoveMultiplier;
                    }
                    else if(bioStatsData.ageGroup == BioStatsData.AgeGroup.Old)
                    {
                        basicNeedsData.hungerIncrease = basicNeedsData.oldHungerIncrease;
                        movementData.moveMultiplier = movementData.oldMoveMultiplier;
                    }
                }

                if(bioStatsData.gender == BioStatsData.Gender.Male)
                {

                }

                //Priorities: Mating>Drinking>Eating>Wandering
                switch (stateData.state)
                {
                    case StateData.States.Wandering:
                        if (mateData.reproductiveUrge >= mateData.matingThreshold)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.Hungry;
                        }
                        else if (basicNeedsData.thirst >= basicNeedsData.thirstyThreshold)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.Thirsty;
                        }
                        else if (basicNeedsData.hunger >= basicNeedsData.hungryThreshold)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.Hungry;
                        }

                        break;
                    case StateData.States.Hungry:
                        if (mateData.reproductiveUrge >= mateData.matingThreshold)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.SexuallyActive;
                        }

                        if (basicNeedsData.entityToEat != Entity.Null)
                        {
                            float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[basicNeedsData.entityToEat].Value);
                            if (euclidian <= visionData.touchRadius)
                            {
                                stateData.previousState = stateData.state;
                                stateData.state = StateData.States.Eating;
                            }
                        }
                        break;
                    case StateData.States.Eating:
                        if (basicNeedsData.entityToEat == Entity.Null)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.Hungry;
                        }
                        if (basicNeedsData.hunger <= 0 )
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.Wandering;
                        }
                        break;
                    case StateData.States.Thirsty:
                        if (mateData.reproductiveUrge >= mateData.matingThreshold)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.SexuallyActive;
                        }
                        if (basicNeedsData.entityToDrink != Entity.Null)
                        {
                            float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[basicNeedsData.entityToDrink].Value);
                            if (euclidian <= visionData.touchRadius)
                            {
                                stateData.previousState = stateData.state;
                                stateData.state = StateData.States.Drinking;
                            }
                        }
                        break;
                    case StateData.States.Drinking:
                        if (basicNeedsData.entityToDrink == Entity.Null)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.Thirsty;
                        }
                        if (basicNeedsData.thirst <= 0)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.Wandering;
                        }
                        break;
                    case StateData.States.SexuallyActive:
                        if (mateData.entityToMate != Entity.Null)
                        {
                            float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[mateData.entityToMate].Value);
                            if (euclidian <= visionData.touchRadius)
                            {
                                stateData.previousState = stateData.state;
                                stateData.state = StateData.States.Mating;
                            }
                        }
                        break;
                    case StateData.States.Mating:
                        if (mateData.entityToMate == Entity.Null)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.SexuallyActive;
                        }
                        if (mateData.reproductiveUrge <= 0)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.Wandering;
                        }
                        // TODO Pregnancy, mating and giving birth states
                        break;
                    case StateData.States.Pregnant:
                        break;
                    case StateData.States.GivingBirth:
                        break;
                    case StateData.States.Fleeing:
                        break;
                    case StateData.States.Dead:
                        //entitycommandbuffer
                        //DestroyEntity(entity);
                        break;
                    default:
                        break;
                }
            }).ScheduleParallel();
        // Make sure that the ECB system knows about our job
        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
