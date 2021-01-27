using System;
using Unity.Entities;

namespace Components
{
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
        public ColliderType Collider;
    }
}
