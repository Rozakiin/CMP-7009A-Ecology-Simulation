using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PathFindingRequestData : IComponentData
{
    public float3 startPosition;
    public float3 endPosition;
}
