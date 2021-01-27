using Components;
using EntityDefaults;
using Unity.Entities;
using UnityEngine;
using Hash128 = UnityEngine.Hash128;

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
                float RthirstThreshold = RabbitDefaults.ThirstyThreshold;
                float RthirstMax = RabbitDefaults.ThirstMax;
                float RthirstIncrease = RabbitDefaults.ThirstIncrease;
                float RdrinkingSpeed = RabbitDefaults.DrinkingSpeed;

                float RhungerThreshold = RabbitDefaults.HungryThreshold;
                float RhungerMax = RabbitDefaults.HungerMax;
                float RhunderIncrease = RabbitDefaults.HungerIncrease;
                float RpregnancyHungerIncrease = RabbitDefaults.PregnancyHungerIncrease;
                float RyoungHungerIncrease = RabbitDefaults.YoungHungerIncrease;
                float RadultHungerIncrease = RabbitDefaults.AdultHungerIncrease;
                float RoldHungerIncrease = RabbitDefaults.OldHungerIncrease;
                float ReatingSpeed = RabbitDefaults.EatingSpeed;
                Entities.WithAll<IsRabbitTag>().ForEach((ref BasicNeedsData basicNeedsData) =>
                {
                    basicNeedsData.ThirstyThreshold = RthirstThreshold;
                    basicNeedsData.ThirstMax = RthirstMax;
                    basicNeedsData.ThirstIncrease = RthirstIncrease;
                    basicNeedsData.DrinkingSpeed = RdrinkingSpeed;

                    basicNeedsData.HungryThreshold = RhungerThreshold;
                    basicNeedsData.HungerMax = RhungerMax;
                    basicNeedsData.HungerIncrease = RhunderIncrease;
                    basicNeedsData.PregnancyHungerIncrease = RpregnancyHungerIncrease;
                    basicNeedsData.YoungHungerIncrease = RyoungHungerIncrease;
                    basicNeedsData.AdultHungerIncrease = RadultHungerIncrease;
                    basicNeedsData.OldHungerIncrease = RoldHungerIncrease;
                    basicNeedsData.EatingSpeed = ReatingSpeed;
                }).ScheduleParallel();

                //Update BioStatsData
                float RageIncrease = RabbitDefaults.AgeIncrease;
                float RageMax = RabbitDefaults.AgeMax;
                Entities.WithAll<IsRabbitTag>().ForEach((ref BioStatsData bioStatsData) =>
                {
                    bioStatsData.AgeIncrease = RageIncrease;
                    bioStatsData.AgeMax = RageMax;
                }).ScheduleParallel();

                //Update EdibleData
                float RnutritionalValueBase = RabbitDefaults.NutritionalValue;
                bool RcanBeEaten = RabbitDefaults.CanBeEaten;
                Entities.WithAll<IsRabbitTag>().ForEach((ref EdibleData edibleData) =>
                {
                    edibleData.CanBeEaten = RcanBeEaten;
                    edibleData.NutritionalValueBase = RnutritionalValueBase;
                }).ScheduleParallel();

                //Update MovementData
                float RmoveSpeed = RabbitDefaults.MoveSpeed;
                float RpregnancyMoveMultiplier = RabbitDefaults.PregnancyMoveMultiplier;
                float RyoungMoveMultiplier = RabbitDefaults.YoungMoveMultiplier;
                float RadultMoveMultiplier = RabbitDefaults.AdultMoveMultiplier;
                float RoldMoveMultiplier = RabbitDefaults.OldMoveMultiplier;
                Entities.WithAll<IsRabbitTag>().ForEach((ref MovementData movementData) =>
                {
                    movementData.MoveSpeedBase = RmoveSpeed;
                    movementData.PregnancyMoveMultiplier = RpregnancyMoveMultiplier;
                    movementData.YoungMoveMultiplier = RyoungMoveMultiplier;
                    movementData.AdultMoveMultiplier = RadultMoveMultiplier;
                    movementData.OldMoveMultiplier = RadultMoveMultiplier;
                }).ScheduleParallel();

                //Update ReproductiveData
                float RmatingDuration = RabbitDefaults.MatingDuration;
                float RmatingThreshold = RabbitDefaults.MatingThreshold;
                float RreproductiveUrgeIncrease = RabbitDefaults.ReproductiveUrgeIncreaseMale;
                float RpregnancyLength = RabbitDefaults.PregnancyLength;
                float RbirthDuration = RabbitDefaults.BirthDuration;
                int RlitterSizeMin = RabbitDefaults.LitterSizeMin;
                int RlitterSizeMax = RabbitDefaults.LitterSizeMax;
                int RlitterSizeAve = RabbitDefaults.LitterSizeAve;
                Entities.WithAll<IsRabbitTag>().ForEach((ref ReproductiveData reproductiveData) =>
                {
                    reproductiveData.MatingDuration = RmatingDuration;
                    reproductiveData.MatingThreshold = RmatingThreshold;
                    reproductiveData.ReproductiveUrgeIncrease = RreproductiveUrgeIncrease;
                    reproductiveData.DefaultReproductiveIncrease = RreproductiveUrgeIncrease;
                    reproductiveData.PregnancyLengthBase = RpregnancyLength;
                    reproductiveData.BirthDuration = RbirthDuration;
                    reproductiveData.LitterSizeMin = RlitterSizeMin;
                    reproductiveData.LitterSizeMax = RlitterSizeMax;
                    reproductiveData.LitterSizeAve = RlitterSizeAve;
                }).ScheduleParallel();

                //Update SizeData
                //Update Scale dependent on gender
                float RmaleSize = RabbitDefaults.ScaleMale;
                float RfemaleSize = RabbitDefaults.ScaleFemale;
                float RyoungSizeMultiplier = RabbitDefaults.YoungSizeMultiplier;
                float RadultSizeMultiplier = RabbitDefaults.AdultSizeMultiplier;
                float RoldSizeMultiplier = RabbitDefaults.OldSizeMultiplier;
                Entities.WithAll<IsRabbitTag>().ForEach((ref SizeData sizeData, in BioStatsData bioStatsData) =>
                {
                    if (bioStatsData.Gender == BioStatsData.Genders.Male)
                        sizeData.SizeBase = RmaleSize;
                    else if (bioStatsData.Gender == BioStatsData.Genders.Female)
                        sizeData.SizeBase = RfemaleSize;
                    sizeData.YoungSizeMultiplier = RyoungSizeMultiplier;
                    sizeData.AdultSizeMultiplier = RadultSizeMultiplier;
                    sizeData.OldSizeMultiplier = RoldSizeMultiplier;
                }).ScheduleParallel();

                //Update TargetData
                float RsightRadius = RabbitDefaults.SightRadius;
                Entities.WithAll<IsRabbitTag>().ForEach((ref TargetData targetData) =>
                {
                    targetData.SightRadius = RsightRadius;
                }).ScheduleParallel();



                /*FOX*/

                //Update BasicNeedsData
                float FthirstThreshold = FoxDefaults.ThirstyThreshold;
                float FthirstMax = FoxDefaults.ThirstMax;
                float FthirstIncrease = FoxDefaults.ThirstIncrease;
                float FdrinkingSpeed = FoxDefaults.DrinkingSpeed;

                float FhungerThreshold = FoxDefaults.HungryThreshold;
                float FhungerMax = FoxDefaults.HungerMax;
                float FhunderIncrease = FoxDefaults.HungerIncrease;
                float FpregnancyHungerIncrease = FoxDefaults.PregnancyHungerIncrease;
                float FyoungHungerIncrease = FoxDefaults.YoungHungerIncrease;
                float FadultHungerIncrease = FoxDefaults.AdultHungerIncrease;
                float FoldHungerIncrease = FoxDefaults.OldHungerIncrease;
                float FeatingSpeed = FoxDefaults.EatingSpeed;
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
                float FageIncrease = FoxDefaults.AgeIncrease;
                float FageMax = FoxDefaults.AgeMax;
                Entities.WithAll<IsFoxTag>().ForEach((ref BioStatsData bioStatsData) =>
                {
                    bioStatsData.AgeIncrease = FageIncrease;
                    bioStatsData.AgeMax = FageMax;
                }).ScheduleParallel();

                //Update EdibleData
                float FnutritionalValueBase = FoxDefaults.NutritionalValue;
                bool FcanBeEaten = FoxDefaults.CanBeEaten;
                Entities.WithAll<IsFoxTag>().ForEach((ref EdibleData edibleData) =>
                {
                    edibleData.CanBeEaten = FcanBeEaten;
                    edibleData.NutritionalValueBase = FnutritionalValueBase;
                }).ScheduleParallel();

                //Update MovementData
                float FmoveSpeed = FoxDefaults.MoveSpeed;
                float FpregnancyMoveMultiplier = FoxDefaults.PregnancyMoveMultiplier;
                float FyoungMoveMultiplier = FoxDefaults.YoungMoveMultiplier;
                float FadultMoveMultiplier = FoxDefaults.AdultMoveMultiplier;
                float FoldMoveMultiplier = FoxDefaults.OldMoveMultiplier;
                Entities.WithAll<IsFoxTag>().ForEach((ref MovementData movementData) =>
                {
                    movementData.MoveSpeedBase = FmoveSpeed;
                    movementData.PregnancyMoveMultiplier = FpregnancyMoveMultiplier;
                    movementData.YoungMoveMultiplier = FyoungMoveMultiplier;
                    movementData.AdultMoveMultiplier = FadultMoveMultiplier;
                    movementData.OldMoveMultiplier = FadultMoveMultiplier;
                }).ScheduleParallel();

                //Update ReproductiveData
                float FmatingDuration = FoxDefaults.MatingDuration;
                float FmatingThreshold = FoxDefaults.MatingThreshold;
                float FreproductiveUrgeIncrease = FoxDefaults.ReproductiveUrgeIncreaseMale;
                float FpregnancyLength = FoxDefaults.PregnancyLength;
                float FbirthDuration = FoxDefaults.BirthDuration;
                int FlitterSizeMin = FoxDefaults.LitterSizeMin;
                int FlitterSizeMax = FoxDefaults.LitterSizeMax;
                int FlitterSizeAve = FoxDefaults.LitterSizeAve;
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
                float FmaleSize = FoxDefaults.ScaleMale;
                float FfemaleSize = FoxDefaults.ScaleFemale;
                float FyoungSizeMultiplier = FoxDefaults.YoungSizeMultiplier;
                float FadultSizeMultiplier = FoxDefaults.AdultSizeMultiplier;
                float FoldSizeMultiplier = FoxDefaults.OldSizeMultiplier;
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
                float FsightRadius = FoxDefaults.SightRadius;
                Entities.WithAll<IsFoxTag>().ForEach((ref TargetData targetData) =>
                {
                    targetData.SightRadius = FsightRadius;
                }).ScheduleParallel();



                /*GRASS*/

                //Update EdibleData
                float GrassnutritionalValueBase = GrassDefaults.NutritionalValue;
                bool GrasscanBeEaten = GrassDefaults.CanBeEaten;
                Entities.WithAll<IsGrassTag>().ForEach((ref EdibleData edibleData) =>
                {
                    edibleData.CanBeEaten = GrasscanBeEaten;
                    edibleData.NutritionalValueBase = GrassnutritionalValueBase;
                }).ScheduleParallel();

                //Update SizeData
                float GrassSize = GrassDefaults.Scale;
                Entities.WithAll<IsGrassTag>().ForEach((ref SizeData sizeData) =>
                {
                    sizeData.SizeBase = GrassSize;
                }).ScheduleParallel();


                SomethingChangedFlag = false;
            }
        }
    }
}
