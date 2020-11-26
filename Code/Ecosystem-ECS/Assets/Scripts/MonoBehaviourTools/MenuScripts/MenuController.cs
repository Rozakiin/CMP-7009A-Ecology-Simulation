using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;

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
        #region Initial properties objects

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
                if (menuNumber == MenuNumber.Options || menuNumber == MenuNumber.NewGame || menuNumber == MenuNumber.LoadGame || menuNumber == MenuNumber.InitialProperties)
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
