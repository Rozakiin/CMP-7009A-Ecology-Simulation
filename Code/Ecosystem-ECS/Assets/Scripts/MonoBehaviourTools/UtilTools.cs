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
        public static int GaussianDistribution(float sigma, float mu, float randomSeed)
        {

            float x1, x2, w, y1; //, y2;
            Random randomGen = new Random((uint)randomSeed + 1);
            do
            {
                x1 = (float)(2f * randomGen.NextDouble() - 1f);
                x2 = 2f * (float)randomGen.NextDouble() - 1f;
                w = x1 * x1 + x2 * x2;
            } while (w >= 1f);

            w = math.sqrt((-2f * math.log(w)) / w);
            y1 = x1 * w;
            // y2 = x2 * w;
            return (int)((y1 * sigma) + mu);
        }
    }
}
