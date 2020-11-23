using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    EntityManager entityManager;
    public static SimulationManager Instance;

    #region GameObjects
    [Header("GameObjects")]
    [SerializeField] public GameObject grassTile;
    [SerializeField] public GameObject lightGrassTile;
    [SerializeField] public GameObject waterTile;
    [SerializeField] public GameObject sandTile;
    [SerializeField] public GameObject rockTile;
    [SerializeField] public GameObject rabbit;
    [SerializeField] public GameObject grass;
    public GameObject collisionPlaneForMap;
    #endregion

    #region Numbers for Entity Spawning 
    [Header("Map Data")]
    [SerializeField] public int numberOfRabbitsToSpawn = 0;
    [SerializeField] public int numberOfFoxesToSpawn = 0;
    [SerializeField] public int numberOfGrassToSpawn = 0;
    #endregion

    #region Map Data 
    [Header("Map Data")]
    public static string mapPath = "Assets/Scripts/MonoBehaviourTools/Map/MapExample.txt";
    public static string mapString;
    public static int gridWidth;
    public static int gridHeight;
    public static Vector2 worldSize;
    public static Vector3 worldBottomLeft;
    public static float tileSize;
    public static float leftLimit;
    public static float upLimit;
    public static float rightLimit;
    public static float downLimit;
    #endregion

    #region Initialisation
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        // Only continue if no errors creating the map
        if (CreateMap())
        {
            CreateEntitiesFromGameObject(grass, numberOfGrassToSpawn);
            CreateEntitiesFromGameObject(rabbit, numberOfRabbitsToSpawn);
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
        if (mapPath != null)
        {
            if (MapReader.ReadInMapFromFile(mapPath, ref mapList))
                CreateEntityTilesFromMapList(in mapList);
            else
                return false;
        }
        else if (mapString != null)
        {
            if (MapReader.ReadInMapFromString(mapString, ref mapList))
                CreateEntityTilesFromMapList(in mapList);
            else
                return false;
        }
        else
        {
            return false;
        }

        // Create a GameObject the size of the map with collider for ray hits
        collisionPlaneForMap = new GameObject();
        collisionPlaneForMap.transform.position = transform.position;
        collisionPlaneForMap.AddComponent<BoxCollider>();
        BoxCollider collider = collisionPlaneForMap.GetComponent<BoxCollider>();
        collider.size = new Vector3(worldSize.x,0,worldSize.y);

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


        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * tileSize + tileSize / 2) + Vector3.forward * (y * tileSize + tileSize / 2);//Get the world co ordinates of the tile from the bottom left of the graph
                Entity prototypeTile;

                switch (mapList[y][x])
                {
                    case MapReader.TerrainCost.Water:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = entityManager.Instantiate(entityWaterTile);

                        // Place the instantiated entity in position on the map
                        //var position = transform.TransformPoint(new float3(i, noise.cnoise(new float2(i) * 0.21F) * 2, i * 1.3F));
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS)
                        entityManager.SetName(prototypeTile, "WaterTile "+y + "," + x);
                        //tileClone.name += y + "" + x;
                        //tileClone.layer = 8; //set layer to unwalkable
                        break;
                    case MapReader.TerrainCost.Grass:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = entityManager.Instantiate(entityGrassTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        entityManager.SetName(prototypeTile, "GrassTile " + y + "," + x);

                        //tileClone.name += y + "" + x;
                        //tileClone.layer = 9; //set layer to grass
                        break;
                    case MapReader.TerrainCost.Sand:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = entityManager.Instantiate(entitySandTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        entityManager.SetName(prototypeTile, "SandTile " + y + "," + x);

                        //tileClone.name += y + "" + x;
                        //tileClone.layer = 10; //set layer to grass
                        break;
                    case MapReader.TerrainCost.Rock:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = entityManager.Instantiate(entityRockTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        entityManager.SetName(prototypeTile, "RockTile " + y + "," + x);

                        //tileClone.name += y + "" + x;
                        //tileClone.layer = 11; //set layer to grass
                        break;
                    default:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = entityManager.Instantiate(entityGrassTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
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

        entityManager.DestroyEntity(entityGrassTile);
        entityManager.DestroyEntity(entityWaterTile);
        entityManager.DestroyEntity(entitySandTile);
        entityManager.DestroyEntity(entityRockTile);


    }

    private void SetLimits()
    {
        leftLimit = -worldSize.x / 2;
        rightLimit = worldSize.x / 2;
        downLimit = -worldSize.y / 2;
        upLimit = worldSize.y / 2;
    }
    #endregion

    #region Entity Spawning
    // Creates entities from a gameobject in a given quantity
    private void CreateEntitiesFromGameObject(GameObject gameObject, int quantity)
    {
        // Create entity prefab from the game object hierarchy once
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var convertedEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObject, settings);


        for (int i = 0; i < quantity; i++)
        {
            // Efficiently instantiate a bunch of entities from the already converted entity prefab
            var prototypeEntity = entityManager.Instantiate(convertedEntity);

            // Calc random point on map
            int randWidth = UnityEngine.Random.Range(0, (int)gridWidth - 1);
            int randHeight = UnityEngine.Random.Range(0, (int)gridHeight - 1);
            // Get the world co ordinates from the bottom left of the graph
            Vector3 worldPoint = worldBottomLeft + Vector3.right * (randWidth * tileSize + tileSize / 2) + Vector3.forward * (randHeight * tileSize + tileSize / 2); 

            // Place the instantiated entity in a random point on the map
            //Set Component Data for the entity
            entityManager.SetComponentData(prototypeEntity, new Translation { Value = worldPoint }); // set position data (called translation in ECS)
            entityManager.SetName(prototypeEntity, gameObject.name + i); // set name
            //if has target data component set target to self
            if (entityManager.HasComponent<TargetData>(prototypeEntity))
            {
                entityManager.SetComponentData(prototypeEntity, new TargetData { atTarget = true, currentTarget = worldPoint, oldTarget = worldPoint});
            }
        }
        entityManager.DestroyEntity(convertedEntity);

    }
    #endregion

}
