using Components;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public class PathRequestSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;


        protected override void OnCreate()
        {
            base.OnCreate();
            ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        /*
         * Adds a path request data component to entities with a target that are not currently moving 
         */
        protected override void OnUpdate()
        {
            var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

            // Makes a path request using the current target in TargetData 
            Entities
                .WithNone<PathFindingRequestData>()
                .ForEach((
                    Entity entity,
                    int entityInQueryIndex,
                    in TargetData targetData,
                    in PathFollowData pathFollowData,
                    in Translation translation
                ) =>
                {
                    //if not following a path and not at target
                    if (pathFollowData.pathIndex < 0 && targetData.atTarget == false)
                    {
                        // make a path finding request
                        ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                        ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = translation.Value, endPosition = targetData.currentTarget });
                    }
                }).ScheduleParallel();

            // Make sure that the ECB system knows about our job
            ecbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}
