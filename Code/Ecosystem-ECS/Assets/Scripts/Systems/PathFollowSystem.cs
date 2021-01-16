using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class PathFollowSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((
                DynamicBuffer<PathPositionData> pathPositionDataBuffer,
                ref Translation translation,
                ref Rotation rotation,
                ref PathFollowData pathFollowData,
                in MovementData movementData,
                in TargetData targetData) =>
            {
                // if currently following a path
                if (pathFollowData.pathIndex >= 0)
                {
                    //get the world position of next path node to follow
                    float3 targetPosition = pathPositionDataBuffer[pathFollowData.pathIndex].position;

                    //calc the direction to move
                    float3 moveDir = math.normalizesafe(targetPosition - translation.Value);
                    float step = movementData.rotationSpeed * deltaTime;// to be used to smoothly change rotation (not just implemented)

                    //rotate towards the targetPosition
                    rotation.Value = math.slerp(math.normalizesafe(rotation.Value), quaternion.LookRotationSafe(moveDir, math.up()), step);
                    //move towards the targetPosition
                    translation.Value += moveDir * movementData.MoveSpeed * deltaTime;

                    //If at the targetPosition
                    if (math.distance(translation.Value, targetPosition) <= targetData.touchRadius)
                    {
                        // Next waypoint
                        pathFollowData.pathIndex--;
                    }
                }
            }).ScheduleParallel();
        }
    }
}
