using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class PathFollowSystem : SystemBase
    {
        /*
         * makes entity follow the path stored in PathPositionDataBuffer,
         * entity rotates towards the target node
         */
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((
                DynamicBuffer<PathPositionData> pathPositionDataBuffer,
                ref Translation translation,
                ref Rotation rotation,
                ref PathFollowData pathFollowData,
                in MovementData movementData,
                in TargetData targetData
            ) =>
            {
                // if currently following a path
                if (pathFollowData.PathIndex >= 0)
                {
                    //get the world position of next path node to follow
                    var targetPosition = pathPositionDataBuffer[pathFollowData.PathIndex].Position;

                    //calc the direction to move
                    var moveDir = math.normalizesafe(targetPosition - translation.Value);
                    var rotationStep = movementData.RotationSpeed * deltaTime; // to be used to smoothly change rotation
                    var movementStep = movementData.MoveSpeed * deltaTime; // to be used to smoothly change movement

                    var distanceToTarget = math.distance(translation.Value, targetPosition);
                    var distanceToTargetAfterMoving = math.distance(translation.Value + moveDir * movementStep, targetPosition);

                    // if you have moved further away from target (ie overshot target)
                    if (distanceToTarget <= distanceToTargetAfterMoving)
                    {
                        //look at and move to targetPosition
                        rotation.Value = quaternion.LookRotationSafe(moveDir, math.up());
                        translation.Value = targetPosition;
                    }
                    else
                    {
                        //rotate and move towards the targetPosition
                        rotation.Value = math.slerp(math.normalizesafe(rotation.Value), quaternion.LookRotationSafe(moveDir, math.up()), rotationStep);
                        translation.Value += moveDir * movementStep;
                    }

                    //If at the targetPosition, target the next waypoint
                    if (distanceToTarget <= targetData.TouchRadius)
                        pathFollowData.PathIndex--;
                }
            }).ScheduleParallel();
        }
    }
}