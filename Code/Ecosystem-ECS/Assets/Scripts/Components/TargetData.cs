using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct TargetData : IComponentData
{
    // Add fields to your component here. Remember that:
    //
    // * A component itself is for storing data and doesn't 'do' anything.
    //
    // * To act on the data, you will need a System.
    //
    // * Data in a component must be blittable, which means a component can
    //   only contain fields which are primitive types or other blittable
    //   structs; they cannot contain references to classes.
    //
    // * You should focus on the data structure that makes the most sense
    //   for runtime use here. Authoring Components will be used for 
    //   authoring the data in the Editor.

    public float3 currentTarget;
    public float3 oldTarget;
    public bool atTarget;

    public float sightRadius;
    public float touchRadius;


    // from lookingEntityData
    public Entity predatorEntity; // if this is != null, rabbit go to Fleeing States 
    public Entity entityToEat;
    public Entity entityToDrink;
    public Entity entityToMate;

    // just for test, #### don't forget to delete in the end;
    public float shortestToEdibleDistance; // set initial distance to 100, must more than sight radius
    public float shortestToWaterDistance;
    public float shortestToPredatorDistance;
    public float shortestToMateDistance;
}
