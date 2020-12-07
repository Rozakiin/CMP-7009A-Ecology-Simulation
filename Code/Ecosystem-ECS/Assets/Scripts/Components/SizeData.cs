﻿using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct SizeData : IComponentData
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

    public float size;
    public float sizeMultiplier;
    public float ageSizeMultiplier;
    public float youngSizeMultiplier;
    public float adultSizeMultiplier;
    public float oldSizeMultiplier;
    public float Size
    {
        get { return size * sizeMultiplier * ageSizeMultiplier; }
    }
}