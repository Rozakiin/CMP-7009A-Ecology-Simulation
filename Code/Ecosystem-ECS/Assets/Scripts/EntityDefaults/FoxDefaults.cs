using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public static class FoxDefaults
{

    //Age
    public static float age = 0f;
    public static float ageIncrease = 1f;
    public static float ageMax = 800f;
    public static BioStatsData.AgeGroup ageGroup = BioStatsData.AgeGroup.Young;
    public static float adultEntryTimer = 10f;
    public static float oldEntryTimer = 60f;


    //Edible
    public static float nutritionalValue = 10f;
    public static bool canBeEaten = false;
    public static float nutritionalValueMultiplier = 1f;
    public static EdibleData.FoodType foodType = EdibleData.FoodType.Meat;


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

    public static BasicNeedsData.Diet diet = BasicNeedsData.Diet.Carnivore;
    //Thirst
    public static float thirst = 0f;
    public static float thirstMax = 100f;
    public static float thirstyThreshold = 10f;
    public static float thirstIncrease = 0.5f;
    public static float drinkingSpeed = 2f;



    //Mate
    public static float mateStartTime = 0f;
    public static float matingDuration = 5f;
    public static float reproductiveUrge = 0f;
    public static float reproductiveUrgeIncreaseMale = 0.3f;
    public static float reproductiveUrgeIncreaseFemale = 0f;
    public static float matingThreshold = 50f;

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


    //Movement
    public static float moveSpeed = 35f;
    public static float rotationSpeed = 10f;
    public static float moveMultiplier = 1f;
    public static float pregnancyMoveMultiplier = 0.5f;
    public static float originalMoveMultiplier = 1f;
    public static float youngMoveMultiplier = 0.4f;
    public static float adultMoveMultiplier = 1f;
    public static float oldMoveMultiplier = 0.4f;


    //Size
    public static float sizeMultiplier = 1f;
    public static float scaleMale = 3f;
    public static float scaleFemale = 2f;
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
    public static float touchRadius = 1f;
    public static float sightRadius = 20f;

    //lookingEntityData
    public static Entity predatorEntity = Entity.Null;
    public static int predatorEntityCount = 0;
    public static Entity entityToEat = Entity.Null;
    public static int edibleEntityCount = 0;
    public static Entity entityToDrink = Entity.Null;
    public static int waterEntityCount = 0;
    public static Entity entityToMate = Entity.Null;
    public static int mateEntityCount = 0;

    //ColliderTypeData
    public static int ColliderTypeNumber = 1;
}
