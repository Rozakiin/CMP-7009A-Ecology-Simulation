using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    private Unit pathfindingUnit; // reference to the Animals pathfindingUnit object

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
        sightRadius = 20;
        touchRadius = 1;
        tileSize = scene.GetTileSize();
        state = States.Wandering;
        eatingSpeed = 2f;

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
        pathfindingUnit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        DisableLineRenderer();
        SetPosition();
        hunger += 1 * Time.deltaTime;

        if (gender == Gender.Male)
        {
            reproductiveUrge += 0.3f * Time.deltaTime;
            //print(reproductiveUrge);
        }

        if (state == States.Wandering)
        {
            WanderAround();
            //Reproductive Urge stronger than Idle and Hunger
            if(reproductiveUrge >= 5)
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
            if(reproductiveUrge >= 5)
            {
                state = States.SexuallyActive;
            }
            
            Edible closestGrass = LookForConsumable("Grass");//Look for closest grass
            if (closestGrass != null)
            {
                float distanceToGrass = Vector3.Distance(transform.position, closestGrass.transform.position);
                // Grass within touching distance
                if(distanceToGrass <= touchRadius)
                {
                    state = States.Eating;
                }
                //Grass within sight radius
                if(distanceToGrass <= sightRadius)
                {
                    edibleObject = closestGrass.GetComponent<Edible>();//set the edible object to the closest grass object
                    target = closestGrass.transform;
                    DrawLine(transform.position, target.position);
                }
            }
            //No grass nearby
            WanderAround();
        }
        else if (state == States.Eating)
        {
            DisableLineRenderer();
            print("Eating");
            //Eat the object (method call instead?)
            if(edibleObject != null)
            {
                edibleObject.Die();
                hunger -= 5;
                scene.CreateGrass();
            }

            state = States.Wandering;
        }
        else if (state == States.Thirsty)
        {
            DisableLineRenderer();
            Transform closestWater = FindClosestWater();
        }
        else if(state == States.SexuallyActive)
        {
            DisableLineRenderer();
            closestMate = LookForMate("FemaleRabbit");
            if (closestMate != null)
            {
                float distanceToMate = Vector3.Distance(transform.position, closestMate.transform.position);
                // Mate within touching distance
                if(distanceToMate <= touchRadius)
                {
                    state = States.Mating;
                }
                //Mate within sight radius
                if(distanceToMate <= sightRadius)
                {
                    target = closestMate.transform;
                    DrawLine(transform.position, target.position);
                }
            }
            //No mate nearby
            WanderAround();
        }
        else if(state == States.Mating)
        {
            //temp code to check pathfinding
            DisableLineRenderer();
            print("Mating");
            reproductiveUrge = 0;
            state = States.Wandering;
        }
    }

    private void WanderAround()
    {
        if(target == null)
        {
            bool isTargetWalkable = false;
            Vector3 targetWorldPoint;
            //find walkable targetWorldPoint
            while (!isTargetWalkable)
            {
                float randX = UnityEngine.Random.Range(-sightRadius, sightRadius);
                float randZ = UnityEngine.Random.Range(-sightRadius, sightRadius);
                // random point within the sight radius of the rabbit
                targetWorldPoint = transform.position + Vector3.right * randX + Vector3.forward * randZ;
                //check targetWorldPoint is walkable
                isTargetWalkable = CheckIfWalkable(targetWorldPoint);
                if (isTargetWalkable)
                {
                    //set target position to transform of a new gameobject
                    //bug - eventual memory overflow(new GameObject arent being deleted, should change target to be an object)
                    GameObject targetObject = new GameObject();
                    target = targetObject.transform;
                    //set target position to the targetWorldPoint
                    target.position = targetWorldPoint;
                }
            }
        }
    }

    //Method to check if a given position is a walkable tile(could be extended to check if the whole path is walkable?)
    //Uses ray hits to check type of tile underneath
    private bool CheckIfWalkable(Vector3 worldPoint)
    {
        Ray ray = new Ray(worldPoint+Vector3.up*50, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) 
        {
            //If not hits a wall
            if (hit.collider.gameObject.layer != 8)
            {
                return true;
            }
        }
        return false;
    }
}
