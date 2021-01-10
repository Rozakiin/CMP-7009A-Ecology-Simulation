using System;
using Unity.Entities;

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
