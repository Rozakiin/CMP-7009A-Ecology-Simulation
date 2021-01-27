using Components;
using MonoBehaviourTools.Grid;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Systems
{
    public class TargetingSystem : SystemBase
    {
        private NativeArray<GridNode> _gridNodeArray;

        /*
         * The system determines the target to move to based primarily on the current state 
         * Grid nativearray is created to be used when checking if worldpoint is walkable
         */
        protected override void OnUpdate()
        {
            // only run if grid has a size aka it has been created
            if (GridSetup.Instance.GridMaxSize > 0)
            {
                if (!_gridNodeArray.IsCreated)
                    _gridNodeArray = CreateGridNodeArray();
                NativeArray<GridNode> grid = new NativeArray<GridNode>(_gridNodeArray, Allocator.TempJob);

                //Get grid data needed to check walkable
                int2 gridSize = GridSetup.Instance.GridSize;
                float2 worldSize = SimulationManager.WorldSize;
                float tileSize = SimulationManager.TileSize;

                float time = UnityEngine.Time.time;
                float timeSeed = time * System.DateTimeOffset.Now.Millisecond;

                //entities without path request data
                Entities
                    .WithNone<PathFindingRequestData>()
                    .ForEach((
                        Entity entity,
                        ref TargetData targetData,
                        in BasicNeedsData basicNeedsData,
                        in PathFollowData pathFollowData,
                        in Translation translation,
                        in StateData stateData
                    ) =>
                    {
                        if (pathFollowData.PathIndex >= 0)
                            targetData.AtTarget = false;
                        else
                            targetData.AtTarget = true;

                        // if not following a path
                        if (pathFollowData.PathIndex < 0)
                        {
                            float3 targetPosition = float.PositiveInfinity; // should be further than everything else in scene

                            float seed = timeSeed * (translation.Value.x * translation.Value.z) + entity.Index;//create unique seed for random

                            //Fleeing over other states
                            if (stateData.IsFleeing)
                            {
                                targetPosition = 2 * translation.Value - GetComponentDataFromEntity<Translation>(true)[targetData.PredatorEntity].Value;
                                targetData.Target = targetPosition;
                                targetData.AtTarget = false;
                            }
                            else
                            {
                                //Prioritize finding a mate if the entity isn't about to die out of hunger or thirst

                                bool isAboutToDieOfHunger = basicNeedsData.Hunger > basicNeedsData.HungerMax * 0.9; //90% of max hunger
                                bool isAboutToDieOfThirst = basicNeedsData.Thirst > basicNeedsData.ThirstMax * 0.9; //90% of max thirst

                                if (stateData.IsSexuallyActive && !stateData.IsMating && HasComponent<Translation>(targetData.EntityToMate) && !isAboutToDieOfThirst && !isAboutToDieOfHunger)
                                {
                                    targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.EntityToMate].Value;
                                }
                                //choose target based on which need is higher
                                else if ((stateData.IsThirsty && !stateData.IsDrinking) || (stateData.IsHungry && !stateData.IsEating))
                                {
                                    //cache result as HasComponent costly call
                                    bool hasValidDrinkTarget = HasComponent<Translation>(targetData.EntityToDrink);
                                    bool hasValidEatTarget = HasComponent<Translation>(targetData.EntityToEat);

                                    //if thirst greater or eq than hunger, and has valid drink target
                                    if ((basicNeedsData.Thirst >= basicNeedsData.Hunger) && stateData.IsThirsty && hasValidDrinkTarget)
                                    {
                                        targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.EntityToDrink].Value;
                                        //determine what side of the tile the entity is, and set target to that
                                        targetPosition = GetNearestSideOfTargetTile(translation.Value, targetPosition, tileSize);
                                    }
                                    //has hunger greater than thirst, and has valid eat target
                                    else if (stateData.IsHungry && hasValidEatTarget)
                                    {
                                        targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.EntityToEat].Value;
                                    }
                                    //has hunger greater than thirst, and has valid drink target
                                    else if (stateData.IsThirsty && hasValidDrinkTarget)
                                    {
                                        targetPosition = GetComponentDataFromEntity<Translation>(true)[targetData.EntityToDrink].Value;
                                        //determine what side of the tile the entity is, and set target to that
                                        targetPosition = GetNearestSideOfTargetTile(translation.Value, targetPosition, tileSize);
                                    }
                                }


                                //if not positive infinity aka target position has been calculated
                                if (!float.IsPositiveInfinity(targetPosition.x) && !float.IsPositiveInfinity(targetPosition.y) && !float.IsPositiveInfinity(targetPosition.z))
                                {
                                    //check that the target is walkable
                                    if (IsWorldPointWalkableFromGridNativeArray(targetPosition, worldSize, gridSize, grid))
                                    {
                                        targetData.Target = targetPosition;
                                        targetData.AtTarget = false;
                                    }
                                    else //find a random target (same as wandering)
                                    {
                                        targetPosition = FindRandomWalkableTargetInVision(translation.Value, targetData.SightRadius, seed, worldSize, gridSize, grid);
                                        targetData.Target = targetPosition;
                                        targetData.AtTarget = false;
                                    }
                                }
                                // if in a state where you should wander, find a random target
                                else if (stateData.IsWandering)
                                {
                                    targetPosition = FindRandomWalkableTargetInVision(translation.Value, targetData.SightRadius, seed, worldSize, gridSize, grid);
                                    targetData.Target = targetPosition;
                                    targetData.AtTarget = false;
                                }
                            }
                        }
                    }).WithDeallocateOnJobCompletion(grid)
                    .ScheduleParallel();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_gridNodeArray.IsCreated)
                _gridNodeArray.Dispose();
        }

        private static float3 GetNearestSideOfTargetTile(float3 currentPosition, float3 targetPosition, float tileSize)
        {
            if (currentPosition.x > targetPosition.x + tileSize / 2)
                targetPosition = new float3(targetPosition.x + tileSize / 2 + 1, targetPosition.y, targetPosition.z);
            else if (currentPosition.x < targetPosition.x - tileSize / 2)
                targetPosition = new float3(targetPosition.x - tileSize / 2 - 1, targetPosition.y, targetPosition.z);

            if (currentPosition.z > targetPosition.z + tileSize / 2)
                targetPosition = new float3(targetPosition.x, targetPosition.y, targetPosition.z + tileSize / 2 + 1);
            else if (currentPosition.z > targetPosition.z - tileSize / 2)
                targetPosition = new float3(targetPosition.x, targetPosition.y, targetPosition.z - tileSize / 2 - 1);

            return targetPosition;
        }

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

            return grid[x + y * gridSize.x].IsWalkable;
        }

        private static float3 FindRandomWalkableTargetInVision(float3 currentPosition, float sightRadius, float randomSeed, float2 worldSize, int2 gridSize, NativeArray<GridNode> grid)
        {
            float3 target = new float3(worldSize.x + currentPosition.x + 1, 0, worldSize.y + currentPosition.z + 1); //position off the map
            bool isTargetWalkable = false;
            float3 targetWorldPoint;

            // create random generator from seed
            Random randomGen = new Random((uint)randomSeed + 1);
            int timeout = 0; //iteration counter so after certain number of attempts to find a target the loop ends

            //find walkable targetWorldPoint
            while (!isTargetWalkable && timeout < 100)
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
            GridNode[,] grid = GridSetup.Instance.Grid;
            int2 gridSize = GridSetup.Instance.GridSize;
            int gridMaxSize = GridSetup.Instance.GridMaxSize;
            NativeArray<GridNode> gridNodeArray = new NativeArray<GridNode>(gridMaxSize, Allocator.Persistent);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    GridNode gridNode = new GridNode
                    {
                        X = x,
                        Y = y,

                        IsWalkable = grid[x, y].IsWalkable,
                        MovementPenalty = grid[x, y].MovementPenalty,
                        WorldPosition = grid[x, y].WorldPosition,
                    };

                    gridNodeArray[x + y * gridSize.x] = gridNode;
                }
            }

            return gridNodeArray;
        }
    }
}