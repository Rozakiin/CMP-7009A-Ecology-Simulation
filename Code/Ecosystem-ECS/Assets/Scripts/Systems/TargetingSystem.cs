using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
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

        EntityManager entityManager = EntityManager;


        Entities.ForEach((
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
            //if at target - should change to using visiondata
            if (targetData.currentTarget.Equals(translation.Value))
            {
                targetData.atTarget = true;
                targetData.currentTarget = targetData.oldTarget;
                switch (stateData.state)
                {
                    case StateData.States.Wandering:
                        float3 randomTarget = FindRandomTargetInVision(translation.Value, visionData.sightRadius);
                        if (WorldPointIsWalkable(randomTarget))
                        {
                            targetData.currentTarget = randomTarget;
                        }
                        break;
                    case StateData.States.Hungry:
                        Entity edible = FindNearestEdible();
                        if (entityManager.Exists(edible))
                        {
                            targetData.currentTarget = entityManager.GetComponentData<Translation>(edible).Value;
                            hungerData.entityToEat = edible;
                        }
                        break;
                    case StateData.States.Thirsty:
                        Entity water = FindNearestWater();
                        if (entityManager.Exists(water))
                        {
                            targetData.currentTarget = entityManager.GetComponentData<Translation>(water).Value;
                            thirstData.entityToDrink = water;
                        }
                        break;
                    case StateData.States.SexuallyActive:
                        Entity mate = FindNearestMate();
                        if (entityManager.Exists(mate))
                        {
                            targetData.currentTarget = entityManager.GetComponentData<Translation>(mate).Value;
                            mateData.closestMate = mate;
                        }
                        break;
                    default:
                        throw new System.IndexOutOfRangeException("state:" + stateData.state);
                }
            }
            else
                targetData.atTarget = false;
        }).Schedule();
    }

    private static bool WorldPointIsWalkable(float3 worldPoint)
    {
        Vector3 point = new Vector3(worldPoint.x, worldPoint.y, worldPoint.z); // convert to Vector3
        Ray ray = new Ray(point + Vector3.up * 50, Vector3.down);//50 is just a high value
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Node targetNode = PathFinderController.Instance.NodeFromWorldPoint(worldPoint);//Gets the node closest to the world point
            return targetNode.isWalkable;
        }
        return false;// didn't hit so out of map area
    }

    private static float3 FindRandomTargetInVision(float3 position, float sightRadius)
    {
        float3 target = new float3(SimulationManager.Instance.leftLimit+1, 0, SimulationManager.Instance.downLimit+1); //position off the map

        bool isTargetWalkable = false;
        float3 targetWorldPoint;
        //find walkable targetWorldPoint
        while (!isTargetWalkable) // possible infinite loop?
        {
            float randX = UnityEngine.Random.Range(-sightRadius, sightRadius);
            float randZ = UnityEngine.Random.Range(-sightRadius, sightRadius);
            // random point within the sight radius of the rabbit
            targetWorldPoint = position + new float3(1*randX,0,1*randZ);
            //check targetWorldPoint is walkable
            isTargetWalkable = WorldPointIsWalkable(targetWorldPoint);
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
        Entity edible = new Entity();
        return edible;
    }

    private static Entity FindNearestMate()
    {
        Entity mate = new Entity();
        return mate;
    }

    private static Entity FindNearestWater()
    {
        Entity water = new Entity();
        return water;
    }
}
//protected void WanderAround()
//{
//    if (target == transform.position) //if target is self then no target(Vector3 can't be null)
//    {
//        bool isTargetWalkable = false;
//        Vector3 targetWorldPoint;
//        //find walkable targetWorldPoint
//        while (!isTargetWalkable)
//        {
//            float randX = UnityEngine.Random.Range(-sightRadius, sightRadius);
//            float randZ = UnityEngine.Random.Range(-sightRadius, sightRadius);
//            // random point within the sight radius of the rabbit
//            targetWorldPoint = transform.position + Vector3.right * randX + Vector3.forward * randZ;
//            //check targetWorldPoint is walkable
//            isTargetWalkable = CheckIfWalkable(targetWorldPoint);
//            if (isTargetWalkable)
//            {
//                //set target to the targetWorldPoint
//                target = targetWorldPoint;
//            }
//        }
//    }
//}

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