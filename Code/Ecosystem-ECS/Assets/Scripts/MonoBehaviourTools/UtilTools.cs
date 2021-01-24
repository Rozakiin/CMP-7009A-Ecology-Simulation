using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace UtilTools
{
    public static class PhysicsTools
    {
        // Adapted From https://docs.unity3d.com/Packages/com.unity.physics@0.5/manual/collision_queries.html
        public static Entity GetEntityFromRaycast(float3 RayFrom, float3 RayTo, CollisionFilter filter)
        {
            BuildPhysicsWorld buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
            PhysicsWorld physicsWorld = buildPhysicsWorld.PhysicsWorld;
            CollisionWorld world = physicsWorld.CollisionWorld;
            RaycastInput input = new RaycastInput()
            {
                Start = RayFrom,
                End = RayTo,
                Filter = filter
            };

            if (world.CastRay(input, out Unity.Physics.RaycastHit hit))
            {
                return physicsWorld.Bodies[hit.RigidBodyIndex].Entity;
            }

            return Entity.Null;
        }
    }

    public static class GridTools
    {
        // Returns false is collides with unwalkabletile or doesnt collide with a tile
        public static bool IsWorldPointOnWalkableTile(float3 worldPoint, EntityManager entityManager)
        {
            float3 tempUp = worldPoint + math.up() * 10000;
            float3 tempDown = worldPoint + math.up() * -10000;
            CollisionFilter tempTileFilter = new CollisionFilter { BelongsTo = ~0u, CollidesWith = 1 >> 0, GroupIndex = 0 }; //filter to only collide with tiles
            //raycast from positive 10,000 to negative 10,000, colliding with only tiles
            Entity collidedEntity = PhysicsTools.GetEntityFromRaycast(tempUp, tempDown, tempTileFilter);

            if (entityManager.HasComponent<TerrainTypeData>(collidedEntity))
            {
                return entityManager.GetComponentData<TerrainTypeData>(collidedEntity).isWalkable;
            }

            return false;
        }
    }

    public static class ComponentTools
    {
        //Returns if flagStates contains a given state
        public static bool ContainsState(StateData.FlagStates state, StateData.FlagStates stateDataflagStates)
        {
            return (stateDataflagStates & state) == state;
        }

        // mu is average, max = sigma + mu; min = sigma-mu
        public static int GaussianDistribution(int sigma, int mu, float randomSeed)
        {
            Random randomGen = new Random((uint)randomSeed + 1);

            double x1 = 1f - randomGen.NextDouble();
            double x2 = 1f - randomGen.NextDouble();

            double randStdNormal = (math.sqrt(-2.0 * math.log(x1)) * math.sin(2.0 * math.PI * x2)) * sigma + mu;

            return (int)math.round((int)randStdNormal);
        }
    }

}
