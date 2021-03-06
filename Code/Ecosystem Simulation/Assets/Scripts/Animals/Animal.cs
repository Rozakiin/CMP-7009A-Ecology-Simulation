using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : Edible
{
    #region Properties
    [Header("Animal Properties")]
    [SerializeField] protected Vector3 target;

    [Header("Position and Movement Properties")]
    private static float moveSpeedBase;
    [SerializeField] protected float moveMultiplier; 
    public virtual float MoveSpeed 
    {
        get { return moveSpeedBase*moveMultiplier; }
        protected set { moveSpeedBase = value; }
    }

    [Header("Status Properties")]
    [SerializeField] protected float hunger;
    [SerializeField] protected float eatingSpeed;
    private static float hungerMax;
    public virtual float HungerMax
    {
        get { return hungerMax; }
        protected set { hungerMax = value; }
    }

    [SerializeField] protected float thirst;
    private static float thirstMax;
    public virtual float ThirstMax
    {
        get { return thirstMax; }
        protected set { thirstMax = value; }
    }

    [SerializeField] protected float age;
    private static float ageMax;
    public virtual float AgeMax
    {
        get { return ageMax; }
        protected set { ageMax = value; }
    }

    [SerializeField] protected Gender gender;
    [SerializeField] protected Animal closestMate;
    [SerializeField] protected bool pregnant;
    [SerializeField] public abstract float MatingDuration { get; protected set; }
    [SerializeField] protected float mateStartTime;
    [SerializeField] public abstract float BirthDuration { get; protected set; }   //How long between babies being born
    [SerializeField] protected float birthStartTime;
    [SerializeField] protected int numberOfBabies;  //How many the female is carrying right now
    [SerializeField] protected int babiesBorn;      //How many she has given birth to
    [SerializeField] public abstract int LitterSizeMin { get; protected set; }
    [SerializeField] public abstract int LitterSizeMax { get; protected set; }
    [SerializeField] public abstract int LitterSizeAve { get; protected set; }

    [SerializeField] protected float reproductiveUrge;
    [SerializeField] protected float pregnancyStartTime;
    private static float pregnancyLengthBase;
    protected float pregnancyLengthModifier;
    public virtual float PregnancyLength
    {
        get { return pregnancyLengthBase * pregnancyLengthModifier; }
        protected set { pregnancyLengthBase = value; }
    }

    [SerializeField] public abstract float SightRadius { get; protected set; }
    [SerializeField] public abstract float TouchRadius { get; protected set; }

    [Header("Scene Data")]
    [SerializeField] protected float tileSize;                                                                 //The size of each tile on the map
    [SerializeField] protected float leftLimit;
    [SerializeField] protected float upLimit;
    [SerializeField] protected float rightLimit;
    [SerializeField] protected float downLimit;
    [SerializeField] protected int numberOfTurns;

    [Header("Other")]
    [SerializeField] protected Edible edibleObject;
    [SerializeField] protected Renderer renderer;
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected float timer;

    [Header("Scale")]
    //Scale
    private static float scaleFemaleBase;
    protected virtual float ScaleFemale
    {
        get { return scaleFemaleBase * scaleMultiplier; }
        set { scaleFemaleBase = value; }
    }

    private static float scaleMaleBase;
    protected virtual float ScaleMale
    {
        get { return scaleMaleBase * scaleMultiplier; }
        set { scaleMaleBase = value; }
    }

    [SerializeField] protected float scaleMultiplier;
    public virtual float Scale
    {
        get { return gender == Gender.Female ? ScaleFemale : ScaleMale; }
    }



    protected enum Gender
    {
        Male, Female
    }

    protected enum Directions
    {
        Left, Up, Right, Down
    }
    [SerializeField] protected Directions currentDirection;

    protected enum States
    {
        Wandering, Hungry, Thirsty, Eating, Drinking, SexuallyActive, Mating, Fleeing, Dead, Pregnant, GivingBirth
    }
    [SerializeField] protected States state;

    public enum DeathReason
    {
        Hunger,
        Thirst,
        Age,
        Eaten
    }

    // count death reason
    [SerializeField] public static int diedFromHunger;
    [SerializeField] public static int diedFromThirst;
    [SerializeField] public static int diedFromAge;
    [SerializeField] public static int diedFromEaten;
    #endregion

    #region Initialisation
    public void Start()
    {
        target = transform.position;
        int rnd = UnityEngine.Random.Range(0, 2);
        gender = (Gender)rnd;
        if(gender == Gender.Female)
        {
            string type = this.GetType().ToString();
            gameObject.tag = "Female" + type;
            babiesBorn = 0;
        }
        print(gender);
        pregnant = false;
        timer = 0f;
        moveMultiplier = 1f;
        pregnancyLengthModifier = 1f;
        scaleMultiplier = 1f;
        reproductiveUrge = 0f;
        thirst = 0f;
        hunger = 0f;
    }
    #endregion

    #region Global Setters
    public virtual void SetGlobalBaseMoveSpeed(float _speed)
    {
        MoveSpeed = _speed;
    }

    public virtual void SetGlobalBaseMaxHunger(float _hungerMax)
    {
        HungerMax = _hungerMax;
    }

    public virtual void SetGlobalBaseMaxThirst(float _thirstMax)
    {
        ThirstMax = _thirstMax;
    }

    public virtual void SetGlobalBaseMaxAge(float _ageMax)
    {
        AgeMax = _ageMax;
    }

    public virtual void SetGlobalBaseTouchRadius(float _touchRadius)
    {
        TouchRadius = _touchRadius;
    }

    public virtual void SetGlobalBaseSightRadius(float _sightRadius)
    {
        SightRadius = _sightRadius;
    }

    public virtual void SetGlobalBaseMatingDuration(float _matingDuration)
    {
        MatingDuration = _matingDuration;
    }

    public virtual void SetGlobalBaseBirthDuration(float _birthDuration)
    {
        BirthDuration = _birthDuration;
    }

    public virtual void SetGlobalBaseLitterSizeMin(int _litterSizeMin)
    {
        LitterSizeMin = _litterSizeMin;
    }

    public virtual void SetGlobalBaseLitterSizeMax(int _litterSizeMax)
    {
        LitterSizeMax = _litterSizeMax;
    }

    public virtual void SetGlobalBaseLitterSizeAve(int _litterSizeAve)
    {
        LitterSizeAve = _litterSizeAve;
    }

    public virtual void SetGlobalBasePregnancyLength(float _pregnancyLength)
    {
        PregnancyLength = _pregnancyLength;
    }

    public virtual void SetGlobalBaseFemaleScale(float _scale)
    {
        ScaleFemale = _scale;
    }

    public virtual void SetGlobalBaseMaleScale(float _scale)
    {
        ScaleMale = _scale;
    }
    #endregion

    protected void WanderAround()
    {
        if (target == transform.position) //if target is self then no target(Vector3 can't be null)
        {
            bool isTargetWalkable = false;
            Vector3 targetWorldPoint;
            //find walkable targetWorldPoint
            while (!isTargetWalkable)
            {
                float randX = UnityEngine.Random.Range(-SightRadius, SightRadius);
                float randZ = UnityEngine.Random.Range(-SightRadius, SightRadius);
                // random point within the sight radius of the rabbit
                targetWorldPoint = transform.position + Vector3.right * randX + Vector3.forward * randZ;
                //check targetWorldPoint is walkable
                isTargetWalkable = CheckIfWalkable(targetWorldPoint);
                if (isTargetWalkable)
                {
                    //set target to the targetWorldPoint
                    target = targetWorldPoint;
                }
            }
        }
    }


    //Method to check if a given position is a walkable node(could be extended to check if the whole path is walkable?)
    //Uses ray hits to check if collided with anything
    protected bool CheckIfWalkable(Vector3 worldPoint)
    {
        Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);//50 is just a high value
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Node targetNode = scene.GetComponent<PathFinderController>().NodeFromWorldPoint(worldPoint);//Gets the node closest to the world point
            return targetNode.isWalkable;
        }
        return false;// didn't hit so out of map area
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
        //When looking for female mates, ignore the ones that have already been impregnated
        if (searchedTag.Contains("Female"))
        {
            List<GameObject> bufferList = new List<GameObject>();
            foreach (GameObject female in allChildren)
            {
                if (!female.GetComponent<Animal>().pregnant)
                {
                    bufferList.Add(female);
                }
            }
            allChildren = null;
            allChildren = new GameObject[bufferList.Count];
            for (int i = 0; i < bufferList.Count; i++)
            {
                allChildren[i] = bufferList[i];
            }
        }
        foreach (GameObject childConsumable in allChildren)
        {
            distanceToConsumable = Vector3.Distance(transform.position, childConsumable.transform.position);
            if(shortestDistance == -1 || distanceToConsumable < shortestDistance)
            {
                //if the child is on a walkable position
                if (CheckIfWalkable(childConsumable.transform.position))
                {
                    shortestDistance = distanceToConsumable;
                    closestConsumable = childConsumable;
                }
            }
        }
        if (closestConsumable != null)
        {
            return closestConsumable.GetComponent<Edible>();
        }
        return null;
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


    protected virtual void Mate(Animal femaleMate)
    {
        if (femaleMate.state != States.Mating)
        {
            femaleMate.state = States.Mating;
        }
    }

    protected virtual void GiveBirth()
    {
        Vector3 position = transform.position;
        scene.CreateRabbitAtPos(ref position);
        //scene.CreateAnimal(this.gameObject);
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
                Debug.Log("Unknown death reason: " + reason.ToString());
                break;
        }
        Destroy(gameObject);
    }

    public float RandomGaussian(float sigma, float mu)                  //Get a random number with a Gaussian distribution
    {
        float x1, x2, w, y1; //, y2;
        System.Random random = new System.Random();
        do
        {
            x1 = 2f * (float)random.NextDouble() - 1f;
            x2 = 2f * (float)random.NextDouble() - 1f;
            w = x1 * x1 + x2 * x2;
        } while (w >= 1f);

        w = Mathf.Sqrt((-2f * Mathf.Log(w)) / w);
        y1 = x1 * w;
        // y2 = x2 * w;
        return (y1 * sigma) + mu;                                       // returns a random number
    }


    public Vector3 GetTarget()
    {
        return target;
    }


    public void SetTarget(Vector3 _target)
    {
        target = _target;
    }

    public override void SetNutritionalValue()
    {
        nutritionalValue = baseNutritionalValue * (int)age;
    }


    public override int GetNutritionalValue()
    {
        return nutritionalValue;
    }
}