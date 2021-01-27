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
    private const int MaxPopulation = 50000;

    EntityManager _entityManager;
    public GameObjectConversionSettings Settings;
    public static SimulationManager Instance;
    public bool IsDebugEnabled;
    public bool IsSetupComplete;

    private int _secondsOfLastGrassSpawn;

    #region Converted Entities
    public Entity ConversionGrassTile;
    public Entity ConversionLightGrassTile;
    public Entity ConversionWaterTile;
    public Entity ConversionSandTile;
    public Entity ConversionRockTile;
    public Entity ConversionRabbit;
    public Entity ConversionFox;
    public Entity ConversionGrass;
    #endregion

    #region GameObjects
    [Header("GameObjects")]
    [SerializeField] private GameObject _grassTile;
    [SerializeField] private GameObject _lightGrassTile;
    [SerializeField] private GameObject _waterTile;
    [SerializeField] private GameObject _sandTile;
    [SerializeField] private GameObject _rockTile;
    [SerializeField] private GameObject _rabbit;
    [SerializeField] private GameObject _fox;
    [SerializeField] private GameObject _grass;
    public GameObject MapCollisionPlane;
    #endregion

    #region Numbers for Entity Spawning 
    [Header("Numbers of Entities Spawning")]
    public int NumberOfRabbitsToSpawn;
    public int NumberOfFoxesToSpawn;
    public int NumberOfGrassToSpawn;
    public static int InitialRabbitsToSpawn = -1;
    public static int InitialFoxesToSpawn = -1;
    public static int InitialGrassToSpawn = -1;
    #endregion

    #region Population Info for Entities
    [Header("Population Data")]
    public int RabbitPopulation;
    public int FoxPopulation;
    public int GrassPopulation;
    #endregion

    #region Death Info for Entities
    [Header("Death Data")]
    public int NumberOfGrassEaten = 0;

    public int NumberOfRabbitsDeadTotal = 0;
    public int NumberOfRabbitsDeadHunger = 0;
    public int NumberOfRabbitsDeadThirst = 0;
    public int NumberOfRabbitsDeadEaten = 0;
    public int NumberOfRabbitsDeadAge = 0;

    public int NumberOfFoxesDeadTotal = 0;
    public int NumberOfFoxesDeadHunger = 0;
    public int NumberOfFoxesDeadThirst = 0;
    public int NumberOfFoxesDeadEaten = 0;
    public int NumberOfFoxesDeadAge = 0;

    #endregion

    #region Map Data 
    [Header("Map Data")]
    public static string MapPath;
    public static string MapString;
    public static int GridWidth;
    public static int GridHeight;
    public static float2 WorldSize;
    public static Vector3 WorldBottomLeft;
    public static float TileSize;
    public static float LeftLimit;
    public static float UpLimit;
    public static float RightLimit;
    public static float DownLimit;
    #endregion
    #region Initialisation
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        IsSetupComplete = false;

        if (InitialRabbitsToSpawn >= 0)
            NumberOfRabbitsToSpawn = InitialRabbitsToSpawn;
        if (InitialFoxesToSpawn >= 0)
            NumberOfFoxesToSpawn = InitialFoxesToSpawn;
        if (InitialGrassToSpawn >= 0)
            NumberOfGrassToSpawn = InitialGrassToSpawn;
        _secondsOfLastGrassSpawn = 0;
    }
    private void Start()
    {

        Application.targetFrameRate = 60; // Target 60fps

        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());



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
        if (RabbitPopulation > MaxPopulation || FoxPopulation > MaxPopulation)
        {
            MonoBehaviourTools.UI.UITimeControl.Instance.Pause();
            //TODO: should display message to user saying sim is paused due to excessive population
        }

        //check if the setup has completed yet, finish setup
        if (!IsSetupComplete)
        {
            if (GridSetup.Instance.CreateGrid())
            {
                CreateEntitiesFromGameObject(_grass, NumberOfGrassToSpawn);
                CreateEntitiesFromGameObject(_rabbit, NumberOfRabbitsToSpawn);
                CreateEntitiesFromGameObject(_fox, NumberOfFoxesToSpawn);
                IsSetupComplete = true;
            }
        }
        else
        {
            SpawnRabbitAtPosOnLClick();
            SpawnFoxAtPosOnRClick();
            SpawnGrassAtPosOnMClick();
            /* Spawn grass entity at random location once every 10 in game seconds */
            if ((int)Time.timeSinceLevelLoad % 10 == 0 && _secondsOfLastGrassSpawn != (int)Time.timeSinceLevelLoad)
            {
                _secondsOfLastGrassSpawn = (int)Time.timeSinceLevelLoad; //update the time in seconds the code was ran
                if (GrassPopulation < 2 * GridHeight * GridHeight)//limit to 2x grass per grid square
                    CreateEntitiesFromGameObject(_grass, (int)math.ceil(GrassPopulation / 10));
            }
        }
    }

    private void OnDestroy()
    {
        //dispose of blobassetstore on destroy
        if (Settings != null)
            Settings.BlobAssetStore.Dispose();
    }

    #region Map Creation Methods
    // Creates the map with entities
    private bool CreateMap()
    {
        List<List<MapReader.TerrainCost>> mapList = new List<List<MapReader.TerrainCost>>();
        if (MapPath != null)
        {
            if (MapReader.ReadInMapFromFile(MapPath, ref mapList))
                CreateEntityTilesFromMapList(in mapList);
            else
                return false;
        }
        else if (MapString != null)
        {
            if (MapReader.ReadInMapFromString(MapString, ref mapList))
                CreateEntityTilesFromMapList(in mapList);
            else
                return false;
        }
        else
        {
            //Last resort try default map location
            MapPath = Application.dataPath + "/MapDefault.txt";
            if (MapReader.ReadInMapFromFile(MapPath, ref mapList))
                CreateEntityTilesFromMapList(in mapList);
            else
                return false;
        }

        // Create a GameObject the size of the map with collider for UnityEngine.Physics ray hits
        MapCollisionPlane = new GameObject
        {
            name = "MapCollisionPlane"
        };
        MapCollisionPlane.transform.position = transform.position;
        MapCollisionPlane.AddComponent<BoxCollider>();
        var collisionPlaneCollider = MapCollisionPlane.GetComponent<BoxCollider>();
        collisionPlaneCollider.size = new Vector3(WorldSize.x, 0, WorldSize.y);

        SetLimits();
        return true;
    }

    private void CreateEntityTilesFromMapList(in List<List<MapReader.TerrainCost>> mapList)
    {
        // Set world map data
        GridWidth = mapList[0].Count;
        GridHeight = mapList.Count;
        TileSize = _grassTile.GetComponent<Renderer>().bounds.size.x; //Get the width of the tile
        WorldSize.x = GridWidth * TileSize;
        WorldSize.y = GridHeight * TileSize;
        WorldBottomLeft = transform.position - Vector3.right * WorldSize.x / 2 - Vector3.forward * WorldSize.y / 2;//Get the real world position of the bottom left of the grid.

        // Create entity prefabs from the game objects hierarchy once
        if (ConversionGrassTile == Entity.Null)
            ConversionGrassTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(_grassTile, Settings);
        if (ConversionLightGrassTile == Entity.Null)
            ConversionLightGrassTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(_grassTile, Settings);
        if (ConversionWaterTile == Entity.Null)
            ConversionWaterTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(_waterTile, Settings);
        if (ConversionSandTile == Entity.Null)
            ConversionSandTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(_sandTile, Settings);
        if (ConversionRockTile == Entity.Null)
            ConversionRockTile = GameObjectConversionUtility.ConvertGameObjectHierarchy(_rockTile, Settings);


        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                Vector3 worldPoint = WorldBottomLeft + Vector3.right * (x * TileSize + TileSize / 2) + Vector3.forward * (y * TileSize + TileSize / 2);//Get the world co ordinates of the tile from the bottom left of the graph
                Entity prototypeTile;

                switch (mapList[y][x])
                {
                    case MapReader.TerrainCost.Water:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = _entityManager.Instantiate(ConversionWaterTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        _entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS)
                        _entityManager.SetName(prototypeTile, "WaterTile " + y + "," + x);
                        //_entityManager.SetComponentData(prototypeTile,
                        //    new ColliderTypeData
                        //    {
                        //        Collider = ColliderTypeData.ColliderType.Water
                        //    }
                        //);
                        break;
                    case MapReader.TerrainCost.Grass:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = _entityManager.Instantiate(ConversionGrassTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        _entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        _entityManager.SetName(prototypeTile, "GrassTile " + y + "," + x);
                        break;
                    case MapReader.TerrainCost.Sand:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = _entityManager.Instantiate(ConversionSandTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        _entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        _entityManager.SetName(prototypeTile, "SandTile " + y + "," + x);
                        break;
                    case MapReader.TerrainCost.Rock:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = _entityManager.Instantiate(ConversionRockTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        _entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        _entityManager.SetName(prototypeTile, "RockTile " + y + "," + x);
                        break;
                    default:
                        // Efficiently instantiate an entity from the already converted entity prefab
                        prototypeTile = _entityManager.Instantiate(ConversionGrassTile);

                        // Place the instantiated entity in position on the map
                        //Set Component Data for the entity
                        _entityManager.SetComponentData(prototypeTile, new Translation { Value = worldPoint }); // set position data (called translation in ECS) 
                        Debug.Log("Unknown TerrainCost value");
                        break;
                }
            }
        }
    }

    private void SetLimits()
    {
        LeftLimit = -WorldSize.x / 2;
        RightLimit = WorldSize.x / 2;
        DownLimit = -WorldSize.y / 2;
        UpLimit = WorldSize.y / 2;
    }
    #endregion

    #region Entity Spawning
    // Creates entities from a gameobject in a given quantity
    private void CreateEntitiesFromGameObject(Object objectToCreate, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            // Get the random world co ordinates from the bottom left of the graph
            Vector3 worldPoint;
            do
            {
                // Calc random point on map
                int randWidth = UnityEngine.Random.Range(0, GridWidth);
                int randHeight = UnityEngine.Random.Range(0, GridHeight);
                worldPoint = WorldBottomLeft + Vector3.right * (randWidth * TileSize + TileSize / 2) + Vector3.forward * (randHeight * TileSize + TileSize / 2);
            } while (!UtilTools.GridTools.IsWorldPointOnWalkableTile(worldPoint, _entityManager));

            // Place the instantiated entity in a random point on the map
            //set default variables based on objectToCreate name - not great solution but works for now
            if (objectToCreate.name.Contains("Rabbit"))
            {
                CreateRabbitAtWorldPoint(worldPoint);
            }
            else if (objectToCreate.name.Contains("Fox"))
            {
                CreateFoxAtWorldPoint(worldPoint);
            }
            else if (objectToCreate.name.Contains("Grass"))
            {
                CreateGrassAtWorldPoint(worldPoint);
            }
        }
    }

    private void CreateFoxAtWorldPoint(in float3 worldPoint)
    {
        if (ConversionFox == Entity.Null)
            ConversionFox = GameObjectConversionUtility.ConvertGameObjectHierarchy(_fox, Settings);
        Entity prototypeFox = _entityManager.Instantiate(ConversionFox);

        //set name of entity
        _entityManager.SetName(prototypeFox, $"Fox {FoxPopulation}");

        _entityManager.AddComponent<IsFoxTag>(prototypeFox);
        _entityManager.SetComponentData(prototypeFox,
            new Translation
            {
                Value = worldPoint,
            }
        );

        _entityManager.SetComponentData(prototypeFox,
            new EdibleData
            {
                CanBeEaten = FoxDefaults.CanBeEaten,
                NutritionalValueBase = FoxDefaults.NutritionalValue,
                NutritionalValueMultiplier = FoxDefaults.NutritionalValueMultiplier,
                FoodType = FoxDefaults.FoodType,
            }
        );

        _entityManager.SetComponentData(prototypeFox,
            new MovementData
            {
                RotationSpeed = FoxDefaults.RotationSpeed,
                MoveSpeedBase = FoxDefaults.MoveSpeed,
                MoveMultiplier = FoxDefaults.MoveMultiplier,
                PregnancyMoveMultiplier = FoxDefaults.PregnancyMoveMultiplier,
                OriginalMoveMultiplier = FoxDefaults.OriginalMoveMultiplier,
                YoungMoveMultiplier = FoxDefaults.YoungMoveMultiplier,
                AdultMoveMultiplier = FoxDefaults.AdultMoveMultiplier,
                OldMoveMultiplier = FoxDefaults.OldMoveMultiplier,
            }
        );

        _entityManager.SetComponentData(prototypeFox,
            new StateData
            {
                FlagStateCurrent = FoxDefaults.FlagState,
                FlagStatePrevious = FoxDefaults.PreviousFlagState,
                DeathReason = FoxDefaults.DeathReason,
                BeenEaten = FoxDefaults.BeenEaten,
            }
        );

        _entityManager.SetComponentData(prototypeFox,
            new TargetData
            {
                AtTarget = true,
                Target = worldPoint,
                TargetOld = worldPoint,

                SightRadius = FoxDefaults.SightRadius,
                TouchRadius = FoxDefaults.TouchRadius,
                MateRadius = FoxDefaults.MateRadius,

                PredatorEntity = FoxDefaults.PredatorEntity,
                EntityToEat = FoxDefaults.EntityToEat,
                EntityToDrink = FoxDefaults.EntityToDrink,
                EntityToMate = FoxDefaults.EntityToMate,
                ShortestDistanceToEdible = FoxDefaults.ShortestToEdibleDistance,
                ShortestDistanceToWater = FoxDefaults.ShortestToWaterDistance,
                ShortestDistanceToPredator = FoxDefaults.ShortestToPredatorDistance,
                ShortestDistanceToMate = FoxDefaults.ShortestToMateDistance,
            }
        );

        _entityManager.SetComponentData(prototypeFox,
            new PathFollowData
            {
                PathIndex = -1,
            }
        );

        _entityManager.SetComponentData(prototypeFox,
            new BasicNeedsData
            {
                Hunger = FoxDefaults.Hunger,
                HungryThreshold = FoxDefaults.HungryThreshold,
                HungerMax = FoxDefaults.HungerMax,
                HungerIncrease = FoxDefaults.HungerIncrease,
                PregnancyHungerIncrease = FoxDefaults.PregnancyHungerIncrease,
                YoungHungerIncrease = FoxDefaults.YoungHungerIncrease,
                AdultHungerIncrease = FoxDefaults.AdultHungerIncrease,
                OldHungerIncrease = FoxDefaults.OldHungerIncrease,
                EatingSpeed = FoxDefaults.EatingSpeed,

                Diet = FoxDefaults.Diet,
                Thirst = FoxDefaults.Thirst,
                ThirstyThreshold = FoxDefaults.ThirstyThreshold,
                ThirstMax = FoxDefaults.ThirstMax,
                ThirstIncrease = FoxDefaults.ThirstIncrease,
                DrinkingSpeed = FoxDefaults.DrinkingSpeed,
            }
        );

        //randomise gender of fox - equal distribution
        BioStatsData.Genders randGender = UnityEngine.Random.Range(0, 2) == 1 ? BioStatsData.Genders.Female : BioStatsData.Genders.Male;
        //set gender differing components

        _entityManager.SetComponentData(prototypeFox,
            new BioStatsData
            {
                Age = FoxDefaults.Age,
                AgeIncrease = FoxDefaults.AgeIncrease,
                AgeMax = FoxDefaults.AgeMax,
                AgeGroup = FoxDefaults.AgeGroup,
                AdultEntryTimer = FoxDefaults.AdultEntryTimer,
                OldEntryTimer = FoxDefaults.OldEntryTimer,
                Gender = randGender,
            }
        );

        //set reproductive data differing on gender
        _entityManager.SetComponentData(prototypeFox,
                new ReproductiveData
                {
                    MatingDuration = FoxDefaults.MatingDuration,
                    MateStartTime = FoxDefaults.MateStartTime,
                    ReproductiveUrge = FoxDefaults.ReproductiveUrge,
                    ReproductiveUrgeIncrease = (randGender == BioStatsData.Genders.Female ? FoxDefaults.ReproductiveUrgeIncreaseFemale : FoxDefaults.ReproductiveUrgeIncreaseMale),
                    DefaultReproductiveIncrease = (randGender == BioStatsData.Genders.Female ? FoxDefaults.ReproductiveUrgeIncreaseFemale : FoxDefaults.ReproductiveUrgeIncreaseMale),
                    MatingThreshold = FoxDefaults.MatingThreshold,

                    BirthDuration = FoxDefaults.BirthDuration,
                    BabiesBorn = FoxDefaults.BabiesBorn,
                    BirthStartTime = FoxDefaults.BirthStartTime,
                    CurrentLitterSize = FoxDefaults.CurrentLitterSize,
                    LitterSizeMin = FoxDefaults.LitterSizeMin,
                    LitterSizeMax = FoxDefaults.LitterSizeMax,
                    LitterSizeAve = FoxDefaults.LitterSizeAve,
                    PregnancyLengthBase = FoxDefaults.PregnancyLength,
                    PregnancyLengthModifier = FoxDefaults.PregnancyLengthModifier,
                    PregnancyStartTime = FoxDefaults.PregnancyStartTime,
                }
            );

        //set size differing on gender
        _entityManager.SetComponentData(prototypeFox,
            new SizeData
            {
                size = (randGender == BioStatsData.Genders.Female ? FoxDefaults.ScaleFemale : FoxDefaults.ScaleMale),
                SizeMultiplier = FoxDefaults.SizeMultiplier,
                AgeSizeMultiplier = FoxDefaults.AgeSizeMultiplier,
                YoungSizeMultiplier = FoxDefaults.YoungSizeMultiplier,
                AdultSizeMultiplier = FoxDefaults.AdultSizeMultiplier,
                OldSizeMultiplier = FoxDefaults.OldSizeMultiplier,
            }
        );
        // set ColliderTypeData to Fox entity
        _entityManager.SetComponentData(prototypeFox,
            new ColliderTypeData
            {
                Collider = FoxDefaults.Collider,
            }
        );

        FoxPopulation++;
    }

    private void CreateGrassAtWorldPoint(in float3 worldPoint)
    {
        if (ConversionGrass == Entity.Null)
            ConversionGrass = GameObjectConversionUtility.ConvertGameObjectHierarchy(_grass, Settings);
        Entity prototypeGrass = _entityManager.Instantiate(ConversionGrass);

        //set name of entity
        _entityManager.SetName(prototypeGrass, $"Grass {GrassPopulation}");

        _entityManager.AddComponent<IsGrassTag>(prototypeGrass);
        _entityManager.SetComponentData(prototypeGrass,
            new Translation
            {
                Value = worldPoint,
            }
        );

        _entityManager.SetComponentData(prototypeGrass,
            new EdibleData
            {
                CanBeEaten = GrassDefaults.CanBeEaten,
                NutritionalValueBase = GrassDefaults.NutritionalValue,
                NutritionalValueMultiplier = GrassDefaults.NutritionalValueMultiplier,
                FoodType = GrassDefaults.FoodType,
            }
        );

        _entityManager.SetComponentData(prototypeGrass,
            new StateData
            {
                FlagStateCurrent = GrassDefaults.FlagState,
                FlagStatePrevious = GrassDefaults.PreviousFlagState,
                DeathReason = GrassDefaults.DeathReason,
                BeenEaten = GrassDefaults.BeenEaten,
            }
        );

        //set size differing on gender
        _entityManager.SetComponentData(prototypeGrass,
            new SizeData
            {
                size = GrassDefaults.Scale,
                SizeMultiplier = GrassDefaults.SizeMultiplier,
                AgeSizeMultiplier = 1f,
            }
        );

        // set ColliderTypeData to Grass entity
        _entityManager.SetComponentData(prototypeGrass,
            new ColliderTypeData
            {
                Collider = GrassDefaults.Collider,
            }
        );


        GrassPopulation++;

    }

    private void CreateRabbitAtWorldPoint(in float3 worldPoint)
    {
        if (ConversionRabbit == Entity.Null)
            ConversionRabbit = GameObjectConversionUtility.ConvertGameObjectHierarchy(_rabbit, Settings);
        Entity prototypeRabbit = _entityManager.Instantiate(ConversionRabbit);

        //set name of entity
        _entityManager.SetName(prototypeRabbit, $"Rabbit {RabbitPopulation}");

        _entityManager.AddComponent<IsRabbitTag>(prototypeRabbit);
        _entityManager.SetComponentData(prototypeRabbit,
            new Translation
            {
                Value = worldPoint,
            }
        );


        _entityManager.SetComponentData(prototypeRabbit,
            new EdibleData
            {
                CanBeEaten = RabbitDefaults.CanBeEaten,
                NutritionalValueBase = RabbitDefaults.NutritionalValue,
                NutritionalValueMultiplier = RabbitDefaults.NutritionalValueMultiplier,
                FoodType = RabbitDefaults.FoodType,
            }
        );

        _entityManager.SetComponentData(prototypeRabbit,
            new MovementData
            {
                RotationSpeed = RabbitDefaults.RotationSpeed,
                MoveSpeedBase = RabbitDefaults.MoveSpeed,
                MoveMultiplier = RabbitDefaults.MoveMultiplier,
                PregnancyMoveMultiplier = RabbitDefaults.PregnancyMoveMultiplier,
                OriginalMoveMultiplier = RabbitDefaults.OriginalMoveMultiplier,
                YoungMoveMultiplier = RabbitDefaults.YoungMoveMultiplier,
                AdultMoveMultiplier = RabbitDefaults.AdultMoveMultiplier,
                OldMoveMultiplier = RabbitDefaults.OldMoveMultiplier,
            }
        );

        _entityManager.SetComponentData(prototypeRabbit,
            new StateData
            {
                FlagStateCurrent = RabbitDefaults.FlagState,
                FlagStatePrevious = RabbitDefaults.FlagStatePrevious,
                DeathReason = RabbitDefaults.DeathReason,
                BeenEaten = RabbitDefaults.BeenEaten,
            }
        );

        _entityManager.SetComponentData(prototypeRabbit,
            new TargetData
            {
                AtTarget = true,
                Target = worldPoint,
                TargetOld = worldPoint,

                SightRadius = RabbitDefaults.SightRadius,
                TouchRadius = RabbitDefaults.TouchRadius,
                MateRadius = RabbitDefaults.MateRadius,

                PredatorEntity = FoxDefaults.PredatorEntity,
                EntityToEat = FoxDefaults.EntityToEat,
                EntityToDrink = FoxDefaults.EntityToDrink,
                EntityToMate = FoxDefaults.EntityToMate,
                ShortestDistanceToEdible = FoxDefaults.ShortestToEdibleDistance,
                ShortestDistanceToWater = FoxDefaults.ShortestToWaterDistance,
                ShortestDistanceToPredator = FoxDefaults.ShortestToPredatorDistance,
                ShortestDistanceToMate = FoxDefaults.ShortestToMateDistance,
            }
        );

        _entityManager.SetComponentData(prototypeRabbit,
            new PathFollowData
            {
                PathIndex = -1,
            }
        );

        _entityManager.SetComponentData(prototypeRabbit,
            new BasicNeedsData
            {
                Hunger = RabbitDefaults.Hunger,
                HungryThreshold = RabbitDefaults.HungryThreshold,
                HungerMax = RabbitDefaults.HungerMax,
                HungerIncrease = RabbitDefaults.HungerIncrease,
                PregnancyHungerIncrease = RabbitDefaults.PregnancyHungerIncrease,
                YoungHungerIncrease = RabbitDefaults.YoungHungerIncrease,
                AdultHungerIncrease = RabbitDefaults.AdultHungerIncrease,
                OldHungerIncrease = RabbitDefaults.OldHungerIncrease,
                EatingSpeed = RabbitDefaults.EatingSpeed,

                Diet = RabbitDefaults.Diet,
                Thirst = RabbitDefaults.Thirst,
                ThirstyThreshold = RabbitDefaults.ThirstyThreshold,
                ThirstMax = RabbitDefaults.ThirstMax,
                ThirstIncrease = RabbitDefaults.ThirstIncrease,
                DrinkingSpeed = RabbitDefaults.DrinkingSpeed,

            }
        );

        //randomise gender of rabbit - equal distribution
        BioStatsData.Genders randGender = UnityEngine.Random.Range(0, 2) == 1 ? BioStatsData.Genders.Female : BioStatsData.Genders.Male;
        //set gender differing components

        _entityManager.SetComponentData(prototypeRabbit,
            new BioStatsData
            {
                Age = RabbitDefaults.Age,
                AgeIncrease = RabbitDefaults.AgeIncrease,
                AgeMax = RabbitDefaults.AgeMax,
                AgeGroup = RabbitDefaults.AgeGroup,
                AdultEntryTimer = RabbitDefaults.AdultEntryTimer,
                OldEntryTimer = RabbitDefaults.OldEntryTimer,
                Gender = randGender,
            }
        );

        //set reproductive data differing on gender
        _entityManager.SetComponentData(prototypeRabbit,
                new ReproductiveData
                {
                    MatingDuration = RabbitDefaults.MatingDuration,
                    MateStartTime = RabbitDefaults.MateStartTime,
                    ReproductiveUrge = RabbitDefaults.ReproductiveUrge,
                    ReproductiveUrgeIncrease = (randGender == BioStatsData.Genders.Female ? RabbitDefaults.ReproductiveUrgeIncreaseFemale : RabbitDefaults.ReproductiveUrgeIncreaseMale),
                    DefaultReproductiveIncrease = (randGender == BioStatsData.Genders.Female ? RabbitDefaults.ReproductiveUrgeIncreaseFemale : RabbitDefaults.ReproductiveUrgeIncreaseMale),
                    MatingThreshold = RabbitDefaults.MatingThreshold,


                    BirthDuration = RabbitDefaults.BirthDuration,
                    BabiesBorn = RabbitDefaults.BabiesBorn,
                    BirthStartTime = RabbitDefaults.BirthStartTime,
                    CurrentLitterSize = RabbitDefaults.CurrentLitterSize,
                    LitterSizeMin = RabbitDefaults.LitterSizeMin,
                    LitterSizeMax = RabbitDefaults.LitterSizeMax,
                    LitterSizeAve = RabbitDefaults.LitterSizeAve,
                    PregnancyLengthBase = RabbitDefaults.PregnancyLength,
                    PregnancyLengthModifier = RabbitDefaults.PregnancyLengthModifier,
                    PregnancyStartTime = RabbitDefaults.PregnancyStartTime,
                }
            );

        //set size differing on gender
        _entityManager.SetComponentData(prototypeRabbit,
            new SizeData
            {
                size = (randGender == BioStatsData.Genders.Female ? RabbitDefaults.ScaleFemale : RabbitDefaults.ScaleMale),
                SizeMultiplier = RabbitDefaults.SizeMultiplier,
                AgeSizeMultiplier = RabbitDefaults.AgeSizeMultiplier,
                YoungSizeMultiplier = RabbitDefaults.YoungSizeMultiplier,
                AdultSizeMultiplier = RabbitDefaults.AdultSizeMultiplier,
                OldSizeMultiplier = RabbitDefaults.OldSizeMultiplier,
            }
        );

        // set ColliderTypeData to Rabbit entity
        _entityManager.SetComponentData(prototypeRabbit,
            new ColliderTypeData
            {
                Collider = RabbitDefaults.Collider,
            }
        );

        RabbitPopulation++;
    }

    private void SpawnRabbitAtPosOnLClick()
    {
        //checks for click of the mouse, sends ray out from camera, creates rabbit where it hits
        if (Input.GetMouseButtonDown(0))
        {
            // if not over the UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 targetPosition = hit.point;
                    Debug.Log(targetPosition.ToString());
                    if (UtilTools.GridTools.IsWorldPointOnWalkableTile(targetPosition, _entityManager))
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 targetPosition = hit.point;
                    Debug.Log(targetPosition.ToString());
                    if (UtilTools.GridTools.IsWorldPointOnWalkableTile(targetPosition, _entityManager))
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 targetPosition = hit.point;
                    Debug.Log(targetPosition.ToString());
                    if (UtilTools.GridTools.IsWorldPointOnWalkableTile(targetPosition, _entityManager))
                        CreateGrassAtWorldPoint(targetPosition);
                }
            }
        }
    }
    #endregion

    public static Vector2 MapSize()
    {
        return new Vector2(GridWidth, GridHeight);
    }
}
