using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct ColliderTypeData : IComponentData
{
    public enum ColliderType
    {
        Fox,
        Rabbit,
        Grass,
        Water
    }
    public ColliderType colliderType;
}
