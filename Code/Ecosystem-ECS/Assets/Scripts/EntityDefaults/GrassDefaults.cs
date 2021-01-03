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
    public static float scale = 1f;


    //State
    public static StateData.FlagStates flagState = StateData.FlagStates.None;
    public static StateData.FlagStates previousFlagState = StateData.FlagStates.None;
    public static StateData.DeathReason deathReason = StateData.DeathReason.Eaten;
    public static bool beenEaten = false;

    //ColliderTypeData
    public static ColliderTypeData.ColliderType GrassColliderType = ColliderTypeData.ColliderType.Grass;
}
