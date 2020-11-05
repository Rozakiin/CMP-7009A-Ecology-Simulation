using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : Edible
{
    public Transform target;

    // possition and movement properties
    protected float startXPos, startZPos;                                                               //The starting x and z position
    protected float currentXPos, currentZPos;                                                 //The current x and z position
    public float moveSpeed;

    // status properties (could be made into a struct?)
    protected float hunger;
    protected float thirst;
    protected int age;
    protected float reproductiveUrge;
    protected float sightRadius;
    protected float touchRadius;
    protected float eatingSpeed;
    protected float pregnancyLength;
    protected Gender gender;
    protected abstract float maxLifeExpectancy { get; set;}
    protected abstract float babyNumber { get; set;}

    //scene data
    protected float tileSize;                                                                 //The size of each tile on the map
    protected float leftLimit, upLimit, rightLimit, downLimit;
    protected int numberOfTurns;

    //other
    protected Edible edibleObject;
    protected Renderer renderer;
    protected float scaleMult;
    protected LineRenderer lineRenderer;

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
        Wandering, Hungry, Thirsty, Eating, Drinking, SexuallyActive, Mating, Fleeing, Dead
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
        print(gender);
    }


    protected void WanderAround()
    {
        if (target == null)
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
    protected bool CheckIfWalkable(Vector3 worldPoint)
    {
        Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
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
        if (closestConsumable != null)
        {
            return closestConsumable.GetComponent<Edible>();
        }
        Edible edible = null; 
        return edible;
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
}