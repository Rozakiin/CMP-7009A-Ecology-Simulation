using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct PathFollowData : IComponentData
{
    public int pathIndex;    // index in path to follow of world positions
}
