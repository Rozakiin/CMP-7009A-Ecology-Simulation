using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public static class GrassDefaults
{
    //Edible
    public static float nutritionalValue = 10f;
    public static bool canBeEaten = true;
    public static float nutritionalValueMultiplier = 1f;
    public static EdibleData.FoodType foodType = EdibleData.FoodType.Plant;

    //Size
    public static float sizeMultiplier = 1f;
    public static float scaleMale = 1f;
    public static float scaleFemale = 1f;
    public static float youngSizeMultiplier = 0.6f;
    public static float adultSizeMultiplier = 1f;
    public static float oldSizeMultiplier = 0.75f;
    public static float ageSizeMultiplier = youngSizeMultiplier;


    //State
    public static StateData.States state = StateData.States.Wandering;
    public static StateData.States previousState = StateData.States.Wandering;
    public static StateData.DeathReason deathReason = StateData.DeathReason.Eaten;
    public static bool beenEaten = false;
}
