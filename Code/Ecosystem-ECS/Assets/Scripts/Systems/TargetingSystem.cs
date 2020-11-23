using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class TargetingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     float deltaTime = Time.DeltaTime;

        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.

        float leftLimit = SimulationManager.leftLimit;
        float rightLimit = SimulationManager.rightLimit;
        float downLimit = SimulationManager.downLimit;
        float upLimit = SimulationManager.upLimit;
        float3 worldBottomLeft = SimulationManager.worldBottomLeft;

        float deltaTime = Time.DeltaTime;
        float time = UnityEngine.Time.time;
        float timeSeed = time * System.DateTimeOffset.Now.Millisecond;

        //entity queries for edible, water, mate
        EntityQueryDesc edibleDesc = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(EdibleData), typeof(Translation) }
        };
        EntityQueryDesc waterDesc = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(DrinkableData), typeof(Translation) }
        };
        EntityQueryDesc mateDesc = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(MateData), typeof(Translation) }
        };

        //create Entity and Component arrays from queries
        var edibleEntityArray = GetEntityQuery(edibleDesc).ToEntityArray(Allocator.TempJob);
        var edibleEntityEdibleDataArray = GetEntityQuery(edibleDesc).ToComponentDataArray<EdibleData>(Allocator.TempJob);
        var edibleEntityTranslationArray = GetEntityQuery(edibleDesc).ToComponentDataArray<Translation>(Allocator.TempJob);

        var waterEntityArray = GetEntityQuery(waterDesc).ToEntityArray(Allocator.TempJob);
        var waterEntityDrinkableDataArray = GetEntityQuery(waterDesc).ToComponentDataArray<DrinkableData>(Allocator.TempJob);
        var waterEntityTranslationArray = GetEntityQuery(waterDesc).ToComponentDataArray<Translation>(Allocator.TempJob);

        var mateEntityArray = GetEntityQuery(mateDesc).ToEntityArray(Allocator.TempJob);
        //var mateEntityMateDataArray = GetEntityQuery(mateDesc).ToComponentDataArray<MateData>(Allocator.TempJob);
        //var mateEntityTranslationArray = GetEntityQuery(mateDesc).ToComponentDataArray<Translation>(Allocator.TempJob);



        Entities.ForEach((
            Entity entity,
            ref TargetData targetData,
            ref HungerData hungerData,
            ref ThirstData thirstData,
            ref MateData mateData,
            in Translation translation,
            in StateData stateData,
            in VisionData visionData
            ) =>
        {
            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as 'in', which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,

            //if physically at target
            float euclidian = math.distance(translation.Value, targetData.currentTarget);
            if (euclidian <= visionData.touchRadius)
            {
                targetData.atTarget = true;
                targetData.oldTarget = targetData.currentTarget;
            }
            else // might not be needed
            {
                targetData.atTarget = false;
            }

            // if flag says is at target
            if (targetData.atTarget)
            {
                float3 currentpositon = translation.Value;
                float3 targetPosition = worldBottomLeft*2; // set to double bottom left as should be further than everything else in scene
                //create unique seed for random
                float seed = timeSeed * (translation.Value.x * translation.Value.z) + entity.Index;
                switch (stateData.state)
                {
                    case StateData.States.Wandering:
                        targetPosition = FindRandomTargetInVision(translation.Value, visionData.sightRadius, leftLimit, rightLimit, downLimit, upLimit, seed);
                        if (WorldPointIsWalkable(targetPosition, leftLimit, rightLimit, downLimit, upLimit))
                        {
                            targetData.currentTarget = targetPosition;
                            targetData.atTarget = false;
                        }
                        break;
                    case StateData.States.Hungry:
                        Entity edible = Entity.Null;
                        // Really bad slow way, itterate over all edible object to find closest
                        for (int i = 0; i != edibleEntityArray.Length; i++)
                        {
                            // if AND bitise diet and edible's foodtype == the edible's foodtype 
                            EdibleData.FoodType entityDiet = (EdibleData.FoodType)hungerData.diet;
                            bool isInDiet = (edibleEntityEdibleDataArray[i].foodType & entityDiet) == edibleEntityEdibleDataArray[i].foodType;
                            //Debug.Log($"{entity.Index}:{i}:{edibleEntityArray[i].Index}: {isInDiet}");

                            //can be eaten and foodtype in diet?
                            if (edibleEntityEdibleDataArray[i].canBeEaten && isInDiet)
                            {
                                //Debug.Log($"{entity.Index}:{i}:{edibleEntityArray[i].Index}");
                                // is in sight?
                                if (math.distance(currentpositon, edibleEntityTranslationArray[i].Value) <= visionData.sightRadius)
                                {
                                    // no target yet
                                    if (edible == Entity.Null)
                                    {
                                        edible = edibleEntityArray[i];
                                        targetPosition = edibleEntityTranslationArray[i].Value;
                                    }
                                    else
                                    {
                                        // Already has target, closest?
                                        if (math.distance(currentpositon, edibleEntityTranslationArray[i].Value) < math.distance(currentpositon, targetPosition))
                                        {
                                            // New Target closer
                                            edible = edibleEntityArray[i];
                                            targetPosition = edibleEntityTranslationArray[i].Value;
                                        }
                                    }
                                }
                            }
                        }
                        // if found valid target
                        if (edible != Entity.Null)
                        {
                            targetData.currentTarget = targetPosition;
                            targetData.atTarget = false;
                            hungerData.entityToEat = edible;
                        }
                        else 
                        {
                            // find randomTarget
                            targetPosition = FindRandomTargetInVision(translation.Value, visionData.sightRadius, leftLimit, rightLimit, downLimit, upLimit, seed);
                            if (WorldPointIsWalkable(targetPosition, leftLimit, rightLimit, downLimit, upLimit))
                            {
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                        }
                        break;
                    case StateData.States.Thirsty:
                        Entity water = Entity.Null;
                        // Really bad slow way, itterate over all edible object to find closest
                        for (int i = 0; i != waterEntityArray.Length; i++)
                        {
                            //if can be eaten
                            if (waterEntityDrinkableDataArray[i].canBeDrunk)
                            {
                                // is in sight?
                                if (math.distance(currentpositon, waterEntityTranslationArray[i].Value) <= visionData.sightRadius)
                                {
                                    // no target yet
                                    if (water == Entity.Null)
                                    {
                                        water = waterEntityArray[i];
                                        targetPosition = waterEntityTranslationArray[i].Value;
                                    }
                                    else
                                    {
                                        // Already has target, closest?
                                        if (math.distance(currentpositon, waterEntityTranslationArray[i].Value) < math.distance(currentpositon, targetPosition))
                                        {
                                            // New Target closer
                                            water = waterEntityArray[i];
                                            targetPosition = waterEntityTranslationArray[i].Value;
                                        }
                                    }
                                }
                            }
                        }

                        if (water != Entity.Null)
                        {
                            targetData.currentTarget = targetPosition;
                            targetData.atTarget = false;
                            thirstData.entityToDrink = water;
                        }
                        else
                        {
                            // find randomTarget
                            targetPosition = FindRandomTargetInVision(translation.Value, visionData.sightRadius, leftLimit, rightLimit, downLimit, upLimit, seed);
                            if (WorldPointIsWalkable(targetPosition, leftLimit, rightLimit, downLimit, upLimit))
                            {
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                        }
                        break;
                    case StateData.States.SexuallyActive:
                        Entity mate = Entity.Null;
                        // Really bad slow way, itterate over all edible object to find closest
                        for (int i = 0; i != mateEntityArray.Length; i++)
                        {
                            ////if can be eaten
                            //if (!mateEntityMateDataArray[i].pregnant)
                            //{
                            //    // no target yet
                            //    if (mate == Entity.Null)
                            //    {
                            //        mate = entity;
                            //    }
                            //    else
                            //    {
                            //        // Already has target, closest?
                            //        if (math.distance(currentpositon, mateEntityTranslationArray[i].Value) < math.distance(currentpositon, targetPosition))
                            //        {
                            //            // New Target closer
                            //            edible = mateEntityArray[i];
                            //            targetPosition = mateEntityTranslationArray[i].Value;
                            //        }
                            //    }
                            //}
                        }

                        if (mate != Entity.Null)
                        {
                            targetData.currentTarget = GetComponentDataFromEntity<Translation>(true)[mate].Value;
                            mateData.entityToMate = mate;
                        }
                        break;
                    default:
                        break;
                }
            }
        })
            .WithDeallocateOnJobCompletion(edibleEntityArray)
            .WithDeallocateOnJobCompletion(edibleEntityEdibleDataArray)
            .WithDeallocateOnJobCompletion(edibleEntityTranslationArray)
            .WithDeallocateOnJobCompletion(waterEntityArray)
            .WithDeallocateOnJobCompletion(waterEntityDrinkableDataArray)
            .WithDeallocateOnJobCompletion(waterEntityTranslationArray)
            .WithDeallocateOnJobCompletion(mateEntityArray)
            //.WithDeallocateOnJobCompletion(mateEntityMateDataArray)
            //.WithDeallocateOnJobCompletion(mateEntityTranslationArray)
            .ScheduleParallel();
    }

    private static bool WorldPointIsWalkable(float3 worldPoint, float leftLimit, float rightLimit, float downLimit, float upLimit)
    {
        // temp bounds checking algo
        if (worldPoint.x > leftLimit && worldPoint.x < rightLimit && worldPoint.z > downLimit && worldPoint.z < upLimit)
            return true;
        return false;

        //Vector3 point = new Vector3(worldPoint.x, worldPoint.y, worldPoint.z); // convert to Vector3
        //Ray ray = new Ray(point + Vector3.up * 50, Vector3.down);//50 is just a high value
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    Node targetNode = PathFinderController.Instance.NodeFromWorldPoint(worldPoint);//Gets the node closest to the world point
        //    return targetNode.isWalkable;
        //}
        //return false;// didn't hit so out of map area
    }

    private static float3 FindRandomTargetInVision(float3 position, float sightRadius, float leftLimit, float rightLimit, float downLimit, float upLimit, float seed)
    {
        float3 target = new float3(leftLimit+1, 0, downLimit+1); //position off the map
        bool isTargetWalkable = false;
        float3 targetWorldPoint;

        // create random generator from seed
        Random randomGen = new Random((uint)seed + 1);

        //find walkable targetWorldPoint
        while (!isTargetWalkable) // possible infinite loop?
        {
            // generate random numbers with bounds of sightDiameter
            float randX = randomGen.NextFloat(-sightRadius, sightRadius);
            float randZ = randomGen.NextFloat(-sightRadius, sightRadius);
            // random point within the sight radius of the rabbit
            targetWorldPoint = position + new float3(1*randX,0,1*randZ);
            //check targetWorldPoint is walkable
            isTargetWalkable = WorldPointIsWalkable(targetWorldPoint, leftLimit, rightLimit, downLimit, upLimit);
            if (isTargetWalkable)
            {
                //set target to the targetWorldPoint
                target = targetWorldPoint;
            }
        }

        return target;
    }

    private static Entity FindNearestEdible()
    {
        Entity edible = Entity.Null;

        return edible;
    }

    private static Entity FindNearestMate()
    {
        Entity mate = Entity.Null;
        return mate;
    }

    private static Entity FindNearestWater()
    {
        Entity water = Entity.Null;
        return water;
    }
}

