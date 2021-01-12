using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpeedTutorMainMenuSystem
{
    public class MenuController : MonoBehaviour
    {
        public enum MenuNumber
        {
            Main,
            NewGame,
            LoadGame,
            Options,
            Graphics,
            Sound,
            Gameplay,
            Controls,
            InitialProperties,
            GenerateMap,
        }

        string fileContents;

        #region Default Values
        [Header("Default Menu Values")]
        [SerializeField] private float defaultBrightness;
        [SerializeField] private float defaultVolume;
        [SerializeField] private int defaultSen;
        [SerializeField] private bool defaultInvertY;



        [Header("Levels To Load")]
        public string _newGameButtonLevel;
        private string levelToLoad;

        private MenuNumber menuNumber;

        #region Entity Default Values For Reset
        float rabbitAgeMaxReset;
        float rabbitNutritionalValueReset;
        bool rabbitCanBeEatenReset;
        float rabbitHungerMaxReset;
        float rabbitHungerThresholdReset;
        float rabbitHungerIncreaseBaseReset;
        float rabbitHungerIncreaseYoungReset;
        float rabbitHungerIncreaseAdultReset;
        float rabbitHungerIncreaseOldReset;
        float rabbitEatingSpeedReset;
        float rabbitThirstMaxReset;
        float rabbitThirstThresholdReset;
        float rabbitThirstIncreaseBaseReset;
        //float rabbitThirstIncreaseYoungReset;
        //float rabbitThirstIncreaseAdultReset;
        //float rabbitThirstIncreaseOldReset;
        float rabbitDrinkingSpeedReset;
        float rabbitMatingDurationReset;
        float rabbitPregnancyLengthReset;
        float rabbitBirthDurationReset;
        float rabbitLitterSizeMinReset;
        float rabbitLitterSizeMaxReset;
        float rabbitLitterSizeAveReset;
        float rabbitMovementSpeedReset;
        float rabbitMovementMultiplierBaseReset;
        float rabbitMovementMultiplierYoungReset;
        float rabbitMovementMultiplierAdultReset;
        float rabbitMovementMultiplierOldReset;
        float rabbitMovementMultiplierPregnantReset;
        float rabbitSightRadiusReset;
        float rabbitSizeMaleReset;
        float rabbitSizeFemaleReset;


        float foxAgeMaxReset;
        float foxNutritionalValueReset;
        bool foxCanBeEatenReset;
        float foxHungerMaxReset;
        float foxHungerThresholdReset;
        float foxHungerIncreaseBaseReset;
        float foxHungerIncreaseYoungReset;
        float foxHungerIncreaseAdultReset;
        float foxHungerIncreaseOldReset;
        float foxEatingSpeedReset;
        float foxThirstMaxReset;
        float foxThirstThresholdReset;
        float foxThirstIncreaseBaseReset;
        //float foxThirstIncreaseYoungReset;
        //float foxThirstIncreaseAdultReset;
        //float foxThirstIncreaseOldReset;
        float foxDrinkingSpeedReset;
        float foxMatingDurationReset;
        float foxPregnancyLengthReset;
        float foxBirthDurationReset;
        float foxLitterSizeMinReset;
        float foxLitterSizeMaxReset;
        float foxLitterSizeAveReset;
        float foxMovementSpeedReset;
        float foxMovementMultiplierBaseReset;
        float foxMovementMultiplierYoungReset;
        float foxMovementMultiplierAdultReset;
        float foxMovementMultiplierOldReset;
        float foxMovementMultiplierPregnantReset;
        float foxSightRadiusReset;
        float foxSizeMaleReset;
        float foxSizeFemaleReset;


        float grassNutritionalValueReset;
        bool grassCanBeEatenReset;
        float grassSizeReset;


        #endregion

        #endregion

        #region Menu Dialogs
        [Header("Main Menu Components")]
        [SerializeField] private GameObject menuDefaultCanvas;
        [SerializeField] private GameObject GeneralSettingsCanvas;
        [SerializeField] private GameObject graphicsMenu;
        [SerializeField] private GameObject soundMenu;
        [SerializeField] private GameObject gameplayMenu;
        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject confirmationMenu;
        [SerializeField] private GameObject initialPropertiesCanvas;
        [SerializeField] private GameObject generateMapCanvas;
        [Space(10)]
        [Header("Menu Popout Dialogs")]
        [SerializeField] private GameObject noSaveDialog;
        [SerializeField] private GameObject newGameDialog;
        [SerializeField] private GameObject loadGameDialog;
        #endregion

        #region Slider Linking
        #region Options
        [Header("Menu Sliders")]
        [SerializeField] private Text controllerSenText;
        [SerializeField] private Slider controllerSenSlider;
        public float controlSenFloat = 2f;
        [Space(10)]
        [SerializeField] private Brightness brightnessEffect;
        [SerializeField] private Slider brightnessSlider;
        [SerializeField] private Text brightnessText;
        [Space(10)]
        [SerializeField] private Text volumeText;
        [SerializeField] private Slider volumeSlider;
        [Space(10)]
        [SerializeField] private Toggle invertYToggle;
        #endregion
        #region Initial Properties
        [Header("Initial Numbers InputFields")]
        [SerializeField] private InputField rabbitNumberInputField;
        [SerializeField] private InputField foxNumberInputField;
        [SerializeField] private InputField grassNumberInputField;

        [Header("Initial Properties Sliders")]
        #region Rabbit
        [Header("Rabbit")]
        [Header("Age")]
        [SerializeField] private Text rabbitAgeMaxText;
        [SerializeField] private Slider rabbitAgeMaxSlider;

        [Header("Edible")]
        [SerializeField] private Text rabbitNutritionalValueText;
        [SerializeField] private Slider rabbitNutritionalValueSlider;
        [Space(5)]
        [SerializeField] private Toggle rabbitCanBeEaten;

        [Header("Hunger")]
        [SerializeField] private Text rabbitHungerMaxText;
        [SerializeField] private Slider rabbitHungerMaxSlider;
        [Space(5)]
        [SerializeField] private Text rabbitHungerThresholdText;
        [SerializeField] private Slider rabbitHungerThresholdSlider;
        [Space(5)]
        [SerializeField] private Text rabbitHungerIncreaseBaseText;
        [SerializeField] private Slider rabbitHungerIncreaseBaseSlider;
        [Space(5)]
        [SerializeField] private Text rabbitHungerIncreaseYoungText;
        [SerializeField] private Slider rabbitHungerIncreaseYoungSlider;
        [Space(5)]
        [SerializeField] private Text rabbitHungerIncreaseAdultText;
        [SerializeField] private Slider rabbitHungerIncreaseAdultSlider;
        [Space(5)]
        [SerializeField] private Text rabbitHungerIncreaseOldText;
        [SerializeField] private Slider rabbitHungerIncreaseOldSlider;
        [Space(5)]
        [SerializeField] private Text rabbitEatingSpeedText;
        [SerializeField] private Slider rabbitEatingSpeedSlider;

        [Header("Thirst")]
        [SerializeField] private Text rabbitThirstMaxText;
        [SerializeField] private Slider rabbitThirstMaxSlider;
        [Space(5)]
        [SerializeField] private Text rabbitThirstThresholdText;
        [SerializeField] private Slider rabbitThirstThresholdSlider;
        [Space(5)]
        [SerializeField] private Text rabbitThirstIncreaseBaseText;
        [SerializeField] private Slider rabbitThirstIncreaseBaseSlider;
        [Space(5)]
        [SerializeField] private Text rabbitThirstIncreaseYoungText;
        [SerializeField] private Slider rabbitThirstIncreaseYoungSlider;
        [Space(5)]
        [SerializeField] private Text rabbitThirstIncreaseAdultText;
        [SerializeField] private Slider rabbitThirstIncreaseAdultSlider;
        [Space(5)]
        [SerializeField] private Text rabbitThirstIncreaseOldText;
        [SerializeField] private Slider rabbitThirstIncreaseOldSlider;
        [Space(5)]
        [SerializeField] private Text rabbitDrinkingSpeedText;
        [SerializeField] private Slider rabbitDrinkingSpeedSlider;

        [Header("Mate")]
        [SerializeField] private Text rabbitMatingDurationText;
        [SerializeField] private Slider rabbitMatingDurationSlider;

        [Header("Pregnancy")]
        [SerializeField] private Text rabbitPregnancyLengthText;
        [SerializeField] private Slider rabbitPregnancyLengthSlider;
        [Space(5)]
        [SerializeField] private Text rabbitBirthDurationText;
        [SerializeField] private Slider rabbitBirthDurationSlider;
        [Space(5)]
        [SerializeField] private Text rabbitLitterSizeMinText;
        [SerializeField] private Slider rabbitLitterSizeMinSlider;
        [Space(5)]
        [SerializeField] private Text rabbitLitterSizeMaxText;
        [SerializeField] private Slider rabbitLitterSizeMaxSlider;
        [Space(5)]
        [SerializeField] private Text rabbitLitterSizeAveText;
        [SerializeField] private Slider rabbitLitterSizeAveSlider;

        [Header("Movement")]
        [SerializeField] private Text rabbitMovementSpeedText;
        [SerializeField] private Slider rabbitMovementSpeedSlider;
        [Space(5)]
        [SerializeField] private Text rabbitMovementMultiplierBaseText;
        [SerializeField] private Slider rabbitMovementMultiplierBaseSlider;
        [Space(5)]
        [SerializeField] private Text rabbitMovementMultiplierYoungText;
        [SerializeField] private Slider rabbitMovementMultiplierYoungSlider;
        [Space(5)]
        [SerializeField] private Text rabbitMovementMultiplierAdultText;
        [SerializeField] private Slider rabbitMovementMultiplierAdultSlider;
        [Space(5)]
        [SerializeField] private Text rabbitMovementMultiplierOldText;
        [SerializeField] private Slider rabbitMovementMultiplierOldSlider;
        [Space(5)]
        [SerializeField] private Text rabbitMovementMultiplierPregnantText;
        [SerializeField] private Slider rabbitMovementMultiplierPregnantSlider;

        [Header("Target")]
        [SerializeField] private Text rabbitSightRadiusText;
        [SerializeField] private Slider rabbitSightRadiusSlider;

        [Header("Size")]
        [SerializeField] private Text rabbitSizeMaleText;
        [SerializeField] private Slider rabbitSizeMaleSlider;
        [Space(5)]
        [SerializeField] private Text rabbitSizeFemaleText;
        [SerializeField] private Slider rabbitSizeFemaleSlider;
        #endregion
        #region Fox
        [Header("Fox")]
        [Header("Age")]
        [SerializeField] private Text foxAgeMaxText;
        [SerializeField] private Slider foxAgeMaxSlider;

        [Header("Edible")]
        [SerializeField] private Text foxNutritionalValueText;
        [SerializeField] private Slider foxNutritionalValueSlider;
        [Space(5)]
        [SerializeField] private Toggle foxCanBeEaten;

        [Header("Hunger")]
        [SerializeField] private Text foxHungerMaxText;
        [SerializeField] private Slider foxHungerMaxSlider;
        [Space(5)]
        [SerializeField] private Text foxHungerThresholdText;
        [SerializeField] private Slider foxHungerThresholdSlider;
        [Space(5)]
        [SerializeField] private Text foxHungerIncreaseBaseText;
        [SerializeField] private Slider foxHungerIncreaseBaseSlider;
        [Space(5)]
        [SerializeField] private Text foxHungerIncreaseYoungText;
        [SerializeField] private Slider foxHungerIncreaseYoungSlider;
        [Space(5)]
        [SerializeField] private Text foxHungerIncreaseAdultText;
        [SerializeField] private Slider foxHungerIncreaseAdultSlider;
        [Space(5)]
        [SerializeField] private Text foxHungerIncreaseOldText;
        [SerializeField] private Slider foxHungerIncreaseOldSlider;
        [Space(5)]
        [SerializeField] private Text foxEatingSpeedText;
        [SerializeField] private Slider foxEatingSpeedSlider;

        [Header("Thirst")]
        [SerializeField] private Text foxThirstMaxText;
        [SerializeField] private Slider foxThirstMaxSlider;
        [Space(5)]
        [SerializeField] private Text foxThirstThresholdText;
        [SerializeField] private Slider foxThirstThresholdSlider;
        [Space(5)]
        [SerializeField] private Text foxThirstIncreaseBaseText;
        [SerializeField] private Slider foxThirstIncreaseBaseSlider;
        [Space(5)]
        [SerializeField] private Text foxThirstIncreaseYoungText;
        [SerializeField] private Slider foxThirstIncreaseYoungSlider;
        [Space(5)]
        [SerializeField] private Text foxThirstIncreaseAdultText;
        [SerializeField] private Slider foxThirstIncreaseAdultSlider;
        [Space(5)]
        [SerializeField] private Text foxThirstIncreaseOldText;
        [SerializeField] private Slider foxThirstIncreaseOldSlider;
        [Space(5)]
        [SerializeField] private Text foxDrinkingSpeedText;
        [SerializeField] private Slider foxDrinkingSpeedSlider;

        [Header("Mate")]
        [SerializeField] private Text foxMatingDurationText;
        [SerializeField] private Slider foxMatingDurationSlider;

        [Header("Pregnancy")]
        [SerializeField] private Text foxPregnancyLengthText;
        [SerializeField] private Slider foxPregnancyLengthSlider;
        [Space(5)]
        [SerializeField] private Text foxBirthDurationText;
        [SerializeField] private Slider foxBirthDurationSlider;
        [Space(5)]
        [SerializeField] private Text foxLitterSizeMinText;
        [SerializeField] private Slider foxLitterSizeMinSlider;
        [Space(5)]
        [SerializeField] private Text foxLitterSizeMaxText;
        [SerializeField] private Slider foxLitterSizeMaxSlider;
        [Space(5)]
        [SerializeField] private Text foxLitterSizeAveText;
        [SerializeField] private Slider foxLitterSizeAveSlider;

        [Header("Movement")]
        [SerializeField] private Text foxMovementSpeedText;
        [SerializeField] private Slider foxMovementSpeedSlider;
        [Space(5)]
        [SerializeField] private Text foxMovementMultiplierBaseText;
        [SerializeField] private Slider foxMovementMultiplierBaseSlider;
        [Space(5)]
        [SerializeField] private Text foxMovementMultiplierYoungText;
        [SerializeField] private Slider foxMovementMultiplierYoungSlider;
        [Space(5)]
        [SerializeField] private Text foxMovementMultiplierAdultText;
        [SerializeField] private Slider foxMovementMultiplierAdultSlider;
        [Space(5)]
        [SerializeField] private Text foxMovementMultiplierOldText;
        [SerializeField] private Slider foxMovementMultiplierOldSlider;
        [Space(5)]
        [SerializeField] private Text foxMovementMultiplierPregnantText;
        [SerializeField] private Slider foxMovementMultiplierPregnantSlider;

        [Header("Target")]
        [SerializeField] private Text foxSightRadiusText;
        [SerializeField] private Slider foxSightRadiusSlider;

        [Header("Size")]
        [SerializeField] private Text foxSizeMaleText;
        [SerializeField] private Slider foxSizeMaleSlider;
        [Space(5)]
        [SerializeField] private Text foxSizeFemaleText;
        [SerializeField] private Slider foxSizeFemaleSlider;
        #endregion
        #region Grass
        [Header("Grass")]
        [Header("Edible")]
        [SerializeField] private Text grassNutritionalValueText;
        [SerializeField] private Slider grassNutritionalValueSlider;
        [Space(5)]
        [SerializeField] private Toggle grassCanBeEaten;

        [Header("Size")]
        [SerializeField] private Text grassSizeText;
        [SerializeField] private Slider grassSizeSlider;
        #endregion
        #endregion
        #endregion

        #region Initialisation - Button Selection & Menu Order
        private void Start()
        {
            menuNumber = MenuNumber.Main;
            StoreInitialEntityDefaults();
        }
        #endregion

        #region Main Section
        public IEnumerator ConfirmationBox()
        {
            confirmationMenu.SetActive(true);
            yield return new WaitForSeconds(2);
            confirmationMenu.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuNumber == MenuNumber.Options || menuNumber == MenuNumber.NewGame || menuNumber == MenuNumber.LoadGame || menuNumber == MenuNumber.InitialProperties || menuNumber == MenuNumber.GenerateMap)
                {
                    GoBackToMainMenu();
                    ClickSound();
                }

                else if (menuNumber == MenuNumber.Graphics || menuNumber == MenuNumber.Sound || menuNumber == MenuNumber.Gameplay)
                {
                    GoBackToOptionsMenu();
                    ClickSound();
                }

                else if (menuNumber == MenuNumber.Controls) //CONTROLS MENU
                {
                    GoBackToGameplayMenu();
                    ClickSound();
                }
            }
        }

        private void ClickSound()
        {
            GetComponent<AudioSource>().Play();
        }
        #endregion

        #region Menu Mouse Clicks
        public void MouseClick(string buttonType)
        {
            switch (buttonType)
            {
                case "Controls":
                    gameplayMenu.SetActive(false);
                    controlsMenu.SetActive(true);
                    menuNumber = MenuNumber.Controls;
                    break;
                case "Graphics":
                    GeneralSettingsCanvas.SetActive(false);
                    graphicsMenu.SetActive(true);
                    menuNumber = MenuNumber.Graphics;
                    break;
                case "Sound":
                    GeneralSettingsCanvas.SetActive(false);
                    soundMenu.SetActive(true);
                    menuNumber = MenuNumber.Sound;
                    break;
                case "Gameplay":
                    GeneralSettingsCanvas.SetActive(false);
                    gameplayMenu.SetActive(true);
                    menuNumber = MenuNumber.Gameplay;
                    break;
                case "Exit":
                    Debug.Log("YES QUIT!");
                    Application.Quit();
                    break;
                case "Options":
                    menuDefaultCanvas.SetActive(false);
                    GeneralSettingsCanvas.SetActive(true);
                    menuNumber = MenuNumber.Options;
                    break;
                case "LoadGame":
                    menuDefaultCanvas.SetActive(false);
                    loadGameDialog.SetActive(true);
                    menuNumber = MenuNumber.LoadGame;
                    LoadMap();
                    break;
                case "NewGame":
                    menuDefaultCanvas.SetActive(false);
                    ResetButton("InitialProperties");
                    initialPropertiesCanvas.SetActive(true);
                    menuNumber = MenuNumber.NewGame;
                    break;
                case "InitialProperties":
                    initialPropertiesCanvas.SetActive(false);
                    newGameDialog.SetActive(true);
                    menuNumber = MenuNumber.InitialProperties;
                    break;
                case "GenerateMap":
                    menuDefaultCanvas.SetActive(false);
                    generateMapCanvas.SetActive(true);
                    menuNumber = MenuNumber.GenerateMap;
                    break;
                default:
                    Debug.Log("Button clicked with no known case.");
                    break;
            }
        }
        #endregion

        #region Loading Map From File
        public void LoadMap()
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", false);
            if (paths.Length > 0)
            {
                StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
            }
        }

        private IEnumerator OutputRoutine(string url)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                fileContents = webRequest.downloadHandler.text;
                Debug.Log(fileContents);
            }
        }

        // attempts to load the given file into the MapReader
        private bool MapFileValid()
        {
            List<List<MapReader.TerrainCost>> mapList = new List<List<MapReader.TerrainCost>>();
            return MapReader.ReadInMapFromString(fileContents, ref mapList);
        }
        #endregion

        #region Options
        public void VolumeSlider(float volume)
        {
            AudioListener.volume = volume;
            volumeText.text = volume.ToString("0.0");
        }

        public void VolumeApply()
        {
            PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
            Debug.Log(PlayerPrefs.GetFloat("masterVolume"));
            StartCoroutine(ConfirmationBox());
        }

        public void BrightnessSlider(float brightness)
        {
            brightnessEffect.brightness = brightness;
            brightnessText.text = brightness.ToString("0.0");
        }

        public void BrightnessApply()
        {
            PlayerPrefs.SetFloat("masterBrightness", brightnessEffect.brightness);
            Debug.Log(PlayerPrefs.GetFloat("masterBrightness"));
            StartCoroutine(ConfirmationBox());
        }

        public void ControllerSen()
        {
            controllerSenText.text = controllerSenSlider.value.ToString("0");
            controlSenFloat = controllerSenSlider.value;
        }

        public void GameplayApply()
        {
            if (invertYToggle.isOn) //Invert Y ON
            {
                PlayerPrefs.SetInt("masterInvertY", 1);
                Debug.Log("Invert" + " " + PlayerPrefs.GetInt("masterInvertY"));
            }

            else if (!invertYToggle.isOn) //Invert Y OFF
            {
                PlayerPrefs.SetInt("masterInvertY", 0);
                Debug.Log(PlayerPrefs.GetInt("masterInvertY"));
            }

            PlayerPrefs.SetFloat("masterSen", controlSenFloat);
            Debug.Log("Sensitivity" + " " + PlayerPrefs.GetFloat("masterSen"));

            StartCoroutine(ConfirmationBox());
        }
        #endregion

        #region Initial Properties
        public void InitialPropertiesApply()
        {
            Debug.Log("Apply Initial Properties");
                    RabbitDefaults.ageMax = rabbitAgeMaxSlider.value;
                    RabbitDefaults.nutritionalValue = rabbitNutritionalValueSlider.value;
            RabbitDefaults.canBeEaten = rabbitCanBeEaten.isOn;

                    RabbitDefaults.hungerMax = rabbitHungerMaxSlider.value;
                    RabbitDefaults.hungryThreshold = rabbitHungerThresholdSlider.value;
                    RabbitDefaults.hungerIncrease = rabbitHungerIncreaseBaseSlider.value;
                    RabbitDefaults.youngHungerIncrease = rabbitHungerIncreaseYoungSlider.value;
                    RabbitDefaults.adultHungerIncrease = rabbitHungerIncreaseAdultSlider.value;
                    RabbitDefaults.oldHungerIncrease = rabbitHungerIncreaseOldSlider.value;
                    RabbitDefaults.eatingSpeed = rabbitEatingSpeedSlider.value;
                    RabbitDefaults.thirstMax = rabbitThirstMaxSlider.value;
                    RabbitDefaults.thirstyThreshold = rabbitThirstThresholdSlider.value;
                    RabbitDefaults.thirstIncrease = rabbitThirstIncreaseBaseSlider.value;
                    //RabbitDefaults. = rabbitThirstIncreaseYoungSlider.value;
                    //RabbitDefaults. = rabbitThirstIncreaseAdultSlider.value;
                    //RabbitDefaults. = rabbitThirstIncreaseOldSlider.value;
                    RabbitDefaults.drinkingSpeed = rabbitDrinkingSpeedSlider.value;
                    RabbitDefaults.matingDuration = rabbitMatingDurationSlider.value;
                    RabbitDefaults.pregnancyLength = rabbitPregnancyLengthSlider.value;
                    RabbitDefaults.birthDuration = rabbitBirthDurationSlider.value;
                    RabbitDefaults.litterSizeMin = (int)rabbitLitterSizeMinSlider.value;
                    RabbitDefaults.litterSizeMax = (int)rabbitLitterSizeMaxSlider.value;
                    RabbitDefaults.litterSizeAve = (int)rabbitLitterSizeAveSlider.value;
                    RabbitDefaults.moveSpeed = rabbitMovementSpeedSlider.value;
                    RabbitDefaults.originalMoveMultiplier = rabbitMovementMultiplierBaseSlider.value;
                    RabbitDefaults.youngMoveMultiplier = rabbitMovementMultiplierYoungSlider.value;
                    RabbitDefaults.adultMoveMultiplier = rabbitMovementMultiplierAdultSlider.value;
                    RabbitDefaults.oldMoveMultiplier = rabbitMovementMultiplierOldSlider.value;
                    RabbitDefaults.pregnancyMoveMultiplier = rabbitMovementMultiplierPregnantSlider.value;
                    RabbitDefaults.sightRadius = rabbitSightRadiusSlider.value;
                    RabbitDefaults.scaleMale = rabbitSizeMaleSlider.value;
                    RabbitDefaults.scaleFemale = rabbitSizeFemaleSlider.value;
                    FoxDefaults.ageMax = foxAgeMaxSlider.value;
                    FoxDefaults.nutritionalValue = foxNutritionalValueSlider.value;
                    FoxDefaults.hungerMax = foxHungerMaxSlider.value;
                    FoxDefaults.hungryThreshold = foxHungerThresholdSlider.value;
                    FoxDefaults.hungerIncrease = foxHungerIncreaseBaseSlider.value;
                    FoxDefaults.youngHungerIncrease = foxHungerIncreaseYoungSlider.value;
                    FoxDefaults.adultHungerIncrease = foxHungerIncreaseAdultSlider.value;
                    FoxDefaults.oldHungerIncrease = foxHungerIncreaseOldSlider.value;
                    FoxDefaults.eatingSpeed = foxEatingSpeedSlider.value;
                    FoxDefaults.thirstMax = foxThirstMaxSlider.value;
                    FoxDefaults.thirstyThreshold = foxThirstThresholdSlider.value;
                    FoxDefaults.thirstIncrease = foxThirstIncreaseBaseSlider.value;
                    //FoxDefaults. = foxThirstIncreaseYoungSlider.value;
                    //FoxDefaults. = foxThirstIncreaseAdultSlider.value;
                    //FoxDefaults. = foxThirstIncreaseOldSlider.value;
                    FoxDefaults.drinkingSpeed = foxDrinkingSpeedSlider.value;
                    FoxDefaults.matingDuration = foxMatingDurationSlider.value;
                    FoxDefaults.pregnancyLength = foxPregnancyLengthSlider.value;
                    FoxDefaults.birthDuration = foxBirthDurationSlider.value;
                    FoxDefaults.litterSizeMin = (int)foxLitterSizeMinSlider.value;
                    FoxDefaults.litterSizeMax = (int)foxLitterSizeMaxSlider.value;
                    FoxDefaults.litterSizeAve = (int)foxLitterSizeAveSlider.value;
                    FoxDefaults.moveSpeed = foxMovementSpeedSlider.value;
                    FoxDefaults.originalMoveMultiplier = foxMovementMultiplierBaseSlider.value;
                    FoxDefaults.youngMoveMultiplier = foxMovementMultiplierYoungSlider.value;
                    FoxDefaults.adultMoveMultiplier = foxMovementMultiplierAdultSlider.value;
                    FoxDefaults.oldMoveMultiplier = foxMovementMultiplierOldSlider.value;
                    FoxDefaults.pregnancyMoveMultiplier = foxMovementMultiplierPregnantSlider.value;
                    FoxDefaults.sightRadius = foxSightRadiusSlider.value;
                    FoxDefaults.scaleMale = foxSizeMaleSlider.value;
                    FoxDefaults.scaleFemale = foxSizeFemaleSlider.value;
                    GrassDefaults.nutritionalValue = grassNutritionalValueSlider.value;
                    GrassDefaults.canBeEaten = grassCanBeEaten.isOn;
                    GrassDefaults.scale = grassSizeSlider.value;
            StartCoroutine(ConfirmationBox());
        }

        public void OnSetNumberToSpawn(string entityToUpdate)
        {
            switch (entityToUpdate)
            {
                case "Rabbit":
                    SimulationManager.InitialRabbitsToSpawn = int.Parse(rabbitNumberInputField.text);
                    break;
                case "Fox":
                    SimulationManager.InitialFoxesToSpawn = int.Parse(foxNumberInputField.text);
                    break;
                case "Grass":
                    SimulationManager.InitialGrassToSpawn = int.Parse(grassNumberInputField.text);
                    break;
                default:
                    Debug.LogWarning("Attempted to update unknown entity in switch: " + entityToUpdate, this);
                    break;
            }
        }

        public void InitialPropertiesUpdate(string propertyToUpdate)
        {
            switch (propertyToUpdate)
            {
                case "RabbitAgeMax":
                    rabbitAgeMaxText.text = rabbitAgeMaxSlider.value.ToString();
                    break;
                case "RabbitNutritionalValue":
                    rabbitNutritionalValueText.text = rabbitNutritionalValueSlider.value.ToString();
                    break;
                case "RabbitCanBeEaten":
                    //rabbitCanBeEaten;
                    break;
                case "RabbitHungerMax":
                    rabbitHungerMaxText.text = rabbitHungerMaxSlider.value.ToString();
                    break;
                case "RabbitHungerThreshold":
                    rabbitHungerThresholdText.text = rabbitHungerThresholdSlider.value.ToString();
                    break;
                case "RabbitHungerIncreaseBase":
                    rabbitHungerIncreaseBaseText.text = rabbitHungerIncreaseBaseSlider.value.ToString();
                    break;
                case "RabbitHungerIncreaseYoung":
                    rabbitHungerIncreaseYoungText.text = rabbitHungerIncreaseYoungSlider.value.ToString();
                    break;
                case "RabbitHungerIncreaseAdult":
                    rabbitHungerIncreaseAdultText.text = rabbitHungerIncreaseAdultSlider.value.ToString();
                    break;
                case "RabbitHungerIncreaseOld":
                    rabbitHungerIncreaseOldText.text = rabbitHungerIncreaseOldSlider.value.ToString();
                    break;
                case "RabbitEatingSpeed":
                    rabbitEatingSpeedText.text = rabbitEatingSpeedSlider.value.ToString();
                    break;
                case "RabbitThirstMax":
                    rabbitThirstMaxText.text = rabbitThirstMaxSlider.value.ToString();
                    break;
                case "RabbitThirstThreshold":
                    rabbitThirstThresholdText.text = rabbitThirstThresholdSlider.value.ToString();
                    break;
                case "RabbitThirstIncreaseBase":
                    rabbitThirstIncreaseBaseText.text = rabbitThirstIncreaseBaseSlider.value.ToString();
                    break;
                case "RabbitThirstIncreaseYoung":
                    rabbitThirstIncreaseYoungText.text = rabbitThirstIncreaseYoungSlider.value.ToString();
                    break;
                case "RabbitThirstIncreaseAdult":
                    rabbitThirstIncreaseAdultText.text = rabbitThirstIncreaseAdultSlider.value.ToString();
                    break;
                case "RabbitThirstIncreaseOld":
                    rabbitThirstIncreaseOldText.text = rabbitThirstIncreaseOldSlider.value.ToString();
                    break;
                case "RabbitDrinkingSpeed":
                    rabbitDrinkingSpeedText.text = rabbitDrinkingSpeedSlider.value.ToString();
                    break;
                case "RabbitMatingDuration":
                    rabbitMatingDurationText.text = rabbitMatingDurationSlider.value.ToString();
                    break;
                case "RabbitPregnancyLength":
                    rabbitPregnancyLengthText.text = rabbitPregnancyLengthSlider.value.ToString();
                    break;
                case "RabbitBirthDuration":
                    rabbitBirthDurationText.text = rabbitBirthDurationSlider.value.ToString();
                    break;
                case "RabbitLitterSizeMin":
                    rabbitLitterSizeMinText.text = rabbitLitterSizeMinSlider.value.ToString();
                    break;
                case "RabbitLitterSizeMax":
                    rabbitLitterSizeMaxText.text = rabbitLitterSizeMaxSlider.value.ToString();
                    break;
                case "RabbitLitterSizeAve":
                    rabbitLitterSizeAveText.text = rabbitLitterSizeAveSlider.value.ToString();
                    break;
                case "RabbitMovementSpeed":
                    rabbitMovementSpeedText.text = rabbitMovementSpeedSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierBase":
                    rabbitMovementMultiplierBaseText.text = rabbitMovementMultiplierBaseSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierYoung":
                    rabbitMovementMultiplierYoungText.text = rabbitMovementMultiplierYoungSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierAdult":
                    rabbitMovementMultiplierAdultText.text = rabbitMovementMultiplierAdultSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierOld":
                    rabbitMovementMultiplierOldText.text = rabbitMovementMultiplierOldSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierPregnant":
                    rabbitMovementMultiplierPregnantText.text = rabbitMovementMultiplierPregnantSlider.value.ToString();
                    break;
                case "RabbitSightRadius":
                    rabbitSightRadiusText.text = rabbitSightRadiusSlider.value.ToString();
                    break;
                case "RabbitSizeMale":
                    rabbitSizeMaleText.text = rabbitSizeMaleSlider.value.ToString();
                    break;
                case "RabbitSizeFemale":
                    rabbitSizeFemaleText.text = rabbitSizeFemaleSlider.value.ToString();
                    break;
                case "FoxAgeMax":
                    foxAgeMaxText.text = foxAgeMaxSlider.value.ToString();
                    break;
                case "FoxNutritionalValue":
                    foxNutritionalValueText.text = foxNutritionalValueSlider.value.ToString();
                    break;
                case "FoxCanBeEaten":
                    //foxCanBeEaten;
                    FoxDefaults.canBeEaten = foxCanBeEaten.isOn;
                    break;
                case "FoxHungerMax":
                    foxHungerMaxText.text = foxHungerMaxSlider.value.ToString();
                    break;
                case "FoxHungerThreshold":
                    foxHungerThresholdText.text = foxHungerThresholdSlider.value.ToString();
                    break;
                case "FoxHungerIncreaseBase":
                    foxHungerIncreaseBaseText.text = foxHungerIncreaseBaseSlider.value.ToString();
                    break;
                case "FoxHungerIncreaseYoung":
                    foxHungerIncreaseYoungText.text = foxHungerIncreaseYoungSlider.value.ToString();
                    break;
                case "FoxHungerIncreaseAdult":
                    foxHungerIncreaseAdultText.text = foxHungerIncreaseAdultSlider.value.ToString();
                    break;
                case "FoxHungerIncreaseOld":
                    foxHungerIncreaseOldText.text = foxHungerIncreaseOldSlider.value.ToString();
                    break;
                case "FoxEatingSpeed":
                    foxEatingSpeedText.text = foxEatingSpeedSlider.value.ToString();
                    break;
                case "FoxThirstMax":
                    foxThirstMaxText.text = foxThirstMaxSlider.value.ToString();
                    break;
                case "FoxThirstThreshold":
                    foxThirstThresholdText.text = foxThirstThresholdSlider.value.ToString();
                    break;
                case "FoxThirstIncreaseBase":
                    foxThirstIncreaseBaseText.text = foxThirstIncreaseBaseSlider.value.ToString();
                    break;
                case "FoxThirstIncreaseYoung":
                    foxThirstIncreaseYoungText.text = foxThirstIncreaseYoungSlider.value.ToString();
                    break;
                case "FoxThirstIncreaseAdult":
                    foxThirstIncreaseAdultText.text = foxThirstIncreaseAdultSlider.value.ToString();
                    break;
                case "FoxThirstIncreaseOld":
                    foxThirstIncreaseOldText.text = foxThirstIncreaseOldSlider.value.ToString();
                    break;
                case "FoxDrinkingSpeed":
                    foxDrinkingSpeedText.text = foxDrinkingSpeedSlider.value.ToString();
                    break;
                case "FoxMatingDuration":
                    foxMatingDurationText.text = foxMatingDurationSlider.value.ToString();
                    break;
                case "FoxPregnancyLength":
                    foxPregnancyLengthText.text = foxPregnancyLengthSlider.value.ToString();
                    break;
                case "FoxBirthDuration":
                    foxBirthDurationText.text = foxBirthDurationSlider.value.ToString();
                    break;
                case "FoxLitterSizeMin":
                    foxLitterSizeMinText.text = foxLitterSizeMinSlider.value.ToString();
                    break;
                case "FoxLitterSizeMax":
                    foxLitterSizeMaxText.text = foxLitterSizeMaxSlider.value.ToString();
                    break;
                case "FoxLitterSizeAve":
                    foxLitterSizeAveText.text = foxLitterSizeAveSlider.value.ToString();
                    break;
                case "FoxMovementSpeed":
                    foxMovementSpeedText.text = foxMovementSpeedSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierBase":
                    foxMovementMultiplierBaseText.text = foxMovementMultiplierBaseSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierYoung":
                    foxMovementMultiplierYoungText.text = foxMovementMultiplierYoungSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierAdult":
                    foxMovementMultiplierAdultText.text = foxMovementMultiplierAdultSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierOld":
                    foxMovementMultiplierOldText.text = foxMovementMultiplierOldSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierPregnant":
                    foxMovementMultiplierPregnantText.text = foxMovementMultiplierPregnantSlider.value.ToString();
                    break;
                case "FoxSightRadius":
                    foxSightRadiusText.text = foxSightRadiusSlider.value.ToString();
                    break;
                case "FoxSizeMale":
                    foxSizeMaleText.text = foxSizeMaleSlider.value.ToString();
                    break;
                case "FoxSizeFemale":
                    foxSizeFemaleText.text = foxSizeFemaleSlider.value.ToString();
                    break;
                case "GrassNutritionalValue":
                    grassNutritionalValueText.text = grassNutritionalValueSlider.value.ToString();
                    break;
                case "GrassCanBeEaten":
                    //grassCanBeEaten;
                    break;
                case "GrassSize":
                    grassSizeText.text = grassSizeSlider.value.ToString();
                    break;
                default:
                    Debug.LogWarning("Attempted to update unknown property in switch: " + propertyToUpdate, this);
                    break;
            }
            InitialPropertiesApply();
        }
        #endregion

        #region ResetButton

        private void StoreInitialEntityDefaults()
        {
            rabbitAgeMaxReset = RabbitDefaults.ageMax;

            rabbitNutritionalValueReset = RabbitDefaults.nutritionalValue;

            rabbitCanBeEatenReset = RabbitDefaults.canBeEaten;

            rabbitHungerMaxReset = RabbitDefaults.hungerMax;

            rabbitHungerThresholdReset = RabbitDefaults.hungryThreshold;

            rabbitHungerIncreaseBaseReset = RabbitDefaults.hungerIncrease;

            rabbitHungerIncreaseYoungReset = RabbitDefaults.youngHungerIncrease;

            rabbitHungerIncreaseAdultReset = RabbitDefaults.adultHungerIncrease;

            rabbitHungerIncreaseOldReset = RabbitDefaults.oldHungerIncrease;

            rabbitEatingSpeedReset = RabbitDefaults.eatingSpeed;

            rabbitThirstMaxReset = RabbitDefaults.thirstMax;

            rabbitThirstThresholdReset = RabbitDefaults.thirstyThreshold;

            rabbitThirstIncreaseBaseReset = RabbitDefaults.thirstIncrease;

            //rabbitThirstIncreaseYoungReset = RabbitDefaults. ;

            //rabbitThirstIncreaseAdultReset = RabbitDefaults. ;

            //rabbitThirstIncreaseOldReset = RabbitDefaults. ;

            rabbitDrinkingSpeedReset = RabbitDefaults.drinkingSpeed;

            rabbitMatingDurationReset = RabbitDefaults.matingDuration;

            rabbitPregnancyLengthReset = RabbitDefaults.pregnancyLength;

            rabbitBirthDurationReset = RabbitDefaults.birthDuration;

            rabbitLitterSizeMinReset = RabbitDefaults.litterSizeMin;

            rabbitLitterSizeMaxReset = RabbitDefaults.litterSizeMax;

            rabbitLitterSizeAveReset = RabbitDefaults.litterSizeAve;

            rabbitMovementSpeedReset = RabbitDefaults.moveSpeed;

            rabbitMovementMultiplierBaseReset = RabbitDefaults.originalMoveMultiplier;

            rabbitMovementMultiplierYoungReset = RabbitDefaults.youngMoveMultiplier;

            rabbitMovementMultiplierAdultReset = RabbitDefaults.adultMoveMultiplier;

            rabbitMovementMultiplierOldReset = RabbitDefaults.oldMoveMultiplier;

            rabbitMovementMultiplierPregnantReset = RabbitDefaults.pregnancyMoveMultiplier;

            rabbitSightRadiusReset = RabbitDefaults.sightRadius;

            rabbitSizeMaleReset = RabbitDefaults.scaleMale;

            rabbitSizeFemaleReset = RabbitDefaults.scaleFemale;



            foxAgeMaxReset = FoxDefaults.ageMax;

            foxNutritionalValueReset = FoxDefaults.nutritionalValue;

            foxCanBeEatenReset = FoxDefaults.canBeEaten;

            foxHungerMaxReset = FoxDefaults.hungerMax;

            foxHungerThresholdReset = FoxDefaults.hungryThreshold;

            foxHungerIncreaseBaseReset = FoxDefaults.hungerIncrease;

            foxHungerIncreaseYoungReset = FoxDefaults.youngHungerIncrease;

            foxHungerIncreaseAdultReset = FoxDefaults.adultHungerIncrease;

            foxHungerIncreaseOldReset = FoxDefaults.oldHungerIncrease;

            foxEatingSpeedReset = FoxDefaults.eatingSpeed;

            foxThirstMaxReset = FoxDefaults.thirstMax;

            foxThirstThresholdReset = FoxDefaults.thirstyThreshold;

            foxThirstIncreaseBaseReset = FoxDefaults.thirstIncrease;

            //foxThirstIncreaseYoungReset = FoxDefaults. ;

            //foxThirstIncreaseAdultReset = FoxDefaults. ;

            //foxThirstIncreaseOldReset = FoxDefaults. ;

            foxDrinkingSpeedReset = FoxDefaults.drinkingSpeed;

            foxMatingDurationReset = FoxDefaults.matingDuration;

            foxPregnancyLengthReset = FoxDefaults.pregnancyLength;

            foxBirthDurationReset = FoxDefaults.birthDuration;

            foxLitterSizeMinReset = FoxDefaults.litterSizeMin;

            foxLitterSizeMaxReset = FoxDefaults.litterSizeMax;

            foxLitterSizeAveReset = FoxDefaults.litterSizeAve;

            foxMovementSpeedReset = FoxDefaults.moveSpeed;

            foxMovementMultiplierBaseReset = FoxDefaults.originalMoveMultiplier;

            foxMovementMultiplierYoungReset = FoxDefaults.youngMoveMultiplier;

            foxMovementMultiplierAdultReset = FoxDefaults.adultMoveMultiplier;

            foxMovementMultiplierOldReset = FoxDefaults.oldMoveMultiplier;

            foxMovementMultiplierPregnantReset = FoxDefaults.pregnancyMoveMultiplier;

            foxSightRadiusReset = FoxDefaults.sightRadius;

            foxSizeMaleReset = FoxDefaults.scaleMale;

            foxSizeFemaleReset = FoxDefaults.scaleFemale;



            grassNutritionalValueReset = GrassDefaults.nutritionalValue;

            grassCanBeEatenReset = GrassDefaults.canBeEaten;

            grassSizeReset = GrassDefaults.scale;
        }

        public void ResetButton(string menuToReset)
        {
            switch (menuToReset)
            {
                case "Brightness":
                    brightnessEffect.brightness = defaultBrightness;
                    brightnessSlider.value = defaultBrightness;
                    brightnessText.text = defaultBrightness.ToString("0.0");
                    BrightnessApply();
                    break;
                case "Audio":
                    AudioListener.volume = defaultVolume;
                    volumeSlider.value = defaultVolume;
                    volumeText.text = defaultVolume.ToString("0.0");
                    VolumeApply();
                    break;
                case "Graphics":
                    controllerSenText.text = defaultSen.ToString("0");
                    controllerSenSlider.value = defaultSen;
                    controlSenFloat = defaultSen;

                    invertYToggle.isOn = false;
                    GameplayApply();
                    break;
                case "InitialProperties":
                    rabbitAgeMaxText.text = rabbitAgeMaxReset.ToString();
                    rabbitAgeMaxSlider.value = rabbitAgeMaxReset;

                    rabbitNutritionalValueText.text = rabbitNutritionalValueReset.ToString();
                    rabbitNutritionalValueSlider.value = rabbitNutritionalValueReset;

                    rabbitCanBeEaten.isOn = rabbitCanBeEatenReset;

                    rabbitHungerMaxText.text = rabbitHungerMaxReset.ToString();
                    rabbitHungerMaxSlider.value = rabbitHungerMaxReset;

                    rabbitHungerThresholdText.text = rabbitHungerThresholdReset.ToString();
                    rabbitHungerThresholdSlider.value = rabbitHungerThresholdReset;

                    rabbitHungerIncreaseBaseText.text = rabbitHungerIncreaseBaseReset.ToString();
                    rabbitHungerIncreaseBaseSlider.value = rabbitHungerIncreaseBaseReset;

                    rabbitHungerIncreaseYoungText.text = rabbitHungerIncreaseYoungReset.ToString();
                    rabbitHungerIncreaseYoungSlider.value = rabbitHungerIncreaseYoungReset;

                    rabbitHungerIncreaseAdultText.text = rabbitHungerIncreaseAdultReset.ToString();
                    rabbitHungerIncreaseAdultSlider.value = rabbitHungerIncreaseAdultReset;

                    rabbitHungerIncreaseOldText.text = rabbitHungerIncreaseOldReset.ToString();
                    rabbitHungerIncreaseOldSlider.value = rabbitHungerIncreaseOldReset;

                    rabbitEatingSpeedText.text = rabbitEatingSpeedReset.ToString();
                    rabbitEatingSpeedSlider.value = rabbitEatingSpeedReset;

                    rabbitThirstMaxText.text = rabbitThirstMaxReset.ToString();
                    rabbitThirstMaxSlider.value = rabbitThirstMaxReset;

                    rabbitThirstThresholdText.text = rabbitThirstThresholdReset.ToString();
                    rabbitThirstThresholdSlider.value = rabbitThirstThresholdReset;

                    rabbitThirstIncreaseBaseText.text = rabbitThirstIncreaseBaseReset.ToString();
                    rabbitThirstIncreaseBaseSlider.value = rabbitThirstIncreaseBaseReset;

                    //rabbitThirstIncreaseYoungText.text = rabbiReset.ToString(); 
                    //rabbitThirstIncreaseYoungSlider.value = rabbit ;

                    //rabbitThirstIncreaseAdultText.text =  rabbiReset.ToString(); 
                    //rabbitThirstIncreaseAdultSlider.value = rabbit ;

                    //rabbitThirstIncreaseOldText.text = rabbiReset.ToString(); 
                    //rabbitThirstIncreaseOldSlider.value = rabbit ;

                    rabbitDrinkingSpeedText.text = rabbitDrinkingSpeedReset.ToString();
                    rabbitDrinkingSpeedSlider.value = rabbitDrinkingSpeedReset;

                    rabbitMatingDurationText.text = rabbitMatingDurationReset.ToString();
                    rabbitMatingDurationSlider.value = rabbitMatingDurationReset;

                    rabbitPregnancyLengthText.text = rabbitPregnancyLengthReset.ToString();
                    rabbitPregnancyLengthSlider.value = rabbitPregnancyLengthReset;

                    rabbitBirthDurationText.text = rabbitBirthDurationReset.ToString();
                    rabbitBirthDurationSlider.value = rabbitBirthDurationReset;

                    rabbitLitterSizeMinText.text = rabbitLitterSizeMinReset.ToString();
                    rabbitLitterSizeMinSlider.value = rabbitLitterSizeMinReset;

                    rabbitLitterSizeMaxText.text = rabbitLitterSizeMaxReset.ToString();
                    rabbitLitterSizeMaxSlider.value = rabbitLitterSizeMaxReset;

                    rabbitLitterSizeAveText.text = rabbitLitterSizeAveReset.ToString();
                    rabbitLitterSizeAveSlider.value = rabbitLitterSizeAveReset;

                    rabbitMovementSpeedText.text = rabbitMovementSpeedReset.ToString();
                    rabbitMovementSpeedSlider.value = rabbitMovementSpeedReset;

                    rabbitMovementMultiplierBaseText.text = rabbitMovementMultiplierBaseReset.ToString();
                    rabbitMovementMultiplierBaseSlider.value = rabbitMovementMultiplierBaseReset;

                    rabbitMovementMultiplierYoungText.text = rabbitMovementMultiplierYoungReset.ToString();
                    rabbitMovementMultiplierYoungSlider.value = rabbitMovementMultiplierYoungReset;

                    rabbitMovementMultiplierAdultText.text = rabbitMovementMultiplierAdultReset.ToString();
                    rabbitMovementMultiplierAdultSlider.value = rabbitMovementMultiplierAdultReset;

                    rabbitMovementMultiplierOldText.text = rabbitMovementMultiplierOldReset.ToString();
                    rabbitMovementMultiplierOldSlider.value = rabbitMovementMultiplierOldReset;

                    rabbitMovementMultiplierPregnantText.text = rabbitMovementMultiplierPregnantReset.ToString();
                    rabbitMovementMultiplierPregnantSlider.value = rabbitMovementMultiplierPregnantReset;

                    rabbitSightRadiusText.text = rabbitSightRadiusReset.ToString();
                    rabbitSightRadiusSlider.value = rabbitSightRadiusReset;

                    rabbitSizeMaleText.text = rabbitSizeMaleReset.ToString();
                    rabbitSizeMaleSlider.value = rabbitSizeMaleReset;

                    rabbitSizeFemaleText.text = rabbitSizeFemaleReset.ToString();
                    rabbitSizeFemaleSlider.value = rabbitSizeFemaleReset;



                    foxAgeMaxText.text = foxAgeMaxReset.ToString();
                    foxAgeMaxSlider.value = foxAgeMaxReset;

                    foxNutritionalValueText.text = foxNutritionalValueReset.ToString();
                    foxNutritionalValueSlider.value = foxNutritionalValueReset;

                    foxCanBeEaten.isOn = foxCanBeEatenReset;

                    foxHungerMaxText.text = foxHungerMaxReset.ToString();
                    foxHungerMaxSlider.value = foxHungerMaxReset;

                    foxHungerThresholdText.text = foxHungerThresholdReset.ToString();
                    foxHungerThresholdSlider.value = foxHungerThresholdReset;

                    foxHungerIncreaseBaseText.text = foxHungerIncreaseBaseReset.ToString();
                    foxHungerIncreaseBaseSlider.value = foxHungerIncreaseBaseReset;

                    foxHungerIncreaseYoungText.text = foxHungerIncreaseYoungReset.ToString();
                    foxHungerIncreaseYoungSlider.value = foxHungerIncreaseYoungReset;

                    foxHungerIncreaseAdultText.text = foxHungerIncreaseAdultReset.ToString();
                    foxHungerIncreaseAdultSlider.value = foxHungerIncreaseAdultReset;

                    foxHungerIncreaseOldText.text = foxHungerIncreaseOldReset.ToString();
                    foxHungerIncreaseOldSlider.value = foxHungerIncreaseOldReset;

                    foxEatingSpeedText.text = foxEatingSpeedReset.ToString();
                    foxEatingSpeedSlider.value = foxEatingSpeedReset;

                    foxThirstMaxText.text = foxThirstMaxReset.ToString();
                    foxThirstMaxSlider.value = foxThirstMaxReset;

                    foxThirstThresholdText.text = foxThirstThresholdReset.ToString();
                    foxThirstThresholdSlider.value = foxThirstThresholdReset;

                    foxThirstIncreaseBaseText.text = foxThirstIncreaseBaseReset.ToString();
                    foxThirstIncreaseBaseSlider.value = foxThirstIncreaseBaseReset;

                    //foxThirstIncreaseYoungText.text = foxEset.ToString(); 
                    //foxThirstIncreaseYoungSlider.value = fox ;

                    //foxThirstIncreaseAdultText.text = foxEset.ToString(); 
                    //foxThirstIncreaseAdultSlider.value = fox ;

                    //foxThirstIncreaseOldText.text = foxReset.ToString(); 
                    //foxThirstIncreaseOldSlider.value = fox ;

                    foxDrinkingSpeedText.text = foxDrinkingSpeedReset.ToString();
                    foxDrinkingSpeedSlider.value = foxDrinkingSpeedReset;

                    foxMatingDurationText.text = foxMatingDurationReset.ToString();
                    foxMatingDurationSlider.value = foxMatingDurationReset;

                    foxPregnancyLengthText.text = foxPregnancyLengthReset.ToString();
                    foxPregnancyLengthSlider.value = foxPregnancyLengthReset;

                    foxBirthDurationText.text = foxBirthDurationReset.ToString();
                    foxBirthDurationSlider.value = foxBirthDurationReset;

                    foxLitterSizeMinText.text = foxLitterSizeMinReset.ToString();
                    foxLitterSizeMinSlider.value = foxLitterSizeMinReset;

                    foxLitterSizeMaxText.text = foxLitterSizeMaxReset.ToString();
                    foxLitterSizeMaxSlider.value = foxLitterSizeMaxReset;

                    foxLitterSizeAveText.text = foxLitterSizeAveReset.ToString();
                    foxLitterSizeAveSlider.value = foxLitterSizeAveReset;

                    foxMovementSpeedText.text = foxMovementSpeedReset.ToString();
                    foxMovementSpeedSlider.value = foxMovementSpeedReset;

                    foxMovementMultiplierBaseText.text = foxMovementMultiplierBaseReset.ToString();
                    foxMovementMultiplierBaseSlider.value = foxMovementMultiplierBaseReset;

                    foxMovementMultiplierYoungText.text = foxMovementMultiplierYoungReset.ToString();
                    foxMovementMultiplierYoungSlider.value = foxMovementMultiplierYoungReset;

                    foxMovementMultiplierAdultText.text = foxMovementMultiplierAdultReset.ToString();
                    foxMovementMultiplierAdultSlider.value = foxMovementMultiplierAdultReset;

                    foxMovementMultiplierOldText.text = foxMovementMultiplierOldReset.ToString();
                    foxMovementMultiplierOldSlider.value = foxMovementMultiplierOldReset;

                    foxMovementMultiplierPregnantText.text = foxMovementMultiplierPregnantReset.ToString();
                    foxMovementMultiplierPregnantSlider.value = foxMovementMultiplierPregnantReset;

                    foxSightRadiusText.text = foxSightRadiusReset.ToString();
                    foxSightRadiusSlider.value = foxSightRadiusReset;

                    foxSizeMaleText.text = foxSizeMaleReset.ToString();
                    foxSizeMaleSlider.value = foxSizeMaleReset;

                    foxSizeFemaleText.text = foxSizeFemaleReset.ToString();
                    foxSizeFemaleSlider.value = foxSizeFemaleReset;



                    grassNutritionalValueText.text = grassNutritionalValueReset.ToString();
                    grassNutritionalValueSlider.value = grassNutritionalValueReset;

                    grassCanBeEaten.isOn = grassCanBeEatenReset;

                    grassSizeText.text = grassSizeReset.ToString();
                    grassSizeSlider.value = grassSizeReset;

                    InitialPropertiesApply();
                    break;
                default:
                    Debug.Log("menu to reset doesn't exist in switch statement.");
                    break;
            }
        }
        #endregion

        #region Dialog Options - This is where we load what has been saved in player prefs!
        public void ClickNewGameDialog(string ButtonType)
        {
            if (ButtonType == "Yes")
            {
                SceneManager.LoadScene(_newGameButtonLevel);
            }

            if (ButtonType == "No")
            {
                GoBackToMainMenu();
            }
        }

        public void ClickLoadGameDialog(string ButtonType)
        {
            if (ButtonType == "Yes")
            {
                //if the file is valid open the initial properties menu
                if (MapFileValid())
                {
                    SimulationManager.mapString = fileContents; //set the map string in Simualtion to filecontents
                    SimulationManager.mapPath = null; // set map path to null as not using it
                    Debug.Log("I WANT TO LOAD THE MAP");
                    loadGameDialog.SetActive(false);
                    initialPropertiesCanvas.SetActive(true);
                    ResetButton("InitialProperties");
                    menuNumber = MenuNumber.InitialProperties;
                }
                else
                {
                    Debug.Log("Load Game Dialog");
                    menuDefaultCanvas.SetActive(false);
                    loadGameDialog.SetActive(false);
                    noSaveDialog.SetActive(true);
                }
            }

            if (ButtonType == "No")
            {
                GoBackToMainMenu();
            }
        }
        #endregion

        #region Back to Menus
        public void GoBackToOptionsMenu()
        {
            GeneralSettingsCanvas.SetActive(true);
            graphicsMenu.SetActive(false);
            soundMenu.SetActive(false);
            gameplayMenu.SetActive(false);

            GameplayApply();
            BrightnessApply();
            VolumeApply();

            menuNumber = MenuNumber.Options;
        }

        public void GoBackToMainMenu()
        {
            menuDefaultCanvas.SetActive(true);
            newGameDialog.SetActive(false);
            loadGameDialog.SetActive(false);
            noSaveDialog.SetActive(false);
            GeneralSettingsCanvas.SetActive(false);
            graphicsMenu.SetActive(false);
            soundMenu.SetActive(false);
            gameplayMenu.SetActive(false);
            initialPropertiesCanvas.SetActive(false);
            generateMapCanvas.SetActive(false);
            menuNumber = MenuNumber.Main;
        }

        public void GoBackToGameplayMenu()
        {
            controlsMenu.SetActive(false);
            gameplayMenu.SetActive(true);
            menuNumber = MenuNumber.Gameplay;
        }

        public void ClickQuitOptions()
        {
            GoBackToMainMenu();
        }

        public void ClickNoSaveDialog()
        {
            GoBackToMainMenu();
        }
        #endregion
    }
}
