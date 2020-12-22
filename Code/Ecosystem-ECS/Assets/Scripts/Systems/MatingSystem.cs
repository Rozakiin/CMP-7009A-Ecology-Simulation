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

        #region Female data
        var rabbitMatingDuration = RabbitDefaults.matingDuration;
        var rabbitMateStartTime = RabbitDefaults.mateStartTime;
        var rabbitReproductiveUrge = RabbitDefaults.reproductiveUrge;
        var rabbitMatingThreshold = RabbitDefaults.matingThreshold;
        var rabbitEntityToMate = RabbitDefaults.entityToMate;

        var rabbitPregnant = RabbitDefaults.pregnant;
        var rabbitBirthDuration = RabbitDefaults.birthDuration;
        var rabbitBabiesBorn = RabbitDefaults.babiesBorn;
        var rabbitBirthStartTime = RabbitDefaults.birthStartTime;
        var rabbitCurrentLitterSize = RabbitDefaults.currentLitterSize;
        var rabbitLitterSizeMin = RabbitDefaults.litterSizeMin;
        var rabbitLitterSizeMax = RabbitDefaults.litterSizeMax;
        var rabbitLitterSizeAve = RabbitDefaults.litterSizeAve;
        var rabbitPregnancyLengthBase = RabbitDefaults.pregnancyLength;
        var rabbitPregnancyLengthModifier = RabbitDefaults.pregnancyLengthModifier;
        var rabbitPregnancyStartTime = RabbitDefaults.pregnancyStartTime;

        var rabbitReproductiveUrgeIncreaseFemale = RabbitDefaults.reproductiveUrgeIncreaseFemale;
        var rabbitReproductiveUrgeIncreaseMale = RabbitDefaults.reproductiveUrgeIncreaseMale;
        #endregion


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
                        previousState = femaleState,
                        state = StateData.States.Mating,
                        beenEaten = false
                    }
                );

                //Set the females mateStartTime to her age
                ecb.SetComponent(entityInQueryIndex, targetData.entityToMate,
                    new ReproductiveData
                    {
                        matingDuration = rabbitMatingDuration,
                        mateStartTime = GetComponentDataFromEntity<BioStatsData>(true)[targetData.entityToMate].age,
                        reproductiveUrge = rabbitReproductiveUrge,
                        reproductiveUrgeIncrease = rabbitReproductiveUrgeIncreaseFemale,
                        defaultRepoductiveIncrease = rabbitReproductiveUrgeIncreaseFemale,
                        matingThreshold = rabbitMatingThreshold,
                        entityToMate = rabbitEntityToMate,

                        pregnant = rabbitPregnant,
                        birthDuration = rabbitBirthDuration,
                        babiesBorn = GetComponentDataFromEntity<ReproductiveData>(true)[targetData.entityToMate].babiesBorn,
                        birthStartTime = rabbitBirthStartTime,
                        currentLitterSize = GetComponentDataFromEntity<ReproductiveData>(true)[targetData.entityToMate].currentLitterSize,
                        litterSizeMin = rabbitLitterSizeMin,
                        litterSizeMax = rabbitLitterSizeMax,
                        litterSizeAve = rabbitLitterSizeAve,
                        pregnancyLengthBase = rabbitPregnancyLengthBase,
                        pregnancyLengthModifier = GetComponentDataFromEntity<ReproductiveData>(true)[targetData.entityToMate].pregnancyLengthModifier,
                        pregnancyStartTime = rabbitPregnancyStartTime
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
