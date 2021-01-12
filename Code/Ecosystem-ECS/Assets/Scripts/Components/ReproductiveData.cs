using System;
using Unity.Entities;
using UnityEngine;

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

    public int LitterSize
    {
        get { return UtilTools.ComponentTools.GaussianDistribution((litterSizeMax - litterSizeMin) / 2, litterSizeAve, birthStartTime); }
    }


    public float pregnancyStartTime;
    public float pregnancyLengthBase;
    public float pregnancyLengthModifier;
    public float PregnancyLength
    {
        get { return pregnancyLengthBase * pregnancyLengthModifier; }
    }
}
