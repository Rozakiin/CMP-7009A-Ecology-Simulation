using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MovementData : IComponentData
    {
        public float RotationSpeed;
        public float MoveSpeedBase;
        public float MoveMultiplier;
        public float PregnancyMoveMultiplier;
        public float OriginalMoveMultiplier;
        public float YoungMoveMultiplier;
        public float AdultMoveMultiplier;
        public float OldMoveMultiplier;
        public float MoveSpeed
        {
            get { return MoveSpeedBase * MoveMultiplier; }
        }
    }
}
