using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [Serializable]
    public struct PathFindingRequestData : IComponentData
    {
        public float3 startPosition;
        public float3 endPosition;
    }
}
