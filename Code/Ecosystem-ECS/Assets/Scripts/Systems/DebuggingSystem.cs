using Components;
using Unity.Entities;
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
            if (SimulationManager.Instance.isDebugEnabled)
            {
                Entities.ForEach((in Translation translation, in TargetData targetData) =>
                {
                    //Debugging: draw a line to target
                    Debug.DrawLine(translation.Value, targetData.currentTarget);

                    if (HasComponent<Translation>(targetData.entityToDrink))
                    {
                        Debug.DrawLine(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToDrink].Value, Color.blue);
                    }
                    if (HasComponent<Translation>(targetData.entityToEat))
                    {
                        Debug.DrawLine(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToEat].Value, Color.green);
                    }
                    if (HasComponent<Translation>(targetData.entityToMate))
                    {
                        Debug.DrawLine(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.entityToMate].Value, Color.magenta);
                    }
                    if (HasComponent<Translation>(targetData.predatorEntity))
                    {
                        Debug.DrawLine(translation.Value, GetComponentDataFromEntity<Translation>(true)[targetData.predatorEntity].Value, Color.red);
                    }
                }).Run();


                Entities.ForEach((DynamicBuffer<PathPositionData> pathPositionDataBuffer, in PathFollowData pathFollowData) =>
                {
                    //Debugging: draw a line to each node of path
                    if (pathFollowData.pathIndex >= 0)
                    {
                        for (int i = pathPositionDataBuffer.Length - 1; i > 0; i--)
                        {
                            Debug.DrawLine(pathPositionDataBuffer[i].position, pathPositionDataBuffer[i - 1].position, Color.cyan);
                        }
                    }
                }).Run();
            }
        }
    }
}
