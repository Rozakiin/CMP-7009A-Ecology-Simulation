using System.Xml;
using EntityDefaults;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.UI
{
    public class UIEscape : MonoBehaviour
    {
        [SerializeField] private GameObject escapeMenu;
        [SerializeField] private UITimeControl uITimeControl;
        [SerializeField] private Button resume;
        [SerializeField] private Button save;
        [SerializeField] private Button quit;
        private void Awake()
        {
            escapeMenu.SetActive(false);
            resume.onClick.AddListener(ResumeGame);
            save.onClick.AddListener(SaveFile);
            quit.onClick.AddListener(QuitGame);
        }
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (uITimeControl.GetPause())
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        private void ResumeGame()
        {
            escapeMenu.SetActive(false);
            uITimeControl.Play();
        }

        private void PauseGame()
        {
            escapeMenu.SetActive(true);
            uITimeControl.Pause();
        }

        private void QuitGame()
        {
            Application.Quit();
            Debug.Log("I am Quit");
        }

        private void SaveFile()
        {
            string path = StandaloneFileBrowser.StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "xml");
            if (path != string.Empty)
            {
                SaveGame(path);
            }
            else
            {
                Debug.Log("No Path Insert");
            }
        
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
            XmlElement rAgeGroup = xmlDocument.CreateElement("ageGroup");
            XmlElement rAdultEntryTimer = xmlDocument.CreateElement("adultEntryTimer");
            XmlElement rOldEntryTimer = xmlDocument.CreateElement("oldEntryTimer");

            //public static BioStatsData.AgeGroup ageGroup = BioStatsData.AgeGroup.Young;
            rAge.InnerText = RabbitDefaults.age.ToString();
            rAgeIncrease.InnerText = RabbitDefaults.ageIncrease.ToString();
            rAgeMax.InnerText = RabbitDefaults.ageMax.ToString();
            rAgeGroup.InnerText = RabbitDefaults.ageGroup.ToString();
            rAdultEntryTimer.InnerText = RabbitDefaults.adultEntryTimer.ToString();
            rOldEntryTimer.InnerText = RabbitDefaults.oldEntryTimer.ToString();

            rAgeData.AppendChild(rAge);
            rAgeData.AppendChild(rAgeIncrease);
            rAgeData.AppendChild(rAgeMax);
            rAgeData.AppendChild(rAgeGroup);
            rAgeData.AppendChild(rAdultEntryTimer);
            rAgeData.AppendChild(rOldEntryTimer);

            Rabbit.AppendChild(rAgeData);
            #endregion age data

            #region edibleData
            XmlElement rEdibledata = xmlDocument.CreateElement("edibleData");

            XmlElement rNutritionalvalue = xmlDocument.CreateElement("nutritionalValue");
            XmlElement rCanbeeaten = xmlDocument.CreateElement("canBeEaten");
            XmlElement rNutritionalvaluemultiplier = xmlDocument.CreateElement("nutritionalValueMultiplier");
            XmlElement rFoodtype = xmlDocument.CreateElement("foodType");


            rNutritionalvalue.InnerText = RabbitDefaults.nutritionalValue.ToString();
            rCanbeeaten.InnerText = RabbitDefaults.canBeEaten.ToString();
            rNutritionalvaluemultiplier.InnerText = RabbitDefaults.nutritionalValueMultiplier.ToString();
            rFoodtype.InnerText = RabbitDefaults.foodType.ToString();


            rEdibledata.AppendChild(rNutritionalvalue);
            rEdibledata.AppendChild(rCanbeeaten);
            rEdibledata.AppendChild(rNutritionalvaluemultiplier);
            rEdibledata.AppendChild(rFoodtype);

            Rabbit.AppendChild(rEdibledata);
            #endregion edibleData

            #region hungerData
            XmlElement rHungerdata = xmlDocument.CreateElement("hungerData");

            XmlElement rHunger = xmlDocument.CreateElement("hunger");
            XmlElement rHungermax = xmlDocument.CreateElement("hungerMax");
            XmlElement rHungrythreshold = xmlDocument.CreateElement("hungryThreshold");
            XmlElement rHungerincrease = xmlDocument.CreateElement("hungerIncrease");
            XmlElement rPregnancyhungerincrease = xmlDocument.CreateElement("pregnancyHungerIncrease");
            XmlElement rYounghungerincrease = xmlDocument.CreateElement("youngHungerIncrease");
            XmlElement rAdulthungerincrease = xmlDocument.CreateElement("adultHungerIncrease");
            XmlElement rOldhungerincrease = xmlDocument.CreateElement("oldHungerIncrease");
            XmlElement rEatingspeed = xmlDocument.CreateElement("eatingSpeed");
            XmlElement rDiet = xmlDocument.CreateElement("diet");


            rHunger.InnerText = RabbitDefaults.hunger.ToString();
            rHungermax.InnerText = RabbitDefaults.hungerMax.ToString();
            rHungrythreshold.InnerText = RabbitDefaults.hungryThreshold.ToString();
            rHungerincrease.InnerText = RabbitDefaults.hungerIncrease.ToString();
            rPregnancyhungerincrease.InnerText = RabbitDefaults.pregnancyHungerIncrease.ToString();
            rYounghungerincrease.InnerText = RabbitDefaults.youngHungerIncrease.ToString();
            rAdulthungerincrease.InnerText = RabbitDefaults.adultHungerIncrease.ToString();
            rOldhungerincrease.InnerText = RabbitDefaults.oldHungerIncrease.ToString();
            rEatingspeed.InnerText = RabbitDefaults.eatingSpeed.ToString();
            rDiet.InnerText = RabbitDefaults.diet.ToString();


            rHungerdata.AppendChild(rHunger);
            rHungerdata.AppendChild(rHungermax);
            rHungerdata.AppendChild(rHungrythreshold);
            rHungerdata.AppendChild(rHungerincrease);
            rHungerdata.AppendChild(rPregnancyhungerincrease);
            rHungerdata.AppendChild(rYounghungerincrease);
            rHungerdata.AppendChild(rAdulthungerincrease);
            rHungerdata.AppendChild(rOldhungerincrease);
            rHungerdata.AppendChild(rEatingspeed);
            rHungerdata.AppendChild(rDiet);

            Rabbit.AppendChild(rHungerdata);
            #endregion hungerData

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
            XmlElement rPregnancydata = xmlDocument.CreateElement("pregnancyData");

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


            rPregnancydata.AppendChild(rPregnancystarttime);
            rPregnancydata.AppendChild(rBabiesborn);
            rPregnancydata.AppendChild(rBirthstarttime);
            rPregnancydata.AppendChild(rCurrentlittersize);
            rPregnancydata.AppendChild(rPregnancylengthmodifier);
            rPregnancydata.AppendChild(rPregnancylength);
            rPregnancydata.AppendChild(rBirthduration);
            rPregnancydata.AppendChild(rLittersizemin);
            rPregnancydata.AppendChild(rLittersizemax);
            rPregnancydata.AppendChild(rLittersizeave);

            Rabbit.AppendChild(rPregnancydata);
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


            #region stateData
            XmlElement rStatedata = xmlDocument.CreateElement("stateData");

            XmlElement rState = xmlDocument.CreateElement("flagState");
            XmlElement rPreviousstate = xmlDocument.CreateElement("previousFlagState");
            XmlElement rDeathreason = xmlDocument.CreateElement("deathReason");
            XmlElement rBeeneaten = xmlDocument.CreateElement("beenEaten");


            rState.InnerText = RabbitDefaults.flagState.ToString();
            rPreviousstate.InnerText = RabbitDefaults.previousFlagState.ToString();
            rDeathreason.InnerText = RabbitDefaults.deathReason.ToString();
            rBeeneaten.InnerText = RabbitDefaults.beenEaten.ToString();


            rStatedata.AppendChild(rState);
            rStatedata.AppendChild(rPreviousstate);
            rStatedata.AppendChild(rDeathreason);
            rStatedata.AppendChild(rBeeneaten);

            Rabbit.AppendChild(rStatedata);
            #endregion stateData


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

            #region lookingEntityData
            XmlElement rLookingentitydata = xmlDocument.CreateElement("lookingEntityData");

            XmlElement rShortesttoedibledistance = xmlDocument.CreateElement("shortestToEdibleDistance");
            XmlElement rShortesttowaterdistance = xmlDocument.CreateElement("shortestToWaterDistance");
            XmlElement rShortesttopredatordistance = xmlDocument.CreateElement("shortestToPredatorDistance");
            XmlElement rShortesttomatedistance = xmlDocument.CreateElement("shortestToMateDistance");


            rShortesttoedibledistance.InnerText = RabbitDefaults.shortestToEdibleDistance.ToString();
            rShortesttowaterdistance.InnerText = RabbitDefaults.shortestToWaterDistance.ToString();
            rShortesttopredatordistance.InnerText = RabbitDefaults.shortestToPredatorDistance.ToString();
            rShortesttomatedistance.InnerText = RabbitDefaults.shortestToMateDistance.ToString();


            rLookingentitydata.AppendChild(rShortesttoedibledistance);
            rLookingentitydata.AppendChild(rShortesttowaterdistance);
            rLookingentitydata.AppendChild(rShortesttopredatordistance);
            rLookingentitydata.AppendChild(rShortesttomatedistance);

            Rabbit.AppendChild(rLookingentitydata);
            #endregion lookingEntityData


            #region ColliderTypeData
            XmlElement rCollidertypedata = xmlDocument.CreateElement("ColliderTypeData");

            XmlElement rCollidertype = xmlDocument.CreateElement("colliderType");


            rCollidertype.InnerText = RabbitDefaults.colliderType.ToString();


            rCollidertypedata.AppendChild(rCollidertype);

            Rabbit.AppendChild(rCollidertypedata);
            #endregion ColliderTypeData


            root.AppendChild(Rabbit);
            #endregion Rabbit default


            #region Fox default
            XmlElement Fox = xmlDocument.CreateElement("Fox");

            #region ageData
            XmlElement fAgedata = xmlDocument.CreateElement("ageData");

            XmlElement fAge = xmlDocument.CreateElement("age");
            XmlElement fAgeincrease = xmlDocument.CreateElement("ageIncrease");
            XmlElement fAgemax = xmlDocument.CreateElement("ageMax");
            XmlElement fAgegroup = xmlDocument.CreateElement("ageGroup");
            XmlElement fAdultentrytimer = xmlDocument.CreateElement("adultEntryTimer");
            XmlElement fOldentrytimer = xmlDocument.CreateElement("oldEntryTimer");


            fAge.InnerText = FoxDefaults.age.ToString();
            fAgeincrease.InnerText = FoxDefaults.ageIncrease.ToString();
            fAgemax.InnerText = FoxDefaults.ageMax.ToString();
            fAgegroup.InnerText = FoxDefaults.ageGroup.ToString();
            fAdultentrytimer.InnerText = FoxDefaults.adultEntryTimer.ToString();
            fOldentrytimer.InnerText = FoxDefaults.oldEntryTimer.ToString();


            fAgedata.AppendChild(fAge);
            fAgedata.AppendChild(fAgeincrease);
            fAgedata.AppendChild(fAgemax);
            fAgedata.AppendChild(fAgegroup);
            fAgedata.AppendChild(fAdultentrytimer);
            fAgedata.AppendChild(fOldentrytimer);

            Fox.AppendChild(fAgedata);
            #endregion ageData

            #region edibleData
            XmlElement fEdibledata = xmlDocument.CreateElement("edibleData");

            XmlElement fNutritionalvalue = xmlDocument.CreateElement("nutritionalValue");
            XmlElement fCanbeeaten = xmlDocument.CreateElement("canBeEaten");
            XmlElement fNutritionalvaluemultiplier = xmlDocument.CreateElement("nutritionalValueMultiplier");
            XmlElement fFoodtype = xmlDocument.CreateElement("foodType");


            fNutritionalvalue.InnerText = FoxDefaults.nutritionalValue.ToString();
            fCanbeeaten.InnerText = FoxDefaults.canBeEaten.ToString();
            fNutritionalvaluemultiplier.InnerText = FoxDefaults.nutritionalValueMultiplier.ToString();
            fFoodtype.InnerText = FoxDefaults.foodType.ToString();


            fEdibledata.AppendChild(fNutritionalvalue);
            fEdibledata.AppendChild(fCanbeeaten);
            fEdibledata.AppendChild(fNutritionalvaluemultiplier);
            fEdibledata.AppendChild(fFoodtype);

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
            XmlElement fDiet = xmlDocument.CreateElement("diet");


            fHunger.InnerText = FoxDefaults.hunger.ToString();
            fHungermax.InnerText = FoxDefaults.hungerMax.ToString();
            fHungrythreshold.InnerText = FoxDefaults.hungryThreshold.ToString();
            fHungerincrease.InnerText = FoxDefaults.hungerIncrease.ToString();
            fPregnancyhungerincrease.InnerText = FoxDefaults.pregnancyHungerIncrease.ToString();
            fYounghungerincrease.InnerText = FoxDefaults.youngHungerIncrease.ToString();
            fAdulthungerincrease.InnerText = FoxDefaults.adultHungerIncrease.ToString();
            fOldhungerincrease.InnerText = FoxDefaults.oldHungerIncrease.ToString();
            fEatingspeed.InnerText = FoxDefaults.eatingSpeed.ToString();
            fDiet.InnerText = FoxDefaults.diet.ToString();


            fHungerdata.AppendChild(fHunger);
            fHungerdata.AppendChild(fHungermax);
            fHungerdata.AppendChild(fHungrythreshold);
            fHungerdata.AppendChild(fHungerincrease);
            fHungerdata.AppendChild(fPregnancyhungerincrease);
            fHungerdata.AppendChild(fYounghungerincrease);
            fHungerdata.AppendChild(fAdulthungerincrease);
            fHungerdata.AppendChild(fOldhungerincrease);
            fHungerdata.AppendChild(fEatingspeed);
            fHungerdata.AppendChild(fDiet);

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


            #region stateData
            XmlElement fStatedata = xmlDocument.CreateElement("stateData");

            XmlElement fState = xmlDocument.CreateElement("flagState");
            XmlElement fPreviousstate = xmlDocument.CreateElement("previousFlagState");
            XmlElement fDeathreason = xmlDocument.CreateElement("deathReason");
            XmlElement fBeeneaten = xmlDocument.CreateElement("beenEaten");


            fState.InnerText = FoxDefaults.flagState.ToString();
            fPreviousstate.InnerText = FoxDefaults.previousFlagState.ToString();
            fDeathreason.InnerText = FoxDefaults.deathReason.ToString();
            fBeeneaten.InnerText = FoxDefaults.beenEaten.ToString();


            fStatedata.AppendChild(fState);
            fStatedata.AppendChild(fPreviousstate);
            fStatedata.AppendChild(fDeathreason);
            fStatedata.AppendChild(fBeeneaten);

            Fox.AppendChild(fStatedata);
            #endregion stateData


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


            #region lookingEntityData
            XmlElement fLookingentitydata = xmlDocument.CreateElement("lookingEntityData");

            XmlElement fShortesttoedibledistance = xmlDocument.CreateElement("shortestToEdibleDistance");
            XmlElement fShortesttowaterdistance = xmlDocument.CreateElement("shortestToWaterDistance");
            XmlElement fShortesttopredatordistance = xmlDocument.CreateElement("shortestToPredatorDistance");
            XmlElement fShortesttomatedistance = xmlDocument.CreateElement("shortestToMateDistance");


            fShortesttoedibledistance.InnerText = FoxDefaults.shortestToEdibleDistance.ToString();
            fShortesttowaterdistance.InnerText = FoxDefaults.shortestToWaterDistance.ToString();
            fShortesttopredatordistance.InnerText = FoxDefaults.shortestToPredatorDistance.ToString();
            fShortesttomatedistance.InnerText = FoxDefaults.shortestToMateDistance.ToString();


            fLookingentitydata.AppendChild(fShortesttoedibledistance);
            fLookingentitydata.AppendChild(fShortesttowaterdistance);
            fLookingentitydata.AppendChild(fShortesttopredatordistance);
            fLookingentitydata.AppendChild(fShortesttomatedistance);

            Fox.AppendChild(fLookingentitydata);
            #endregion lookingEntityData


            #region ColliderTypeData
            XmlElement fCollidertypedata = xmlDocument.CreateElement("ColliderTypeData");

            XmlElement fCollidertype = xmlDocument.CreateElement("colliderType");


            fCollidertype.InnerText = FoxDefaults.colliderType.ToString();


            fCollidertypedata.AppendChild(fCollidertype);

            Fox.AppendChild(fCollidertypedata);
            #endregion ColliderTypeData


            root.AppendChild(Fox);
            #endregion Fox default


            #region Grass default

            XmlElement Grass = xmlDocument.CreateElement("Grass");

            #region edibleData
            XmlElement gEdibledata = xmlDocument.CreateElement("edibleData");

            XmlElement gNutritionalvalue = xmlDocument.CreateElement("nutritionalValue");
            XmlElement gCanbeeaten = xmlDocument.CreateElement("canBeEaten");
            XmlElement gNutritionalvaluemultiplier = xmlDocument.CreateElement("nutritionalValueMultiplier");
            XmlElement gFoodtype = xmlDocument.CreateElement("foodType");


            gNutritionalvalue.InnerText = GrassDefaults.nutritionalValue.ToString();
            gCanbeeaten.InnerText = GrassDefaults.canBeEaten.ToString();
            gNutritionalvaluemultiplier.InnerText = GrassDefaults.nutritionalValueMultiplier.ToString();
            gFoodtype.InnerText = GrassDefaults.foodType.ToString();


            gEdibledata.AppendChild(gNutritionalvalue);
            gEdibledata.AppendChild(gCanbeeaten);
            gEdibledata.AppendChild(gNutritionalvaluemultiplier);
            gEdibledata.AppendChild(gFoodtype);

            Grass.AppendChild(gEdibledata);
            #endregion edibleData


            #region sizeData
            XmlElement gSizedata = xmlDocument.CreateElement("sizeData");

            XmlElement gSizemultiplier = xmlDocument.CreateElement("sizeMultiplier");
            XmlElement gScale = xmlDocument.CreateElement("scale");


            gSizemultiplier.InnerText = GrassDefaults.sizeMultiplier.ToString();
            gScale.InnerText = GrassDefaults.scale.ToString();


            gSizedata.AppendChild(gSizemultiplier);
            gSizedata.AppendChild(gScale);

            Grass.AppendChild(gSizedata);
            #endregion sizeData



            #region stateData
            XmlElement gStatedata = xmlDocument.CreateElement("stateData");

            XmlElement gState = xmlDocument.CreateElement("flagState");
            XmlElement gPreviousstate = xmlDocument.CreateElement("previousFlagState");
            XmlElement gDeathreason = xmlDocument.CreateElement("deathReason");
            XmlElement gBeeneaten = xmlDocument.CreateElement("beenEaten");


            gState.InnerText = GrassDefaults.flagState.ToString();
            gPreviousstate.InnerText = GrassDefaults.previousFlagState.ToString();
            gDeathreason.InnerText = GrassDefaults.deathReason.ToString();
            gBeeneaten.InnerText = GrassDefaults.beenEaten.ToString();


            gStatedata.AppendChild(gState);
            gStatedata.AppendChild(gPreviousstate);
            gStatedata.AppendChild(gDeathreason);
            gStatedata.AppendChild(gBeeneaten);

            Grass.AppendChild(gStatedata);
            #endregion stateData

            #region ColliderTypeData
            XmlElement gCollidertypedata = xmlDocument.CreateElement("ColliderTypeData");

            XmlElement gCollidertype = xmlDocument.CreateElement("colliderType");


            gCollidertype.InnerText = GrassDefaults.GrassColliderType.ToString();


            gCollidertypedata.AppendChild(gCollidertype);

            Grass.AppendChild(gCollidertypedata);
            #endregion ColliderTypeData


            root.AppendChild(Grass);

            #endregion Grass default

            xmlDocument.AppendChild(root);


            xmlDocument.Save(path);
        }
    }
}
