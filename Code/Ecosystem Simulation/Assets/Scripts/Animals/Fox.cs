﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{
	protected override float maxLifeExpectancy   // overriding property
    {
        get
        {
            return maxLifeExpectancy;
        }
        set
        {
        }
    }

    protected override float maxBabyNumber   // overriding property
    {
        get
        {
            return maxBabyNumber;
        }
        set
        {
        }
    }

    #region Initialisation
    // Start is called before the first frame update
    void Start()
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
