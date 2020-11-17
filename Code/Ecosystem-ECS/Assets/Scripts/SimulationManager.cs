using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public int numberOfRabbitsToSpawn = 0;
    public GameObject grass;

    // Start is called before the first frame update
    void Start()
    {
        // Create entity prefab from the game object hierarchy once
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var grassEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(grass, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


        for (int i = 0; i < numberOfRabbitsToSpawn; i++)
        {
            // Efficiently instantiate a bunch of entities from the already converted entity prefab
            var instance = entityManager.Instantiate(grassEntity);

            // Place the instantiated entity in a grid with some noise
            var position = transform.TransformPoint(new float3(i, noise.cnoise(new float2(i) * 0.21F) * 2, i * 1.3F));
            entityManager.SetComponentData(instance, new Translation { Value = position });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
