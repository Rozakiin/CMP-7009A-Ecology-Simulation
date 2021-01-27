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
            Age
        }

        public DeathReasons DeathReason;
        public bool BeenEaten;

        [Flags]
        public enum FlagStates
        {
            None = 0b_0000_0000_0000,
            Wandering = 0b_0000_0000_0001,
            Hungry = 0b_0000_0000_0010,
            Thirsty = 0b_0000_0000_0100,
            Eating = 0b_0000_0000_1000,
            Drinking = 0b_0000_0001_0000,
            SexuallyActive = 0b_0000_0010_0000,
            Mating = 0b_0000_0100_0000,
            Fleeing = 0b_0000_1000_0000,
            Dead = 0b_0001_0000_0000,
            Pregnant = 0b_0010_0000_0000,
            GivingBirth = 0b_0100_0000_0000
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