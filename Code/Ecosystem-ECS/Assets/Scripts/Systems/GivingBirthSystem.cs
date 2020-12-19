using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class GivingBirthSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {

        var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();
        Entities.ForEach((
            Entity e,
            int entityInQueryIndex,
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
                        ////give birth
                        //Entity newEntity = e;
                        //EntityManager.Instantiate(e);
                        
                        //ArchetypeChunkEntityType archetype =  this.GetArchetypeChunkEntityType();
                        Entity newEntity = ecb.Instantiate(entityInQueryIndex, e);
                        /*ecb.SetComponent(entityInqueryIndex, newEntity, new StateData
                        {
                            //
                        })*/
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
