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
    public static StateData.States state = StateData.States.Wandering;
    public static StateData.States previousState = StateData.States.Wandering;
    public static StateData.DeathReason deathReason = StateData.DeathReason.Eaten;
    public static bool beenEaten = false;

    //ColliderTypeData
    public static int GrassColliderNumber = 3;
    public static int WaterTileColliderNumber = 4;
    public static int GrassTileColliderNumber = 5;
    public static int RockTileColliderNumber = 6;
    public static int SandTileColliderNumber = 7;
}
