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
            ref TargetData targetData
            , in ColliderTypeData colliderTypeData
            , in ReproductiveData reproductiveData
            , in StateData stateData
            , in Translation translation) =>
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
                Entity EntityToEat = Entity.Null;
                Entity EntityToDrink = Entity.Null;
                Entity EntityToPredator = Entity.Null;
                Entity EntityToMate = Entity.Null;
                float shortestToEdibleDistance = 100f;
                float shortestToWaterDistance = 100f;
                float shortestToPredatorDistance = 100f;
                float shortestToMateDistance = 100f;

                //Foreach detected unitsGroup check we compare the unitsGroup node vs the one of the units
                for (int i = 0; i < hitsIndices.Length; i++)
                {
                    Entity childEntity = collisionWorld.Bodies[hitsIndices[i]].Entity;

                    float distanceToEntity = math.distance(translation.Value, GetComponentDataFromEntity<Translation>(true)[childEntity].Value);
                    ColliderTypeData.ColliderType childEntityNumber = GetComponentDataFromEntity<ColliderTypeData>(true)[childEntity].colliderType;
                    // I know this code not good, when we finish findCloestEntity I will try to remove Duplicate code
                    if (colliderTypeData.colliderType == ColliderTypeData.ColliderType.Fox) // if you are Fox
                    {
                        if (childEntityNumber != ColliderTypeData.ColliderType.Fox)// to avoid fox find fox, fox don't have mate system
                        {
                            if (childEntityNumber == ColliderTypeData.ColliderType.Rabbit && stateData.state == StateData.States.Hungry)             //find rabbit and calculate the distance and compare distance with previous closetdistance and store it 
                            {
                                if (distanceToEntity < shortestToEdibleDistance)
                                {
                                    shortestToEdibleDistance = distanceToEntity;
                                    EntityToEat = childEntity;
                                }
                            }
                            else if (childEntityNumber == ColliderTypeData.ColliderType.Water && stateData.state == StateData.States.Thirsty)        // find water
                            {
                                if (distanceToEntity < shortestToWaterDistance)
                                {
                                    shortestToWaterDistance = distanceToEntity;
                                    EntityToDrink = childEntity;
                                }
                            }
                        }
                    }
                    else // if you are Rabbit
                    {
                        if (childEntityNumber != ColliderTypeData.ColliderType.Rabbit) //rabbit don't need to find rabbit now, will change late in find CloestMateEntity
                        {
                            if (childEntityNumber == ColliderTypeData.ColliderType.Grass && stateData.state == StateData.States.Hungry)             // find Grass
                            {
                                if (distanceToEntity < shortestToEdibleDistance)
                                {
                                    shortestToEdibleDistance = distanceToEntity;
                                    EntityToEat = childEntity;
                                }
                            }
                            else if (childEntityNumber == ColliderTypeData.ColliderType.Water && stateData.state == StateData.States.Thirsty)        //find Water
                            {
                                if (distanceToEntity < shortestToWaterDistance)
                                {
                                    shortestToWaterDistance = distanceToEntity;
                                    EntityToDrink = childEntity;
                                }
                            }

                            else if (childEntityNumber == ColliderTypeData.ColliderType.Fox)        //find Fox
                            {
                                if (distanceToEntity < shortestToPredatorDistance)
                                {
                                    shortestToPredatorDistance = distanceToEntity;
                                    EntityToPredator = childEntity;
                                }
                            }
                        }
                        else
                        {
                            if (stateData.state == StateData.States.SexuallyActive)
                            {
                                if ((GetComponentDataFromEntity<ReproductiveData>(true)[childEntity].pregnant == false) && (GetComponentDataFromEntity<BioStatsData>(true)[childEntity].gender == BioStatsData.Gender.Female))
                                {
                                    if (distanceToEntity < shortestToMateDistance)
                                    {
                                        shortestToMateDistance = distanceToEntity;
                                        EntityToMate = childEntity;
                                    }
                                }
                            }
                        }
                    }
                }
                // if some type of entity didn't find in this frame so just set up to entity.null
                // because statesystem will based on, for example predatorEntity is null or not to get into some states
                // so if no predatorEntity rabbit will go back to wanfering states something like that
                targetData.entityToEat = EntityToEat;
                targetData.entityToDrink = EntityToDrink;
                targetData.predatorEntity = EntityToPredator;
                targetData.entityToMate = EntityToMate;

                //just test, delete to user version
                targetData.shortestToEdibleDistance = shortestToEdibleDistance;
                targetData.shortestToPredatorDistance = shortestToPredatorDistance;
                targetData.shortestToWaterDistance = shortestToWaterDistance;
                targetData.shortestToMateDistance = shortestToMateDistance;
            }
            hitsIndices.Dispose();
           
        }).ScheduleParallel(Dependency);
        // not sure why....
        Dependency = JobHandle.CombineDependencies(Dependency, buildPhysicsWorld.GetOutputDependency());
        Dependency = JobHandle.CombineDependencies(Dependency, FindUnitsGroupInProximityJobHandle);
        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
