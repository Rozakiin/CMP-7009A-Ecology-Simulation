using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using System.IO;
using System.Xml;

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

                #region Rabbit Data

                #region ageData 

                XmlNodeList rAge = xmlDocument.GetElementsByTagName("age");
                float age = float.Parse(rAge[0].InnerText);
                RabbitDefaults.age = age;
                XmlNodeList rAgeincrease = xmlDocument.GetElementsByTagName("ageIncrease");
                float ageIncrease = float.Parse(rAgeincrease[0].InnerText);
                RabbitDefaults.ageIncrease = ageIncrease;
                XmlNodeList rAgemax = xmlDocument.GetElementsByTagName("ageMax");
                float ageMax = float.Parse(rAgemax[0].InnerText);
                RabbitDefaults.ageMax = ageMax;
                XmlNodeList rAdultentrytimer = xmlDocument.GetElementsByTagName("adultEntryTimer");
                float adultEntryTimer = float.Parse(rAdultentrytimer[0].InnerText);
                RabbitDefaults.adultEntryTimer = adultEntryTimer;
                XmlNodeList rOldentrytimer = xmlDocument.GetElementsByTagName("oldEntryTimer");
                float oldEntryTimer = float.Parse(rOldentrytimer[0].InnerText);
                RabbitDefaults.oldEntryTimer = oldEntryTimer;

                #endregion ageData

                #region edibleData 

                XmlNodeList rNutritionalvalue = xmlDocument.GetElementsByTagName("nutritionalValue");
                float nutritionalValue = float.Parse(rNutritionalvalue[0].InnerText);
                RabbitDefaults.nutritionalValue = nutritionalValue;
                XmlNodeList rNutritionalvaluemultiplier = xmlDocument.GetElementsByTagName("nutritionalValueMultiplier");
                float nutritionalValueMultiplier = float.Parse(rNutritionalvaluemultiplier[0].InnerText);
                RabbitDefaults.nutritionalValueMultiplier = nutritionalValueMultiplier;

                #endregion edibleData

                #region hungerData 

                XmlNodeList rHunger = xmlDocument.GetElementsByTagName("hunger");
                float hunger = float.Parse(rHunger[0].InnerText);
                RabbitDefaults.hunger = hunger;
                XmlNodeList rHungermax = xmlDocument.GetElementsByTagName("hungerMax");
                float hungerMax = float.Parse(rHungermax[0].InnerText);
                RabbitDefaults.hungerMax = hungerMax;
                XmlNodeList rHungrythreshold = xmlDocument.GetElementsByTagName("hungryThreshold");
                float hungryThreshold = float.Parse(rHungrythreshold[0].InnerText);
                RabbitDefaults.hungryThreshold = hungryThreshold;
                XmlNodeList rHungerincrease = xmlDocument.GetElementsByTagName("hungerIncrease");
                float hungerIncrease = float.Parse(rHungerincrease[0].InnerText);
                RabbitDefaults.hungerIncrease = hungerIncrease;
                XmlNodeList rPregnancyhungerincrease = xmlDocument.GetElementsByTagName("pregnancyHungerIncrease");
                float pregnancyHungerIncrease = float.Parse(rPregnancyhungerincrease[0].InnerText);
                RabbitDefaults.pregnancyHungerIncrease = pregnancyHungerIncrease;
                XmlNodeList rYounghungerincrease = xmlDocument.GetElementsByTagName("youngHungerIncrease");
                float youngHungerIncrease = float.Parse(rYounghungerincrease[0].InnerText);
                RabbitDefaults.youngHungerIncrease = youngHungerIncrease;
                XmlNodeList rAdulthungerincrease = xmlDocument.GetElementsByTagName("adultHungerIncrease");
                float adultHungerIncrease = float.Parse(rAdulthungerincrease[0].InnerText);
                RabbitDefaults.adultHungerIncrease = adultHungerIncrease;
                XmlNodeList rOldhungerincrease = xmlDocument.GetElementsByTagName("oldHungerIncrease");
                float oldHungerIncrease = float.Parse(rOldhungerincrease[0].InnerText);
                RabbitDefaults.oldHungerIncrease = oldHungerIncrease;
                XmlNodeList rEatingspeed = xmlDocument.GetElementsByTagName("eatingSpeed");
                float eatingSpeed = float.Parse(rEatingspeed[0].InnerText);
                RabbitDefaults.eatingSpeed = eatingSpeed;

                #endregion hungerData

                #region thirstData 

                XmlNodeList rThirst = xmlDocument.GetElementsByTagName("thirst");
                float thirst = float.Parse(rThirst[0].InnerText);
                RabbitDefaults.thirst = thirst;
                XmlNodeList rThirstmax = xmlDocument.GetElementsByTagName("thirstMax");
                float thirstMax = float.Parse(rThirstmax[0].InnerText);
                RabbitDefaults.thirstMax = thirstMax;
                XmlNodeList rThirstythreshold = xmlDocument.GetElementsByTagName("thirstyThreshold");
                float thirstyThreshold = float.Parse(rThirstythreshold[0].InnerText);
                RabbitDefaults.thirstyThreshold = thirstyThreshold;
                XmlNodeList rThirstincrease = xmlDocument.GetElementsByTagName("thirstIncrease");
                float thirstIncrease = float.Parse(rThirstincrease[0].InnerText);
                RabbitDefaults.thirstIncrease = thirstIncrease;
                XmlNodeList rDrinkingspeed = xmlDocument.GetElementsByTagName("drinkingSpeed");
                float drinkingSpeed = float.Parse(rDrinkingspeed[0].InnerText);
                RabbitDefaults.drinkingSpeed = drinkingSpeed;

                #endregion thirstData

                #region mateData 

                XmlNodeList rMatestarttime = xmlDocument.GetElementsByTagName("mateStartTime");
                float mateStartTime = float.Parse(rMatestarttime[0].InnerText);
                RabbitDefaults.mateStartTime = mateStartTime;
                XmlNodeList rMatingduration = xmlDocument.GetElementsByTagName("matingDuration");
                float matingDuration = float.Parse(rMatingduration[0].InnerText);
                RabbitDefaults.matingDuration = matingDuration;
                XmlNodeList rReproductiveurge = xmlDocument.GetElementsByTagName("reproductiveUrge");
                float reproductiveUrge = float.Parse(rReproductiveurge[0].InnerText);
                RabbitDefaults.reproductiveUrge = reproductiveUrge;
                XmlNodeList rReproductiveurgeincreasemale = xmlDocument.GetElementsByTagName("reproductiveUrgeIncreaseMale");
                float reproductiveUrgeIncreaseMale = float.Parse(rReproductiveurgeincreasemale[0].InnerText);
                RabbitDefaults.reproductiveUrgeIncreaseMale = reproductiveUrgeIncreaseMale;
                XmlNodeList rReproductiveurgeincreasefemale = xmlDocument.GetElementsByTagName("reproductiveUrgeIncreaseFemale");
                float reproductiveUrgeIncreaseFemale = float.Parse(rReproductiveurgeincreasefemale[0].InnerText);
                RabbitDefaults.reproductiveUrgeIncreaseFemale = reproductiveUrgeIncreaseFemale;
                XmlNodeList rMatingthreshold = xmlDocument.GetElementsByTagName("matingThreshold");
                float matingThreshold = float.Parse(rMatingthreshold[0].InnerText);
                RabbitDefaults.matingThreshold = matingThreshold;

                #endregion mateData

                #region pregnancyData 

                XmlNodeList rPregnancystarttime = xmlDocument.GetElementsByTagName("pregnancyStartTime");
                float pregnancyStartTime = float.Parse(rPregnancystarttime[0].InnerText);
                RabbitDefaults.pregnancyStartTime = pregnancyStartTime;
                XmlNodeList rBabiesborn = xmlDocument.GetElementsByTagName("babiesBorn");
                int babiesBorn = int.Parse(rBabiesborn[0].InnerText);
                RabbitDefaults.babiesBorn = babiesBorn;
                XmlNodeList rBirthstarttime = xmlDocument.GetElementsByTagName("birthStartTime");
                float birthStartTime = float.Parse(rBirthstarttime[0].InnerText);
                RabbitDefaults.birthStartTime = birthStartTime;
                XmlNodeList rCurrentlittersize = xmlDocument.GetElementsByTagName("currentLitterSize");
                int currentLitterSize = int.Parse(rCurrentlittersize[0].InnerText);
                RabbitDefaults.currentLitterSize = currentLitterSize;
                XmlNodeList rPregnancylengthmodifier = xmlDocument.GetElementsByTagName("pregnancyLengthModifier");
                float pregnancyLengthModifier = float.Parse(rPregnancylengthmodifier[0].InnerText);
                RabbitDefaults.pregnancyLengthModifier = pregnancyLengthModifier;
                XmlNodeList rPregnancylength = xmlDocument.GetElementsByTagName("pregnancyLength");
                float pregnancyLength = float.Parse(rPregnancylength[0].InnerText);
                RabbitDefaults.pregnancyLength = pregnancyLength;
                XmlNodeList rBirthduration = xmlDocument.GetElementsByTagName("birthDuration");
                float birthDuration = float.Parse(rBirthduration[0].InnerText);
                RabbitDefaults.birthDuration = birthDuration;
                XmlNodeList rLittersizemin = xmlDocument.GetElementsByTagName("litterSizeMin");
                int litterSizeMin = int.Parse(rLittersizemin[0].InnerText);
                RabbitDefaults.litterSizeMin = litterSizeMin;
                XmlNodeList rLittersizemax = xmlDocument.GetElementsByTagName("litterSizeMax");
                int litterSizeMax = int.Parse(rLittersizemax[0].InnerText);
                RabbitDefaults.litterSizeMax = litterSizeMax;
                XmlNodeList rLittersizeave = xmlDocument.GetElementsByTagName("litterSizeAve");
                int litterSizeAve = int.Parse(rLittersizeave[0].InnerText);
                RabbitDefaults.litterSizeAve = litterSizeAve;

                #endregion pregnancyData

                #region movementData

                XmlNodeList rMovespeed = xmlDocument.GetElementsByTagName("moveSpeed");
                float moveSpeed = float.Parse(rMovespeed[0].InnerText);
                RabbitDefaults.moveSpeed = moveSpeed;
                XmlNodeList rRotationspeed = xmlDocument.GetElementsByTagName("rotationSpeed");
                float rotationSpeed = float.Parse(rRotationspeed[0].InnerText);
                RabbitDefaults.rotationSpeed = rotationSpeed;
                XmlNodeList rMovemultiplier = xmlDocument.GetElementsByTagName("moveMultiplier");
                float moveMultiplier = float.Parse(rMovemultiplier[0].InnerText);
                RabbitDefaults.moveMultiplier = moveMultiplier;
                XmlNodeList rPregnancymovemultiplier = xmlDocument.GetElementsByTagName("pregnancyMoveMultiplier");
                float pregnancyMoveMultiplier = float.Parse(rPregnancymovemultiplier[0].InnerText);
                RabbitDefaults.pregnancyMoveMultiplier = pregnancyMoveMultiplier;
                XmlNodeList rOriginalmovemultiplier = xmlDocument.GetElementsByTagName("originalMoveMultiplier");
                float originalMoveMultiplier = float.Parse(rOriginalmovemultiplier[0].InnerText);
                RabbitDefaults.originalMoveMultiplier = originalMoveMultiplier;
                XmlNodeList rYoungmovemultiplier = xmlDocument.GetElementsByTagName("youngMoveMultiplier");
                float youngMoveMultiplier = float.Parse(rYoungmovemultiplier[0].InnerText);
                RabbitDefaults.youngMoveMultiplier = youngMoveMultiplier;
                XmlNodeList rAdultmovemultiplier = xmlDocument.GetElementsByTagName("adultMoveMultiplier");
                float adultMoveMultiplier = float.Parse(rAdultmovemultiplier[0].InnerText);
                RabbitDefaults.adultMoveMultiplier = adultMoveMultiplier;
                XmlNodeList rOldmovemultiplier = xmlDocument.GetElementsByTagName("oldMoveMultiplier");
                float oldMoveMultiplier = float.Parse(rOldmovemultiplier[0].InnerText);
                RabbitDefaults.oldMoveMultiplier = oldMoveMultiplier;

                #endregion movementData

                #region sizeData 

                XmlNodeList rSizemultiplier = xmlDocument.GetElementsByTagName("sizeMultiplier");
                float sizeMultiplier = float.Parse(rSizemultiplier[0].InnerText);
                RabbitDefaults.sizeMultiplier = sizeMultiplier;
                XmlNodeList rScalemale = xmlDocument.GetElementsByTagName("scaleMale");
                float scaleMale = float.Parse(rScalemale[0].InnerText);
                RabbitDefaults.scaleMale = scaleMale;
                XmlNodeList rScalefemale = xmlDocument.GetElementsByTagName("scaleFemale");
                float scaleFemale = float.Parse(rScalefemale[0].InnerText);
                RabbitDefaults.scaleFemale = scaleFemale;
                XmlNodeList rYoungsizemultiplier = xmlDocument.GetElementsByTagName("youngSizeMultiplier");
                float youngSizeMultiplier = float.Parse(rYoungsizemultiplier[0].InnerText);
                RabbitDefaults.youngSizeMultiplier = youngSizeMultiplier;
                XmlNodeList rAdultsizemultiplier = xmlDocument.GetElementsByTagName("adultSizeMultiplier");
                float adultSizeMultiplier = float.Parse(rAdultsizemultiplier[0].InnerText);
                RabbitDefaults.adultSizeMultiplier = adultSizeMultiplier;
                XmlNodeList rOldsizemultiplier = xmlDocument.GetElementsByTagName("oldSizeMultiplier");
                float oldSizeMultiplier = float.Parse(rOldsizemultiplier[0].InnerText);
                RabbitDefaults.oldSizeMultiplier = oldSizeMultiplier;

                #endregion sizeData

                #region targetData


                XmlNodeList rTouchradius = xmlDocument.GetElementsByTagName("touchRadius");
                float touchRadius = float.Parse(rTouchradius[0].InnerText);
                RabbitDefaults.touchRadius = touchRadius;
                XmlNodeList rSightradius = xmlDocument.GetElementsByTagName("sightRadius");
                float sightRadius = float.Parse(rSightradius[0].InnerText);
                RabbitDefaults.sightRadius = sightRadius;


                #endregion targetData

                #endregion RabbitData

                #region Fox Data

                #region ageData 

                XmlNodeList fAge = xmlDocument.GetElementsByTagName("age");
                float ffage = float.Parse(fAge[0].InnerText);
                FoxDefaults.age = ffage;
                XmlNodeList fAgeincrease = xmlDocument.GetElementsByTagName("ageIncrease");
                float ffageIncrease = float.Parse(fAgeincrease[0].InnerText);
                FoxDefaults.ageIncrease = ffageIncrease;
                XmlNodeList fAgemax = xmlDocument.GetElementsByTagName("ageMax");
                float ffageMax = float.Parse(fAgemax[0].InnerText);
                FoxDefaults.ageMax = ffageMax;
                XmlNodeList fAdultentrytimer = xmlDocument.GetElementsByTagName("adultEntryTimer");
                float ffadultEntryTimer = float.Parse(fAdultentrytimer[0].InnerText);
                FoxDefaults.adultEntryTimer = ffadultEntryTimer;
                XmlNodeList fOldentrytimer = xmlDocument.GetElementsByTagName("oldEntryTimer");
                float ffoldEntryTimer = float.Parse(fOldentrytimer[0].InnerText);
                FoxDefaults.oldEntryTimer = ffoldEntryTimer;

                #endregion ageData

                #region edibleData 

                XmlNodeList fNutritionalvalue = xmlDocument.GetElementsByTagName("nutritionalValue");
                float ffnutritionalValue = float.Parse(fNutritionalvalue[0].InnerText);
                FoxDefaults.nutritionalValue = ffnutritionalValue;
                XmlNodeList fNutritionalvaluemultiplier = xmlDocument.GetElementsByTagName("nutritionalValueMultiplier");
                float ffnutritionalValueMultiplier = float.Parse(fNutritionalvaluemultiplier[0].InnerText);
                FoxDefaults.nutritionalValueMultiplier = ffnutritionalValueMultiplier;

                #endregion edibleData

                #region hungerData 

                XmlNodeList fHunger = xmlDocument.GetElementsByTagName("hunger");
                float ffhunger = float.Parse(fHunger[0].InnerText);
                FoxDefaults.hunger = ffhunger;
                XmlNodeList fHungermax = xmlDocument.GetElementsByTagName("hungerMax");
                float ffhungerMax = float.Parse(fHungermax[0].InnerText);
                FoxDefaults.hungerMax = ffhungerMax;
                XmlNodeList fHungrythreshold = xmlDocument.GetElementsByTagName("hungryThreshold");
                float ffhungryThreshold = float.Parse(fHungrythreshold[0].InnerText);
                FoxDefaults.hungryThreshold = ffhungryThreshold;
                XmlNodeList fHungerincrease = xmlDocument.GetElementsByTagName("hungerIncrease");
                float ffhungerIncrease = float.Parse(fHungerincrease[0].InnerText);
                FoxDefaults.hungerIncrease = ffhungerIncrease;
                XmlNodeList fPregnancyhungerincrease = xmlDocument.GetElementsByTagName("pregnancyHungerIncrease");
                float ffpregnancyHungerIncrease = float.Parse(fPregnancyhungerincrease[0].InnerText);
                FoxDefaults.pregnancyHungerIncrease = ffpregnancyHungerIncrease;
                XmlNodeList fYounghungerincrease = xmlDocument.GetElementsByTagName("youngHungerIncrease");
                float ffyoungHungerIncrease = float.Parse(fYounghungerincrease[0].InnerText);
                FoxDefaults.youngHungerIncrease = ffyoungHungerIncrease;
                XmlNodeList fAdulthungerincrease = xmlDocument.GetElementsByTagName("adultHungerIncrease");
                float ffadultHungerIncrease = float.Parse(fAdulthungerincrease[0].InnerText);
                FoxDefaults.adultHungerIncrease = ffadultHungerIncrease;
                XmlNodeList fOldhungerincrease = xmlDocument.GetElementsByTagName("oldHungerIncrease");
                float ffoldHungerIncrease = float.Parse(fOldhungerincrease[0].InnerText);
                FoxDefaults.oldHungerIncrease = ffoldHungerIncrease;
                XmlNodeList fEatingspeed = xmlDocument.GetElementsByTagName("eatingSpeed");
                float ffeatingSpeed = float.Parse(fEatingspeed[0].InnerText);
                FoxDefaults.eatingSpeed = ffeatingSpeed;

                #endregion hungerData

                #region thirstData 

                XmlNodeList fThirst = xmlDocument.GetElementsByTagName("thirst");
                float ffthirst = float.Parse(fThirst[0].InnerText);
                FoxDefaults.thirst = ffthirst;
                XmlNodeList fThirstmax = xmlDocument.GetElementsByTagName("thirstMax");
                float ffthirstMax = float.Parse(fThirstmax[0].InnerText);
                FoxDefaults.thirstMax = ffthirstMax;
                XmlNodeList fThirstythreshold = xmlDocument.GetElementsByTagName("thirstyThreshold");
                float ffthirstyThreshold = float.Parse(fThirstythreshold[0].InnerText);
                FoxDefaults.thirstyThreshold = ffthirstyThreshold;
                XmlNodeList fThirstincrease = xmlDocument.GetElementsByTagName("thirstIncrease");
                float ffthirstIncrease = float.Parse(fThirstincrease[0].InnerText);
                FoxDefaults.thirstIncrease = ffthirstIncrease;
                XmlNodeList fDrinkingspeed = xmlDocument.GetElementsByTagName("drinkingSpeed");
                float ffdrinkingSpeed = float.Parse(fDrinkingspeed[0].InnerText);
                FoxDefaults.drinkingSpeed = ffdrinkingSpeed;

                #endregion thirstData

                #region mateData 

                XmlNodeList fMatestarttime = xmlDocument.GetElementsByTagName("mateStartTime");
                float ffmateStartTime = float.Parse(fMatestarttime[0].InnerText);
                FoxDefaults.mateStartTime = ffmateStartTime;
                XmlNodeList fMatingduration = xmlDocument.GetElementsByTagName("matingDuration");
                float ffmatingDuration = float.Parse(fMatingduration[0].InnerText);
                FoxDefaults.matingDuration = ffmatingDuration;
                XmlNodeList fReproductiveurge = xmlDocument.GetElementsByTagName("reproductiveUrge");
                float ffreproductiveUrge = float.Parse(fReproductiveurge[0].InnerText);
                FoxDefaults.reproductiveUrge = ffreproductiveUrge;
                XmlNodeList fReproductiveurgeincreasemale = xmlDocument.GetElementsByTagName("reproductiveUrgeIncreaseMale");
                float ffreproductiveUrgeIncreaseMale = float.Parse(fReproductiveurgeincreasemale[0].InnerText);
                FoxDefaults.reproductiveUrgeIncreaseMale = ffreproductiveUrgeIncreaseMale;
                XmlNodeList fReproductiveurgeincreasefemale = xmlDocument.GetElementsByTagName("reproductiveUrgeIncreaseFemale");
                float ffreproductiveUrgeIncreaseFemale = float.Parse(fReproductiveurgeincreasefemale[0].InnerText);
                FoxDefaults.reproductiveUrgeIncreaseFemale = ffreproductiveUrgeIncreaseFemale;
                XmlNodeList fMatingthreshold = xmlDocument.GetElementsByTagName("matingThreshold");
                float ffmatingThreshold = float.Parse(fMatingthreshold[0].InnerText);
                FoxDefaults.matingThreshold = ffmatingThreshold;

                #endregion mateData

                #region pregnancyData 

                XmlNodeList fPregnancystarttime = xmlDocument.GetElementsByTagName("pregnancyStartTime");
                float ffpregnancyStartTime = float.Parse(fPregnancystarttime[0].InnerText);
                FoxDefaults.pregnancyStartTime = ffpregnancyStartTime;
                XmlNodeList fBabiesborn = xmlDocument.GetElementsByTagName("babiesBorn");
                int ffbabiesBorn = int.Parse(fBabiesborn[0].InnerText);
                FoxDefaults.babiesBorn = ffbabiesBorn;
                XmlNodeList fBirthstarttime = xmlDocument.GetElementsByTagName("birthStartTime");
                float ffbirthStartTime = float.Parse(fBirthstarttime[0].InnerText);
                FoxDefaults.birthStartTime = ffbirthStartTime;
                XmlNodeList fCurrentlittersize = xmlDocument.GetElementsByTagName("currentLitterSize");
                int ffcurrentLitterSize = int.Parse(fCurrentlittersize[0].InnerText);
                FoxDefaults.currentLitterSize = ffcurrentLitterSize;
                XmlNodeList fPregnancylengthmodifier = xmlDocument.GetElementsByTagName("pregnancyLengthModifier");
                float ffpregnancyLengthModifier = float.Parse(fPregnancylengthmodifier[0].InnerText);
                FoxDefaults.pregnancyLengthModifier = ffpregnancyLengthModifier;
                XmlNodeList fPregnancylength = xmlDocument.GetElementsByTagName("pregnancyLength");
                float ffpregnancyLength = float.Parse(fPregnancylength[0].InnerText);
                FoxDefaults.pregnancyLength = ffpregnancyLength;
                XmlNodeList fBirthduration = xmlDocument.GetElementsByTagName("birthDuration");
                float ffbirthDuration = float.Parse(fBirthduration[0].InnerText);
                FoxDefaults.birthDuration = ffbirthDuration;
                XmlNodeList fLittersizemin = xmlDocument.GetElementsByTagName("litterSizeMin");
                int fflitterSizeMin = int.Parse(fLittersizemin[0].InnerText);
                FoxDefaults.litterSizeMin = fflitterSizeMin;
                XmlNodeList fLittersizemax = xmlDocument.GetElementsByTagName("litterSizeMax");
                int fflitterSizeMax = int.Parse(fLittersizemax[0].InnerText);
                FoxDefaults.litterSizeMax = fflitterSizeMax;
                XmlNodeList fLittersizeave = xmlDocument.GetElementsByTagName("litterSizeAve");
                int fflitterSizeAve = int.Parse(fLittersizeave[0].InnerText);
                FoxDefaults.litterSizeAve = fflitterSizeAve;

                #endregion pregnancyData

                #region movementData 

                XmlNodeList fMovespeed = xmlDocument.GetElementsByTagName("moveSpeed");
                float ffmoveSpeed = float.Parse(fMovespeed[0].InnerText);
                FoxDefaults.moveSpeed = ffmoveSpeed;
                XmlNodeList fRotationspeed = xmlDocument.GetElementsByTagName("rotationSpeed");
                float ffrotationSpeed = float.Parse(fRotationspeed[0].InnerText);
                FoxDefaults.rotationSpeed = ffrotationSpeed;
                XmlNodeList fMovemultiplier = xmlDocument.GetElementsByTagName("moveMultiplier");
                float ffmoveMultiplier = float.Parse(fMovemultiplier[0].InnerText);
                FoxDefaults.moveMultiplier = ffmoveMultiplier;
                XmlNodeList fPregnancymovemultiplier = xmlDocument.GetElementsByTagName("pregnancyMoveMultiplier");
                float ffpregnancyMoveMultiplier = float.Parse(fPregnancymovemultiplier[0].InnerText);
                FoxDefaults.pregnancyMoveMultiplier = ffpregnancyMoveMultiplier;
                XmlNodeList fOriginalmovemultiplier = xmlDocument.GetElementsByTagName("originalMoveMultiplier");
                float fforiginalMoveMultiplier = float.Parse(fOriginalmovemultiplier[0].InnerText);
                FoxDefaults.originalMoveMultiplier = fforiginalMoveMultiplier;
                XmlNodeList fYoungmovemultiplier = xmlDocument.GetElementsByTagName("youngMoveMultiplier");
                float ffyoungMoveMultiplier = float.Parse(fYoungmovemultiplier[0].InnerText);
                FoxDefaults.youngMoveMultiplier = ffyoungMoveMultiplier;
                XmlNodeList fAdultmovemultiplier = xmlDocument.GetElementsByTagName("adultMoveMultiplier");
                float ffadultMoveMultiplier = float.Parse(fAdultmovemultiplier[0].InnerText);
                FoxDefaults.adultMoveMultiplier = ffadultMoveMultiplier;
                XmlNodeList fOldmovemultiplier = xmlDocument.GetElementsByTagName("oldMoveMultiplier");
                float ffoldMoveMultiplier = float.Parse(fOldmovemultiplier[0].InnerText);
                FoxDefaults.oldMoveMultiplier = ffoldMoveMultiplier;

                #endregion movementData

                #region sizeData 

                XmlNodeList fSizemultiplier = xmlDocument.GetElementsByTagName("sizeMultiplier");
                float ffsizeMultiplier = float.Parse(fSizemultiplier[0].InnerText);
                FoxDefaults.sizeMultiplier = ffsizeMultiplier;
                XmlNodeList fScalemale = xmlDocument.GetElementsByTagName("scaleMale");
                float ffscaleMale = float.Parse(fScalemale[0].InnerText);
                FoxDefaults.scaleMale = ffscaleMale;
                XmlNodeList fScalefemale = xmlDocument.GetElementsByTagName("scaleFemale");
                float ffscaleFemale = float.Parse(fScalefemale[0].InnerText);
                FoxDefaults.scaleFemale = ffscaleFemale;
                XmlNodeList fYoungsizemultiplier = xmlDocument.GetElementsByTagName("youngSizeMultiplier");
                float ffyoungSizeMultiplier = float.Parse(fYoungsizemultiplier[0].InnerText);
                FoxDefaults.youngSizeMultiplier = ffyoungSizeMultiplier;
                XmlNodeList fAdultsizemultiplier = xmlDocument.GetElementsByTagName("adultSizeMultiplier");
                float ffadultSizeMultiplier = float.Parse(fAdultsizemultiplier[0].InnerText);
                FoxDefaults.adultSizeMultiplier = ffadultSizeMultiplier;
                XmlNodeList fOldsizemultiplier = xmlDocument.GetElementsByTagName("oldSizeMultiplier");
                float ffoldSizeMultiplier = float.Parse(fOldsizemultiplier[0].InnerText);
                FoxDefaults.oldSizeMultiplier = ffoldSizeMultiplier;

                #endregion sizeData

                #region targetData 

                XmlNodeList fTouchradius = xmlDocument.GetElementsByTagName("touchRadius");
                float fftouchRadius = float.Parse(fTouchradius[0].InnerText);
                FoxDefaults.touchRadius = fftouchRadius;
                XmlNodeList fSightradius = xmlDocument.GetElementsByTagName("sightRadius");
                float ffsightRadius = float.Parse(fSightradius[0].InnerText);
                FoxDefaults.sightRadius = ffsightRadius;

                #endregion targetData

                #endregion Fox Data

                #region GrassData 

                XmlNodeList gNutritionalvalue = xmlDocument.GetElementsByTagName("nutritionalValue");
                float ggnutritionalValue = float.Parse(gNutritionalvalue[0].InnerText);
                GrassDefaults.nutritionalValue = ggnutritionalValue;
                XmlNodeList gNutritionalvaluemultiplier = xmlDocument.GetElementsByTagName("nutritionalValueMultiplier");
                float ggnutritionalValueMultiplier = float.Parse(gNutritionalvaluemultiplier[0].InnerText);
                GrassDefaults.nutritionalValueMultiplier = ggnutritionalValueMultiplier;
                XmlNodeList gSizemultiplier = xmlDocument.GetElementsByTagName("sizeMultiplier");
                float ggsizeMultiplier = float.Parse(gSizemultiplier[0].InnerText);
                GrassDefaults.sizeMultiplier = ggsizeMultiplier;
                XmlNodeList gScale = xmlDocument.GetElementsByTagName("scale");
                float ggscale = float.Parse(gScale[0].InnerText);
                GrassDefaults.scale = ggscale;

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
