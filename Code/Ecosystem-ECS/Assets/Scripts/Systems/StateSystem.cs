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

        Entities.ForEach((
            ref StateData stateData,
            ref ReproductiveData reproductiveData,
            in BasicNeedsData basicNeedsData,
            in TargetData targetData,
            in BioStatsData bioStatsData,
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

                //being chased over all other states
                if (HasComponent<Translation>(targetData.predatorEntity))
                {
                    stateData.previousState = stateData.state;
                    stateData.state = StateData.States.Fleeing;
                }
                //Update pregnancy status
                else if (reproductiveData.pregnant)
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
                            //float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToEat].Value);
                            if (targetData.shortestToEdibleDistance <= targetData.touchRadius)
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
                            //float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToDrink].Value);
                            if (targetData.shortestToWaterDistance <= targetData.touchRadius)
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
                            //float euclidian = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToMate].Value);
                            if (targetData.shortestToMateDistance <= targetData.touchRadius)
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
                        // this is ok , I think? or back to previousState?
                        if (!HasComponent<Translation>(targetData.predatorEntity))
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
