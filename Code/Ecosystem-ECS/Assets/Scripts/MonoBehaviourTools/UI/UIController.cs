using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region UI Canvas'
    [SerializeField] private GameObject uiSliderCanvas;
    [SerializeField] private GameObject uiTimeCanvas;
    [SerializeField] private GameObject uiTurnCanvas;
    [SerializeField] private GameObject uiGraphCanvas;

    #endregion

    #region Initial Properties Linking
    [Header("Initial Properties")]
    [SerializeField] private Dropdown dropdownPropertyDropdown;
    [SerializeField] private InputField dropdownInputField;
    #endregion

    #region Initialisation
    void Awake()
    {
        SetDropDownPropertyValues();
        OnSelectDropdown();
    }

    private void SetDropDownPropertyValues()
    {
        List<string> propertyOptions = new List<string>
        {
            "None",
            "RabbitAgeMax",
            "RabbitNutritionalValue",
            "RabbitCanBeEaten",
            "RabbitHungerMax",
            "RabbitHungerThreshold",
            "RabbitHungerIncreaseBase",
            "RabbitHungerIncreaseYoung",
            "RabbitHungerIncreaseAdult",
            "RabbitHungerIncreaseOld",
            "RabbitEatingSpeed",
            "RabbitThirstMax",
            "RabbitThirstThreshold",
            "RabbitThirstIncreaseBase",
            "RabbitDrinkingSpeed",
            "RabbitMatingDuration",
            "RabbitPregnancyLength",
            "RabbitBirthDuration",
            "RabbitLitterSizeMin",
            "RabbitLitterSizeMax",
            "RabbitLitterSizeAve",
            "RabbitMovementSpeed",
            "RabbitMovementMultiplierBase",
            "RabbitMovementMultiplierYoung",
            "RabbitMovementMultiplierAdult",
            "RabbitMovementMultiplierOld",
            "RabbitMovementMultiplierPregnant",
            "RabbitSightRadius",
            "RabbitSizeMale",
            "RabbitSizeFemale",


            "FoxAgeMax",
            "FoxNutritionalValue",
            "FoxCanBeEaten",
            "FoxHungerMax",
            "FoxHungerThreshold",
            "FoxHungerIncreaseBase",
            "FoxHungerIncreaseYoung",
            "FoxHungerIncreaseAdult",
            "FoxHungerIncreaseOld",
            "FoxEatingSpeed",
            "FoxThirstMax",
            "FoxThirstThreshold",
            "FoxThirstIncreaseBase",
            "FoxDrinkingSpeed",
            "FoxMatingDuration",
            "FoxPregnancyLength",
            "FoxBirthDuration",
            "FoxLitterSizeMin",
            "FoxLitterSizeMax",
            "FoxLitterSizeAve",
            "FoxMovementSpeed",
            "FoxMovementMultiplierBase",
            "FoxMovementMultiplierYoung",
            "FoxMovementMultiplierAdult",
            "FoxMovementMultiplierOld",
            "FoxMovementMultiplierPregnant",
            "FoxSightRadius",
            "FoxSizeMale",
            "FoxSizeFemale",


            "GrassNutritionalValue",
            "GrassCanBeEaten",
            "GrassSize"
        };

        dropdownPropertyDropdown.ClearOptions();
        dropdownPropertyDropdown.AddOptions(propertyOptions);
    }

    void Start()
    {
        uiSliderCanvas.SetActive(true);
        uiTimeCanvas.SetActive(true);
        uiTurnCanvas.SetActive(true);
        uiGraphCanvas.SetActive(true);
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
                break;
            case "RabbitNutritionalValue":
                dropdownInputField.text = RabbitDefaults.nutritionalValue.ToString();
                break;
            case "RabbitCanBeEaten":
                dropdownInputField.text = Convert.ToInt32(RabbitDefaults.canBeEaten).ToString();
                break;
            case "RabbitHungerMax":
                dropdownInputField.text = RabbitDefaults.hungerMax.ToString();
                break;
            case "RabbitHungerThreshold":
                dropdownInputField.text = RabbitDefaults.hungryThreshold.ToString();
                break;
            case "RabbitHungerIncreaseBase":
                dropdownInputField.text = RabbitDefaults.hungerIncrease.ToString();
                break;
            case "RabbitHungerIncreaseYoung":
                dropdownInputField.text = RabbitDefaults.youngHungerIncrease.ToString();
                break;
            case "RabbitHungerIncreaseAdult":
                dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();
                break;
            case "RabbitHungerIncreaseOld":
                dropdownInputField.text = RabbitDefaults.oldHungerIncrease.ToString();
                break;
            case "RabbitEatingSpeed":
                dropdownInputField.text = RabbitDefaults.eatingSpeed.ToString();
                break;
            case "RabbitThirstMax":
                dropdownInputField.text = RabbitDefaults.thirstMax.ToString();
                break;
            case "RabbitThirstThreshold":
                dropdownInputField.text = RabbitDefaults.thirstyThreshold.ToString();
                break;
            case "RabbitThirstIncreaseBase":
                dropdownInputField.text = RabbitDefaults.thirstIncrease.ToString();
                break;
            case "RabbitThirstIncreaseYoung":
                //dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();
                break;
            case "RabbitThirstIncreaseAdult":
                //dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();
                break;
            case "RabbitThirstIncreaseOld":
                //dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();
                break;
            case "RabbitDrinkingSpeed":
                dropdownInputField.text = RabbitDefaults.drinkingSpeed.ToString();
                break;
            case "RabbitMatingDuration":
                dropdownInputField.text = RabbitDefaults.matingDuration.ToString();
                break;
            case "RabbitPregnancyLength":
                dropdownInputField.text = RabbitDefaults.pregnancyLength.ToString();
                break;
            case "RabbitBirthDuration":
                dropdownInputField.text = RabbitDefaults.birthDuration.ToString();
                break;
            case "RabbitLitterSizeMin":
                dropdownInputField.text = RabbitDefaults.litterSizeMin.ToString();
                break;
            case "RabbitLitterSizeMax":
                dropdownInputField.text = RabbitDefaults.litterSizeMax.ToString();
                break;
            case "RabbitLitterSizeAve":
                dropdownInputField.text = RabbitDefaults.litterSizeAve.ToString();
                break;
            case "RabbitMovementSpeed":
                dropdownInputField.text = RabbitDefaults.moveSpeed.ToString();
                break;
            case "RabbitMovementMultiplierBase":
                dropdownInputField.text = RabbitDefaults.originalMoveMultiplier.ToString();
                break;
            case "RabbitMovementMultiplierYoung":
                dropdownInputField.text = RabbitDefaults.youngMoveMultiplier.ToString();
                break;
            case "RabbitMovementMultiplierAdult":
                dropdownInputField.text = RabbitDefaults.adultMoveMultiplier.ToString();
                break;
            case "RabbitMovementMultiplierOld":
                dropdownInputField.text = RabbitDefaults.oldMoveMultiplier.ToString();
                break;
            case "RabbitMovementMultiplierPregnant":
                dropdownInputField.text = RabbitDefaults.pregnancyMoveMultiplier.ToString();
                break;
            case "RabbitSightRadius":
                dropdownInputField.text = RabbitDefaults.sightRadius.ToString();
                break;
            case "RabbitSizeMale":
                dropdownInputField.text = RabbitDefaults.scaleMale.ToString();
                break;
            case "RabbitSizeFemale":
                dropdownInputField.text = RabbitDefaults.scaleFemale.ToString();
                break;
            case "FoxAgeMax":
                dropdownInputField.text = FoxDefaults.ageMax.ToString();
                break;
            case "FoxNutritionalValue":
                dropdownInputField.text = FoxDefaults.nutritionalValue.ToString();
                break;
            case "FoxCanBeEaten":
                dropdownInputField.text = Convert.ToInt32(FoxDefaults.canBeEaten).ToString();
                break;
            case "FoxHungerMax":
                dropdownInputField.text = FoxDefaults.hungerMax.ToString();
                break;
            case "FoxHungerThreshold":
                dropdownInputField.text = FoxDefaults.hungryThreshold.ToString();
                break;
            case "FoxHungerIncreaseBase":
                dropdownInputField.text = FoxDefaults.hungerIncrease.ToString();
                break;
            case "FoxHungerIncreaseYoung":
                dropdownInputField.text = FoxDefaults.youngHungerIncrease.ToString();
                break;
            case "FoxHungerIncreaseAdult":
                dropdownInputField.text = FoxDefaults.scaleFemale.ToString();
                break;
            case "FoxHungerIncreaseOld":
                dropdownInputField.text = FoxDefaults.oldHungerIncrease.ToString();
                break;
            case "FoxEatingSpeed":
                dropdownInputField.text = FoxDefaults.eatingSpeed.ToString();
                break;
            case "FoxThirstMax":
                dropdownInputField.text = FoxDefaults.thirstMax.ToString();
                break;
            case "FoxThirstThreshold":
                dropdownInputField.text = FoxDefaults.thirstyThreshold.ToString();
                break;
            case "FoxThirstIncreaseBase":
                dropdownInputField.text = FoxDefaults.thirstIncrease.ToString();
                break;
            case "FoxThirstIncreaseYoung":
                //dropdownInputField.text = FoxDefaults.scaleFemale.ToString();
                break;
            case "FoxThirstIncreaseAdult":
                //dropdownInputField.text = FoxDefaults.scaleFemale.ToString();
                break;
            case "FoxThirstIncreaseOld":
                //dropdownInputField.text = FoxDefaults.scaleFemale.ToString();
                break;
            case "FoxDrinkingSpeed":
                dropdownInputField.text = FoxDefaults.drinkingSpeed.ToString();
                break;
            case "FoxMatingDuration":
                dropdownInputField.text = FoxDefaults.matingDuration.ToString();
                break;
            case "FoxPregnancyLength":
                dropdownInputField.text = FoxDefaults.pregnancyLength.ToString();
                break;
            case "FoxBirthDuration":
                dropdownInputField.text = FoxDefaults.birthDuration.ToString();
                break;
            case "FoxLitterSizeMin":
                dropdownInputField.text = FoxDefaults.litterSizeMin.ToString();
                break;
            case "FoxLitterSizeMax":
                dropdownInputField.text = FoxDefaults.litterSizeMax.ToString();
                break;
            case "FoxLitterSizeAve":
                dropdownInputField.text = FoxDefaults.litterSizeAve.ToString();
                break;
            case "FoxMovementSpeed":
                dropdownInputField.text = FoxDefaults.moveSpeed.ToString();
                break;
            case "FoxMovementMultiplierBase":
                dropdownInputField.text = FoxDefaults.originalMoveMultiplier.ToString();
                break;
            case "FoxMovementMultiplierYoung":
                dropdownInputField.text = FoxDefaults.youngMoveMultiplier.ToString();
                break;
            case "FoxMovementMultiplierAdult":
                dropdownInputField.text = FoxDefaults.adultMoveMultiplier.ToString();
                break;
            case "FoxMovementMultiplierOld":
                dropdownInputField.text = FoxDefaults.oldMoveMultiplier.ToString();
                break;
            case "FoxMovementMultiplierPregnant":
                dropdownInputField.text = FoxDefaults.pregnancyMoveMultiplier.ToString();
                break;
            case "FoxSightRadius":
                dropdownInputField.text = FoxDefaults.sightRadius.ToString();
                break;
            case "FoxSizeMale":
                dropdownInputField.text = FoxDefaults.scaleMale.ToString();
                break;
            case "FoxSizeFemale":
                dropdownInputField.text = FoxDefaults.scaleFemale.ToString();
                break;
            case "GrassNutritionalValue":
                dropdownInputField.text = GrassDefaults.nutritionalValue.ToString();
                break;
            case "GrassCanBeEaten":
                dropdownInputField.text = Convert.ToInt32(GrassDefaults.canBeEaten).ToString();
                break;
            case "GrassSize":
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
        //Debug.Log($"{selectedOption}: {float.Parse(dropdownInputField.text)}");
        switch (selectedOption)
        {
            case "None":
                break;
            case "RabbitAgeMax":
                RabbitDefaults.ageMax = float.Parse(dropdownInputField.text);
                break;
            case "RabbitNutritionalValue":
                RabbitDefaults.nutritionalValue = float.Parse(dropdownInputField.text);
                break;
            case "RabbitCanBeEaten":
                if (int.Parse(dropdownInputField.text) == 0)
                    RabbitDefaults.canBeEaten = false;
                else
                    RabbitDefaults.canBeEaten = true;
                break;
            case "RabbitHungerMax":
                RabbitDefaults.hungerMax = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerThreshold":
                RabbitDefaults.hungryThreshold = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerIncreaseBase":
                RabbitDefaults.hungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerIncreaseYoung":
                RabbitDefaults.youngHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerIncreaseAdult":
                RabbitDefaults.adultHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitHungerIncreaseOld":
                RabbitDefaults.oldHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitEatingSpeed":
                RabbitDefaults.eatingSpeed = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstMax":
                RabbitDefaults.thirstMax = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstThreshold":
                RabbitDefaults.thirstyThreshold = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstIncreaseBase":
                RabbitDefaults.thirstIncrease = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstIncreaseYoung":
                //RabbitDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstIncreaseAdult":
                //RabbitDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "RabbitThirstIncreaseOld":
                //RabbitDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "RabbitDrinkingSpeed":
                RabbitDefaults.drinkingSpeed = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMatingDuration":
                RabbitDefaults.matingDuration = float.Parse(dropdownInputField.text);
                break;
            case "RabbitPregnancyLength":
                RabbitDefaults.pregnancyLength = float.Parse(dropdownInputField.text);
                break;
            case "RabbitBirthDuration":
                RabbitDefaults.birthDuration = float.Parse(dropdownInputField.text);
                break;
            case "RabbitLitterSizeMin":
                RabbitDefaults.litterSizeMin = int.Parse(dropdownInputField.text);
                break;
            case "RabbitLitterSizeMax":
                RabbitDefaults.litterSizeMax = int.Parse(dropdownInputField.text);
                break;
            case "RabbitLitterSizeAve":
                RabbitDefaults.litterSizeAve = int.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementSpeed":
                RabbitDefaults.moveSpeed = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierBase":
                RabbitDefaults.originalMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierYoung":
                RabbitDefaults.youngMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierAdult":
                RabbitDefaults.adultMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierOld":
                RabbitDefaults.oldMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitMovementMultiplierPregnant":
                RabbitDefaults.pregnancyMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "RabbitSightRadius":
                RabbitDefaults.sightRadius = float.Parse(dropdownInputField.text);
                break;
            case "RabbitSizeMale":
                RabbitDefaults.scaleMale = float.Parse(dropdownInputField.text);
                break;
            case "RabbitSizeFemale":
                RabbitDefaults.scaleFemale = float.Parse(dropdownInputField.text);
                break;
            case "FoxAgeMax":
                FoxDefaults.ageMax = float.Parse(dropdownInputField.text);
                break;
            case "FoxNutritionalValue":
                FoxDefaults.nutritionalValue = float.Parse(dropdownInputField.text);
                break;
            case "FoxCanBeEaten":
                if (int.Parse(dropdownInputField.text) == 0)
                    FoxDefaults.canBeEaten = false;
                else
                    FoxDefaults.canBeEaten = true;
                break;
            case "FoxHungerMax":
                FoxDefaults.hungerMax = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerThreshold":
                FoxDefaults.hungryThreshold = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerIncreaseBase":
                FoxDefaults.hungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerIncreaseYoung":
                FoxDefaults.youngHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerIncreaseAdult":
                FoxDefaults.adultHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxHungerIncreaseOld":
                FoxDefaults.oldHungerIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxEatingSpeed":
                FoxDefaults.eatingSpeed = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstMax":
                FoxDefaults.thirstMax = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstThreshold":
                FoxDefaults.thirstyThreshold = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstIncreaseBase":
                FoxDefaults.thirstIncrease = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstIncreaseYoung":
                //FoxDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstIncreaseAdult":
                //FoxDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "FoxThirstIncreaseOld":
                //FoxDefaults. = float.Parse(dropdownInputField.text);
                break;
            case "FoxDrinkingSpeed":
                FoxDefaults.drinkingSpeed = float.Parse(dropdownInputField.text);
                break;
            case "FoxMatingDuration":
                FoxDefaults.matingDuration = float.Parse(dropdownInputField.text);
                break;
            case "FoxPregnancyLength":
                FoxDefaults.pregnancyLength = float.Parse(dropdownInputField.text);
                break;
            case "FoxBirthDuration":
                FoxDefaults.birthDuration = float.Parse(dropdownInputField.text);
                break;
            case "FoxLitterSizeMin":
                FoxDefaults.litterSizeMin = int.Parse(dropdownInputField.text);
                break;
            case "FoxLitterSizeMax":
                FoxDefaults.litterSizeMax = int.Parse(dropdownInputField.text);
                break;
            case "FoxLitterSizeAve":
                FoxDefaults.litterSizeAve = int.Parse(dropdownInputField.text);
                break;
            case "FoxMovementSpeed":
                FoxDefaults.moveSpeed = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierBase":
                FoxDefaults.originalMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierYoung":
                FoxDefaults.youngMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierAdult":
                FoxDefaults.adultMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierOld":
                FoxDefaults.oldMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxMovementMultiplierPregnant":
                FoxDefaults.pregnancyMoveMultiplier = float.Parse(dropdownInputField.text);
                break;
            case "FoxSightRadius":
                FoxDefaults.sightRadius = float.Parse(dropdownInputField.text);
                break;
            case "FoxSizeMale":
                FoxDefaults.scaleMale = float.Parse(dropdownInputField.text);
                break;
            case "FoxSizeFemale":
                FoxDefaults.scaleFemale = float.Parse(dropdownInputField.text);
                break;
            case "GrassNutritionalValue":
                GrassDefaults.nutritionalValue = float.Parse(dropdownInputField.text);
                break;
            case "GrassCanBeEaten":
                if (int.Parse(dropdownInputField.text) == 0)
                    GrassDefaults.canBeEaten = false;
                else
                    GrassDefaults.canBeEaten = true;
                break;
            case "GrassSize":
                GrassDefaults.scale = float.Parse(dropdownInputField.text);
                break;
            default:
                Debug.LogWarning("Attempted to update unknown property in switch: " + selectedOption, this);
                break;
        }
        UIUpdateSystem.Instance.somethingChangedFlag = true;
    }
}
