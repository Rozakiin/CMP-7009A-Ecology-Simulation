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

    protected override float babyNumber   // overriding property
    {
        get
        {
            return babyNumber;
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
        moveSpeed = 25f;
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
        }
        else if (state == States.Thirsty)
        {
            //WanderAround();
            DisableLineRenderer();
            Transform closestWater = FindClosestWater();
        }
        else if(state == States.SexuallyActive)
        {
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
            }
        }
    }
}
