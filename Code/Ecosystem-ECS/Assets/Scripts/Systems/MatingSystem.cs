using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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

        //for each that edits reproductivedata of the entity
        Entities.ForEach((
            ref ReproductiveData reproductiveData,
            ref StateData stateData,
            in BioStatsData bioStatsData
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


            //If entity is mating
            if (stateData.isMating)
            {
                //If the mating has ended, the female becomes pregnant
                if (bioStatsData.age - reproductiveData.mateStartTime >= reproductiveData.matingDuration)
                {
                    if (bioStatsData.gender == BioStatsData.Gender.Female)
                    {
                        reproductiveData.pregnancyStartTime = bioStatsData.age;
                        stateData.flagState ^= StateData.FlagStates.Pregnant;
                        reproductiveData.pregnant = true;
                        reproductiveData.currentLitterSize = reproductiveData.LitterSize;
                    }
                    stateData.flagState ^= StateData.FlagStates.Mating;
                    reproductiveData.reproductiveUrge = 0;
                    //stateData.state = StateData.States.Wandering;
                    //stateData.flagState = StateData.FlagStates.Wandering;
                }         
            }
        }).ScheduleParallel();

        //for each that edits the entityToMate components
        Entities.ForEach((
            Entity entity,
            int entityInQueryIndex,
            in BioStatsData bioStatsData,
            in TargetData targetData,
            in StateData stateData
            ) =>
        {
            //if entityToMate exists (everything should have translation)
            if (HasComponent<Translation>(targetData.entityToMate))
            {
                //get state of entityToMate
                StateData.States mateState = GetComponentDataFromEntity<StateData>(true)[targetData.entityToMate].state;
                StateData.FlagStates flagMateState = GetComponentDataFromEntity<StateData>(true)[targetData.entityToMate].flagState;

                //If it's a male, who's mating, and the mate is not mating yet, set state of mate to Mating
                if (
                bioStatsData.gender == BioStatsData.Gender.Male &&
                stateData.flagState == StateData.FlagStates.Mating &&
                flagMateState != StateData.FlagStates.Mating
                )
                {
                    //Set the mate's state to Mating
                    ecb.SetComponent(entityInQueryIndex, targetData.entityToMate,
                        new StateData
                        {
                            previousState = mateState,
                            state = StateData.States.Mating,

                            previousFlagState = flagMateState,
                            flagState = StateData.FlagStates.Mating,
                            beenEaten = false
                        }
                    );

                    //GetComponent calls are slow so cache for multiple uses
                    BioStatsData mateBioStatsData = GetComponentDataFromEntity<BioStatsData>(true)[targetData.entityToMate];
                    var mateBabiesBorn = GetComponentDataFromEntity<ReproductiveData>(true)[targetData.entityToMate].babiesBorn;
                    var mateCurrentLitterSize = GetComponentDataFromEntity<ReproductiveData>(true)[targetData.entityToMate].currentLitterSize;
                    var matePregnancyLengthModifier = GetComponentDataFromEntity<ReproductiveData>(true)[targetData.entityToMate].pregnancyLengthModifier;


                    //Set the mates mateStartTime to her age
                    ecb.SetComponent(entityInQueryIndex, targetData.entityToMate,
                        new ReproductiveData
                        {
                            matingDuration = rabbitMatingDuration,
                            mateStartTime = mateBioStatsData.age,
                            reproductiveUrge = rabbitReproductiveUrge,
                            reproductiveUrgeIncrease = rabbitReproductiveUrgeIncreaseFemale,
                            defaultRepoductiveIncrease = rabbitReproductiveUrgeIncreaseFemale,
                            matingThreshold = rabbitMatingThreshold,

                            pregnant = rabbitPregnant,
                            birthDuration = rabbitBirthDuration,
                            babiesBorn = mateBabiesBorn,
                            birthStartTime = rabbitBirthStartTime,
                            currentLitterSize = mateCurrentLitterSize,
                            litterSizeMin = rabbitLitterSizeMin,
                            litterSizeMax = rabbitLitterSizeMax,
                            litterSizeAve = rabbitLitterSizeAve,
                            pregnancyLengthBase = rabbitPregnancyLengthBase,
                            pregnancyLengthModifier = matePregnancyLengthModifier,
                            pregnancyStartTime = rabbitPregnancyStartTime
                        }
                    );
                }
            }
        }).ScheduleParallel();
    }
}