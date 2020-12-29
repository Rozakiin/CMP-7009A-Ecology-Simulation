using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
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
    public float sigma;
    public float mu;

    public int LitterSize
    {
        get { return GaussianDistribution((litterSizeMax-litterSizeMin)/2, litterSizeAve);}
    }


    public float pregnancyStartTime;
    public float pregnancyLengthBase;
    public float pregnancyLengthModifier;
    public float PregnancyLength
    {
        get { return pregnancyLengthBase * pregnancyLengthModifier; }
    }


    // mu is average, max = sigma + mu; min = sigma-mu
    private int GaussianDistribution(float sigma, float mu)
    {

        float x1, x2, w, y1; //, y2;
        System.Random random = new System.Random();
        do
        {
            x1 = 2f * (float)random.NextDouble() - 1f;
            x2 = 2f * (float)random.NextDouble() - 1f;
            w = x1 * x1 + x2 * x2;
        } while (w >= 1f);

        w = Mathf.Sqrt((-2f * Mathf.Log(w)) / w);
        y1 = x1 * w;
        // y2 = x2 * w;
        return (int)((y1 * sigma) + mu);
    }

}
