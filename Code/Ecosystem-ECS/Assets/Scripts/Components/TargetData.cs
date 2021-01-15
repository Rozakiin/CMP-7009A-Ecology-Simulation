using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct TargetData : IComponentData
    {
        public float3 currentTarget;
        public float3 oldTarget;
        public bool atTarget;

        public float sightRadius;
        public float touchRadius;
        public float mateRadius;


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
}
