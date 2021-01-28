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

        private static void QuitGame()
        {
            Application.Quit();
            Debug.Log("I am Quit");
        }

        private void SaveFile()
        {
            var path = StandaloneFileBrowser.StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "xml");
            if (path != string.Empty)
            {
                SaveGame(path);
            }
            else
            {
                Debug.Log("No Path Insert");
            }

        }

        private static void SaveGame(string path)
        {
            var xmlDocument = new XmlDocument();

            //#region Create XMLDocument Elements
            var root = xmlDocument.CreateElement("XML");

            #region Rabbit default
            var rabbit = xmlDocument.CreateElement("Rabbit");

            #region AgeData
            var rAgeData = xmlDocument.CreateElement("AgeData");

            var rAge = xmlDocument.CreateElement("age");
            var rAgeIncrease = xmlDocument.CreateElement("ageIncrease");
            var rAgeMax = xmlDocument.CreateElement("ageMax");
            var rAgeGroup = xmlDocument.CreateElement("ageGroup");
            var rAdultEntryTimer = xmlDocument.CreateElement("adultEntryTimer");
            var rOldEntryTimer = xmlDocument.CreateElement("oldEntryTimer");

            //public static BioStatsData.AgeGroup ageGroup = BioStatsData.AgeGroup.Young;
            rAge.InnerText = RabbitDefaults.Age.ToString("N");
            rAgeIncrease.InnerText = RabbitDefaults.AgeIncrease.ToString("N");
            rAgeMax.InnerText = RabbitDefaults.AgeMax.ToString("N");
            rAgeGroup.InnerText = RabbitDefaults.AgeGroup.ToString("N");
            rAdultEntryTimer.InnerText = RabbitDefaults.AdultEntryTimer.ToString("N");
            rOldEntryTimer.InnerText = RabbitDefaults.OldEntryTimer.ToString("N");

            rAgeData.AppendChild(rAge);
            rAgeData.AppendChild(rAgeIncrease);
            rAgeData.AppendChild(rAgeMax);
            rAgeData.AppendChild(rAgeGroup);
            rAgeData.AppendChild(rAdultEntryTimer);
            rAgeData.AppendChild(rOldEntryTimer);

            rabbit.AppendChild(rAgeData);
            #endregion age data

            #region edibleData
            var rEdibledata = xmlDocument.CreateElement("edibleData");

            var rNutritionalvalue = xmlDocument.CreateElement("nutritionalValue");
            var rCanbeeaten = xmlDocument.CreateElement("canBeEaten");
            var rNutritionalvaluemultiplier = xmlDocument.CreateElement("nutritionalValueMultiplier");
            var rFoodtype = xmlDocument.CreateElement("foodType");


            rNutritionalvalue.InnerText = RabbitDefaults.NutritionalValue.ToString("N");
            rCanbeeaten.InnerText = RabbitDefaults.CanBeEaten.ToString();
            rNutritionalvaluemultiplier.InnerText = RabbitDefaults.NutritionalValueMultiplier.ToString("N");
            rFoodtype.InnerText = RabbitDefaults.FoodType.ToString("N");


            rEdibledata.AppendChild(rNutritionalvalue);
            rEdibledata.AppendChild(rCanbeeaten);
            rEdibledata.AppendChild(rNutritionalvaluemultiplier);
            rEdibledata.AppendChild(rFoodtype);

            rabbit.AppendChild(rEdibledata);
            #endregion edibleData

            #region hungerData
            var rHungerdata = xmlDocument.CreateElement("hungerData");

            var rHunger = xmlDocument.CreateElement("hunger");
            var rHungermax = xmlDocument.CreateElement("hungerMax");
            var rHungrythreshold = xmlDocument.CreateElement("hungryThreshold");
            var rHungerincrease = xmlDocument.CreateElement("hungerIncrease");
            var rPregnancyhungerincrease = xmlDocument.CreateElement("pregnancyHungerIncrease");
            var rYounghungerincrease = xmlDocument.CreateElement("youngHungerIncrease");
            var rAdulthungerincrease = xmlDocument.CreateElement("adultHungerIncrease");
            var rOldhungerincrease = xmlDocument.CreateElement("oldHungerIncrease");
            var rEatingspeed = xmlDocument.CreateElement("eatingSpeed");
            var rDiet = xmlDocument.CreateElement("DietType");


            rHunger.InnerText = RabbitDefaults.Hunger.ToString("N");
            rHungermax.InnerText = RabbitDefaults.HungerMax.ToString("N");
            rHungrythreshold.InnerText = RabbitDefaults.HungryThreshold.ToString("N");
            rHungerincrease.InnerText = RabbitDefaults.HungerIncrease.ToString("N");
            rPregnancyhungerincrease.InnerText = RabbitDefaults.PregnancyHungerIncrease.ToString("N");
            rYounghungerincrease.InnerText = RabbitDefaults.YoungHungerIncrease.ToString("N");
            rAdulthungerincrease.InnerText = RabbitDefaults.AdultHungerIncrease.ToString("N");
            rOldhungerincrease.InnerText = RabbitDefaults.OldHungerIncrease.ToString("N");
            rEatingspeed.InnerText = RabbitDefaults.EatingSpeed.ToString("N");
            rDiet.InnerText = RabbitDefaults.Diet.ToString("N");


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

            rabbit.AppendChild(rHungerdata);
            #endregion hungerData

            #region Thirst data
            var rThirstData = xmlDocument.CreateElement("ThirstData");

            var rThirst = xmlDocument.CreateElement("thirst");
            var rThirstMax = xmlDocument.CreateElement("thirstMax");
            var rThirstyThreshold = xmlDocument.CreateElement("thirstyThreshold");
            var rThirstIncrease = xmlDocument.CreateElement("thirstIncrease");
            var rDrinkingSpeed = xmlDocument.CreateElement("drinkingSpeed");

            rThirst.InnerText = RabbitDefaults.Thirst.ToString("N");
            rThirstMax.InnerText = RabbitDefaults.ThirstMax.ToString("N");
            rThirstyThreshold.InnerText = RabbitDefaults.ThirstyThreshold.ToString("N");
            rThirstIncrease.InnerText = RabbitDefaults.ThirstIncrease.ToString("N");
            rDrinkingSpeed.InnerText = RabbitDefaults.DrinkingSpeed.ToString("N");

            rThirstData.AppendChild(rThirst);
            rThirstData.AppendChild(rThirstMax);
            rThirstData.AppendChild(rThirstyThreshold);
            rThirstData.AppendChild(rThirstIncrease);
            rThirstData.AppendChild(rDrinkingSpeed);

            rabbit.AppendChild(rThirstData);
            #endregion Thirst data

            #region mateData
            var rMatedata = xmlDocument.CreateElement("mateData");


            var rMatestarttime = xmlDocument.CreateElement("mateStartTime");
            var rMatingduration = xmlDocument.CreateElement("matingDuration");
            var rReproductiveurge = xmlDocument.CreateElement("reproductiveUrge");
            var rReproductiveurgeincreasemale = xmlDocument.CreateElement("reproductiveUrgeIncreaseMale");
            var rReproductiveurgeincreasefemale = xmlDocument.CreateElement("reproductiveUrgeIncreaseFemale");
            var rMatingthreshold = xmlDocument.CreateElement("matingThreshold");


            rMatestarttime.InnerText = RabbitDefaults.MateStartTime.ToString("N");
            rMatingduration.InnerText = RabbitDefaults.MatingDuration.ToString("N");
            rReproductiveurge.InnerText = RabbitDefaults.ReproductiveUrge.ToString("N");
            rReproductiveurgeincreasemale.InnerText = RabbitDefaults.ReproductiveUrgeIncreaseMale.ToString("N");
            rReproductiveurgeincreasefemale.InnerText = RabbitDefaults.ReproductiveUrgeIncreaseFemale.ToString("N");
            rMatingthreshold.InnerText = RabbitDefaults.MatingThreshold.ToString("N");


            rMatedata.AppendChild(rMatestarttime);
            rMatedata.AppendChild(rMatingduration);
            rMatedata.AppendChild(rReproductiveurge);
            rMatedata.AppendChild(rReproductiveurgeincreasemale);
            rMatedata.AppendChild(rReproductiveurgeincreasefemale);
            rMatedata.AppendChild(rMatingthreshold);


            rabbit.AppendChild(rMatedata);
            #endregion mateData

            #region pregnancyData
            var rPregnancydata = xmlDocument.CreateElement("pregnancyData");

            var rPregnancystarttime = xmlDocument.CreateElement("pregnancyStartTime");
            var rBabiesborn = xmlDocument.CreateElement("babiesBorn");
            var rBirthstarttime = xmlDocument.CreateElement("birthStartTime");
            var rCurrentlittersize = xmlDocument.CreateElement("currentLitterSize");
            var rPregnancylengthmodifier = xmlDocument.CreateElement("pregnancyLengthModifier");
            var rPregnancylength = xmlDocument.CreateElement("pregnancyLength");
            var rBirthduration = xmlDocument.CreateElement("birthDuration");
            var rLittersizemin = xmlDocument.CreateElement("litterSizeMin");
            var rLittersizemax = xmlDocument.CreateElement("litterSizeMax");
            var rLittersizeave = xmlDocument.CreateElement("litterSizeAve");

            rPregnancystarttime.InnerText = RabbitDefaults.PregnancyStartTime.ToString("N");
            rBabiesborn.InnerText = RabbitDefaults.BabiesBorn.ToString("N");
            rBirthstarttime.InnerText = RabbitDefaults.BirthStartTime.ToString("N");
            rCurrentlittersize.InnerText = RabbitDefaults.CurrentLitterSize.ToString("N");
            rPregnancylengthmodifier.InnerText = RabbitDefaults.PregnancyLengthModifier.ToString("N");
            rPregnancylength.InnerText = RabbitDefaults.PregnancyLength.ToString("N");
            rBirthduration.InnerText = RabbitDefaults.BirthDuration.ToString("N");
            rLittersizemin.InnerText = RabbitDefaults.LitterSizeMin.ToString("N");
            rLittersizemax.InnerText = RabbitDefaults.LitterSizeMax.ToString("N");
            rLittersizeave.InnerText = RabbitDefaults.LitterSizeAve.ToString("N");


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

            rabbit.AppendChild(rPregnancydata);
            #endregion pregnancyData

            #region movementData
            var rMovementData = xmlDocument.CreateElement("movementData");


            var rMovespeed = xmlDocument.CreateElement("moveSpeed");
            var rRotationspeed = xmlDocument.CreateElement("rotationSpeed");
            var rMovemultiplier = xmlDocument.CreateElement("moveMultiplier");
            var rPregnancymovemultiplier = xmlDocument.CreateElement("pregnancyMoveMultiplier");
            var rOriginalmovemultiplier = xmlDocument.CreateElement("originalMoveMultiplier");
            var rYoungmovemultiplier = xmlDocument.CreateElement("youngMoveMultiplier");
            var rAdultmovemultiplier = xmlDocument.CreateElement("adultMoveMultiplier");
            var rOldmovemultiplier = xmlDocument.CreateElement("oldMoveMultiplier");


            rMovespeed.InnerText = RabbitDefaults.MoveSpeed.ToString("N");
            rRotationspeed.InnerText = RabbitDefaults.RotationSpeed.ToString("N");
            rMovemultiplier.InnerText = RabbitDefaults.MoveMultiplier.ToString("N");
            rPregnancymovemultiplier.InnerText = RabbitDefaults.PregnancyMoveMultiplier.ToString("N");
            rOriginalmovemultiplier.InnerText = RabbitDefaults.OriginalMoveMultiplier.ToString("N");
            rYoungmovemultiplier.InnerText = RabbitDefaults.YoungMoveMultiplier.ToString("N");
            rAdultmovemultiplier.InnerText = RabbitDefaults.AdultMoveMultiplier.ToString("N");
            rOldmovemultiplier.InnerText = RabbitDefaults.OldMoveMultiplier.ToString("N");


            rMovementData.AppendChild(rMovespeed);
            rMovementData.AppendChild(rRotationspeed);
            rMovementData.AppendChild(rMovemultiplier);
            rMovementData.AppendChild(rPregnancymovemultiplier);
            rMovementData.AppendChild(rOriginalmovemultiplier);
            rMovementData.AppendChild(rYoungmovemultiplier);
            rMovementData.AppendChild(rAdultmovemultiplier);
            rMovementData.AppendChild(rOldmovemultiplier);


            rabbit.AppendChild(rMovementData);
            #endregion movementData

            #region sizeData
            var rSizeData = xmlDocument.CreateElement("sizeData");


            var rSizemultiplier = xmlDocument.CreateElement("sizeMultiplier");
            var rScalemale = xmlDocument.CreateElement("scaleMale");
            var rScalefemale = xmlDocument.CreateElement("scaleFemale");
            var rYoungsizemultiplier = xmlDocument.CreateElement("youngSizeMultiplier");
            var rAdultsizemultiplier = xmlDocument.CreateElement("adultSizeMultiplier");
            var rOldsizemultiplier = xmlDocument.CreateElement("oldSizeMultiplier");


            rSizemultiplier.InnerText = RabbitDefaults.SizeMultiplier.ToString("N");
            rScalemale.InnerText = RabbitDefaults.ScaleMale.ToString("N");
            rScalefemale.InnerText = RabbitDefaults.ScaleFemale.ToString("N");
            rYoungsizemultiplier.InnerText = RabbitDefaults.YoungSizeMultiplier.ToString("N");
            rAdultsizemultiplier.InnerText = RabbitDefaults.AdultSizeMultiplier.ToString("N");
            rOldsizemultiplier.InnerText = RabbitDefaults.OldSizeMultiplier.ToString("N");


            rSizeData.AppendChild(rSizemultiplier);
            rSizeData.AppendChild(rScalemale);
            rSizeData.AppendChild(rScalefemale);
            rSizeData.AppendChild(rYoungsizemultiplier);
            rSizeData.AppendChild(rAdultsizemultiplier);
            rSizeData.AppendChild(rOldsizemultiplier);


            rabbit.AppendChild(rSizeData);
            #endregion sizeData


            #region stateData
            var rStatedata = xmlDocument.CreateElement("stateData");

            var rState = xmlDocument.CreateElement("flagState");
            var rPreviousstate = xmlDocument.CreateElement("FlagStatePrevious");
            var rDeathreason = xmlDocument.CreateElement("DeathReason");
            var rBeeneaten = xmlDocument.CreateElement("beenEaten");


            rState.InnerText = RabbitDefaults.FlagState.ToString("N");
            rPreviousstate.InnerText = RabbitDefaults.FlagStatePrevious.ToString("N");
            rDeathreason.InnerText = RabbitDefaults.DeathReason.ToString("N");
            rBeeneaten.InnerText = RabbitDefaults.BeenEaten.ToString();


            rStatedata.AppendChild(rState);
            rStatedata.AppendChild(rPreviousstate);
            rStatedata.AppendChild(rDeathreason);
            rStatedata.AppendChild(rBeeneaten);

            rabbit.AppendChild(rStatedata);
            #endregion stateData


            #region targetData
            var rTargetData = xmlDocument.CreateElement("targetData");


            var rTouchradius = xmlDocument.CreateElement("touchRadius");
            var rSightradius = xmlDocument.CreateElement("sightRadius");


            rTouchradius.InnerText = RabbitDefaults.TouchRadius.ToString("N");
            rSightradius.InnerText = RabbitDefaults.SightRadius.ToString("N");


            rTargetData.AppendChild(rTouchradius);
            rTargetData.AppendChild(rSightradius);


            rabbit.AppendChild(rTargetData);
            #endregion targetData

            #region lookingEntityData
            var rLookingentitydata = xmlDocument.CreateElement("lookingEntityData");

            var rShortesttoedibledistance = xmlDocument.CreateElement("shortestToEdibleDistance");
            var rShortesttowaterdistance = xmlDocument.CreateElement("shortestToWaterDistance");
            var rShortesttopredatordistance = xmlDocument.CreateElement("shortestToPredatorDistance");
            var rShortesttomatedistance = xmlDocument.CreateElement("shortestToMateDistance");


            rShortesttoedibledistance.InnerText = RabbitDefaults.ShortestToEdibleDistance.ToString("N");
            rShortesttowaterdistance.InnerText = RabbitDefaults.ShortestToWaterDistance.ToString("N");
            rShortesttopredatordistance.InnerText = RabbitDefaults.ShortestToPredatorDistance.ToString("N");
            rShortesttomatedistance.InnerText = RabbitDefaults.ShortestToMateDistance.ToString("N");


            rLookingentitydata.AppendChild(rShortesttoedibledistance);
            rLookingentitydata.AppendChild(rShortesttowaterdistance);
            rLookingentitydata.AppendChild(rShortesttopredatordistance);
            rLookingentitydata.AppendChild(rShortesttomatedistance);

            rabbit.AppendChild(rLookingentitydata);
            #endregion lookingEntityData


            #region ColliderTypeData
            var rCollidertypedata = xmlDocument.CreateElement("ColliderTypeData");

            var rCollidertype = xmlDocument.CreateElement("Collider");


            rCollidertype.InnerText = RabbitDefaults.Collider.ToString("N");


            rCollidertypedata.AppendChild(rCollidertype);

            rabbit.AppendChild(rCollidertypedata);
            #endregion ColliderTypeData


            root.AppendChild(rabbit);
            #endregion Rabbit default


            #region Fox default
            var fox = xmlDocument.CreateElement("Fox");

            #region ageData
            var fAgedata = xmlDocument.CreateElement("ageData");

            var fAge = xmlDocument.CreateElement("age");
            var fAgeincrease = xmlDocument.CreateElement("ageIncrease");
            var fAgemax = xmlDocument.CreateElement("ageMax");
            var fAgegroup = xmlDocument.CreateElement("ageGroup");
            var fAdultentrytimer = xmlDocument.CreateElement("adultEntryTimer");
            var fOldentrytimer = xmlDocument.CreateElement("oldEntryTimer");


            fAge.InnerText = FoxDefaults.Age.ToString("N");
            fAgeincrease.InnerText = FoxDefaults.AgeIncrease.ToString("N");
            fAgemax.InnerText = FoxDefaults.AgeMax.ToString("N");
            fAgegroup.InnerText = FoxDefaults.AgeGroup.ToString("N");
            fAdultentrytimer.InnerText = FoxDefaults.AdultEntryTimer.ToString("N");
            fOldentrytimer.InnerText = FoxDefaults.OldEntryTimer.ToString("N");


            fAgedata.AppendChild(fAge);
            fAgedata.AppendChild(fAgeincrease);
            fAgedata.AppendChild(fAgemax);
            fAgedata.AppendChild(fAgegroup);
            fAgedata.AppendChild(fAdultentrytimer);
            fAgedata.AppendChild(fOldentrytimer);

            fox.AppendChild(fAgedata);
            #endregion ageData

            #region edibleData
            var fEdibledata = xmlDocument.CreateElement("edibleData");

            var fNutritionalvalue = xmlDocument.CreateElement("nutritionalValue");
            var fCanbeeaten = xmlDocument.CreateElement("canBeEaten");
            var fNutritionalvaluemultiplier = xmlDocument.CreateElement("nutritionalValueMultiplier");
            var fFoodtype = xmlDocument.CreateElement("foodType");


            fNutritionalvalue.InnerText = FoxDefaults.NutritionalValue.ToString("N");
            fCanbeeaten.InnerText = FoxDefaults.CanBeEaten.ToString();
            fNutritionalvaluemultiplier.InnerText = FoxDefaults.NutritionalValueMultiplier.ToString("N");
            fFoodtype.InnerText = FoxDefaults.FoodType.ToString("N");


            fEdibledata.AppendChild(fNutritionalvalue);
            fEdibledata.AppendChild(fCanbeeaten);
            fEdibledata.AppendChild(fNutritionalvaluemultiplier);
            fEdibledata.AppendChild(fFoodtype);

            fox.AppendChild(fEdibledata);
            #endregion edibleData


            #region hungerData
            var fHungerdata = xmlDocument.CreateElement("hungerData");

            var fHunger = xmlDocument.CreateElement("hunger");
            var fHungermax = xmlDocument.CreateElement("hungerMax");
            var fHungrythreshold = xmlDocument.CreateElement("hungryThreshold");
            var fHungerincrease = xmlDocument.CreateElement("hungerIncrease");
            var fPregnancyhungerincrease = xmlDocument.CreateElement("pregnancyHungerIncrease");
            var fYounghungerincrease = xmlDocument.CreateElement("youngHungerIncrease");
            var fAdulthungerincrease = xmlDocument.CreateElement("adultHungerIncrease");
            var fOldhungerincrease = xmlDocument.CreateElement("oldHungerIncrease");
            var fEatingspeed = xmlDocument.CreateElement("eatingSpeed");
            var fDiet = xmlDocument.CreateElement("DietType");


            fHunger.InnerText = FoxDefaults.Hunger.ToString("N");
            fHungermax.InnerText = FoxDefaults.HungerMax.ToString("N");
            fHungrythreshold.InnerText = FoxDefaults.HungryThreshold.ToString("N");
            fHungerincrease.InnerText = FoxDefaults.HungerIncrease.ToString("N");
            fPregnancyhungerincrease.InnerText = FoxDefaults.PregnancyHungerIncrease.ToString("N");
            fYounghungerincrease.InnerText = FoxDefaults.YoungHungerIncrease.ToString("N");
            fAdulthungerincrease.InnerText = FoxDefaults.AdultHungerIncrease.ToString("N");
            fOldhungerincrease.InnerText = FoxDefaults.OldHungerIncrease.ToString("N");
            fEatingspeed.InnerText = FoxDefaults.EatingSpeed.ToString("N");
            fDiet.InnerText = FoxDefaults.Diet.ToString("N");


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

            fox.AppendChild(fHungerdata);
            #endregion hungerData



            #region thirstData
            var fThirstdata = xmlDocument.CreateElement("thirstData");

            var fThirst = xmlDocument.CreateElement("thirst");
            var fThirstmax = xmlDocument.CreateElement("thirstMax");
            var fThirstythreshold = xmlDocument.CreateElement("thirstyThreshold");
            var fThirstincrease = xmlDocument.CreateElement("thirstIncrease");
            var fDrinkingspeed = xmlDocument.CreateElement("drinkingSpeed");


            fThirst.InnerText = FoxDefaults.Thirst.ToString("N");
            fThirstmax.InnerText = FoxDefaults.ThirstMax.ToString("N");
            fThirstythreshold.InnerText = FoxDefaults.ThirstyThreshold.ToString("N");
            fThirstincrease.InnerText = FoxDefaults.ThirstIncrease.ToString("N");
            fDrinkingspeed.InnerText = FoxDefaults.DrinkingSpeed.ToString("N");


            fThirstdata.AppendChild(fThirst);
            fThirstdata.AppendChild(fThirstmax);
            fThirstdata.AppendChild(fThirstythreshold);
            fThirstdata.AppendChild(fThirstincrease);
            fThirstdata.AppendChild(fDrinkingspeed);

            fox.AppendChild(fThirstdata);
            #endregion thirstData

            #region mateData
            var fMatedata = xmlDocument.CreateElement("mateData");

            var fMatestarttime = xmlDocument.CreateElement("mateStartTime");
            var fMatingduration = xmlDocument.CreateElement("matingDuration");
            var fReproductiveurge = xmlDocument.CreateElement("reproductiveUrge");
            var fReproductiveurgeincreasemale = xmlDocument.CreateElement("reproductiveUrgeIncreaseMale");
            var fReproductiveurgeincreasefemale = xmlDocument.CreateElement("reproductiveUrgeIncreaseFemale");
            var fMatingthreshold = xmlDocument.CreateElement("matingThreshold");


            fMatestarttime.InnerText = FoxDefaults.MateStartTime.ToString("N");
            fMatingduration.InnerText = FoxDefaults.MatingDuration.ToString("N");
            fReproductiveurge.InnerText = FoxDefaults.ReproductiveUrge.ToString("N");
            fReproductiveurgeincreasemale.InnerText = FoxDefaults.ReproductiveUrgeIncreaseMale.ToString("N");
            fReproductiveurgeincreasefemale.InnerText = FoxDefaults.ReproductiveUrgeIncreaseFemale.ToString("N");
            fMatingthreshold.InnerText = FoxDefaults.MatingThreshold.ToString("N");


            fMatedata.AppendChild(fMatestarttime);
            fMatedata.AppendChild(fMatingduration);
            fMatedata.AppendChild(fReproductiveurge);
            fMatedata.AppendChild(fReproductiveurgeincreasemale);
            fMatedata.AppendChild(fReproductiveurgeincreasefemale);
            fMatedata.AppendChild(fMatingthreshold);

            fox.AppendChild(fMatedata);
            #endregion mateData



            #region pregnancyData
            var fPregnancydata = xmlDocument.CreateElement("pregnancyData");

            var fPregnancystarttime = xmlDocument.CreateElement("pregnancyStartTime");
            var fBabiesborn = xmlDocument.CreateElement("babiesBorn");
            var fBirthstarttime = xmlDocument.CreateElement("birthStartTime");
            var fCurrentlittersize = xmlDocument.CreateElement("currentLitterSize");
            var fPregnancylengthmodifier = xmlDocument.CreateElement("pregnancyLengthModifier");
            var fPregnancylength = xmlDocument.CreateElement("pregnancyLength");
            var fBirthduration = xmlDocument.CreateElement("birthDuration");
            var fLittersizemin = xmlDocument.CreateElement("litterSizeMin");
            var fLittersizemax = xmlDocument.CreateElement("litterSizeMax");
            var fLittersizeave = xmlDocument.CreateElement("litterSizeAve");


            fPregnancystarttime.InnerText = FoxDefaults.PregnancyStartTime.ToString("N");
            fBabiesborn.InnerText = FoxDefaults.BabiesBorn.ToString("N");
            fBirthstarttime.InnerText = FoxDefaults.BirthStartTime.ToString("N");
            fCurrentlittersize.InnerText = FoxDefaults.CurrentLitterSize.ToString("N");
            fPregnancylengthmodifier.InnerText = FoxDefaults.PregnancyLengthModifier.ToString("N");
            fPregnancylength.InnerText = FoxDefaults.PregnancyLength.ToString("N");
            fBirthduration.InnerText = FoxDefaults.BirthDuration.ToString("N");
            fLittersizemin.InnerText = FoxDefaults.LitterSizeMin.ToString("N");
            fLittersizemax.InnerText = FoxDefaults.LitterSizeMax.ToString("N");
            fLittersizeave.InnerText = FoxDefaults.LitterSizeAve.ToString("N");

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

            fox.AppendChild(fPregnancydata);
            #endregion pregnancyData



            #region movementData
            var fMovementdata = xmlDocument.CreateElement("movementData");

            var fMovespeed = xmlDocument.CreateElement("moveSpeed");
            var fRotationspeed = xmlDocument.CreateElement("rotationSpeed");
            var fMovemultiplier = xmlDocument.CreateElement("moveMultiplier");
            var fPregnancymovemultiplier = xmlDocument.CreateElement("pregnancyMoveMultiplier");
            var fOriginalmovemultiplier = xmlDocument.CreateElement("originalMoveMultiplier");
            var fYoungmovemultiplier = xmlDocument.CreateElement("youngMoveMultiplier");
            var fAdultmovemultiplier = xmlDocument.CreateElement("adultMoveMultiplier");
            var fOldmovemultiplier = xmlDocument.CreateElement("oldMoveMultiplier");


            fMovespeed.InnerText = FoxDefaults.MoveSpeed.ToString("N");
            fRotationspeed.InnerText = FoxDefaults.RotationSpeed.ToString("N");
            fMovemultiplier.InnerText = FoxDefaults.MoveMultiplier.ToString("N");
            fPregnancymovemultiplier.InnerText = FoxDefaults.PregnancyMoveMultiplier.ToString("N");
            fOriginalmovemultiplier.InnerText = FoxDefaults.OriginalMoveMultiplier.ToString("N");
            fYoungmovemultiplier.InnerText = FoxDefaults.YoungMoveMultiplier.ToString("N");
            fAdultmovemultiplier.InnerText = FoxDefaults.AdultMoveMultiplier.ToString("N");
            fOldmovemultiplier.InnerText = FoxDefaults.OldMoveMultiplier.ToString("N");


            fMovementdata.AppendChild(fMovespeed);
            fMovementdata.AppendChild(fRotationspeed);
            fMovementdata.AppendChild(fMovemultiplier);
            fMovementdata.AppendChild(fPregnancymovemultiplier);
            fMovementdata.AppendChild(fOriginalmovemultiplier);
            fMovementdata.AppendChild(fYoungmovemultiplier);
            fMovementdata.AppendChild(fAdultmovemultiplier);
            fMovementdata.AppendChild(fOldmovemultiplier);

            fox.AppendChild(fMovementdata);
            #endregion movementData

            #region sizeData
            var fSizedata = xmlDocument.CreateElement("sizeData");

            var fSizemultiplier = xmlDocument.CreateElement("sizeMultiplier");
            var fScalemale = xmlDocument.CreateElement("scaleMale");
            var fScalefemale = xmlDocument.CreateElement("scaleFemale");
            var fYoungsizemultiplier = xmlDocument.CreateElement("youngSizeMultiplier");
            var fAdultsizemultiplier = xmlDocument.CreateElement("adultSizeMultiplier");
            var fOldsizemultiplier = xmlDocument.CreateElement("oldSizeMultiplier");


            fSizemultiplier.InnerText = FoxDefaults.SizeMultiplier.ToString("N");
            fScalemale.InnerText = FoxDefaults.ScaleMale.ToString("N");
            fScalefemale.InnerText = FoxDefaults.ScaleFemale.ToString("N");
            fYoungsizemultiplier.InnerText = FoxDefaults.YoungSizeMultiplier.ToString("N");
            fAdultsizemultiplier.InnerText = FoxDefaults.AdultSizeMultiplier.ToString("N");
            fOldsizemultiplier.InnerText = FoxDefaults.OldSizeMultiplier.ToString("N");


            fSizedata.AppendChild(fSizemultiplier);
            fSizedata.AppendChild(fScalemale);
            fSizedata.AppendChild(fScalefemale);
            fSizedata.AppendChild(fYoungsizemultiplier);
            fSizedata.AppendChild(fAdultsizemultiplier);
            fSizedata.AppendChild(fOldsizemultiplier);

            fox.AppendChild(fSizedata);
            #endregion sizeData


            #region stateData
            var fStatedata = xmlDocument.CreateElement("stateData");

            var fState = xmlDocument.CreateElement("flagState");
            var fPreviousstate = xmlDocument.CreateElement("FlagStatePrevious");
            var fDeathreason = xmlDocument.CreateElement("DeathReason");
            var fBeeneaten = xmlDocument.CreateElement("beenEaten");


            fState.InnerText = FoxDefaults.FlagState.ToString("N");
            fPreviousstate.InnerText = FoxDefaults.PreviousFlagState.ToString("N");
            fDeathreason.InnerText = FoxDefaults.DeathReason.ToString("N");
            fBeeneaten.InnerText = FoxDefaults.BeenEaten.ToString();


            fStatedata.AppendChild(fState);
            fStatedata.AppendChild(fPreviousstate);
            fStatedata.AppendChild(fDeathreason);
            fStatedata.AppendChild(fBeeneaten);

            fox.AppendChild(fStatedata);
            #endregion stateData


            #region targetData
            var fTargetdata = xmlDocument.CreateElement("targetData");

            var fTouchradius = xmlDocument.CreateElement("touchRadius");
            var fSightradius = xmlDocument.CreateElement("sightRadius");


            fTouchradius.InnerText = FoxDefaults.TouchRadius.ToString("N");
            fSightradius.InnerText = FoxDefaults.SightRadius.ToString("N");


            fTargetdata.AppendChild(fTouchradius);
            fTargetdata.AppendChild(fSightradius);

            fox.AppendChild(fTargetdata);
            #endregion targetData


            #region lookingEntityData
            var fLookingentitydata = xmlDocument.CreateElement("lookingEntityData");

            var fShortesttoedibledistance = xmlDocument.CreateElement("shortestToEdibleDistance");
            var fShortesttowaterdistance = xmlDocument.CreateElement("shortestToWaterDistance");
            var fShortesttopredatordistance = xmlDocument.CreateElement("shortestToPredatorDistance");
            var fShortesttomatedistance = xmlDocument.CreateElement("shortestToMateDistance");


            fShortesttoedibledistance.InnerText = FoxDefaults.ShortestToEdibleDistance.ToString("N");
            fShortesttowaterdistance.InnerText = FoxDefaults.ShortestToWaterDistance.ToString("N");
            fShortesttopredatordistance.InnerText = FoxDefaults.ShortestToPredatorDistance.ToString("N");
            fShortesttomatedistance.InnerText = FoxDefaults.ShortestToMateDistance.ToString("N");


            fLookingentitydata.AppendChild(fShortesttoedibledistance);
            fLookingentitydata.AppendChild(fShortesttowaterdistance);
            fLookingentitydata.AppendChild(fShortesttopredatordistance);
            fLookingentitydata.AppendChild(fShortesttomatedistance);

            fox.AppendChild(fLookingentitydata);
            #endregion lookingEntityData


            #region ColliderTypeData
            var fCollidertypedata = xmlDocument.CreateElement("ColliderTypeData");

            var fCollidertype = xmlDocument.CreateElement("Collider");


            fCollidertype.InnerText = FoxDefaults.Collider.ToString("N");


            fCollidertypedata.AppendChild(fCollidertype);

            fox.AppendChild(fCollidertypedata);
            #endregion ColliderTypeData


            root.AppendChild(fox);
            #endregion Fox default


            #region Grass default

            var grass = xmlDocument.CreateElement("Grass");

            #region edibleData
            var gEdibledata = xmlDocument.CreateElement("edibleData");

            var gNutritionalvalue = xmlDocument.CreateElement("nutritionalValue");
            var gCanbeeaten = xmlDocument.CreateElement("canBeEaten");
            var gNutritionalvaluemultiplier = xmlDocument.CreateElement("nutritionalValueMultiplier");
            var gFoodtype = xmlDocument.CreateElement("foodType");


            gNutritionalvalue.InnerText = GrassDefaults.NutritionalValue.ToString("N");
            gCanbeeaten.InnerText = GrassDefaults.CanBeEaten.ToString();
            gNutritionalvaluemultiplier.InnerText = GrassDefaults.NutritionalValueMultiplier.ToString("N");
            gFoodtype.InnerText = GrassDefaults.FoodType.ToString("N");


            gEdibledata.AppendChild(gNutritionalvalue);
            gEdibledata.AppendChild(gCanbeeaten);
            gEdibledata.AppendChild(gNutritionalvaluemultiplier);
            gEdibledata.AppendChild(gFoodtype);

            grass.AppendChild(gEdibledata);
            #endregion edibleData


            #region sizeData
            var gSizedata = xmlDocument.CreateElement("sizeData");

            var gSizemultiplier = xmlDocument.CreateElement("sizeMultiplier");
            var gScale = xmlDocument.CreateElement("scale");


            gSizemultiplier.InnerText = GrassDefaults.SizeMultiplier.ToString("N");
            gScale.InnerText = GrassDefaults.Scale.ToString("N");


            gSizedata.AppendChild(gSizemultiplier);
            gSizedata.AppendChild(gScale);

            grass.AppendChild(gSizedata);
            #endregion sizeData



            #region stateData
            var gStatedata = xmlDocument.CreateElement("stateData");

            var gState = xmlDocument.CreateElement("flagState");
            var gPreviousstate = xmlDocument.CreateElement("FlagStatePrevious");
            var gDeathreason = xmlDocument.CreateElement("DeathReason");
            var gBeeneaten = xmlDocument.CreateElement("beenEaten");


            gState.InnerText = GrassDefaults.FlagState.ToString("N");
            gPreviousstate.InnerText = GrassDefaults.PreviousFlagState.ToString("N");
            gDeathreason.InnerText = GrassDefaults.DeathReason.ToString("N");
            gBeeneaten.InnerText = GrassDefaults.BeenEaten.ToString();


            gStatedata.AppendChild(gState);
            gStatedata.AppendChild(gPreviousstate);
            gStatedata.AppendChild(gDeathreason);
            gStatedata.AppendChild(gBeeneaten);

            grass.AppendChild(gStatedata);
            #endregion stateData

            #region ColliderTypeData
            var gCollidertypedata = xmlDocument.CreateElement("ColliderTypeData");

            var gCollidertype = xmlDocument.CreateElement("Collider");


            gCollidertype.InnerText = GrassDefaults.Collider.ToString("N");


            gCollidertypedata.AppendChild(gCollidertype);

            grass.AppendChild(gCollidertypedata);
            #endregion ColliderTypeData


            root.AppendChild(grass);

            #endregion Grass default

            xmlDocument.AppendChild(root);


            xmlDocument.Save(path);
        }
    }
}
