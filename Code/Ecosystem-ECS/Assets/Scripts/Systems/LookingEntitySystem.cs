using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;


// this system must update in the end of frame
[UpdateBefore(typeof(EndFramePhysicsSystem)), UpdateAfter(typeof(StepPhysicsWorld))]
public class LookingEntitySystem : SystemBase
{
    // honestly, not sure why using ECB...... guess it is like some command buffer and allocate job order something like that
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
                //Foreach detected unitsGroup check we compare the unitsGroup node vs the one of the units
                for (int i = 0; i < hitsIndices.Length; i++)
                {
                    Entity childEntity = collisionWorld.Bodies[hitsIndices[i]].Entity;

                    float distanceToEntity = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[childEntity].Value);
                    int childEntityNumber = GetComponentDataFromEntity<ColliderTypeData>(true)[childEntity].ColliderTypeNumber;
                    // I know this code not good, when we finish findCloestEntity I will try to remove Duplicate code
                    if (colliderTypeData.ColliderTypeNumber == 1) // if you are Fox
                    {
                        if (childEntityNumber != colliderTypeData.ColliderTypeNumber)// to avoid fox find fox
                        {
                            if (childEntityNumber == 2)             //find rabbit and calculate the distance and compare distance with previous closetdistance and store it 
                            {
                                if (distanceToEntity < lookingEntityData.shortestToEdibleDistance)
                                {
                                    lookingEntityData.shortestToEdibleDistance = distanceToEntity;
                                    lookingEntityData.entityToEat = childEntity;
                                    lookingEntityData.edibleEntityCount += 1;
                                }
                            }
                            else if (childEntityNumber == 4)        // find water
                            {
                                if (distanceToEntity < lookingEntityData.shortestToWaterDistance)
                                {
                                    lookingEntityData.shortestToWaterDistance = distanceToEntity;
                                    lookingEntityData.entityToDrink = childEntity;
                                    lookingEntityData.waterEntityCount += 1;
                                }
                            }
                        }
                    }
                    else if (colliderTypeData.ColliderTypeNumber == 2) // if you are Rabbit
                    {
                        if (childEntityNumber != colliderTypeData.ColliderTypeNumber) //rabbit don't need to find rabbit now, will change late in find CloestMateEntity
                        {
                            if (childEntityNumber == 3)             // find Grass
                            {
                                if (distanceToEntity < lookingEntityData.shortestToEdibleDistance)
                                {
                                    lookingEntityData.shortestToEdibleDistance = distanceToEntity;
                                    lookingEntityData.entityToEat = childEntity;
                                    lookingEntityData.predatorEntityCount += 1;
                                }
                            }
                            else if (childEntityNumber == 4)        //find Water
                            {
                                if (distanceToEntity < lookingEntityData.shortestToWaterDistance)
                                {
                                    lookingEntityData.shortestToWaterDistance = distanceToEntity;
                                    lookingEntityData.entityToDrink = childEntity;
                                    lookingEntityData.waterEntityCount += 1;
                                }
                            }
                            else if (childEntityNumber == 1)        //find Fox
                            {
                                if (distanceToEntity < lookingEntityData.shortestToPredatorDistance)
                                {
                                    lookingEntityData.shortestToPredatorDistance = distanceToEntity;
                                    lookingEntityData.predatorEntity = childEntity;
                                    lookingEntityData.edibleEntityCount += 1;
                                }
                            }
                        }
                    }
                }
                // if some type of entity didn't find in this frame so just set up to entity.null
                // because statesystem will based on, for example predatorEntity is null or not to get into some states
                // so if no predatorEntity rabbit will go back to wanfering states something like that
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
        // not sure why....
        Dependency = JobHandle.CombineDependencies(Dependency, buildPhysicsWorld.GetOutputDependency());
        Dependency = JobHandle.CombineDependencies(Dependency, FindUnitsGroupInProximityJobHandle);
        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
