using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PathFindingTestSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate()
    {
        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     float deltaTime = Time.DeltaTime;

        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.
        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        if (Input.GetMouseButtonDown(0))
        {
            //TESTING set a path to find on click of mouse
            Entities.WithAll<isRabbitTag>().ForEach((
            Entity entity, int entityInQueryIndex) =>
            {

                ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                ecb.SetComponent<PathFindingRequestData>(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = new float3(-95, 0, -95), endPosition = new float3(95, 0, -75) });
            }).WithoutBurst().Schedule();
        }
        // Make sure that the ECB system knows about our job
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
