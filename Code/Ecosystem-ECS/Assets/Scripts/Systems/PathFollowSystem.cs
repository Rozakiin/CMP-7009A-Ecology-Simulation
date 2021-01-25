using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class PathFollowSystem : SystemBase
    {
        /*
         * makes entity follow the path stored in pathpositiondatabuffer,
         * entity rotates towards the target node
         */
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
                    float rotationStep = movementData.rotationSpeed * deltaTime;// to be used to smoothly change rotation
                    float movementStep = movementData.MoveSpeed * deltaTime;// to be used to smoothly change movement

                    // if you have moved further away from target (ie overshot target)
                    if (math.distance(translation.Value, targetPosition) <= math.distance(translation.Value + moveDir * movementStep, targetPosition))
                    {
                        //look at targetPosition
                        rotation.Value = quaternion.LookRotationSafe(moveDir, math.up());
                        //move to targetposition
                        translation.Value = targetPosition;
                    }
                    else
                    {
                        //rotate towards the targetPosition
                        rotation.Value = math.slerp(math.normalizesafe(rotation.Value), quaternion.LookRotationSafe(moveDir, math.up()), rotationStep);
                        //move towards the targetPosition
                        translation.Value += moveDir * movementStep;
                    }

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
