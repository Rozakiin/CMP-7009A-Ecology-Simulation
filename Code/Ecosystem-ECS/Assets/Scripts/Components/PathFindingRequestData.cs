using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [Serializable]
    public struct PathFindingRequestData : IComponentData
    {
        public float3 StartPosition;
        public float3 EndPosition;
    }
}
