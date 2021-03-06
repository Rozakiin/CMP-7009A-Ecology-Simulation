﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{
    #region Defaults
    public class DefaultValues
    {
        public static readonly float moveSpeed = 25f;
        public static readonly float scaleMale = 3f;
        public static readonly float scaleFemale = 3f;
        public static readonly float hungerMax = 100f;
        public static readonly float thirstMax = 100f;
        public static readonly float ageMax = 800f;
        public static readonly float pregnancyLength = 20f;
        public static readonly int litterSizeMax;
    }
    #endregion

    #region Properties
    private static float moveSpeedBase = DefaultValues.moveSpeed;
    public override float MoveSpeed
    {
        get { return moveSpeedBase*moveMultiplier; }
        protected set { moveSpeedBase = value; }
    }

    private static float hungerMax = DefaultValues.hungerMax;
    public override float HungerMax
    {
        get { return hungerMax; }
        protected set { hungerMax = value; }
    }

    private static float thirstMax = DefaultValues.thirstMax;
    public override float ThirstMax
    {
        get { return thirstMax; }
        protected set { thirstMax = value; }
    }

    private static float ageMax = DefaultValues.ageMax;
    public override float AgeMax
    {
        get { return ageMax; }
        protected set { ageMax = value; }
    }

    private static int litterSizeMax = DefaultValues.litterSizeMax;
    public override int LitterSizeMax
    {
        get { return litterSizeMax; }
        protected set { litterSizeMax = value; }
    }

    private static float pregnancyLengthBase = DefaultValues.pregnancyLength;
    public override float PregnancyLength
    {
        get { return pregnancyLengthBase * pregnancyLengthModifier; }
        protected set { pregnancyLengthBase = value; }
    }

    private static float scaleFemaleBase = DefaultValues.scaleFemale;
    protected override float ScaleFemale
    {
        get { return scaleFemaleBase * scaleMultiplier; }
        set { scaleFemaleBase = value; }
    }

    private static float scaleMaleBase = DefaultValues.scaleMale;
    protected override float ScaleMale
    {
        get { return scaleMaleBase * scaleMultiplier; }
        set { scaleMaleBase = value; }
    }

    public override float MatingDuration { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override float BirthDuration { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override int LitterSizeMin { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override int LitterSizeAve { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override float SightRadius { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    public override float TouchRadius { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }
    #endregion

    #region Initialisation
    // Start is called before the first frame update
    new void Start()
    {
        
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }

    public override int GetNutritionalValue()
    {
        return baseNutritionalValue * (int)age;
    }
}
