﻿using Unity.Collections;
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
                    
                    float seed = timeSeed * (translation.Value.x * translation.Value.z) + entity.Index;//create unique seed for random

                    //Fleeing over other states
                    if (stateData.isFleeing)
                    {
                        // if found valid target
                        if (HasComponent<Translation>(targetData.predatorEntity))
                        {
                            targetPosition = 2 * translation.Value - GetComponentDataFromEntity<Translation>(true)[targetData.predatorEntity].Value;
                            targetData.currentTarget = targetPosition;
                            targetData.atTarget = false;
                        }
                        else
                        {
                            // find randomTarget
                            targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                            if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                            {
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                        }
                    }
                    //Prioritize finding a mate if the entity isn't about to die out of hunger or thirst
                    //And water or food isn't currently in its reach
                    else if(targetData.entityToMate != null && 
                    (
                    (basicNeedsData.hunger <= 0.9 * basicNeedsData.hungerMax && targetData.entityToEat == null) ||
                    (basicNeedsData.thirst <= 0.9 * basicNeedsData.thirstMax && targetData.entityToDrink == null)
                    ))
                    {
                        // if found valid target
                        if (HasComponent<Translation>(targetData.entityToMate))
                        {
                            targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.entityToMate].Value;
                            targetData.currentTarget = targetPosition;
                            targetData.atTarget = false;
                        }
                        else
                        {
                            // find randomTarget
                            targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                            if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                            {
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                        }
                    }
                    //Check if the entity has food or water nearby
                    else if(targetData.entityToDrink != null || targetData.entityToEat != null)
                    {
                        if(targetData.entityToDrink != null && basicNeedsData.thirst >= basicNeedsData.hunger)
                        {
                            // if found valid target
                            if (HasComponent<Translation>(targetData.entityToDrink))
                            {
                                targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.entityToDrink].Value;
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                        }
                        else if(targetData.entityToEat != null && basicNeedsData.hunger >= basicNeedsData.thirst)
                        {
                            // if found valid target
                            if (HasComponent<Translation>(targetData.entityToEat))
                            {
                                targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.entityToEat].Value;
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                        }
                    }
                    //Else normal wandering (I think)
                    else
                    {
                        targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                        if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                        {
                            targetData.currentTarget = targetPosition;
                            targetData.atTarget = false;
                        }
                    }

                    /* old system
                    switch (stateData.state)
                    {
                        case StateData.States.Wandering:
                            targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                            if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                            {
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                            break;
                        case StateData.States.Hungry:
                            // if found valid target
                            if(HasComponent<Translation>(targetData.entityToEat))
                            {
                                targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.entityToEat].Value;
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            break;
                        case StateData.States.Thirsty:
                            // if found valid target
                            if (HasComponent<Translation>(targetData.entityToDrink))
                            {
                                targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.entityToDrink].Value;
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            break;
                        case StateData.States.SexuallyActive:
                            // if found valid target
                            if (HasComponent<Translation>(targetData.entityToMate))
                            {
                                targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.entityToMate].Value;
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            break;
                        case StateData.States.Fleeing:
                            // if found valid target
                            if (HasComponent<Translation>(targetData.predatorEntity))
                            {
                                targetPosition = 2 * translation.Value - GetComponentDataFromEntity<Translation>(true)[targetData.predatorEntity].Value;
                                targetData.currentTarget = targetPosition;
                                targetData.atTarget = false;
                            }
                            else
                            {
                                // find randomTarget
                                targetPosition = FindRandomWalkableTargetInVision(currentPosition, targetData.sightRadius, seed, worldSize, gridSize, grid);
                                if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                {
                                    targetData.currentTarget = targetPosition;
                                    targetData.atTarget = false;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    */
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
}