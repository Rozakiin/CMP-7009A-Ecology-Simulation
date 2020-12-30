using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;


public class StateSystem : SystemBase
{

    protected override void OnUpdate()
    {
        float tileSize = SimulationManager.tileSize;

        Entities.ForEach((
            ref StateData stateData,
            ref BasicNeedsData basicNeedsData,
            ref MovementData movementData,
            in ReproductiveData reproductiveData,
            in BioStatsData bioStatsData,
            in TargetData targetData,
            in Translation translation
            )=> {

                //Priorities: Eaten>Thirst>Hunger>Age
                if (targetData.predatorEntity != Entity.Null)
                {
                    stateData.previousState = stateData.state;
                    stateData.state = StateData.States.Fleeing;
                }
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
                        if (reproductiveData.reproductiveUrge >= reproductiveData.matingThreshold)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.SexuallyActive;
                        }

                        if (targetData.entityToEat != Entity.Null)
                        {
                            //float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToEat].Value);
                            if (targetData.shortestToEdibleDistance <= targetData.touchRadius)
                            {
                                stateData.previousState = stateData.state;
                                stateData.state = StateData.States.Eating;
                            }
                        }
                        break;
                    case StateData.States.Eating:
                        if (targetData.entityToEat == Entity.Null)
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
                            //float euclidian = math.distance(translation.Value, targetData.currentTarget);
                            //some very rough estimation of distance in corner of tile
                            if (targetData.shortestToWaterDistance <= targetData.touchRadius+tileSize/1.4+1)
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
                        if (targetData.entityToMate != Entity.Null)
                        {
                            //float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToMate].Value);
                            if (targetData.shortestToMateDistance <= targetData.touchRadius)
                            {
                                stateData.previousState = stateData.state;
                                stateData.state = StateData.States.Mating;
                            }
                        }
                        break;
                    case StateData.States.Mating:
                        if (targetData.entityToMate == Entity.Null)
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
                    // this is ok , I think? or back to previousState?
                        if (targetData.predatorEntity == Entity.Null)
                        {
                            stateData.previousState = stateData.state;
                            stateData.state = StateData.States.Wandering;
                        }
                        break;
                    case StateData.States.Dead:
                        break;
                    default:
                        break;
                }
            }).ScheduleParallel();

    }
}
