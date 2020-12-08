using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;



[UpdateBefore(typeof(EndFramePhysicsSystem)), UpdateAfter(typeof(StepPhysicsWorld))]
public class LookingEntitySystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        BuildPhysicsWorld buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
        CollisionWorld collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
        JobHandle FindUnitsGroupInProximityJobHandle = Entities.WithAll<MovementData>().ForEach((
            ref LookingEntityData lookingEntityData
            , in TargetData targetData
            , in ColliderTypeData colliderTypeData
            , in Translation translation) =>
        {
            NativeList<int> hitsIndices = new NativeList<int>(Allocator.Temp);
            CollisionFilter filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0
            };

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
            if (collisionWorld.OverlapAabb(overlapAabbInput, ref hitsIndices))
            {
                float shortestToEdibleDistance = 100f; // set initial distance to 100, must more than sight radius
                float shortestToWaterDistance = 100f;
                float shortestToPredatorDistance = 100f;// not sure it is better store parameter in here or componentdata

                //Foreach detected unitsGroup check we compare the unitsGroup node vs the one of the units
                for (int i = 0; i < hitsIndices.Length; i++)
                {
                    Entity childEntity = collisionWorld.Bodies[hitsIndices[i]].Entity;

                    float distanceToEntity = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[childEntity].Value);
                    int childEntityNumber = GetComponentDataFromEntity<ColliderTypeData>(true)[childEntity].ColliderTypeNumber;
                    if (colliderTypeData.ColliderTypeNumber == 1) // if you are Fox
                    {
                        if (childEntityNumber != colliderTypeData.ColliderTypeNumber)
                        {
                            if (childEntityNumber == 2)
                            {
                                if (distanceToEntity < shortestToEdibleDistance)
                                {
                                    shortestToEdibleDistance = distanceToEntity;
                                    lookingEntityData.entityToEat = childEntity;
                                    lookingEntityData.edibleEntityCount += 1;
                                }
                            }
                            else if (childEntityNumber == 4)
                            {
                                if (distanceToEntity < shortestToWaterDistance)
                                {
                                    shortestToWaterDistance = distanceToEntity;
                                    lookingEntityData.entityToDrink = childEntity;
                                    lookingEntityData.waterEntityCount += 1;
                                }
                            }
                        }
                    }
                    else if (colliderTypeData.ColliderTypeNumber == 2)
                    {
                        if (childEntityNumber != colliderTypeData.ColliderTypeNumber)
                        {
                            if (childEntityNumber == 3)
                            {
                                if (distanceToEntity < shortestToEdibleDistance)
                                {
                                    shortestToEdibleDistance = distanceToEntity;
                                    lookingEntityData.entityToEat = childEntity;
                                    lookingEntityData.predatorEntityCount += 1;
                                }
                            }
                            else if (childEntityNumber == 4)
                            {
                                if (distanceToEntity < shortestToWaterDistance)
                                {
                                    shortestToWaterDistance = distanceToEntity;
                                    lookingEntityData.entityToDrink = childEntity;
                                    lookingEntityData.waterEntityCount += 1;
                                }
                            }
                            else if (childEntityNumber == 1)
                            {
                                if (distanceToEntity < shortestToPredatorDistance)
                                {
                                    shortestToPredatorDistance = distanceToEntity;
                                    lookingEntityData.predatorEntity = childEntity;
                                    lookingEntityData.edibleEntityCount += 1;
                                }
                            }
                        }
                    }
                }
                if (lookingEntityData.edibleEntityCount == 0)
                {
                    lookingEntityData.entityToEat = Entity.Null;
                }
                if (lookingEntityData.waterEntityCount == 0)
                {
                    lookingEntityData.entityToDrink = Entity.Null;
                }
                if (lookingEntityData.predatorEntityCount == 0)
                {
                    lookingEntityData.predatorEntity = Entity.Null;
                }
                lookingEntityData.edibleEntityCount = 0;
                lookingEntityData.waterEntityCount = 0;
                lookingEntityData.predatorEntityCount = 0;
            }
            hitsIndices.Dispose();
        }).ScheduleParallel(Dependency);

        Dependency = JobHandle.CombineDependencies(Dependency, buildPhysicsWorld.GetOutputDependency());
        Dependency = JobHandle.CombineDependencies(Dependency, FindUnitsGroupInProximityJobHandle);
        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
