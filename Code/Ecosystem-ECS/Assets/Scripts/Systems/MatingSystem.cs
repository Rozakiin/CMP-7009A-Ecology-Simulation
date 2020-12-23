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


        Entities.ForEach((ref ReproductiveData reproductiveData, in TargetData targetData,in StateData stateData, in BioStatsData bioStatsData) => {

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

            


            //If the entityToMate exists and entity is mating
            if (targetData.entityToMate != Entity.Null && stateData.state == StateData.States.Mating)
            {

            }
        }).Schedule();
    }
}
