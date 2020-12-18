using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class TargetingSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem ecbSystem;
    private NativeArray<GridNode> gridNodeArray;

    protected override void OnCreate()
    {
        base.OnCreate();
        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        
        // only run if grid has a size aka it has been created
        if (GridSetup.Instance.GridMaxSize > 0)
        {
            if (!gridNodeArray.IsCreated)
                gridNodeArray = CreateGridNodeArray();
            NativeArray<GridNode> grid = new NativeArray<GridNode>(gridNodeArray, Allocator.TempJob);

            //Get grid data needed to check walkable
            int2 gridSize = GridSetup.Instance.gridSize;
            float2 worldSize = SimulationManager.worldSize;

            float leftLimit = SimulationManager.leftLimit;
            float rightLimit = SimulationManager.rightLimit;
            float downLimit = SimulationManager.downLimit;
            float upLimit = SimulationManager.upLimit;
            float3 worldBottomLeft = SimulationManager.worldBottomLeft;

            float deltaTime = Time.DeltaTime;
            float time = UnityEngine.Time.time;
            float timeSeed = time * System.DateTimeOffset.Now.Millisecond;

            //entities without path request data
            Entities
                .WithNone<PathFindingRequestData>()
                .ForEach((
                Entity entity,
                int entityInQueryIndex,
                ref TargetData targetData,
                ref BasicNeedsData basicNeedsData,
                in PathFollowData pathFollowData,
                in Translation translation,
                in StateData stateData
                ) =>
            {

            //if physically at target
            float euclidian = math.distance(translation.Value, targetData.currentTarget);
                if (euclidian <= targetData.touchRadius)
                {
                    targetData.atTarget = true;
                    targetData.oldTarget = targetData.currentTarget;
                }
                else // might not be needed
                {
                    targetData.atTarget = false;
                }

            // if not following a path
            if (pathFollowData.pathIndex < 0)
                {
                    float3 currentPosition = translation.Value;
                    float3 targetPosition = worldBottomLeft * 2; // set to double bottom left as should be further than everything else in scene
                                                                 //create unique seed for random
                    float seed = timeSeed * (translation.Value.x * translation.Value.z) + entity.Index;
                    switch (stateData.state)
                    {
                        case StateData.States.Wandering:
                            targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                            if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                            {
                            //ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                            //ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = currentPosition, endPosition = targetPosition });

                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                            break;
                        case StateData.States.Hungry:
                            // if found valid target
                            if (targetData.entityToEat != Entity.Null)
                            {
                                // make a path finding request
                                //ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                                //ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = currentPosition, endPosition = targetPosition });
                                if(HasComponent<Translation>(targetData.entityToEat))
                                {
                                    targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.entityToEat].Value;
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    //ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                                    //ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = currentPosition, endPosition = targetPosition });
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            break;
                        case StateData.States.Thirsty:
                            // if found valid target
                            if (targetData.entityToDrink != Entity.Null)
                            {
                                // make a path finding request
                                //ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                                //ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = currentPosition, endPosition = targetPosition });
                                if (HasComponent<Translation>(targetData.entityToDrink))
                                {
                                    targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.entityToDrink].Value;
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    //ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                                    //ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = currentPosition, endPosition = targetPosition });
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            break;
                        case StateData.States.SexuallyActive:
                            // if found valid target
                            if (targetData.entityToMate != Entity.Null)
                            {
                                // make a path finding request
                                //ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                                //ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = currentPosition, endPosition = targetPosition });
                                if (HasComponent<Translation>(targetData.entityToMate))
                                {
                                    targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.entityToMate].Value;
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    //ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                                    //ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = currentPosition, endPosition = targetPosition });
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            break;
                        case StateData.States.Fleeing:
                            // if found valid target
                            if (targetData.predatorEntity != Entity.Null)
                            {
                                // make a path finding request
                                //ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                                //ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = currentPosition, endPosition = targetPosition });
                                if (HasComponent<Translation>(targetData.predatorEntity))
                                {
                                    
                                    targetPosition = 2 * translation.Value - GetComponentDataFromEntity<Translation>(true)[targetData.predatorEntity].Value;
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    //ecb.AddComponent<PathFindingRequestData>(entityInQueryIndex, entity);
                                    //ecb.SetComponent(entityInQueryIndex, entity, new PathFindingRequestData { startPosition = currentPosition, endPosition = targetPosition });
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            })
                .WithDeallocateOnJobCompletion(grid)
                .ScheduleParallel();
        }
        // Make sure that the ECB system knows about our job
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        gridNodeArray.Dispose();
    }

    //new pathfinding method
    private static bool IsWorldPointWalkableFromGridNativeArray(float3 worldPos, float2 worldSize, int2 gridSize, NativeArray<GridNode> grid)
    {
        // how far along the grid the position is (left 0, middle 0.5, right 1)
        float percentX = worldPos.x / worldSize.x + 0.5f; // optimisation of maths
        float percentY = worldPos.z / worldSize.y + 0.5f;

        //clamp percent between 0 and 1
        percentX = math.clamp(percentX, 0, 1);
        percentY = math.clamp(percentY, 0, 1);

        // calc x,y position in the node array for the world position
        int x = Mathf.FloorToInt(math.min(gridSize.x * percentX, gridSize.x - 1));
        int y = Mathf.FloorToInt(math.min(gridSize.y * percentY, gridSize.y - 1));

        return grid[x+y * gridSize.x].isWalkable;
    }
    //new pathfinding method
    private static float3 FindRandomWalkableTargetInVision(float3 currentPosition, float sightRadius, float randomSeed, float2 worldSize, int2 gridSize, NativeArray<GridNode> grid)
    {
        float3 target = new float3(worldSize.x + currentPosition.x + 1, 0, worldSize.y + currentPosition.z + 1); //position off the map
        bool isTargetWalkable = false;
        float3 targetWorldPoint;

        // create random generator from seed
        Random randomGen = new Random((uint)randomSeed + 1);
        int timeout = 0; //iteration counter so after certain number of attempts to find a target the loop ends

        //find walkable targetWorldPoint
        while (!isTargetWalkable && timeout<100)
        {
            // generate random numbers with bounds of sightDiameter
            float randX = randomGen.NextFloat(-sightRadius, sightRadius);
            float randZ = randomGen.NextFloat(-sightRadius, sightRadius);
            // random point within the sight of the rabbit
            targetWorldPoint = currentPosition + new float3(randX, 0, randZ);
            //check targetWorldPoint is walkable
            isTargetWalkable = IsWorldPointWalkableFromGridNativeArray(targetWorldPoint, worldSize, gridSize, grid);
            if (isTargetWalkable)
            {
                //set target to the targetWorldPoint
                target = targetWorldPoint;
            }
            timeout++;
        }

        return target;
    }

    private static NativeArray<GridNode> CreateGridNodeArray()
    {
        GridNode[,] grid = GridSetup.Instance.grid;
        int2 gridSize = GridSetup.Instance.gridSize;
        int gridMaxSize = GridSetup.Instance.GridMaxSize;
        NativeArray<GridNode> gridNodeArray = new NativeArray<GridNode>(gridMaxSize, Allocator.Persistent);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GridNode gridNode = new GridNode
                {
                    x = x,
                    y = y,

                    isWalkable = grid[x, y].isWalkable,
                    movementPenalty = grid[x, y].movementPenalty,
                    worldPosition = grid[x, y].worldPosition,
                };

                gridNodeArray[x + y * gridSize.x] = gridNode;
            }
        }

        return gridNodeArray;
    }


    //old method
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

    //old method
    private static float3 FindRandomTargetInVision(float3 position, float sightRadius, float leftLimit, float rightLimit, float downLimit, float upLimit, float seed)
    {
        float3 target = new float3(leftLimit+1, 0, downLimit+1); //position off the map
        bool isTargetWalkable = false;
        float3 targetWorldPoint;

        // create random generator from seed
        Random randomGen = new Random((uint)seed + 1);

        //find walkable targetWorldPoint
        while (!isTargetWalkable) // possible infinite loop if no walkable place?
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