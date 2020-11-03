using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    private float tileSize;                                                                 //The size of each tile on the map
    private float leftLimit, upLimit, rightLimit, downLimit;
    private int numberOfTurns;
    private Edible closestGrass;
    private Edible edibleObject;
    private Renderer renderer;
    private float scaleMult;
    private LineRenderer lineRenderer;

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

        if (gender == Gender.Male)
        {
            reproductiveUrge += 0.3f * Time.deltaTime;
            //print(reproductiveUrge);
        }

        if (state == States.Wandering)
        {
            WanderAround();

            if(reproductiveUrge >= 5)
            {
                state = States.SexuallyActive;
            }

            if (hunger >= 10)
            {
                state = States.Hungry;
            }
        }
        else if (state == States.Hungry)
        {
            if(reproductiveUrge >= 5)
            {
                state = States.SexuallyActive;
            }
            //WanderAround();
            DisableLineRenderer();            
            
            //List<Edible> edibleList = scene.GetGrassList();
            //GameObject grassObject = LookForConsumable(scene.grassContainer, scene.GetGrassList());

            closestGrass = LookForConsumable("Grass");
            edibleObject = closestGrass.GetComponent<Edible>();
            float distanceToGrass = Vector3.Distance(position, closestGrass.transform.position);
            target = closestGrass.transform;
            DrawLine(transform.position, target.position);

            if(distanceToGrass <= sightRadius)
            {
                state = States.Eating;
            }
        }
        else if (state == States.Eating)
        {
            edibleObject.Die();

            hunger -= 5;
            state = States.Wandering;
            scene.CreateGrass();

            if (hunger <= 0)                         //if the rabbit is sated he goes back to wandering around
            {
                state = States.Wandering;
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
            Animal closestMate = LookForMate("FemaleRabbit");
            DrawLine(position, closestMate.position);
        }
    }

    private void WanderAround()
    {
        float distanceMoved;
        Move();
        currentXPos = transform.position.x;
        currentZPos = transform.position.z;

        if (currentDirection == Directions.Left || currentDirection == Directions.Right)
        {
            distanceMoved = CalculateDistanceMoved(startXPos, currentXPos);
        }
        else
        {
            distanceMoved = CalculateDistanceMoved(startZPos, currentZPos);
        }

        if (distanceMoved >= tileSize)                                                      //If the distance moved is bigger than the size of the tile
        {                                                                                   //it means that it's time to randomize a new direction.
            currentXPos = (int)Math.Round(currentXPos);                                     //With the float being inaccurate each movement is slightly off,
            currentZPos = (int)Math.Round(currentZPos);                                     //Rounding to the closest value solves that problem.
            startXPos = currentXPos;                                                             //Rabbit's current position becomes its starting position, which
            startZPos = currentZPos;                                                             //allows for calculating the distance travelled.
            transform.position = new Vector3(startXPos, 0, startZPos);
            RandomizeDirection();
        }
    }

    //Randomize what direction the rabbit should move next. The number of is randomized from 0 up to directionsCounter,
    //and the switch statement is used to determine the direction. canMove variable is used to determine if the movement
    //in that direction is allowed. CheckIfCanMove is called to check it. UnityEngine.Random used instead of the System
    //one to randomize numbers not tied to the system's clock. This way the numbers are unique to each object and prevent
    //the rabbits from moving in the same direction.
    //bug if limits not set
    private void RandomizeDirection()
    {
        bool canMove = false;
        do
        {
            int directionsCounter = Directions.GetNames(typeof(Directions)).Length;
            int rnd = UnityEngine.Random.Range(0, directionsCounter);
            switch (rnd)
            {
                case 0:
                    currentDirection = Directions.Left;
                    break;
                case 1:
                    currentDirection = Directions.Up;
                    break;
                case 2:
                    currentDirection = Directions.Right;
                    break;
                case 3:
                    currentDirection = Directions.Down;
                    break;
            }
            canMove = CheckIfCanMove(currentDirection);
        } while (canMove == false);
    }

    //Check if the rabbit can move in the randomized direction. Takes currentDirection of the Directions type as a parameter, which is the randomized direction.
    //bug if limits not set
    private bool CheckIfCanMove(Directions currentDirection)
    {
        if(currentXPos <= leftLimit && currentDirection == Directions.Left)             //Check if the rabbit wants to go left despite being at the left edge of the map
        {
            return false;
        }
        else if(currentXPos >= rightLimit && currentDirection == Directions.Right)      //Check if the rabbit want to go right despite being at the right edge of the map
        {
            return false;
        }
        else if(currentZPos <= downLimit && currentDirection == Directions.Down)        //Same but down
        {
            return false;
        }
        else if(currentZPos >= upLimit && currentDirection == Directions.Up)            //Same but up
        {
            return false;
        }
        return true;                                                                    //Otherwise return true, meaning the move is possible
    }

    //Calculate how much the rabbit has moved since the last frame. Used to check if it's time to randomize a new direction
    private float CalculateDistanceMoved(float startPos, float currentPos)
    {
        return System.Math.Abs(currentPos - startPos);                                  
    }

    //Move the rabbit according to the randomized direction. The movement is done by changing the position of the rabbit. The moveSpeed is multiplied by
    //Time.deltaTime to ensure that the value is identical on all machines no matter how fast they are. The new vector is created by either subtracting
    //or adding to the x or z value.
    private void Move()
    {
        if (currentDirection == Directions.Left)
        {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
        }
        else if (currentDirection == Directions.Right)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
        }
        else if (currentDirection == Directions.Up)
        {
            transform.position += new Vector3(0f, 0f, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position += new Vector3(0f, 0f, -moveSpeed * Time.deltaTime);
        }
    }

    //Call scene's functions to acquire the limits of the map. Used to determine where the rabbit's movements should be blocked.
    private void GetLimits()
    {
        leftLimit = scene.GetLeftLimit();
        upLimit = scene.GetUpLimit();
        rightLimit = scene.GetRightLimit();
        downLimit = scene.GetDownLimit();
    }

    private void CreateLineRenderer()
    {
        //For creating line renderer object
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void DrawLine(Vector3 position1, Vector3 position2)
    {
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, position1);
        lineRenderer.SetPosition(1, position2);
    }

    private Transform FindClosestGrass()
    {
        Transform closestGrass = transform;
        float distanceToGrass;
        float shortestDistance = -1;
        GameObject grassContainer = scene.grassContainer;
        Transform[] allChildren = grassContainer.GetComponentsInChildren<Transform>();

        foreach (Transform childGrass in allChildren)
        {
            distanceToGrass = Vector3.Distance(transform.position, childGrass.position);
            if (shortestDistance == -1 || distanceToGrass < shortestDistance)
            {
                shortestDistance = distanceToGrass;
                closestGrass = childGrass;
            }
        }
        
        return closestGrass;
    }

    private Transform FindClosestWater()
    {
        Transform closestWater = transform;
        float distanceToWater;
        float shortestDistance = -1;
        GameObject waterContainer = scene.waterContainer;
        Transform[] allWaterTile = waterContainer.GetComponentsInChildren<Transform>();

        foreach (Transform childWater in allWaterTile)
        {
            distanceToWater = Vector3.Distance(transform.position, childWater.position);
            if (shortestDistance == -1 || distanceToWater < shortestDistance)
            {
                shortestDistance = distanceToWater;
                closestWater = childWater;
            }
        }

        return closestWater;
    }

    private void DisableLineRenderer()
    {
        lineRenderer.SetVertexCount(0);
    }
}
