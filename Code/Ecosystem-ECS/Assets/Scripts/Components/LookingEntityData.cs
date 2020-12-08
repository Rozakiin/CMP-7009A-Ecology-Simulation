using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct LookingEntityData : IComponentData
{
    //public enum EntityType
    //{
    //    Fox,
    //    Rabbit,
    //    Grass,
    //    Water,
    //}
    public Entity predatorEntity; // if this is != null, rabbit go to Fleeing States
    public int predatorEntityCount; // count the number of prefator entity in the sightradius
    public Entity entityToEat;
    public int edibleEntityCount;
    public Entity entityToDrink;
    public int waterEntityCount;
    public Entity entityToMate;
    public int mateEntityCount;
}