//Method to check if a given position is a walkable node(could be extended to check if the whole path is walkable?)
//Uses ray hits to check if collided with anything
//protected bool CheckIfWalkable(Vector3 worldPoint)
//{
//    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);//50 is just a high value
//    RaycastHit hit;
//    if (Physics.Raycast(ray, out hit))
//    {
//        Node targetNode = scene.GetComponent<PathFinderController>().NodeFromWorldPoint(worldPoint);//Gets the node closest to the world point
//        return targetNode.isWalkable;
//    }
//    return false;// didn't hit so out of map area
//}

//protected Edible LookForConsumable(string searchedTag)
//{
//    //Edible edible = this;
//    GameObject closestConsumable = null;
//    float distanceToConsumable;
//    float shortestDistance = 1000000000;
//    GameObject[] allChildren = GameObject.FindGameObjectsWithTag(searchedTag);
//    //When looking for female mates, ignore the ones that have already been impregnated
//    if (searchedTag.Contains("Female"))
//    {
//        List<GameObject> bufferList = new List<GameObject>();
//        foreach (GameObject female in allChildren)
//        {
//            if (!female.GetComponent<Animal>().pregnant)
//            {
//                bufferList.Add(female);
//            }
//        }
//        allChildren = null;
//        allChildren = new GameObject[bufferList.Count];
//        for (int i = 0; i < bufferList.Count; i++)
//        {
//            allChildren[i] = bufferList[i];
//        }
//    }
//    foreach (GameObject childConsumable in allChildren)
//    {
//        distanceToConsumable = Vector3.Distance(transform.position, childConsumable.transform.position);
//        if (shortestDistance == -1 || distanceToConsumable < shortestDistance)
//        {
//            //if the child is on a walkable position
//            if (CheckIfWalkable(childConsumable.transform.position))
//            {
//                shortestDistance = distanceToConsumable;
//                closestConsumable = childConsumable;
//            }
//        }
//    }
//    if (closestConsumable != null)
//    {
//        return closestConsumable.GetComponent<Edible>();
//    }
//    return null;
//}


//protected virtual Animal LookForMate(string searchedTag)
//{
//    return (Animal)LookForConsumable(searchedTag);
//}

//protected Transform FindClosestWater()
//{
//    Transform closestWater = transform;
//    float distanceToWater;
//    float shortestDistance = -1;
//    GameObject waterContainer = scene.waterContainer;
//    Transform[] allWaterTile = waterContainer.GetComponentsInChildren<Transform>();

//    foreach (Transform childWater in allWaterTile)
//    {
//        distanceToWater = Vector3.Distance(transform.position, childWater.position);
//        if (shortestDistance == -1 || distanceToWater < shortestDistance)
//        {
//            shortestDistance = distanceToWater;
//            closestWater = childWater;
//        }
//    }

//    return closestWater;
//}