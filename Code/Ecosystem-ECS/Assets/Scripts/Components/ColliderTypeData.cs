using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct ColliderTypeData : IComponentData
{
    public int ColliderTypeNumber; // 1 is fox 2 is rabbit 3is grass 4 is water tile 5 is GrassTile, 6 is Rocktile, 7 is sandTile
}
