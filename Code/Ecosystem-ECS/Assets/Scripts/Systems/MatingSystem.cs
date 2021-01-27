using Components;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public class MatingSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecbSystem;


        protected override void OnCreate()
        {
            base.OnCreate();
            _ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        /*
         * increases reproductive urge for adult males
         * mates with females and sets females state to mating
         * female becomes pregnant after mating has finished
         */
        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer().ToConcurrent();

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
                if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Adult)
                {
                    reproductiveData.ReproductiveUrgeIncrease = reproductiveData.DefaultReproductiveIncrease;
                    // Increase reproductive urge
                    reproductiveData.ReproductiveUrge += reproductiveData.ReproductiveUrgeIncrease * deltaTime;
                }
                else
                {
                    reproductiveData.ReproductiveUrgeIncrease = 0f;
                }

                //If it's in a state of looking for a mate
                if (stateData.IsSexuallyActive)
                {
                    if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Adult)
                    {
                        //If it has found an entity to mate with
                        if (HasComponent<Translation>(targetData.EntityToMate))
                        {
                            //If the mate is close enough to mate with
                            if (targetData.ShortestDistanceToMate <= targetData.MateRadius)
                            {
                                reproductiveData.MateStartTime = bioStatsData.Age;
                            }
                        }
                    }
                    else
                    {
                        reproductiveData.ReproductiveUrge = 0f;
                    }
                }

                //If entity is mating
                if (stateData.IsMating)
                {
                    //If the mating has ended, the female becomes pregnant
                    if (bioStatsData.Age - reproductiveData.MateStartTime >= reproductiveData.MatingDuration)
                    {
                        if (bioStatsData.Gender == BioStatsData.Genders.Female)
                        {
                            reproductiveData.PregnancyStartTime = bioStatsData.Age;
                            reproductiveData.BabiesBorn = 0;
                            reproductiveData.CurrentLitterSize = reproductiveData.LitterSize;
                        }

                        reproductiveData.ReproductiveUrge = 0;
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
                if (stateData.IsSexuallyActive)
                {
                    //if entityToMate exists (everything should have translation)
                    if (HasComponent<Translation>(targetData.EntityToMate))
                    {
                        //get stateData of entityToMate
                        StateData mateStateData = GetComponentDataFromEntity<StateData>(true)[targetData.EntityToMate];

                        //If it's a male, who's mating, and the mate is not mating yet, set state of mate to Mating
                        if (bioStatsData.Gender == BioStatsData.Genders.Male &&
                            stateData.IsMating &&
                            !mateStateData.IsMating)
                        {
                            //Set the mate's state to Mating
                            mateStateData.FlagStateCurrent |= StateData.FlagStates.Mating;
                            ecb.SetComponent(entityInQueryIndex, targetData.EntityToMate, mateStateData);

                            //GetComponent calls are slow so cache for multiple uses
                            float mateAge = GetComponentDataFromEntity<BioStatsData>(true)[targetData.EntityToMate].Age;
                            ReproductiveData mateReproductiveData =
                                GetComponentDataFromEntity<ReproductiveData>(true)[targetData.EntityToMate];

                            //Set the mates mateStartTime to her age
                            mateReproductiveData.MateStartTime = mateAge;
                            ecb.SetComponent(entityInQueryIndex, targetData.EntityToMate, mateReproductiveData);
                        }
                    }
                }
            }).ScheduleParallel();

            // Make sure that the ECB system knows about our job
            _ecbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}