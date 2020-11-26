using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    #region Slider Linking
    [Header("Properties Sliders")]
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

    #region Initialisation
    void Awake()
    {
        // Set sliders to default
        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();

        rabbitSizeFemaleSlider.value = RabbitDefaults.scaleFemale;
        rabbitSizeFemaleText.text = RabbitDefaults.scaleFemale.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        
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
}
