using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class HungerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     float deltaTime = Time.DeltaTime;

        float deltaTime = Time.DeltaTime;


        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.


        Entities.ForEach((ref HungerData hungerData, in StateData stateData) =>
        {
            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as 'in', which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //     translation.Value += math.mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;

            // Increase hunger
            hungerData.hunger += hungerData.hungerIncrease * deltaTime;

            //If the entityToEat exists and entity is eating, set entityToEat state to dead and eaten.Decrease hunger by nutritionvalue of entity
            if (hungerData.entityToEat != Entity.Null && stateData.state == StateData.States.Eating)
            {
                if (HasComponent<EdibleData>(hungerData.entityToEat))
                {
                    hungerData.hunger -= GetComponentDataFromEntity<EdibleData>(true)[hungerData.entityToEat].NutritionalValue * hungerData.eatingSpeed * deltaTime; //gets nutritionalValue from entityToEat (GetComponentDataFromEntity gives array like access)
                    //TODO set beenEaten to true in entityToEat - need to use entitybuffersystem
                    //    if (entityManager.HasComponent<StateData>(hungerData.entityToEat))
                    //    {
                    //        entityManager.SetComponentData<StateData>(hungerData.entityToEat, new StateData { state = StateData.States.Dead, deathReason = StateData.DeathReason.Eaten }); //might have issues as setting data in another entity?
                    //    }
                }
            }
        }).ScheduleParallel();
    }
}
