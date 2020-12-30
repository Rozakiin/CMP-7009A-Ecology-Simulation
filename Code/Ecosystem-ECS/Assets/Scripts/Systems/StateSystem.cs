using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor;
using UnityEngine;


public class StateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float tileSize = SimulationManager.tileSize;

        Entities.ForEach((
            ref StateData stateData,
            ref ReproductiveData reproductiveData,
            in BasicNeedsData basicNeedsData,
            in TargetData targetData,
            in BioStatsData bioStatsData,
            in Translation translation
            ) =>
        {

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


            //new system

            if (!stateData.isDead)
            {
                if (HasComponent<Translation>(targetData.predatorEntity))
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState |= StateData.FlagStates.Fleeing;
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
                /*if ((stateData.flagState & StateData.FlagStates.Pregnant) == StateData.FlagStates.Pregnant)
                {
                    //Debug.Log("Age: " + bioStatsData.age);
                    //Debug.Log("Pregnancy Start Time: " + reproductiveData.pregnancyStartTime);
                    //Debug.Log("Preg Length: " + reproductiveData.PregnancyLength);
                    Debug.Log(bioStatsData.age - reproductiveData.pregnancyStartTime);
                    if (bioStatsData.age - reproductiveData.pregnancyStartTime >= reproductiveData.PregnancyLength)
                    {
                        Debug.Log("cos");
                        //stateData.previousFlagState = stateData.flagState;
                        //stateData.flagState = StateData.FlagStates.GivingBirth;
                    }
                }*/

                if (((stateData.flagState & StateData.FlagStates.GivingBirth) == 0) &&
                    ((stateData.flagState & StateData.FlagStates.Eating) == 0) &&
                    ((stateData.flagState & StateData.FlagStates.Drinking) == 0) &&
                    ((stateData.flagState & StateData.FlagStates.Mating) == 0) &&
                    ((stateData.flagState & StateData.FlagStates.Fleeing) == 0))
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

                    if (stateData.isHungry && !stateData.isEating)
                    {
                        if (HasComponent<Translation>(targetData.entityToEat))
                        {
                            if (targetData.shortestToEdibleDistance <= targetData.touchRadius)
                            {
                                stateData.previousFlagState = stateData.flagState;
                                stateData.flagState |= StateData.FlagStates.Eating;
                            }
                        }
                    }

                    if (stateData.isThirsty && !stateData.isDrinking)
                    {
                        if (HasComponent<Translation>(targetData.entityToDrink))
                        {
                            if (targetData.shortestToWaterDistance <= targetData.touchRadius)
                            {
                                stateData.previousFlagState = stateData.flagState;
                                stateData.flagState |= StateData.FlagStates.Drinking;
                            }
                        }
                    }

                    if (stateData.isSexuallyActive)
                    {
                        //If the entity isn't old, mate
                        if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Adult)
                        {
                            if (HasComponent<Translation>(targetData.entityToMate))
                            {
                                if (targetData.shortestToMateDistance <= targetData.touchRadius * 2)
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

                if (stateData.isEating)
                {
                    stateData.flagState &= (~StateData.FlagStates.Hungry);
                    if (!HasComponent<Translation>(targetData.entityToEat))
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState |= StateData.FlagStates.Hungry;
                        stateData.flagState ^= StateData.FlagStates.Eating;
                    }
                    if (basicNeedsData.hunger <= basicNeedsData.hungryThreshold)
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState |= StateData.FlagStates.Wandering;
                        stateData.flagState ^= StateData.FlagStates.Eating;
                    }
                }

                if (stateData.isDrinking)
                {
                    stateData.flagState &= (~StateData.FlagStates.Thirsty);
                    if (!HasComponent<Translation>(targetData.entityToDrink))
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState |= StateData.FlagStates.Thirsty;
                        stateData.flagState ^= StateData.FlagStates.Drinking;
                    }
                    if (basicNeedsData.thirst <= 0)
                    {
                        stateData.previousFlagState = stateData.flagState;
                        stateData.flagState |= StateData.FlagStates.Wandering;
                        stateData.flagState ^= StateData.FlagStates.Drinking;
                    }
                }

                if (stateData.isMating)
                {
                    if (bioStatsData.gender == BioStatsData.Gender.Male)
                    {
                        /*if (!HasComponent<Translation>(targetData.entityToMate))
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState = StateData.FlagStates.SexuallyActive;
                        }*/
                        if (reproductiveData.reproductiveUrge <= 0)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState = StateData.FlagStates.Wandering;
                        }
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

        }).WithoutBurst().Run();
    }
}
