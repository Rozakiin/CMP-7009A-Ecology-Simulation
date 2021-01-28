using Components;
using EntityDefaults;
using Unity.Entities;

namespace Systems
{
    public class UIUpdateSystem : SystemBase
    {
        public bool SomethingChangedFlag;
        public static UIUpdateSystem Instance; // public reference to self (singleton)


        protected override void OnCreate()
        {
            base.OnCreate();
            Instance = this;
            SomethingChangedFlag = false;
        }

        /*
         * When somethingChangedFlag is true,
         * the entities' properties are updated from Defaults.
         * This is used when the defaults are updated when entities could exist
         * in the sim.
         */
        protected override void OnUpdate()
        {
            //only update entities if something has changed
            if (SomethingChangedFlag)
            {
                // updates each component individually that has default values associated with it

                /*RABBIT*/

                //Update BasicNeedsData
                var RabbitthirstThreshold = RabbitDefaults.ThirstyThreshold;
                var RabbitthirstMax = RabbitDefaults.ThirstMax;
                var RabbitthirstIncrease = RabbitDefaults.ThirstIncrease;
                var RabbitdrinkingSpeed = RabbitDefaults.DrinkingSpeed;

                var RabbithungerThreshold = RabbitDefaults.HungryThreshold;
                var RabbithungerMax = RabbitDefaults.HungerMax;
                var RabbithunderIncrease = RabbitDefaults.HungerIncrease;
                var RabbitpregnancyHungerIncrease = RabbitDefaults.PregnancyHungerIncrease;
                var RabbityoungHungerIncrease = RabbitDefaults.YoungHungerIncrease;
                var RabbitadultHungerIncrease = RabbitDefaults.AdultHungerIncrease;
                var RabbitoldHungerIncrease = RabbitDefaults.OldHungerIncrease;
                var RabbiteatingSpeed = RabbitDefaults.EatingSpeed;
                Entities.WithAll<IsRabbitTag>().ForEach((ref BasicNeedsData basicNeedsData) =>
                {
                    basicNeedsData.ThirstyThreshold = RabbitthirstThreshold;
                    basicNeedsData.ThirstMax = RabbitthirstMax;
                    basicNeedsData.ThirstIncrease = RabbitthirstIncrease;
                    basicNeedsData.DrinkingSpeed = RabbitdrinkingSpeed;

                    basicNeedsData.HungryThreshold = RabbithungerThreshold;
                    basicNeedsData.HungerMax = RabbithungerMax;
                    basicNeedsData.HungerIncrease = RabbithunderIncrease;
                    basicNeedsData.PregnancyHungerIncrease = RabbitpregnancyHungerIncrease;
                    basicNeedsData.YoungHungerIncrease = RabbityoungHungerIncrease;
                    basicNeedsData.AdultHungerIncrease = RabbitadultHungerIncrease;
                    basicNeedsData.OldHungerIncrease = RabbitoldHungerIncrease;
                    basicNeedsData.EatingSpeed = RabbiteatingSpeed;
                }).ScheduleParallel();

                //Update BioStatsData
                var RabbitageIncrease = RabbitDefaults.AgeIncrease;
                var RabbitageMax = RabbitDefaults.AgeMax;
                Entities.WithAll<IsRabbitTag>().ForEach((ref BioStatsData bioStatsData) =>
                {
                    bioStatsData.AgeIncrease = RabbitageIncrease;
                    bioStatsData.AgeMax = RabbitageMax;
                }).ScheduleParallel();

                //Update EdibleData
                var RabbitnutritionalValueBase = RabbitDefaults.NutritionalValue;
                var RabbitcanBeEaten = RabbitDefaults.CanBeEaten;
                Entities.WithAll<IsRabbitTag>().ForEach((ref EdibleData edibleData) =>
                {
                    edibleData.CanBeEaten = RabbitcanBeEaten;
                    edibleData.NutritionalValueBase = RabbitnutritionalValueBase;
                }).ScheduleParallel();

                //Update MovementData
                var RabbitmoveSpeed = RabbitDefaults.MoveSpeed;
                var RabbitpregnancyMoveMultiplier = RabbitDefaults.PregnancyMoveMultiplier;
                var RabbityoungMoveMultiplier = RabbitDefaults.YoungMoveMultiplier;
                var RabbitadultMoveMultiplier = RabbitDefaults.AdultMoveMultiplier;
                var RabbitoldMoveMultiplier = RabbitDefaults.OldMoveMultiplier;
                Entities.WithAll<IsRabbitTag>().ForEach((ref MovementData movementData) =>
                {
                    movementData.MoveSpeedBase = RabbitmoveSpeed;
                    movementData.PregnancyMoveMultiplier = RabbitpregnancyMoveMultiplier;
                    movementData.YoungMoveMultiplier = RabbityoungMoveMultiplier;
                    movementData.AdultMoveMultiplier = RabbitadultMoveMultiplier;
                    movementData.OldMoveMultiplier = RabbitoldMoveMultiplier;
                }).ScheduleParallel();

                //Update ReproductiveData
                var RabbitmatingDuration = RabbitDefaults.MatingDuration;
                var RabbitmatingThreshold = RabbitDefaults.MatingThreshold;
                var RabbitreproductiveUrgeIncrease = RabbitDefaults.ReproductiveUrgeIncreaseMale;
                var RabbitpregnancyLength = RabbitDefaults.PregnancyLength;
                var RabbitbirthDuration = RabbitDefaults.BirthDuration;
                var RabbitlitterSizeMin = RabbitDefaults.LitterSizeMin;
                var RabbitlitterSizeMax = RabbitDefaults.LitterSizeMax;
                var RabbitlitterSizeAve = RabbitDefaults.LitterSizeAve;
                Entities.WithAll<IsRabbitTag>().ForEach((ref ReproductiveData reproductiveData) =>
                {
                    reproductiveData.MatingDuration = RabbitmatingDuration;
                    reproductiveData.MatingThreshold = RabbitmatingThreshold;
                    reproductiveData.ReproductiveUrgeIncrease = RabbitreproductiveUrgeIncrease;
                    reproductiveData.DefaultReproductiveIncrease = RabbitreproductiveUrgeIncrease;
                    reproductiveData.PregnancyLengthBase = RabbitpregnancyLength;
                    reproductiveData.BirthDuration = RabbitbirthDuration;
                    reproductiveData.LitterSizeMin = RabbitlitterSizeMin;
                    reproductiveData.LitterSizeMax = RabbitlitterSizeMax;
                    reproductiveData.LitterSizeAve = RabbitlitterSizeAve;
                }).ScheduleParallel();

                //Update SizeData
                //Update Scale dependent on gender
                var RabbitmaleSize = RabbitDefaults.ScaleMale;
                var RabbitfemaleSize = RabbitDefaults.ScaleFemale;
                var RabbityoungSizeMultiplier = RabbitDefaults.YoungSizeMultiplier;
                var RabbitadultSizeMultiplier = RabbitDefaults.AdultSizeMultiplier;
                var RabbitoldSizeMultiplier = RabbitDefaults.OldSizeMultiplier;
                Entities.WithAll<IsRabbitTag>().ForEach((ref SizeData sizeData, in BioStatsData bioStatsData) =>
                {
                    if (bioStatsData.Gender == BioStatsData.Genders.Male)
                        sizeData.SizeBase = RabbitmaleSize;
                    else if (bioStatsData.Gender == BioStatsData.Genders.Female)
                        sizeData.SizeBase = RabbitfemaleSize;
                    sizeData.YoungSizeMultiplier = RabbityoungSizeMultiplier;
                    sizeData.AdultSizeMultiplier = RabbitadultSizeMultiplier;
                    sizeData.OldSizeMultiplier = RabbitoldSizeMultiplier;
                }).ScheduleParallel();

                //Update TargetData
                var RabbitsightRadius = RabbitDefaults.SightRadius;
                Entities.WithAll<IsRabbitTag>().ForEach((ref TargetData targetData) =>
                {
                    targetData.SightRadius = RabbitsightRadius;
                }).ScheduleParallel();



                /*FOX*/

                //Update BasicNeedsData
                var FthirstThreshold = FoxDefaults.ThirstyThreshold;
                var FthirstMax = FoxDefaults.ThirstMax;
                var FthirstIncrease = FoxDefaults.ThirstIncrease;
                var FdrinkingSpeed = FoxDefaults.DrinkingSpeed;

                var FhungerThreshold = FoxDefaults.HungryThreshold;
                var FhungerMax = FoxDefaults.HungerMax;
                var FhunderIncrease = FoxDefaults.HungerIncrease;
                var FpregnancyHungerIncrease = FoxDefaults.PregnancyHungerIncrease;
                var FyoungHungerIncrease = FoxDefaults.YoungHungerIncrease;
                var FadultHungerIncrease = FoxDefaults.AdultHungerIncrease;
                var FoldHungerIncrease = FoxDefaults.OldHungerIncrease;
                var FeatingSpeed = FoxDefaults.EatingSpeed;
                Entities.WithAll<IsFoxTag>().ForEach((ref BasicNeedsData basicNeedsData) =>
                {
                    basicNeedsData.ThirstyThreshold = FthirstThreshold;
                    basicNeedsData.ThirstMax = FthirstMax;
                    basicNeedsData.ThirstIncrease = FthirstIncrease;
                    basicNeedsData.DrinkingSpeed = FdrinkingSpeed;

                    basicNeedsData.HungryThreshold = FhungerThreshold;
                    basicNeedsData.HungerMax = FhungerMax;
                    basicNeedsData.HungerIncrease = FhunderIncrease;
                    basicNeedsData.PregnancyHungerIncrease = FpregnancyHungerIncrease;
                    basicNeedsData.YoungHungerIncrease = FyoungHungerIncrease;
                    basicNeedsData.AdultHungerIncrease = FadultHungerIncrease;
                    basicNeedsData.OldHungerIncrease = FoldHungerIncrease;
                    basicNeedsData.EatingSpeed = FeatingSpeed;
                }).ScheduleParallel();

                //Update BioStatsData
                var FageIncrease = FoxDefaults.AgeIncrease;
                var FageMax = FoxDefaults.AgeMax;
                Entities.WithAll<IsFoxTag>().ForEach((ref BioStatsData bioStatsData) =>
                {
                    bioStatsData.AgeIncrease = FageIncrease;
                    bioStatsData.AgeMax = FageMax;
                }).ScheduleParallel();

                //Update EdibleData
                var FnutritionalValueBase = FoxDefaults.NutritionalValue;
                var FcanBeEaten = FoxDefaults.CanBeEaten;
                Entities.WithAll<IsFoxTag>().ForEach((ref EdibleData edibleData) =>
                {
                    edibleData.CanBeEaten = FcanBeEaten;
                    edibleData.NutritionalValueBase = FnutritionalValueBase;
                }).ScheduleParallel();

                //Update MovementData
                var FmoveSpeed = FoxDefaults.MoveSpeed;
                var FpregnancyMoveMultiplier = FoxDefaults.PregnancyMoveMultiplier;
                var FyoungMoveMultiplier = FoxDefaults.YoungMoveMultiplier;
                var FadultMoveMultiplier = FoxDefaults.AdultMoveMultiplier;
                var FoldMoveMultiplier = FoxDefaults.OldMoveMultiplier;
                Entities.WithAll<IsFoxTag>().ForEach((ref MovementData movementData) =>
                {
                    movementData.MoveSpeedBase = FmoveSpeed;
                    movementData.PregnancyMoveMultiplier = FpregnancyMoveMultiplier;
                    movementData.YoungMoveMultiplier = FyoungMoveMultiplier;
                    movementData.AdultMoveMultiplier = FadultMoveMultiplier;
                    movementData.OldMoveMultiplier = FoldMoveMultiplier;
                }).ScheduleParallel();

                //Update ReproductiveData
                var FmatingDuration = FoxDefaults.MatingDuration;
                var FmatingThreshold = FoxDefaults.MatingThreshold;
                var FreproductiveUrgeIncrease = FoxDefaults.ReproductiveUrgeIncreaseMale;
                var FpregnancyLength = FoxDefaults.PregnancyLength;
                var FbirthDuration = FoxDefaults.BirthDuration;
                var FlitterSizeMin = FoxDefaults.LitterSizeMin;
                var FlitterSizeMax = FoxDefaults.LitterSizeMax;
                var FlitterSizeAve = FoxDefaults.LitterSizeAve;
                Entities.WithAll<IsFoxTag>().ForEach((ref ReproductiveData reproductiveData) =>
                {
                    reproductiveData.MatingDuration = FmatingDuration;
                    reproductiveData.MatingThreshold = FmatingThreshold;
                    reproductiveData.ReproductiveUrgeIncrease = FreproductiveUrgeIncrease;
                    reproductiveData.DefaultReproductiveIncrease = FreproductiveUrgeIncrease;
                    reproductiveData.PregnancyLengthBase = FpregnancyLength;
                    reproductiveData.BirthDuration = FbirthDuration;
                    reproductiveData.LitterSizeMin = FlitterSizeMin;
                    reproductiveData.LitterSizeMax = FlitterSizeMax;
                    reproductiveData.LitterSizeAve = FlitterSizeAve;
                }).ScheduleParallel();

                //Update SizeData
                //Update Scale dependent on gender
                var FmaleSize = FoxDefaults.ScaleMale;
                var FfemaleSize = FoxDefaults.ScaleFemale;
                var FyoungSizeMultiplier = FoxDefaults.YoungSizeMultiplier;
                var FadultSizeMultiplier = FoxDefaults.AdultSizeMultiplier;
                var FoldSizeMultiplier = FoxDefaults.OldSizeMultiplier;
                Entities.WithAll<IsFoxTag>().ForEach((ref SizeData sizeData, in BioStatsData bioStatsData) =>
                {
                    if (bioStatsData.Gender == BioStatsData.Genders.Male)
                        sizeData.SizeBase = FmaleSize;
                    else if (bioStatsData.Gender == BioStatsData.Genders.Female)
                        sizeData.SizeBase = FfemaleSize;
                    sizeData.YoungSizeMultiplier = FyoungSizeMultiplier;
                    sizeData.AdultSizeMultiplier = FadultSizeMultiplier;
                    sizeData.OldSizeMultiplier = FoldSizeMultiplier;
                }).ScheduleParallel();

                //Update TargetData
                var FsightRadius = FoxDefaults.SightRadius;
                Entities.WithAll<IsFoxTag>().ForEach((ref TargetData targetData) =>
                {
                    targetData.SightRadius = FsightRadius;
                }).ScheduleParallel();



                /*GRASS*/

                //Update EdibleData
                var GrassnutritionalValueBase = GrassDefaults.NutritionalValue;
                var GrasscanBeEaten = GrassDefaults.CanBeEaten;
                Entities.WithAll<IsGrassTag>().ForEach((ref EdibleData edibleData) =>
                {
                    edibleData.CanBeEaten = GrasscanBeEaten;
                    edibleData.NutritionalValueBase = GrassnutritionalValueBase;
                }).ScheduleParallel();

                //Update SizeData
                var GrassSize = GrassDefaults.Scale;
                Entities.WithAll<IsGrassTag>().ForEach((ref SizeData sizeData) =>
                {
                    sizeData.SizeBase = GrassSize;
                }).ScheduleParallel();


                SomethingChangedFlag = false;
            }
        }
    }
}
