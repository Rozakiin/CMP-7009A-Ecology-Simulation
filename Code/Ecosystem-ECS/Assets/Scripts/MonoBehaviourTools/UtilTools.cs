using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                // see hit.Position
                // see hit.SurfaceNormal
                Entity e = physicsWorld.Bodies[hit.RigidBodyIndex].Entity;
                return e;
            }
            return Entity.Null;
        }
    }
    public static class GridTools
    {
        // Returns false is collides with unwalkabletile or doesnt collide with a tile
        public static bool IsWorldPointOnWalkableTile(float3 worldPoint, EntityManager entityManager)
        {
            float3 tempUp = worldPoint + math.up() * Mathf.Infinity;
            float3 tempDown = worldPoint + math.up() * -Mathf.Infinity;
            CollisionFilter tempTileFilter = new CollisionFilter { BelongsTo = ~0u, CollidesWith = 1 >> 0, GroupIndex = 0 }; //filter to only collide with tiles
            //raycast from positive infinity to negative infinity, colliding with only tiles
            Entity collidedEntity = PhysicsTools.GetEntityFromRaycast(tempUp, tempDown, tempTileFilter);

            if (collidedEntity != Entity.Null && entityManager.HasComponent<TerrainTypeData>(collidedEntity))
            {
                return entityManager.GetComponentData<TerrainTypeData>(collidedEntity).isWalkable;
            }

            return false;
        }
    }
}

