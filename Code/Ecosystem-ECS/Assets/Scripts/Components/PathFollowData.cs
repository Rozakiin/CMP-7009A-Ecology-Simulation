using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct PathFollowData : IComponentData
{
    public int pathIndex;    // index in path to follow of world positions
}
