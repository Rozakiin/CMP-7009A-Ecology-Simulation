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
    protected float eatingSpeed;
    protected float pregnancyLength;
    protected Gender gender;
    protected abstract float maxLifeExpectancy { get; set;}
    protected abstract float babyNumber { get; set;}

    public int NutritionalValue()
    {
        return baseNutritionalValue * age;
    }

    protected enum Gender
    {
        Male, Female
    }

    protected enum Directions
    {
        Left, Up, Right, Down
    }
    protected Directions currentDirection;

    protected enum States
    {
        Wandering, Hungry, Thirsty, Eating, Drinking, SexuallyActive, Mating, Fleeing, Dead
    }
    protected States state;

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

    protected virtual void RandomMove()
    {

    }

    //create path to somewhere coordinate (x,y)
    protected virtual void MoveToward(GameObject target)
    {

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