using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    protected float hunger;
    protected float thirsty;
    protected float age;
    protected float reproUrge;
    protected float rabEatTime = 1;
    protected float pregnaTime = 5;
    protected float sightRadius;
    protected abstract float maxLifeExpe { get; set;}
    protected abstract float babyNum { get; set;}
    protected Animal()
    {
    	hunger = 60;
    	thirsty = 60;
    	age = 0;
    	reproUrge= 0;
    }
    public static void destroyBody()
    {
    	
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
