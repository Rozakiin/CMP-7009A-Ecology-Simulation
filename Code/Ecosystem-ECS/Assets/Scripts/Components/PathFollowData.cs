using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct PathFollowData : IComponentData
    {
        public int PathIndex;    // index in path to follow of world positions
    }
}
