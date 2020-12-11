using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class GivingBirthSystem : SystemBase
{
    protected override void OnUpdate()
    {
   
        Entities.ForEach((
            Entity e,
            ref ReproductiveData reproductiveData,
            ref StateData stateData,
            ref MovementData movementData,
            in BioStatsData bioStatsData
            ) => {
            
                if(stateData.state == StateData.States.GivingBirth)
                {
                    if(bioStatsData.age - reproductiveData.birthStartTime >= reproductiveData.birthDuration &&
                    reproductiveData.babiesBorn < reproductiveData.currentLitterSize)
                    {
                        //give birth
                        Entity newEntity = e;
                        EntityManager.Instantiate(e);
                        reproductiveData.birthStartTime = bioStatsData.age;
                        reproductiveData.babiesBorn++;
                    }
                    if(reproductiveData.babiesBorn >= reproductiveData.currentLitterSize)
                    {
                        reproductiveData.pregnant = false;
                        movementData.moveMultiplier = movementData.originalMoveMultiplier;
                        stateData.state = StateData.States.Wandering;
                    }
                }
        }).Schedule();
    }
}
