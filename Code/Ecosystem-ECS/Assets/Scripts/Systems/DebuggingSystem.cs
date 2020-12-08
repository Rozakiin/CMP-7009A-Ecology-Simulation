using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class DebuggingSystem : SystemBase
{
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
        
        
        
        Entities.ForEach((in Translation translation, in TargetData targetData) => {

            //Debugging: draw a line to target
            if (targetData.atTarget == false)
            {
                Debug.DrawLine(translation.Value, targetData.currentTarget);
            }
        }).Run();


        Entities.ForEach((DynamicBuffer<PathPositionData> pathPositionDataBuffer, PathFollowData pathFollowData) => {

            //Debugging: draw a line to each node of path
            if (pathFollowData.pathIndex >= 0)
            {
                //Debug.Log($"{pathPositionDataBuffer[pathFollowData.pathIndex].position}");
                for (int i = pathPositionDataBuffer.Length-1; i > 0; i--)
                {
                    Debug.DrawLine(pathPositionDataBuffer[i].position, pathPositionDataBuffer[i - 1].position, Color.red);
                }
            }
        }).Run();
    }
}
