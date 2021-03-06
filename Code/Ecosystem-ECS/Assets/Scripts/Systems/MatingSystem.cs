﻿using Components;
using EntityDefaults;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public class MatingSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;


        protected override void OnCreate()
        {
            base.OnCreate();
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        /*
         * increases reproductive urge for adult males
         * mates with females and sets females state to mating
         * female becomes pregnant after mating has finished
         */
        protected override void OnUpdate()
        {
            var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

            float deltaTime = Time.DeltaTime;

            //for each that edits reproductivedata of the entity
            Entities.ForEach((
                ref ReproductiveData reproductiveData,
                in StateData stateData,
                in BioStatsData bioStatsData,
                in TargetData targetData
            ) =>
            {
                //Disable urge increase for non adults
                if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Adult)
                {
                    reproductiveData.reproductiveUrgeIncrease = reproductiveData.defaultRepoductiveIncrease;
                    // Increase reproductive urge
                    reproductiveData.reproductiveUrge += reproductiveData.reproductiveUrgeIncrease * deltaTime;
                }
                else
                {
                    reproductiveData.reproductiveUrgeIncrease = 0f;
                }

                //If it's in a state of looking for a mate
                if (stateData.isSexuallyActive)
                {
                    if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Adult)
                    {
                        //If it has found an entity to mate with
                        if (HasComponent<Translation>(targetData.entityToMate))
                        {
                            //If the mate is close enough to mate with
                            if (targetData.shortestToMateDistance <= targetData.mateRadius)
                            {
                                reproductiveData.mateStartTime = bioStatsData.age;
                            }
                        }
                    }
                    else
                    {
                        reproductiveData.reproductiveUrge = 0f;
                    }
                }

                //If entity is mating
                if (stateData.isMating)
                {
                    //If the mating has ended, the female becomes pregnant
                    if (bioStatsData.age - reproductiveData.mateStartTime >= reproductiveData.matingDuration)
                    {
                        if (bioStatsData.gender == BioStatsData.Gender.Female)
                        {
                            reproductiveData.pregnancyStartTime = bioStatsData.age;
                            reproductiveData.babiesBorn = 0;
                            reproductiveData.currentLitterSize = reproductiveData.LitterSize;
                        }

                        reproductiveData.reproductiveUrge = 0;
                    }
                }
            }).ScheduleParallel();

            //for each that edits the entityToMate components
            Entities.ForEach((
                int entityInQueryIndex,
                in BioStatsData bioStatsData,
                in TargetData targetData,
                in StateData stateData
            ) =>
            {
                if (stateData.isSexuallyActive)
                {
                    //if entityToMate exists (everything should have translation)
                    if (HasComponent<Translation>(targetData.entityToMate))
                    {
                        //get stateData of entityToMate
                        StateData mateStateData = GetComponentDataFromEntity<StateData>(true)[targetData.entityToMate];

                        //If it's a male, who's mating, and the mate is not mating yet, set state of mate to Mating
                        if (bioStatsData.gender == BioStatsData.Gender.Male &&
                            stateData.isMating &&
                            !mateStateData.isMating)
                        {
                            //Set the mate's state to Mating
                            mateStateData.flagState |= StateData.FlagStates.Mating;
                            ecb.SetComponent(entityInQueryIndex, targetData.entityToMate, mateStateData);

                            //GetComponent calls are slow so cache for multiple uses
                            float mateAge = GetComponentDataFromEntity<BioStatsData>(true)[targetData.entityToMate].age;
                            ReproductiveData mateReproductiveData =
                                GetComponentDataFromEntity<ReproductiveData>(true)[targetData.entityToMate];

                            //Set the mates mateStartTime to her age
                            mateReproductiveData.mateStartTime = mateAge;
                            ecb.SetComponent(entityInQueryIndex, targetData.entityToMate, mateReproductiveData);
                        }
                    }
                }
            }).ScheduleParallel();

            // Make sure that the ECB system knows about our job
            ecbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}