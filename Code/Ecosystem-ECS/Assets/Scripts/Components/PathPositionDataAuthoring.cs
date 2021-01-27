using Unity.Entities;
using UnityEngine;

//[DisallowMultipleComponent]
//[RequiresEntityConversion]
namespace Components
{
    public class PathPositionDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddBuffer<PathPositionData>(entity);
        }
    }
}