using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MatingSystem : SystemBase
{
    protected override void OnUpdate()
    {

        float deltaTime = Time.DeltaTime;


        Entities.ForEach((
            ref ReproductiveData reproductiveData, 
            ref StateData stateData, 
            in BioStatsData bioStatsData, 
            in Translation translation,
            in TargetData targetData
            ) => {

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

            float distanceToMate = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[reproductiveData.entityToMate].Value);

            //When the male target gets close
            if (bioStatsData.gender == BioStatsData.Gender.Female && distanceToMate < targetData.touchRadius &&
                stateData.state != StateData.States.Mating)
            {
                    reproductiveData.mateStartTime = bioStatsData.age;
                    stateData.state = StateData.States.Mating;
            }

            //If the entityToMate exists and entity is mating
            if (reproductiveData.entityToMate != Entity.Null && stateData.state == StateData.States.Mating)
            {
                    //If the mating has ended, the female becomes pregnant
                    if(bioStatsData.age - reproductiveData.mateStartTime >= reproductiveData.matingDuration)
                    {
                        if (bioStatsData.gender == BioStatsData.Gender.Female)
                        {
                            reproductiveData.pregnancyStartTime = bioStatsData.age;
                            reproductiveData.pregnant = true;
                            reproductiveData.currentLitterSize = reproductiveData.LitterSize;
                        }

                    }
                    reproductiveData.reproductiveUrge = 0;
                    stateData.state = StateData.States.Wandering;
            }
        }).Schedule();
    }
}
