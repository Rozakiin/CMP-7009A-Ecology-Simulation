using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]

public struct DrinkableData : IComponentData
{
    public float Value;
    public bool canBeDrunk;
}
