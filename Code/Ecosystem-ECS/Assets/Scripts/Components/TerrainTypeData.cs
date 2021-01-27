using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]

    public struct TerrainTypeData : IComponentData
    {
        public enum TerrainType
        {
            Water, Grass, Sand, Rock
        }
        public TerrainType Terrain;
        public int TerrainPenalty;
        public bool IsWalkable;
    }
}
