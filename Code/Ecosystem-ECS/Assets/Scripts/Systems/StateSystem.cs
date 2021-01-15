using Components;
using MonoBehaviourTools.Grid;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class StateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float tileSize = SimulationManager.tileSize;
            float gridNodeDiameter = GridSetup.Instance.gridNodeDiameter;

            Entities.ForEach((
                ref StateData stateData,
                ref ReproductiveData reproductiveData,
                in BasicNeedsData basicNeedsData,
                in TargetData targetData,
                in BioStatsData bioStatsData
            ) =>
            {
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


                /* SETTING OF STATES DUE TO EXTERNAL FACTORS */

                // determine if fleeing
                if (HasComponent<Translation>(targetData.predatorEntity))
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState |= StateData.FlagStates.Fleeing; //enable fleeing
                }
                else
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState &= (~StateData.FlagStates.Fleeing); //disable fleeing
                }

                // if they are over mating threshold and Adult enable sexually active state
                if (reproductiveData.reproductiveUrge >= reproductiveData.matingThreshold && bioStatsData.ageGroup == BioStatsData.AgeGroup.Adult)
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState |= StateData.FlagStates.SexuallyActive; //enable sexually active
                }

                // if they are over thirst threshold enable thirsty state
                if (basicNeedsData.thirst >= basicNeedsData.thirstyThreshold)
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState |= StateData.FlagStates.Thirsty; //enable thirsty
                }

                // if they are over hunger threshold enable hungry state
                if (basicNeedsData.hunger >= basicNeedsData.hungryThreshold)
                {
                    stateData.previousFlagState = stateData.flagState;
                    stateData.flagState |= StateData.FlagStates.Hungry; //enable hungry
                }


                /* SETTING OF STATES DUE TO CURRENT STATE */
                //if not dead            
                stateData.isDead = ((stateData.flagState & StateData.FlagStates.Dead) == StateData.FlagStates.Dead);
                if (!stateData.isDead)
                {
                    // enable eating state if close to entity to eat
                    stateData.isHungry = ((stateData.flagState & StateData.FlagStates.Hungry) == StateData.FlagStates.Hungry);
                    stateData.isEating = ((stateData.flagState & StateData.FlagStates.Eating) == StateData.FlagStates.Eating);
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


                    // enable eating state if close to entity to eat
                    stateData.isThirsty = ((stateData.flagState & StateData.FlagStates.Thirsty) == StateData.FlagStates.Thirsty);
                    stateData.isDrinking = ((stateData.flagState & StateData.FlagStates.Drinking) == StateData.FlagStates.Drinking);
                    if (stateData.isThirsty && !stateData.isDrinking)
                    {
                        if (HasComponent<Translation>(targetData.entityToDrink))
                        {
                            //sqrt due to square tiles (furthest point possible is right in corner of gridnode next to edge of tile
                            if (targetData.shortestToWaterDistance <= targetData.touchRadius + math.sqrt(tileSize * tileSize / 2) + math.sqrt(gridNodeDiameter * gridNodeDiameter / 2))
                            {
                                stateData.previousFlagState = stateData.flagState;
                                stateData.flagState &= ~StateData.FlagStates.Wandering;//disable wandering
                                stateData.flagState |= StateData.FlagStates.Drinking;//enable drinking
                            }
                        }
                    }


                    // enable mating if close to entity to mate
                    stateData.isSexuallyActive = ((stateData.flagState & StateData.FlagStates.SexuallyActive) == StateData.FlagStates.SexuallyActive);
                    stateData.isMating = ((stateData.flagState & StateData.FlagStates.Mating) == StateData.FlagStates.Mating);
                    if (stateData.isSexuallyActive && !stateData.isMating)
                    {
                        if (HasComponent<Translation>(targetData.entityToMate))
                        {
                            if (targetData.shortestToMateDistance <= targetData.mateRadius)
                            {
                                stateData.previousFlagState = stateData.flagState;
                                stateData.flagState &= ~StateData.FlagStates.Wandering;//disable wandering
                                stateData.flagState |= StateData.FlagStates.Mating;//enable mating
                            }
                        }
                    }


                    stateData.isEating = ((stateData.flagState & StateData.FlagStates.Eating) == StateData.FlagStates.Eating);
                    if (stateData.isEating)
                    {
                        //entity doesnt exist, disable eating
                        if (!HasComponent<Translation>(targetData.entityToEat))
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState &= ~StateData.FlagStates.Eating;
                        }
                        //hunger saited, disable hungry and eating, enable wandering
                        if (basicNeedsData.hunger <= basicNeedsData.hungryThreshold)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState |= StateData.FlagStates.Wandering;
                            stateData.flagState &= ~StateData.FlagStates.Hungry;
                            stateData.flagState &= ~StateData.FlagStates.Eating;
                        }
                    }


                    stateData.isDrinking = ((stateData.flagState & StateData.FlagStates.Drinking) == StateData.FlagStates.Drinking);
                    if (stateData.isDrinking)
                    {
                        //entity doesnt exist, disable drinking
                        if (!HasComponent<Translation>(targetData.entityToDrink))
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState &= ~StateData.FlagStates.Drinking;
                        }
                        //thirst quenched, disable hungry and eating, enable wandering
                        if (basicNeedsData.thirst <= 0)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState |= StateData.FlagStates.Wandering;
                            stateData.flagState &= ~StateData.FlagStates.Thirsty;
                            stateData.flagState &= ~StateData.FlagStates.Drinking;
                        }
                    }


                    stateData.isMating = ((stateData.flagState & StateData.FlagStates.Mating) == StateData.FlagStates.Mating);
                    if (stateData.isMating)
                    {
                        stateData.flagState &= ~StateData.FlagStates.Wandering;

                        //If the mating has ended, the female becomes pregnant
                        if (bioStatsData.age - reproductiveData.mateStartTime >= reproductiveData.matingDuration)
                        {
                            if (bioStatsData.gender == BioStatsData.Gender.Female)
                            {
                                stateData.previousFlagState = stateData.flagState;
                                stateData.flagState |= StateData.FlagStates.Pregnant; //enable pregnant state
                            }
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState |= StateData.FlagStates.Wandering;
                            stateData.flagState &= ~StateData.FlagStates.Mating; //disable mating state
                        }

                        if (bioStatsData.gender == BioStatsData.Gender.Male)
                        {
                            //reproductive urge saited, disable sexually active and mating, enable wandering
                            if (reproductiveData.reproductiveUrge <= 0)
                            {
                                stateData.previousFlagState = stateData.flagState;
                                stateData.flagState |= StateData.FlagStates.Wandering;
                                stateData.flagState &= ~StateData.FlagStates.SexuallyActive;
                                stateData.flagState &= ~StateData.FlagStates.Mating;
                            }
                        }
                    }

                    //The rabbit can still give birth when fleeing - bad luck I guess
                    stateData.isPregnant = ((stateData.flagState & StateData.FlagStates.Pregnant) == StateData.FlagStates.Pregnant);
                    if (stateData.isPregnant)
                    {
                        if (bioStatsData.age - reproductiveData.pregnancyStartTime >= reproductiveData.PregnancyLength)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState &= ~StateData.FlagStates.Pregnant;
                            stateData.flagState |= StateData.FlagStates.GivingBirth;
                        }

                    }

                    stateData.isGivingBirth = ((stateData.flagState & StateData.FlagStates.GivingBirth) == StateData.FlagStates.GivingBirth);
                    if (stateData.isGivingBirth)
                    {
                        if (reproductiveData.babiesBorn >= reproductiveData.currentLitterSize)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState &= ~StateData.FlagStates.GivingBirth;
                        }

                        if (reproductiveData.babiesBorn >= reproductiveData.currentLitterSize)
                        {
                            stateData.previousFlagState = stateData.flagState;
                            stateData.flagState &= ~StateData.FlagStates.Pregnant;
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
            }).ScheduleParallel();
        }
    }
}
