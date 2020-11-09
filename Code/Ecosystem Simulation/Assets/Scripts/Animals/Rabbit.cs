using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
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
    void Awake()//Ran once the program starts
    {
        scene = GameObject.FindWithTag("GameController").GetComponent<Simulation>(); // get reference to Simulation
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        canBeEaten = true;
        moveSpeed = 25f;
        hunger = 0f;
        thirst = 0f;
        startXPos = position.x;
        startZPos = position.z;
        numberOfTurns = 0;
        age = 1;
        baseNutritionalValue = 5;
        reproductiveUrge = 0f;
        sightRadius = 20;
        touchRadius = 1;
        tileSize = scene.GetTileSize();
        state = States.Wandering;
        eatingSpeed = 2f;
        originalMoveSpeed = 25f;
        moveSpeed = originalMoveSpeed;
        matingDuration = 3f;
        pregnancyLength = 5f;
        maxBabyNumber = 13;
        birthDuration = 0.2f;

        scaleMult = (gender == Gender.Female ? 3.7f : 2.7f);                        //transform.localScale is used for making the rabbit bigger -
        transform.localScale = new Vector3(scaleMult, scaleMult, scaleMult);        //the standard one is quite small and barely 

        SetPosition();
        CreateLineRenderer();
        GetLimits();
        SetNutritionalValue();
    }                                                                                       //the standard one is quite small and barely visible
    #endregion

    // Update is called once per frame
    void Update()
    {
        DisableLineRenderer();
        SetPosition();
        hunger += 1 * Time.deltaTime;
        timer += 1 * Time.deltaTime;
        if (pregnant)
        {
            moveSpeed = 0.6f * originalMoveSpeed;
            if (timer - pregnancyStartTime >= pregnancyLength)
            {
                state = States.GivingBirth;
                birthStartTime = timer;
                pregnant = false;
            }
        }


        if (gender == Gender.Male)
        {
            reproductiveUrge += 0.3f * Time.deltaTime;
            //print(reproductiveUrge);
        }

        if (state == States.Wandering)
        {
            WanderAround();
            //Reproductive Urge stronger than Idle and Hunger
            if (reproductiveUrge >= 5)
            {
                state = States.SexuallyActive;
            }
            //Hunger stronger than Idle
            if (hunger >= 10)
            {
                state = States.Hungry;
            }
        }
        else if (state == States.Hungry)
        {
            DisableLineRenderer();
            //Reproductive Urge stronger than Hunger?
            if (reproductiveUrge >= 5)
            {
                state = States.SexuallyActive;
            }

            Edible closestGrass = LookForConsumable("Grass");//Look for closest grass
            if (closestGrass != null)
            {
                float distanceToGrass = Vector3.Distance(transform.position, closestGrass.transform.position);
                // Grass within touching distance
                if (distanceToGrass <= touchRadius)
                {
                    state = States.Eating;
                }
                //Grass within sight radius
                if (distanceToGrass <= sightRadius)
                {
                    edibleObject = closestGrass.GetComponent<Edible>();//set the edible object to the closest grass object
                    target = closestGrass.transform.position;
                    DrawLine(transform.position, target);
                }
            }
            //No grass
            WanderAround();
        }
        else if (state == States.Eating)
        {
            DisableLineRenderer();
            print("Eating");
            //Eat the object (method call instead?)
            if (edibleObject != null)
            {
                edibleObject.Die();
                hunger -= 5;
                scene.CreateGrass();
            }
            //If the grass has been eaten by someone else, go back to being hungry
            else
            {
                state = States.Hungry;
            }

            if (hunger <= 0)
            {
                state = States.Wandering;
            }
        }
        else if (state == States.Thirsty)
        {
            DisableLineRenderer();
            Transform closestWater = FindClosestWater();
        }
        else if (state == States.SexuallyActive)
        {
            DisableLineRenderer();
            Animal closestMate = LookForMate("FemaleRabbit");
            if (closestMate != null)
            {
                float distanceToMate = Vector3.Distance(transform.position, closestMate.transform.position);
                // Mate within touching distance
                if (distanceToMate <= touchRadius)
                {
                    state = States.Mating;
                    mateStartTime = timer;
                    Mate(closestMate);
                }
                //Mate within sight radius
                if (distanceToMate <= sightRadius)
                {
                    target = closestMate.transform.position;
                    DrawLine(transform.position, target);
                }
            }
            //No mate
            WanderAround();
        }
        else if (state == States.Mating)
        {
            //temp code to check pathfinding          
            DisableLineRenderer();
            print("Mating");
            //If mating should end
            if (timer - mateStartTime >= matingDuration)
            {
                if (gender == Gender.Female)
                {
                    pregnancyStartTime = timer;
                    pregnant = true;
                    numberOfBabies = 5;
                    //numberOfBabies = (int)UnityEngine.Random.Range(1, maxBabyNumber);
                }
                reproductiveUrge = 0;
                state = States.Wandering;
            }
        }
        else if (state == States.GivingBirth)
        {
            if (timer - birthStartTime >= birthDuration && babiesBorn < numberOfBabies)
            {
                GiveBirth();
                birthStartTime = timer;
                babiesBorn++;
            }
            if (babiesBorn >= numberOfBabies)
            {
                moveSpeed = originalMoveSpeed;
                state = States.Wandering;
            }
        }
    }
}
