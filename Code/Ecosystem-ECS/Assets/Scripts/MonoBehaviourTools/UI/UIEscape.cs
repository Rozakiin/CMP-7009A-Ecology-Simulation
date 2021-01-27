using System.Xml;
using EntityDefaults;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.UI
{
    public class UIEscape : MonoBehaviour
    {
        [SerializeField] private GameObject _escapeMenu;
        [SerializeField] private Button _resume;
        [SerializeField] private Button _save;
        [SerializeField] private Button _quit;
        private void Awake()
        {
            _escapeMenu.SetActive(false);
            _resume.onClick.AddListener(ResumeGame);
            _save.onClick.AddListener(SaveFile);
            _quit.onClick.AddListener(QuitGame);
        }
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (_escapeMenu.activeSelf)
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
            _escapeMenu.SetActive(false);
            UITimeControl.Instance.Play();
        }

        private void PauseGame()
        {
            _escapeMenu.SetActive(true);
            UITimeControl.Instance.Pause();
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
            rAge.InnerText = RabbitDefaults.Age.ToString();
            rAgeIncrease.InnerText = RabbitDefaults.AgeIncrease.ToString();
            rAgeMax.InnerText = RabbitDefaults.AgeMax.ToString();
            rAgeGroup.InnerText = RabbitDefaults.AgeGroup.ToString();
            rAdultEntryTimer.InnerText = RabbitDefaults.AdultEntryTimer.ToString();
            rOldEntryTimer.InnerText = RabbitDefaults.OldEntryTimer.ToString();

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


            rNutritionalvalue.InnerText = RabbitDefaults.NutritionalValue.ToString();
            rCanbeeaten.InnerText = RabbitDefaults.CanBeEaten.ToString();
            rNutritionalvaluemultiplier.InnerText = RabbitDefaults.NutritionalValueMultiplier.ToString();
            rFoodtype.InnerText = RabbitDefaults.FoodType.ToString();


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
            XmlElement rDiet = xmlDocument.CreateElement("DietType");


            rHunger.InnerText = RabbitDefaults.Hunger.ToString();
            rHungermax.InnerText = RabbitDefaults.HungerMax.ToString();
            rHungrythreshold.InnerText = RabbitDefaults.HungryThreshold.ToString();
            rHungerincrease.InnerText = RabbitDefaults.HungerIncrease.ToString();
            rPregnancyhungerincrease.InnerText = RabbitDefaults.PregnancyHungerIncrease.ToString();
            rYounghungerincrease.InnerText = RabbitDefaults.YoungHungerIncrease.ToString();
            rAdulthungerincrease.InnerText = RabbitDefaults.AdultHungerIncrease.ToString();
            rOldhungerincrease.InnerText = RabbitDefaults.OldHungerIncrease.ToString();
            rEatingspeed.InnerText = RabbitDefaults.EatingSpeed.ToString();
            rDiet.InnerText = RabbitDefaults.Diet.ToString();


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

            rThirst.InnerText = RabbitDefaults.Thirst.ToString();
            rThirstMax.InnerText = RabbitDefaults.ThirstMax.ToString();
            rThirstyThreshold.InnerText = RabbitDefaults.ThirstyThreshold.ToString();
            rThirstIncrease.InnerText = RabbitDefaults.ThirstIncrease.ToString();
            rDrinkingSpeed.InnerText = RabbitDefaults.DrinkingSpeed.ToString();

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


            rMatestarttime.InnerText = RabbitDefaults.MateStartTime.ToString();
            rMatingduration.InnerText = RabbitDefaults.MatingDuration.ToString();
            rReproductiveurge.InnerText = RabbitDefaults.ReproductiveUrge.ToString();
            rReproductiveurgeincreasemale.InnerText = RabbitDefaults.ReproductiveUrgeIncreaseMale.ToString();
            rReproductiveurgeincreasefemale.InnerText = RabbitDefaults.ReproductiveUrgeIncreaseFemale.ToString();
            rMatingthreshold.InnerText = RabbitDefaults.MatingThreshold.ToString();


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

            rPregnancystarttime.InnerText = RabbitDefaults.PregnancyStartTime.ToString();
            rBabiesborn.InnerText = RabbitDefaults.BabiesBorn.ToString();
            rBirthstarttime.InnerText = RabbitDefaults.BirthStartTime.ToString();
            rCurrentlittersize.InnerText = RabbitDefaults.CurrentLitterSize.ToString();
            rPregnancylengthmodifier.InnerText = RabbitDefaults.PregnancyLengthModifier.ToString();
            rPregnancylength.InnerText = RabbitDefaults.PregnancyLength.ToString();
            rBirthduration.InnerText = RabbitDefaults.BirthDuration.ToString();
            rLittersizemin.InnerText = RabbitDefaults.LitterSizeMin.ToString();
            rLittersizemax.InnerText = RabbitDefaults.LitterSizeMax.ToString();
            rLittersizeave.InnerText = RabbitDefaults.LitterSizeAve.ToString();


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


            rMovespeed.InnerText = RabbitDefaults.MoveSpeed.ToString();
            rRotationspeed.InnerText = RabbitDefaults.RotationSpeed.ToString();
            rMovemultiplier.InnerText = RabbitDefaults.MoveMultiplier.ToString();
            rPregnancymovemultiplier.InnerText = RabbitDefaults.PregnancyMoveMultiplier.ToString();
            rOriginalmovemultiplier.InnerText = RabbitDefaults.OriginalMoveMultiplier.ToString();
            rYoungmovemultiplier.InnerText = RabbitDefaults.YoungMoveMultiplier.ToString();
            rAdultmovemultiplier.InnerText = RabbitDefaults.AdultMoveMultiplier.ToString();
            rOldmovemultiplier.InnerText = RabbitDefaults.OldMoveMultiplier.ToString();


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


            rSizemultiplier.InnerText = RabbitDefaults.SizeMultiplier.ToString();
            rScalemale.InnerText = RabbitDefaults.ScaleMale.ToString();
            rScalefemale.InnerText = RabbitDefaults.ScaleFemale.ToString();
            rYoungsizemultiplier.InnerText = RabbitDefaults.YoungSizeMultiplier.ToString();
            rAdultsizemultiplier.InnerText = RabbitDefaults.AdultSizeMultiplier.ToString();
            rOldsizemultiplier.InnerText = RabbitDefaults.OldSizeMultiplier.ToString();


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
            XmlElement rPreviousstate = xmlDocument.CreateElement("FlagStatePrevious");
            XmlElement rDeathreason = xmlDocument.CreateElement("DeathReason");
            XmlElement rBeeneaten = xmlDocument.CreateElement("beenEaten");


            rState.InnerText = RabbitDefaults.FlagState.ToString();
            rPreviousstate.InnerText = RabbitDefaults.FlagStatePrevious.ToString();
            rDeathreason.InnerText = RabbitDefaults.DeathReason.ToString();
            rBeeneaten.InnerText = RabbitDefaults.BeenEaten.ToString();


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


            rTouchradius.InnerText = RabbitDefaults.TouchRadius.ToString();
            rSightradius.InnerText = RabbitDefaults.SightRadius.ToString();


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


            rShortesttoedibledistance.InnerText = RabbitDefaults.ShortestToEdibleDistance.ToString();
            rShortesttowaterdistance.InnerText = RabbitDefaults.ShortestToWaterDistance.ToString();
            rShortesttopredatordistance.InnerText = RabbitDefaults.ShortestToPredatorDistance.ToString();
            rShortesttomatedistance.InnerText = RabbitDefaults.ShortestToMateDistance.ToString();


            rLookingentitydata.AppendChild(rShortesttoedibledistance);
            rLookingentitydata.AppendChild(rShortesttowaterdistance);
            rLookingentitydata.AppendChild(rShortesttopredatordistance);
            rLookingentitydata.AppendChild(rShortesttomatedistance);

            Rabbit.AppendChild(rLookingentitydata);
            #endregion lookingEntityData


            #region ColliderTypeData
            XmlElement rCollidertypedata = xmlDocument.CreateElement("ColliderTypeData");

            XmlElement rCollidertype = xmlDocument.CreateElement("Collider");


            rCollidertype.InnerText = RabbitDefaults.Collider.ToString();


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


            fAge.InnerText = FoxDefaults.Age.ToString();
            fAgeincrease.InnerText = FoxDefaults.AgeIncrease.ToString();
            fAgemax.InnerText = FoxDefaults.AgeMax.ToString();
            fAgegroup.InnerText = FoxDefaults.AgeGroup.ToString();
            fAdultentrytimer.InnerText = FoxDefaults.AdultEntryTimer.ToString();
            fOldentrytimer.InnerText = FoxDefaults.OldEntryTimer.ToString();


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


            fNutritionalvalue.InnerText = FoxDefaults.NutritionalValue.ToString();
            fCanbeeaten.InnerText = FoxDefaults.CanBeEaten.ToString();
            fNutritionalvaluemultiplier.InnerText = FoxDefaults.NutritionalValueMultiplier.ToString();
            fFoodtype.InnerText = FoxDefaults.FoodType.ToString();


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
            XmlElement fDiet = xmlDocument.CreateElement("DietType");


            fHunger.InnerText = FoxDefaults.Hunger.ToString();
            fHungermax.InnerText = FoxDefaults.HungerMax.ToString();
            fHungrythreshold.InnerText = FoxDefaults.HungryThreshold.ToString();
            fHungerincrease.InnerText = FoxDefaults.HungerIncrease.ToString();
            fPregnancyhungerincrease.InnerText = FoxDefaults.PregnancyHungerIncrease.ToString();
            fYounghungerincrease.InnerText = FoxDefaults.YoungHungerIncrease.ToString();
            fAdulthungerincrease.InnerText = FoxDefaults.AdultHungerIncrease.ToString();
            fOldhungerincrease.InnerText = FoxDefaults.OldHungerIncrease.ToString();
            fEatingspeed.InnerText = FoxDefaults.EatingSpeed.ToString();
            fDiet.InnerText = FoxDefaults.Diet.ToString();


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


            fThirst.InnerText = FoxDefaults.Thirst.ToString();
            fThirstmax.InnerText = FoxDefaults.ThirstMax.ToString();
            fThirstythreshold.InnerText = FoxDefaults.ThirstyThreshold.ToString();
            fThirstincrease.InnerText = FoxDefaults.ThirstIncrease.ToString();
            fDrinkingspeed.InnerText = FoxDefaults.DrinkingSpeed.ToString();


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


            fMatestarttime.InnerText = FoxDefaults.MateStartTime.ToString();
            fMatingduration.InnerText = FoxDefaults.MatingDuration.ToString();
            fReproductiveurge.InnerText = FoxDefaults.ReproductiveUrge.ToString();
            fReproductiveurgeincreasemale.InnerText = FoxDefaults.ReproductiveUrgeIncreaseMale.ToString();
            fReproductiveurgeincreasefemale.InnerText = FoxDefaults.ReproductiveUrgeIncreaseFemale.ToString();
            fMatingthreshold.InnerText = FoxDefaults.MatingThreshold.ToString();


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


            fPregnancystarttime.InnerText = FoxDefaults.PregnancyStartTime.ToString();
            fBabiesborn.InnerText = FoxDefaults.BabiesBorn.ToString();
            fBirthstarttime.InnerText = FoxDefaults.BirthStartTime.ToString();
            fCurrentlittersize.InnerText = FoxDefaults.CurrentLitterSize.ToString();
            fPregnancylengthmodifier.InnerText = FoxDefaults.PregnancyLengthModifier.ToString();
            fPregnancylength.InnerText = FoxDefaults.PregnancyLength.ToString();
            fBirthduration.InnerText = FoxDefaults.BirthDuration.ToString();
            fLittersizemin.InnerText = FoxDefaults.LitterSizeMin.ToString();
            fLittersizemax.InnerText = FoxDefaults.LitterSizeMax.ToString();
            fLittersizeave.InnerText = FoxDefaults.LitterSizeAve.ToString();

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


            fMovespeed.InnerText = FoxDefaults.MoveSpeed.ToString();
            fRotationspeed.InnerText = FoxDefaults.RotationSpeed.ToString();
            fMovemultiplier.InnerText = FoxDefaults.MoveMultiplier.ToString();
            fPregnancymovemultiplier.InnerText = FoxDefaults.PregnancyMoveMultiplier.ToString();
            fOriginalmovemultiplier.InnerText = FoxDefaults.OriginalMoveMultiplier.ToString();
            fYoungmovemultiplier.InnerText = FoxDefaults.YoungMoveMultiplier.ToString();
            fAdultmovemultiplier.InnerText = FoxDefaults.AdultMoveMultiplier.ToString();
            fOldmovemultiplier.InnerText = FoxDefaults.OldMoveMultiplier.ToString();


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


            fSizemultiplier.InnerText = FoxDefaults.SizeMultiplier.ToString();
            fScalemale.InnerText = FoxDefaults.ScaleMale.ToString();
            fScalefemale.InnerText = FoxDefaults.ScaleFemale.ToString();
            fYoungsizemultiplier.InnerText = FoxDefaults.YoungSizeMultiplier.ToString();
            fAdultsizemultiplier.InnerText = FoxDefaults.AdultSizeMultiplier.ToString();
            fOldsizemultiplier.InnerText = FoxDefaults.OldSizeMultiplier.ToString();


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
            XmlElement fPreviousstate = xmlDocument.CreateElement("FlagStatePrevious");
            XmlElement fDeathreason = xmlDocument.CreateElement("DeathReason");
            XmlElement fBeeneaten = xmlDocument.CreateElement("beenEaten");


            fState.InnerText = FoxDefaults.FlagState.ToString();
            fPreviousstate.InnerText = FoxDefaults.PreviousFlagState.ToString();
            fDeathreason.InnerText = FoxDefaults.DeathReason.ToString();
            fBeeneaten.InnerText = FoxDefaults.BeenEaten.ToString();


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


            fTouchradius.InnerText = FoxDefaults.TouchRadius.ToString();
            fSightradius.InnerText = FoxDefaults.SightRadius.ToString();


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


            fShortesttoedibledistance.InnerText = FoxDefaults.ShortestToEdibleDistance.ToString();
            fShortesttowaterdistance.InnerText = FoxDefaults.ShortestToWaterDistance.ToString();
            fShortesttopredatordistance.InnerText = FoxDefaults.ShortestToPredatorDistance.ToString();
            fShortesttomatedistance.InnerText = FoxDefaults.ShortestToMateDistance.ToString();


            fLookingentitydata.AppendChild(fShortesttoedibledistance);
            fLookingentitydata.AppendChild(fShortesttowaterdistance);
            fLookingentitydata.AppendChild(fShortesttopredatordistance);
            fLookingentitydata.AppendChild(fShortesttomatedistance);

            Fox.AppendChild(fLookingentitydata);
            #endregion lookingEntityData


            #region ColliderTypeData
            XmlElement fCollidertypedata = xmlDocument.CreateElement("ColliderTypeData");

            XmlElement fCollidertype = xmlDocument.CreateElement("Collider");


            fCollidertype.InnerText = FoxDefaults.Collider.ToString();


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


            gNutritionalvalue.InnerText = GrassDefaults.NutritionalValue.ToString();
            gCanbeeaten.InnerText = GrassDefaults.CanBeEaten.ToString();
            gNutritionalvaluemultiplier.InnerText = GrassDefaults.NutritionalValueMultiplier.ToString();
            gFoodtype.InnerText = GrassDefaults.FoodType.ToString();


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


            gSizemultiplier.InnerText = GrassDefaults.SizeMultiplier.ToString();
            gScale.InnerText = GrassDefaults.Scale.ToString();


            gSizedata.AppendChild(gSizemultiplier);
            gSizedata.AppendChild(gScale);

            Grass.AppendChild(gSizedata);
            #endregion sizeData



            #region stateData
            XmlElement gStatedata = xmlDocument.CreateElement("stateData");

            XmlElement gState = xmlDocument.CreateElement("flagState");
            XmlElement gPreviousstate = xmlDocument.CreateElement("FlagStatePrevious");
            XmlElement gDeathreason = xmlDocument.CreateElement("DeathReason");
            XmlElement gBeeneaten = xmlDocument.CreateElement("beenEaten");


            gState.InnerText = GrassDefaults.FlagState.ToString();
            gPreviousstate.InnerText = GrassDefaults.PreviousFlagState.ToString();
            gDeathreason.InnerText = GrassDefaults.DeathReason.ToString();
            gBeeneaten.InnerText = GrassDefaults.BeenEaten.ToString();


            gStatedata.AppendChild(gState);
            gStatedata.AppendChild(gPreviousstate);
            gStatedata.AppendChild(gDeathreason);
            gStatedata.AppendChild(gBeeneaten);

            Grass.AppendChild(gStatedata);
            #endregion stateData

            #region ColliderTypeData
            XmlElement gCollidertypedata = xmlDocument.CreateElement("ColliderTypeData");

            XmlElement gCollidertype = xmlDocument.CreateElement("Collider");


            gCollidertype.InnerText = GrassDefaults.Collider.ToString();


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
