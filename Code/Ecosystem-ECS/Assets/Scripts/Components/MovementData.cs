using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
    public float rotationSpeed;
    public float moveSpeedBase;
    public float moveMultiplier;
    public float pregnancyMoveMultiplier;
    public float originalMoveMultiplier;
    public float youngMoveMultiplier;
    public float adultMoveMultiplier;
    public float oldMoveMultiplier;
    public float MoveSpeed
    {
        get { return moveSpeedBase * moveMultiplier; }
    }
}
