using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : Edible
{
    public Transform target;

    // possition and movement properties
    protected float startXPos, startZPos;                                                               //The starting x and z position
    protected float currentXPos, currentZPos;                                                           //The current x and z position
    public float moveSpeed;

    // status properties (could be made into a struct?)
    protected float hunger;
    protected float thirst;
    protected int age;
    protected float reproductiveUrge;
    protected float sightRadius;
    protected float eatingSpeed;
    protected float pregnancyLength;
    public bool pregnant;
    protected Gender gender;
    protected abstract float maxLifeExpectancy { get; set;}
    protected abstract float babyNumber { get; set;}
    protected float matingDuration;
    protected float matingTimeStarted;
    protected float timer;

    //scene data
    protected float tileSize;                                                                 //The size of each tile on the map
    protected float leftLimit, upLimit, rightLimit, downLimit;
    protected int numberOfTurns;

    //other
    protected Edible edibleObject;
    protected Renderer renderer;
    protected float scaleMult;
    protected LineRenderer lineRenderer;
    protected float distanceToTarget;

    protected enum Gender
    {
        Male, Female
    }

    protected enum Directions
    {
        Left, Up, Right, Down
    }
    protected Directions currentDirection;

    public enum States
    {
        Wandering, Hungry, Thirsty, Eating, Drinking, SexuallyActive, Mating, Fleeing, Dead, Pregnant
    }
    public States state;

    public enum DeathReason
    {
        Hunger,
        Thirst,
        Age,
        Eaten
    }

    // count death reason
    public static int diedFromHunger;
    public static int diedFromThirst;
    public static int diedFromAge;
    public static int diedFromEaten;


    public void Start()
    {
        int rnd = UnityEngine.Random.Range(0, 2);
        gender = (Gender)rnd;
        if(gender == Gender.Female)
        {
            string type = this.GetType().ToString();
            gameObject.tag = "Female" + type;
        }
        distanceToTarget = 1000000;
        print(gender);
        pregnant = false;
        timer = 0f;
    }


    protected void WanderAround()
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
    protected void RandomizeDirection()
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
    protected bool CheckIfCanMove(Directions currentDirection)
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
    protected float CalculateDistanceMoved(float startPos, float currentPos)
    {
        return System.Math.Abs(currentPos - startPos);                                  
    }


    //Move the rabbit according to the randomized direction. The movement is done by changing the position of the rabbit. The moveSpeed is multiplied by
    //Time.deltaTime to ensure that the value is identical on all machines no matter how fast they are. The new vector is created by either subtracting
    //or adding to the x or z value.
    protected void Move()
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
    protected void GetLimits()
    {
        leftLimit = scene.GetLeftLimit();
        upLimit = scene.GetUpLimit();
        rightLimit = scene.GetRightLimit();
        downLimit = scene.GetDownLimit();
    }


    protected void CreateLineRenderer()
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


    protected void DisableLineRenderer()
    {
        lineRenderer.SetVertexCount(0);
    }


    protected void DrawLine(Vector3 position1, Vector3 position2)
    {
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, position1);
        lineRenderer.SetPosition(1, position2);
    }


    protected Edible LookForConsumable(string searchedTag)
    {
        //Edible edible = this;
        GameObject closestConsumable = null;
        float distanceToConsumable;
        float shortestDistance = 1000000000;
        GameObject[] allChildren = GameObject.FindGameObjectsWithTag(searchedTag);
        //Edible[] allChildren = targetContainer.GetComponentsInChildren<Grass>();
        foreach (GameObject childConsumable in allChildren)
        {
            distanceToConsumable = Vector3.Distance(transform.position, childConsumable.transform.position);
            if(shortestDistance == -1 || distanceToConsumable < shortestDistance)
            {
                shortestDistance = distanceToConsumable;
                closestConsumable = childConsumable;
            }
        }
        return closestConsumable.GetComponent<Edible>();
    }


    protected virtual Animal LookForMate(string searchedTag)
    {
        return (Animal)LookForConsumable(searchedTag);
    }


    protected Transform FindClosestWater()
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


    protected virtual void Mate()
    {

    }


    protected virtual void Flee()
    {

    }


    protected virtual void Eat(Edible edibleObject, int value, List<Edible> edibleList)
    {
        if (edibleObject != null)
        {
            edibleObject.LowerNutritionalValue(value);
            scene.RemoveFromList(edibleObject, edibleList);
            scene.DestroyObject(edibleObject.gameObject);
            //Destroy(edibleObject.gameObject);
            print("Ate");
        }
        //edibleObject.gameObject.GetComponents<GameObject>();
    }


    protected virtual void Drink()
    {

    }


    public void destroyGameObject(DeathReason reason)
    {
        switch (reason)
        {
            case DeathReason.Hunger:
                Debug.Log("Died from " + reason.ToString());
                diedFromHunger++;
                break;
            case DeathReason.Thirst:
                Debug.Log("Died from " + reason.ToString());
                diedFromThirst++;
                break;
            case DeathReason.Age:
                Debug.Log("Died from " + reason.ToString());
                diedFromAge++;
                break;
            case DeathReason.Eaten:
                Debug.Log("Died from " + reason.ToString());
                diedFromEaten++;
                break;
            default:
                Debug.Log("Unkown death reason: " + reason.ToString());
                break;
        }
        Destroy(gameObject);
    }


    public override void SetNutritionalValue()
    {
        nutritionalValue = baseNutritionalValue * age;
    }


    public override int GetNutritionalValue()
    {
        return nutritionalValue;
    }

    public void SetState(States state)
    {
        this.state = state;
    }

    public States GetState()
    {
        return state;
    }
}