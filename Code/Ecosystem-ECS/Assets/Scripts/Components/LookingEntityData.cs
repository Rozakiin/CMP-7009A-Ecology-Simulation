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
    public int predatorEntityCount; // count the number of prefator entity in the sightradius, more comment in looking entity system
    public Entity entityToEat;
    public int edibleEntityCount;
    public Entity entityToDrink;
    public int waterEntityCount;
    public Entity entityToMate;
    public int mateEntityCount;
    public float shortestToEdibleDistance; // set initial distance to 100, must more than sight radius
    public float shortestToWaterDistance;
    public float shortestToPredatorDistance;
}
