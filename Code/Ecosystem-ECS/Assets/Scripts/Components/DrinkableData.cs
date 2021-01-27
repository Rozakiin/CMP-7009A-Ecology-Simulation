using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct DrinkableData : IComponentData
    {
        public float Value;
        public bool CanBeDrunk;
    }
}