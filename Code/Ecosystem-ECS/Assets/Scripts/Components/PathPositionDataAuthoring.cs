using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

//[DisallowMultipleComponent]
//[RequiresEntityConversion]
public class PathPositionDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<PathPositionData>(entity);
    }
}
