using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    EntityManager entityManager;
    GameObjectConversionSettings settings;
    public static SimulationManager Instance;

    public bool isSetupComplete;

    #region Archetypes
    // declare All of archetypes
    public static EntityArchetype GrassTileArchetype { get; private set; }
    public static EntityArchetype WaterTileArchetype { get; private set; }
    public static EntityArchetype RockTileArchetype { get; private set; }
    public static EntityArchetype SandTileArchetype { get; private set; }
    public static EntityArchetype MaleRabbitArchetype { get; private set; }
    public static EntityArchetype FemaleRabbitArchetype { get; private set; }
    public static EntityArchetype GrassArchetype { get; private set; }
    public static EntityArchetype FoxArchetype { get; private set; }   //I am not sure do we need Female Fox, just Fox if we need, I can add later
    #endregion

    #region GameObjects
    [Header("GameObjects")]
    [SerializeField] public GameObject grassTile;
    [SerializeField] public GameObject lightGrassTile;
    [SerializeField] public GameObject waterTile;
    [SerializeField] public GameObject sandTile;
    [SerializeField] public GameObject rockTile;
    [SerializeField] public GameObject rabbit;
    [SerializeField] public GameObject fox;
    [SerializeField] public GameObject grass;
    public GameObject collisionPlaneForMap;
    #endregion

    #region Numbers for Entity Spawning 
    [Header("Map Data")]
    [SerializeField] public int numberOfRabbitsToSpawn = 0;
    [SerializeField] public int numberOfFoxesToSpawn = 0;
    [SerializeField] public int numberOfGrassToSpawn = 0;
    #endregion

    #region Population Info for Entities
    [Header("Population Data")]
    public int rabbitPopulation = 0;
    public int foxPopulation = 0;
    public int grassPopulation = 0;
    #endregion

    #region Death Info for Entities
    [Header("Death Data")]
    public int numberOfGrassEaten = 0;

    public int numberOfRabbitsDeadTotal = 0;
    public int numberOfRabbitsDeadHunger = 0;
    public int numberOfRabbitsDeadThirst = 0;
    public int numberOfRabbitsDeadEaten = 0;
    public int numberOfRabbitsDeadAge = 0;

    public int numberOfFoxesDeadTotal = 0;
    public int numberOfFoxesDeadHunger = 0;
    public int numberOfFoxesDeadThirst = 0;
    public int numberOfFoxesDeadEaten = 0;
    public int numberOfFoxesDeadAge = 0;

    #endregion

    #region Map Data 
    [Header("Map Data")]
    public static string mapPath = "Assets/Scripts/MonoBehaviourTools/Map/MapExample.txt";
    public static string mapString;
    public static int gridWidth;
    public static int gridHeight;
    public static float2 worldSize;
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
        isSetupComplete = false;

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());

        CreateArchetypes();

        // Only continue if no errors creating the map
        if (!CreateMap())
        {
            Debug.Log("Error Loading Map");
        }
    }

    private void CreateArchetypes()
    {
        //Create archetype of Grass tile, water tile, rock tile, sand tile
        GrassTileArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(NonUniformScale),
            typeof(TerrainTypeData)
            );

        WaterTileArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(NonUniformScale),
            typeof(TerrainTypeData),
            typeof(DrinkableData)
            );

        RockTileArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(NonUniformScale),
            typeof(TerrainTypeData)
            );

        SandTileArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(NonUniformScale),
            typeof(TerrainTypeData)
            );

        //  Create architype of Fox MaleRabbit Female Rabbit Grass
        MaleRabbitArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(NonUniformScale),
            typeof(EdibleData),
            typeof(MovementData),
            typeof(ReproductiveData),
            typeof(SizeData),
            typeof(StateData),
            typeof(TargetData),
            typeof(PathFollowData),
            typeof(BasicNeedsData),
            typeof(BioStatsData)
            );

        FemaleRabbitArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(NonUniformScale),
            typeof(ReproductiveData),
            typeof(EdibleData),
            typeof(MovementData),
            typeof(SizeData),
            typeof(StateData),
            typeof(TargetData),
            typeof(PathFollowData),
            typeof(BasicNeedsData),
            typeof(BioStatsData)
            );

        FoxArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(NonUniformScale),
            typeof(MovementData),
            typeof(StateData),
            typeof(TargetData),
            typeof(PathFollowData),
            typeof(SizeData),
            typeof(BasicNeedsData),
            typeof(BioStatsData)
            );

        GrassArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(NonUniformScale),
            typeof(EdibleData)
            );
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        //check if the setup has completed yet, finish setup
        if (!isSetupComplete)
        {
            if (GridSetup.Instance.CreateGrid())
            {
                CreateEntitiesFromGameObject(grass, numberOfGrassToSpawn);
                CreateEntitiesFromGameObject(rabbit, numberOfRabbitsToSpawn);
                CreateEntitiesFromGameObject(fox, numberOfFoxesToSpawn);
                isSetupComplete = true;
            }
        }
        else 
        {
            SpawnRabbitAtPosOnClick();
            EnforceGrassPopulation(); 
        }
        
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

        // Create a GameObject the size of the map with collider for UnityEngine.Physics ray hits
        collisionPlaneForMap = new GameObject();
        collisionPlaneForMap.transform.position = transform.position;
        collisionPlaneForMap.AddComponent<UnityEngine.BoxCollider>();
        UnityEngine.BoxCollider collider = collisionPlaneForMap.GetComponent<UnityEngine.BoxCollider>();
        collider.size = new Vector3(worldSize.x, 0, worldSize.y);

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
                        entityManager.SetName(prototypeTile, "WaterTile " + y + "," + x);
                        entityManager.SetComponentData(prototypeTile,
                            new ColliderTypeData
                            {
                                colliderType = ColliderTypeData.ColliderType.Water
                            }
                        );
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
        var convertedEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObject, settings);

        for (int i = 0; i < quantity; i++)
        {
            
            // Get the random world co ordinates from the bottom left of the graph
            Vector3 worldPoint;
            do
            {
                // Calc random point on map
                int randWidth = UnityEngine.Random.Range(0, (int)gridWidth - 1);
                int randHeight = UnityEngine.Random.Range(0, (int)gridHeight - 1);
                worldPoint = worldBottomLeft + Vector3.right * (randWidth * tileSize + tileSize / 2) + Vector3.forward * (randHeight * tileSize + tileSize / 2);
            } while (!UtilTools.GridTools.IsWorldPointOnWalkableTile(worldPoint, entityManager));

            // Place the instantiated entity in a random point on the map
            //set default variables based on gameObject name - not great solution but works for now
            //FIXME
            if (gameObject.name.Contains("Rabbit"))
            {
                CreateRabbitAtWorldPoint(worldPoint);
            }
            else if (gameObject.name.Contains("Fox"))
            {
                CreateFoxAtWorldPoint(worldPoint);
            }
            else if (gameObject.name.Contains("Grass"))
            {
                CreateGrassAtWorldPoint(worldPoint);
            }
            else
            {
                // Efficiently instantiate a bunch of entities from the already converted entity prefab
                var prototypeEntity = entityManager.Instantiate(convertedEntity);
                //Set Component Data for the entity
                entityManager.SetName(prototypeEntity, gameObject.name + i); // set name

                entityManager.SetComponentData(prototypeEntity,
                    new Translation
                    {
                        Value = worldPoint
                    }
                );
                //if has target data component set target to self
                if (entityManager.HasComponent<TargetData>(prototypeEntity))
                {
                    entityManager.SetComponentData(prototypeEntity,
                        new TargetData
                        {
                            atTarget = true,
                            currentTarget = worldPoint,
                            oldTarget = worldPoint,

                            sightRadius = 0.1f,
                            touchRadius = 0.1f
                        }
                    );
                }
            }

        }
        entityManager.DestroyEntity(convertedEntity);

    }

    private void CreateFoxAtWorldPoint(in float3 worldPoint)
    {
        var entityFox = GameObjectConversionUtility.ConvertGameObjectHierarchy(fox, settings);
        Entity prototypeFox = entityManager.Instantiate(entityFox);

        //set name of entity
        entityManager.SetName(prototypeFox, $"Fox {foxPopulation}");

        entityManager.AddComponent<isFoxTag>(prototypeFox);
        entityManager.SetComponentData(prototypeFox,
            new Translation
            {
                Value = worldPoint
            }
        );


        entityManager.SetComponentData(prototypeFox,
            new EdibleData
            {
                canBeEaten = FoxDefaults.canBeEaten,
                nutritionalValueBase = FoxDefaults.nutritionalValue,
                nutritionalValueMultiplier = FoxDefaults.nutritionalValueMultiplier,
                foodType = FoxDefaults.foodType
            }
        );
        entityManager.SetComponentData(prototypeFox,
            new MovementData
            {
                rotationSpeed = FoxDefaults.rotationSpeed,
                moveSpeedBase = FoxDefaults.moveSpeed,
                moveMultiplier = FoxDefaults.moveMultiplier,
                pregnancyMoveMultiplier = FoxDefaults.pregnancyMoveMultiplier,
                originalMoveMultiplier = FoxDefaults.originalMoveMultiplier,
                youngMoveMultiplier = FoxDefaults.youngMoveMultiplier,
                adultMoveMultiplier = FoxDefaults.adultMoveMultiplier,
                oldMoveMultiplier = FoxDefaults.oldMoveMultiplier
            }
        );
        entityManager.SetComponentData(prototypeFox,
            new StateData
            {
                flagState = FoxDefaults.flagState,
                previousFlagState = FoxDefaults.previousFlagState,
                deathReason = FoxDefaults.deathReason,
                beenEaten = FoxDefaults.beenEaten
            }
        );
        entityManager.SetComponentData(prototypeFox,
            new TargetData
            {
                atTarget = true,
                currentTarget = worldPoint,
                oldTarget = worldPoint,

                sightRadius = FoxDefaults.sightRadius,
                touchRadius = FoxDefaults.touchRadius,

                predatorEntity = FoxDefaults.predatorEntity,
                entityToEat = FoxDefaults.entityToEat,
                entityToDrink = FoxDefaults.entityToDrink,
                entityToMate = FoxDefaults.entityToMate,
                shortestToEdibleDistance = FoxDefaults.shortestToEdibleDistance,
                shortestToWaterDistance = FoxDefaults.shortestToWaterDistance,
                shortestToPredatorDistance = FoxDefaults.shortestToPredatorDistance,
                shortestToMateDistance = FoxDefaults.shortestToMateDistance
            }
        );
        entityManager.SetComponentData(prototypeFox,
            new PathFollowData
            {
                pathIndex = -1
            }
        );
        entityManager.SetComponentData(prototypeFox,
            new BasicNeedsData
            {
                hunger = FoxDefaults.hunger,
                hungryThreshold = FoxDefaults.hungryThreshold,
                hungerMax = FoxDefaults.hungerMax,
                hungerIncrease = FoxDefaults.hungerIncrease,
                pregnancyHungerIncrease = FoxDefaults.pregnancyHungerIncrease,
                youngHungerIncrease = FoxDefaults.youngHungerIncrease,
                adultHungerIncrease = FoxDefaults.adultHungerIncrease,
                oldHungerIncrease = FoxDefaults.oldHungerIncrease,
                eatingSpeed = FoxDefaults.eatingSpeed,

                diet = FoxDefaults.diet,
                thirst = FoxDefaults.thirst,
                thirstyThreshold = FoxDefaults.thirstyThreshold,
                thirstMax = FoxDefaults.thirstMax,
                thirstIncrease = FoxDefaults.thirstIncrease,
                drinkingSpeed = FoxDefaults.drinkingSpeed,

            }
        );

        //randomise gender of fox - equal distribution
        BioStatsData.Gender randGender = UnityEngine.Random.Range(0, 2) == 1 ? randGender = BioStatsData.Gender.Female : randGender = BioStatsData.Gender.Male;
        //set gender differing components

        entityManager.SetComponentData(prototypeFox,
            new BioStatsData
            {
                age = FoxDefaults.age,
                ageIncrease = FoxDefaults.ageIncrease,
                ageMax = FoxDefaults.ageMax,
                ageGroup = FoxDefaults.ageGroup,
                adultEntryTimer = FoxDefaults.adultEntryTimer,
                oldEntryTimer = FoxDefaults.oldEntryTimer,
                gender = randGender
            }
        );
        //set reproductive data differing on gender
        entityManager.SetComponentData(prototypeFox,
                new ReproductiveData
                {
                    matingDuration = FoxDefaults.matingDuration,
                    mateStartTime = FoxDefaults.mateStartTime,
                    reproductiveUrge = FoxDefaults.reproductiveUrge,
                    reproductiveUrgeIncrease = (randGender == BioStatsData.Gender.Female ? FoxDefaults.reproductiveUrgeIncreaseFemale : FoxDefaults.reproductiveUrgeIncreaseMale),
                    defaultRepoductiveIncrease = (randGender == BioStatsData.Gender.Female ? FoxDefaults.reproductiveUrgeIncreaseFemale : FoxDefaults.reproductiveUrgeIncreaseMale),
                    matingThreshold = FoxDefaults.matingThreshold,


                    pregnant = FoxDefaults.pregnant,
                    birthDuration = FoxDefaults.birthDuration,
                    babiesBorn = FoxDefaults.babiesBorn,
                    birthStartTime = FoxDefaults.birthStartTime,
                    currentLitterSize = FoxDefaults.currentLitterSize,
                    litterSizeMin = FoxDefaults.litterSizeMin,
                    litterSizeMax = FoxDefaults.litterSizeMax,
                    litterSizeAve = FoxDefaults.litterSizeAve,
                    pregnancyLengthBase = FoxDefaults.pregnancyLength,
                    pregnancyLengthModifier = FoxDefaults.pregnancyLengthModifier,
                    pregnancyStartTime = FoxDefaults.pregnancyStartTime
                }
            );

        //set size differing on gender
        entityManager.SetComponentData(prototypeFox,
            new SizeData
            {
                size = (randGender == BioStatsData.Gender.Female ? FoxDefaults.scaleFemale : FoxDefaults.scaleMale),
                sizeMultiplier = FoxDefaults.sizeMultiplier,
                ageSizeMultiplier = FoxDefaults.ageSizeMultiplier,
                youngSizeMultiplier = FoxDefaults.youngSizeMultiplier,
                adultSizeMultiplier = FoxDefaults.adultSizeMultiplier,
                oldSizeMultiplier = FoxDefaults.oldSizeMultiplier
            }
        );
        // set ColliderTypeData to Fox entity
        entityManager.SetComponentData(prototypeFox,
            new ColliderTypeData
            {
                colliderType = FoxDefaults.colliderType
            }
        );

        foxPopulation++;

        entityManager.DestroyEntity(entityFox);
    }
    private void CreateGrassAtWorldPoint(in float3 worldPoint)
    {
        var entityGrass = GameObjectConversionUtility.ConvertGameObjectHierarchy(grass, settings);
        Entity prototypeGrass = entityManager.Instantiate(entityGrass);

        //set name of entity
        entityManager.SetName(prototypeGrass, $"Grass {grassPopulation}");

        entityManager.AddComponent<isGrassTag>(prototypeGrass);
        entityManager.SetComponentData(prototypeGrass,
            new Translation
            {
                Value = worldPoint
            }
        );

        entityManager.SetComponentData(prototypeGrass,
            new EdibleData
            {
                canBeEaten = GrassDefaults.canBeEaten,
                nutritionalValueBase = GrassDefaults.nutritionalValue,
                nutritionalValueMultiplier = GrassDefaults.nutritionalValueMultiplier,
                foodType = GrassDefaults.foodType
            }
        );

        entityManager.SetComponentData(prototypeGrass,
            new StateData
            {
                flagState = GrassDefaults.flagState,
                previousFlagState = GrassDefaults.previousFlagState,
                deathReason = GrassDefaults.deathReason,
                beenEaten = GrassDefaults.beenEaten
            }
        );

        //set size differing on gender
        entityManager.SetComponentData(prototypeGrass,
            new SizeData
            {
                size = GrassDefaults.scale,
                sizeMultiplier = GrassDefaults.sizeMultiplier
            }
        );
        // set ColliderTypeData to Grass entity
        entityManager.SetComponentData(prototypeGrass,
            new ColliderTypeData
            {
                colliderType = GrassDefaults.GrassColliderType
            }
        );


        grassPopulation++;

        entityManager.DestroyEntity(entityGrass);
    }

    private void CreateRabbitAtWorldPoint(in float3 worldPoint)
    {
        var entityRabbit = GameObjectConversionUtility.ConvertGameObjectHierarchy(rabbit, settings);
        Entity prototypeRabbit = entityManager.Instantiate(entityRabbit);

        //set name of entity
        entityManager.SetName(prototypeRabbit, $"Rabbit {rabbitPopulation}");

        entityManager.AddComponent<isRabbitTag>(prototypeRabbit);
        entityManager.SetComponentData(prototypeRabbit,
            new Translation
            {
                Value = worldPoint
            }
        );


        entityManager.SetComponentData(prototypeRabbit,
            new EdibleData
            {
                canBeEaten = RabbitDefaults.canBeEaten,
                nutritionalValueBase = RabbitDefaults.nutritionalValue,
                nutritionalValueMultiplier = RabbitDefaults.nutritionalValueMultiplier,
                foodType = RabbitDefaults.foodType
            }
        );
        entityManager.SetComponentData(prototypeRabbit,
            new MovementData
            {
                rotationSpeed = RabbitDefaults.rotationSpeed,
                moveSpeedBase = RabbitDefaults.moveSpeed,
                moveMultiplier = RabbitDefaults.moveMultiplier,
                pregnancyMoveMultiplier = RabbitDefaults.pregnancyMoveMultiplier,
                originalMoveMultiplier = RabbitDefaults.originalMoveMultiplier,
                youngMoveMultiplier = RabbitDefaults.youngMoveMultiplier,
                adultMoveMultiplier = RabbitDefaults.adultMoveMultiplier,
                oldMoveMultiplier = RabbitDefaults.oldMoveMultiplier
            }
        );
        entityManager.SetComponentData(prototypeRabbit,
            new StateData
            {
                flagState = RabbitDefaults.flagState,
                previousFlagState = RabbitDefaults.previousFlagState,
                deathReason = RabbitDefaults.deathReason,
                beenEaten = RabbitDefaults.beenEaten,
            }
        );
        entityManager.SetComponentData(prototypeRabbit,
            new TargetData
            {
                atTarget = true,
                currentTarget = worldPoint,
                oldTarget = worldPoint,

                sightRadius = RabbitDefaults.sightRadius,
                touchRadius = RabbitDefaults.touchRadius,
                mateRadius = RabbitDefaults.mateRadius,

                predatorEntity = FoxDefaults.predatorEntity,
                entityToEat = FoxDefaults.entityToEat,
                entityToDrink = FoxDefaults.entityToDrink,
                entityToMate = FoxDefaults.entityToMate,
                shortestToEdibleDistance = FoxDefaults.shortestToEdibleDistance,
                shortestToWaterDistance = FoxDefaults.shortestToWaterDistance,
                shortestToPredatorDistance = FoxDefaults.shortestToPredatorDistance,
                shortestToMateDistance = FoxDefaults.shortestToMateDistance
            }
        );
        entityManager.SetComponentData(prototypeRabbit,
            new PathFollowData
            {
                pathIndex = -1
            }
        );
        entityManager.SetComponentData(prototypeRabbit,
            new BasicNeedsData
            {
                hunger = RabbitDefaults.hunger,
                hungryThreshold = RabbitDefaults.hungryThreshold,
                hungerMax = RabbitDefaults.hungerMax,
                hungerIncrease = RabbitDefaults.hungerIncrease,
                pregnancyHungerIncrease = RabbitDefaults.pregnancyHungerIncrease,
                youngHungerIncrease = RabbitDefaults.youngHungerIncrease,
                adultHungerIncrease = RabbitDefaults.adultHungerIncrease,
                oldHungerIncrease = RabbitDefaults.oldHungerIncrease,
                eatingSpeed = RabbitDefaults.eatingSpeed,

                diet = RabbitDefaults.diet,
                thirst = RabbitDefaults.thirst,
                thirstyThreshold = RabbitDefaults.thirstyThreshold,
                thirstMax = RabbitDefaults.thirstMax,
                thirstIncrease = RabbitDefaults.thirstIncrease,
                drinkingSpeed = RabbitDefaults.drinkingSpeed,

            }
        );

        //randomise gender of rabbit - equal distribution
        BioStatsData.Gender randGender = UnityEngine.Random.Range(0, 2) == 1 ? randGender = BioStatsData.Gender.Female : randGender = BioStatsData.Gender.Male;
        //set gender differing components

        entityManager.SetComponentData(prototypeRabbit,
            new BioStatsData
            {
                age = RabbitDefaults.age,
                ageIncrease = RabbitDefaults.ageIncrease,
                ageMax = RabbitDefaults.ageMax,
                ageGroup = RabbitDefaults.ageGroup,
                adultEntryTimer = RabbitDefaults.adultEntryTimer,
                oldEntryTimer = RabbitDefaults.oldEntryTimer,
                gender = randGender
            }
        );
        //set reproductive data differing on gender
        entityManager.SetComponentData(prototypeRabbit,
                new ReproductiveData
                {
                    matingDuration = RabbitDefaults.matingDuration,
                    mateStartTime = RabbitDefaults.mateStartTime,
                    reproductiveUrge = RabbitDefaults.reproductiveUrge,
                    reproductiveUrgeIncrease = (randGender == BioStatsData.Gender.Female ? RabbitDefaults.reproductiveUrgeIncreaseFemale : RabbitDefaults.reproductiveUrgeIncreaseMale),
                    defaultRepoductiveIncrease = (randGender == BioStatsData.Gender.Female ? RabbitDefaults.reproductiveUrgeIncreaseFemale : RabbitDefaults.reproductiveUrgeIncreaseMale),
                    matingThreshold = RabbitDefaults.matingThreshold,


                    pregnant = RabbitDefaults.pregnant,
                    birthDuration = RabbitDefaults.birthDuration,
                    babiesBorn = RabbitDefaults.babiesBorn,
                    birthStartTime = RabbitDefaults.birthStartTime,
                    currentLitterSize = RabbitDefaults.currentLitterSize,
                    litterSizeMin = RabbitDefaults.litterSizeMin,
                    litterSizeMax = RabbitDefaults.litterSizeMax,
                    litterSizeAve = RabbitDefaults.litterSizeAve,
                    pregnancyLengthBase = RabbitDefaults.pregnancyLength,
                    pregnancyLengthModifier = RabbitDefaults.pregnancyLengthModifier,
                    pregnancyStartTime = RabbitDefaults.pregnancyStartTime
                }
            );

        //set size differing on gender
        entityManager.SetComponentData(prototypeRabbit,
            new SizeData
            {
                size = (randGender == BioStatsData.Gender.Female ? RabbitDefaults.scaleFemale : RabbitDefaults.scaleMale),
                sizeMultiplier = RabbitDefaults.sizeMultiplier,
                ageSizeMultiplier = RabbitDefaults.ageSizeMultiplier,
                youngSizeMultiplier = RabbitDefaults.youngSizeMultiplier,
                adultSizeMultiplier = RabbitDefaults.adultSizeMultiplier,
                oldSizeMultiplier = RabbitDefaults.oldSizeMultiplier
            }
        );
        // set ColliderTypeData to Rabbit entity
        entityManager.SetComponentData(prototypeRabbit,
            new ColliderTypeData
            {
                colliderType = RabbitDefaults.colliderType
            }
        );

        rabbitPopulation++;

        entityManager.DestroyEntity(entityRabbit);
    }

    private void SpawnRabbitAtPosOnClick()
    {
        //checks for click of the mouse, sends ray out from camera, creates rabbit where it hits
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out UnityEngine.RaycastHit hit))
            {
                Vector3 targetPosition = hit.point;
                Debug.Log(targetPosition.ToString());
                if (UtilTools.GridTools.IsWorldPointOnWalkableTile(targetPosition, entityManager))
                    CreateRabbitAtWorldPoint(targetPosition);
            }
        }
    }

    private void EnforceGrassPopulation()
    {
        //randomly spawn as many grass as the number less than numberOfGrassToSpawn
        if (grassPopulation < numberOfGrassToSpawn)
            CreateEntitiesFromGameObject(grass, numberOfGrassToSpawn - grassPopulation);
    }
    #endregion

}
