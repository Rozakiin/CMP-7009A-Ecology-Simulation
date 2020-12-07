using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

//Use this class to define all the default values for components related to Rabbit Entities
public static class RabbitDefaults
{
    //Age
    public static float age = 0f;
    public static float ageIncrease = 1f;
    public static float ageMax = 600f;
    public static BioStatsData.AgeGroup ageGroup = BioStatsData.AgeGroup.Young;
    public static float adultEntryTimer = 10f;
    public static float oldEntryTimer = 60f;

    //Drinkable


    //Edible
    public static float nutritionalValue = 10f;
    public static bool canBeEaten = true;
    public static float nutritionalValueMultiplier = 1f;
    public static EdibleData.FoodType foodType = EdibleData.FoodType.Meat;

    //Gender


    //Hunger
    public static float hunger = 0f;
    public static float hungerMax = 100f;
    public static float hungryThreshold = 20f;
    public static float hungerIncrease = 0.5f;
    public static float pregnancyHungerIncrease = 0.7f;
    public static float youngHungerIncrease = 0.3f;
    public static float adultHungerIncrease = 1f;
    public static float oldHungerIncrease = 0.5f;
    public static float eatingSpeed = 1f;
    public static Entity entityToEat = Entity.Null;
    public static HungerData.Diet diet = HungerData.Diet.Herbivore;

    //Mate
    public static float mateStartTime = 0f;
    public static float matingDuration = 5f;
    public static float reproductiveUrge = 0f;
    public static float defaultReproductiveIncrease = 0.3f;
    public static float reproductiveUrgeIncrease = defaultReproductiveIncrease;
    public static float matingThreshold = 50f;
    public static Entity entityToMate = Entity.Null;

    //Movement
    public static float moveSpeed = 25f;
    public static float rotationSpeed = 10f;
    public static float moveMultiplier = 1f;
    public static float pregnancyMoveMultiplier = 0.5f;
    public static float originalMoveMultiplier = 1f;
    public static float youngMoveMultiplier = 0.4f;
    public static float adultMoveMultiplier = 1f;
    public static float oldMoveMultiplier = 0.4f;

    //Pregnancy
    public static float pregnancyStartTime = 0f;
    public static bool pregnant = false;
    public static int babiesBorn = 0;
    public static float birthStartTime = 0f;
    public static int currentLitterSize = 0;
    public static float pregnancyLengthModifier = 1f;
    public static float pregnancyLength = 10f; 
    public static float birthDuration = 10f;
    public static int litterSizeMin = 1;
    public static int litterSizeMax = 13;
    public static int litterSizeAve = 7;

    //Size
    public static float sizeMultiplier = 1f;
    public static float scaleMale = 2f;
    public static float scaleFemale = 3f;
    public static float youngSizeMultiplier = 0.6f;
    public static float adultSizeMultiplier = 1f;
    public static float oldSizeMultiplier = 0.75f;
    public static float ageSizeMultiplier = youngSizeMultiplier;

    //State
    public static StateData.States state = StateData.States.Wandering;
    public static StateData.States previousState = StateData.States.Wandering;
    public static StateData.DeathReason deathReason = StateData.DeathReason.Eaten;
    public static bool beenEaten = false;

    //Target


    //TerrainType


    //Thirst
    public static float thirst = 0f;
    public static float thirstMax = 100f;
    public static float thirstyThreshold = 10f;
    public static float thirstIncrease = 0.5f;
    public static float drinkingSpeed = 2f;
    public static Entity entityToDrink = Entity.Null;

    //Vision
    public static float touchRadius = 1f;
    public static float sightRadius = 20f;
}
