using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct TargetData : IComponentData
    {
        public float3 Target;
        public float3 TargetOld;
        public bool AtTarget;

        public float SightRadius;
        public float TouchRadius;
        public float MateRadius;


        // from lookingEntityData
        public Entity PredatorEntity;
        public Entity EntityToEat;
        public Entity EntityToDrink;
        public Entity EntityToMate;

        // just for test, #### don't forget to delete in the end;
        public float ShortestDistanceToEdible;
        public float ShortestDistanceToWater;
        public float ShortestDistanceToPredator;
        public float ShortestDistanceToMate;
    }
}
