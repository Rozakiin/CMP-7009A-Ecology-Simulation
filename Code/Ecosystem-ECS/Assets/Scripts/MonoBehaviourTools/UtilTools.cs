using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

namespace UtilTools
{
    public static class PhysicsTools
    {
        /*
         * Adapted From https://docs.unity3d.com/Packages/com.unity.physics@0.5/manual/collision_queries.html
         * Casts ray and returns an entity it hits
         */
        public static Entity GetEntityFromRaycast(float3 rayFrom, float3 rayTo, CollisionFilter filter)
        {
            var buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
            var physicsWorld = buildPhysicsWorld.PhysicsWorld;
            var world = physicsWorld.CollisionWorld;
            var input = new RaycastInput()
            {
                Start = rayFrom,
                End = rayTo,
                Filter = filter
            };

            return world.CastRay(input, out RaycastHit hit) ? physicsWorld.Bodies[hit.RigidBodyIndex].Entity : Entity.Null;
        }
    }

    public static class GridTools
    {
        /* Returns false is collides with unwalkabletile or doesnt collide with a tile*/
        public static bool IsWorldPointOnWalkableTile(float3 worldPoint, EntityManager entityManager)
        {
            float3 tempUp = worldPoint + math.up() * 10000;
            float3 tempDown = worldPoint + math.up() * -10000;
            var tempTileFilter = new CollisionFilter { BelongsTo = ~0u, CollidesWith = 1 >> 0, GroupIndex = 0 }; //filter to only collide with tiles
            //raycast from positive 10,000 to negative 10,000, colliding with only tiles
            var collidedEntity = PhysicsTools.GetEntityFromRaycast(tempUp, tempDown, tempTileFilter);

            return entityManager.HasComponent<TerrainTypeData>(collidedEntity) && entityManager.GetComponentData<TerrainTypeData>(collidedEntity).IsWalkable;
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
