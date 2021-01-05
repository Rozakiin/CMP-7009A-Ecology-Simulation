using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using SFB;
using System;

public class Escape : MonoBehaviour
{
    [SerializeField] GameObject EscapeMeun;
    public TimeControlSystem timeControlSystem;
    private void Awake()
    {
        EscapeMeun.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (timeControlSystem.pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        EscapeMeun.SetActive(false);
        timeControlSystem.Play();
    }

    void PauseGame()
    {
        EscapeMeun.SetActive(true);
        timeControlSystem.Pause();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("I am Quit");
    }

    public void SaveFile()
    {
        string timeStamp = DateTime.Now.ToString();
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", timeStamp, "txt");
        SaveGame(path);
    }

    private void SaveGame(string path)
    {
        XmlDocument xmlDocument = new XmlDocument();

        //#region Create XMLDocument Elements
        XmlElement root = xmlDocument.CreateElement("XML");

        #region Rabbit default
        XmlElement Rabbit = xmlDocument.CreateElement("Rabbit");

        #region AgeData
        XmlElement rAgeData = xmlDocument.CreateElement("AgeData");

        XmlElement rAge = xmlDocument.CreateElement("age");
        XmlElement rAgeIncrease = xmlDocument.CreateElement("ageIncrease");
        XmlElement rAgeMax = xmlDocument.CreateElement("ageMax");
        XmlElement rAdultEntryTimer = xmlDocument.CreateElement("adultEntryTimer");
        XmlElement rOldEntryTimer = xmlDocument.CreateElement("oldEntryTimer");

        rAge.InnerText = RabbitDefaults.age.ToString();
        rAgeIncrease.InnerText = RabbitDefaults.ageIncrease.ToString();
        rAgeMax.InnerText = RabbitDefaults.ageMax.ToString();
        rAdultEntryTimer.InnerText = RabbitDefaults.adultEntryTimer.ToString();
        rOldEntryTimer.InnerText = RabbitDefaults.oldEntryTimer.ToString();

        rAgeData.AppendChild(rAge);
        rAgeData.AppendChild(rAgeIncrease);
        rAgeData.AppendChild(rAgeMax);
        rAgeData.AppendChild(rAdultEntryTimer);
        rAgeData.AppendChild(rOldEntryTimer);

        Rabbit.AppendChild(rAgeData);
        #endregion age data

        #region Edible data
        XmlElement rEdibleData = xmlDocument.CreateElement("EdibleData");

        XmlElement rNutritionalValue = xmlDocument.CreateElement("nutritionalValue");
        XmlElement rNutritionalValueMultiplier = xmlDocument.CreateElement("nutritionalValueMultiplier");

        rNutritionalValue.InnerText = RabbitDefaults.nutritionalValue.ToString();
        rNutritionalValueMultiplier.InnerText = RabbitDefaults.nutritionalValueMultiplier.ToString();

        rEdibleData.AppendChild(rNutritionalValue);
        rEdibleData.AppendChild(rNutritionalValueMultiplier);

        Rabbit.AppendChild(rEdibleData);
        #endregion Edible data

        #region HungerData
        XmlElement rHungerData = xmlDocument.CreateElement("HungerData");

        XmlElement rHunger = xmlDocument.CreateElement("hunger");
        XmlElement rHungerMax = xmlDocument.CreateElement("hungerMax");
        XmlElement rHungryThreshold = xmlDocument.CreateElement("hungryThreshold");
        XmlElement rHungerIncrease = xmlDocument.CreateElement("hungerIncrease");
        XmlElement rPregnancyHungerIncrease = xmlDocument.CreateElement("pregnancyHungerIncrease");
        XmlElement rYoungHungerIncrease = xmlDocument.CreateElement("youngHungerIncrease");
        XmlElement rAdultHungerIncrease = xmlDocument.CreateElement("adultHungerIncrease");
        XmlElement rOldHungerIncrease = xmlDocument.CreateElement("oldHungerIncrease");
        XmlElement rEatingSpeed = xmlDocument.CreateElement("eatingSpeed");

        rHunger.InnerText = RabbitDefaults.hunger.ToString();
        rHungerMax.InnerText = RabbitDefaults.hungerMax.ToString();
        rHungryThreshold.InnerText = RabbitDefaults.hungryThreshold.ToString();
        rHungerIncrease.InnerText = RabbitDefaults.hungerIncrease.ToString();
        rPregnancyHungerIncrease.InnerText = RabbitDefaults.pregnancyHungerIncrease.ToString();
        rYoungHungerIncrease.InnerText = RabbitDefaults.youngHungerIncrease.ToString();
        rAdultHungerIncrease.InnerText = RabbitDefaults.adultHungerIncrease.ToString();
        rOldHungerIncrease.InnerText = RabbitDefaults.oldHungerIncrease.ToString();
        rEatingSpeed.InnerText = RabbitDefaults.eatingSpeed.ToString();

        rHungerData.AppendChild(rHunger);
        rHungerData.AppendChild(rHungerMax);
        rHungerData.AppendChild(rHungryThreshold);
        rHungerData.AppendChild(rHungerIncrease);
        rHungerData.AppendChild(rPregnancyHungerIncrease);
        rHungerData.AppendChild(rYoungHungerIncrease);
        rHungerData.AppendChild(rAdultHungerIncrease);
        rHungerData.AppendChild(rOldHungerIncrease);
        rHungerData.AppendChild(rEatingSpeed);

        Rabbit.AppendChild(rHungerData);
        #endregion hunger data

        #region Thirst data
        XmlElement rThirstData = xmlDocument.CreateElement("ThirstData");

        XmlElement rThirst = xmlDocument.CreateElement("thirst");
        XmlElement rThirstMax = xmlDocument.CreateElement("thirstMax");
        XmlElement rThirstyThreshold = xmlDocument.CreateElement("thirstyThreshold");
        XmlElement rThirstIncrease = xmlDocument.CreateElement("thirstIncrease");
        XmlElement rDrinkingSpeed = xmlDocument.CreateElement("drinkingSpeed");

        rThirst.InnerText = RabbitDefaults.thirst.ToString();
        rThirstMax.InnerText = RabbitDefaults.thirstMax.ToString();
        rThirstyThreshold.InnerText = RabbitDefaults.thirstyThreshold.ToString();
        rThirstIncrease.InnerText = RabbitDefaults.thirstIncrease.ToString();
        rDrinkingSpeed.InnerText = RabbitDefaults.drinkingSpeed.ToString();

        rThirstData.AppendChild(rThirst);
        rThirstData.AppendChild(rThirstMax);
        rThirstData.AppendChild(rThirstyThreshold);
        rThirstData.AppendChild(rThirstIncrease);
        rThirstData.AppendChild(rDrinkingSpeed);

        Rabbit.AppendChild(rThirstData);
        #endregion Thirst data

        #region mateData
        XmlElement rMatedata = xmlDocument.CreateElement("mateData");


        XmlElement rMatestarttime = xmlDocument.CreateElement("mateStartTime");
        XmlElement rMatingduration = xmlDocument.CreateElement("matingDuration");
        XmlElement rReproductiveurge = xmlDocument.CreateElement("reproductiveUrge");
        XmlElement rReproductiveurgeincreasemale = xmlDocument.CreateElement("reproductiveUrgeIncreaseMale");
        XmlElement rReproductiveurgeincreasefemale = xmlDocument.CreateElement("reproductiveUrgeIncreaseFemale");
        XmlElement rMatingthreshold = xmlDocument.CreateElement("matingThreshold");


        rMatestarttime.InnerText = RabbitDefaults.mateStartTime.ToString();
        rMatingduration.InnerText = RabbitDefaults.matingDuration.ToString();
        rReproductiveurge.InnerText = RabbitDefaults.reproductiveUrge.ToString();
        rReproductiveurgeincreasemale.InnerText = RabbitDefaults.reproductiveUrgeIncreaseMale.ToString();
        rReproductiveurgeincreasefemale.InnerText = RabbitDefaults.reproductiveUrgeIncreaseFemale.ToString();
        rMatingthreshold.InnerText = RabbitDefaults.matingThreshold.ToString();


        rMatedata.AppendChild(rMatestarttime);
        rMatedata.AppendChild(rMatingduration);
        rMatedata.AppendChild(rReproductiveurge);
        rMatedata.AppendChild(rReproductiveurgeincreasemale);
        rMatedata.AppendChild(rReproductiveurgeincreasefemale);
        rMatedata.AppendChild(rMatingthreshold);


        Rabbit.AppendChild(rMatedata);
        #endregion mateData

        #region pregnancyData
        XmlElement rPregnancyData = xmlDocument.CreateElement("pregnancyData");


        XmlElement rPregnancystarttime = xmlDocument.CreateElement("pregnancyStartTime");
        XmlElement rBabiesborn = xmlDocument.CreateElement("babiesBorn");
        XmlElement rBirthstarttime = xmlDocument.CreateElement("birthStartTime");
        XmlElement rCurrentlittersize = xmlDocument.CreateElement("currentLitterSize");
        XmlElement rPregnancylengthmodifier = xmlDocument.CreateElement("pregnancyLengthModifier");
        XmlElement rPregnancylength = xmlDocument.CreateElement("pregnancyLength");
        XmlElement rBirthduration = xmlDocument.CreateElement("birthDuration");
        XmlElement rLittersizemin = xmlDocument.CreateElement("litterSizeMin");
        XmlElement rLittersizemax = xmlDocument.CreateElement("litterSizeMax");
        XmlElement rLittersizeave = xmlDocument.CreateElement("litterSizeAve");


        rPregnancystarttime.InnerText = RabbitDefaults.pregnancyStartTime.ToString();
        rBabiesborn.InnerText = RabbitDefaults.babiesBorn.ToString();
        rBirthstarttime.InnerText = RabbitDefaults.birthStartTime.ToString();
        rCurrentlittersize.InnerText = RabbitDefaults.currentLitterSize.ToString();
        rPregnancylengthmodifier.InnerText = RabbitDefaults.pregnancyLengthModifier.ToString();
        rPregnancylength.InnerText = RabbitDefaults.pregnancyLength.ToString();
        rBirthduration.InnerText = RabbitDefaults.birthDuration.ToString();
        rLittersizemin.InnerText = RabbitDefaults.litterSizeMin.ToString();
        rLittersizemax.InnerText = RabbitDefaults.litterSizeMax.ToString();
        rLittersizeave.InnerText = RabbitDefaults.litterSizeAve.ToString();


        rPregnancyData.AppendChild(rPregnancystarttime);
        rPregnancyData.AppendChild(rBabiesborn);
        rPregnancyData.AppendChild(rBirthstarttime);
        rPregnancyData.AppendChild(rCurrentlittersize);
        rPregnancyData.AppendChild(rPregnancylengthmodifier);
        rPregnancyData.AppendChild(rPregnancylength);
        rPregnancyData.AppendChild(rBirthduration);
        rPregnancyData.AppendChild(rLittersizemin);
        rPregnancyData.AppendChild(rLittersizemax);
        rPregnancyData.AppendChild(rLittersizeave);


        Rabbit.AppendChild(rPregnancyData);
        #endregion pregnancyData

        #region movementData
        XmlElement rMovementData = xmlDocument.CreateElement("movementData");


        XmlElement rMovespeed = xmlDocument.CreateElement("moveSpeed");
        XmlElement rRotationspeed = xmlDocument.CreateElement("rotationSpeed");
        XmlElement rMovemultiplier = xmlDocument.CreateElement("moveMultiplier");
        XmlElement rPregnancymovemultiplier = xmlDocument.CreateElement("pregnancyMoveMultiplier");
        XmlElement rOriginalmovemultiplier = xmlDocument.CreateElement("originalMoveMultiplier");
        XmlElement rYoungmovemultiplier = xmlDocument.CreateElement("youngMoveMultiplier");
        XmlElement rAdultmovemultiplier = xmlDocument.CreateElement("adultMoveMultiplier");
        XmlElement rOldmovemultiplier = xmlDocument.CreateElement("oldMoveMultiplier");


        rMovespeed.InnerText = RabbitDefaults.moveSpeed.ToString();
        rRotationspeed.InnerText = RabbitDefaults.rotationSpeed.ToString();
        rMovemultiplier.InnerText = RabbitDefaults.moveMultiplier.ToString();
        rPregnancymovemultiplier.InnerText = RabbitDefaults.pregnancyMoveMultiplier.ToString();
        rOriginalmovemultiplier.InnerText = RabbitDefaults.originalMoveMultiplier.ToString();
        rYoungmovemultiplier.InnerText = RabbitDefaults.youngMoveMultiplier.ToString();
        rAdultmovemultiplier.InnerText = RabbitDefaults.adultMoveMultiplier.ToString();
        rOldmovemultiplier.InnerText = RabbitDefaults.oldMoveMultiplier.ToString();


        rMovementData.AppendChild(rMovespeed);
        rMovementData.AppendChild(rRotationspeed);
        rMovementData.AppendChild(rMovemultiplier);
        rMovementData.AppendChild(rPregnancymovemultiplier);
        rMovementData.AppendChild(rOriginalmovemultiplier);
        rMovementData.AppendChild(rYoungmovemultiplier);
        rMovementData.AppendChild(rAdultmovemultiplier);
        rMovementData.AppendChild(rOldmovemultiplier);


        Rabbit.AppendChild(rMovementData);
        #endregion movementData

        #region sizeData
        XmlElement rSizeData = xmlDocument.CreateElement("sizeData");


        XmlElement rSizemultiplier = xmlDocument.CreateElement("sizeMultiplier");
        XmlElement rScalemale = xmlDocument.CreateElement("scaleMale");
        XmlElement rScalefemale = xmlDocument.CreateElement("scaleFemale");
        XmlElement rYoungsizemultiplier = xmlDocument.CreateElement("youngSizeMultiplier");
        XmlElement rAdultsizemultiplier = xmlDocument.CreateElement("adultSizeMultiplier");
        XmlElement rOldsizemultiplier = xmlDocument.CreateElement("oldSizeMultiplier");


        rSizemultiplier.InnerText = RabbitDefaults.sizeMultiplier.ToString();
        rScalemale.InnerText = RabbitDefaults.scaleMale.ToString();
        rScalefemale.InnerText = RabbitDefaults.scaleFemale.ToString();
        rYoungsizemultiplier.InnerText = RabbitDefaults.youngSizeMultiplier.ToString();
        rAdultsizemultiplier.InnerText = RabbitDefaults.adultSizeMultiplier.ToString();
        rOldsizemultiplier.InnerText = RabbitDefaults.oldSizeMultiplier.ToString();


        rSizeData.AppendChild(rSizemultiplier);
        rSizeData.AppendChild(rScalemale);
        rSizeData.AppendChild(rScalefemale);
        rSizeData.AppendChild(rYoungsizemultiplier);
        rSizeData.AppendChild(rAdultsizemultiplier);
        rSizeData.AppendChild(rOldsizemultiplier);


        Rabbit.AppendChild(rSizeData);
        #endregion sizeData

        #region targetData
        XmlElement rTargetData = xmlDocument.CreateElement("targetData");


        XmlElement rTouchradius = xmlDocument.CreateElement("touchRadius");
        XmlElement rSightradius = xmlDocument.CreateElement("sightRadius");


        rTouchradius.InnerText = RabbitDefaults.touchRadius.ToString();
        rSightradius.InnerText = RabbitDefaults.sightRadius.ToString();


        rTargetData.AppendChild(rTouchradius);
        rTargetData.AppendChild(rSightradius);


        Rabbit.AppendChild(rTargetData);
        #endregion targetData

        root.AppendChild(Rabbit);
        #endregion Rabbit default


        #region Fox default
        XmlElement Fox = xmlDocument.CreateElement("Fox");

        #region ageData
        XmlElement fAgedata = xmlDocument.CreateElement("ageData");

        XmlElement fAge = xmlDocument.CreateElement("age");
        XmlElement fAgeincrease = xmlDocument.CreateElement("ageIncrease");
        XmlElement fAgemax = xmlDocument.CreateElement("ageMax");
        XmlElement fAdultentrytimer = xmlDocument.CreateElement("adultEntryTimer");
        XmlElement fOldentrytimer = xmlDocument.CreateElement("oldEntryTimer");


        fAge.InnerText = FoxDefaults.age.ToString();
        fAgeincrease.InnerText = FoxDefaults.ageIncrease.ToString();
        fAgemax.InnerText = FoxDefaults.ageMax.ToString();
        fAdultentrytimer.InnerText = FoxDefaults.adultEntryTimer.ToString();
        fOldentrytimer.InnerText = FoxDefaults.oldEntryTimer.ToString();


        fAgedata.AppendChild(fAge);
        fAgedata.AppendChild(fAgeincrease);
        fAgedata.AppendChild(fAgemax);
        fAgedata.AppendChild(fAdultentrytimer);
        fAgedata.AppendChild(fOldentrytimer);

        Fox.AppendChild(fAgedata);
        #endregion ageData

        #region edibleData
        XmlElement fEdibledata = xmlDocument.CreateElement("edibleData");

        XmlElement fNutritionalvalue = xmlDocument.CreateElement("nutritionalValue");
        XmlElement fNutritionalvaluemultiplier = xmlDocument.CreateElement("nutritionalValueMultiplier");


        fNutritionalvalue.InnerText = FoxDefaults.nutritionalValue.ToString();
        fNutritionalvaluemultiplier.InnerText = FoxDefaults.nutritionalValueMultiplier.ToString();


        fEdibledata.AppendChild(fNutritionalvalue);
        fEdibledata.AppendChild(fNutritionalvaluemultiplier);

        Fox.AppendChild(fEdibledata);
        #endregion edibleData

        #region hungerData
        XmlElement fHungerdata = xmlDocument.CreateElement("hungerData");

        XmlElement fHunger = xmlDocument.CreateElement("hunger");
        XmlElement fHungermax = xmlDocument.CreateElement("hungerMax");
        XmlElement fHungrythreshold = xmlDocument.CreateElement("hungryThreshold");
        XmlElement fHungerincrease = xmlDocument.CreateElement("hungerIncrease");
        XmlElement fPregnancyhungerincrease = xmlDocument.CreateElement("pregnancyHungerIncrease");
        XmlElement fYounghungerincrease = xmlDocument.CreateElement("youngHungerIncrease");
        XmlElement fAdulthungerincrease = xmlDocument.CreateElement("adultHungerIncrease");
        XmlElement fOldhungerincrease = xmlDocument.CreateElement("oldHungerIncrease");
        XmlElement fEatingspeed = xmlDocument.CreateElement("eatingSpeed");


        fHunger.InnerText = FoxDefaults.hunger.ToString();
        fHungermax.InnerText = FoxDefaults.hungerMax.ToString();
        fHungrythreshold.InnerText = FoxDefaults.hungryThreshold.ToString();
        fHungerincrease.InnerText = FoxDefaults.hungerIncrease.ToString();
        fPregnancyhungerincrease.InnerText = FoxDefaults.pregnancyHungerIncrease.ToString();
        fYounghungerincrease.InnerText = FoxDefaults.youngHungerIncrease.ToString();
        fAdulthungerincrease.InnerText = FoxDefaults.adultHungerIncrease.ToString();
        fOldhungerincrease.InnerText = FoxDefaults.oldHungerIncrease.ToString();
        fEatingspeed.InnerText = FoxDefaults.eatingSpeed.ToString();


        fHungerdata.AppendChild(fHunger);
        fHungerdata.AppendChild(fHungermax);
        fHungerdata.AppendChild(fHungrythreshold);
        fHungerdata.AppendChild(fHungerincrease);
        fHungerdata.AppendChild(fPregnancyhungerincrease);
        fHungerdata.AppendChild(fYounghungerincrease);
        fHungerdata.AppendChild(fAdulthungerincrease);
        fHungerdata.AppendChild(fOldhungerincrease);
        fHungerdata.AppendChild(fEatingspeed);

        Fox.AppendChild(fHungerdata);
        #endregion hungerData

        #region thirstData
        XmlElement fThirstdata = xmlDocument.CreateElement("thirstData");

        XmlElement fThirst = xmlDocument.CreateElement("thirst");
        XmlElement fThirstmax = xmlDocument.CreateElement("thirstMax");
        XmlElement fThirstythreshold = xmlDocument.CreateElement("thirstyThreshold");
        XmlElement fThirstincrease = xmlDocument.CreateElement("thirstIncrease");
        XmlElement fDrinkingspeed = xmlDocument.CreateElement("drinkingSpeed");


        fThirst.InnerText = FoxDefaults.thirst.ToString();
        fThirstmax.InnerText = FoxDefaults.thirstMax.ToString();
        fThirstythreshold.InnerText = FoxDefaults.thirstyThreshold.ToString();
        fThirstincrease.InnerText = FoxDefaults.thirstIncrease.ToString();
        fDrinkingspeed.InnerText = FoxDefaults.drinkingSpeed.ToString();


        fThirstdata.AppendChild(fThirst);
        fThirstdata.AppendChild(fThirstmax);
        fThirstdata.AppendChild(fThirstythreshold);
        fThirstdata.AppendChild(fThirstincrease);
        fThirstdata.AppendChild(fDrinkingspeed);

        Fox.AppendChild(fThirstdata);
        #endregion thirstData

        #region mateData
        XmlElement fMatedata = xmlDocument.CreateElement("mateData");

        XmlElement fMatestarttime = xmlDocument.CreateElement("mateStartTime");
        XmlElement fMatingduration = xmlDocument.CreateElement("matingDuration");
        XmlElement fReproductiveurge = xmlDocument.CreateElement("reproductiveUrge");
        XmlElement fReproductiveurgeincreasemale = xmlDocument.CreateElement("reproductiveUrgeIncreaseMale");
        XmlElement fReproductiveurgeincreasefemale = xmlDocument.CreateElement("reproductiveUrgeIncreaseFemale");
        XmlElement fMatingthreshold = xmlDocument.CreateElement("matingThreshold");


        fMatestarttime.InnerText = FoxDefaults.mateStartTime.ToString();
        fMatingduration.InnerText = FoxDefaults.matingDuration.ToString();
        fReproductiveurge.InnerText = FoxDefaults.reproductiveUrge.ToString();
        fReproductiveurgeincreasemale.InnerText = FoxDefaults.reproductiveUrgeIncreaseMale.ToString();
        fReproductiveurgeincreasefemale.InnerText = FoxDefaults.reproductiveUrgeIncreaseFemale.ToString();
        fMatingthreshold.InnerText = FoxDefaults.matingThreshold.ToString();


        fMatedata.AppendChild(fMatestarttime);
        fMatedata.AppendChild(fMatingduration);
        fMatedata.AppendChild(fReproductiveurge);
        fMatedata.AppendChild(fReproductiveurgeincreasemale);
        fMatedata.AppendChild(fReproductiveurgeincreasefemale);
        fMatedata.AppendChild(fMatingthreshold);

        Fox.AppendChild(fMatedata);
        #endregion mateData

        #region pregnancyData
        XmlElement fPregnancydata = xmlDocument.CreateElement("pregnancyData");

        XmlElement fPregnancystarttime = xmlDocument.CreateElement("pregnancyStartTime");
        XmlElement fBabiesborn = xmlDocument.CreateElement("babiesBorn");
        XmlElement fBirthstarttime = xmlDocument.CreateElement("birthStartTime");
        XmlElement fCurrentlittersize = xmlDocument.CreateElement("currentLitterSize");
        XmlElement fPregnancylengthmodifier = xmlDocument.CreateElement("pregnancyLengthModifier");
        XmlElement fPregnancylength = xmlDocument.CreateElement("pregnancyLength");
        XmlElement fBirthduration = xmlDocument.CreateElement("birthDuration");
        XmlElement fLittersizemin = xmlDocument.CreateElement("litterSizeMin");
        XmlElement fLittersizemax = xmlDocument.CreateElement("litterSizeMax");
        XmlElement fLittersizeave = xmlDocument.CreateElement("litterSizeAve");


        fPregnancystarttime.InnerText = FoxDefaults.pregnancyStartTime.ToString();
        fBabiesborn.InnerText = FoxDefaults.babiesBorn.ToString();
        fBirthstarttime.InnerText = FoxDefaults.birthStartTime.ToString();
        fCurrentlittersize.InnerText = FoxDefaults.currentLitterSize.ToString();
        fPregnancylengthmodifier.InnerText = FoxDefaults.pregnancyLengthModifier.ToString();
        fPregnancylength.InnerText = FoxDefaults.pregnancyLength.ToString();
        fBirthduration.InnerText = FoxDefaults.birthDuration.ToString();
        fLittersizemin.InnerText = FoxDefaults.litterSizeMin.ToString();
        fLittersizemax.InnerText = FoxDefaults.litterSizeMax.ToString();
        fLittersizeave.InnerText = FoxDefaults.litterSizeAve.ToString();


        fPregnancydata.AppendChild(fPregnancystarttime);
        fPregnancydata.AppendChild(fBabiesborn);
        fPregnancydata.AppendChild(fBirthstarttime);
        fPregnancydata.AppendChild(fCurrentlittersize);
        fPregnancydata.AppendChild(fPregnancylengthmodifier);
        fPregnancydata.AppendChild(fPregnancylength);
        fPregnancydata.AppendChild(fBirthduration);
        fPregnancydata.AppendChild(fLittersizemin);
        fPregnancydata.AppendChild(fLittersizemax);
        fPregnancydata.AppendChild(fLittersizeave);

        Fox.AppendChild(fPregnancydata);
        #endregion pregnancyData

        #region movementData
        XmlElement fMovementdata = xmlDocument.CreateElement("movementData");

        XmlElement fMovespeed = xmlDocument.CreateElement("moveSpeed");
        XmlElement fRotationspeed = xmlDocument.CreateElement("rotationSpeed");
        XmlElement fMovemultiplier = xmlDocument.CreateElement("moveMultiplier");
        XmlElement fPregnancymovemultiplier = xmlDocument.CreateElement("pregnancyMoveMultiplier");
        XmlElement fOriginalmovemultiplier = xmlDocument.CreateElement("originalMoveMultiplier");
        XmlElement fYoungmovemultiplier = xmlDocument.CreateElement("youngMoveMultiplier");
        XmlElement fAdultmovemultiplier = xmlDocument.CreateElement("adultMoveMultiplier");
        XmlElement fOldmovemultiplier = xmlDocument.CreateElement("oldMoveMultiplier");


        fMovespeed.InnerText = FoxDefaults.moveSpeed.ToString();
        fRotationspeed.InnerText = FoxDefaults.rotationSpeed.ToString();
        fMovemultiplier.InnerText = FoxDefaults.moveMultiplier.ToString();
        fPregnancymovemultiplier.InnerText = FoxDefaults.pregnancyMoveMultiplier.ToString();
        fOriginalmovemultiplier.InnerText = FoxDefaults.originalMoveMultiplier.ToString();
        fYoungmovemultiplier.InnerText = FoxDefaults.youngMoveMultiplier.ToString();
        fAdultmovemultiplier.InnerText = FoxDefaults.adultMoveMultiplier.ToString();
        fOldmovemultiplier.InnerText = FoxDefaults.oldMoveMultiplier.ToString();


        fMovementdata.AppendChild(fMovespeed);
        fMovementdata.AppendChild(fRotationspeed);
        fMovementdata.AppendChild(fMovemultiplier);
        fMovementdata.AppendChild(fPregnancymovemultiplier);
        fMovementdata.AppendChild(fOriginalmovemultiplier);
        fMovementdata.AppendChild(fYoungmovemultiplier);
        fMovementdata.AppendChild(fAdultmovemultiplier);
        fMovementdata.AppendChild(fOldmovemultiplier);

        Fox.AppendChild(fMovementdata);
        #endregion movementData

        #region sizeData
        XmlElement fSizedata = xmlDocument.CreateElement("sizeData");

        XmlElement fSizemultiplier = xmlDocument.CreateElement("sizeMultiplier");
        XmlElement fScalemale = xmlDocument.CreateElement("scaleMale");
        XmlElement fScalefemale = xmlDocument.CreateElement("scaleFemale");
        XmlElement fYoungsizemultiplier = xmlDocument.CreateElement("youngSizeMultiplier");
        XmlElement fAdultsizemultiplier = xmlDocument.CreateElement("adultSizeMultiplier");
        XmlElement fOldsizemultiplier = xmlDocument.CreateElement("oldSizeMultiplier");


        fSizemultiplier.InnerText = FoxDefaults.sizeMultiplier.ToString();
        fScalemale.InnerText = FoxDefaults.scaleMale.ToString();
        fScalefemale.InnerText = FoxDefaults.scaleFemale.ToString();
        fYoungsizemultiplier.InnerText = FoxDefaults.youngSizeMultiplier.ToString();
        fAdultsizemultiplier.InnerText = FoxDefaults.adultSizeMultiplier.ToString();
        fOldsizemultiplier.InnerText = FoxDefaults.oldSizeMultiplier.ToString();


        fSizedata.AppendChild(fSizemultiplier);
        fSizedata.AppendChild(fScalemale);
        fSizedata.AppendChild(fScalefemale);
        fSizedata.AppendChild(fYoungsizemultiplier);
        fSizedata.AppendChild(fAdultsizemultiplier);
        fSizedata.AppendChild(fOldsizemultiplier);

        Fox.AppendChild(fSizedata);
        #endregion sizeData

        #region targetData
        XmlElement fTargetdata = xmlDocument.CreateElement("targetData");

        XmlElement fTouchradius = xmlDocument.CreateElement("touchRadius");
        XmlElement fSightradius = xmlDocument.CreateElement("sightRadius");


        fTouchradius.InnerText = FoxDefaults.touchRadius.ToString();
        fSightradius.InnerText = FoxDefaults.sightRadius.ToString();


        fTargetdata.AppendChild(fTouchradius);
        fTargetdata.AppendChild(fSightradius);

        Fox.AppendChild(fTargetdata);
        #endregion targetData

        root.AppendChild(Fox);
        #endregion Fox default


        #region Grass default
        XmlElement gGrassdata = xmlDocument.CreateElement("GrassData");

        XmlElement gNutritionalvalue = xmlDocument.CreateElement("nutritionalValue");
        XmlElement gNutritionalvaluemultiplier = xmlDocument.CreateElement("nutritionalValueMultiplier");
        XmlElement gSizemultiplier = xmlDocument.CreateElement("sizeMultiplier");
        XmlElement gScale = xmlDocument.CreateElement("scale");


        gNutritionalvalue.InnerText = GrassDefaults.nutritionalValue.ToString();
        gNutritionalvaluemultiplier.InnerText = GrassDefaults.nutritionalValueMultiplier.ToString();
        gSizemultiplier.InnerText = GrassDefaults.sizeMultiplier.ToString();
        gScale.InnerText = GrassDefaults.scale.ToString();


        gGrassdata.AppendChild(gNutritionalvalue);
        gGrassdata.AppendChild(gNutritionalvaluemultiplier);
        gGrassdata.AppendChild(gSizemultiplier);
        gGrassdata.AppendChild(gScale);

        root.AppendChild(gGrassdata);

        #endregion Grass default

        xmlDocument.AppendChild(root);


        xmlDocument.Save(path);
    }
}
