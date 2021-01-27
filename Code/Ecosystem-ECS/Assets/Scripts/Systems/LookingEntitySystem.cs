using Components;
using MonoBehaviourTools.UI;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;


// this system must update in the end of frame
namespace Systems
{
    [UpdateBefore(typeof(EndFramePhysicsSystem))]
    [UpdateAfter(typeof(BuildPhysicsWorld))]
    public class LookingEntitySystem : SystemBase
    {
        private BuildPhysicsWorld _buildPhysicsWorld;


        protected override void OnCreate()
        {
            base.OnCreate();
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        }

        /*
         * finds closest entity of edible, drinkable, same animal type, and predator
         */
        protected override void OnUpdate()
        {
            //catch to not run if paused
            if (UITimeControl.Instance.GetPause()) return;

            var collisionWorld = _buildPhysicsWorld.PhysicsWorld.CollisionWorld;

            Entities.ForEach((
                ref TargetData targetData,
                in ColliderTypeData colliderTypeData,
                in BasicNeedsData basicNeedsData,
                in EdibleData edibleData,
                in StateData stateData,
                in Translation translation
            ) =>
            {
                var hitsIndices = new NativeList<int>(Allocator.Temp);
                const uint mask = 1 << 4;
                var filter = new CollisionFilter
                {
                    BelongsTo = ~0u,
                    CollidesWith = mask,
                    GroupIndex = 0
                };

                // hit certain area 
                var aabb = new Aabb
                {
                    Min = translation.Value + new float3(-targetData.SightRadius, 0, -targetData.SightRadius),
                    Max = translation.Value + new float3(targetData.SightRadius, 0, targetData.SightRadius)
                };

                var overlapAabbInput = new OverlapAabbInput
                {
                    Aabb = aabb,
                    Filter = filter
                };

                // input is filter and range, out all entity who has collider, Aabb is very famous in website a way to detect 3D collision world
                if (collisionWorld.OverlapAabb(overlapAabbInput, ref hitsIndices))
                {
                    var entityToEat = Entity.Null;
                    var entityToDrink = Entity.Null;
                    var entityToPredator = Entity.Null;
                    var entityToMate = Entity.Null;
                    var shortestToEdibleDistance = float.PositiveInfinity;
                    var shortestToWaterDistance = float.PositiveInfinity;
                    var shortestToPredatorDistance = float.PositiveInfinity;
                    var shortestToMateDistance = float.PositiveInfinity;

                    //Foreach detected unitsGroup check we compare the unitsGroup node vs the one of the units
                    for (var i = 0; i < hitsIndices.Length; i++)
                    {
                        var childEntity = collisionWorld.Bodies[i].Entity;

                        var distanceToEntity = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[childEntity].Value);
                        var childEntityNumber = GetComponentDataFromEntity<ColliderTypeData>(true)[childEntity].Collider;

                        // find food
                        if (HasComponent<EdibleData>(childEntity))
                        {
                            var childEdibleData = GetComponentDataFromEntity<EdibleData>(true)[childEntity];
                            // if foodtype in DietType and not same type
                            if ((childEdibleData.FoodType & (EdibleData.FoodTypes) basicNeedsData.Diet) == childEdibleData.FoodType &&
                                childEdibleData.CanBeEaten &&
                                childEntityNumber != colliderTypeData.Collider)
                                if (distanceToEntity < shortestToEdibleDistance)
                                {
                                    shortestToEdibleDistance = distanceToEntity;
                                    entityToEat = childEntity;
                                }
                        }

                        //find drink
                        if (HasComponent<DrinkableData>(childEntity))
                        {
                            var childDrinkableData =
                                GetComponentDataFromEntity<DrinkableData>(true)[childEntity];
                            if (childDrinkableData.CanBeDrunk)
                                if (distanceToEntity < shortestToWaterDistance)
                                {
                                    shortestToWaterDistance = distanceToEntity;
                                    entityToDrink = childEntity;
                                }
                        }

                        //find mate
                        if (childEntityNumber == colliderTypeData.Collider)
                        {
                            var childStateData = GetComponentDataFromEntity<StateData>(true)[childEntity];
                            var childBioStatsData = GetComponentDataFromEntity<BioStatsData>(true)[childEntity];
                            if (childBioStatsData.Gender == BioStatsData.Genders.Female &&
                                childBioStatsData.AgeGroup == BioStatsData.AgeGroups.Adult &&
                                !childStateData.IsPregnant &&
                                !childStateData.IsMating &&
                                !childStateData.IsGivingBirth)
                                if (distanceToEntity < shortestToMateDistance)
                                {
                                    //if not currently mating change to that closer entity, if mating keep current mate
                                    if (!stateData.IsMating)
                                    {
                                        shortestToMateDistance = distanceToEntity;
                                        entityToMate = childEntity;
                                    }
                                    else
                                    {
                                        //recalc distance
                                        if (HasComponent<Translation>(targetData.EntityToMate))
                                        {
                                            shortestToMateDistance = math.distance(translation.Value,
                                                GetComponentDataFromEntity<Translation>(true)[targetData.EntityToMate].Value);
                                            entityToMate = targetData.EntityToMate;
                                        }
                                    }
                                }
                        }

                        //find predator
                        if (HasComponent<BasicNeedsData>(childEntity))
                        {
                            var childDietType =
                                GetComponentDataFromEntity<BasicNeedsData>(true)[childEntity].Diet;
                            // if child entity has DietType that contains this entities foodtype ie predator
                            if (((EdibleData.FoodTypes) childDietType & edibleData.FoodType) == edibleData.FoodType &&
                                childEntityNumber != colliderTypeData.Collider)
                                if (distanceToEntity < shortestToPredatorDistance)
                                {
                                    shortestToPredatorDistance = distanceToEntity;
                                    entityToPredator = childEntity;
                                }
                        }
                    }

                    // if some type of entity didn't find in this frame so just set up to entity.null
                    // because statesystem will based on, for example predatorEntity is null or not to get into some states
                    // so if no predatorEntity rabbit will go back to wanfering states something like that
                    targetData.EntityToEat = entityToEat;
                    targetData.EntityToDrink = entityToDrink;
                    targetData.PredatorEntity = entityToPredator;
                    targetData.EntityToMate = entityToMate;

                    //just test, delete to user version
                    targetData.ShortestDistanceToEdible = shortestToEdibleDistance;
                    targetData.ShortestDistanceToPredator = shortestToPredatorDistance;
                    targetData.ShortestDistanceToWater = shortestToWaterDistance;
                    targetData.ShortestDistanceToMate = shortestToMateDistance;
                }

                hitsIndices.Dispose();
            }).ScheduleParallel();
        }
    }
}