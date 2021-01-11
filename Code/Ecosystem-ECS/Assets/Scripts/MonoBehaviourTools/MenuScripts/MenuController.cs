using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        [Header("Initial Properties Sliders")]
        [SerializeField] private Text rabbitSizeMaleText;
        [SerializeField] private Slider rabbitSizeMaleSlider;
        [Space(5)]
        [SerializeField] private Text rabbitSizeFemaleText;
        [SerializeField] private Slider rabbitSizeFemaleSlider;
        [Space(10)]
        [SerializeField] private Text rabbitSpeedText;
        [SerializeField] private Slider rabbitSpeedSlider;
        [Space(10)]
        [SerializeField] private Text rabbitHungerText;
        [SerializeField] private Slider rabbitHungerSlider;
        [Space(10)]
        [SerializeField] private Text rabbitThirstText;
        [SerializeField] private Slider rabbitThirstSlider;
        [Space(10)]
        [SerializeField] private Text rabbitAgeText;
        [SerializeField] private Slider rabbitAgeSlider;
        [Space(10)]
        [SerializeField] private Text rabbitTouchRadiusText;
        [SerializeField] private Slider rabbitTouchRadiusSlider;
        [Space(5)]
        [SerializeField] private Text rabbitSightRadiusText;
        [SerializeField] private Slider rabbitSightRadiusSlider;
        [Space(10)]
        [SerializeField] private Text rabbitPregnancyLengthText;
        [SerializeField] private Slider rabbitPregnancyLengthSlider;
        [Space(5)]
        [SerializeField] private Text rabbitMatingDurationText;
        [SerializeField] private Slider rabbitMatingDurationSlider;
        [Space(5)]
        [SerializeField] private Text rabbitBirthDurationText;
        [SerializeField] private Slider rabbitBirthDurationSlider;
        [Space(10)]
        [SerializeField] private Text rabbitLitterSizeMinText;
        [SerializeField] private Slider rabbitLitterSizeMinSlider;
        [Space(5)]
        [SerializeField] private Text rabbitLitterSizeMaxText;
        [SerializeField] private Slider rabbitLitterSizeMaxSlider;
        [Space(5)]
        [SerializeField] private Text rabbitLitterSizeAveText;
        [SerializeField] private Slider rabbitLitterSizeAveSlider;
        #endregion

        #region Initialisation - Button Selection & Menu Order
        private void Start()
        {
            menuNumber = MenuNumber.Main;
        }
        #endregion

        //MAIN SECTION
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
            var loader = new WWW(url);
            yield return loader;
            fileContents = loader.text;
            Debug.Log(fileContents);
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

        public void InitialPropertiesApply()
        {
            Debug.Log("Apply Initial Properties");
            StartCoroutine(ConfirmationBox());
        }

        public void InitialPropertiesUpdate(string propertyToUpdate)
        {
            switch (propertyToUpdate)
            {                
                case "RabbitSizeFemale":
                    rabbitSizeFemaleText.text = rabbitSizeFemaleSlider.value.ToString();
                    RabbitDefaults.scaleFemale = rabbitSizeFemaleSlider.value;
                    break;
                case "RabbitSizeMale":
                    rabbitSizeMaleText.text = rabbitSizeMaleSlider.value.ToString();
                    RabbitDefaults.scaleMale = rabbitSizeMaleSlider.value;
                    break;
                case "RabbitSpeed":
                    rabbitSpeedText.text = rabbitSpeedSlider.value.ToString();
                    RabbitDefaults.moveSpeed = rabbitSpeedSlider.value;
                    break;
                case "RabbitHungerMax":
                    rabbitHungerText.text = rabbitHungerSlider.value.ToString();
                    RabbitDefaults.hungerMax = rabbitHungerSlider.value;
                    break;
                case "RabbitThirstMax":
                    rabbitThirstText.text = rabbitThirstSlider.value.ToString();
                    RabbitDefaults.thirstMax = rabbitThirstSlider.value;
                    break;
                case "RabbitAgeMax":
                    rabbitAgeText.text = rabbitAgeSlider.value.ToString();
                    RabbitDefaults.ageMax = rabbitAgeSlider.value;
                    break;
                case "RabbitTouchRadius":
                    rabbitTouchRadiusText.text = rabbitTouchRadiusSlider.value.ToString();
                    RabbitDefaults.touchRadius = rabbitTouchRadiusSlider.value;
                    break;
                case "RabbitSightRadius":
                    rabbitSightRadiusText.text = rabbitSightRadiusSlider.value.ToString();
                    RabbitDefaults.sightRadius = rabbitSightRadiusSlider.value;
                    break;
                case "RabbitPregnancyLength":
                    rabbitPregnancyLengthText.text = rabbitPregnancyLengthSlider.value.ToString();
                    RabbitDefaults.pregnancyLength = rabbitPregnancyLengthSlider.value;
                    break;
                case "RabbitMatingDuration":
                    rabbitMatingDurationText.text = rabbitMatingDurationSlider.value.ToString();
                    RabbitDefaults.matingDuration = rabbitMatingDurationSlider.value;
                    break;
                case "RabbitBirthDuration":
                    rabbitBirthDurationText.text = rabbitBirthDurationSlider.value.ToString();
                    RabbitDefaults.birthDuration = rabbitBirthDurationSlider.value;
                    break;
                case "RabbitLitterMin":
                    rabbitLitterSizeMinText.text = rabbitLitterSizeMinSlider.value.ToString();
                    RabbitDefaults.litterSizeMin = (int)rabbitLitterSizeMinSlider.value;
                    break;
                case "RabbitLitterMax":
                    rabbitLitterSizeMaxText.text = rabbitLitterSizeMaxSlider.value.ToString();
                    RabbitDefaults.litterSizeMax = (int)rabbitLitterSizeMaxSlider.value;
                    break;
                case "RabbitLitterAve":
                    rabbitLitterSizeAveText.text = rabbitLitterSizeAveSlider.value.ToString();
                    RabbitDefaults.litterSizeAve = (int)rabbitLitterSizeAveSlider.value;
                    break;
                default:
                    Debug.LogWarning("Attempted to update unknown property in switch: " + propertyToUpdate);
                    break;
            }
        }

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
                    rabbitSizeMaleText.text = RabbitDefaults.scaleMale.ToString();
                    rabbitSizeMaleSlider.value = RabbitDefaults.scaleMale;

                    rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();
                    rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;

                    rabbitSpeedText.text = RabbitDefaults.moveSpeed.ToString();
                    rabbitSpeedSlider.value = RabbitDefaults.moveSpeed;

                    rabbitHungerText.text = RabbitDefaults.hungerMax.ToString();
                    rabbitHungerSlider.value = RabbitDefaults.hungerMax;

                    rabbitThirstText.text = RabbitDefaults.thirstMax.ToString();
                    rabbitThirstSlider.value = RabbitDefaults.thirstMax;

                    rabbitAgeText.text = RabbitDefaults.ageMax.ToString();
                    rabbitAgeSlider.value = RabbitDefaults.ageMax;

                    rabbitTouchRadiusText.text = RabbitDefaults.touchRadius.ToString();
                    rabbitTouchRadiusSlider.value = RabbitDefaults.touchRadius;

                    rabbitSightRadiusText.text = RabbitDefaults.sightRadius.ToString();
                    rabbitSightRadiusSlider.value = RabbitDefaults.sightRadius;

                    rabbitPregnancyLengthText.text = RabbitDefaults.pregnancyLength.ToString();
                    rabbitPregnancyLengthSlider.value = RabbitDefaults.pregnancyLength;

                    rabbitMatingDurationText.text = RabbitDefaults.matingDuration.ToString();
                    rabbitMatingDurationSlider.value = RabbitDefaults.matingDuration;

                    rabbitBirthDurationText.text = RabbitDefaults.birthDuration.ToString();
                    rabbitBirthDurationSlider.value = RabbitDefaults.birthDuration;

                    rabbitLitterSizeMinText.text = RabbitDefaults.litterSizeMin.ToString();
                    rabbitLitterSizeMinSlider.value = RabbitDefaults.litterSizeMin;

                    rabbitLitterSizeMaxText.text = RabbitDefaults.litterSizeMax.ToString();
                    rabbitLitterSizeMaxSlider.value = RabbitDefaults.litterSizeMax;

                    rabbitLitterSizeAveText.text = RabbitDefaults.litterSizeAve.ToString();
                    rabbitLitterSizeAveSlider.value = RabbitDefaults.litterSizeAve;

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
            string[] filePaths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
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
                XmlNodeList state = xmlDocument.GetElementsByTagName("state");
                XmlNodeList previousState = xmlDocument.GetElementsByTagName("previousState");
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
                RabbitDefaults.ageMax = float.Parse(ageMax[0].InnerText);
                RabbitDefaults.ageGroup = (BioStatsData.AgeGroup)Enum.Parse(typeof(BioStatsData.AgeGroup), ageGroup[0].InnerText);
                RabbitDefaults.adultEntryTimer = float.Parse(adultEntryTimer[0].InnerText);
                RabbitDefaults.oldEntryTimer = float.Parse(oldEntryTimer[0].InnerText);
                RabbitDefaults.nutritionalValue = float.Parse(nutritionalValue[0].InnerText);
                RabbitDefaults.canBeEaten = bool.Parse(canBeEaten[0].InnerText);
                RabbitDefaults.nutritionalValueMultiplier = float.Parse(nutritionalValueMultiplier[0].InnerText);
                RabbitDefaults.foodType = (EdibleData.FoodType)Enum.Parse(typeof(EdibleData.FoodType), foodType[0].InnerText);
                RabbitDefaults.hunger = float.Parse(hunger[0].InnerText);
                RabbitDefaults.hungerMax = float.Parse(hungerMax[0].InnerText);
                RabbitDefaults.hungryThreshold = float.Parse(hungryThreshold[0].InnerText);
                RabbitDefaults.hungerIncrease = float.Parse(hungerIncrease[0].InnerText);
                RabbitDefaults.pregnancyHungerIncrease = float.Parse(pregnancyHungerIncrease[0].InnerText);
                RabbitDefaults.youngHungerIncrease = float.Parse(youngHungerIncrease[0].InnerText);
                RabbitDefaults.adultHungerIncrease = float.Parse(adultHungerIncrease[0].InnerText);
                RabbitDefaults.oldHungerIncrease = float.Parse(oldHungerIncrease[0].InnerText);
                RabbitDefaults.eatingSpeed = float.Parse(eatingSpeed[0].InnerText);
                RabbitDefaults.diet = (BasicNeedsData.Diet)Enum.Parse(typeof(BasicNeedsData.Diet), diet[0].InnerText);
                RabbitDefaults.thirst = float.Parse(thirst[0].InnerText);
                RabbitDefaults.thirstMax = float.Parse(thirstMax[0].InnerText);
                RabbitDefaults.thirstyThreshold = float.Parse(thirstyThreshold[0].InnerText);
                RabbitDefaults.thirstIncrease = float.Parse(thirstIncrease[0].InnerText);
                RabbitDefaults.drinkingSpeed = float.Parse(drinkingSpeed[0].InnerText);
                RabbitDefaults.mateStartTime = float.Parse(mateStartTime[0].InnerText);
                RabbitDefaults.matingDuration = float.Parse(matingDuration[0].InnerText);
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
                RabbitDefaults.pregnancyLength = float.Parse(pregnancyLength[0].InnerText);
                RabbitDefaults.birthDuration = float.Parse(birthDuration[0].InnerText);
                RabbitDefaults.litterSizeMin = int.Parse(litterSizeMin[0].InnerText);
                RabbitDefaults.litterSizeMax = int.Parse(litterSizeMax[0].InnerText);
                RabbitDefaults.litterSizeAve = int.Parse(litterSizeAve[0].InnerText);
                RabbitDefaults.moveSpeed = float.Parse(moveSpeed[0].InnerText);
                RabbitDefaults.rotationSpeed = float.Parse(rotationSpeed[0].InnerText);
                RabbitDefaults.moveMultiplier = float.Parse(moveMultiplier[0].InnerText);
                RabbitDefaults.pregnancyMoveMultiplier = float.Parse(pregnancyMoveMultiplier[0].InnerText);
                RabbitDefaults.originalMoveMultiplier = float.Parse(originalMoveMultiplier[0].InnerText);
                RabbitDefaults.youngMoveMultiplier = float.Parse(youngMoveMultiplier[0].InnerText);
                RabbitDefaults.adultMoveMultiplier = float.Parse(adultMoveMultiplier[0].InnerText);
                RabbitDefaults.oldMoveMultiplier = float.Parse(oldMoveMultiplier[0].InnerText);
                RabbitDefaults.sizeMultiplier = float.Parse(sizeMultiplier[0].InnerText);
                RabbitDefaults.scaleMale = float.Parse(scaleMale[0].InnerText);
                RabbitDefaults.scaleFemale = float.Parse(scaleFemale[0].InnerText);
                RabbitDefaults.youngSizeMultiplier = float.Parse(youngSizeMultiplier[0].InnerText);
                RabbitDefaults.adultSizeMultiplier = float.Parse(adultSizeMultiplier[0].InnerText);
                RabbitDefaults.oldSizeMultiplier = float.Parse(oldSizeMultiplier[0].InnerText);
                RabbitDefaults.state = (StateData.States)Enum.Parse(typeof(StateData.States), state[0].InnerText);
                RabbitDefaults.previousState = (StateData.States)Enum.Parse(typeof(StateData.States), previousState[0].InnerText);
                RabbitDefaults.deathReason = (StateData.DeathReason)Enum.Parse(typeof(StateData.DeathReason), deathReason[0].InnerText);
                RabbitDefaults.beenEaten = bool.Parse(beenEaten[0].InnerText);
                RabbitDefaults.touchRadius = float.Parse(touchRadius[0].InnerText);
                RabbitDefaults.sightRadius = float.Parse(sightRadius[0].InnerText);
                RabbitDefaults.shortestToEdibleDistance = float.Parse(shortestToEdibleDistance[0].InnerText);
                RabbitDefaults.shortestToWaterDistance = float.Parse(shortestToWaterDistance[0].InnerText);
                RabbitDefaults.shortestToPredatorDistance = float.Parse(shortestToPredatorDistance[0].InnerText);
                RabbitDefaults.shortestToMateDistance = float.Parse(shortestToMateDistance[0].InnerText);
                RabbitDefaults.colliderType = (ColliderTypeData.ColliderType)Enum.Parse(typeof(ColliderTypeData.ColliderType), colliderType[0].InnerText);

                #endregion


                #region fox default

                FoxDefaults.age = float.Parse(age[1].InnerText);
                FoxDefaults.ageIncrease = float.Parse(ageIncrease[1].InnerText);
                FoxDefaults.ageMax = float.Parse(ageMax[1].InnerText);
                FoxDefaults.ageGroup = (BioStatsData.AgeGroup)Enum.Parse(typeof(BioStatsData.AgeGroup), ageGroup[1].InnerText);
                FoxDefaults.adultEntryTimer = float.Parse(adultEntryTimer[1].InnerText);
                FoxDefaults.oldEntryTimer = float.Parse(oldEntryTimer[1].InnerText);
                FoxDefaults.nutritionalValue = float.Parse(nutritionalValue[1].InnerText);
                FoxDefaults.canBeEaten = bool.Parse(canBeEaten[1].InnerText);
                FoxDefaults.nutritionalValueMultiplier = float.Parse(nutritionalValueMultiplier[1].InnerText);
                FoxDefaults.foodType = (EdibleData.FoodType)Enum.Parse(typeof(EdibleData.FoodType), foodType[1].InnerText);
                FoxDefaults.hunger = float.Parse(hunger[1].InnerText);
                FoxDefaults.hungerMax = float.Parse(hungerMax[1].InnerText);
                FoxDefaults.hungryThreshold = float.Parse(hungryThreshold[1].InnerText);
                FoxDefaults.hungerIncrease = float.Parse(hungerIncrease[1].InnerText);
                FoxDefaults.pregnancyHungerIncrease = float.Parse(pregnancyHungerIncrease[1].InnerText);
                FoxDefaults.youngHungerIncrease = float.Parse(youngHungerIncrease[1].InnerText);
                FoxDefaults.adultHungerIncrease = float.Parse(adultHungerIncrease[1].InnerText);
                FoxDefaults.oldHungerIncrease = float.Parse(oldHungerIncrease[1].InnerText);
                FoxDefaults.eatingSpeed = float.Parse(eatingSpeed[1].InnerText);
                FoxDefaults.diet = (BasicNeedsData.Diet)Enum.Parse(typeof(BasicNeedsData.Diet), diet[1].InnerText);
                FoxDefaults.thirst = float.Parse(thirst[1].InnerText);
                FoxDefaults.thirstMax = float.Parse(thirstMax[1].InnerText);
                FoxDefaults.thirstyThreshold = float.Parse(thirstyThreshold[1].InnerText);
                FoxDefaults.thirstIncrease = float.Parse(thirstIncrease[1].InnerText);
                FoxDefaults.drinkingSpeed = float.Parse(drinkingSpeed[1].InnerText);
                FoxDefaults.mateStartTime = float.Parse(mateStartTime[1].InnerText);
                FoxDefaults.matingDuration = float.Parse(matingDuration[1].InnerText);
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
                FoxDefaults.pregnancyLength = float.Parse(pregnancyLength[1].InnerText);
                FoxDefaults.birthDuration = float.Parse(birthDuration[1].InnerText);
                FoxDefaults.litterSizeMin = int.Parse(litterSizeMin[1].InnerText);
                FoxDefaults.litterSizeMax = int.Parse(litterSizeMax[1].InnerText);
                FoxDefaults.litterSizeAve = int.Parse(litterSizeAve[1].InnerText);
                FoxDefaults.moveSpeed = float.Parse(moveSpeed[1].InnerText);
                FoxDefaults.rotationSpeed = float.Parse(rotationSpeed[1].InnerText);
                FoxDefaults.moveMultiplier = float.Parse(moveMultiplier[1].InnerText);
                FoxDefaults.pregnancyMoveMultiplier = float.Parse(pregnancyMoveMultiplier[1].InnerText);
                FoxDefaults.originalMoveMultiplier = float.Parse(originalMoveMultiplier[1].InnerText);
                FoxDefaults.youngMoveMultiplier = float.Parse(youngMoveMultiplier[1].InnerText);
                FoxDefaults.adultMoveMultiplier = float.Parse(adultMoveMultiplier[1].InnerText);
                FoxDefaults.oldMoveMultiplier = float.Parse(oldMoveMultiplier[1].InnerText);
                FoxDefaults.sizeMultiplier = float.Parse(sizeMultiplier[1].InnerText);
                FoxDefaults.scaleMale = float.Parse(scaleMale[1].InnerText);
                FoxDefaults.scaleFemale = float.Parse(scaleFemale[1].InnerText);
                FoxDefaults.youngSizeMultiplier = float.Parse(youngSizeMultiplier[1].InnerText);
                FoxDefaults.adultSizeMultiplier = float.Parse(adultSizeMultiplier[1].InnerText);
                FoxDefaults.oldSizeMultiplier = float.Parse(oldSizeMultiplier[1].InnerText);
                FoxDefaults.state = (StateData.States)Enum.Parse(typeof(StateData.States), state[1].InnerText);
                FoxDefaults.previousState = (StateData.States)Enum.Parse(typeof(StateData.States), previousState[1].InnerText);
                FoxDefaults.deathReason = (StateData.DeathReason)Enum.Parse(typeof(StateData.DeathReason), deathReason[1].InnerText);
                FoxDefaults.beenEaten = bool.Parse(beenEaten[1].InnerText);
                FoxDefaults.touchRadius = float.Parse(touchRadius[1].InnerText);
                FoxDefaults.sightRadius = float.Parse(sightRadius[1].InnerText);
                FoxDefaults.shortestToEdibleDistance = float.Parse(shortestToEdibleDistance[1].InnerText);
                FoxDefaults.shortestToWaterDistance = float.Parse(shortestToWaterDistance[1].InnerText);
                FoxDefaults.shortestToPredatorDistance = float.Parse(shortestToPredatorDistance[1].InnerText);
                FoxDefaults.shortestToMateDistance = float.Parse(shortestToMateDistance[1].InnerText);
                FoxDefaults.colliderType = (ColliderTypeData.ColliderType)Enum.Parse(typeof(ColliderTypeData.ColliderType), colliderType[1].InnerText);


                #endregion

                #region GrassData 

                GrassDefaults.nutritionalValue = float.Parse(nutritionalValue[2].InnerText);
         
                GrassDefaults.nutritionalValueMultiplier = float.Parse(nutritionalValueMultiplier[2].InnerText);

                GrassDefaults.sizeMultiplier = float.Parse(sizeMultiplier[2].InnerText);

                XmlNodeList Scale = xmlDocument.GetElementsByTagName("scale");
                GrassDefaults.scale = float.Parse(Scale[0].InnerText);

                GrassDefaults.foodType = (EdibleData.FoodType)Enum.Parse(typeof(EdibleData.FoodType), foodType[2].InnerText);
                
                GrassDefaults.state = (StateData.States)Enum.Parse(typeof(StateData.States), state[2].InnerText);
                 
                GrassDefaults.previousState = (StateData.States)Enum.Parse(typeof(StateData.States), previousState[2].InnerText);

                GrassDefaults.deathReason = (StateData.DeathReason)Enum.Parse(typeof(StateData.DeathReason), deathReason[2].InnerText);;

                GrassDefaults.GrassColliderType = (ColliderTypeData.ColliderType)Enum.Parse(typeof(ColliderTypeData.ColliderType), colliderType[2].InnerText);

                #endregion GrassData
                print("I am load game haha");
            }
            else
            {
                Debug.Log("NOT FOUNDED");
            }
            SceneManager.LoadScene(_newGameButtonLevel);
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
