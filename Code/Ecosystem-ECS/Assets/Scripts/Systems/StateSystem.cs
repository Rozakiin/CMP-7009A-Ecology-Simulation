using Components;
using MonoBehaviourTools.Grid;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class StateSystem : SystemBase
    {
        /*
         * determines the states the entity is in, also determines death reason
         */
        protected override void OnUpdate()
        {
            float tileSize = SimulationManager.TileSize;
            float gridNodeDiameter = GridSetup.Instance.GridNodeDiameter;

            Entities.ForEach((
                ref StateData stateData,
                ref ReproductiveData reproductiveData,
                in BasicNeedsData basicNeedsData,
                in TargetData targetData,
                in BioStatsData bioStatsData
            ) =>
            {
                // determine if the entity shouldn't be dead
                if (stateData.BeenEaten)
                {
                    stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                    stateData.FlagStateCurrent = StateData.FlagStates.Dead;
                    stateData.DeathReason = StateData.DeathReasons.Eaten;
                }
                else if (basicNeedsData.Thirst >= basicNeedsData.ThirstMax)
                {
                    stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                    stateData.FlagStateCurrent = StateData.FlagStates.Dead;
                    stateData.DeathReason = StateData.DeathReasons.Thirst;
                }
                else if (basicNeedsData.Hunger >= basicNeedsData.HungerMax)
                {
                    stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                    stateData.FlagStateCurrent = StateData.FlagStates.Dead;
                    stateData.DeathReason = StateData.DeathReasons.Hunger;
                }
                else if (bioStatsData.Age >= bioStatsData.AgeMax)
                {
                    stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                    stateData.FlagStateCurrent = StateData.FlagStates.Dead;
                    stateData.DeathReason = StateData.DeathReasons.Age;
                }


                /* SETTING OF STATES DUE TO EXTERNAL FACTORS */

                // determine if fleeing
                if (HasComponent<Translation>(targetData.PredatorEntity))
                {
                    stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                    stateData.FlagStateCurrent |= StateData.FlagStates.Fleeing; //enable fleeing
                }
                else
                {
                    stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                    stateData.FlagStateCurrent &= (~StateData.FlagStates.Fleeing); //disable fleeing
                }

                // if they are over mating threshold and Adult enable sexually active state
                if (reproductiveData.ReproductiveUrge >= reproductiveData.MatingThreshold &&
                    bioStatsData.AgeGroup == BioStatsData.AgeGroups.Adult)
                {
                    stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                    stateData.FlagStateCurrent |= StateData.FlagStates.SexuallyActive; //enable sexually active
                }

                // if they are over thirst threshold enable thirsty state
                if (basicNeedsData.Thirst >= basicNeedsData.ThirstyThreshold)
                {
                    stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                    stateData.FlagStateCurrent |= StateData.FlagStates.Thirsty; //enable thirsty
                }

                // if they are over hunger threshold enable hungry state
                if (basicNeedsData.Hunger >= basicNeedsData.HungryThreshold)
                {
                    stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                    stateData.FlagStateCurrent |= StateData.FlagStates.Hungry; //enable hungry
                }


                /* SETTING OF STATES DUE TO CURRENT STATE */
                //if not dead            
                stateData.IsDead = ((stateData.FlagStateCurrent & StateData.FlagStates.Dead) == StateData.FlagStates.Dead);
                if (!stateData.IsDead)
                {
                    // enable eating state if close to entity to eat
                    stateData.IsHungry = ((stateData.FlagStateCurrent & StateData.FlagStates.Hungry) ==
                                          StateData.FlagStates.Hungry);
                    stateData.IsEating = ((stateData.FlagStateCurrent & StateData.FlagStates.Eating) ==
                                          StateData.FlagStates.Eating);
                    if (stateData.IsHungry && !stateData.IsEating)
                    {
                        if (HasComponent<Translation>(targetData.EntityToEat))
                        {
                            if (targetData.ShortestDistanceToEdible <= targetData.TouchRadius)
                            {
                                stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                                stateData.FlagStateCurrent |= StateData.FlagStates.Eating;
                            }
                        }
                    }


                    // enable eating state if close to entity to eat
                    stateData.IsThirsty = ((stateData.FlagStateCurrent & StateData.FlagStates.Thirsty) ==
                                           StateData.FlagStates.Thirsty);
                    stateData.IsDrinking = ((stateData.FlagStateCurrent & StateData.FlagStates.Drinking) ==
                                            StateData.FlagStates.Drinking);
                    if (stateData.IsThirsty && !stateData.IsDrinking)
                    {
                        if (HasComponent<Translation>(targetData.EntityToDrink))
                        {
                            //sqrt due to square tiles (furthest point possible is right in corner of gridnode next to edge of tile
                            if (targetData.ShortestDistanceToWater <= targetData.TouchRadius +
                                math.sqrt(tileSize * tileSize / 2) + math.sqrt(gridNodeDiameter * gridNodeDiameter / 2))
                            {
                                stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                                stateData.FlagStateCurrent &= ~StateData.FlagStates.Wandering; //disable wandering
                                stateData.FlagStateCurrent |= StateData.FlagStates.Drinking; //enable drinking
                            }
                        }
                    }


                    // enable mating if close to entity to mate
                    stateData.IsSexuallyActive = ((stateData.FlagStateCurrent & StateData.FlagStates.SexuallyActive) ==
                                                  StateData.FlagStates.SexuallyActive);
                    stateData.IsMating = ((stateData.FlagStateCurrent & StateData.FlagStates.Mating) ==
                                          StateData.FlagStates.Mating);
                    if (stateData.IsSexuallyActive && !stateData.IsMating)
                    {
                        if (HasComponent<Translation>(targetData.EntityToMate))
                        {
                            if (targetData.ShortestDistanceToMate <= targetData.MateRadius)
                            {
                                stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                                stateData.FlagStateCurrent &= ~StateData.FlagStates.Wandering; //disable wandering
                                stateData.FlagStateCurrent |= StateData.FlagStates.Mating; //enable mating
                            }
                        }

                        if (bioStatsData.Gender == BioStatsData.Genders.Male)
                        {
                            //reproductive urge saited, disable sexually active and mating, enable wandering
                            if (reproductiveData.ReproductiveUrge <= 0)
                            {
                                stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                                stateData.FlagStateCurrent |= StateData.FlagStates.Wandering;
                                stateData.FlagStateCurrent &= ~StateData.FlagStates.SexuallyActive;
                            }
                        }
                    }


                    stateData.IsEating = ((stateData.FlagStateCurrent & StateData.FlagStates.Eating) ==
                                          StateData.FlagStates.Eating);
                    if (stateData.IsEating)
                    {
                        //entity doesnt exist, disable eating
                        if (!HasComponent<Translation>(targetData.EntityToEat))
                        {
                            stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.Eating;
                        }

                        //hunger saited, disable hungry and eating, enable wandering
                        if (basicNeedsData.Hunger <= basicNeedsData.HungryThreshold)
                        {
                            stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                            stateData.FlagStateCurrent |= StateData.FlagStates.Wandering;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.Hungry;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.Eating;
                        }
                    }


                    stateData.IsDrinking = ((stateData.FlagStateCurrent & StateData.FlagStates.Drinking) ==
                                            StateData.FlagStates.Drinking);
                    if (stateData.IsDrinking)
                    {
                        //entity doesnt exist, disable drinking
                        if (!HasComponent<Translation>(targetData.EntityToDrink))
                        {
                            stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.Drinking;
                        }

                        //thirst quenched, disable thirsty and drinking, enable wandering
                        if (basicNeedsData.Thirst <= 0)
                        {
                            stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                            stateData.FlagStateCurrent |= StateData.FlagStates.Wandering;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.Thirsty;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.Drinking;
                        }
                    }


                    stateData.IsMating = ((stateData.FlagStateCurrent & StateData.FlagStates.Mating) ==
                                          StateData.FlagStates.Mating);
                    if (stateData.IsMating)
                    {
                        stateData.FlagStateCurrent &= ~StateData.FlagStates.Wandering;

                        //If the mating has ended, the female becomes pregnant
                        if (bioStatsData.Age - reproductiveData.MateStartTime >= reproductiveData.MatingDuration)
                        {
                            if (bioStatsData.Gender == BioStatsData.Genders.Female)
                            {
                                stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                                stateData.FlagStateCurrent |= StateData.FlagStates.Pregnant; //enable pregnant state
                            }

                            stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                            stateData.FlagStateCurrent |= StateData.FlagStates.Wandering;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.Mating; //disable mating state
                        }
                    }

                    //The rabbit can still give birth when fleeing - bad luck I guess
                    stateData.IsPregnant = ((stateData.FlagStateCurrent & StateData.FlagStates.Pregnant) ==
                                            StateData.FlagStates.Pregnant);
                    if (stateData.IsPregnant)
                    {
                        if (bioStatsData.Age - reproductiveData.PregnancyStartTime >= reproductiveData.PregnancyLength)
                        {
                            stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.Pregnant;
                            stateData.FlagStateCurrent |= StateData.FlagStates.GivingBirth;
                        }
                    }

                    stateData.IsGivingBirth = ((stateData.FlagStateCurrent & StateData.FlagStates.GivingBirth) ==
                                               StateData.FlagStates.GivingBirth);
                    if (stateData.IsGivingBirth)
                    {
                        if (reproductiveData.BabiesBorn >= reproductiveData.CurrentLitterSize)
                        {
                            stateData.FlagStatePrevious = stateData.FlagStateCurrent;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.Pregnant;
                            stateData.FlagStateCurrent &= ~StateData.FlagStates.GivingBirth;
                        }
                    }
                }

                //Update states at the end
                stateData.IsWandering = ((stateData.FlagStateCurrent & StateData.FlagStates.Wandering) ==
                                         StateData.FlagStates.Wandering);
                stateData.IsHungry =
                    ((stateData.FlagStateCurrent & StateData.FlagStates.Hungry) == StateData.FlagStates.Hungry);
                stateData.IsThirsty =
                    ((stateData.FlagStateCurrent & StateData.FlagStates.Thirsty) == StateData.FlagStates.Thirsty);
                stateData.IsEating =
                    ((stateData.FlagStateCurrent & StateData.FlagStates.Eating) == StateData.FlagStates.Eating);
                stateData.IsDrinking = ((stateData.FlagStateCurrent & StateData.FlagStates.Drinking) ==
                                        StateData.FlagStates.Drinking);
                stateData.IsSexuallyActive = ((stateData.FlagStateCurrent & StateData.FlagStates.SexuallyActive) ==
                                              StateData.FlagStates.SexuallyActive);
                stateData.IsMating =
                    ((stateData.FlagStateCurrent & StateData.FlagStates.Mating) == StateData.FlagStates.Mating);
                stateData.IsFleeing =
                    ((stateData.FlagStateCurrent & StateData.FlagStates.Fleeing) == StateData.FlagStates.Fleeing);
                stateData.IsDead = ((stateData.FlagStateCurrent & StateData.FlagStates.Dead) == StateData.FlagStates.Dead);
                stateData.IsPregnant = ((stateData.FlagStateCurrent & StateData.FlagStates.Pregnant) ==
                                        StateData.FlagStates.Pregnant);
                stateData.IsGivingBirth = ((stateData.FlagStateCurrent & StateData.FlagStates.GivingBirth) ==
                                           StateData.FlagStates.GivingBirth);
            }).ScheduleParallel();
        }
    }
}
