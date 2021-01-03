using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]

public struct TerrainTypeData : IComponentData
{
    public enum TerrainType
    {
        Water, Grass, Sand, Rock
    }
    public TerrainType terrainType;
    public int terrainPenalty;
    public bool isWalkable;
}
