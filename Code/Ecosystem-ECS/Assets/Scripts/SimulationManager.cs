using System.Collections.Generic;
using Components;
using EntityDefaults;
using MonoBehaviourTools.Grid;
using MonoBehaviourTools.Map;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static int MAX_POP = 50000;

    EntityManager entityManager;
    public GameObjectConversionSettings settings;
    public static SimulationManager Instance;
    public bool isDebugEnabled;
    public bool isSetupComplete;

    private int secondsOfLastGrassSpawn;

    #region Converted Entities
    public Entity conversionGrassTile;
    public Entity conversionLightGrassTile;
    public Entity conversionWaterTile;
    public Entity conversionSandTile;
    public Entity conversionRockTile;
    public Entity conversionRabbit;
    public Entity conversionFox;
    public Entity conversionGrass;
    #endregion

    #region GameObjects
    [Header("GameObjects")]
    [SerializeField] private GameObject grassTile;
    [SerializeField] private GameObject lightGrassTile;
    [SerializeField] private GameObject waterTile;
    [SerializeField] private GameObject sandTile;
    [SerializeField] private GameObject rockTile;
    [SerializeField] private GameObject rabbit;
    [SerializeField] private GameObject fox;
    [SerializeField] private GameObject grass;
    public GameObject collisionPlaneForMap;
    #endregion

    #region Numbers for Entity Spawning 
    [Header("Numbers of Entities Spawning")]
    public int numberOfRabbitsToSpawn = 0;
    public int numberOfFoxesToSpawn = 0;
    public int numberOfGrassToSpawn = 0;
    public static int InitialRabbitsToSpawn = -1;
    public static int InitialFoxesToSpawn = -1;
    public static int InitialGrassToSpawn = -1;
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
    public static string mapPath;
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
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        isSetupComplete = false;

        if (InitialRabbitsToSpawn >= 0)
            numberOfRabbitsToSpawn = InitialRabbitsToSpawn;
        if (InitialFoxesToSpawn >= 0)
            numberOfFoxesToSpawn = InitialFoxesToSpawn;
        if (InitialGrassToSpawn >= 0)
            numberOfGrassToSpawn = InitialGrassToSpawn;
        secondsOfLastGrassSpawn = 0;
    }
    private void Start()
    {
        
        Application.targetFrameRate = 60; // Target 60fps

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());

        

        // Only continue if no errors creating the map
        if (!CreateMap())
        {
            Debug.LogError("Error Loading Map");

            //TODO: Should display error to user that Error loading map: maybe default map is missing
            Application.Quit();
            Destroy(this);
        }
    }
    #endregion

    private void Update()
    {
        //Emergency Pause to stop excessive population explosion that could cause freezing
        if (rabbitPopulation > MAX_POP || foxPopulation > MAX_POP)
        {
            MonoBehaviourTools.UI.UITimeControl.Instance.Pause();
            //TODO: should display message to user saying sim is paused due to excessive population
        }

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
            SpawnRabbitAtPosOnLClick();
            SpawnFoxAtPosOnRClick();
            SpawnGrassAtPosOnMClick();
            /* Spawn grass entity at random location once every 10 in game seconds */
            if ((int)Time.timeSinceLevelLoad % 10 == 0 && secondsOfLastGrassSpawn != (int)Time.timeSinceLevelLoad)
            {
                secondsOfLastGrassSpawn = (int)Time.timeSinceLevelLoad; //update the time in seconds the code was ran
                if (grassPopulation < 2 * gridHeight * gridHeight)//limit to 2x grass per grid square
                    CreateEntitiesFromGameObject(grass, (int)math.ceil(grassPopulation / 10));
            }
        }
    }

    private void OnDestroy()
    {
        //dispose of blobassetstore on destroy
        if (settings != null)
            settings.BlobAssetStore.Dispose();
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
            //Last resort try default map location
            mapPath = Application.dataPath + "/MapDefault.txt";
            if (MapReader.ReadInMapFromFile(mapPath, ref mapList))
                CreateEntityTilesFromMapList(in mapList);
            else
                return false;
        }

        // Create a GameObject the size of the map with collider for UnityEngine.Physics ray hits
        collisionPlaneForMap = new GameObject
        {
            name = "MapCollisionPlaneGO"
        };
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
        tileSize = grassTile.GetComponent<Renderer>().bounds.size.x; //Get the width of the tile
        worldSize.x = gridWidth * tileSize;
        worldSize.y = gridHeight * tileSize;
        worldBottomLeft = transform.position - Vector3.right * worldSize.x / 2 - Vector3.forward * worldSize.y / 2;//Get the real world position of the bottom left of the grid.

        // Create entity prefabs from the game objects hierarchy once
        if (conversionGrassTile == Entity.Null)
            conversionGrassTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(grassTile, settings);
        if (conversionLightGrassTile == Entity.Null)
            conversionLightGrassTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(grassTile, settings);
        if (conversionWaterTile == Entity.Null)
            conversionWaterTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(waterTile, settings);
        if (conversionSandTile == Entity.Null)
            conversionSandTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(sandTile, settings);
        if (conversionRockTile == Entity.Null)
            conversionRockTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(rockTile, settings);


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
                        prototypeTile = entityManager.Instantiate(conversionWaterTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS)
                        entityManager.SetName(prototypeTile, "WaterTile " + y + "," + x);
                        entityManager.SetComponentData(prototypeTile,
                            new ColliderTypeData
                            {
                                colliderType = ColliderTypeData.ColliderType.Water
                            }
                        );
                        break;
                    case MapReader.TerrainCost.Grass:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = entityManager.Instantiate(conversionGrassTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        entityManager.SetName(prototypeTile, "GrassTile " + y + "," + x);
                        break;
                    case MapReader.TerrainCost.Sand:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = entityManager.Instantiate(conversionSandTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        entityManager.SetName(prototypeTile, "SandTile " + y + "," + x);
                        break;
                    case MapReader.TerrainCost.Rock:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = entityManager.Instantiate(conversionRockTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        entityManager.SetName(prototypeTile, "RockTile " + y + "," + x);
                        break;
                    default:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = entityManager.Instantiate(conversionGrassTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        Debug.Log("Unknown TerrainCost value");
                        break;
                }
            }
        }
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
        for (int i = 0; i < quantity; i++)
        {
            // Get the random world co ordinates from the bottom left of the graph
            Vector3 worldPoint;
            do
            {
                // Calc random point on map
                int randWidth = UnityEngine.Random.Range(0, (int)gridWidth);
                int randHeight = UnityEngine.Random.Range(0, (int)gridHeight);
                worldPoint = worldBottomLeft + Vector3.right * (randWidth * tileSize + tileSize / 2) + Vector3.forward * (randHeight * tileSize + tileSize / 2);
            } while (!UtilTools.GridTools.IsWorldPointOnWalkableTile(worldPoint, entityManager));

            // Place the instantiated entity in a random point on the map
            //set default variables based on gameObject name - not great solution but works for now
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
        }
    }

    private void CreateFoxAtWorldPoint(in float3 worldPoint)
    {
        if (conversionFox == Entity.Null)
            conversionFox = GameObjectConversionUtility.ConvertGameObjectHierarchy(fox, settings);
        Entity prototypeFox = entityManager.Instantiate(conversionFox);

        //set name of entity
        entityManager.SetName(prototypeFox, $"Fox {foxPopulation}");

        entityManager.AddComponent<IsFoxTag>(prototypeFox);
        entityManager.SetComponentData(prototypeFox,
            new Translation
            {
                Value = worldPoint,
            }
        );

        entityManager.SetComponentData(prototypeFox,
            new EdibleData
            {
                canBeEaten = FoxDefaults.canBeEaten,
                nutritionalValueBase = FoxDefaults.nutritionalValue,
                nutritionalValueMultiplier = FoxDefaults.nutritionalValueMultiplier,
                foodType = FoxDefaults.foodType,
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
                oldMoveMultiplier = FoxDefaults.oldMoveMultiplier,
            }
        );

        entityManager.SetComponentData(prototypeFox,
            new StateData
            {
                flagState = FoxDefaults.flagState,
                previousFlagState = FoxDefaults.previousFlagState,
                deathReason = FoxDefaults.deathReason,
                beenEaten = FoxDefaults.beenEaten,
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
                mateRadius = FoxDefaults.mateRadius,

                predatorEntity = FoxDefaults.predatorEntity,
                entityToEat = FoxDefaults.entityToEat,
                entityToDrink = FoxDefaults.entityToDrink,
                entityToMate = FoxDefaults.entityToMate,
                shortestToEdibleDistance = FoxDefaults.shortestToEdibleDistance,
                shortestToWaterDistance = FoxDefaults.shortestToWaterDistance,
                shortestToPredatorDistance = FoxDefaults.shortestToPredatorDistance,
                shortestToMateDistance = FoxDefaults.shortestToMateDistance,
            }
        );

        entityManager.SetComponentData(prototypeFox,
            new PathFollowData
            {
                pathIndex = -1,
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
        BioStatsData.Gender randGender = UnityEngine.Random.Range(0, 2) == 1 ? BioStatsData.Gender.Female : BioStatsData.Gender.Male;
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
                gender = randGender,
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

                    birthDuration = FoxDefaults.birthDuration,
                    babiesBorn = FoxDefaults.babiesBorn,
                    birthStartTime = FoxDefaults.birthStartTime,
                    currentLitterSize = FoxDefaults.currentLitterSize,
                    litterSizeMin = FoxDefaults.litterSizeMin,
                    litterSizeMax = FoxDefaults.litterSizeMax,
                    litterSizeAve = FoxDefaults.litterSizeAve,
                    pregnancyLengthBase = FoxDefaults.pregnancyLength,
                    pregnancyLengthModifier = FoxDefaults.pregnancyLengthModifier,
                    pregnancyStartTime = FoxDefaults.pregnancyStartTime,
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
                oldSizeMultiplier = FoxDefaults.oldSizeMultiplier,
            }
        );
        // set ColliderTypeData to Fox entity
        entityManager.SetComponentData(prototypeFox,
            new ColliderTypeData
            {
                colliderType = FoxDefaults.colliderType,
            }
        );

        foxPopulation++;
    }

    private void CreateGrassAtWorldPoint(in float3 worldPoint)
    {
        if (conversionGrass == Entity.Null)
            conversionGrass = GameObjectConversionUtility.ConvertGameObjectHierarchy(grass, settings);
        Entity prototypeGrass = entityManager.Instantiate(conversionGrass);

        //set name of entity
        entityManager.SetName(prototypeGrass, $"Grass {grassPopulation}");

        entityManager.AddComponent<IsGrassTag>(prototypeGrass);
        entityManager.SetComponentData(prototypeGrass,
            new Translation
            {
                Value = worldPoint,
            }
        );

        entityManager.SetComponentData(prototypeGrass,
            new EdibleData
            {
                canBeEaten = GrassDefaults.canBeEaten,
                nutritionalValueBase = GrassDefaults.nutritionalValue,
                nutritionalValueMultiplier = GrassDefaults.nutritionalValueMultiplier,
                foodType = GrassDefaults.foodType,
            }
        );

        entityManager.SetComponentData(prototypeGrass,
            new StateData
            {
                flagState = GrassDefaults.flagState,
                previousFlagState = GrassDefaults.previousFlagState,
                deathReason = GrassDefaults.deathReason,
                beenEaten = GrassDefaults.beenEaten,
            }
        );

        //set size differing on gender
        entityManager.SetComponentData(prototypeGrass,
            new SizeData
            {
                size = GrassDefaults.scale,
                sizeMultiplier = GrassDefaults.sizeMultiplier,
                ageSizeMultiplier = 1f,
            }
        );

        // set ColliderTypeData to Grass entity
        entityManager.SetComponentData(prototypeGrass,
            new ColliderTypeData
            {
                colliderType = GrassDefaults.GrassColliderType,
            }
        );


        grassPopulation++;

    }

    private void CreateRabbitAtWorldPoint(in float3 worldPoint)
    {
        if (conversionRabbit == Entity.Null)
            conversionRabbit = GameObjectConversionUtility.ConvertGameObjectHierarchy(rabbit, settings);
        Entity prototypeRabbit = entityManager.Instantiate(conversionRabbit);

        //set name of entity
        entityManager.SetName(prototypeRabbit, $"Rabbit {rabbitPopulation}");

        entityManager.AddComponent<IsRabbitTag>(prototypeRabbit);
        entityManager.SetComponentData(prototypeRabbit,
            new Translation
            {
                Value = worldPoint,
            }
        );


        entityManager.SetComponentData(prototypeRabbit,
            new EdibleData
            {
                canBeEaten = RabbitDefaults.canBeEaten,
                nutritionalValueBase = RabbitDefaults.nutritionalValue,
                nutritionalValueMultiplier = RabbitDefaults.nutritionalValueMultiplier,
                foodType = RabbitDefaults.foodType,
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
                oldMoveMultiplier = RabbitDefaults.oldMoveMultiplier,
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
                shortestToMateDistance = FoxDefaults.shortestToMateDistance,
            }
        );

        entityManager.SetComponentData(prototypeRabbit,
            new PathFollowData
            {
                pathIndex = -1,
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
        BioStatsData.Gender randGender = UnityEngine.Random.Range(0, 2) == 1 ? BioStatsData.Gender.Female : BioStatsData.Gender.Male;
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
                gender = randGender,
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


                    birthDuration = RabbitDefaults.birthDuration,
                    babiesBorn = RabbitDefaults.babiesBorn,
                    birthStartTime = RabbitDefaults.birthStartTime,
                    currentLitterSize = RabbitDefaults.currentLitterSize,
                    litterSizeMin = RabbitDefaults.litterSizeMin,
                    litterSizeMax = RabbitDefaults.litterSizeMax,
                    litterSizeAve = RabbitDefaults.litterSizeAve,
                    pregnancyLengthBase = RabbitDefaults.pregnancyLength,
                    pregnancyLengthModifier = RabbitDefaults.pregnancyLengthModifier,
                    pregnancyStartTime = RabbitDefaults.pregnancyStartTime,
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
                oldSizeMultiplier = RabbitDefaults.oldSizeMultiplier,
            }
        );

        // set ColliderTypeData to Rabbit entity
        entityManager.SetComponentData(prototypeRabbit,
            new ColliderTypeData
            {
                colliderType = RabbitDefaults.colliderType,
            }
        );

        rabbitPopulation++;
    }

    private void SpawnRabbitAtPosOnLClick()
    {
        //checks for click of the mouse, sends ray out from camera, creates rabbit where it hits
        if (Input.GetMouseButtonDown(0))
        {
            // if not over the UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
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
    }
    private void SpawnFoxAtPosOnRClick()
    {
        //checks for click of the mouse, sends ray out from camera, creates rabbit where it hits
        if (Input.GetMouseButtonDown(1))
        {
            // if not over the UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out UnityEngine.RaycastHit hit))
                {
                    Vector3 targetPosition = hit.point;
                    Debug.Log(targetPosition.ToString());
                    if (UtilTools.GridTools.IsWorldPointOnWalkableTile(targetPosition, entityManager))
                        CreateFoxAtWorldPoint(targetPosition);
                }
            }
        }
    }
    private void SpawnGrassAtPosOnMClick()
    {
        //checks for click of the mouse, sends ray out from camera, creates rabbit where it hits
        if (Input.GetMouseButtonDown(2))
        {
            // if not over the UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out UnityEngine.RaycastHit hit))
                {
                    Vector3 targetPosition = hit.point;
                    Debug.Log(targetPosition.ToString());
                    if (UtilTools.GridTools.IsWorldPointOnWalkableTile(targetPosition, entityManager))
                        CreateGrassAtWorldPoint(targetPosition);
                }
            }
        }
    }
    #endregion

    public int RabbitSpawn()
    {
        return numberOfRabbitsToSpawn;
    }
    public int RabbitPopulation()
    {
        return rabbitPopulation;
    }

    public int FoxSpawn()
    {
        return numberOfFoxesToSpawn;
    }
    public int FoxPopulation()
    {
        return foxPopulation;
    }

    public int GrassSpawn()
    {
        return numberOfGrassToSpawn;
    }
    public int GrassPopulation()
    {
        return grassPopulation;
    }

    public static Vector2 MapSize()
    {
        return new Vector2(gridWidth, gridHeight);
    }

    public float GetTileSize()
    {
        return tileSize;
    }
}
