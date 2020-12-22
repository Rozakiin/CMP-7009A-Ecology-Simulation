﻿using System.Diagnostics;
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

                //Update pregnancy status
                if(reproductiveData.pregnant)
                {
                    if(bioStatsData.age - reproductiveData.pregnancyStartTime >= reproductiveData.PregnancyLength)
                    {
                        stateData.previousState = stateData.state;
                        stateData.state = StateData.States.GivingBirth;
                    }
                }



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
                            float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[reproductiveData.entityToMate].Value);
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
            }).ScheduleParallel();
        // Make sure that the ECB system knows about our job
        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
