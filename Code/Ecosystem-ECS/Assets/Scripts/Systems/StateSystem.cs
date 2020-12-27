using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;


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

        // Acquire an ECB and convert it to a concurrent one to be able
        // to use it from a parallel job.

        Entities.ForEach((
            ref StateData stateData,
            ref BasicNeedsData basicNeedsData,
            ref MovementData movementData,
            ref TargetData targetData,
            ref BioStatsData bioStatsData,
            ref ReproductiveData reproductiveData,
            in Translation translation
            )=> {

                bool isHungry = ((stateData.flagState & StateData.FlagStates.Hungry) == StateData.FlagStates.Hungry);
                bool isThirsty = ((stateData.flagState & StateData.FlagStates.Thirsty) == StateData.FlagStates.Thirsty);
                bool isEating = ((stateData.flagState & StateData.FlagStates.Eating) == StateData.FlagStates.Eating);
                bool isDrinking = ((stateData.flagState & StateData.FlagStates.Drinking) == StateData.FlagStates.Drinking);
                bool isSexuallyActive = ((stateData.flagState & StateData.FlagStates.SexuallyActive) == StateData.FlagStates.SexuallyActive);
                bool isMating = ((stateData.flagState & StateData.FlagStates.Mating) == StateData.FlagStates.Mating);
                bool isFleeing = ((stateData.flagState & StateData.FlagStates.Fleeing) == StateData.FlagStates.Fleeing);
                bool isDead = ((stateData.flagState & StateData.FlagStates.Dead) == StateData.FlagStates.Dead);
                bool isPregnant = ((stateData.flagState & StateData.FlagStates.Pregnant) == StateData.FlagStates.Pregnant);
                bool isGivingBirth = ((stateData.flagState & StateData.FlagStates.GivingBirth) == StateData.FlagStates.GivingBirth);

                //Priorities: Eaten>Thirst>Hunger>Age
                /*old system
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
                */

                //New state system
                if (stateData.beenEaten)
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState = StateData.FlagStates.Dead;
                    stateData.deathReason = StateData.DeathReason.Eaten;
                }
                else if (basicNeedsData.thirst >= basicNeedsData.thirstMax)
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState = StateData.FlagStates.Dead;
                    stateData.deathReason = StateData.DeathReason.Thirst;
                }
                else if (basicNeedsData.hunger >= basicNeedsData.hungerMax)
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState = StateData.FlagStates.Dead;
                    stateData.deathReason = StateData.DeathReason.Hunger;
                }
                else if (bioStatsData.age >= bioStatsData.ageMax)
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState = StateData.FlagStates.Dead;
                    stateData.deathReason = StateData.DeathReason.Age;
                }

                //Update pregnancy status
                /*old system
                if (reproductiveData.pregnant)
                {
                    if(bioStatsData.age - reproductiveData.pregnancyStartTime >= reproductiveData.PregnancyLength)
                    {
                        stateData.previousState = stateData.state;
                        stateData.state = StateData.States.GivingBirth;
                    }
                }
                */

                //new system
                if (isPregnant)
                {
                    if (bioStatsData.age - reproductiveData.pregnancyStartTime >= reproductiveData.PregnancyLength)
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState = StateData.FlagStates.GivingBirth;
                    }
                }

                if (!isGivingBirth && !isEating && !isDrinking && !isMating)
                {
                    if(reproductiveData.reproductiveUrge >= reproductiveData.matingThreshold &&
                    !isSexuallyActive)
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState ^= StateData.FlagStates.SexuallyActive;
                    }
                    if(basicNeedsData.thirst >= basicNeedsData.thirstyThreshold &&
                    !isThirsty)
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState ^= StateData.FlagStates.Thirsty;
                    }
                    if(basicNeedsData.hunger >= basicNeedsData.hungryThreshold &&
                    !isHungry)
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState ^= StateData.FlagStates.Hungry;
                    }

                    if (isHungry)
                    {
                        if (HasComponent<Translation>(targetData.entityToEat))
                        {
                            if (targetData.shortestToEdibleDistance <= targetData.touchRadius)
                            {
                                stateData.previousFlagState = stateData.flagState;
                                stateData.flagState = StateData.FlagStates.Eating;
                            }
                        }
                    }

                    if (isThirsty)
                    {
                        if (HasComponent<Translation>(targetData.entityToDrink))
                        {
                            if (targetData.shortestToWaterDistance <= targetData.touchRadius)
                            {
                                stateData.previousFlagState = stateData.flagState;
                                stateData.flagState = StateData.FlagStates.Drinking;
                            }
                        }
                    }

                    if (isSexuallyActive)
                    {
                        if (HasComponent<Translation>(targetData.entityToMate))
                        {
                            if (targetData.shortestToMateDistance <= targetData.touchRadius)
                            {
                                stateData.previousFlagState = stateData.flagState;
                                stateData.flagState = StateData.FlagStates.Mating;
                                reproductiveData.mateStartTime = bioStatsData.age;
                            }
                        }
                    }               
                }

                //This 3 if's will probably have to be modified to accommodate for Will's new targetting system
                //Instead of checking for the distance to the entity, it might be check what is its intended target
                //At the moment?
                if (isEating)
                {
                    if (!HasComponent<Translation>(targetData.entityToEat))
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState = StateData.FlagStates.Hungry;
                    }
                    if (basicNeedsData.hunger <= 0)
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState = StateData.FlagStates.Wandering;
                    }
                }

                if (isDrinking)
                {
                    if (!HasComponent<Translation>(targetData.entityToDrink))
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState = StateData.FlagStates.Thirsty;
                    }
                    if (basicNeedsData.thirst <= 0)
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState = StateData.FlagStates.Wandering;
                    }
                }

                if (isMating)
                {
                    if (!HasComponent<Translation>(targetData.entityToMate))
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState = StateData.FlagStates.SexuallyActive;
                    }
                    if (reproductiveData.reproductiveUrge <= 0)
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState = StateData.FlagStates.Wandering;
                    }
                }

                /*old system
                //Priorities: Mating>Drinking>Eating>Wandering
                switch (stateData.state)
                {
                    case StateData.States.Wandering:
                        if (reproductiveData.reproductiveUrge >= reproductiveData.matingThreshold)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.SexuallyActive;
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
                        if (reproductiveData.reproductiveUrge >= reproductiveData.matingThreshold)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.SexuallyActive;
                        }

                        if (HasComponent<Translation>(targetData.entityToEat))
                        {
                            float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[basicNeedsData.entityToEat].Value);
                            if (euclidian <= targetData.touchRadius)
                            {
                                stateData.previousState = stateData.state;
                                stateData.state = StateData.States.Eating;
                            }
                        }
                        break;
                    case StateData.States.Eating:
                        if (!HasComponent<Translation>(targetData.entityToEat))
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
                        if (reproductiveData.reproductiveUrge >= reproductiveData.matingThreshold)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.SexuallyActive;
                        }
                        if (HasComponent<Translation>(targetData.entityToDrink))
                        {
                            float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[basicNeedsData.entityToDrink].Value);
                            if (euclidian <= targetData.touchRadius)
                            {
                                stateData.previousState = stateData.state;
                                stateData.state = StateData.States.Drinking;
                            }
                        }
                        break;
                    case StateData.States.Drinking:
                        if (!HasComponent<Translation>(targetData.entityToDrink))
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
                        if (HasComponent<Translation>(targetData.entityToMate))
                        {
                            float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToMate].Value);
                            if (euclidian <= targetData.touchRadius)
                            {
                                stateData.previousState = stateData.state;
                                stateData.state = StateData.States.Mating;
                                reproductiveData.mateStartTime = bioStatsData.age;
                            }
                        }
                        break;
                    case StateData.States.Mating:
                        if (!HasComponent<Translation>(targetData.entityToMate))
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.SexuallyActive;
                        }
                        if (reproductiveData.reproductiveUrge <= 0)
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
                */
            }).ScheduleParallel();
        // Make sure that the ECB system knows about our job
        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
