using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using System.IO;
using System.Xml;
using System;

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
        [SerializeField] private bool rabbitCanBeEaten;

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
        [SerializeField] private bool foxCanBeEaten;

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
        [SerializeField] private bool grassCanBeEaten;

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
                    RabbitDefaults.ageMax = rabbitAgeMaxSlider.value;
                    break;
                case "RabbitNutritionalValue":
                    rabbitNutritionalValueText.text = rabbitNutritionalValueSlider.value.ToString();
                    RabbitDefaults.nutritionalValue = rabbitNutritionalValueSlider.value;
                    break;
                case "RabbitCanBeEaten":
                    //rabbitCanBeEaten;
                    //RabbitDefaults.canBeEaten = rabbitCanBeEaten.value;
                    break;
                case "RabbitHungerMax":
                    rabbitHungerMaxText.text = rabbitHungerMaxSlider.value.ToString();
                    RabbitDefaults.hungerMax = rabbitHungerMaxSlider.value;
                    break;
                case "RabbitHungerThreshold":
                    rabbitHungerThresholdText.text = rabbitHungerThresholdSlider.value.ToString();
                    RabbitDefaults.hungryThreshold = rabbitHungerThresholdSlider.value;
                    break;
                case "RabbitHungerIncreaseBase":
                    rabbitHungerIncreaseBaseText.text = rabbitHungerIncreaseBaseSlider.value.ToString();
                    RabbitDefaults.hungerIncrease = rabbitHungerIncreaseBaseSlider.value;
                    break;
                case "RabbitHungerIncreaseYoung":
                    rabbitHungerIncreaseYoungText.text = rabbitHungerIncreaseYoungSlider.value.ToString();
                    RabbitDefaults.youngHungerIncrease = rabbitHungerIncreaseYoungSlider.value;
                    break;
                case "RabbitHungerIncreaseAdult":
                    rabbitHungerIncreaseAdultText.text = rabbitHungerIncreaseAdultSlider.value.ToString();
                    RabbitDefaults.adultHungerIncrease = rabbitHungerIncreaseAdultSlider.value;
                    break;
                case "RabbitHungerIncreaseOld":
                    rabbitHungerIncreaseOldText.text = rabbitHungerIncreaseOldSlider.value.ToString();
                    RabbitDefaults.oldHungerIncrease = rabbitHungerIncreaseOldSlider.value;
                    break;
                case "RabbitEatingSpeed":
                    rabbitEatingSpeedText.text = rabbitEatingSpeedSlider.value.ToString();
                    RabbitDefaults.eatingSpeed = rabbitEatingSpeedSlider.value;
                    break;
                case "RabbitThirstMax":
                    rabbitThirstMaxText.text = rabbitThirstMaxSlider.value.ToString();
                    RabbitDefaults.thirstMax = rabbitThirstMaxSlider.value;
                    break;
                case "RabbitThirstThreshold":
                    rabbitThirstThresholdText.text = rabbitThirstThresholdSlider.value.ToString();
                    RabbitDefaults.thirstyThreshold = rabbitThirstThresholdSlider.value;
                    break;
                case "RabbitThirstIncreaseBase":
                    rabbitThirstIncreaseBaseText.text = rabbitThirstIncreaseBaseSlider.value.ToString();
                    RabbitDefaults.thirstIncrease = rabbitThirstIncreaseBaseSlider.value;
                    break;
                case "RabbitThirstIncreaseYoung":
                    rabbitThirstIncreaseYoungText.text = rabbitThirstIncreaseYoungSlider.value.ToString();
                    //RabbitDefaults. = rabbitThirstIncreaseYoungSlider.value;
                    break;
                case "RabbitThirstIncreaseAdult":
                    rabbitThirstIncreaseAdultText.text = rabbitThirstIncreaseAdultSlider.value.ToString();
                    //RabbitDefaults. = rabbitThirstIncreaseAdultSlider.value;
                    break;
                case "RabbitThirstIncreaseOld":
                    rabbitThirstIncreaseOldText.text = rabbitThirstIncreaseOldSlider.value.ToString();
                    //RabbitDefaults. = rabbitThirstIncreaseOldSlider.value;
                    break;
                case "RabbitDrinkingSpeed":
                    rabbitDrinkingSpeedText.text = rabbitDrinkingSpeedSlider.value.ToString();
                    RabbitDefaults.drinkingSpeed = rabbitDrinkingSpeedSlider.value;
                    break;
                case "RabbitMatingDuration":
                    rabbitMatingDurationText.text = rabbitMatingDurationSlider.value.ToString();
                    RabbitDefaults.matingDuration = rabbitMatingDurationSlider.value;
                    break;
                case "RabbitPregnancyLength":
                    rabbitPregnancyLengthText.text = rabbitPregnancyLengthSlider.value.ToString();
                    RabbitDefaults.pregnancyLength = rabbitPregnancyLengthSlider.value;
                    break;
                case "RabbitBirthDuration":
                    rabbitBirthDurationText.text = rabbitBirthDurationSlider.value.ToString();
                    RabbitDefaults.birthDuration = rabbitBirthDurationSlider.value;
                    break;
                case "RabbitLitterSizeMin":
                    rabbitLitterSizeMinText.text = rabbitLitterSizeMinSlider.value.ToString();
                    RabbitDefaults.litterSizeMin = (int)rabbitLitterSizeMinSlider.value;
                    break;
                case "RabbitLitterSizeMax":
                    rabbitLitterSizeMaxText.text = rabbitLitterSizeMaxSlider.value.ToString();
                    RabbitDefaults.litterSizeMax = (int)rabbitLitterSizeMaxSlider.value;
                    break;
                case "RabbitLitterSizeAve":
                    rabbitLitterSizeAveText.text = rabbitLitterSizeAveSlider.value.ToString();
                    RabbitDefaults.litterSizeAve = (int)rabbitLitterSizeAveSlider.value;
                    break;
                case "RabbitMovementSpeed":
                    rabbitMovementSpeedText.text = rabbitMovementSpeedSlider.value.ToString();
                    RabbitDefaults.moveSpeed = rabbitMovementSpeedSlider.value;
                    break;
                case "RabbitMovementMultiplierBase":
                    rabbitMovementMultiplierBaseText.text = rabbitMovementMultiplierBaseSlider.value.ToString();
                    RabbitDefaults.originalMoveMultiplier = rabbitMovementMultiplierBaseSlider.value;
                    break;
                case "RabbitMovementMultiplierYoung":
                    rabbitMovementMultiplierYoungText.text = rabbitMovementMultiplierYoungSlider.value.ToString();
                    RabbitDefaults.youngMoveMultiplier = rabbitMovementMultiplierYoungSlider.value;
                    break;
                case "RabbitMovementMultiplierAdult":
                    rabbitMovementMultiplierAdultText.text = rabbitMovementMultiplierAdultSlider.value.ToString();
                    RabbitDefaults.adultMoveMultiplier = rabbitMovementMultiplierAdultSlider.value;
                    break;
                case "RabbitMovementMultiplierOld":
                    rabbitMovementMultiplierOldText.text = rabbitMovementMultiplierOldSlider.value.ToString();
                    RabbitDefaults.oldMoveMultiplier = rabbitMovementMultiplierOldSlider.value;
                    break;
                case "RabbitMovementMultiplierPregnant":
                    rabbitMovementMultiplierPregnantText.text = rabbitMovementMultiplierPregnantSlider.value.ToString();
                    RabbitDefaults.pregnancyMoveMultiplier = rabbitMovementMultiplierPregnantSlider.value;
                    break;
                case "RabbitSightRadius":
                    rabbitSightRadiusText.text = rabbitSightRadiusSlider.value.ToString();
                    RabbitDefaults.sightRadius = rabbitSightRadiusSlider.value;
                    break;
                case "RabbitSizeMale":
                    rabbitSizeMaleText.text = rabbitSizeMaleSlider.value.ToString();
                    RabbitDefaults.scaleMale = rabbitSizeMaleSlider.value;
                    break;
                case "RabbitSizeFemale":
                    rabbitSizeFemaleText.text = rabbitSizeFemaleSlider.value.ToString();
                    RabbitDefaults.scaleFemale = rabbitSizeFemaleSlider.value;
                    break;
                case "FoxAgeMax":
                    foxAgeMaxText.text = foxAgeMaxSlider.value.ToString();
                    FoxDefaults.ageMax = foxAgeMaxSlider.value;
                    break;
                case "FoxNutritionalValue":
                    foxNutritionalValueText.text = foxNutritionalValueSlider.value.ToString();
                    FoxDefaults.nutritionalValue = foxNutritionalValueSlider.value;
                    break;
                case "FoxCanBeEaten":
                    //foxCanBeEaten;
                    //FoxDefaults.canBeEaten = foxCanBeEaten.value;
                    break;
                case "FoxHungerMax":
                    foxHungerMaxText.text = foxHungerMaxSlider.value.ToString();
                    FoxDefaults.hungerMax = foxHungerMaxSlider.value;
                    break;
                case "FoxHungerThreshold":
                    foxHungerThresholdText.text = foxHungerThresholdSlider.value.ToString();
                    FoxDefaults.hungryThreshold = foxHungerThresholdSlider.value;
                    break;
                case "FoxHungerIncreaseBase":
                    foxHungerIncreaseBaseText.text = foxHungerIncreaseBaseSlider.value.ToString();
                    FoxDefaults.hungerIncrease = foxHungerIncreaseBaseSlider.value;
                    break;
                case "FoxHungerIncreaseYoung":
                    foxHungerIncreaseYoungText.text = foxHungerIncreaseYoungSlider.value.ToString();
                    FoxDefaults.youngHungerIncrease = foxHungerIncreaseYoungSlider.value;
                    break;
                case "FoxHungerIncreaseAdult":
                    foxHungerIncreaseAdultText.text = foxHungerIncreaseAdultSlider.value.ToString();
                    FoxDefaults.adultHungerIncrease = foxHungerIncreaseAdultSlider.value;
                    break;
                case "FoxHungerIncreaseOld":
                    foxHungerIncreaseOldText.text = foxHungerIncreaseOldSlider.value.ToString();
                    FoxDefaults.oldHungerIncrease = foxHungerIncreaseOldSlider.value;
                    break;
                case "FoxEatingSpeed":
                    foxEatingSpeedText.text = foxEatingSpeedSlider.value.ToString();
                    FoxDefaults.eatingSpeed = foxEatingSpeedSlider.value;
                    break;
                case "FoxThirstMax":
                    foxThirstMaxText.text = foxThirstMaxSlider.value.ToString();
                    FoxDefaults.thirstMax = foxThirstMaxSlider.value;
                    break;
                case "FoxThirstThreshold":
                    foxThirstThresholdText.text = foxThirstThresholdSlider.value.ToString();
                    FoxDefaults.thirstyThreshold = foxThirstThresholdSlider.value;
                    break;
                case "FoxThirstIncreaseBase":
                    foxThirstIncreaseBaseText.text = foxThirstIncreaseBaseSlider.value.ToString();
                    FoxDefaults.thirstIncrease = foxThirstIncreaseBaseSlider.value;
                    break;
                case "FoxThirstIncreaseYoung":
                    foxThirstIncreaseYoungText.text = foxThirstIncreaseYoungSlider.value.ToString();
                    //FoxDefaults. = foxThirstIncreaseYoungSlider.value;
                    break;
                case "FoxThirstIncreaseAdult":
                    foxThirstIncreaseAdultText.text = foxThirstIncreaseAdultSlider.value.ToString();
                    //FoxDefaults. = foxThirstIncreaseAdultSlider.value;
                    break;
                case "FoxThirstIncreaseOld":
                    foxThirstIncreaseOldText.text = foxThirstIncreaseOldSlider.value.ToString();
                    //FoxDefaults. = foxThirstIncreaseOldSlider.value;
                    break;
                case "FoxDrinkingSpeed":
                    foxDrinkingSpeedText.text = foxDrinkingSpeedSlider.value.ToString();
                    FoxDefaults.drinkingSpeed = foxDrinkingSpeedSlider.value;
                    break;
                case "FoxMatingDuration":
                    foxMatingDurationText.text = foxMatingDurationSlider.value.ToString();
                    FoxDefaults.matingDuration = foxMatingDurationSlider.value;
                    break;
                case "FoxPregnancyLength":
                    foxPregnancyLengthText.text = foxPregnancyLengthSlider.value.ToString();
                    FoxDefaults.pregnancyLength = foxPregnancyLengthSlider.value;
                    break;
                case "FoxBirthDuration":
                    foxBirthDurationText.text = foxBirthDurationSlider.value.ToString();
                    FoxDefaults.birthDuration = foxBirthDurationSlider.value;
                    break;
                case "FoxLitterSizeMin":
                    foxLitterSizeMinText.text = foxLitterSizeMinSlider.value.ToString();
                    FoxDefaults.litterSizeMin = (int)foxLitterSizeMinSlider.value;
                    break;
                case "FoxLitterSizeMax":
                    foxLitterSizeMaxText.text = foxLitterSizeMaxSlider.value.ToString();
                    FoxDefaults.litterSizeMax = (int)foxLitterSizeMaxSlider.value;
                    break;
                case "FoxLitterSizeAve":
                    foxLitterSizeAveText.text = foxLitterSizeAveSlider.value.ToString();
                    FoxDefaults.litterSizeAve = (int)foxLitterSizeAveSlider.value;
                    break;
                case "FoxMovementSpeed":
                    foxMovementSpeedText.text = foxMovementSpeedSlider.value.ToString();
                    FoxDefaults.moveSpeed = foxMovementSpeedSlider.value;
                    break;
                case "FoxMovementMultiplierBase":
                    foxMovementMultiplierBaseText.text = foxMovementMultiplierBaseSlider.value.ToString();
                    FoxDefaults.originalMoveMultiplier = foxMovementMultiplierBaseSlider.value;
                    break;
                case "FoxMovementMultiplierYoung":
                    foxMovementMultiplierYoungText.text = foxMovementMultiplierYoungSlider.value.ToString();
                    FoxDefaults.youngMoveMultiplier = foxMovementMultiplierYoungSlider.value;
                    break;
                case "FoxMovementMultiplierAdult":
                    foxMovementMultiplierAdultText.text = foxMovementMultiplierAdultSlider.value.ToString();
                    FoxDefaults.adultMoveMultiplier = foxMovementMultiplierAdultSlider.value;
                    break;
                case "FoxMovementMultiplierOld":
                    foxMovementMultiplierOldText.text = foxMovementMultiplierOldSlider.value.ToString();
                    FoxDefaults.oldMoveMultiplier = foxMovementMultiplierOldSlider.value;
                    break;
                case "FoxMovementMultiplierPregnant":
                    foxMovementMultiplierPregnantText.text = foxMovementMultiplierPregnantSlider.value.ToString();
                    FoxDefaults.pregnancyMoveMultiplier = foxMovementMultiplierPregnantSlider.value;
                    break;
                case "FoxSightRadius":
                    foxSightRadiusText.text = foxSightRadiusSlider.value.ToString();
                    FoxDefaults.sightRadius = foxSightRadiusSlider.value;
                    break;
                case "FoxSizeMale":
                    foxSizeMaleText.text = foxSizeMaleSlider.value.ToString();
                    FoxDefaults.scaleMale = foxSizeMaleSlider.value;
                    break;
                case "FoxSizeFemale":
                    foxSizeFemaleText.text = foxSizeFemaleSlider.value.ToString();
                    FoxDefaults.scaleFemale = foxSizeFemaleSlider.value;
                    break;
                case "GrassNutritionalValue":
                    grassNutritionalValueText.text = grassNutritionalValueSlider.value.ToString();
                    GrassDefaults.nutritionalValue = grassNutritionalValueSlider.value;
                    break;
                case "GrassCanBeEaten":
                    //grassCanBeEaten;
                    //GrassDefaults.canBeEaten = grassCanBeEaten.value;
                    break;
                case "GrassSize":
                    grassSizeText.text = grassSizeSlider.value.ToString();
                    GrassDefaults.scale = grassSizeSlider.value;
                    break;
                default:
                    Debug.LogWarning("Attempted to update unknown property in switch: " + propertyToUpdate, this);
                    break;
            }
        }
        #endregion

        #region ResetButton
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
                    rabbitAgeMaxText.text = RabbitDefaults.ageMax.ToString();
                    rabbitAgeMaxSlider.value = RabbitDefaults.ageMax;

                    rabbitNutritionalValueText.text = RabbitDefaults.nutritionalValue.ToString();
                    rabbitNutritionalValueSlider.value = RabbitDefaults.nutritionalValue;

                    //rabbitCanBeEaten=                     RabbitDefaults.canBeEaten.ToString();
                    //rabbitCanBeEaten.value = RabbitDefaults .canBeEaten;

                    rabbitHungerMaxText.text = RabbitDefaults.hungerMax.ToString();
                    rabbitHungerMaxSlider.value = RabbitDefaults.hungerMax;

                    rabbitHungerThresholdText.text = RabbitDefaults.hungryThreshold.ToString();
                    rabbitHungerThresholdSlider.value = RabbitDefaults.hungryThreshold;

                    rabbitHungerIncreaseBaseText.text = RabbitDefaults.hungerIncrease.ToString();
                    rabbitHungerIncreaseBaseSlider.value = RabbitDefaults.hungerIncrease;

                    rabbitHungerIncreaseYoungText.text = RabbitDefaults.youngHungerIncrease.ToString();
                    rabbitHungerIncreaseYoungSlider.value = RabbitDefaults.youngHungerIncrease;

                    rabbitHungerIncreaseAdultText.text = RabbitDefaults.adultHungerIncrease.ToString();
                    rabbitHungerIncreaseAdultSlider.value = RabbitDefaults.adultHungerIncrease;

                    rabbitHungerIncreaseOldText.text = RabbitDefaults.oldHungerIncrease.ToString();
                    rabbitHungerIncreaseOldSlider.value = RabbitDefaults.oldHungerIncrease;

                    rabbitEatingSpeedText.text = RabbitDefaults.eatingSpeed.ToString();
                    rabbitEatingSpeedSlider.value = RabbitDefaults.eatingSpeed;

                    rabbitThirstMaxText.text = RabbitDefaults.thirstMax.ToString();
                    rabbitThirstMaxSlider.value = RabbitDefaults.thirstMax;

                    rabbitThirstThresholdText.text = RabbitDefaults.thirstyThreshold.ToString();
                    rabbitThirstThresholdSlider.value = RabbitDefaults.thirstyThreshold;

                    rabbitThirstIncreaseBaseText.text = RabbitDefaults.thirstIncrease.ToString();
                    rabbitThirstIncreaseBaseSlider.value = RabbitDefaults.thirstIncrease;

                    //rabbitThirstIncreaseYoungText.text = RabbitDefaults.ToString();
                    //rabbitThirstIncreaseYoungSlider.value = RabbitDefaults. ;

                    //rabbitThirstIncreaseAdultText.text =  RabbitDefaults.ToString();
                    //rabbitThirstIncreaseAdultSlider.value = RabbitDefaults. ;

                    //rabbitThirstIncreaseOldText.text = RabbitDefaults.ToString();
                    //rabbitThirstIncreaseOldSlider.value = RabbitDefaults. ;

                    rabbitDrinkingSpeedText.text = RabbitDefaults.drinkingSpeed.ToString();
                    rabbitDrinkingSpeedSlider.value = RabbitDefaults.drinkingSpeed;

                    rabbitMatingDurationText.text = RabbitDefaults.matingDuration.ToString();
                    rabbitMatingDurationSlider.value = RabbitDefaults.matingDuration;

                    rabbitPregnancyLengthText.text = RabbitDefaults.pregnancyLength.ToString();
                    rabbitPregnancyLengthSlider.value = RabbitDefaults.pregnancyLength;

                    rabbitBirthDurationText.text = RabbitDefaults.birthDuration.ToString();
                    rabbitBirthDurationSlider.value = RabbitDefaults.birthDuration;

                    rabbitLitterSizeMinText.text = RabbitDefaults.litterSizeMin.ToString();
                    rabbitLitterSizeMinSlider.value = RabbitDefaults.litterSizeMin;

                    rabbitLitterSizeMaxText.text = RabbitDefaults.litterSizeMax.ToString();
                    rabbitLitterSizeMaxSlider.value = RabbitDefaults.litterSizeMax;

                    rabbitLitterSizeAveText.text = RabbitDefaults.litterSizeAve.ToString();
                    rabbitLitterSizeAveSlider.value = RabbitDefaults.litterSizeAve;

                    rabbitMovementSpeedText.text = RabbitDefaults.moveSpeed.ToString();
                    rabbitMovementSpeedSlider.value = RabbitDefaults.moveSpeed;

                    rabbitMovementMultiplierBaseText.text = RabbitDefaults.originalMoveMultiplier.ToString();
                    rabbitMovementMultiplierBaseSlider.value = RabbitDefaults.originalMoveMultiplier;

                    rabbitMovementMultiplierYoungText.text = RabbitDefaults.youngMoveMultiplier.ToString();
                    rabbitMovementMultiplierYoungSlider.value = RabbitDefaults.youngMoveMultiplier;

                    rabbitMovementMultiplierAdultText.text = RabbitDefaults.adultMoveMultiplier.ToString();
                    rabbitMovementMultiplierAdultSlider.value = RabbitDefaults.adultMoveMultiplier;

                    rabbitMovementMultiplierOldText.text = RabbitDefaults.oldMoveMultiplier.ToString();
                    rabbitMovementMultiplierOldSlider.value = RabbitDefaults.oldMoveMultiplier;

                    rabbitMovementMultiplierPregnantText.text = RabbitDefaults.pregnancyMoveMultiplier.ToString();
                    rabbitMovementMultiplierPregnantSlider.value = RabbitDefaults.pregnancyMoveMultiplier;

                    rabbitSightRadiusText.text = RabbitDefaults.sightRadius.ToString();
                    rabbitSightRadiusSlider.value = RabbitDefaults.sightRadius;

                    rabbitSizeMaleText.text = RabbitDefaults.scaleMale.ToString();
                    rabbitSizeMaleSlider.value = RabbitDefaults.scaleMale;

                    rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();
                    rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;



                    foxAgeMaxText.text = FoxDefaults.ageMax.ToString();
                    foxAgeMaxSlider.value = FoxDefaults.ageMax;

                    foxNutritionalValueText.text = FoxDefaults.nutritionalValue.ToString();
                    foxNutritionalValueSlider.value = FoxDefaults.nutritionalValue;

                    //foxCanBeEaten= FoxDefaults.canBeEaten.ToString();
                    //foxCanBeEaten.value = FoxDefaults.canBeEaten;

                    foxHungerMaxText.text = FoxDefaults.hungerMax.ToString();
                    foxHungerMaxSlider.value = FoxDefaults.hungerMax;

                    foxHungerThresholdText.text = FoxDefaults.hungryThreshold.ToString();
                    foxHungerThresholdSlider.value = FoxDefaults.hungryThreshold;

                    foxHungerIncreaseBaseText.text = FoxDefaults.hungerIncrease.ToString();
                    foxHungerIncreaseBaseSlider.value = FoxDefaults.hungerIncrease;

                    foxHungerIncreaseYoungText.text = FoxDefaults.youngHungerIncrease.ToString();
                    foxHungerIncreaseYoungSlider.value = FoxDefaults.youngHungerIncrease;

                    foxHungerIncreaseAdultText.text = FoxDefaults.adultHungerIncrease.ToString();
                    foxHungerIncreaseAdultSlider.value = FoxDefaults.adultHungerIncrease;

                    foxHungerIncreaseOldText.text = FoxDefaults.oldHungerIncrease.ToString();
                    foxHungerIncreaseOldSlider.value = FoxDefaults.oldHungerIncrease;

                    foxEatingSpeedText.text = FoxDefaults.eatingSpeed.ToString();
                    foxEatingSpeedSlider.value = FoxDefaults.eatingSpeed;

                    foxThirstMaxText.text = FoxDefaults.thirstMax.ToString();
                    foxThirstMaxSlider.value = FoxDefaults.thirstMax;

                    foxThirstThresholdText.text = FoxDefaults.thirstyThreshold.ToString();
                    foxThirstThresholdSlider.value = FoxDefaults.thirstyThreshold;

                    foxThirstIncreaseBaseText.text = FoxDefaults.thirstIncrease.ToString();
                    foxThirstIncreaseBaseSlider.value = FoxDefaults.thirstIncrease;

                    //foxThirstIncreaseYoungText.text = FoxDefaults.ToString();
                    //foxThirstIncreaseYoungSlider.value = FoxDefaults. ;

                    //foxThirstIncreaseAdultText.text = FoxDefaults.ToString();
                    //foxThirstIncreaseAdultSlider.value = FoxDefaults. ;

                    //foxThirstIncreaseOldText.text = FoxDefaults..ToString();
                    //foxThirstIncreaseOldSlider.value = FoxDefaults. ;

                    foxDrinkingSpeedText.text = FoxDefaults.drinkingSpeed.ToString();
                    foxDrinkingSpeedSlider.value = FoxDefaults.drinkingSpeed;

                    foxMatingDurationText.text = FoxDefaults.matingDuration.ToString();
                    foxMatingDurationSlider.value = FoxDefaults.matingDuration;

                    foxPregnancyLengthText.text = FoxDefaults.pregnancyLength.ToString();
                    foxPregnancyLengthSlider.value = FoxDefaults.pregnancyLength;

                    foxBirthDurationText.text = FoxDefaults.birthDuration.ToString();
                    foxBirthDurationSlider.value = FoxDefaults.birthDuration;

                    foxLitterSizeMinText.text = FoxDefaults.litterSizeMin.ToString();
                    foxLitterSizeMinSlider.value = FoxDefaults.litterSizeMin;

                    foxLitterSizeMaxText.text = FoxDefaults.litterSizeMax.ToString();
                    foxLitterSizeMaxSlider.value = FoxDefaults.litterSizeMax;

                    foxLitterSizeAveText.text = FoxDefaults.litterSizeAve.ToString();
                    foxLitterSizeAveSlider.value = FoxDefaults.litterSizeAve;

                    foxMovementSpeedText.text = FoxDefaults.moveSpeed.ToString();
                    foxMovementSpeedSlider.value = FoxDefaults.moveSpeed;

                    foxMovementMultiplierBaseText.text = FoxDefaults.originalMoveMultiplier.ToString();
                    foxMovementMultiplierBaseSlider.value = FoxDefaults.originalMoveMultiplier;

                    foxMovementMultiplierYoungText.text = FoxDefaults.youngMoveMultiplier.ToString();
                    foxMovementMultiplierYoungSlider.value = FoxDefaults.youngMoveMultiplier;

                    foxMovementMultiplierAdultText.text = FoxDefaults.adultMoveMultiplier.ToString();
                    foxMovementMultiplierAdultSlider.value = FoxDefaults.adultMoveMultiplier;

                    foxMovementMultiplierOldText.text = FoxDefaults.oldMoveMultiplier.ToString();
                    foxMovementMultiplierOldSlider.value = FoxDefaults.oldMoveMultiplier;

                    foxMovementMultiplierPregnantText.text = FoxDefaults.pregnancyMoveMultiplier.ToString();
                    foxMovementMultiplierPregnantSlider.value = FoxDefaults.pregnancyMoveMultiplier;

                    foxSightRadiusText.text = FoxDefaults.sightRadius.ToString();
                    foxSightRadiusSlider.value = FoxDefaults.sightRadius;

                    foxSizeMaleText.text = FoxDefaults.scaleMale.ToString();
                    foxSizeMaleSlider.value = FoxDefaults.scaleMale;

                    foxSizeFemaleText.text = FoxDefaults.scaleFemale.ToString();
                    foxSizeFemaleSlider.value = FoxDefaults.scaleFemale;




                    grassNutritionalValueText.text = GrassDefaults.nutritionalValue.ToString();
                    grassNutritionalValueSlider.value = GrassDefaults.nutritionalValue;

                    //grassCanBeEaten= GrassDefaults.canBeEaten.ToString();
                    //grassCanBeEaten.value = GrassDefaults .canBeEaten;

                    grassSizeText.text = GrassDefaults.scale.ToString();
                    grassSizeSlider.value = GrassDefaults.scale;

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

        public void LoadGame()
        {
            string[] filePaths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "xml", false);
            string filePath = filePaths[0];
            if (File.Exists(filePath))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);

                #region  open tage by name

                XmlNodeList age = xmlDocument.GetElementsByTagName("age");
                XmlNodeList ageIncrease = xmlDocument.GetElementsByTagName("ageIncrease");
                XmlNodeList ageMax = xmlDocument.GetElementsByTagName("ageMax");
                XmlNodeList ageGroup = xmlDocument.GetElementsByTagName("ageGroup");
                XmlNodeList adultEntryTimer = xmlDocument.GetElementsByTagName("adultEntryTimer");
                XmlNodeList oldEntryTimer = xmlDocument.GetElementsByTagName("oldEntryTimer");
                XmlNodeList nutritionalValue = xmlDocument.GetElementsByTagName("nutritionalValue");
                XmlNodeList canBeEaten = xmlDocument.GetElementsByTagName("canBeEaten");
                XmlNodeList nutritionalValueMultiplier = xmlDocument.GetElementsByTagName("nutritionalValueMultiplier");
                XmlNodeList foodType = xmlDocument.GetElementsByTagName("foodType");
                XmlNodeList hunger = xmlDocument.GetElementsByTagName("hunger");
                XmlNodeList hungerMax = xmlDocument.GetElementsByTagName("hungerMax");
                XmlNodeList hungryThreshold = xmlDocument.GetElementsByTagName("hungryThreshold");
                XmlNodeList hungerIncrease = xmlDocument.GetElementsByTagName("hungerIncrease");
                XmlNodeList pregnancyHungerIncrease = xmlDocument.GetElementsByTagName("pregnancyHungerIncrease");
                XmlNodeList youngHungerIncrease = xmlDocument.GetElementsByTagName("youngHungerIncrease");
                XmlNodeList adultHungerIncrease = xmlDocument.GetElementsByTagName("adultHungerIncrease");
                XmlNodeList oldHungerIncrease = xmlDocument.GetElementsByTagName("oldHungerIncrease");
                XmlNodeList eatingSpeed = xmlDocument.GetElementsByTagName("eatingSpeed");
                XmlNodeList diet = xmlDocument.GetElementsByTagName("diet");
                XmlNodeList thirst = xmlDocument.GetElementsByTagName("thirst");
                XmlNodeList thirstMax = xmlDocument.GetElementsByTagName("thirstMax");
                XmlNodeList thirstyThreshold = xmlDocument.GetElementsByTagName("thirstyThreshold");
                XmlNodeList thirstIncrease = xmlDocument.GetElementsByTagName("thirstIncrease");
                XmlNodeList drinkingSpeed = xmlDocument.GetElementsByTagName("drinkingSpeed");
                XmlNodeList mateStartTime = xmlDocument.GetElementsByTagName("mateStartTime");
                XmlNodeList matingDuration = xmlDocument.GetElementsByTagName("matingDuration");
                XmlNodeList reproductiveUrge = xmlDocument.GetElementsByTagName("reproductiveUrge");
                XmlNodeList reproductiveUrgeIncreaseMale = xmlDocument.GetElementsByTagName("reproductiveUrgeIncreaseMale");
                XmlNodeList reproductiveUrgeIncreaseFemale = xmlDocument.GetElementsByTagName("reproductiveUrgeIncreaseFemale");
                XmlNodeList matingThreshold = xmlDocument.GetElementsByTagName("matingThreshold");
                XmlNodeList pregnancyStartTime = xmlDocument.GetElementsByTagName("pregnancyStartTime");
                XmlNodeList pregnant = xmlDocument.GetElementsByTagName("pregnant");
                XmlNodeList babiesBorn = xmlDocument.GetElementsByTagName("babiesBorn");
                XmlNodeList birthStartTime = xmlDocument.GetElementsByTagName("birthStartTime");
                XmlNodeList currentLitterSize = xmlDocument.GetElementsByTagName("currentLitterSize");
                XmlNodeList pregnancyLengthModifier = xmlDocument.GetElementsByTagName("pregnancyLengthModifier");
                XmlNodeList pregnancyLength = xmlDocument.GetElementsByTagName("pregnancyLength");
                XmlNodeList birthDuration = xmlDocument.GetElementsByTagName("birthDuration");
                XmlNodeList litterSizeMin = xmlDocument.GetElementsByTagName("litterSizeMin");
                XmlNodeList litterSizeMax = xmlDocument.GetElementsByTagName("litterSizeMax");
                XmlNodeList litterSizeAve = xmlDocument.GetElementsByTagName("litterSizeAve");
                XmlNodeList moveSpeed = xmlDocument.GetElementsByTagName("moveSpeed");
                XmlNodeList rotationSpeed = xmlDocument.GetElementsByTagName("rotationSpeed");
                XmlNodeList moveMultiplier = xmlDocument.GetElementsByTagName("moveMultiplier");
                XmlNodeList pregnancyMoveMultiplier = xmlDocument.GetElementsByTagName("pregnancyMoveMultiplier");
                XmlNodeList originalMoveMultiplier = xmlDocument.GetElementsByTagName("originalMoveMultiplier");
                XmlNodeList youngMoveMultiplier = xmlDocument.GetElementsByTagName("youngMoveMultiplier");
                XmlNodeList adultMoveMultiplier = xmlDocument.GetElementsByTagName("adultMoveMultiplier");
                XmlNodeList oldMoveMultiplier = xmlDocument.GetElementsByTagName("oldMoveMultiplier");
                XmlNodeList sizeMultiplier = xmlDocument.GetElementsByTagName("sizeMultiplier");
                XmlNodeList scaleMale = xmlDocument.GetElementsByTagName("scaleMale");
                XmlNodeList scaleFemale = xmlDocument.GetElementsByTagName("scaleFemale");
                XmlNodeList youngSizeMultiplier = xmlDocument.GetElementsByTagName("youngSizeMultiplier");
                XmlNodeList adultSizeMultiplier = xmlDocument.GetElementsByTagName("adultSizeMultiplier");
                XmlNodeList oldSizeMultiplier = xmlDocument.GetElementsByTagName("oldSizeMultiplier");
                XmlNodeList flagState = xmlDocument.GetElementsByTagName("flagState");
                XmlNodeList previousFlagState = xmlDocument.GetElementsByTagName("previousFlagState");
                XmlNodeList deathReason = xmlDocument.GetElementsByTagName("deathReason");
                XmlNodeList beenEaten = xmlDocument.GetElementsByTagName("beenEaten");
                XmlNodeList touchRadius = xmlDocument.GetElementsByTagName("touchRadius");
                XmlNodeList sightRadius = xmlDocument.GetElementsByTagName("sightRadius");
                XmlNodeList shortestToEdibleDistance = xmlDocument.GetElementsByTagName("shortestToEdibleDistance");
                XmlNodeList shortestToWaterDistance = xmlDocument.GetElementsByTagName("shortestToWaterDistance");
                XmlNodeList shortestToPredatorDistance = xmlDocument.GetElementsByTagName("shortestToPredatorDistance");
                XmlNodeList shortestToMateDistance = xmlDocument.GetElementsByTagName("shortestToMateDistance");
                XmlNodeList colliderType = xmlDocument.GetElementsByTagName("colliderType");


                #endregion



                #region rabbit default

                RabbitDefaults.age = float.Parse(age[0].InnerText);
                RabbitDefaults.ageIncrease = float.Parse(ageIncrease[0].InnerText);
                rabbitAgeMaxSlider.value = float.Parse(ageMax[0].InnerText);
                RabbitDefaults.ageGroup = (BioStatsData.AgeGroup)Enum.Parse(typeof(BioStatsData.AgeGroup), ageGroup[0].InnerText);
                RabbitDefaults.adultEntryTimer = float.Parse(adultEntryTimer[0].InnerText);
                RabbitDefaults.oldEntryTimer = float.Parse(oldEntryTimer[0].InnerText);
                rabbitNutritionalValueSlider.value = float.Parse(nutritionalValue[0].InnerText);
                RabbitDefaults.canBeEaten = bool.Parse(canBeEaten[0].InnerText);
                RabbitDefaults.nutritionalValueMultiplier = float.Parse(nutritionalValueMultiplier[0].InnerText);
                RabbitDefaults.foodType = (EdibleData.FoodType)Enum.Parse(typeof(EdibleData.FoodType), foodType[0].InnerText);
                RabbitDefaults.hunger = float.Parse(hunger[0].InnerText);
                rabbitHungerMaxSlider.value = float.Parse(hungerMax[0].InnerText);
                rabbitHungerThresholdSlider.value = float.Parse(hungryThreshold[0].InnerText);
                rabbitHungerIncreaseBaseSlider.value = float.Parse(hungerIncrease[0].InnerText);
                RabbitDefaults.pregnancyHungerIncrease = float.Parse(pregnancyHungerIncrease[0].InnerText);
                rabbitHungerIncreaseYoungSlider.value = float.Parse(youngHungerIncrease[0].InnerText);
                rabbitHungerIncreaseAdultSlider.value = float.Parse(adultHungerIncrease[0].InnerText);
                rabbitHungerIncreaseOldSlider.value = float.Parse(oldHungerIncrease[0].InnerText);
                rabbitEatingSpeedSlider.value = float.Parse(eatingSpeed[0].InnerText);
                RabbitDefaults.diet = (BasicNeedsData.Diet)Enum.Parse(typeof(BasicNeedsData.Diet), diet[0].InnerText);
                RabbitDefaults.thirst = float.Parse(thirst[0].InnerText);
                rabbitThirstMaxSlider.value = float.Parse(thirstMax[0].InnerText);
                rabbitThirstThresholdSlider.value = float.Parse(thirstyThreshold[0].InnerText);
                rabbitThirstIncreaseBaseSlider.value = float.Parse(thirstIncrease[0].InnerText);
                rabbitDrinkingSpeedSlider.value = float.Parse(drinkingSpeed[0].InnerText);
                RabbitDefaults.mateStartTime = float.Parse(mateStartTime[0].InnerText);
                rabbitMatingDurationSlider.value = float.Parse(matingDuration[0].InnerText);
                RabbitDefaults.reproductiveUrge = float.Parse(reproductiveUrge[0].InnerText);
                RabbitDefaults.reproductiveUrgeIncreaseMale = float.Parse(reproductiveUrgeIncreaseMale[0].InnerText);
                RabbitDefaults.reproductiveUrgeIncreaseFemale = float.Parse(reproductiveUrgeIncreaseFemale[0].InnerText);
                RabbitDefaults.matingThreshold = float.Parse(matingThreshold[0].InnerText);
                RabbitDefaults.pregnancyStartTime = float.Parse(pregnancyStartTime[0].InnerText);
                RabbitDefaults.pregnant = bool.Parse(pregnant[0].InnerText);
                RabbitDefaults.babiesBorn = int.Parse(babiesBorn[0].InnerText);
                RabbitDefaults.birthStartTime = float.Parse(birthStartTime[0].InnerText);
                RabbitDefaults.currentLitterSize = int.Parse(currentLitterSize[0].InnerText);
                RabbitDefaults.pregnancyLengthModifier = float.Parse(pregnancyLengthModifier[0].InnerText);
                rabbitPregnancyLengthSlider.value = float.Parse(pregnancyLength[0].InnerText);
                rabbitBirthDurationSlider.value = float.Parse(birthDuration[0].InnerText);
                rabbitLitterSizeMinSlider.value = int.Parse(litterSizeMin[0].InnerText);
                rabbitLitterSizeMaxSlider.value = int.Parse(litterSizeMax[0].InnerText);
                rabbitLitterSizeAveSlider.value = int.Parse(litterSizeAve[0].InnerText);
                rabbitMovementSpeedSlider.value = float.Parse(moveSpeed[0].InnerText);
                RabbitDefaults.rotationSpeed = float.Parse(rotationSpeed[0].InnerText);
                RabbitDefaults.moveMultiplier = float.Parse(moveMultiplier[0].InnerText);
                rabbitMovementMultiplierPregnantSlider.value = float.Parse(pregnancyMoveMultiplier[0].InnerText);
                rabbitMovementMultiplierBaseSlider.value = float.Parse(originalMoveMultiplier[0].InnerText);
                rabbitMovementMultiplierYoungSlider.value = float.Parse(youngMoveMultiplier[0].InnerText);
                rabbitMovementMultiplierAdultSlider.value = float.Parse(adultMoveMultiplier[0].InnerText);
                rabbitMovementMultiplierOldSlider.value = float.Parse(oldMoveMultiplier[0].InnerText);
                RabbitDefaults.sizeMultiplier = float.Parse(sizeMultiplier[0].InnerText);
                rabbitSizeMaleSlider.value = float.Parse(scaleMale[0].InnerText);
                rabbitSizeFemaleSlider.value = float.Parse(scaleFemale[0].InnerText);
                RabbitDefaults.youngSizeMultiplier = float.Parse(youngSizeMultiplier[0].InnerText);
                RabbitDefaults.adultSizeMultiplier = float.Parse(adultSizeMultiplier[0].InnerText);
                RabbitDefaults.oldSizeMultiplier = float.Parse(oldSizeMultiplier[0].InnerText);
                RabbitDefaults.flagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), flagState[0].InnerText);
                RabbitDefaults.previousFlagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), previousFlagState[0].InnerText);
                RabbitDefaults.deathReason = (StateData.DeathReason)Enum.Parse(typeof(StateData.DeathReason), deathReason[0].InnerText);
                RabbitDefaults.beenEaten = bool.Parse(beenEaten[0].InnerText);
                RabbitDefaults.touchRadius = float.Parse(touchRadius[0].InnerText);
                rabbitSightRadiusSlider.value = float.Parse(sightRadius[0].InnerText);
                RabbitDefaults.shortestToEdibleDistance = float.Parse(shortestToEdibleDistance[0].InnerText);
                RabbitDefaults.shortestToWaterDistance = float.Parse(shortestToWaterDistance[0].InnerText);
                RabbitDefaults.shortestToPredatorDistance = float.Parse(shortestToPredatorDistance[0].InnerText);
                RabbitDefaults.shortestToMateDistance = float.Parse(shortestToMateDistance[0].InnerText);
                RabbitDefaults.colliderType = (ColliderTypeData.ColliderType)Enum.Parse(typeof(ColliderTypeData.ColliderType), colliderType[0].InnerText);

                #endregion


                #region fox default

                FoxDefaults.age = float.Parse(age[1].InnerText);
                FoxDefaults.ageIncrease = float.Parse(ageIncrease[1].InnerText);
                foxAgeMaxSlider.value = float.Parse(ageMax[1].InnerText);
                FoxDefaults.ageGroup = (BioStatsData.AgeGroup)Enum.Parse(typeof(BioStatsData.AgeGroup), ageGroup[1].InnerText);
                FoxDefaults.adultEntryTimer = float.Parse(adultEntryTimer[1].InnerText);
                FoxDefaults.oldEntryTimer = float.Parse(oldEntryTimer[1].InnerText);
                foxNutritionalValueSlider.value = float.Parse(nutritionalValue[1].InnerText);
                FoxDefaults.canBeEaten = bool.Parse(canBeEaten[1].InnerText);
                FoxDefaults.nutritionalValueMultiplier = float.Parse(nutritionalValueMultiplier[1].InnerText);
                FoxDefaults.foodType = (EdibleData.FoodType)Enum.Parse(typeof(EdibleData.FoodType), foodType[1].InnerText);
                FoxDefaults.hunger = float.Parse(hunger[1].InnerText);
                foxHungerMaxSlider.value = float.Parse(hungerMax[1].InnerText);
                foxHungerThresholdSlider.value = float.Parse(hungryThreshold[1].InnerText);
                foxHungerIncreaseBaseSlider.value = float.Parse(hungerIncrease[1].InnerText);
                FoxDefaults.pregnancyHungerIncrease = float.Parse(pregnancyHungerIncrease[1].InnerText);
                foxHungerIncreaseYoungSlider.value = float.Parse(youngHungerIncrease[1].InnerText);
                foxHungerIncreaseAdultSlider.value = float.Parse(adultHungerIncrease[1].InnerText);
                foxHungerIncreaseOldSlider.value = float.Parse(oldHungerIncrease[1].InnerText);
                foxEatingSpeedSlider.value = float.Parse(eatingSpeed[1].InnerText);
                FoxDefaults.diet = (BasicNeedsData.Diet)Enum.Parse(typeof(BasicNeedsData.Diet), diet[1].InnerText);
                FoxDefaults.thirst = float.Parse(thirst[1].InnerText);
                foxThirstMaxSlider.value = float.Parse(thirstMax[1].InnerText);
                foxThirstThresholdSlider.value = float.Parse(thirstyThreshold[1].InnerText);
                foxThirstIncreaseBaseSlider.value= float.Parse(thirstIncrease[1].InnerText);
                foxDrinkingSpeedSlider.value = float.Parse(drinkingSpeed[1].InnerText);
                FoxDefaults.mateStartTime = float.Parse(mateStartTime[1].InnerText);
                foxMatingDurationSlider.value = float.Parse(matingDuration[1].InnerText);
                FoxDefaults.reproductiveUrge = float.Parse(reproductiveUrge[1].InnerText);
                FoxDefaults.reproductiveUrgeIncreaseMale = float.Parse(reproductiveUrgeIncreaseMale[1].InnerText);
                FoxDefaults.reproductiveUrgeIncreaseFemale = float.Parse(reproductiveUrgeIncreaseFemale[1].InnerText);
                FoxDefaults.matingThreshold = float.Parse(matingThreshold[1].InnerText);
                FoxDefaults.pregnancyStartTime = float.Parse(pregnancyStartTime[1].InnerText);
                FoxDefaults.pregnant = bool.Parse(pregnant[1].InnerText);
                FoxDefaults.babiesBorn = int.Parse(babiesBorn[1].InnerText);
                FoxDefaults.birthStartTime = float.Parse(birthStartTime[1].InnerText);
                FoxDefaults.currentLitterSize = int.Parse(currentLitterSize[1].InnerText);
                FoxDefaults.pregnancyLengthModifier = float.Parse(pregnancyLengthModifier[1].InnerText);
                foxPregnancyLengthSlider.value = float.Parse(pregnancyLength[1].InnerText);
                foxBirthDurationSlider.value = float.Parse(birthDuration[1].InnerText);
                foxLitterSizeMinSlider.value = int.Parse(litterSizeMin[1].InnerText);
                foxLitterSizeMaxSlider.value = int.Parse(litterSizeMax[1].InnerText);
                foxLitterSizeAveSlider.value = int.Parse(litterSizeAve[1].InnerText);
                foxMovementSpeedSlider.value = float.Parse(moveSpeed[1].InnerText);
                FoxDefaults.rotationSpeed = float.Parse(rotationSpeed[1].InnerText);
                FoxDefaults.moveMultiplier = float.Parse(moveMultiplier[1].InnerText);
                foxMovementMultiplierPregnantSlider.value = float.Parse(pregnancyMoveMultiplier[1].InnerText);
                foxMovementMultiplierBaseSlider.value = float.Parse(originalMoveMultiplier[1].InnerText);
                foxMovementMultiplierYoungSlider.value = float.Parse(youngMoveMultiplier[1].InnerText);
                foxMovementMultiplierAdultSlider.value = float.Parse(adultMoveMultiplier[1].InnerText);
                foxMovementMultiplierOldSlider.value = float.Parse(oldMoveMultiplier[1].InnerText);
                FoxDefaults.sizeMultiplier = float.Parse(sizeMultiplier[1].InnerText);
                foxSizeMaleSlider.value = float.Parse(scaleMale[1].InnerText);
                foxSizeFemaleSlider.value = float.Parse(scaleFemale[1].InnerText);
                FoxDefaults.youngSizeMultiplier = float.Parse(youngSizeMultiplier[1].InnerText);
                FoxDefaults.adultSizeMultiplier = float.Parse(adultSizeMultiplier[1].InnerText);
                FoxDefaults.oldSizeMultiplier = float.Parse(oldSizeMultiplier[1].InnerText);
                FoxDefaults.flagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), flagState[1].InnerText);
                FoxDefaults.previousFlagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), previousFlagState[1].InnerText);
                FoxDefaults.deathReason = (StateData.DeathReason)Enum.Parse(typeof(StateData.DeathReason), deathReason[1].InnerText);
                FoxDefaults.beenEaten = bool.Parse(beenEaten[1].InnerText);
                FoxDefaults.touchRadius = float.Parse(touchRadius[1].InnerText);
                foxSightRadiusSlider.value = float.Parse(sightRadius[1].InnerText);
                FoxDefaults.shortestToEdibleDistance = float.Parse(shortestToEdibleDistance[1].InnerText);
                FoxDefaults.shortestToWaterDistance = float.Parse(shortestToWaterDistance[1].InnerText);
                FoxDefaults.shortestToPredatorDistance = float.Parse(shortestToPredatorDistance[1].InnerText);
                FoxDefaults.shortestToMateDistance = float.Parse(shortestToMateDistance[1].InnerText);
                FoxDefaults.colliderType = (ColliderTypeData.ColliderType)Enum.Parse(typeof(ColliderTypeData.ColliderType), colliderType[1].InnerText);


                #endregion

                #region GrassData

                grassNutritionalValueSlider.value = float.Parse(nutritionalValue[2].InnerText);

                GrassDefaults.nutritionalValueMultiplier = float.Parse(nutritionalValueMultiplier[2].InnerText);

                GrassDefaults.sizeMultiplier = float.Parse(sizeMultiplier[2].InnerText);

                XmlNodeList Scale = xmlDocument.GetElementsByTagName("scale");
                grassSizeSlider.value = float.Parse(Scale[0].InnerText);

                GrassDefaults.foodType = (EdibleData.FoodType)Enum.Parse(typeof(EdibleData.FoodType), foodType[2].InnerText);

                GrassDefaults.flagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), flagState[2].InnerText);

                GrassDefaults.previousFlagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), previousFlagState[2].InnerText);

                GrassDefaults.deathReason = (StateData.DeathReason)Enum.Parse(typeof(StateData.DeathReason), deathReason[2].InnerText);;

                GrassDefaults.GrassColliderType = (ColliderTypeData.ColliderType)Enum.Parse(typeof(ColliderTypeData.ColliderType), colliderType[2].InnerText);

                #endregion GrassData
                print("I am load game haha");
            }
            else
            {
                Debug.Log("NOT FOUNDED");
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
