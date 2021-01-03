using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct BioStatsData : IComponentData
{
    //Age data
    public float age;
    public float ageIncrease;
    public float ageMax;
    public AgeGroup ageGroup;

    public float adultEntryTimer;
    public float oldEntryTimer;

    public enum AgeGroup
    {
        Young, Adult, Old
    }

    //Gender data
    public enum Gender
    {
        Male, Female
    }
    public Gender gender;
}
