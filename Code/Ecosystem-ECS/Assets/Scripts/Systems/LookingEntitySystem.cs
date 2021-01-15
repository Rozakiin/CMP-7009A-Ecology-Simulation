using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;


// this system must update in the end of frame
namespace Systems
{
    [UpdateBefore(typeof(EndFramePhysicsSystem)), UpdateAfter(typeof(BuildPhysicsWorld))]
    public class LookingEntitySystem : SystemBase
    {
        BuildPhysicsWorld buildPhysicsWorld;


    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        CollisionWorld collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;

        Entities.ForEach((
            ref TargetData targetData,
            in ColliderTypeData colliderTypeData,
            in BasicNeedsData basicNeedsData,
            in EdibleData edibleData,
            in Translation translation
            ) =>
        {
            NativeList<int> hitsIndices = new NativeList<int>(Allocator.Temp);
            uint mask = 1 << 4;
            CollisionFilter filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = mask,
                GroupIndex = 0
            };

            // hit certain area 
            Aabb aabb = new Aabb
            {
                Min = translation.Value + new float3(-targetData.sightRadius, 0, -targetData.sightRadius),
                Max = translation.Value + new float3(targetData.sightRadius, 0, targetData.sightRadius)
            };

            OverlapAabbInput overlapAabbInput = new OverlapAabbInput
            {
                Aabb = aabb,
                Filter = filter,
            };

                // input is filter and range, out all entity who has collider, Aabb is very famous in website a way to detect 3D collision world
                if (collisionWorld.OverlapAabb(overlapAabbInput, ref hitsIndices))
                {
                    Entity entityToEat = Entity.Null;
                    Entity entityToDrink = Entity.Null;
                    Entity entityToPredator = Entity.Null;
                    Entity entityToMate = Entity.Null;
                    float shortestToEdibleDistance = float.PositiveInfinity;
                    float shortestToWaterDistance = float.PositiveInfinity;
                    float shortestToPredatorDistance = float.PositiveInfinity;
                    float shortestToMateDistance = float.PositiveInfinity;

                //Foreach detected unitsGroup check we compare the unitsGroup node vs the one of the units
                for (int i = 0; i < hitsIndices.Length; i++)
                {
                    Entity childEntity = collisionWorld.Bodies[hitsIndices[i]].Entity;

                    float distanceToEntity = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[childEntity].Value);
                    ColliderTypeData.ColliderType childEntityNumber = GetComponentDataFromEntity<ColliderTypeData>(true)[childEntity].colliderType;

                    // find food
                    if (HasComponent<EdibleData>(childEntity))
                    {
                        EdibleData childEdibleData = GetComponentDataFromEntity<EdibleData>(true)[childEntity];
                        // if foodtype in diet and not same type
                        if (((childEdibleData.foodType & (EdibleData.FoodType)basicNeedsData.diet) == childEdibleData.foodType) &&
                            (childEdibleData.canBeEaten) &&
                            (childEntityNumber != colliderTypeData.colliderType))
                        {
                            if (distanceToEntity < shortestToEdibleDistance)
                            {
                                shortestToEdibleDistance = distanceToEntity;
                                EntityToEat = childEntity;
                            }
                        }
                    }

                    //find drink
                    if (HasComponent<DrinkableData>(childEntity))
                    {
                        DrinkableData childDrinkableData = GetComponentDataFromEntity<DrinkableData>(true)[childEntity];
                        if (childDrinkableData.canBeDrunk)
                        {
                            if (distanceToEntity < shortestToWaterDistance)
                            {
                                shortestToWaterDistance = distanceToEntity;
                                EntityToDrink = childEntity;
                            }
                        }
                    }

                    //find mate
                    if (childEntityNumber == colliderTypeData.colliderType)
                    {
                        StateData childStateData = GetComponentDataFromEntity<StateData>(true)[childEntity];
                        BioStatsData.Gender childGender = GetComponentDataFromEntity<BioStatsData>(true)[childEntity].gender;
                        if ((childStateData.isPregnant == false) &&
                            (childGender == BioStatsData.Gender.Female) &&
                            (childStateData.isMating == false))
                        {
                            shortestToMateDistance = distanceToEntity;
                            EntityToMate = childEntity;
                        }
                    }

                    //find predator
                    if (HasComponent<BasicNeedsData>(childEntity))
                    {
                        BasicNeedsData.Diet childDiet = GetComponentDataFromEntity<BasicNeedsData>(true)[childEntity].diet;
                        // if child entity has diet that contains this entities foodtype ie predator
                        if ((((EdibleData.FoodType)childDiet & edibleData.foodType) == edibleData.foodType) &&
                            (childEntityNumber != colliderTypeData.colliderType))
                        {
                            if (distanceToEntity < shortestToPredatorDistance)
                            {
                                shortestToPredatorDistance = distanceToEntity;
                                EntityToPredator = childEntity;
                            }
                        }
                    }
                }
                // if some type of entity didn't find in this frame so just set up to entity.null
                // because statesystem will based on, for example predatorEntity is null or not to get into some states
                // so if no predatorEntity rabbit will go back to wanfering states something like that
                targetData.entityToEat = entityToEat;
                targetData.entityToDrink = entityToDrink;
                targetData.predatorEntity = entityToPredator;
                targetData.entityToMate = entityToMate;

                    //just test, delete to user version
                    targetData.shortestToEdibleDistance = shortestToEdibleDistance;
                    targetData.shortestToPredatorDistance = shortestToPredatorDistance;
                    targetData.shortestToWaterDistance = shortestToWaterDistance;
                    targetData.shortestToMateDistance = shortestToMateDistance;
                }
                hitsIndices.Dispose();
            }).ScheduleParallel();
        }
    }
}
