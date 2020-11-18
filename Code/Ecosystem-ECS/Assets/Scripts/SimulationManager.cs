using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    #region GameObjects
    [Header("GameObjects")]
    [SerializeField] public GameObject grassTile;
    [SerializeField] public GameObject lightGrassTile;
    [SerializeField] public GameObject waterTile;
    [SerializeField] public GameObject sandTile;
    [SerializeField] public GameObject rockTile;
    [SerializeField] public GameObject rabbit;
    [SerializeField] public GameObject grass;
    #endregion

    [SerializeField] public int numberOfRabbitsToSpawn = 0;
    [SerializeField] public int numberOfFoxesToSpawn = 0;
    [SerializeField] public int numberOfGrassToSpawn = 0;

    // Start is called before the first frame update
    void Start()
    {
        CreateEntitiesFromGameObject(grass, numberOfGrassToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CreateMap()
    {
        return true;
    }

    // Creates entities from a gameobject in a given quantity
    private void CreateEntitiesFromGameObject(GameObject gameObject, int quantity)
    {
        // Create entity prefab from the game object hierarchy once
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObject, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


        for (int i = 0; i < quantity; i++)
        {
            // Efficiently instantiate a bunch of entities from the already converted entity prefab
            var instance = entityManager.Instantiate(entity);

            // Place the instantiated entity in a grid with some noise
            var position = transform.TransformPoint(new float3(i, noise.cnoise(new float2(i) * 0.21F) * 2, i * 1.3F));
            //Set Component Data for the entity
            entityManager.SetComponentData(instance, new Translation { Value = position }); // set position data (called translation in ECS)
        }
    }


}
