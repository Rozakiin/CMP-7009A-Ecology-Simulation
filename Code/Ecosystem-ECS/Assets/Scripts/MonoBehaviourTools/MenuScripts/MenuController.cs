using Components;
using EntityDefaults;
using MonoBehaviourTools.Map;
using MonoBehaviourTools.MenuScripts.GUI_Elements.UI_BrightnessShader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MonoBehaviourTools.MenuScripts
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

        string _fileContents;

        #region Default Values
        [Header("Default Menu Values")]
        [SerializeField] private float _defaultBrightness;
        [SerializeField] private float _defaultVolume;



        [Header("Levels To Load")]
        public string NewGameButtonLevel;
        private MenuNumber _menuNumber;
        #endregion
        #region Entity Default Values For Reset
        float _rabbitAgeMaxReset;
        float _rabbitNutritionalValueReset;
        bool _rabbitCanBeEatenReset;
        float _rabbitHungerMaxReset;
        float _rabbitHungerThresholdReset;
        float _rabbitHungerIncreaseBaseReset;
        float _rabbitHungerIncreaseYoungReset;
        float _rabbitHungerIncreaseAdultReset;
        float _rabbitHungerIncreaseOldReset;
        float _rabbitEatingSpeedReset;
        float _rabbitThirstMaxReset;
        float _rabbitThirstThresholdReset;
        float _rabbitThirstIncreaseBaseReset;
        //float rabbitThirstIncreaseYoungReset;
        //float rabbitThirstIncreaseAdultReset;
        //float rabbitThirstIncreaseOldReset;
        float _rabbitDrinkingSpeedReset;
        float _rabbitMatingDurationReset;
        float _rabbitMatingThresholdReset;
        float _rabbitReproductiveUrgeIncreaseReset;
        float _rabbitPregnancyLengthReset;
        float _rabbitBirthDurationReset;
        float _rabbitLitterSizeMinReset;
        float _rabbitLitterSizeMaxReset;
        float _rabbitLitterSizeAveReset;
        float _rabbitMovementSpeedReset;
        float _rabbitMovementMultiplierBaseReset;
        float _rabbitMovementMultiplierYoungReset;
        float _rabbitMovementMultiplierAdultReset;
        float _rabbitMovementMultiplierOldReset;
        float _rabbitMovementMultiplierPregnantReset;
        float _rabbitSightRadiusReset;
        float _rabbitSizeMaleReset;
        float _rabbitSizeFemaleReset;


        float _foxAgeMaxReset;
        float _foxNutritionalValueReset;
        bool _foxCanBeEatenReset;
        float _foxHungerMaxReset;
        float _foxHungerThresholdReset;
        float _foxHungerIncreaseBaseReset;
        float _foxHungerIncreaseYoungReset;
        float _foxHungerIncreaseAdultReset;
        float _foxHungerIncreaseOldReset;
        float _foxEatingSpeedReset;
        float _foxThirstMaxReset;
        float _foxThirstThresholdReset;
        float _foxThirstIncreaseBaseReset;
        //float foxThirstIncreaseYoungReset;
        //float foxThirstIncreaseAdultReset;
        //float foxThirstIncreaseOldReset;
        float _foxDrinkingSpeedReset;
        float _foxMatingDurationReset;
        float _foxMatingThresholdReset;
        float _foxReproductiveUrgeIncreaseReset;
        float _foxPregnancyLengthReset;
        float _foxBirthDurationReset;
        float _foxLitterSizeMinReset;
        float _foxLitterSizeMaxReset;
        float _foxLitterSizeAveReset;
        float _foxMovementSpeedReset;
        float _foxMovementMultiplierBaseReset;
        float _foxMovementMultiplierYoungReset;
        float _foxMovementMultiplierAdultReset;
        float _foxMovementMultiplierOldReset;
        float _foxMovementMultiplierPregnantReset;
        float _foxSightRadiusReset;
        float _foxSizeMaleReset;
        float _foxSizeFemaleReset;


        float _grassNutritionalValueReset;
        bool _grassCanBeEatenReset;
        float _grassSizeReset;


        #endregion


        #region Menu Dialogs
        [Header("Main Menu Components")]
        [SerializeField] private GameObject _menuDefaultCanvas;
        [SerializeField] private GameObject _generalSettingsCanvas;
        [SerializeField] private GameObject _graphicsMenu;
        [SerializeField] private GameObject _soundMenu;
        [SerializeField] private GameObject _gameplayMenu;
        [SerializeField] private GameObject _controlsMenu;
        [SerializeField] private GameObject _confirmationMenu;
        [SerializeField] private GameObject _initialPropertiesCanvas;
        [SerializeField] private GameObject _generateMapCanvas;
        [Space(10)]
        [Header("Menu Popout Dialogs")]
        [SerializeField] private GameObject _noSaveDialog;
        [SerializeField] private GameObject _newGameDialog;
        [SerializeField] private GameObject _loadGameDialog;
        #endregion

        #region Slider Linking
        #region Options
        [Header("Menu Sliders")]
        [SerializeField] private Brightness _brightnessEffect;
        [SerializeField] private Slider _brightnessSlider;
        [SerializeField] private Text _brightnessText;
        [Space(10)]
        [SerializeField] private Text _volumeText;
        [SerializeField] private Slider _volumeSlider;
        #endregion
        #region Initial Properties
        [Header("Initial Numbers InputFields")]
        [SerializeField] private InputField _rabbitNumberInputField;
        [SerializeField] private InputField _foxNumberInputField;
        [SerializeField] private InputField _grassNumberInputField;

        [Header("Initial Properties Sliders")]
        #region Rabbit
        [Header("Rabbit")]
        [Header("Age")]
        [SerializeField] private Text _rabbitAgeMaxText;
        [SerializeField] private Slider _rabbitAgeMaxSlider;

        [Header("Edible")]
        [SerializeField] private Text _rabbitNutritionalValueText;
        [SerializeField] private Slider _rabbitNutritionalValueSlider;
        [Space(5)]
        [SerializeField] private Toggle _rabbitCanBeEaten;

        [Header("Hunger")]
        [SerializeField] private Text _rabbitHungerMaxText;
        [SerializeField] private Slider _rabbitHungerMaxSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitHungerThresholdText;
        [SerializeField] private Slider _rabbitHungerThresholdSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitHungerIncreaseBaseText;
        [SerializeField] private Slider _rabbitHungerIncreaseBaseSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitHungerIncreaseYoungText;
        [SerializeField] private Slider _rabbitHungerIncreaseYoungSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitHungerIncreaseAdultText;
        [SerializeField] private Slider _rabbitHungerIncreaseAdultSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitHungerIncreaseOldText;
        [SerializeField] private Slider _rabbitHungerIncreaseOldSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitEatingSpeedText;
        [SerializeField] private Slider _rabbitEatingSpeedSlider;

        [Header("Thirst")]
        [SerializeField] private Text _rabbitThirstMaxText;
        [SerializeField] private Slider _rabbitThirstMaxSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitThirstThresholdText;
        [SerializeField] private Slider _rabbitThirstThresholdSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitThirstIncreaseBaseText;
        [SerializeField] private Slider _rabbitThirstIncreaseBaseSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitThirstIncreaseYoungText;
        [SerializeField] private Slider _rabbitThirstIncreaseYoungSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitThirstIncreaseAdultText;
        [SerializeField] private Slider _rabbitThirstIncreaseAdultSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitThirstIncreaseOldText;
        [SerializeField] private Slider _rabbitThirstIncreaseOldSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitDrinkingSpeedText;
        [SerializeField] private Slider _rabbitDrinkingSpeedSlider;

        [Header("Mate")]
        [SerializeField] private Text _rabbitMatingDurationText;
        [SerializeField] private Slider _rabbitMatingDurationSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitMatingThresholdText;
        [SerializeField] private Slider _rabbitMatingThresholdSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitReproductiveUrgeIncreaseText;
        [SerializeField] private Slider _rabbitReproductiveUrgeIncreaseSlider;

        [Header("Pregnancy")]
        [SerializeField] private Text _rabbitPregnancyLengthText;
        [SerializeField] private Slider _rabbitPregnancyLengthSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitBirthDurationText;
        [SerializeField] private Slider _rabbitBirthDurationSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitLitterSizeMinText;
        [SerializeField] private Slider _rabbitLitterSizeMinSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitLitterSizeMaxText;
        [SerializeField] private Slider _rabbitLitterSizeMaxSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitLitterSizeAveText;
        [SerializeField] private Slider _rabbitLitterSizeAveSlider;

        [Header("Movement")]
        [SerializeField] private Text _rabbitMovementSpeedText;
        [SerializeField] private Slider _rabbitMovementSpeedSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitMovementMultiplierBaseText;
        [SerializeField] private Slider _rabbitMovementMultiplierBaseSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitMovementMultiplierYoungText;
        [SerializeField] private Slider _rabbitMovementMultiplierYoungSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitMovementMultiplierAdultText;
        [SerializeField] private Slider _rabbitMovementMultiplierAdultSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitMovementMultiplierOldText;
        [SerializeField] private Slider _rabbitMovementMultiplierOldSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitMovementMultiplierPregnantText;
        [SerializeField] private Slider _rabbitMovementMultiplierPregnantSlider;

        [Header("Target")]
        [SerializeField] private Text _rabbitSightRadiusText;
        [SerializeField] private Slider _rabbitSightRadiusSlider;

        [Header("Size")]
        [SerializeField] private Text _rabbitSizeMaleText;
        [SerializeField] private Slider _rabbitSizeMaleSlider;
        [Space(5)]
        [SerializeField] private Text _rabbitSizeFemaleText;
        [SerializeField] private Slider _rabbitSizeFemaleSlider;
        #endregion
        #region Fox
        [Header("Fox")]
        [Header("Age")]
        [SerializeField] private Text _foxAgeMaxText;
        [SerializeField] private Slider _foxAgeMaxSlider;

        [Header("Edible")]
        [SerializeField] private Text _foxNutritionalValueText;
        [SerializeField] private Slider _foxNutritionalValueSlider;
        [Space(5)]
        [SerializeField] private Toggle _foxCanBeEaten;

        [Header("Hunger")]
        [SerializeField] private Text _foxHungerMaxText;
        [SerializeField] private Slider _foxHungerMaxSlider;
        [Space(5)]
        [SerializeField] private Text _foxHungerThresholdText;
        [SerializeField] private Slider _foxHungerThresholdSlider;
        [Space(5)]
        [SerializeField] private Text _foxHungerIncreaseBaseText;
        [SerializeField] private Slider _foxHungerIncreaseBaseSlider;
        [Space(5)]
        [SerializeField] private Text _foxHungerIncreaseYoungText;
        [SerializeField] private Slider _foxHungerIncreaseYoungSlider;
        [Space(5)]
        [SerializeField] private Text _foxHungerIncreaseAdultText;
        [SerializeField] private Slider _foxHungerIncreaseAdultSlider;
        [Space(5)]
        [SerializeField] private Text _foxHungerIncreaseOldText;
        [SerializeField] private Slider _foxHungerIncreaseOldSlider;
        [Space(5)]
        [SerializeField] private Text _foxEatingSpeedText;
        [SerializeField] private Slider _foxEatingSpeedSlider;

        [Header("Thirst")]
        [SerializeField] private Text _foxThirstMaxText;
        [SerializeField] private Slider _foxThirstMaxSlider;
        [Space(5)]
        [SerializeField] private Text _foxThirstThresholdText;
        [SerializeField] private Slider _foxThirstThresholdSlider;
        [Space(5)]
        [SerializeField] private Text _foxThirstIncreaseBaseText;
        [SerializeField] private Slider _foxThirstIncreaseBaseSlider;
        [Space(5)]
        [SerializeField] private Text _foxThirstIncreaseYoungText;
        [SerializeField] private Slider _foxThirstIncreaseYoungSlider;
        [Space(5)]
        [SerializeField] private Text _foxThirstIncreaseAdultText;
        [SerializeField] private Slider _foxThirstIncreaseAdultSlider;
        [Space(5)]
        [SerializeField] private Text _foxThirstIncreaseOldText;
        [SerializeField] private Slider _foxThirstIncreaseOldSlider;
        [Space(5)]
        [SerializeField] private Text _foxDrinkingSpeedText;
        [SerializeField] private Slider _foxDrinkingSpeedSlider;

        [Header("Mate")]
        [SerializeField] private Text _foxMatingDurationText;
        [SerializeField] private Slider _foxMatingDurationSlider;
        [Space(5)]
        [SerializeField] private Text _foxMatingThresholdText;
        [SerializeField] private Slider _foxMatingThresholdSlider;
        [Space(5)]
        [SerializeField] private Text _foxReproductiveUrgeIncreaseText;
        [SerializeField] private Slider _foxReproductiveUrgeIncreaseSlider;

        [Header("Pregnancy")]
        [SerializeField] private Text _foxPregnancyLengthText;
        [SerializeField] private Slider _foxPregnancyLengthSlider;
        [Space(5)]
        [SerializeField] private Text _foxBirthDurationText;
        [SerializeField] private Slider _foxBirthDurationSlider;
        [Space(5)]
        [SerializeField] private Text _foxLitterSizeMinText;
        [SerializeField] private Slider _foxLitterSizeMinSlider;
        [Space(5)]
        [SerializeField] private Text _foxLitterSizeMaxText;
        [SerializeField] private Slider _foxLitterSizeMaxSlider;
        [Space(5)]
        [SerializeField] private Text _foxLitterSizeAveText;
        [SerializeField] private Slider _foxLitterSizeAveSlider;

        [Header("Movement")]
        [SerializeField] private Text _foxMovementSpeedText;
        [SerializeField] private Slider _foxMovementSpeedSlider;
        [Space(5)]
        [SerializeField] private Text _foxMovementMultiplierBaseText;
        [SerializeField] private Slider _foxMovementMultiplierBaseSlider;
        [Space(5)]
        [SerializeField] private Text _foxMovementMultiplierYoungText;
        [SerializeField] private Slider _foxMovementMultiplierYoungSlider;
        [Space(5)]
        [SerializeField] private Text _foxMovementMultiplierAdultText;
        [SerializeField] private Slider _foxMovementMultiplierAdultSlider;
        [Space(5)]
        [SerializeField] private Text _foxMovementMultiplierOldText;
        [SerializeField] private Slider _foxMovementMultiplierOldSlider;
        [Space(5)]
        [SerializeField] private Text _foxMovementMultiplierPregnantText;
        [SerializeField] private Slider _foxMovementMultiplierPregnantSlider;

        [Header("Target")]
        [SerializeField] private Text _foxSightRadiusText;
        [SerializeField] private Slider _foxSightRadiusSlider;

        [Header("Size")]
        [SerializeField] private Text _foxSizeMaleText;
        [SerializeField] private Slider _foxSizeMaleSlider;
        [Space(5)]
        [SerializeField] private Text _foxSizeFemaleText;
        [SerializeField] private Slider _foxSizeFemaleSlider;
        #endregion
        #region Grass
        [Header("Grass")]
        [Header("Edible")]
        [SerializeField] private Text _grassNutritionalValueText;
        [SerializeField] private Slider _grassNutritionalValueSlider;
        [Space(5)]
        [SerializeField] private Toggle _grassCanBeEaten;

        [Header("Size")]
        [SerializeField] private Text _grassSizeText;
        [SerializeField] private Slider _grassSizeSlider;
        #endregion
        #endregion
        #endregion

        #region Initialisation - Button Selection & Menu Order
        private void Start()
        {
            _menuNumber = MenuNumber.Main;
            StoreInitialEntityDefaults();
        }
        #endregion

        #region Main Section
        public IEnumerator ConfirmationBox()
        {
            _confirmationMenu.SetActive(true);
            yield return new WaitForSeconds(2);
            _confirmationMenu.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_menuNumber == MenuNumber.Options || _menuNumber == MenuNumber.NewGame || _menuNumber == MenuNumber.LoadGame || _menuNumber == MenuNumber.InitialProperties || _menuNumber == MenuNumber.GenerateMap)
                {
                    GoBackToMainMenu();
                    ClickSound();
                }

                else if (_menuNumber == MenuNumber.Graphics || _menuNumber == MenuNumber.Sound || _menuNumber == MenuNumber.Gameplay)
                {
                    GoBackToOptionsMenu();
                    ClickSound();
                }

                else if (_menuNumber == MenuNumber.Controls) //CONTROLS MENU
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
                    _gameplayMenu.SetActive(false);
                    _controlsMenu.SetActive(true);
                    _menuNumber = MenuNumber.Controls;
                    break;
                case "Graphics":
                    _generalSettingsCanvas.SetActive(false);
                    _graphicsMenu.SetActive(true);
                    _menuNumber = MenuNumber.Graphics;
                    break;
                case "Sound":
                    _generalSettingsCanvas.SetActive(false);
                    _soundMenu.SetActive(true);
                    _menuNumber = MenuNumber.Sound;
                    break;
                case "Gameplay":
                    _generalSettingsCanvas.SetActive(false);
                    _gameplayMenu.SetActive(true);
                    _menuNumber = MenuNumber.Gameplay;
                    break;
                case "Exit":
                    Debug.Log("YES QUIT!");
                    Application.Quit();
                    break;
                case "Options":
                    _menuDefaultCanvas.SetActive(false);
                    _generalSettingsCanvas.SetActive(true);
                    _menuNumber = MenuNumber.Options;
                    break;
                case "LoadGame":
                    _menuDefaultCanvas.SetActive(false);
                    _loadGameDialog.SetActive(true);
                    _menuNumber = MenuNumber.LoadGame;
                    LoadMap();
                    break;
                case "NewGame":
                    _menuDefaultCanvas.SetActive(false);
                    ResetButton("InitialProperties");
                    _initialPropertiesCanvas.SetActive(true);
                    _menuNumber = MenuNumber.NewGame;
                    break;
                case "InitialProperties":
                    _initialPropertiesCanvas.SetActive(false);
                    _newGameDialog.SetActive(true);
                    _menuNumber = MenuNumber.InitialProperties;
                    break;
                case "GenerateMap":
                    _menuDefaultCanvas.SetActive(false);
                    _generateMapCanvas.SetActive(true);
                    _menuNumber = MenuNumber.GenerateMap;
                    break;
                default:
                    Debug.Log("Button clicked with no known case.");
                    break;
            }
        }
        #endregion

        #region Loading Map From File
        /*Opens file browser to load in the map*/
        public void LoadMap()
        {
            var paths = StandaloneFileBrowser.StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", false);
            if (paths.Length > 0)
            {
                StartCoroutine(OutputRoutine(new Uri(paths[0]).AbsoluteUri));
            }
        }

        /*returns the text representation of the file at url*/
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
                _fileContents = webRequest.downloadHandler.text;
                Debug.Log(_fileContents);
            }
        }

        /* attempts to load the given file into the MapReader */
        private bool MapFileValid()
        {
            List<List<MapReader.TerrainCost>> mapList = new List<List<MapReader.TerrainCost>>();
            return MapReader.ReadInMapFromString(_fileContents, ref mapList);
        }
        #endregion

        #region Options
        public void VolumeSlider(float volume)
        {
            AudioListener.volume = volume;
            _volumeText.text = volume.ToString("0.0");
        }

        public void VolumeApply()
        {
            PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
            Debug.Log(PlayerPrefs.GetFloat("masterVolume"));
            StartCoroutine(ConfirmationBox());
        }

        public void BrightnessSlider(float brightness)
        {
            _brightnessEffect.brightness = brightness;
            _brightnessText.text = brightness.ToString("0.0");
        }

        public void BrightnessApply()
        {
            PlayerPrefs.SetFloat("masterBrightness", _brightnessEffect.brightness);
            Debug.Log(PlayerPrefs.GetFloat("masterBrightness"));
            StartCoroutine(ConfirmationBox());
        }

        public void ControllerSen()
        {

        }

        public void GameplayApply()
        {
            Debug.Log("Sensitivity" + " " + PlayerPrefs.GetFloat("masterSen"));

            StartCoroutine(ConfirmationBox());
        }
        #endregion

        #region Initial Properties
        /*Saves the initial properties slider and toggle values to respective Defaults field */
        public void InitialPropertiesApply()
        {
            Debug.Log("Apply Initial Properties");
            RabbitDefaults.AgeMax = _rabbitAgeMaxSlider.value;

            RabbitDefaults.NutritionalValue = _rabbitNutritionalValueSlider.value;
            RabbitDefaults.CanBeEaten = _rabbitCanBeEaten.isOn;

            RabbitDefaults.HungerMax = _rabbitHungerMaxSlider.value;
            RabbitDefaults.HungryThreshold = _rabbitHungerThresholdSlider.value;
            RabbitDefaults.HungerIncrease = _rabbitHungerIncreaseBaseSlider.value;
            RabbitDefaults.YoungHungerIncrease = _rabbitHungerIncreaseYoungSlider.value;
            RabbitDefaults.AdultHungerIncrease = _rabbitHungerIncreaseAdultSlider.value;
            RabbitDefaults.OldHungerIncrease = _rabbitHungerIncreaseOldSlider.value;
            RabbitDefaults.EatingSpeed = _rabbitEatingSpeedSlider.value;

            RabbitDefaults.ThirstMax = _rabbitThirstMaxSlider.value;
            RabbitDefaults.ThirstyThreshold = _rabbitThirstThresholdSlider.value;
            RabbitDefaults.ThirstIncrease = _rabbitThirstIncreaseBaseSlider.value;
            //RabbitDefaults. = rabbitThirstIncreaseYoungSlider.value;
            //RabbitDefaults. = rabbitThirstIncreaseAdultSlider.value;
            //RabbitDefaults. = rabbitThirstIncreaseOldSlider.value;
            RabbitDefaults.DrinkingSpeed = _rabbitDrinkingSpeedSlider.value;

            RabbitDefaults.MatingDuration = _rabbitMatingDurationSlider.value;
            RabbitDefaults.MatingThreshold = _rabbitMatingThresholdSlider.value;
            RabbitDefaults.ReproductiveUrgeIncreaseMale = _rabbitReproductiveUrgeIncreaseSlider.value;


            RabbitDefaults.PregnancyLength = _rabbitPregnancyLengthSlider.value;
            RabbitDefaults.BirthDuration = _rabbitBirthDurationSlider.value;
            RabbitDefaults.LitterSizeMin = (int)_rabbitLitterSizeMinSlider.value;
            RabbitDefaults.LitterSizeMax = (int)_rabbitLitterSizeMaxSlider.value;
            RabbitDefaults.LitterSizeAve = (int)_rabbitLitterSizeAveSlider.value;

            RabbitDefaults.MoveSpeed = _rabbitMovementSpeedSlider.value;
            RabbitDefaults.OriginalMoveMultiplier = _rabbitMovementMultiplierBaseSlider.value;
            RabbitDefaults.YoungMoveMultiplier = _rabbitMovementMultiplierYoungSlider.value;
            RabbitDefaults.AdultMoveMultiplier = _rabbitMovementMultiplierAdultSlider.value;
            RabbitDefaults.OldMoveMultiplier = _rabbitMovementMultiplierOldSlider.value;
            RabbitDefaults.PregnancyMoveMultiplier = _rabbitMovementMultiplierPregnantSlider.value;

            RabbitDefaults.SightRadius = _rabbitSightRadiusSlider.value;

            RabbitDefaults.ScaleMale = _rabbitSizeMaleSlider.value;
            RabbitDefaults.ScaleFemale = _rabbitSizeFemaleSlider.value;

            FoxDefaults.AgeMax = _foxAgeMaxSlider.value;

            FoxDefaults.CanBeEaten = _foxCanBeEaten.isOn;
            FoxDefaults.NutritionalValue = _foxNutritionalValueSlider.value;

            FoxDefaults.HungerMax = _foxHungerMaxSlider.value;
            FoxDefaults.HungryThreshold = _foxHungerThresholdSlider.value;
            FoxDefaults.HungerIncrease = _foxHungerIncreaseBaseSlider.value;
            FoxDefaults.YoungHungerIncrease = _foxHungerIncreaseYoungSlider.value;
            FoxDefaults.AdultHungerIncrease = _foxHungerIncreaseAdultSlider.value;
            FoxDefaults.OldHungerIncrease = _foxHungerIncreaseOldSlider.value;
            FoxDefaults.EatingSpeed = _foxEatingSpeedSlider.value;

            FoxDefaults.ThirstMax = _foxThirstMaxSlider.value;
            FoxDefaults.ThirstyThreshold = _foxThirstThresholdSlider.value;
            FoxDefaults.ThirstIncrease = _foxThirstIncreaseBaseSlider.value;
            //FoxDefaults. = foxThirstIncreaseYoungSlider.value;
            //FoxDefaults. = foxThirstIncreaseAdultSlider.value;
            //FoxDefaults. = foxThirstIncreaseOldSlider.value;
            FoxDefaults.DrinkingSpeed = _foxDrinkingSpeedSlider.value;

            FoxDefaults.MatingDuration = _foxMatingDurationSlider.value;
            FoxDefaults.MatingThreshold = _foxMatingThresholdSlider.value;
            FoxDefaults.ReproductiveUrgeIncreaseMale = _foxReproductiveUrgeIncreaseSlider.value;

            FoxDefaults.PregnancyLength = _foxPregnancyLengthSlider.value;
            FoxDefaults.BirthDuration = _foxBirthDurationSlider.value;
            FoxDefaults.LitterSizeMin = (int)_foxLitterSizeMinSlider.value;
            FoxDefaults.LitterSizeMax = (int)_foxLitterSizeMaxSlider.value;
            FoxDefaults.LitterSizeAve = (int)_foxLitterSizeAveSlider.value;

            FoxDefaults.MoveSpeed = _foxMovementSpeedSlider.value;
            FoxDefaults.OriginalMoveMultiplier = _foxMovementMultiplierBaseSlider.value;
            FoxDefaults.YoungMoveMultiplier = _foxMovementMultiplierYoungSlider.value;
            FoxDefaults.AdultMoveMultiplier = _foxMovementMultiplierAdultSlider.value;
            FoxDefaults.OldMoveMultiplier = _foxMovementMultiplierOldSlider.value;
            FoxDefaults.PregnancyMoveMultiplier = _foxMovementMultiplierPregnantSlider.value;

            FoxDefaults.SightRadius = _foxSightRadiusSlider.value;

            FoxDefaults.ScaleMale = _foxSizeMaleSlider.value;
            FoxDefaults.ScaleFemale = _foxSizeFemaleSlider.value;

            GrassDefaults.NutritionalValue = _grassNutritionalValueSlider.value;
            GrassDefaults.CanBeEaten = _grassCanBeEaten.isOn;
            GrassDefaults.Scale = _grassSizeSlider.value;
            StartCoroutine(ConfirmationBox());
        }

        /*Update inital animal to spawn count in SimulationManager*/
        public void OnSetNumberToSpawn(string entityToUpdate)
        {
            switch (entityToUpdate)
            {
                case "Rabbit":
                    //limit to 0 or above
                    int rabbitNumber = int.Parse(_rabbitNumberInputField.text);
                    SimulationManager.InitialRabbitsToSpawn = rabbitNumber < 0 ? 0 : rabbitNumber;
                    break;
                case "Fox":
                    //limit to 0 or above
                    int foxNumber = int.Parse(_foxNumberInputField.text);
                    SimulationManager.InitialFoxesToSpawn = foxNumber < 0 ? 0 : foxNumber;
                    break;
                case "Grass":
                    //limit to 0 or above
                    int grassNumber = int.Parse(_grassNumberInputField.text);
                    SimulationManager.InitialGrassToSpawn = grassNumber < 0 ? 0 : grassNumber;
                    break;
                default:
                    Debug.LogWarning("Attempted to update unknown entity in switch: " + entityToUpdate, this);
                    break;
            }
        }

        /*Updates the value text of the slider when the slider is moved, calls InitialPropertiesApply after*/
        public void InitialPropertiesUpdate(string propertyToUpdate)
        {
            switch (propertyToUpdate)
            {
                case "RabbitAgeMax":
                    _rabbitAgeMaxText.text = _rabbitAgeMaxSlider.value.ToString("N");
                    break;
                case "RabbitNutritionalValue":
                    _rabbitNutritionalValueText.text = _rabbitNutritionalValueSlider.value.ToString();
                    break;
                case "RabbitCanBeEaten":
                    //rabbitCanBeEaten;
                    break;
                case "RabbitHungerMax":
                    _rabbitHungerMaxText.text = _rabbitHungerMaxSlider.value.ToString();
                    break;
                case "RabbitHungerThreshold":
                    _rabbitHungerThresholdText.text = _rabbitHungerThresholdSlider.value.ToString();
                    break;
                case "RabbitHungerIncreaseBase":
                    _rabbitHungerIncreaseBaseText.text = _rabbitHungerIncreaseBaseSlider.value.ToString();
                    break;
                case "RabbitHungerIncreaseYoung":
                    _rabbitHungerIncreaseYoungText.text = _rabbitHungerIncreaseYoungSlider.value.ToString();
                    break;
                case "RabbitHungerIncreaseAdult":
                    _rabbitHungerIncreaseAdultText.text = _rabbitHungerIncreaseAdultSlider.value.ToString();
                    break;
                case "RabbitHungerIncreaseOld":
                    _rabbitHungerIncreaseOldText.text = _rabbitHungerIncreaseOldSlider.value.ToString();
                    break;
                case "RabbitEatingSpeed":
                    _rabbitEatingSpeedText.text = _rabbitEatingSpeedSlider.value.ToString();
                    break;
                case "RabbitThirstMax":
                    _rabbitThirstMaxText.text = _rabbitThirstMaxSlider.value.ToString();
                    break;
                case "RabbitThirstThreshold":
                    _rabbitThirstThresholdText.text = _rabbitThirstThresholdSlider.value.ToString();
                    break;
                case "RabbitThirstIncreaseBase":
                    _rabbitThirstIncreaseBaseText.text = _rabbitThirstIncreaseBaseSlider.value.ToString();
                    break;
                case "RabbitThirstIncreaseYoung":
                    _rabbitThirstIncreaseYoungText.text = _rabbitThirstIncreaseYoungSlider.value.ToString();
                    break;
                case "RabbitThirstIncreaseAdult":
                    _rabbitThirstIncreaseAdultText.text = _rabbitThirstIncreaseAdultSlider.value.ToString();
                    break;
                case "RabbitThirstIncreaseOld":
                    _rabbitThirstIncreaseOldText.text = _rabbitThirstIncreaseOldSlider.value.ToString();
                    break;
                case "RabbitDrinkingSpeed":
                    _rabbitDrinkingSpeedText.text = _rabbitDrinkingSpeedSlider.value.ToString();
                    break;
                case "RabbitMatingDuration":
                    _rabbitMatingDurationText.text = _rabbitMatingDurationSlider.value.ToString();
                    break;
                case "RabbitMatingThreshold":
                    _rabbitMatingThresholdText.text = _rabbitMatingThresholdSlider.value.ToString();
                    break;
                case "RabbitReproductiveUrgeIncrease":
                    _rabbitReproductiveUrgeIncreaseText.text = _rabbitReproductiveUrgeIncreaseSlider.value.ToString();
                    break;
                case "RabbitPregnancyLength":
                    _rabbitPregnancyLengthText.text = _rabbitPregnancyLengthSlider.value.ToString();
                    break;
                case "RabbitBirthDuration":
                    _rabbitBirthDurationText.text = _rabbitBirthDurationSlider.value.ToString();
                    break;
                case "RabbitLitterSizeMin":
                    _rabbitLitterSizeMinText.text = _rabbitLitterSizeMinSlider.value.ToString();
                    break;
                case "RabbitLitterSizeMax":
                    _rabbitLitterSizeMaxText.text = _rabbitLitterSizeMaxSlider.value.ToString();
                    break;
                case "RabbitLitterSizeAve":
                    _rabbitLitterSizeAveText.text = _rabbitLitterSizeAveSlider.value.ToString();
                    break;
                case "RabbitMovementSpeed":
                    _rabbitMovementSpeedText.text = _rabbitMovementSpeedSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierBase":
                    _rabbitMovementMultiplierBaseText.text = _rabbitMovementMultiplierBaseSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierYoung":
                    _rabbitMovementMultiplierYoungText.text = _rabbitMovementMultiplierYoungSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierAdult":
                    _rabbitMovementMultiplierAdultText.text = _rabbitMovementMultiplierAdultSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierOld":
                    _rabbitMovementMultiplierOldText.text = _rabbitMovementMultiplierOldSlider.value.ToString();
                    break;
                case "RabbitMovementMultiplierPregnant":
                    _rabbitMovementMultiplierPregnantText.text = _rabbitMovementMultiplierPregnantSlider.value.ToString();
                    break;
                case "RabbitSightRadius":
                    _rabbitSightRadiusText.text = _rabbitSightRadiusSlider.value.ToString();
                    break;
                case "RabbitSizeMale":
                    _rabbitSizeMaleText.text = _rabbitSizeMaleSlider.value.ToString();
                    break;
                case "RabbitSizeFemale":
                    _rabbitSizeFemaleText.text = _rabbitSizeFemaleSlider.value.ToString();
                    break;
                case "FoxAgeMax":
                    _foxAgeMaxText.text = _foxAgeMaxSlider.value.ToString();
                    break;
                case "FoxNutritionalValue":
                    _foxNutritionalValueText.text = _foxNutritionalValueSlider.value.ToString();
                    break;
                case "FoxCanBeEaten":
                    //foxCanBeEaten;
                    break;
                case "FoxHungerMax":
                    _foxHungerMaxText.text = _foxHungerMaxSlider.value.ToString();
                    break;
                case "FoxHungerThreshold":
                    _foxHungerThresholdText.text = _foxHungerThresholdSlider.value.ToString();
                    break;
                case "FoxHungerIncreaseBase":
                    _foxHungerIncreaseBaseText.text = _foxHungerIncreaseBaseSlider.value.ToString();
                    break;
                case "FoxHungerIncreaseYoung":
                    _foxHungerIncreaseYoungText.text = _foxHungerIncreaseYoungSlider.value.ToString();
                    break;
                case "FoxHungerIncreaseAdult":
                    _foxHungerIncreaseAdultText.text = _foxHungerIncreaseAdultSlider.value.ToString();
                    break;
                case "FoxHungerIncreaseOld":
                    _foxHungerIncreaseOldText.text = _foxHungerIncreaseOldSlider.value.ToString();
                    break;
                case "FoxEatingSpeed":
                    _foxEatingSpeedText.text = _foxEatingSpeedSlider.value.ToString();
                    break;
                case "FoxThirstMax":
                    _foxThirstMaxText.text = _foxThirstMaxSlider.value.ToString();
                    break;
                case "FoxThirstThreshold":
                    _foxThirstThresholdText.text = _foxThirstThresholdSlider.value.ToString();
                    break;
                case "FoxThirstIncreaseBase":
                    _foxThirstIncreaseBaseText.text = _foxThirstIncreaseBaseSlider.value.ToString();
                    break;
                case "FoxThirstIncreaseYoung":
                    _foxThirstIncreaseYoungText.text = _foxThirstIncreaseYoungSlider.value.ToString();
                    break;
                case "FoxThirstIncreaseAdult":
                    _foxThirstIncreaseAdultText.text = _foxThirstIncreaseAdultSlider.value.ToString();
                    break;
                case "FoxThirstIncreaseOld":
                    _foxThirstIncreaseOldText.text = _foxThirstIncreaseOldSlider.value.ToString();
                    break;
                case "FoxDrinkingSpeed":
                    _foxDrinkingSpeedText.text = _foxDrinkingSpeedSlider.value.ToString();
                    break;
                case "FoxMatingDuration":
                    _foxMatingDurationText.text = _foxMatingDurationSlider.value.ToString();
                    break;
                case "FoxMatingThreshold":
                    _foxMatingThresholdText.text = _foxMatingThresholdSlider.value.ToString();
                    break;
                case "FoxReproductiveUrgeIncrease":
                    _foxReproductiveUrgeIncreaseText.text = _foxReproductiveUrgeIncreaseSlider.value.ToString();
                    break;
                case "FoxPregnancyLength":
                    _foxPregnancyLengthText.text = _foxPregnancyLengthSlider.value.ToString();
                    break;
                case "FoxBirthDuration":
                    _foxBirthDurationText.text = _foxBirthDurationSlider.value.ToString();
                    break;
                case "FoxLitterSizeMin":
                    _foxLitterSizeMinText.text = _foxLitterSizeMinSlider.value.ToString();
                    break;
                case "FoxLitterSizeMax":
                    _foxLitterSizeMaxText.text = _foxLitterSizeMaxSlider.value.ToString();
                    break;
                case "FoxLitterSizeAve":
                    _foxLitterSizeAveText.text = _foxLitterSizeAveSlider.value.ToString();
                    break;
                case "FoxMovementSpeed":
                    _foxMovementSpeedText.text = _foxMovementSpeedSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierBase":
                    _foxMovementMultiplierBaseText.text = _foxMovementMultiplierBaseSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierYoung":
                    _foxMovementMultiplierYoungText.text = _foxMovementMultiplierYoungSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierAdult":
                    _foxMovementMultiplierAdultText.text = _foxMovementMultiplierAdultSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierOld":
                    _foxMovementMultiplierOldText.text = _foxMovementMultiplierOldSlider.value.ToString();
                    break;
                case "FoxMovementMultiplierPregnant":
                    _foxMovementMultiplierPregnantText.text = _foxMovementMultiplierPregnantSlider.value.ToString();
                    break;
                case "FoxSightRadius":
                    _foxSightRadiusText.text = _foxSightRadiusSlider.value.ToString();
                    break;
                case "FoxSizeMale":
                    _foxSizeMaleText.text = _foxSizeMaleSlider.value.ToString();
                    break;
                case "FoxSizeFemale":
                    _foxSizeFemaleText.text = _foxSizeFemaleSlider.value.ToString();
                    break;
                case "GrassNutritionalValue":
                    _grassNutritionalValueText.text = _grassNutritionalValueSlider.value.ToString();
                    break;
                case "GrassCanBeEaten":
                    //grassCanBeEaten;
                    break;
                case "GrassSize":
                    _grassSizeText.text = _grassSizeSlider.value.ToString();
                    break;
                default:
                    Debug.LogWarning("Attempted to update unknown property in switch: " + propertyToUpdate, this);
                    break;
            }
            InitialPropertiesApply();
        }
        #endregion

        #region ResetButton
        /*stores the starting value of the defaults before they are changed so they can be reset*/
        private void StoreInitialEntityDefaults()
        {
            _rabbitAgeMaxReset = RabbitDefaults.AgeMax;

            _rabbitNutritionalValueReset = RabbitDefaults.NutritionalValue;

            _rabbitCanBeEatenReset = RabbitDefaults.CanBeEaten;

            _rabbitHungerMaxReset = RabbitDefaults.HungerMax;

            _rabbitHungerThresholdReset = RabbitDefaults.HungryThreshold;

            _rabbitHungerIncreaseBaseReset = RabbitDefaults.HungerIncrease;

            _rabbitHungerIncreaseYoungReset = RabbitDefaults.YoungHungerIncrease;

            _rabbitHungerIncreaseAdultReset = RabbitDefaults.AdultHungerIncrease;

            _rabbitHungerIncreaseOldReset = RabbitDefaults.OldHungerIncrease;

            _rabbitEatingSpeedReset = RabbitDefaults.EatingSpeed;

            _rabbitThirstMaxReset = RabbitDefaults.ThirstMax;

            _rabbitThirstThresholdReset = RabbitDefaults.ThirstyThreshold;

            _rabbitThirstIncreaseBaseReset = RabbitDefaults.ThirstIncrease;

            //rabbitThirstIncreaseYoungReset = RabbitDefaults. ;

            //rabbitThirstIncreaseAdultReset = RabbitDefaults. ;

            //rabbitThirstIncreaseOldReset = RabbitDefaults. ;

            _rabbitDrinkingSpeedReset = RabbitDefaults.DrinkingSpeed;

            _rabbitMatingDurationReset = RabbitDefaults.MatingDuration;

            _rabbitMatingThresholdReset = RabbitDefaults.MatingThreshold;

            _rabbitReproductiveUrgeIncreaseReset = RabbitDefaults.ReproductiveUrgeIncreaseMale;

            _rabbitPregnancyLengthReset = RabbitDefaults.PregnancyLength;

            _rabbitBirthDurationReset = RabbitDefaults.BirthDuration;

            _rabbitLitterSizeMinReset = RabbitDefaults.LitterSizeMin;

            _rabbitLitterSizeMaxReset = RabbitDefaults.LitterSizeMax;

            _rabbitLitterSizeAveReset = RabbitDefaults.LitterSizeAve;

            _rabbitMovementSpeedReset = RabbitDefaults.MoveSpeed;

            _rabbitMovementMultiplierBaseReset = RabbitDefaults.OriginalMoveMultiplier;

            _rabbitMovementMultiplierYoungReset = RabbitDefaults.YoungMoveMultiplier;

            _rabbitMovementMultiplierAdultReset = RabbitDefaults.AdultMoveMultiplier;

            _rabbitMovementMultiplierOldReset = RabbitDefaults.OldMoveMultiplier;

            _rabbitMovementMultiplierPregnantReset = RabbitDefaults.PregnancyMoveMultiplier;

            _rabbitSightRadiusReset = RabbitDefaults.SightRadius;

            _rabbitSizeMaleReset = RabbitDefaults.ScaleMale;

            _rabbitSizeFemaleReset = RabbitDefaults.ScaleFemale;



            _foxAgeMaxReset = FoxDefaults.AgeMax;

            _foxNutritionalValueReset = FoxDefaults.NutritionalValue;

            _foxCanBeEatenReset = FoxDefaults.CanBeEaten;

            _foxHungerMaxReset = FoxDefaults.HungerMax;

            _foxHungerThresholdReset = FoxDefaults.HungryThreshold;

            _foxHungerIncreaseBaseReset = FoxDefaults.HungerIncrease;

            _foxHungerIncreaseYoungReset = FoxDefaults.YoungHungerIncrease;

            _foxHungerIncreaseAdultReset = FoxDefaults.AdultHungerIncrease;

            _foxHungerIncreaseOldReset = FoxDefaults.OldHungerIncrease;

            _foxEatingSpeedReset = FoxDefaults.EatingSpeed;

            _foxThirstMaxReset = FoxDefaults.ThirstMax;

            _foxThirstThresholdReset = FoxDefaults.ThirstyThreshold;

            _foxThirstIncreaseBaseReset = FoxDefaults.ThirstIncrease;

            //foxThirstIncreaseYoungReset = FoxDefaults. ;

            //foxThirstIncreaseAdultReset = FoxDefaults. ;

            //foxThirstIncreaseOldReset = FoxDefaults. ;

            _foxDrinkingSpeedReset = FoxDefaults.DrinkingSpeed;

            _foxMatingDurationReset = FoxDefaults.MatingDuration;

            _foxMatingThresholdReset = FoxDefaults.MatingThreshold;

            _foxReproductiveUrgeIncreaseReset = FoxDefaults.ReproductiveUrgeIncreaseMale;

            _foxPregnancyLengthReset = FoxDefaults.PregnancyLength;

            _foxBirthDurationReset = FoxDefaults.BirthDuration;

            _foxLitterSizeMinReset = FoxDefaults.LitterSizeMin;

            _foxLitterSizeMaxReset = FoxDefaults.LitterSizeMax;

            _foxLitterSizeAveReset = FoxDefaults.LitterSizeAve;

            _foxMovementSpeedReset = FoxDefaults.MoveSpeed;

            _foxMovementMultiplierBaseReset = FoxDefaults.OriginalMoveMultiplier;

            _foxMovementMultiplierYoungReset = FoxDefaults.YoungMoveMultiplier;

            _foxMovementMultiplierAdultReset = FoxDefaults.AdultMoveMultiplier;

            _foxMovementMultiplierOldReset = FoxDefaults.OldMoveMultiplier;

            _foxMovementMultiplierPregnantReset = FoxDefaults.PregnancyMoveMultiplier;

            _foxSightRadiusReset = FoxDefaults.SightRadius;

            _foxSizeMaleReset = FoxDefaults.ScaleMale;

            _foxSizeFemaleReset = FoxDefaults.ScaleFemale;



            _grassNutritionalValueReset = GrassDefaults.NutritionalValue;

            _grassCanBeEatenReset = GrassDefaults.CanBeEaten;

            _grassSizeReset = GrassDefaults.Scale;
        }

        /*Resets the menu item values that are for the given menu*/
        public void ResetButton(string menuToReset)
        {
            switch (menuToReset)
            {
                case "Brightness":
                    _brightnessEffect.brightness = _defaultBrightness;
                    _brightnessSlider.value = _defaultBrightness;
                    _brightnessText.text = _defaultBrightness.ToString("0.0");
                    BrightnessApply();
                    break;
                case "Audio":
                    AudioListener.volume = _defaultVolume;
                    _volumeSlider.value = _defaultVolume;
                    _volumeText.text = _defaultVolume.ToString("0.0");
                    VolumeApply();
                    break;
                case "Graphics":
                    GameplayApply();
                    break;
                case "InitialProperties":
                    _rabbitAgeMaxText.text = _rabbitAgeMaxReset.ToString();
                    _rabbitAgeMaxSlider.wholeNumbers = true;
                    _rabbitAgeMaxSlider.minValue = 0;
                    _rabbitAgeMaxSlider.maxValue = _rabbitAgeMaxReset * 10;
                    _rabbitAgeMaxSlider.value = _rabbitAgeMaxReset;

                    _rabbitNutritionalValueText.text = _rabbitNutritionalValueReset.ToString();
                    _rabbitNutritionalValueSlider.wholeNumbers = true;
                    _rabbitNutritionalValueSlider.minValue = 0;
                    _rabbitNutritionalValueSlider.maxValue = _rabbitNutritionalValueReset * 10;
                    _rabbitNutritionalValueSlider.value = _rabbitNutritionalValueReset;

                    _rabbitCanBeEaten.isOn = _rabbitCanBeEatenReset;

                    _rabbitHungerMaxText.text = _rabbitHungerMaxReset.ToString();
                    _rabbitHungerMaxSlider.wholeNumbers = true;
                    _rabbitHungerMaxSlider.minValue = 0;
                    _rabbitHungerMaxSlider.maxValue = _rabbitHungerMaxReset * 10;
                    _rabbitHungerMaxSlider.value = _rabbitHungerMaxReset;

                    _rabbitHungerThresholdText.text = _rabbitHungerThresholdReset.ToString();
                    _rabbitHungerThresholdSlider.wholeNumbers = true;
                    _rabbitHungerThresholdSlider.minValue = 0;
                    _rabbitHungerThresholdSlider.maxValue = _rabbitHungerThresholdReset * 10;
                    _rabbitHungerThresholdSlider.value = _rabbitHungerThresholdReset;

                    _rabbitHungerIncreaseBaseText.text = _rabbitHungerIncreaseBaseReset.ToString();
                    _rabbitHungerIncreaseBaseSlider.wholeNumbers = false;
                    _rabbitHungerIncreaseBaseSlider.minValue = 0;
                    _rabbitHungerIncreaseBaseSlider.maxValue = _rabbitHungerIncreaseBaseReset * 10;
                    _rabbitHungerIncreaseBaseSlider.value = _rabbitHungerIncreaseBaseReset;

                    _rabbitHungerIncreaseYoungText.text = _rabbitHungerIncreaseYoungReset.ToString();
                    _rabbitHungerIncreaseYoungSlider.wholeNumbers = false;
                    _rabbitHungerIncreaseYoungSlider.minValue = 0;
                    _rabbitHungerIncreaseYoungSlider.maxValue = _rabbitHungerIncreaseYoungReset * 10;
                    _rabbitHungerIncreaseYoungSlider.value = _rabbitHungerIncreaseYoungReset;

                    _rabbitHungerIncreaseAdultText.text = _rabbitHungerIncreaseAdultReset.ToString();
                    _rabbitHungerIncreaseAdultSlider.wholeNumbers = false;
                    _rabbitHungerIncreaseAdultSlider.minValue = 0;
                    _rabbitHungerIncreaseAdultSlider.maxValue = _rabbitHungerIncreaseAdultReset * 10;
                    _rabbitHungerIncreaseAdultSlider.value = _rabbitHungerIncreaseAdultReset;

                    _rabbitHungerIncreaseOldText.text = _rabbitHungerIncreaseOldReset.ToString();
                    _rabbitHungerIncreaseOldSlider.wholeNumbers = false;
                    _rabbitHungerIncreaseOldSlider.minValue = 0;
                    _rabbitHungerIncreaseOldSlider.maxValue = _rabbitHungerIncreaseOldReset * 10;
                    _rabbitHungerIncreaseOldSlider.value = _rabbitHungerIncreaseOldReset;

                    _rabbitEatingSpeedText.text = _rabbitEatingSpeedReset.ToString();
                    _rabbitEatingSpeedSlider.wholeNumbers = true;
                    _rabbitEatingSpeedSlider.minValue = 0;
                    _rabbitEatingSpeedSlider.maxValue = _rabbitEatingSpeedReset * 10;
                    _rabbitEatingSpeedSlider.value = _rabbitEatingSpeedReset;

                    _rabbitThirstMaxText.text = _rabbitThirstMaxReset.ToString();
                    _rabbitThirstMaxSlider.wholeNumbers = true;
                    _rabbitThirstMaxSlider.minValue = 0;
                    _rabbitThirstMaxSlider.maxValue = _rabbitThirstMaxReset * 10;
                    _rabbitThirstMaxSlider.value = _rabbitThirstMaxReset;

                    _rabbitThirstThresholdText.text = _rabbitThirstThresholdReset.ToString();
                    _rabbitThirstThresholdSlider.wholeNumbers = true;
                    _rabbitThirstThresholdSlider.minValue = 0;
                    _rabbitThirstThresholdSlider.maxValue = _rabbitThirstThresholdReset * 10;
                    _rabbitThirstThresholdSlider.value = _rabbitThirstThresholdReset;

                    _rabbitThirstIncreaseBaseText.text = _rabbitThirstIncreaseBaseReset.ToString();
                    _rabbitThirstIncreaseBaseSlider.wholeNumbers = false;
                    _rabbitThirstIncreaseBaseSlider.minValue = 0;
                    _rabbitThirstIncreaseBaseSlider.maxValue = _rabbitThirstIncreaseBaseReset * 10;
                    _rabbitThirstIncreaseBaseSlider.value = _rabbitThirstIncreaseBaseReset;

                    //rabbitThirstIncreaseYoungText.text = rabbiReset.ToString(); 
                    //rabbitThirstIncreaseYoungSlider.value = rabbit ;

                    //rabbitThirstIncreaseAdultText.text =  rabbiReset.ToString(); 
                    //rabbitThirstIncreaseAdultSlider.value = rabbit ;

                    //rabbitThirstIncreaseOldText.text = rabbiReset.ToString(); 
                    //rabbitThirstIncreaseOldSlider.value = rabbit ;

                    _rabbitDrinkingSpeedText.text = _rabbitDrinkingSpeedReset.ToString();
                    _rabbitDrinkingSpeedSlider.wholeNumbers = true;
                    _rabbitDrinkingSpeedSlider.minValue = 0;
                    _rabbitDrinkingSpeedSlider.maxValue = _rabbitDrinkingSpeedReset * 10;
                    _rabbitDrinkingSpeedSlider.value = _rabbitDrinkingSpeedReset;

                    _rabbitMatingDurationText.text = _rabbitMatingDurationReset.ToString();
                    _rabbitMatingDurationSlider.wholeNumbers = true;
                    _rabbitMatingDurationSlider.minValue = 0;
                    _rabbitMatingDurationSlider.maxValue = _rabbitMatingDurationReset * 10;
                    _rabbitMatingDurationSlider.value = _rabbitMatingDurationReset;

                    _rabbitMatingThresholdText.text = _rabbitMatingThresholdReset.ToString();
                    _rabbitMatingThresholdSlider.wholeNumbers = true;
                    _rabbitMatingThresholdSlider.minValue = 0;
                    _rabbitMatingThresholdSlider.maxValue = _rabbitMatingThresholdReset * 10;
                    _rabbitMatingThresholdSlider.value = _rabbitMatingThresholdReset;

                    _rabbitReproductiveUrgeIncreaseText.text = _rabbitReproductiveUrgeIncreaseReset.ToString();
                    _rabbitReproductiveUrgeIncreaseSlider.wholeNumbers = false;
                    _rabbitReproductiveUrgeIncreaseSlider.minValue = 0;
                    _rabbitReproductiveUrgeIncreaseSlider.maxValue = _rabbitReproductiveUrgeIncreaseReset * 10;
                    _rabbitReproductiveUrgeIncreaseSlider.value = _rabbitReproductiveUrgeIncreaseReset;

                    _rabbitPregnancyLengthText.text = _rabbitPregnancyLengthReset.ToString();
                    _rabbitPregnancyLengthSlider.wholeNumbers = true;
                    _rabbitPregnancyLengthSlider.minValue = 0;
                    _rabbitPregnancyLengthSlider.maxValue = _rabbitPregnancyLengthReset * 10;
                    _rabbitPregnancyLengthSlider.value = _rabbitPregnancyLengthReset;

                    _rabbitBirthDurationText.text = _rabbitBirthDurationReset.ToString();
                    _rabbitBirthDurationSlider.wholeNumbers = true;
                    _rabbitBirthDurationSlider.minValue = 0;
                    _rabbitBirthDurationSlider.maxValue = _rabbitBirthDurationReset * 10;
                    _rabbitBirthDurationSlider.value = _rabbitBirthDurationReset;

                    _rabbitLitterSizeMinText.text = _rabbitLitterSizeMinReset.ToString();
                    _rabbitLitterSizeMinSlider.wholeNumbers = true;
                    _rabbitLitterSizeMinSlider.minValue = 0;
                    _rabbitLitterSizeMinSlider.maxValue = _rabbitLitterSizeMinReset * 10;
                    _rabbitLitterSizeMinSlider.value = _rabbitLitterSizeMinReset;

                    _rabbitLitterSizeMaxText.text = _rabbitLitterSizeMaxReset.ToString();
                    _rabbitLitterSizeMaxSlider.wholeNumbers = true;
                    _rabbitLitterSizeMaxSlider.minValue = 0;
                    _rabbitLitterSizeMaxSlider.maxValue = _rabbitLitterSizeMaxReset * 10;
                    _rabbitLitterSizeMaxSlider.value = _rabbitLitterSizeMaxReset;

                    _rabbitLitterSizeAveText.text = _rabbitLitterSizeAveReset.ToString();
                    _rabbitLitterSizeAveSlider.wholeNumbers = true;
                    _rabbitLitterSizeAveSlider.minValue = 0;
                    _rabbitLitterSizeAveSlider.maxValue = _rabbitLitterSizeAveReset * 10;
                    _rabbitLitterSizeAveSlider.value = _rabbitLitterSizeAveReset;

                    _rabbitMovementSpeedText.text = _rabbitMovementSpeedReset.ToString();
                    _rabbitMovementSpeedSlider.wholeNumbers = true;
                    _rabbitMovementSpeedSlider.minValue = 0;
                    _rabbitMovementSpeedSlider.maxValue = _rabbitMovementSpeedReset * 10;
                    _rabbitMovementSpeedSlider.value = _rabbitMovementSpeedReset;

                    _rabbitMovementMultiplierBaseText.text = _rabbitMovementMultiplierBaseReset.ToString();
                    _rabbitMovementMultiplierBaseSlider.wholeNumbers = false;
                    _rabbitMovementMultiplierBaseSlider.minValue = 0;
                    _rabbitMovementMultiplierBaseSlider.maxValue = _rabbitMovementMultiplierBaseReset * 10;
                    _rabbitMovementMultiplierBaseSlider.value = _rabbitMovementMultiplierBaseReset;

                    _rabbitMovementMultiplierYoungText.text = _rabbitMovementMultiplierYoungReset.ToString();
                    _rabbitMovementMultiplierYoungSlider.wholeNumbers = false;
                    _rabbitMovementMultiplierYoungSlider.minValue = 0;
                    _rabbitMovementMultiplierYoungSlider.maxValue = _rabbitMovementMultiplierYoungReset * 10;
                    _rabbitMovementMultiplierYoungSlider.value = _rabbitMovementMultiplierYoungReset;

                    _rabbitMovementMultiplierAdultText.text = _rabbitMovementMultiplierAdultReset.ToString();
                    _rabbitMovementMultiplierAdultSlider.wholeNumbers = false;
                    _rabbitMovementMultiplierAdultSlider.minValue = 0;
                    _rabbitMovementMultiplierAdultSlider.maxValue = _rabbitMovementMultiplierAdultReset * 10;
                    _rabbitMovementMultiplierAdultSlider.value = _rabbitMovementMultiplierAdultReset;

                    _rabbitMovementMultiplierOldText.text = _rabbitMovementMultiplierOldReset.ToString();
                    _rabbitMovementMultiplierOldSlider.wholeNumbers = false;
                    _rabbitMovementMultiplierOldSlider.minValue = 0;
                    _rabbitMovementMultiplierOldSlider.maxValue = _rabbitMovementMultiplierOldReset * 10;
                    _rabbitMovementMultiplierOldSlider.value = _rabbitMovementMultiplierOldReset;

                    _rabbitMovementMultiplierPregnantText.text = _rabbitMovementMultiplierPregnantReset.ToString();
                    _rabbitMovementMultiplierPregnantSlider.wholeNumbers = false;
                    _rabbitMovementMultiplierPregnantSlider.minValue = 0;
                    _rabbitMovementMultiplierPregnantSlider.maxValue = _rabbitMovementMultiplierPregnantReset * 10;
                    _rabbitMovementMultiplierPregnantSlider.value = _rabbitMovementMultiplierPregnantReset;

                    _rabbitSightRadiusText.text = _rabbitSightRadiusReset.ToString();
                    _rabbitSightRadiusSlider.wholeNumbers = true;
                    _rabbitSightRadiusSlider.minValue = 0;
                    _rabbitSightRadiusSlider.maxValue = _rabbitSightRadiusReset * 10;
                    _rabbitSightRadiusSlider.value = _rabbitSightRadiusReset;

                    _rabbitSizeMaleText.text = _rabbitSizeMaleReset.ToString();
                    _rabbitSizeMaleSlider.wholeNumbers = true;
                    _rabbitSizeMaleSlider.minValue = 0;
                    _rabbitSizeMaleSlider.maxValue = _rabbitSizeMaleReset * 10;
                    _rabbitSizeMaleSlider.value = _rabbitSizeMaleReset;

                    _rabbitSizeFemaleText.text = _rabbitSizeFemaleReset.ToString();
                    _rabbitSizeFemaleSlider.wholeNumbers = true;
                    _rabbitSizeFemaleSlider.minValue = 0;
                    _rabbitSizeFemaleSlider.maxValue = _rabbitSizeFemaleReset * 10;
                    _rabbitSizeFemaleSlider.value = _rabbitSizeFemaleReset;



                    _foxAgeMaxText.text = _foxAgeMaxReset.ToString();
                    _foxAgeMaxSlider.wholeNumbers = true;
                    _foxAgeMaxSlider.minValue = 0;
                    _foxAgeMaxSlider.maxValue = _foxAgeMaxReset * 10;
                    _foxAgeMaxSlider.value = _foxAgeMaxReset;

                    _foxNutritionalValueText.text = _foxNutritionalValueReset.ToString();
                    _foxNutritionalValueSlider.wholeNumbers = true;
                    _foxNutritionalValueSlider.minValue = 0;
                    _foxNutritionalValueSlider.maxValue = _foxNutritionalValueReset * 10;
                    _foxNutritionalValueSlider.value = _foxNutritionalValueReset;

                    _foxCanBeEaten.isOn = _foxCanBeEatenReset;

                    _foxHungerMaxText.text = _foxHungerMaxReset.ToString();
                    _foxHungerMaxSlider.wholeNumbers = true;
                    _foxHungerMaxSlider.minValue = 0;
                    _foxHungerMaxSlider.maxValue = _foxHungerMaxReset * 10;
                    _foxHungerMaxSlider.value = _foxHungerMaxReset;

                    _foxHungerThresholdText.text = _foxHungerThresholdReset.ToString();
                    _foxHungerThresholdSlider.wholeNumbers = true;
                    _foxHungerThresholdSlider.minValue = 0;
                    _foxHungerThresholdSlider.maxValue = _foxHungerThresholdReset * 10;
                    _foxHungerThresholdSlider.value = _foxHungerThresholdReset;

                    _foxHungerIncreaseBaseText.text = _foxHungerIncreaseBaseReset.ToString();
                    _foxHungerIncreaseBaseSlider.wholeNumbers = false;
                    _foxHungerIncreaseBaseSlider.minValue = 0;
                    _foxHungerIncreaseBaseSlider.maxValue = _foxHungerIncreaseBaseReset * 10;
                    _foxHungerIncreaseBaseSlider.value = _foxHungerIncreaseBaseReset;

                    _foxHungerIncreaseYoungText.text = _foxHungerIncreaseYoungReset.ToString();
                    _foxHungerIncreaseYoungSlider.wholeNumbers = false;
                    _foxHungerIncreaseYoungSlider.minValue = 0;
                    _foxHungerIncreaseYoungSlider.maxValue = _foxHungerIncreaseYoungReset * 10;
                    _foxHungerIncreaseYoungSlider.value = _foxHungerIncreaseYoungReset;

                    _foxHungerIncreaseAdultText.text = _foxHungerIncreaseAdultReset.ToString();
                    _foxHungerIncreaseAdultSlider.wholeNumbers = false;
                    _foxHungerIncreaseAdultSlider.minValue = 0;
                    _foxHungerIncreaseAdultSlider.maxValue = _foxHungerIncreaseAdultReset * 10;
                    _foxHungerIncreaseAdultSlider.value = _foxHungerIncreaseAdultReset;

                    _foxHungerIncreaseOldText.text = _foxHungerIncreaseOldReset.ToString();
                    _foxHungerIncreaseOldSlider.wholeNumbers = false;
                    _foxHungerIncreaseOldSlider.minValue = 0;
                    _foxHungerIncreaseOldSlider.maxValue = _foxHungerIncreaseOldReset * 10;
                    _foxHungerIncreaseOldSlider.value = _foxHungerIncreaseOldReset;

                    _foxEatingSpeedText.text = _foxEatingSpeedReset.ToString();
                    _foxEatingSpeedSlider.wholeNumbers = true;
                    _foxEatingSpeedSlider.minValue = 0;
                    _foxEatingSpeedSlider.maxValue = _foxEatingSpeedReset * 10;
                    _foxEatingSpeedSlider.value = _foxEatingSpeedReset;

                    _foxThirstMaxText.text = _foxThirstMaxReset.ToString();
                    _foxThirstMaxSlider.wholeNumbers = true;
                    _foxThirstMaxSlider.minValue = 0;
                    _foxThirstMaxSlider.maxValue = _foxThirstMaxReset * 10;
                    _foxThirstMaxSlider.value = _foxThirstMaxReset;

                    _foxThirstThresholdText.text = _foxThirstThresholdReset.ToString();
                    _foxThirstThresholdSlider.wholeNumbers = true;
                    _foxThirstThresholdSlider.minValue = 0;
                    _foxThirstThresholdSlider.maxValue = _foxThirstThresholdReset * 10;
                    _foxThirstThresholdSlider.value = _foxThirstThresholdReset;

                    _foxThirstIncreaseBaseText.text = _foxThirstIncreaseBaseReset.ToString();
                    _foxThirstIncreaseBaseSlider.wholeNumbers = false;
                    _foxThirstIncreaseBaseSlider.minValue = 0;
                    _foxThirstIncreaseBaseSlider.maxValue = _foxThirstIncreaseBaseReset * 10;
                    _foxThirstIncreaseBaseSlider.value = _foxThirstIncreaseBaseReset;

                    //foxThirstIncreaseYoungText.text = foxEset.ToString(); 
                    //foxThirstIncreaseYoungSlider.value = fox ;

                    //foxThirstIncreaseAdultText.text = foxEset.ToString(); 
                    //foxThirstIncreaseAdultSlider.value = fox ;

                    //foxThirstIncreaseOldText.text = foxReset.ToString(); 
                    //foxThirstIncreaseOldSlider.value = fox ;

                    _foxDrinkingSpeedText.text = _foxDrinkingSpeedReset.ToString();
                    _foxDrinkingSpeedSlider.wholeNumbers = true;
                    _foxDrinkingSpeedSlider.minValue = 0;
                    _foxDrinkingSpeedSlider.maxValue = _foxDrinkingSpeedReset * 10;
                    _foxDrinkingSpeedSlider.value = _foxDrinkingSpeedReset;

                    _foxMatingDurationText.text = _foxMatingDurationReset.ToString();
                    _foxMatingDurationSlider.wholeNumbers = true;
                    _foxMatingDurationSlider.minValue = 0;
                    _foxMatingDurationSlider.maxValue = _foxMatingDurationReset * 10;
                    _foxMatingDurationSlider.value = _foxMatingDurationReset;

                    _foxMatingThresholdText.text = _foxMatingThresholdReset.ToString();
                    _foxMatingThresholdSlider.wholeNumbers = true;
                    _foxMatingThresholdSlider.minValue = 0;
                    _foxMatingThresholdSlider.maxValue = _foxMatingThresholdReset * 10;
                    _foxMatingThresholdSlider.value = _foxMatingThresholdReset;

                    _foxReproductiveUrgeIncreaseText.text = _foxReproductiveUrgeIncreaseReset.ToString();
                    _foxReproductiveUrgeIncreaseSlider.wholeNumbers = false;
                    _foxReproductiveUrgeIncreaseSlider.minValue = 0;
                    _foxReproductiveUrgeIncreaseSlider.maxValue = _foxReproductiveUrgeIncreaseReset * 10;
                    _foxReproductiveUrgeIncreaseSlider.value = _foxReproductiveUrgeIncreaseReset;

                    _foxPregnancyLengthText.text = _foxPregnancyLengthReset.ToString();
                    _foxPregnancyLengthSlider.wholeNumbers = true;
                    _foxPregnancyLengthSlider.minValue = 0;
                    _foxPregnancyLengthSlider.maxValue = _foxPregnancyLengthReset * 10;
                    _foxPregnancyLengthSlider.value = _foxPregnancyLengthReset;

                    _foxBirthDurationText.text = _foxBirthDurationReset.ToString();
                    _foxBirthDurationSlider.wholeNumbers = true;
                    _foxBirthDurationSlider.minValue = 0;
                    _foxBirthDurationSlider.maxValue = _foxBirthDurationReset * 10;
                    _foxBirthDurationSlider.value = _foxBirthDurationReset;

                    _foxLitterSizeMinText.text = _foxLitterSizeMinReset.ToString();
                    _foxLitterSizeMinSlider.wholeNumbers = true;
                    _foxLitterSizeMinSlider.minValue = 0;
                    _foxLitterSizeMinSlider.maxValue = _foxLitterSizeMinReset * 10;
                    _foxLitterSizeMinSlider.value = _foxLitterSizeMinReset;

                    _foxLitterSizeMaxText.text = _foxLitterSizeMaxReset.ToString();
                    _foxLitterSizeMaxSlider.wholeNumbers = true;
                    _foxLitterSizeMaxSlider.minValue = 0;
                    _foxLitterSizeMaxSlider.maxValue = _foxLitterSizeMaxReset * 10;
                    _foxLitterSizeMaxSlider.value = _foxLitterSizeMaxReset;

                    _foxLitterSizeAveText.text = _foxLitterSizeAveReset.ToString();
                    _foxLitterSizeAveSlider.wholeNumbers = true;
                    _foxLitterSizeAveSlider.minValue = 0;
                    _foxLitterSizeAveSlider.maxValue = _foxLitterSizeAveReset * 10;
                    _foxLitterSizeAveSlider.value = _foxLitterSizeAveReset;

                    _foxMovementSpeedText.text = _foxMovementSpeedReset.ToString();
                    _foxMovementSpeedSlider.wholeNumbers = true;
                    _foxMovementSpeedSlider.minValue = 0;
                    _foxMovementSpeedSlider.maxValue = _foxMovementSpeedReset * 10;
                    _foxMovementSpeedSlider.value = _foxMovementSpeedReset;

                    _foxMovementMultiplierBaseText.text = _foxMovementMultiplierBaseReset.ToString();
                    _foxMovementMultiplierBaseSlider.wholeNumbers = false;
                    _foxMovementMultiplierBaseSlider.minValue = 0;
                    _foxMovementMultiplierBaseSlider.maxValue = _foxMovementMultiplierBaseReset * 10;
                    _foxMovementMultiplierBaseSlider.value = _foxMovementMultiplierBaseReset;

                    _foxMovementMultiplierYoungText.text = _foxMovementMultiplierYoungReset.ToString();
                    _foxMovementMultiplierYoungSlider.wholeNumbers = false;
                    _foxMovementMultiplierYoungSlider.minValue = 0;
                    _foxMovementMultiplierYoungSlider.maxValue = _foxMovementMultiplierYoungReset * 10;
                    _foxMovementMultiplierYoungSlider.value = _foxMovementMultiplierYoungReset;

                    _foxMovementMultiplierAdultText.text = _foxMovementMultiplierAdultReset.ToString();
                    _foxMovementMultiplierAdultSlider.wholeNumbers = false;
                    _foxMovementMultiplierAdultSlider.minValue = 0;
                    _foxMovementMultiplierAdultSlider.maxValue = _foxMovementMultiplierAdultReset * 10;
                    _foxMovementMultiplierAdultSlider.value = _foxMovementMultiplierAdultReset;

                    _foxMovementMultiplierOldText.text = _foxMovementMultiplierOldReset.ToString();
                    _foxMovementMultiplierOldSlider.wholeNumbers = false;
                    _foxMovementMultiplierOldSlider.minValue = 0;
                    _foxMovementMultiplierOldSlider.maxValue = _foxMovementMultiplierOldReset * 10;
                    _foxMovementMultiplierOldSlider.value = _foxMovementMultiplierOldReset;

                    _foxMovementMultiplierPregnantText.text = _foxMovementMultiplierPregnantReset.ToString();
                    _foxMovementMultiplierPregnantSlider.wholeNumbers = false;
                    _foxMovementMultiplierPregnantSlider.minValue = 0;
                    _foxMovementMultiplierPregnantSlider.maxValue = _foxMovementMultiplierPregnantReset * 10;
                    _foxMovementMultiplierPregnantSlider.value = _foxMovementMultiplierPregnantReset;

                    _foxSightRadiusText.text = _foxSightRadiusReset.ToString();
                    _foxSightRadiusSlider.wholeNumbers = true;
                    _foxSightRadiusSlider.minValue = 0;
                    _foxSightRadiusSlider.maxValue = _foxSightRadiusReset * 10;
                    _foxSightRadiusSlider.value = _foxSightRadiusReset;

                    _foxSizeMaleText.text = _foxSizeMaleReset.ToString();
                    _foxSizeMaleSlider.wholeNumbers = true;
                    _foxSizeMaleSlider.minValue = 0;
                    _foxSizeMaleSlider.maxValue = _foxSizeMaleReset * 10;
                    _foxSizeMaleSlider.value = _foxSizeMaleReset;

                    _foxSizeFemaleText.text = _foxSizeFemaleReset.ToString();
                    _foxSizeFemaleSlider.wholeNumbers = true;
                    _foxSizeFemaleSlider.minValue = 0;
                    _foxSizeFemaleSlider.maxValue = _foxSizeFemaleReset * 10;
                    _foxSizeFemaleSlider.value = _foxSizeFemaleReset;



                    _grassNutritionalValueText.text = _grassNutritionalValueReset.ToString();
                    _grassNutritionalValueSlider.value = _grassNutritionalValueReset;

                    _grassCanBeEaten.isOn = _grassCanBeEatenReset;

                    _grassSizeText.text = _grassSizeReset.ToString();
                    _grassSizeSlider.value = _grassSizeReset;

                    InitialPropertiesApply();
                    break;
                default:
                    Debug.Log("menu to reset doesn't exist in switch statement.");
                    break;
            }
        }
        #endregion

        #region Dialog Options - This is where we load what has been saved in player prefs!
        /*Confirmation of if the user wants to load simulation with the current options*/
        public void ClickNewGameDialog(string buttonType)
        {
            if (buttonType == "Yes")
            {
                //if no map loaded set to default map
                if (_fileContents == null)
                    SimulationManager.MapPath = Application.dataPath + "/MapDefault.txt";
                SceneManager.LoadScene(NewGameButtonLevel);
            }

            if (buttonType == "No")
            {
                GoBackToMainMenu();
            }
        }

        /*loads the selected xml of the defaults, sets the slider values where applicable*/
        public void LoadGame()
        {
            string[] filePaths = StandaloneFileBrowser.StandaloneFileBrowser.OpenFilePanel("Open File", "", "xml", false);
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
                XmlNodeList diet = xmlDocument.GetElementsByTagName("DietType");
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
                XmlNodeList previousFlagState = xmlDocument.GetElementsByTagName("FlagStatePrevious");
                XmlNodeList deathReason = xmlDocument.GetElementsByTagName("DeathReason");
                XmlNodeList beenEaten = xmlDocument.GetElementsByTagName("beenEaten");
                XmlNodeList touchRadius = xmlDocument.GetElementsByTagName("touchRadius");
                XmlNodeList sightRadius = xmlDocument.GetElementsByTagName("sightRadius");
                XmlNodeList shortestToEdibleDistance = xmlDocument.GetElementsByTagName("shortestToEdibleDistance");
                XmlNodeList shortestToWaterDistance = xmlDocument.GetElementsByTagName("shortestToWaterDistance");
                XmlNodeList shortestToPredatorDistance = xmlDocument.GetElementsByTagName("shortestToPredatorDistance");
                XmlNodeList shortestToMateDistance = xmlDocument.GetElementsByTagName("shortestToMateDistance");
                XmlNodeList colliderType = xmlDocument.GetElementsByTagName("Colliderollider");


                #endregion



                #region rabbit default

                RabbitDefaults.Age = float.Parse(age[0].InnerText);
                RabbitDefaults.AgeIncrease = float.Parse(ageIncrease[0].InnerText);
                _rabbitAgeMaxSlider.value = float.Parse(ageMax[0].InnerText);
                RabbitDefaults.AgeGroup = (BioStatsData.AgeGroups)Enum.Parse(typeof(BioStatsData.AgeGroups), ageGroup[0].InnerText);
                RabbitDefaults.AdultEntryTimer = float.Parse(adultEntryTimer[0].InnerText);
                RabbitDefaults.OldEntryTimer = float.Parse(oldEntryTimer[0].InnerText);
                _rabbitNutritionalValueSlider.value = float.Parse(nutritionalValue[0].InnerText);
                _rabbitCanBeEaten.isOn = bool.Parse(canBeEaten[0].InnerText);
                RabbitDefaults.NutritionalValueMultiplier = float.Parse(nutritionalValueMultiplier[0].InnerText);
                RabbitDefaults.FoodType = (EdibleData.FoodTypes)Enum.Parse(typeof(EdibleData.FoodTypes), foodType[0].InnerText);
                RabbitDefaults.Hunger = float.Parse(hunger[0].InnerText);
                _rabbitHungerMaxSlider.value = float.Parse(hungerMax[0].InnerText);
                _rabbitHungerThresholdSlider.value = float.Parse(hungryThreshold[0].InnerText);
                _rabbitHungerIncreaseBaseSlider.value = float.Parse(hungerIncrease[0].InnerText);
                RabbitDefaults.PregnancyHungerIncrease = float.Parse(pregnancyHungerIncrease[0].InnerText);
                _rabbitHungerIncreaseYoungSlider.value = float.Parse(youngHungerIncrease[0].InnerText);
                _rabbitHungerIncreaseAdultSlider.value = float.Parse(adultHungerIncrease[0].InnerText);
                _rabbitHungerIncreaseOldSlider.value = float.Parse(oldHungerIncrease[0].InnerText);
                _rabbitEatingSpeedSlider.value = float.Parse(eatingSpeed[0].InnerText);
                RabbitDefaults.Diet = (BasicNeedsData.DietType)Enum.Parse(typeof(BasicNeedsData.DietType), diet[0].InnerText);
                RabbitDefaults.Thirst = float.Parse(thirst[0].InnerText);
                _rabbitThirstMaxSlider.value = float.Parse(thirstMax[0].InnerText);
                _rabbitThirstThresholdSlider.value = float.Parse(thirstyThreshold[0].InnerText);
                _rabbitThirstIncreaseBaseSlider.value = float.Parse(thirstIncrease[0].InnerText);
                _rabbitDrinkingSpeedSlider.value = float.Parse(drinkingSpeed[0].InnerText);
                RabbitDefaults.MateStartTime = float.Parse(mateStartTime[0].InnerText);
                _rabbitMatingDurationSlider.value = float.Parse(matingDuration[0].InnerText);
                RabbitDefaults.ReproductiveUrge = float.Parse(reproductiveUrge[0].InnerText);
                _rabbitReproductiveUrgeIncreaseSlider.value = float.Parse(reproductiveUrgeIncreaseMale[0].InnerText);
                RabbitDefaults.ReproductiveUrgeIncreaseFemale = float.Parse(reproductiveUrgeIncreaseFemale[0].InnerText);
                _rabbitMatingThresholdSlider.value = float.Parse(matingThreshold[0].InnerText);
                RabbitDefaults.PregnancyStartTime = float.Parse(pregnancyStartTime[0].InnerText);
                RabbitDefaults.BabiesBorn = int.Parse(babiesBorn[0].InnerText);
                RabbitDefaults.BirthStartTime = float.Parse(birthStartTime[0].InnerText);
                RabbitDefaults.CurrentLitterSize = int.Parse(currentLitterSize[0].InnerText);
                RabbitDefaults.PregnancyLengthModifier = float.Parse(pregnancyLengthModifier[0].InnerText);
                _rabbitPregnancyLengthSlider.value = float.Parse(pregnancyLength[0].InnerText);
                _rabbitBirthDurationSlider.value = float.Parse(birthDuration[0].InnerText);
                _rabbitLitterSizeMinSlider.value = int.Parse(litterSizeMin[0].InnerText);
                _rabbitLitterSizeMaxSlider.value = int.Parse(litterSizeMax[0].InnerText);
                _rabbitLitterSizeAveSlider.value = int.Parse(litterSizeAve[0].InnerText);
                _rabbitMovementSpeedSlider.value = float.Parse(moveSpeed[0].InnerText);
                RabbitDefaults.RotationSpeed = float.Parse(rotationSpeed[0].InnerText);
                RabbitDefaults.MoveMultiplier = float.Parse(moveMultiplier[0].InnerText);
                _rabbitMovementMultiplierPregnantSlider.value = float.Parse(pregnancyMoveMultiplier[0].InnerText);
                _rabbitMovementMultiplierBaseSlider.value = float.Parse(originalMoveMultiplier[0].InnerText);
                _rabbitMovementMultiplierYoungSlider.value = float.Parse(youngMoveMultiplier[0].InnerText);
                _rabbitMovementMultiplierAdultSlider.value = float.Parse(adultMoveMultiplier[0].InnerText);
                _rabbitMovementMultiplierOldSlider.value = float.Parse(oldMoveMultiplier[0].InnerText);
                RabbitDefaults.SizeMultiplier = float.Parse(sizeMultiplier[0].InnerText);
                _rabbitSizeMaleSlider.value = float.Parse(scaleMale[0].InnerText);
                _rabbitSizeFemaleSlider.value = float.Parse(scaleFemale[0].InnerText);
                RabbitDefaults.YoungSizeMultiplier = float.Parse(youngSizeMultiplier[0].InnerText);
                RabbitDefaults.AdultSizeMultiplier = float.Parse(adultSizeMultiplier[0].InnerText);
                RabbitDefaults.OldSizeMultiplier = float.Parse(oldSizeMultiplier[0].InnerText);
                RabbitDefaults.FlagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), flagState[0].InnerText);
                RabbitDefaults.FlagStatePrevious = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), previousFlagState[0].InnerText);
                RabbitDefaults.DeathReason = (StateData.DeathReasons)Enum.Parse(typeof(StateData.DeathReasons), deathReason[0].InnerText);
                RabbitDefaults.BeenEaten = bool.Parse(beenEaten[0].InnerText);
                RabbitDefaults.TouchRadius = float.Parse(touchRadius[0].InnerText);
                _rabbitSightRadiusSlider.value = float.Parse(sightRadius[0].InnerText);
                RabbitDefaults.ShortestToEdibleDistance = float.Parse(shortestToEdibleDistance[0].InnerText);
                RabbitDefaults.ShortestToWaterDistance = float.Parse(shortestToWaterDistance[0].InnerText);
                RabbitDefaults.ShortestToPredatorDistance = float.Parse(shortestToPredatorDistance[0].InnerText);
                RabbitDefaults.ShortestToMateDistance = float.Parse(shortestToMateDistance[0].InnerText);
                RabbitDefaults.Collider = (ColliderTypeData.ColliderType)Enum.Parse(typeof(ColliderTypeData.ColliderType), colliderType[0].InnerText);

                #endregion


                #region fox default

                FoxDefaults.Age = float.Parse(age[1].InnerText);
                FoxDefaults.AgeIncrease = float.Parse(ageIncrease[1].InnerText);
                _foxAgeMaxSlider.value = float.Parse(ageMax[1].InnerText);
                FoxDefaults.AgeGroup = (BioStatsData.AgeGroups)Enum.Parse(typeof(BioStatsData.AgeGroups), ageGroup[1].InnerText);
                FoxDefaults.AdultEntryTimer = float.Parse(adultEntryTimer[1].InnerText);
                FoxDefaults.OldEntryTimer = float.Parse(oldEntryTimer[1].InnerText);
                _foxNutritionalValueSlider.value = float.Parse(nutritionalValue[1].InnerText);
                _foxCanBeEaten.isOn = bool.Parse(canBeEaten[1].InnerText);
                FoxDefaults.NutritionalValueMultiplier = float.Parse(nutritionalValueMultiplier[1].InnerText);
                FoxDefaults.FoodType = (EdibleData.FoodTypes)Enum.Parse(typeof(EdibleData.FoodTypes), foodType[1].InnerText);
                FoxDefaults.Hunger = float.Parse(hunger[1].InnerText);
                _foxHungerMaxSlider.value = float.Parse(hungerMax[1].InnerText);
                _foxHungerThresholdSlider.value = float.Parse(hungryThreshold[1].InnerText);
                _foxHungerIncreaseBaseSlider.value = float.Parse(hungerIncrease[1].InnerText);
                FoxDefaults.PregnancyHungerIncrease = float.Parse(pregnancyHungerIncrease[1].InnerText);
                _foxHungerIncreaseYoungSlider.value = float.Parse(youngHungerIncrease[1].InnerText);
                _foxHungerIncreaseAdultSlider.value = float.Parse(adultHungerIncrease[1].InnerText);
                _foxHungerIncreaseOldSlider.value = float.Parse(oldHungerIncrease[1].InnerText);
                _foxEatingSpeedSlider.value = float.Parse(eatingSpeed[1].InnerText);
                FoxDefaults.Diet = (BasicNeedsData.DietType)Enum.Parse(typeof(BasicNeedsData.DietType), diet[1].InnerText);
                FoxDefaults.Thirst = float.Parse(thirst[1].InnerText);
                _foxThirstMaxSlider.value = float.Parse(thirstMax[1].InnerText);
                _foxThirstThresholdSlider.value = float.Parse(thirstyThreshold[1].InnerText);
                _foxThirstIncreaseBaseSlider.value = float.Parse(thirstIncrease[1].InnerText);
                _foxDrinkingSpeedSlider.value = float.Parse(drinkingSpeed[1].InnerText);
                FoxDefaults.MateStartTime = float.Parse(mateStartTime[1].InnerText);
                _foxMatingDurationSlider.value = float.Parse(matingDuration[1].InnerText);
                FoxDefaults.ReproductiveUrge = float.Parse(reproductiveUrge[1].InnerText);
                _foxReproductiveUrgeIncreaseSlider.value = float.Parse(reproductiveUrgeIncreaseMale[1].InnerText);
                FoxDefaults.ReproductiveUrgeIncreaseFemale = float.Parse(reproductiveUrgeIncreaseFemale[1].InnerText);
                _foxMatingThresholdSlider.value = float.Parse(matingThreshold[1].InnerText);
                FoxDefaults.PregnancyStartTime = float.Parse(pregnancyStartTime[1].InnerText);
                FoxDefaults.BabiesBorn = int.Parse(babiesBorn[1].InnerText);
                FoxDefaults.BirthStartTime = float.Parse(birthStartTime[1].InnerText);
                FoxDefaults.CurrentLitterSize = int.Parse(currentLitterSize[1].InnerText);
                FoxDefaults.PregnancyLengthModifier = float.Parse(pregnancyLengthModifier[1].InnerText);
                _foxPregnancyLengthSlider.value = float.Parse(pregnancyLength[1].InnerText);
                _foxBirthDurationSlider.value = float.Parse(birthDuration[1].InnerText);
                _foxLitterSizeMinSlider.value = int.Parse(litterSizeMin[1].InnerText);
                _foxLitterSizeMaxSlider.value = int.Parse(litterSizeMax[1].InnerText);
                _foxLitterSizeAveSlider.value = int.Parse(litterSizeAve[1].InnerText);
                _foxMovementSpeedSlider.value = float.Parse(moveSpeed[1].InnerText);
                FoxDefaults.RotationSpeed = float.Parse(rotationSpeed[1].InnerText);
                FoxDefaults.MoveMultiplier = float.Parse(moveMultiplier[1].InnerText);
                _foxMovementMultiplierPregnantSlider.value = float.Parse(pregnancyMoveMultiplier[1].InnerText);
                _foxMovementMultiplierBaseSlider.value = float.Parse(originalMoveMultiplier[1].InnerText);
                _foxMovementMultiplierYoungSlider.value = float.Parse(youngMoveMultiplier[1].InnerText);
                _foxMovementMultiplierAdultSlider.value = float.Parse(adultMoveMultiplier[1].InnerText);
                _foxMovementMultiplierOldSlider.value = float.Parse(oldMoveMultiplier[1].InnerText);
                FoxDefaults.SizeMultiplier = float.Parse(sizeMultiplier[1].InnerText);
                _foxSizeMaleSlider.value = float.Parse(scaleMale[1].InnerText);
                _foxSizeFemaleSlider.value = float.Parse(scaleFemale[1].InnerText);
                FoxDefaults.YoungSizeMultiplier = float.Parse(youngSizeMultiplier[1].InnerText);
                FoxDefaults.AdultSizeMultiplier = float.Parse(adultSizeMultiplier[1].InnerText);
                FoxDefaults.OldSizeMultiplier = float.Parse(oldSizeMultiplier[1].InnerText);
                FoxDefaults.FlagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), flagState[1].InnerText);
                FoxDefaults.PreviousFlagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), previousFlagState[1].InnerText);
                FoxDefaults.DeathReason = (StateData.DeathReasons)Enum.Parse(typeof(StateData.DeathReasons), deathReason[1].InnerText);
                FoxDefaults.BeenEaten = bool.Parse(beenEaten[1].InnerText);
                FoxDefaults.TouchRadius = float.Parse(touchRadius[1].InnerText);
                _foxSightRadiusSlider.value = float.Parse(sightRadius[1].InnerText);
                FoxDefaults.ShortestToEdibleDistance = float.Parse(shortestToEdibleDistance[1].InnerText);
                FoxDefaults.ShortestToWaterDistance = float.Parse(shortestToWaterDistance[1].InnerText);
                FoxDefaults.ShortestToPredatorDistance = float.Parse(shortestToPredatorDistance[1].InnerText);
                FoxDefaults.ShortestToMateDistance = float.Parse(shortestToMateDistance[1].InnerText);
                FoxDefaults.Collider = (ColliderTypeData.ColliderType)Enum.Parse(typeof(ColliderTypeData.ColliderType), colliderType[1].InnerText);


                #endregion

                #region GrassData

                _grassNutritionalValueSlider.value = float.Parse(nutritionalValue[2].InnerText);

                GrassDefaults.NutritionalValueMultiplier = float.Parse(nutritionalValueMultiplier[2].InnerText);
                _grassCanBeEaten.isOn = bool.Parse(canBeEaten[2].InnerText);

                GrassDefaults.SizeMultiplier = float.Parse(sizeMultiplier[2].InnerText);

                XmlNodeList scale = xmlDocument.GetElementsByTagName("scale");
                _grassSizeSlider.value = float.Parse(scale[0].InnerText);

                GrassDefaults.FoodType = (EdibleData.FoodTypes)Enum.Parse(typeof(EdibleData.FoodTypes), foodType[2].InnerText);

                GrassDefaults.FlagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), flagState[2].InnerText);

                GrassDefaults.PreviousFlagState = (StateData.FlagStates)Enum.Parse(typeof(StateData.FlagStates), previousFlagState[2].InnerText);

                GrassDefaults.DeathReason = (StateData.DeathReasons)Enum.Parse(typeof(StateData.DeathReasons), deathReason[2].InnerText);

                GrassDefaults.Collider = (ColliderTypeData.ColliderType)Enum.Parse(typeof(ColliderTypeData.ColliderType), colliderType[2].InnerText);

                #endregion GrassData
                print("I am load game haha");
                InitialPropertiesApply();
            }
            else
            {
                Debug.Log("NOT FOUNDED");
            }
        }
        /*confirmation of if the user wants to try and load the selected maps*/
        public void ClickLoadGameDialog(string buttonType)
        {
            if (buttonType == "Yes")
            {
                //if the file is valid open the initial properties menu
                if (MapFileValid())
                {
                    SimulationManager.MapString = _fileContents; //set the map string in Simualtion to filecontents
                    SimulationManager.MapPath = null; // set map path to null as not using it
                    Debug.Log("I WANT TO LOAD THE MAP");
                    _loadGameDialog.SetActive(false);
                    _initialPropertiesCanvas.SetActive(true);
                    ResetButton("InitialProperties");
                    _menuNumber = MenuNumber.InitialProperties;
                }
                else
                {
                    Debug.Log("Load Game Dialog");
                    _menuDefaultCanvas.SetActive(false);
                    _loadGameDialog.SetActive(false);
                    _noSaveDialog.SetActive(true);
                }
            }

            if (buttonType == "No")
            {
                GoBackToMainMenu();
            }
        }
        #endregion

        #region Back to Menus
        public void GoBackToOptionsMenu()
        {
            _generalSettingsCanvas.SetActive(true);
            _graphicsMenu.SetActive(false);
            _soundMenu.SetActive(false);
            _gameplayMenu.SetActive(false);

            GameplayApply();
            BrightnessApply();
            VolumeApply();

            _menuNumber = MenuNumber.Options;
        }

        public void GoBackToMainMenu()
        {
            _menuDefaultCanvas.SetActive(true);
            _newGameDialog.SetActive(false);
            _loadGameDialog.SetActive(false);
            _noSaveDialog.SetActive(false);
            _generalSettingsCanvas.SetActive(false);
            _graphicsMenu.SetActive(false);
            _soundMenu.SetActive(false);
            _gameplayMenu.SetActive(false);
            _initialPropertiesCanvas.SetActive(false);
            _generateMapCanvas.SetActive(false);
            _menuNumber = MenuNumber.Main;
        }

        public void GoBackToGameplayMenu()
        {
            _controlsMenu.SetActive(false);
            _gameplayMenu.SetActive(true);
            _menuNumber = MenuNumber.Gameplay;
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
