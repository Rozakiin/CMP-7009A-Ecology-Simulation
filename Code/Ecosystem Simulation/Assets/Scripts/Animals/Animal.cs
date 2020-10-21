using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    protected float hunger;
    protected float thirsty;
    protected float age;
    protected float reproductiveUrge;
    protected float sightRadius;
    protected float EatTime = 1;
    protected float pregnancyLength = 5;
    protected abstract float maxLifeExpe { get; set;}
    protected abstract float babyNum { get; set;}
    public enum DeathReason
    {
        Hunger,
        Thirst,
        Age,
        Eaten
    }
    // count death reason
    static dieForHunger;
    static dieForThirst;
    static dieForAge;
    static dieForEaten;
    protected Animal()
    {
    	hunger = 60;
    	thirsty = 60;
    	age = 0;
    	reproductiveUrge= 0;
    }
    protected virtual void RandomMove()
    {

    }
    //create path to somewhere coordinate (x,y)
    protected virtual void MoveToward(int x, int y)
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
    protected virtual void FLee()
    {

    }
    protected virtual void Eating()
    {

    }
    protected virtual void Drinking()
    {

    }
    public static void destroyBody(DeathReason reason)
    {
        if (reason == Hunger)
        {
            dieForHunger++;
        }
        else if (reason == Thirst)
        {
            dieForThirst++;
        }
        else if (reason == Age)
        {
            dirForAge++;
        }
        else (reason == Eaten)
        {
            dieForEaten++;
        }
        Destroy (gameObject);
    }

}