using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor;


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

                stateData.isWandering = ((stateData.flagState & StateData.FlagStates.Wandering) == StateData.FlagStates.Wandering);
                stateData.isHungry = ((stateData.flagState & StateData.FlagStates.Hungry) == StateData.FlagStates.Hungry);
                stateData.isThirsty = ((stateData.flagState & StateData.FlagStates.Thirsty) == StateData.FlagStates.Thirsty);
                stateData.isEating = ((stateData.flagState & StateData.FlagStates.Eating) == StateData.FlagStates.Eating);
                stateData.isDrinking = ((stateData.flagState & StateData.FlagStates.Drinking) == StateData.FlagStates.Drinking);
                stateData.isSexuallyActive = ((stateData.flagState & StateData.FlagStates.SexuallyActive) == StateData.FlagStates.SexuallyActive);
                stateData.isMating = ((stateData.flagState & StateData.FlagStates.Mating) == StateData.FlagStates.Mating);
                stateData.isFleeing = ((stateData.flagState & StateData.FlagStates.Fleeing) == StateData.FlagStates.Fleeing);
                stateData.isDead = ((stateData.flagState & StateData.FlagStates.Dead) == StateData.FlagStates.Dead);
                stateData.isPregnant = ((stateData.flagState & StateData.FlagStates.Pregnant) == StateData.FlagStates.Pregnant);
                stateData.isGivingBirth = ((stateData.flagState & StateData.FlagStates.GivingBirth) == StateData.FlagStates.GivingBirth);

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

                /*
                //old system
                //being chased over all other states
                if (HasComponent<Translation>(targetData.predatorEntity))
                {
                    stateData.previousState = stateData.state;
                    stateData.state = StateData.States.Fleeing;
                }*/

                //Update pregnancy status
                /*old system
                else if (reproductiveData.pregnant)
                {
                    if(bioStatsData.age - reproductiveData.pregnancyStartTime >= reproductiveData.PregnancyLength)
                    {
                        stateData.previousState = stateData.state;
                        stateData.state = StateData.States.GivingBirth;
                    }
                }
                */



                //new system

                if (!stateData.isDead)
                {
                    if (HasComponent<Translation>(targetData.predatorEntity))
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState = StateData.FlagStates.Fleeing;
                    }
                    else
                    {   //untoggle fleeing
                        stateData.flagState &= (~StateData.FlagStates.Fleeing);
                    }

                    if (stateData.isFleeing)
                    {
                        if (targetData.shortestToPredatorDistance <= targetData.touchRadius * 2)
                        {
                            stateData.beenEaten = true;
                        }
                    }

                    //The rabbit can still give birth when fleeing - bad luck I guess
                    if (stateData.isPregnant)
                    {
                        if (bioStatsData.age - reproductiveData.pregnancyStartTime >= reproductiveData.PregnancyLength)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState = StateData.FlagStates.GivingBirth;
                        }
                    }

                    if (!stateData.isGivingBirth && !stateData.isEating && !stateData.isDrinking && !stateData.isMating && !stateData.isFleeing)
                    {
                        if (reproductiveData.reproductiveUrge >= reproductiveData.matingThreshold &&
                        !stateData.isSexuallyActive)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState ^= StateData.FlagStates.SexuallyActive;
                        }
                        if (basicNeedsData.thirst >= basicNeedsData.thirstyThreshold &&
                        !stateData.isThirsty)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState ^= StateData.FlagStates.Thirsty;
                        }
                        if (basicNeedsData.hunger >= basicNeedsData.hungryThreshold &&
                        !stateData.isHungry)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState ^= StateData.FlagStates.Hungry;
                        }

                        if (stateData.isHungry)
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

                        if (stateData.isThirsty)
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

                        if (stateData.isSexuallyActive)
                        {
                            //If the entity isn't old, mate
                            if (bioStatsData.ageGroup != BioStatsData.AgeGroup.Old)
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
                            //Otherwise reset reproducitve urge and toggle the substate
                            else
                            {
                                reproductiveData.reproductiveUrge = 0;
                                stateData.flagState ^= StateData.FlagStates.SexuallyActive;
                            }
                        }
                    }

                    //This 3 if's will probably have to be modified to accommodate for Will's new targetting system
                    //Instead of checking for the distance to the entity, it might be check what is its intended target
                    //At the moment?
                    if (stateData.isEating)
                    {
                        if (!HasComponent<Translation>(targetData.entityToEat))
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState = StateData.FlagStates.Hungry;
                        }
                        if (basicNeedsData.hunger <= basicNeedsData.hungryThreshold)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState = StateData.FlagStates.Wandering;
                        }
                    }

                    if (stateData.isDrinking)
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

                    if (stateData.isMating)
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
                }

                stateData.isWandering = ((stateData.flagState & StateData.FlagStates.Wandering) == StateData.FlagStates.Wandering);
                stateData.isHungry = ((stateData.flagState & StateData.FlagStates.Hungry) == StateData.FlagStates.Hungry);
                stateData.isThirsty = ((stateData.flagState & StateData.FlagStates.Thirsty) == StateData.FlagStates.Thirsty);
                stateData.isEating = ((stateData.flagState & StateData.FlagStates.Eating) == StateData.FlagStates.Eating);
                stateData.isDrinking = ((stateData.flagState & StateData.FlagStates.Drinking) == StateData.FlagStates.Drinking);
                stateData.isSexuallyActive = ((stateData.flagState & StateData.FlagStates.SexuallyActive) == StateData.FlagStates.SexuallyActive);
                stateData.isMating = ((stateData.flagState & StateData.FlagStates.Mating) == StateData.FlagStates.Mating);
                stateData.isFleeing = ((stateData.flagState & StateData.FlagStates.Fleeing) == StateData.FlagStates.Fleeing);
                stateData.isDead = ((stateData.flagState & StateData.FlagStates.Dead) == StateData.FlagStates.Dead);
                stateData.isPregnant = ((stateData.flagState & StateData.FlagStates.Pregnant) == StateData.FlagStates.Pregnant);
                stateData.isGivingBirth = ((stateData.flagState & StateData.FlagStates.GivingBirth) == StateData.FlagStates.GivingBirth);
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
                */
            }).ScheduleParallel();
    }
}
