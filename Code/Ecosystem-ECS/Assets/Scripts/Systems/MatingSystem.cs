using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MatingSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate()
    {
        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        float deltaTime = Time.DeltaTime;


        Entities.ForEach((
            Entity e,
            int entityInQueryIndex,
            ref ReproductiveData reproductiveData,
            in BioStatsData bioStatsData,
            in Translation translation,
            in TargetData targetData
            ) =>
        {
            
            if (!HasComponent<StateData>(e))
                return;

            //Disable urge increase for non adults
            if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Adult)
            {
                reproductiveData.reproductiveUrgeIncrease = reproductiveData.defaultRepoductiveIncrease;
            }
            else
            {
                reproductiveData.reproductiveUrgeIncrease = 0f;
            }

            // Increase reproductive urge
            reproductiveData.reproductiveUrge += reproductiveData.reproductiveUrgeIncrease * deltaTime;

            //float distanceToMate = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToMate].Value);

            if (!HasComponent<Translation>(targetData.entityToMate))
                return;
            StateData state = GetComponent<StateData>(targetData.entityToMate);
            StateData.States femaleState = state.state;

            StateData eState = GetComponent<StateData>(e);
            StateData.States entityState = eState.state;

            //If it's a male, who's mating and the mate is not mating yet, turn her state into Mating
            if (bioStatsData.gender == BioStatsData.Gender.Male && entityState == StateData.States.Mating &&
                femaleState != StateData.States.Mating)
            {
                //Set the female's state to Mating
                ecb.SetComponent(entityInQueryIndex, targetData.entityToMate,
                    new StateData
                    {
                        state = StateData.States.Mating
                    }
                );

                //Set the females mateStartTime to her age
                ecb.SetComponent(entityInQueryIndex, targetData.entityToMate,
                    new ReproductiveData
                    {
                        mateStartTime = GetComponentDataFromEntity<BioStatsData>(true)[targetData.entityToMate].age
                    }
                );
            }

            //If the entityToMate exists and entity is mating
            if (reproductiveData.entityToMate != Entity.Null && entityState == StateData.States.Mating)
            {
                //If the mating has ended, the female becomes pregnant
                if (bioStatsData.age - reproductiveData.mateStartTime >= reproductiveData.matingDuration)
                {
                    if (bioStatsData.gender == BioStatsData.Gender.Female)
                    {
                        reproductiveData.pregnancyStartTime = bioStatsData.age;
                        reproductiveData.pregnant = true;
                        reproductiveData.currentLitterSize = reproductiveData.LitterSize;
                    }

                }
                reproductiveData.reproductiveUrge = 0;
                entityState = StateData.States.Wandering;
            }
        }).ScheduleParallel();
    }
}
