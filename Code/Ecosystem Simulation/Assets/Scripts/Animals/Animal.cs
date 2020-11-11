using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour, Edible
{
    public Simulation scene;

    public Transform target;

    // possition and movement properties
    protected float startXPos, startZPos;                                                               //The starting x and z position
    protected float currentXPos, currentZPos;                                                 //The current x and z position
    public float moveSpeed;

    // status properties (could be made into a struct?)
    protected float hunger;
    protected float thirst;
    protected float age;
    protected float reproductiveUrge;
    protected float sightRadius;
    protected float eatingSpeed;
    protected float pregnancyLength;
    protected abstract float maxLifeExpectancy { get; set;}
    protected abstract float babyNumber { get; set;}

    //Edible Interface
    public int baseNutritionalValue { get; set; } = 10;
    public bool canBeEaten { get; set; } = true;
    public int NutritionalValue()
    {
        return baseNutritionalValue * (int)age;
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

    protected virtual void Awake()
    {
        scene = GameObject.FindWithTag("GameController").GetComponent<Simulation>();
    }

    protected virtual void RandomMove()
    {

    }

    //create path to somewhere coordinate (x,y)
    protected virtual void MoveToward(GameObject target)
    {

    }

    protected virtual void LookForFood()
    {

    }

    protected virtual void LookForWater()
    {

    }

    protected virtual void LookForMate()
    {

    }

    protected virtual void Mating()
    {

    }

    protected virtual void Flee()
    {

    }

    protected virtual void Eating()
    {

    }

    protected virtual void Drinking()
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
}