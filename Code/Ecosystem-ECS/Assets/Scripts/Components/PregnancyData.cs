using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PregnancyData : IComponentData
{
    // Add fields to your component here. Remember that:
    //
    // * A component itself is for storing data and doesn't 'do' anything.
    //
    // * To act on the data, you will need a System.
    //
    // * Data in a component must be blittable, which means a component can
    //   only contain fields which are primitive types or other blittable
    //   structs; they cannot contain references to classes.
    //
    // * You should focus on the data structure that makes the most sense
    //   for runtime use here. Authoring Components will be used for 
    //   authoring the data in the Editor.

    public bool pregnant;
    public float birthDuration;   //How long between babies being born
    public float birthStartTime;
    public int babiesBorn;      //How many she has given birth to
    public int litterSize;  //How many the female is carrying right now
    public int litterSizeMax;
    public float pregnancyStartTime;
    public float pregnancyLengthBase;
    public float pregnancyLengthModifier;
    public float PregnancyLength
    {
        get { return pregnancyLengthBase * pregnancyLengthModifier; }
    }
}
