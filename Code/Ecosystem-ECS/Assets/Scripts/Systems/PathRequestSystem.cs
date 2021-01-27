using Components;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public class PathRequestSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecbSystem;


        protected override void OnCreate()
        {
            base.OnCreate();
            _ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        /*
         * Adds a path request data component to entities with a target that are not currently moving 
         */
        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer().ToConcurrent();

            // Makes a path request using the current target in TargetData 
            Entities.WithNone<PathFindingRequestData>().ForEach((
                Entity entity,
                int entityInQueryIndex,
                in TargetData targetData,
                in PathFollowData pathFollowData,
                in Translation translation
            ) =>
            {
                //if not following a path and not at target
                if (pathFollowData.PathIndex < 0 && targetData.AtTarget == false)
                {
                    // make a path finding request
                    ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                    ecb.SetComponent(entityInQueryIndex, entity,
                        new PathFindingRequestData
                        {
                            StartPosition = translation.Value,
                            EndPosition = targetData.Target
                        });
                }
            }).ScheduleParallel();

            // Make sure that the ECB system knows about our job
            _ecbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}