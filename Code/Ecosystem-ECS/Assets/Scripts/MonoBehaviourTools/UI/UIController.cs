using System;
using System.Collections.Generic;
using Systems;
using EntityDefaults;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.UI
{
    public class UIController : MonoBehaviour
    {
        #region UI Canvas'

        [SerializeField] private GameObject _uiSliderCanvas;
        [SerializeField] private GameObject _uiTimeCanvas;
        [SerializeField] private GameObject _uiTurnCanvas;
        [SerializeField] private GameObject _uiGraphCanvas;

        #endregion

        #region Initial Properties Linking
        [Header("Initial Properties")]
        [SerializeField] private Dropdown _dropdownPropertyDropdown;
        [SerializeField] private InputField _dropdownInputField;
        #endregion

        #region Initialisation
        void Awake()
        {
            SetDropDownPropertyValues();
            OnSelectDropdown();
        }

        /*Sets the list of property options available in the dropdown*/
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
            "RabbitMatingThreshold",
            "RabbitReproductiveUrgeIncrease",
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
            "FoxMatingThreshold",
            "FoxReproductiveUrgeIncrease",
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

            _dropdownPropertyDropdown.ClearOptions();
            _dropdownPropertyDropdown.AddOptions(propertyOptions);
        }

        void Start()
        {
            _uiSliderCanvas.SetActive(true);
            _uiTimeCanvas.SetActive(true);
            _uiTurnCanvas.SetActive(true);
            _uiGraphCanvas.SetActive(true);
        }

        #endregion

        /*change the input field text to the current value of the selected property from the dropdown*/
        public void OnSelectDropdown()
        {
            string selectedOption = _dropdownPropertyDropdown.options[_dropdownPropertyDropdown.value].text;
            switch (selectedOption)
            {
                case "None":
                    //used as the default when started so nothing runs
                    break;
                case "RabbitAgeMax":
                    _dropdownInputField.text = RabbitDefaults.AgeMax.ToString("N");
                    break;
                case "RabbitNutritionalValue":
                    _dropdownInputField.text = RabbitDefaults.NutritionalValue.ToString("N");
                    break;
                case "RabbitCanBeEaten":
                    _dropdownInputField.text = Convert.ToInt32(RabbitDefaults.CanBeEaten).ToString("N");
                    break;
                case "RabbitHungerMax":
                    _dropdownInputField.text = RabbitDefaults.HungerMax.ToString("N");
                    break;
                case "RabbitHungerThreshold":
                    _dropdownInputField.text = RabbitDefaults.HungryThreshold.ToString("N");
                    break;
                case "RabbitHungerIncreaseBase":
                    _dropdownInputField.text = RabbitDefaults.HungerIncrease.ToString("N");
                    break;
                case "RabbitHungerIncreaseYoung":
                    _dropdownInputField.text = RabbitDefaults.YoungHungerIncrease.ToString("N");
                    break;
                case "RabbitHungerIncreaseAdult":
                    _dropdownInputField.text = RabbitDefaults.ScaleFemale.ToString("N");
                    break;
                case "RabbitHungerIncreaseOld":
                    _dropdownInputField.text = RabbitDefaults.OldHungerIncrease.ToString("N");
                    break;
                case "RabbitEatingSpeed":
                    _dropdownInputField.text = RabbitDefaults.EatingSpeed.ToString("N");
                    break;
                case "RabbitThirstMax":
                    _dropdownInputField.text = RabbitDefaults.ThirstMax.ToString("N");
                    break;
                case "RabbitThirstThreshold":
                    _dropdownInputField.text = RabbitDefaults.ThirstyThreshold.ToString("N");
                    break;
                case "RabbitThirstIncreaseBase":
                    _dropdownInputField.text = RabbitDefaults.ThirstIncrease.ToString("N");
                    break;
                case "RabbitDrinkingSpeed":
                    _dropdownInputField.text = RabbitDefaults.DrinkingSpeed.ToString("N");
                    break;
                case "RabbitMatingDuration":
                    _dropdownInputField.text = RabbitDefaults.MatingDuration.ToString("N");
                    break;
                case "RabbitMatingThreshold":
                    _dropdownInputField.text = RabbitDefaults.MatingThreshold.ToString("N");
                    break;
                case "RabbitReproductiveUrgeIncrease":
                    _dropdownInputField.text = RabbitDefaults.ReproductiveUrgeIncreaseMale.ToString("N");
                    break;
                case "RabbitPregnancyLength":
                    _dropdownInputField.text = RabbitDefaults.PregnancyLength.ToString("N");
                    break;
                case "RabbitBirthDuration":
                    _dropdownInputField.text = RabbitDefaults.BirthDuration.ToString("N");
                    break;
                case "RabbitLitterSizeMin":
                    _dropdownInputField.text = RabbitDefaults.LitterSizeMin.ToString("N");
                    break;
                case "RabbitLitterSizeMax":
                    _dropdownInputField.text = RabbitDefaults.LitterSizeMax.ToString("N");
                    break;
                case "RabbitLitterSizeAve":
                    _dropdownInputField.text = RabbitDefaults.LitterSizeAve.ToString("N");
                    break;
                case "RabbitMovementSpeed":
                    _dropdownInputField.text = RabbitDefaults.MoveSpeed.ToString("N");
                    break;
                case "RabbitMovementMultiplierBase":
                    _dropdownInputField.text = RabbitDefaults.OriginalMoveMultiplier.ToString("N");
                    break;
                case "RabbitMovementMultiplierYoung":
                    _dropdownInputField.text = RabbitDefaults.YoungMoveMultiplier.ToString("N");
                    break;
                case "RabbitMovementMultiplierAdult":
                    _dropdownInputField.text = RabbitDefaults.AdultMoveMultiplier.ToString("N");
                    break;
                case "RabbitMovementMultiplierOld":
                    _dropdownInputField.text = RabbitDefaults.OldMoveMultiplier.ToString("N");
                    break;
                case "RabbitMovementMultiplierPregnant":
                    _dropdownInputField.text = RabbitDefaults.PregnancyMoveMultiplier.ToString("N");
                    break;
                case "RabbitSightRadius":
                    _dropdownInputField.text = RabbitDefaults.SightRadius.ToString("N");
                    break;
                case "RabbitSizeMale":
                    _dropdownInputField.text = RabbitDefaults.ScaleMale.ToString("N");
                    break;
                case "RabbitSizeFemale":
                    _dropdownInputField.text = RabbitDefaults.ScaleFemale.ToString("N");
                    break;
                case "FoxAgeMax":
                    _dropdownInputField.text = FoxDefaults.AgeMax.ToString("N");
                    break;
                case "FoxNutritionalValue":
                    _dropdownInputField.text = FoxDefaults.NutritionalValue.ToString("N");
                    break;
                case "FoxCanBeEaten":
                    _dropdownInputField.text = Convert.ToInt32(FoxDefaults.CanBeEaten).ToString("N");
                    break;
                case "FoxHungerMax":
                    _dropdownInputField.text = FoxDefaults.HungerMax.ToString("N");
                    break;
                case "FoxHungerThreshold":
                    _dropdownInputField.text = FoxDefaults.HungryThreshold.ToString("N");
                    break;
                case "FoxHungerIncreaseBase":
                    _dropdownInputField.text = FoxDefaults.HungerIncrease.ToString("N");
                    break;
                case "FoxHungerIncreaseYoung":
                    _dropdownInputField.text = FoxDefaults.YoungHungerIncrease.ToString("N");
                    break;
                case "FoxHungerIncreaseAdult":
                    _dropdownInputField.text = FoxDefaults.ScaleFemale.ToString("N");
                    break;
                case "FoxHungerIncreaseOld":
                    _dropdownInputField.text = FoxDefaults.OldHungerIncrease.ToString("N");
                    break;
                case "FoxEatingSpeed":
                    _dropdownInputField.text = FoxDefaults.EatingSpeed.ToString("N");
                    break;
                case "FoxThirstMax":
                    _dropdownInputField.text = FoxDefaults.ThirstMax.ToString("N");
                    break;
                case "FoxThirstThreshold":
                    _dropdownInputField.text = FoxDefaults.ThirstyThreshold.ToString("N");
                    break;
                case "FoxThirstIncreaseBase":
                    _dropdownInputField.text = FoxDefaults.ThirstIncrease.ToString("N");
                    break;
                case "FoxDrinkingSpeed":
                    _dropdownInputField.text = FoxDefaults.DrinkingSpeed.ToString("N");
                    break;
                case "FoxMatingDuration":
                    _dropdownInputField.text = FoxDefaults.MatingDuration.ToString("N");
                    break;
                case "FoxMatingThreshold":
                    _dropdownInputField.text = FoxDefaults.MatingThreshold.ToString("N");
                    break;
                case "FoxReproductiveUrgeIncrease":
                    _dropdownInputField.text = FoxDefaults.ReproductiveUrgeIncreaseMale.ToString("N");
                    break;
                case "FoxPregnancyLength":
                    _dropdownInputField.text = FoxDefaults.PregnancyLength.ToString("N");
                    break;
                case "FoxBirthDuration":
                    _dropdownInputField.text = FoxDefaults.BirthDuration.ToString("N");
                    break;
                case "FoxLitterSizeMin":
                    _dropdownInputField.text = FoxDefaults.LitterSizeMin.ToString("N");
                    break;
                case "FoxLitterSizeMax":
                    _dropdownInputField.text = FoxDefaults.LitterSizeMax.ToString("N");
                    break;
                case "FoxLitterSizeAve":
                    _dropdownInputField.text = FoxDefaults.LitterSizeAve.ToString("N");
                    break;
                case "FoxMovementSpeed":
                    _dropdownInputField.text = FoxDefaults.MoveSpeed.ToString("N");
                    break;
                case "FoxMovementMultiplierBase":
                    _dropdownInputField.text = FoxDefaults.OriginalMoveMultiplier.ToString("N");
                    break;
                case "FoxMovementMultiplierYoung":
                    _dropdownInputField.text = FoxDefaults.YoungMoveMultiplier.ToString("N");
                    break;
                case "FoxMovementMultiplierAdult":
                    _dropdownInputField.text = FoxDefaults.AdultMoveMultiplier.ToString("N");
                    break;
                case "FoxMovementMultiplierOld":
                    _dropdownInputField.text = FoxDefaults.OldMoveMultiplier.ToString("N");
                    break;
                case "FoxMovementMultiplierPregnant":
                    _dropdownInputField.text = FoxDefaults.PregnancyMoveMultiplier.ToString("N");
                    break;
                case "FoxSightRadius":
                    _dropdownInputField.text = FoxDefaults.SightRadius.ToString("N");
                    break;
                case "FoxSizeMale":
                    _dropdownInputField.text = FoxDefaults.ScaleMale.ToString("N");
                    break;
                case "FoxSizeFemale":
                    _dropdownInputField.text = FoxDefaults.ScaleFemale.ToString("N");
                    break;
                case "GrassNutritionalValue":
                    _dropdownInputField.text = GrassDefaults.NutritionalValue.ToString("N");
                    break;
                case "GrassCanBeEaten":
                    _dropdownInputField.text = Convert.ToInt32(GrassDefaults.CanBeEaten).ToString("N");
                    break;
                case "GrassSize":
                    _dropdownInputField.text = GrassDefaults.Scale.ToString("N");
                    break;
                default:
                    Debug.LogWarning("Attempted to update unknown property in switch: " + selectedOption, this);
                    break;
            }
        }

        /*Set the respective default to the value of the input field, sets the somethingchanged flag to false so existing entities also get updated */
        public void DropdownPropertiesUpdate()
        {
            string selectedOption = _dropdownPropertyDropdown.options[_dropdownPropertyDropdown.value].text;
            switch (selectedOption)
            {
                case "None":
                    break;
                case "RabbitAgeMax":
                    RabbitDefaults.AgeMax = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitNutritionalValue":
                    RabbitDefaults.NutritionalValue = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitCanBeEaten":
                    RabbitDefaults.CanBeEaten = int.Parse(_dropdownInputField.text) != 0;
                    break;
                case "RabbitHungerMax":
                    RabbitDefaults.HungerMax = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitHungerThreshold":
                    RabbitDefaults.HungryThreshold = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitHungerIncreaseBase":
                    RabbitDefaults.HungerIncrease = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitHungerIncreaseYoung":
                    RabbitDefaults.YoungHungerIncrease = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitHungerIncreaseAdult":
                    RabbitDefaults.AdultHungerIncrease = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitHungerIncreaseOld":
                    RabbitDefaults.OldHungerIncrease = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitEatingSpeed":
                    RabbitDefaults.EatingSpeed = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitThirstMax":
                    RabbitDefaults.ThirstMax = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitThirstThreshold":
                    RabbitDefaults.ThirstyThreshold = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitThirstIncreaseBase":
                    RabbitDefaults.ThirstIncrease = float.Parse(_dropdownInputField.text);
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
                    RabbitDefaults.DrinkingSpeed = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitMatingDuration":
                    RabbitDefaults.MatingDuration = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitMatingThreshold":
                    RabbitDefaults.MatingThreshold = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitReproductiveUrgeIncrease":
                    RabbitDefaults.ReproductiveUrgeIncreaseMale = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitPregnancyLength":
                    RabbitDefaults.PregnancyLength = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitBirthDuration":
                    RabbitDefaults.BirthDuration = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitLitterSizeMin":
                    RabbitDefaults.LitterSizeMin = int.Parse(_dropdownInputField.text);
                    break;
                case "RabbitLitterSizeMax":
                    RabbitDefaults.LitterSizeMax = int.Parse(_dropdownInputField.text);
                    break;
                case "RabbitLitterSizeAve":
                    RabbitDefaults.LitterSizeAve = int.Parse(_dropdownInputField.text);
                    break;
                case "RabbitMovementSpeed":
                    RabbitDefaults.MoveSpeed = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitMovementMultiplierBase":
                    RabbitDefaults.OriginalMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitMovementMultiplierYoung":
                    RabbitDefaults.YoungMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitMovementMultiplierAdult":
                    RabbitDefaults.AdultMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitMovementMultiplierOld":
                    RabbitDefaults.OldMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitMovementMultiplierPregnant":
                    RabbitDefaults.PregnancyMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitSightRadius":
                    RabbitDefaults.SightRadius = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitSizeMale":
                    RabbitDefaults.ScaleMale = float.Parse(_dropdownInputField.text);
                    break;
                case "RabbitSizeFemale":
                    RabbitDefaults.ScaleFemale = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxAgeMax":
                    FoxDefaults.AgeMax = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxNutritionalValue":
                    FoxDefaults.NutritionalValue = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxCanBeEaten":
                    FoxDefaults.CanBeEaten = int.Parse(_dropdownInputField.text) != 0;
                    break;
                case "FoxHungerMax":
                    FoxDefaults.HungerMax = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxHungerThreshold":
                    FoxDefaults.HungryThreshold = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxHungerIncreaseBase":
                    FoxDefaults.HungerIncrease = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxHungerIncreaseYoung":
                    FoxDefaults.YoungHungerIncrease = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxHungerIncreaseAdult":
                    FoxDefaults.AdultHungerIncrease = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxHungerIncreaseOld":
                    FoxDefaults.OldHungerIncrease = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxEatingSpeed":
                    FoxDefaults.EatingSpeed = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxThirstMax":
                    FoxDefaults.ThirstMax = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxThirstThreshold":
                    FoxDefaults.ThirstyThreshold = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxThirstIncreaseBase":
                    FoxDefaults.ThirstIncrease = float.Parse(_dropdownInputField.text);
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
                    FoxDefaults.DrinkingSpeed = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxMatingDuration":
                    FoxDefaults.MatingDuration = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxMatingThreshold":
                    FoxDefaults.MatingThreshold = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxReproductiveUrgeIncrease":
                    FoxDefaults.ReproductiveUrgeIncreaseMale = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxPregnancyLength":
                    FoxDefaults.PregnancyLength = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxBirthDuration":
                    FoxDefaults.BirthDuration = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxLitterSizeMin":
                    FoxDefaults.LitterSizeMin = int.Parse(_dropdownInputField.text);
                    break;
                case "FoxLitterSizeMax":
                    FoxDefaults.LitterSizeMax = int.Parse(_dropdownInputField.text);
                    break;
                case "FoxLitterSizeAve":
                    FoxDefaults.LitterSizeAve = int.Parse(_dropdownInputField.text);
                    break;
                case "FoxMovementSpeed":
                    FoxDefaults.MoveSpeed = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxMovementMultiplierBase":
                    FoxDefaults.OriginalMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxMovementMultiplierYoung":
                    FoxDefaults.YoungMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxMovementMultiplierAdult":
                    FoxDefaults.AdultMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxMovementMultiplierOld":
                    FoxDefaults.OldMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxMovementMultiplierPregnant":
                    FoxDefaults.PregnancyMoveMultiplier = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxSightRadius":
                    FoxDefaults.SightRadius = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxSizeMale":
                    FoxDefaults.ScaleMale = float.Parse(_dropdownInputField.text);
                    break;
                case "FoxSizeFemale":
                    FoxDefaults.ScaleFemale = float.Parse(_dropdownInputField.text);
                    break;
                case "GrassNutritionalValue":
                    GrassDefaults.NutritionalValue = float.Parse(_dropdownInputField.text);
                    break;
                case "GrassCanBeEaten":
                    GrassDefaults.CanBeEaten = int.Parse(_dropdownInputField.text) != 0;
                    break;
                case "GrassSize":
                    GrassDefaults.Scale = float.Parse(_dropdownInputField.text);
                    break;
                default:
                    Debug.LogWarning("Attempted to update unknown property in switch: " + selectedOption, this);
                    break;
            }

            UIUpdateSystem.Instance.SomethingChangedFlag = true;
        }
    }
}
