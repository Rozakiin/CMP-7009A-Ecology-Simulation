using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    /* 
     * Used for any visual debugging or logging.
     * can be enabled and disabled in the inspector so it only runs when needed
     */
    public class DebuggingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            //catch to only run when enabled
            if (!SimulationManager.Instance.IsDebugEnabled) return;

            Entities.ForEach((
                in Translation translation,
                in TargetData targetData
            ) =>
            {
                //Debugging: draw a line to target
                Debug.DrawLine(translation.Value, targetData.Target);

                if (HasComponent<Translation>(targetData.EntityToDrink))
                    Debug.DrawLine(translation.Value,
                        GetComponentDataFromEntity<Translation>(true)[targetData.EntityToDrink].Value, Color.blue);

                if (HasComponent<Translation>(targetData.EntityToEat))
                    Debug.DrawLine(translation.Value,
                        GetComponentDataFromEntity<Translation>(true)[targetData.EntityToEat].Value, Color.green);

                if (HasComponent<Translation>(targetData.EntityToMate))
                    Debug.DrawLine(translation.Value,
                        GetComponentDataFromEntity<Translation>(true)[targetData.EntityToMate].Value, Color.magenta);

                if (HasComponent<Translation>(targetData.PredatorEntity))
                    Debug.DrawLine(translation.Value,
                        GetComponentDataFromEntity<Translation>(true)[targetData.PredatorEntity].Value, Color.red);
            }).Run();

            Entities.ForEach((
                in Translation translation,
                in TargetData targetData) =>
            {
                //Debugging: draw aabb 
                var min = translation.Value + new float3(-targetData.SightRadius, 0, -targetData.SightRadius);
                var max = translation.Value + new float3(targetData.SightRadius, 0, targetData.SightRadius);
                Debug.DrawLine(min, new float3(min.x, min.y, max.z));
                Debug.DrawLine(min, new float3(max.x, min.y, min.z));
                Debug.DrawLine(max, new float3(min.x, min.y, max.z));
                Debug.DrawLine(max, new float3(max.x, min.y, min.z));
            }).Run();

            Entities.ForEach((
                in DynamicBuffer<PathPositionData> pathPositionDataBuffer,
                in PathFollowData pathFollowData
            ) =>
            {
                //Debugging: draw a line to each node of path
                if (pathFollowData.PathIndex >= 0)
                    for (var i = pathPositionDataBuffer.Length - 1; i > 0; i--)
                        Debug.DrawLine(pathPositionDataBuffer[i].Position, pathPositionDataBuffer[i - 1].Position,
                            Color.cyan);
            }).Run();
        }
    }
}