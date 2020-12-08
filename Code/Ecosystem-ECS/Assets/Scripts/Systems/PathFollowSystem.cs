using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

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
            in MovementData movementData) => 
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
                rotation.Value = quaternion.LookRotationSafe(moveDir, math.up());
                //move towards the targetPosition
                translation.Value += moveDir * movementData.MoveSpeed * deltaTime;

                //If at the targetPosition
                if (math.distance(translation.Value, targetPosition) < .1f)
                {
                    // Next waypoint
                    pathFollowData.pathIndex--;
                }
            }
        }).Schedule();
    }
}
