using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    #region Defaults
    public class DefaultValues
    {
        public static readonly float moveSpeed = 25f;
        public static readonly float scaleMale = 2.7f;
        public static readonly float scaleFemale = 3.7f;
        public static readonly float hungerMax = 100f;
        public static readonly float thirstMax = 100f;
        public static readonly int ageMax = 600;
        public static readonly float pregnancyLength = 10f;
    }
    #endregion

    #region Properties
    [Header("Rabbit Properties")]
    private static float moveSpeedBase = DefaultValues.moveSpeed;
    public override float MoveSpeed
    {
        get { return moveSpeedBase * moveMultiplier; }
        protected set { moveSpeedBase = value; }
    }

    private static float hungerMax = DefaultValues.hungerMax;
    public override float HungerMax
    {
        get { return hungerMax; }
        protected set { hungerMax = value; }
    }

    private static float thirstMax = DefaultValues.thirstMax;
    public override float ThirstMax
    {
        get { return thirstMax; }
        protected set { thirstMax = value; }
    }

    private static int ageMax = DefaultValues.ageMax;
    public override int AgeMax
    {
        get { return ageMax; }
        protected set { ageMax = value; }
    }

    protected override int LitterSizeMax
    {
        get { return LitterSizeMax; }
        set {}
    }

    private static float pregnancyLengthBase = DefaultValues.pregnancyLength;
    public override float PregnancyLength
    {
        get { return pregnancyLengthBase * pregnancyLengthModifier; }
        protected set { pregnancyLengthBase = value; }
    }

    private static float scaleFemaleBase = DefaultValues.scaleFemale;
    protected override float ScaleFemale
    {
        get { return scaleFemaleBase * scaleMultiplier; }
        set { scaleFemaleBase = value; }
    }

    private static float scaleMaleBase = DefaultValues.scaleMale;
    protected override float ScaleMale
    {
        get { return scaleMaleBase * scaleMultiplier; }
        set { scaleMaleBase = value; }
    }
    #endregion

    #region Initialisation
    void Awake()//Ran once the program starts
    {
        scene = GameObject.FindWithTag("GameController").GetComponent<Simulation>(); // get reference to Simulation
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        canBeEaten = true;
        numberOfTurns = 0;
        age = 0f;
        baseNutritionalValue = 5;
        sightRadius = 20;
        touchRadius = 1;
        tileSize = scene.GetTileSize();
        eatingSpeed = 2f;
        matingDuration = 3f;
        birthDuration = 0.2f;
        LitterSizeMax = 13;

        transform.localScale = new Vector3(Scale, Scale, Scale);        // changes scale depending on gender

        state = States.Wandering;

        SetPosition();
        CreateLineRenderer();
        GetLimits();
        SetNutritionalValue();
    }                                                                                       //the standard one is quite small and barely visible
    #endregion

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(Scale, Scale, Scale);        // changes scale depending on gender

        DisableLineRenderer();
        SetPosition();
        hunger += 1 * Time.deltaTime;
        thirst += 1 * Time.deltaTime;
        age += 1 * Time.deltaTime;
        timer += 1 * Time.deltaTime;
        CheckIfAlive();
        if (pregnant)
        {
            moveMultiplier = 0.6f;
            if (timer - pregnancyStartTime >= PregnancyLength)
            {
                state = States.GivingBirth;
                birthStartTime = timer;
                pregnant = false;
            }
        }

        if (gender == Gender.Male)
        {
            reproductiveUrge += 0.3f * Time.deltaTime;
        }    

        if (state == States.Wandering)
        {
            WanderAround();

            //Hunger stronger than Idle
            if (hunger >= 10)
            {
                state = States.Hungry;
            }
            //Thirst stronger than hunger
            if(thirst >= 10)
            {
                state = States.Thirsty;
            }
            //Reproductive Urge stronger than Idle and Hunger
            if (reproductiveUrge >= 5)
            {
                state = States.SexuallyActive;
            }
                     
        }
        else if (state == States.Hungry)
        {
            DisableLineRenderer();
            //Reproductive Urge stronger than Hunger?
            if (reproductiveUrge >= 20)
            {
                state = States.SexuallyActive;
            }

            Edible closestGrass = LookForConsumable("Grass");//Look for closest grass
            if (closestGrass != null)
            {
                float distanceToGrass = Vector3.Distance(transform.position, closestGrass.transform.position);
                // Grass within touching distance
                if (distanceToGrass <= touchRadius)
                {
                    state = States.Eating;
                }
                //Grass within sight radius
                if (distanceToGrass <= sightRadius)
                {
                    edibleObject = closestGrass.GetComponent<Edible>();//set the edible object to the closest grass object
                    target = closestGrass.transform.position;
                    DrawLine(transform.position, target);
                }
            }
            //No grass
            WanderAround();
        }
        else if (state == States.Eating)
        {
            DisableLineRenderer();
            print("Eating");
            //Eat the object (method call instead?)
            if (edibleObject != null)
            {
                edibleObject.Die();
                hunger -= 5;
                scene.CreateGrass();
            }
            //If the grass has been eaten by someone else, go back to being hungry
            else
            {
                state = States.Hungry;
            }

            if (hunger <= 0)
            {
                state = States.Wandering;
            }
        }
        else if (state == States.Thirsty)
        {
            DisableLineRenderer();
            if (reproductiveUrge >= 5)
            {
                state = States.SexuallyActive;
            }
            Transform closestWater = FindClosestWater();
            float distanceToWater = Vector3.Distance(transform.position, closestWater.position);
            // Water within touching distance
            if (distanceToWater <= touchRadius + tileSize * 2)
            {
                state = States.Drinking;
            }
            //Water within sight radius
            else if (distanceToWater <= sightRadius)
            {
                target = closestWater.position;
                DrawLine(transform.position, target);
            }

            /*if (distanceToWater <= touchRadius + tileSize * 1.2)
            {
                float posX = transform.position.x;
                float posZ = transform.position.z;
                if(transform.position.x < closestWater.position.x)
                {
                    transform.position = new Vector3(posX + MoveSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
                else if(transform.position.x > closestWater.position.x)
                {
                    transform.position = new Vector3(posX -MoveSpeed * Time.deltaTime, 0.0f, 0.0f);
                }

                if(transform.position.z < closestWater.position.z)
                {
                    transform.position = new Vector3(0.0f, 0.0f, posZ + MoveSpeed * Time.deltaTime);
                }
                else if(transform.position.z > closestWater.position.z)
                {
                    transform.position = new Vector3(0.0f, 0.0f, posZ -MoveSpeed * Time.deltaTime);
                }
            }*/
            
            
            WanderAround();
        }
        else if(state == States.Drinking)
        {
            DisableLineRenderer();
            print("Drinking");
            //Drink water (method call instead?)
            thirst -= 20 * Time.deltaTime;
            if(thirst <= 0)
            {
                thirst = 0;
                state = States.Wandering;
            }           
        }
        else if (state == States.SexuallyActive)
        {
            DisableLineRenderer();
            Animal closestMate = LookForMate("FemaleRabbit");
            if (closestMate != null)
            {
                float distanceToMate = Vector3.Distance(transform.position, closestMate.transform.position);
                // Mate within touching distance
                
                if (distanceToMate <= touchRadius)
                {
                    state = States.Mating;
                    mateStartTime = timer;
                    Mate(closestMate);
                }
                else if(distanceToMate <= waitRadius)
                {
                    Wait(closestMate);
                }
                //Mate within sight radius
                if (distanceToMate <= sightRadius)
                {
                    target = closestMate.transform.position;
                    DrawLine(transform.position, target);
                }
            }
            //No mate
            WanderAround();
        }
        else if (state == States.Mating)
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
        else if (state == States.GivingBirth)
        {
            if (timer - birthStartTime >= birthDuration && babiesBorn < numberOfBabies)
            {
                GiveBirth();
                birthStartTime = timer;
                babiesBorn++;
            }
            if (babiesBorn >= numberOfBabies)
            {
                moveMultiplier = 1f;
                state = States.Wandering;
            }
        }
        else if(state == States.Waiting) { }
    }

    private void CheckIfAlive()
    {
        if (hunger >= 100)
        {
            destroyGameObject(DeathReason.Hunger);
        }
        else if (thirst >= 100)
        {
            destroyGameObject(DeathReason.Thirst);
        }
        else if (age >= 600)     // set up 600 Seconds for rabit's lifelong 
        {
            destroyGameObject(DeathReason.Age);
        }
    }
}
