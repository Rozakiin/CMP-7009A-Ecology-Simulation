using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{
    private static float moveSpeedBase;
    public override float MoveSpeed
    {
        get { return moveSpeedBase*moveMultiplier; }
        protected set { moveSpeedBase = value; }
    }

    protected override int LitterSizeMax   // overriding property
    {
        get { return LitterSizeMax; }
        set {}
    }

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
        return baseNutritionalValue * age;
    }
}
