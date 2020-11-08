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
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        canBeEaten = true;
        originalMoveSpeed = 25f;
        moveSpeed = originalMoveSpeed;
        hunger = 0f;
        thirst = 0f;
        startXPos = position.x;
        startZPos = position.z;
        numberOfTurns = 0;
        age = 1;
        baseNutritionalValue = 5;
        reproductiveUrge = 0f;
        sightRadius = 5;
        tileSize = scene.GetTileSize();
        state = States.Wandering;
        eatingSpeed = 2f;
        matingDuration = 3f;
<<<<<<< Updated upstream
=======
        pregnancyLength = 5f;
        maxBabyNumber = 13;
        birthDuration = 0.2f;
>>>>>>> Stashed changes

        scaleMult = (gender == Gender.Female ? 3.7f : 2.7f);                        //transform.localScale is used for making the rabbit bigger -
        transform.localScale = new Vector3(scaleMult, scaleMult, scaleMult);        //the standard one is quite small and barely 

        SetPosition();
        CreateLineRenderer();
        GetLimits();
        RandomizeDirection();
        SetNutritionalValue();
    }                                                                                       //the standard one is quite small and barely visible

    
    void Awake()//Ran once the program starts
    {
        //scene = GetComponent<Simulation>(); // get reference to Simulation
    }

    // Update is called once per frame
    void Update()
    {
        DisableLineRenderer();
        SetPosition();
        hunger += 1 * Time.deltaTime;
        timer += 1 * Time.deltaTime;
<<<<<<< Updated upstream
=======
        if (pregnant)
        {
            moveSpeed = 0.6f * originalMoveSpeed;
            if(timer - pregnancyStartTime >= pregnancyLength)
            {
                state = States.GivingBirth;
                birthStartTime = timer;
                pregnant = false;
                //GiveBirth();
            }
        }
>>>>>>> Stashed changes

        if (gender == Gender.Male)
        {
            reproductiveUrge += 0.3f * Time.deltaTime;
            //print(reproductiveUrge);
        }
        if(reproductiveUrge >= 5)
        {
            state = States.SexuallyActive;
            if(distanceToTarget <= 3)
            {
                SetState(States.Mating);
                if(GetState() != States.Mating)
                {
                    target.GetComponent<Animal>().SetState(States.Mating);
                    matingTimeStarted = timer;
                }
            }
        }
        else if(hunger >= 5)
        {
            state = States.Hungry;
            if(distanceToTarget <= sightRadius)
            {
                state = States.Eating;
            }
        }
        else if(hunger <= 0)
        {
            state = States.Wandering;
        }

        if (state == States.Wandering)
        {
            //WanderAround();

            //if(reproductiveUrge >= 5)
            //{
            //    state = States.SexuallyActive;
            //}
            //else if (hunger >= 5)
            //{
            //    state = States.Hungry;
            //}
        }
        else if (state == States.Hungry)
        {
            //if(reproductiveUrge >= 5)
            //{
            //    state = States.SexuallyActive;
            //}
            //WanderAround();
            DisableLineRenderer();            
            
            //List<Edible> edibleList = scene.GetGrassList();
            //GameObject grassObject = LookForConsumable(scene.grassContainer, scene.GetGrassList());

            Edible closestGrass = LookForConsumable("Grass");
            edibleObject = closestGrass.GetComponent<Edible>();
            distanceToTarget = Vector3.Distance(position, closestGrass.transform.position);
            target = closestGrass.transform;
            DrawLine(transform.position, target.position);

            //if(distanceToTarget <= sightRadius)
            //{
            //    state = States.Eating;
            //}
        }
        else if (state == States.Eating)
        {
            if (edibleObject != null)
            {
                edibleObject.Die();
                scene.CreateGrass();
                state = States.Wandering;
<<<<<<< Updated upstream
                print("Ate");
                //hunger -= 5;
                if (hunger <= 0)                         //if the rabbit is sated he goes back to wandering around
                {
                    state = States.Wandering;
                }
            }
            else
            {
                state = States.Hungry;
                distanceToTarget = 1000000;
            }        
=======
            }
            //If the grass has been eaten by someone else, go back to being hungry
            else
            {
                state = States.Hungry;
            }         
>>>>>>> Stashed changes
        }
        else if (state == States.Thirsty)
        {
            //WanderAround();
            DisableLineRenderer();
            Transform closestWater = FindClosestWater();
        }
        else if(state == States.SexuallyActive)
        {
<<<<<<< Updated upstream
            //print("sexually active");
            Animal closestMate = LookForMate("FemaleRabbit");
            target = closestMate.transform;
            distanceToTarget = Vector3.Distance(position, target.transform.position);
            DrawLine(position, closestMate.position);
        }
        else if(state == States.Mating)
        {
            print("Mating");
            if (timer - matingTimeStarted >= matingDuration)
            {
                reproductiveUrge = 0;
                SetState(States.Wandering);
                if(gender == Gender.Female)
                {
                    pregnant = true;
                }
=======
            DisableLineRenderer();
            closestMate = LookForMate("FemaleRabbit");
            if (closestMate != null)
            {
                float distanceToMate = Vector3.Distance(transform.position, closestMate.transform.position);
                // Mate within touching distance
                if(distanceToMate <= touchRadius)
                {
                    state = States.Mating;
                    mateStartTime = timer;
                    Mate(closestMate);
                }
                //Mate within sight radius
                if(distanceToMate <= sightRadius)
                {
                    target = closestMate.transform.position;
                    DrawLine(transform.position, target);
                }
            }
            //No mate
            WanderAround();
        }
        else if(state == States.Mating)
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
        else if(state == States.GivingBirth)
        {
            if(timer - birthStartTime >= birthDuration && babiesBorn < numberOfBabies)
            {
                GiveBirth();
                birthStartTime = timer;
                babiesBorn++;
                
            }
            if(babiesBorn >= numberOfBabies)
            {
                state = States.Wandering;
>>>>>>> Stashed changes
            }
        }
    }
}
