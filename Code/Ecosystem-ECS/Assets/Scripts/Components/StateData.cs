using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct StateData : IComponentData
    {
        public enum DeathReasons
        {
            Eaten,
            Hunger,
            Thirst,
            Age,
        }
        public DeathReasons DeathReason;
        public bool BeenEaten;

        [Flags]
        public enum FlagStates
        {
            None = 0,               // 000000000000
            Wandering = 1,          // 000000000001
            Hungry = 2,             // 000000000010
            Thirsty = 4,            // 000000000100
            Eating = 8,             // 000000001000
            Drinking = 16,          // 000000010000
            SexuallyActive = 32,    // 000000100000
            Mating = 64,            // 000001000000
            Fleeing = 128,          // 000010000000
            Dead = 256,             // 000100000000
            Pregnant = 512,         // 001000000000
            GivingBirth = 1024,     // 010000000000
        }
        public FlagStates FlagStateCurrent;
        public FlagStates FlagStatePrevious;

        public bool IsWandering;
        public bool IsHungry;
        public bool IsThirsty;
        public bool IsEating;
        public bool IsDrinking;
        public bool IsSexuallyActive;
        public bool IsMating;
        public bool IsFleeing;
        public bool IsDead;
        public bool IsPregnant;
        public bool IsGivingBirth;
    }
}
