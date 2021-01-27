using System;
using Unity.Entities;

namespace Components
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct BioStatsData : IComponentData
    {
        //Age data
        public float Age;
        public float AgeIncrease;
        public float AgeMax;

        public float AdultEntryTimer;
        public float OldEntryTimer;

        public enum AgeGroups
        {
            Young, Adult, Old
        }
        public AgeGroups AgeGroup;

        //Gender data
        public enum Genders
        {
            Male, Female
        }
        public Genders Gender;
    }
}
