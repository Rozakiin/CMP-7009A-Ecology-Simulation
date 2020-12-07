using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct ReproductiveData : IComponentData
{
    //Mate
    public float matingDuration;
    public float mateStartTime;
    public float reproductiveUrge;
    public float defaultRepoductiveIncrease;
    public float reproductiveUrgeIncrease;
    public float matingThreshold;
    public Entity entityToMate;

    //Pregnancy
    public bool pregnant;
    public float birthDuration;   //How long between babies being born
    public float birthStartTime;
    public int babiesBorn;      //How many she has given birth to
    public int litterSizeMin;
    public int litterSizeMax;
    public int litterSizeAve;
    //How many the female is carrying right now
    public int currentLitterSize;
    //TODO use gausian distribution to calc LitterSize
    public int LitterSize
    {
        get { return litterSizeAve; }
    }


    public float pregnancyStartTime;
    public float pregnancyLengthBase;
    public float pregnancyLengthModifier;
    public float PregnancyLength
    {
        get { return pregnancyLengthBase * pregnancyLengthModifier; }
    }
}
