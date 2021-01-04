using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region UI Canvas'
    [SerializeField] private GameObject uiSliderCanvas;
    [SerializeField] private GameObject uiTimeCanvas;
    [SerializeField] private GameObject uiTurnCanvas;

    #endregion

    #region Slider Linking
    #region Initial Properties
    [Header("Initial Properties Sliders")]
    [SerializeField] private Text dropdownPropertyText;
    [SerializeField] private Slider dropdownPropertySlider;
    [SerializeField] private Dropdown dropdownPropertyDropdown;
    [SerializeField] private InputField dropdownInputField;
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

    #region Initialisation
    void Awake()
    {
        SetDropDownPropertyValues();
        OnSelectDropdown();
    }

    private void SetDropDownPropertyValues()
    {
        List<string> propertyOptions = new List<string>();
        propertyOptions.Add("None");
        propertyOptions.Add("RabbitAgeMax");
        propertyOptions.Add("RabbitNutritionalValue");
        propertyOptions.Add("RabbitHungerMax");
        propertyOptions.Add("RabbitHungerThreshold");
        propertyOptions.Add("RabbitHungerIncreaseBase");
        propertyOptions.Add("RabbitHungerIncreaseYoung");
        propertyOptions.Add("RabbitHungerIncreaseAdult");
        propertyOptions.Add("RabbitHungerIncreaseOld");
        propertyOptions.Add("RabbitEatingSpeed");
        propertyOptions.Add("RabbitThirstMax");
        propertyOptions.Add("RabbitThirstThreshold");
        propertyOptions.Add("RabbitThirstIncreaseBase");
        propertyOptions.Add("RabbitDrinkingSpeed");
        propertyOptions.Add("RabbitMatingDuration");
        propertyOptions.Add("RabbitPregnancyLength");
        propertyOptions.Add("RabbitBirthDuration");
        propertyOptions.Add("RabbitLitterSizeMin");
        propertyOptions.Add("RabbitLitterSizeMax");
        propertyOptions.Add("RabbitLitterSizeAve");
        propertyOptions.Add("RabbitMovementSpeed");
        propertyOptions.Add("RabbitMovementMultiplierBase");
        propertyOptions.Add("RabbitMovementMultiplierYoung");
        propertyOptions.Add("RabbitMovementMultiplierAdult");
        propertyOptions.Add("RabbitMovementMultiplierOld");
        propertyOptions.Add("RabbitMovementMultiplierPregnant");
        propertyOptions.Add("RabbitSightRadius");
        propertyOptions.Add("RabbitSizeMale");
        propertyOptions.Add("RabbitSizeFemale");


        propertyOptions.Add("FoxAgeMax");
        propertyOptions.Add("FoxNutritionalValue");
        propertyOptions.Add("FoxHungerMax");
        propertyOptions.Add("FoxHungerThreshold");
        propertyOptions.Add("FoxHungerIncreaseBase");
        propertyOptions.Add("FoxHungerIncreaseYoung");
        propertyOptions.Add("FoxHungerIncreaseAdult");
        propertyOptions.Add("FoxHungerIncreaseOld");
        propertyOptions.Add("FoxEatingSpeed");
        propertyOptions.Add("FoxThirstMax");
        propertyOptions.Add("FoxThirstThreshold");
        propertyOptions.Add("FoxThirstIncreaseBase");
        propertyOptions.Add("FoxDrinkingSpeed");
        propertyOptions.Add("FoxMatingDuration");
        propertyOptions.Add("FoxPregnancyLength");
        propertyOptions.Add("FoxBirthDuration");
        propertyOptions.Add("FoxLitterSizeMin");
        propertyOptions.Add("FoxLitterSizeMax");
        propertyOptions.Add("FoxLitterSizeAve");
        propertyOptions.Add("FoxMovementSpeed");
        propertyOptions.Add("FoxMovementMultiplierBase");
        propertyOptions.Add("FoxMovementMultiplierYoung");
        propertyOptions.Add("FoxMovementMultiplierAdult");
        propertyOptions.Add("FoxMovementMultiplierOld");
        propertyOptions.Add("FoxMovementMultiplierPregnant");
        propertyOptions.Add("FoxSightRadius");
        propertyOptions.Add("FoxSizeMale");
        propertyOptions.Add("FoxSizeFemale");


        propertyOptions.Add("GrassNutritionalValue");
        propertyOptions.Add("GrassSize");

        dropdownPropertyDropdown.ClearOptions();
        dropdownPropertyDropdown.AddOptions(propertyOptions);
    }

    // Start is called before the first frame update
    void Start()
    {
        uiSliderCanvas.SetActive(true);
        uiTimeCanvas.SetActive(true);
        uiTurnCanvas.SetActive(true);
    }
    #endregion

    public void OnSelectDropdown()
    {
        string selectedOption = dropdownPropertyDropdown.options[dropdownPropertyDropdown.value].text;
        switch (selectedOption)
        {
            case "None":
                //used as the default when started so nothing runs
                break;
            case "RabbitAgeMax":
                dropdownInputField.text = RabbitDefaults.ageMax.ToString();
                ////dropdownPropertySlider.value = RabbitDefaults.ageMax;
                //dropdownPropertyText.text = RabbitDefaults.ageMax.ToString();
                break;
            case "RabbitNutritionalValue":
                //dropdownPropertySlider.value = RabbitDefaults.nutritionalValue;
                dropdownInputField.text = RabbitDefaults.nutritionalValue.ToString();
                break;
            case "RabbitCanBeEaten":
                //rabbitCanBeEaten;
                //RabbitDefaults.canBeEaten = foxCan
                break;
            case "RabbitHungerMax":
                //dropdownPropertySlider.value = RabbitDefaults.hungerMax;
                dropdownInputField.text = RabbitDefaults.hungerMax.ToString();
                break;
            case "RabbitHungerThreshold":
                //dropdownPropertySlider.value = RabbitDefaults.hungryThreshold;
                dropdownInputField.text = RabbitDefaults.hungryThreshold.ToString();
                break;
            case "RabbitHungerIncreaseBase":
                //dropdownPropertySlider.value = RabbitDefaults.hungerIncrease;
                dropdownInputField.text = RabbitDefaults.hungerIncrease.ToString();
                break;
            case "RabbitHungerIncreaseYoung":
                //dropdownPropertySlider.value = RabbitDefaults.youngHungerIncrease;
                dropdownInputField.text = RabbitDefaults.youngHungerIncrease.ToString();
                break;
            case "RabbitHungerIncreaseAdult":
                //dropdownPropertySlider.value = RabbitDefaults.adultHungerIncrease;
                dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();
                break;
            case "RabbitHungerIncreaseOld":
                //dropdownPropertySlider.value = RabbitDefaults.oldHungerIncrease;
                dropdownInputField.text = RabbitDefaults.oldHungerIncrease.ToString();
                break;
            case "RabbitEatingSpeed":
                //dropdownPropertySlider.value = RabbitDefaults.eatingSpeed;
                dropdownInputField.text = RabbitDefaults.eatingSpeed.ToString();
                break;
            case "RabbitThirstMax":
                //dropdownPropertySlider.value = RabbitDefaults.thirstMax;
                dropdownInputField.text = RabbitDefaults.thirstMax.ToString();
                break;
            case "RabbitThirstThreshold":
                //dropdownPropertySlider.value = RabbitDefaults.thirstyThreshold;
                dropdownInputField.text = RabbitDefaults.thirstyThreshold.ToString();
                break;
            case "RabbitThirstIncreaseBase":
                //dropdownPropertySlider.value = RabbitDefaults.thirstIncrease;
                dropdownInputField.text = RabbitDefaults.thirstIncrease.ToString();
                break;
            case "RabbitThirstIncreaseYoung":
                ////dropdownPropertySlider.value = RabbitDefaults.scaleFemale;
                //dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();    
                break;
            case "RabbitThirstIncreaseAdult":
                ////dropdownPropertySlider.value = RabbitDefaults.scaleFemale;
                //dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();   
                break;
            case "RabbitThirstIncreaseOld":
                ////dropdownPropertySlider.value = RabbitDefaults.scaleFemale;
                //dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();   
                break;
            case "RabbitDrinkingSpeed":
                //dropdownPropertySlider.value = RabbitDefaults.drinkingSpeed;
                dropdownInputField.text = RabbitDefaults.drinkingSpeed.ToString();
                break;
            case "RabbitMatingDuration":
                //dropdownPropertySlider.value = RabbitDefaults.matingDuration;
                dropdownInputField.text = RabbitDefaults.matingDuration.ToString();
                break;
            case "RabbitPregnancyLength":
                //dropdownPropertySlider.value = RabbitDefaults.pregnancyLength;
                dropdownInputField.text = RabbitDefaults.pregnancyLength.ToString();
                break;
            case "RabbitBirthDuration":
                //dropdownPropertySlider.value = RabbitDefaults.birthDuration;
                dropdownInputField.text = RabbitDefaults.birthDuration.ToString();
                break;
            case "RabbitLitterSizeMin":
                //dropdownPropertySlider.value = RabbitDefaults.litterSizeMin;
                dropdownInputField.text = RabbitDefaults.litterSizeMin.ToString();
                break;
            case "RabbitLitterSizeMax":
                //dropdownPropertySlider.value = RabbitDefaults.litterSizeMax;
                dropdownInputField.text = RabbitDefaults.litterSizeMax.ToString();
                break;
            case "RabbitLitterSizeAve":
                //dropdownPropertySlider.value = RabbitDefaults.litterSizeAve;
                dropdownInputField.text = RabbitDefaults.litterSizeAve.ToString();
                break;
            case "RabbitMovementSpeed":
                //dropdownPropertySlider.value = RabbitDefaults.moveSpeed;
                dropdownInputField.text = RabbitDefaults.moveSpeed.ToString();
                break;
            case "RabbitMovementMultiplierBase":
                //dropdownPropertySlider.value = RabbitDefaults.originalMoveMultiplier;
                dropdownInputField.text = RabbitDefaults.originalMoveMultiplier.ToString();
                break;
            case "RabbitMovementMultiplierYoung":
                //dropdownPropertySlider.value = RabbitDefaults.youngMoveMultiplier;
                dropdownInputField.text = RabbitDefaults.youngMoveMultiplier.ToString();
                break;
            case "RabbitMovementMultiplierAdult":
                //dropdownPropertySlider.value = RabbitDefaults.adultMoveMultiplier;
                dropdownInputField.text = RabbitDefaults.adultMoveMultiplier.ToString();
                break;
            case "RabbitMovementMultiplierOld":
                //dropdownPropertySlider.value = RabbitDefaults.oldMoveMultiplier;
                dropdownInputField.text = RabbitDefaults.oldMoveMultiplier.ToString();
                break;
            case "RabbitMovementMultiplierPregnant":
                //dropdownPropertySlider.value = RabbitDefaults.pregnancyMoveMultiplier;
                dropdownInputField.text = RabbitDefaults.pregnancyMoveMultiplier.ToString();
                break;
            case "RabbitSightRadius":
                //dropdownPropertySlider.value = RabbitDefaults.sightRadius;
                dropdownInputField.text = RabbitDefaults.sightRadius.ToString();
                break;
            case "RabbitSizeMale":
                //dropdownPropertySlider.value = RabbitDefaults.scaleMale;
                dropdownInputField.text = RabbitDefaults.scaleMale.ToString();
                break;
            case "RabbitSizeFemale":
                //dropdownPropertySlider.value = RabbitDefaults.scaleFemale;
                dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();
                break;
            case "FoxAgeMax":
                //dropdownPropertySlider.value = FoxDefaults.ageMax;
                dropdownInputField.text = FoxDefaults.ageMax.ToString();
                break;
            case "FoxNutritionalValue":
                //dropdownPropertySlider.value = FoxDefaults.nutritionalValue;
                dropdownInputField.text = FoxDefaults.nutritionalValue.ToString();
                break;
            case "FoxCanBeEaten":
                //foxCanBeEaten;
                //FoxDefaults.canBeEaten = foxCan
                break;
            case "FoxHungerMax":
                //dropdownPropertySlider.value = FoxDefaults.hungerMax;
                dropdownInputField.text = FoxDefaults.hungerMax.ToString();
                break;
            case "FoxHungerThreshold":
                //dropdownPropertySlider.value = FoxDefaults.hungryThreshold;
                dropdownInputField.text = FoxDefaults.hungryThreshold.ToString();
                break;
            case "FoxHungerIncreaseBase":
                //dropdownPropertySlider.value = FoxDefaults.hungerIncrease;
                dropdownInputField.text = FoxDefaults.hungerIncrease.ToString();
                break;
            case "FoxHungerIncreaseYoung":
                //dropdownPropertySlider.value = FoxDefaults.youngHungerIncrease;
                dropdownInputField.text = FoxDefaults.youngHungerIncrease.ToString();
                break;
            case "FoxHungerIncreaseAdult":
                //dropdownPropertySlider.value = FoxDefaults.adultHungerIncrease;
                dropdownInputField.text = FoxDefaults.scaleFemale.ToString();
                break;
            case "FoxHungerIncreaseOld":
                //dropdownPropertySlider.value = FoxDefaults.oldHungerIncrease;
                dropdownInputField.text = FoxDefaults.oldHungerIncrease.ToString();
                break;
            case "FoxEatingSpeed":
                //dropdownPropertySlider.value = FoxDefaults.eatingSpeed;
                dropdownInputField.text = FoxDefaults.eatingSpeed.ToString();
                break;
            case "FoxThirstMax":
                //dropdownPropertySlider.value = FoxDefaults.thirstMax;
                dropdownInputField.text = FoxDefaults.thirstMax.ToString();
                break;
            case "FoxThirstThreshold":
                //dropdownPropertySlider.value = FoxDefaults.thirstyThreshold;
                dropdownInputField.text = FoxDefaults.thirstyThreshold.ToString();
                break;
            case "FoxThirstIncreaseBase":
                //dropdownPropertySlider.value = FoxDefaults.thirstIncrease;
                dropdownInputField.text = FoxDefaults.thirstIncrease.ToString();
                break;
            case "FoxThirstIncreaseYoung":
                ////dropdownPropertySlider.value = FoxDefaults.scaleFemale;
                //dropdownInputField.text = FoxDefaults.scaleFemale.ToString();    
                break;
            case "FoxThirstIncreaseAdult":
                ////dropdownPropertySlider.value = FoxDefaults.scaleFemale;
                //dropdownInputField.text = FoxDefaults.scaleFemale.ToString();   
                break;
            case "FoxThirstIncreaseOld":
                ////dropdownPropertySlider.value = FoxDefaults.scaleFemale;
                //dropdownInputField.text = FoxDefaults.scaleFemale.ToString();   
                break;
            case "FoxDrinkingSpeed":
                //dropdownPropertySlider.value = FoxDefaults.drinkingSpeed;
                dropdownInputField.text = FoxDefaults.drinkingSpeed.ToString();
                break;
            case "FoxMatingDuration":
                //dropdownPropertySlider.value = FoxDefaults.matingDuration;
                dropdownInputField.text = FoxDefaults.matingDuration.ToString();
                break;
            case "FoxPregnancyLength":
                //dropdownPropertySlider.value = FoxDefaults.pregnancyLength;
                dropdownInputField.text = FoxDefaults.pregnancyLength.ToString();
                break;
            case "FoxBirthDuration":
                //dropdownPropertySlider.value = FoxDefaults.birthDuration;
                dropdownInputField.text = FoxDefaults.birthDuration.ToString();
                break;
            case "FoxLitterSizeMin":
                //dropdownPropertySlider.value = FoxDefaults.litterSizeMin;
                dropdownInputField.text = FoxDefaults.litterSizeMin.ToString();
                break;
            case "FoxLitterSizeMax":
                //dropdownPropertySlider.value = FoxDefaults.litterSizeMax;
                dropdownInputField.text = FoxDefaults.litterSizeMax.ToString();
                break;
            case "FoxLitterSizeAve":
                //dropdownPropertySlider.value = FoxDefaults.litterSizeAve;
                dropdownInputField.text = FoxDefaults.litterSizeAve.ToString();
                break;
            case "FoxMovementSpeed":
                //dropdownPropertySlider.value = FoxDefaults.moveSpeed;
                dropdownInputField.text = FoxDefaults.moveSpeed.ToString();
                break;
            case "FoxMovementMultiplierBase":
                //dropdownPropertySlider.value = FoxDefaults.originalMoveMultiplier;
                dropdownInputField.text = FoxDefaults.originalMoveMultiplier.ToString();
                break;
            case "FoxMovementMultiplierYoung":
                //dropdownPropertySlider.value = FoxDefaults.youngMoveMultiplier;
                dropdownInputField.text = FoxDefaults.youngMoveMultiplier.ToString();
                break;
            case "FoxMovementMultiplierAdult":
                //dropdownPropertySlider.value = FoxDefaults.adultMoveMultiplier;
                dropdownInputField.text = FoxDefaults.adultMoveMultiplier.ToString();
                break;
            case "FoxMovementMultiplierOld":
                //dropdownPropertySlider.value = FoxDefaults.oldMoveMultiplier;
                dropdownInputField.text = FoxDefaults.oldMoveMultiplier.ToString();
                break;
            case "FoxMovementMultiplierPregnant":
                //dropdownPropertySlider.value = FoxDefaults.pregnancyMoveMultiplier;
                dropdownInputField.text = FoxDefaults.pregnancyMoveMultiplier.ToString();
                break;
            case "FoxSightRadius":
                //dropdownPropertySlider.value = FoxDefaults.sightRadius;
                dropdownInputField.text = FoxDefaults.sightRadius.ToString();
                break;
            case "FoxSizeMale":
                //dropdownPropertySlider.value = FoxDefaults.scaleMale;
                dropdownInputField.text = FoxDefaults.scaleMale.ToString();
                break;
            case "FoxSizeFemale":
                //dropdownPropertySlider.value = FoxDefaults.scaleFemale;
                dropdownInputField.text = FoxDefaults.scaleFemale.ToString();
                break;
            case "GrassNutritionalValue":
                //dropdownPropertySlider.value = GrassDefaults.nutritionalValue;
                dropdownInputField.text = GrassDefaults.nutritionalValue.ToString();
                break;
            case "GrassCanBeEaten":
                //grassCanBeEaten;
                //GrassDefaults.canBeEaten = grassCa
                break;
            case "GrassSize":
                //dropdownPropertySlider.value = GrassDefaults.scale;
                dropdownInputField.text = GrassDefaults.scale.ToString();
                break;
            default:
                Debug.LogWarning("Attempted to update unknown property in switch: " + selectedOption, this);
                break;
        }
    }

    public void DropdownPropertiesUpdate()
    {
        string selectedOption = dropdownPropertyDropdown.options[dropdownPropertyDropdown.value].text;
        Debug.Log($"{selectedOption}: {float.Parse(dropdownInputField.text)}");
        switch (selectedOption)
        {
            case "None":
                break;
            case "RabbitAgeMax":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.ageMax = float.Parse(dropdownInputField.text);
                break;
            case "RabbitNutritionalValue":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.nutritionalValue = float.Parse(dropdownInputField.text);
                break;
            case "RabbitCanBeEaten":
                //rabbitCanBeEaten;
                //RabbitDefaults.canBeEaten = rabbitCanBeEaten.value;
                break;
            case "RabbitHungerMax":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.hungerMax = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerThreshold":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.hungryThreshold = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerIncreaseBase":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.hungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerIncreaseYoung":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.youngHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerIncreaseAdult":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.adultHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerIncreaseOld":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.oldHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitEatingSpeed":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.eatingSpeed = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstMax":
                //dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.thirstMax = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstThreshold":
                // dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.thirstyThreshold = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstIncreaseBase":
                //     dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.thirstIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstIncreaseYoung":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                //RabbitDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstIncreaseAdult":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                //RabbitDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstIncreaseOld":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                //RabbitDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "RabbitDrinkingSpeed":
                //  dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.drinkingSpeed = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMatingDuration":
                //  dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.matingDuration = float.Parse(dropdownInputField.text);
                break;
            case "RabbitPregnancyLength":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.pregnancyLength = float.Parse(dropdownInputField.text);
                break;
            case "RabbitBirthDuration":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.birthDuration = float.Parse(dropdownInputField.text);
                break;
            case "RabbitLitterSizeMin":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.litterSizeMin = int.Parse(dropdownInputField.text);
                break;
            case "RabbitLitterSizeMax":
                //    dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.litterSizeMax = int.Parse(dropdownInputField.text);
                break;
            case "RabbitLitterSizeAve":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.litterSizeAve = int.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementSpeed":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.moveSpeed = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierBase":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.originalMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierYoung":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.youngMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierAdult":
                //    dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.adultMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierOld":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.oldMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierPregnant":
                //    dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.pregnancyMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitSightRadius":
                //    dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.sightRadius = float.Parse(dropdownInputField.text);
                break;
            case "RabbitSizeMale":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.scaleMale = float.Parse(dropdownInputField.text);
                break;
            case "RabbitSizeFemale":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                RabbitDefaults.scaleFemale = float.Parse(dropdownInputField.text);
                break;
            case "FoxAgeMax":
                //    dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.ageMax = float.Parse(dropdownInputField.text);
                break;
            case "FoxNutritionalValue":
                //    dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.nutritionalValue = float.Parse(dropdownInputField.text);
                break;
            case "FoxCanBeEaten":
                //foxCanBeEaten;
                //FoxDefaults.canBeEaten = foxCanBeEaten.value;
                break;
            case "FoxHungerMax":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.hungerMax = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerThreshold":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.hungryThreshold = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerIncreaseBase":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.hungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerIncreaseYoung":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.youngHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerIncreaseAdult":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.adultHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerIncreaseOld":
                //  dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.oldHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxEatingSpeed":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.eatingSpeed = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstMax":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.thirstMax = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstThreshold":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.thirstyThreshold = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstIncreaseBase":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.thirstIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstIncreaseYoung":
                //  dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                //FoxDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstIncreaseAdult":
                //  dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                //FoxDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstIncreaseOld":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                //FoxDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "FoxDrinkingSpeed":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.drinkingSpeed = float.Parse(dropdownInputField.text);
                break;
            case "FoxMatingDuration":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.matingDuration = float.Parse(dropdownInputField.text);
                break;
            case "FoxPregnancyLength":
                //    dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.pregnancyLength = float.Parse(dropdownInputField.text);
                break;
            case "FoxBirthDuration":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.birthDuration = float.Parse(dropdownInputField.text);
                break;
            case "FoxLitterSizeMin":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.litterSizeMin = int.Parse(dropdownInputField.text);
                break;
            case "FoxLitterSizeMax":
                //  dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.litterSizeMax = int.Parse(dropdownInputField.text);
                break;
            case "FoxLitterSizeAve":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.litterSizeAve = int.Parse(dropdownInputField.text);
                break;
            case "FoxMovementSpeed":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.moveSpeed = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierBase":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.originalMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierYoung":
                //    dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.youngMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierAdult":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.adultMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierOld":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.oldMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierPregnant":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.pregnancyMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxSightRadius":
                //  dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.sightRadius = float.Parse(dropdownInputField.text);
                break;
            case "FoxSizeMale":
                //  dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.scaleMale = float.Parse(dropdownInputField.text);
                break;
            case "FoxSizeFemale":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                FoxDefaults.scaleFemale = float.Parse(dropdownInputField.text);
                break;
            case "GrassNutritionalValue":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                GrassDefaults.nutritionalValue = float.Parse(dropdownInputField.text);
                break;
            case "GrassCanBeEaten":
                //grassCanBeEaten;
                //GrassDefaults.canBeEaten = grassCanBeEaten.value;
                break;
            case "GrassSize":
                //   dropdownPropertyText.text = float.Parse(dropdownInputField.text).ToString();
                GrassDefaults.scale = float.Parse(dropdownInputField.text);
                break;
            default:
                Debug.LogWarning("Attempted to update unknown property in switch: " + selectedOption, this);
                break;
        }
        UIUpdateSystem.Instance.somethingChangedFlag = true;
    }

    public void PropertiesUpdate(string propertyToUpdate)
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
            case "RabbitMovementSpeed":
                rabbitMovementSpeedText.text = rabbitMovementSpeedSlider.value.ToString();
                RabbitDefaults.moveSpeed = rabbitMovementSpeedSlider.value;
                break;
            default:
                Debug.LogWarning("Attempted to update unknown property in switch: " + propertyToUpdate);
                break;
        }
        UIUpdateSystem.Instance.somethingChangedFlag = true; // could be improved by having UIUpdateSystem check if value changed rather than needing flag, but this works

    }
}
