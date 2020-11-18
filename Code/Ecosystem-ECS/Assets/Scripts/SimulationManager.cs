﻿using System.Collections;
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

    #region Numbers for Entity Spawning 
    [Header("Map Data")]
    [SerializeField] public int numberOfRabbitsToSpawn = 0;
    [SerializeField] public int numberOfFoxesToSpawn = 0;
    [SerializeField] public int numberOfGrassToSpawn = 0;
    #endregion

    #region Map Data 
    [Header("Map Data")]
    [SerializeField] public string mapPath;
    public string mapString;
    [SerializeField] public int gridWidth;
    [SerializeField] public int gridHeight;
    [SerializeField] public Vector2 worldSize;
    [SerializeField] public Vector3 worldBottomLeft;
    [SerializeField] public float tileSize;
    [SerializeField] public float leftLimit;
    [SerializeField] public float upLimit;
    [SerializeField] public float rightLimit;
    [SerializeField] public float downLimit;
    #endregion

    #region Initialisation
    // Start is called before the first frame update
    void Start()
    {
        // Only continue if no errors creating the map
        if (CreateMap())
        {
            CreateEntitiesFromGameObject(grass, numberOfGrassToSpawn);
        }
        else
        {
            Debug.Log("Error Loading Map");
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Map Creation Methods
    // Creates the map with entities
    private bool CreateMap()
    {
        List<List<MapReader.TerrainCost>> mapList = new List<List<MapReader.TerrainCost>>();
        if (mapPath != "")
        {
            if (MapReader.ReadInMapFromFile(mapPath, ref mapList))
                CreateEntityTilesFromMapList(in mapList);
            else
                return false;
        }
        else
        {
            //if (MapReader.ReadInMapFromString(mapString, ref mapList))
            //    CreateEntityTilesFromMapList(in mapList);
            //else
            //    return false;
        }

        SetLimits();
        return true;
    }

    private void CreateEntityTilesFromMapList(in List<List<MapReader.TerrainCost>> mapList)
    {
        // Set world map data
        gridWidth = mapList[0].Count;
        gridHeight = mapList.Count;
        tileSize = grassTile.GetComponent<Renderer>().bounds.size.x;                   //Get the width of the tile
        worldSize.x = gridWidth * tileSize;
        worldSize.y = gridHeight * tileSize;
        worldBottomLeft = transform.position - Vector3.right * worldSize.x / 2 - Vector3.forward * worldSize.y / 2;//Get the real world position of the bottom left of the grid.

        // Create entity prefabs from the game objects hierarchy once
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var entityGrassTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(grassTile, settings);
        var entityWaterTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(waterTile, settings);
        var entitySandTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(sandTile, settings);
        var entityRockTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(rockTile, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * tileSize + tileSize / 2) + Vector3.forward * (y * tileSize + tileSize / 2);//Get the world co ordinates of the tile from the bottom left of the graph
                Entity instance;

                switch (mapList[y][x])
                {
                    case MapReader.TerrainCost.Water:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        instance = entityManager.Instantiate(entityWaterTile);

                        // Place the instantiated entity in position on the map
                        //var position = transform.TransformPoint(new float3(i, noise.cnoise(new float2(i) * 0.21F) * 2, i * 1.3F));
                        //Set Component Data for the entity
                        entityManager.SetComponentData(instance, new Translation { Value = worldPoint }); // set position data (called translation in ECS)
                        //tileClone.name += y + "" + x;
                        //tileClone.layer = 8; //set layer to unwalkable
                        break;
                    case MapReader.TerrainCost.Grass:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        instance = entityManager.Instantiate(entityGrassTile);

                        // Place the instantiated entity in position on the map
                        //var position = transform.TransformPoint(new float3(i, noise.cnoise(new float2(i) * 0.21F) * 2, i * 1.3F));
                        //Set Component Data for the entity
                        entityManager.SetComponentData(instance, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 

                        //tileClone.name += y + "" + x;
                        //tileClone.layer = 9; //set layer to grass
                        break;
                    case MapReader.TerrainCost.Sand:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        instance = entityManager.Instantiate(entitySandTile);

                        // Place the instantiated entity in position on the map
                        //var position = transform.TransformPoint(new float3(i, noise.cnoise(new float2(i) * 0.21F) * 2, i * 1.3F));
                        //Set Component Data for the entity
                        entityManager.SetComponentData(instance, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 

                        //tileClone.name += y + "" + x;
                        //tileClone.layer = 10; //set layer to grass
                        break;
                    case MapReader.TerrainCost.Rock:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        instance = entityManager.Instantiate(entityRockTile);

                        // Place the instantiated entity in position on the map
                        //var position = transform.TransformPoint(new float3(i, noise.cnoise(new float2(i) * 0.21F) * 2, i * 1.3F));
                        //Set Component Data for the entity
                        entityManager.SetComponentData(instance, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 

                        //tileClone.name += y + "" + x;
                        //tileClone.layer = 11; //set layer to grass
                        break;
                    default:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        instance = entityManager.Instantiate(entityGrassTile);

                        // Place the instantiated entity in position on the map
                        //var position = transform.TransformPoint(new float3(i, noise.cnoise(new float2(i) * 0.21F) * 2, i * 1.3F));
                        //Set Component Data for the entity
                        entityManager.SetComponentData(instance, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        //entityManager.SetSharedComponentData<RenderMesh>(entity, new RenderMesh
                        //{
                        //    mesh = theMesh,
                        //    material = theMaterial,
                        //    subMesh = 0,
                        //    layer = 0, // Here
                        //    castShadows = ShadowCastingMode.On,
                        //    receiveShadows = true
                        //});
                        Debug.Log("Unknown TerrainCost value");
                        break;
                }
            }
        }
    }

    private void SetLimits()
    {
        //TODO
        //upLimit = (float)(gridHeight - 1) * tileSize;
        //leftLimit = 0;
        //rightLimit = (float)(gridWidth - 1) * tileSize;
        //downLimit = 0;
    }
    #endregion

    #region Entity Spawning
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

            // Calc random point on map
            int randWidth = UnityEngine.Random.Range(0, (int)gridWidth - 1);
            int randHeight = UnityEngine.Random.Range(0, (int)gridHeight - 1);
            // Get the world co ordinates from the bottom left of the graph
            Vector3 worldPoint = worldBottomLeft + Vector3.right * (randWidth * tileSize + tileSize / 2) + Vector3.forward * (randHeight * tileSize + tileSize / 2); 

            // Place the instantiated entity in a random point on the map
            //Set Component Data for the entity
            entityManager.SetComponentData(instance, new Translation { Value = worldPoint }); // set position data (called translation in ECS)
        }


        //        int randWidth = Random.Range(0, (int)gridWidth-1);
        //        int randHeight = Random.Range(0, (int)gridHeight-1);
        //        Vector3 worldPoint = worldBottomLeft + Vector3.right * (randWidth * tileSize + tileSize/2) + Vector3.forward * (randHeight * tileSize + tileSize/2);//Get the world co ordinates of the rabbit from the bottom left of the graph

        //        GameObject rabbitClone = Instantiate(rabbit, worldPoint, rabbit.transform.rotation) as GameObject;
        //        rabbitCount++;
        //        rabbitList.Add(rabbitClone.GetComponent<Rabbit>());
        //        rabbitClone.transform.parent = rabbitContainer.transform;
        //        rabbitClone.name = "RabbitClone" + rabbitCount;
    }
    #endregion

}
