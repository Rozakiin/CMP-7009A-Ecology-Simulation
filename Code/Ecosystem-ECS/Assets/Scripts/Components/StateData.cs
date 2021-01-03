using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct StateData : IComponentData
{
    public enum DeathReason
    {
        Eaten,
        Hunger,
        Thirst,
        Age,
    }
    public DeathReason deathReason;
    public bool beenEaten;

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
    public FlagStates flagState;
    public FlagStates previousFlagState;

    public bool isWandering;
    public bool isHungry;
    public bool isThirsty;
    public bool isEating;
    public bool isDrinking;
    public bool isSexuallyActive;
    public bool isMating;
    public bool isFleeing;
    public bool isDead;
    public bool isPregnant;
    public bool isGivingBirth;
}
