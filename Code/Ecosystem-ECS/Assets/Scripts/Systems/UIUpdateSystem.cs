using Unity.Entities;
using Unity.Jobs;

public class UIUpdateSystem : SystemBase
{
    public bool somethingChangedFlag;
    public static UIUpdateSystem Instance; // public reference to self (singleton)


    protected override void OnCreate()
    {
        base.OnCreate();
        Instance = this;
        somethingChangedFlag = false;
    }

    protected override void OnUpdate()
    {
        //only update entities if something has changed
        if (somethingChangedFlag)
        {
            // updates each component individually that has default values associated with it

            /*RABBIT*/

            //Update BasicNeedsData
            float RthirstThreshold = RabbitDefaults.thirstyThreshold;
            float RthirstMax = RabbitDefaults.thirstMax;
            float RthirstIncrease = RabbitDefaults.thirstIncrease;
            float RdrinkingSpeed = RabbitDefaults.drinkingSpeed;

            float RhungerThreshold = RabbitDefaults.hungryThreshold;
            float RhungerMax = RabbitDefaults.hungerMax;
            float RhunderIncrease = RabbitDefaults.hungerIncrease;
            float RpregnancyHungerIncrease = RabbitDefaults.pregnancyHungerIncrease;
            float RyoungHungerIncrease = RabbitDefaults.youngHungerIncrease;
            float RadultHungerIncrease = RabbitDefaults.adultHungerIncrease;
            float RoldHungerIncrease = RabbitDefaults.oldHungerIncrease;
            float ReatingSpeed = RabbitDefaults.eatingSpeed;
            Entities.WithAll<isRabbitTag>().ForEach((ref BasicNeedsData basicNeedsData) =>
            {
                basicNeedsData.thirstyThreshold = RthirstThreshold;
                basicNeedsData.thirstMax = RthirstMax;
                basicNeedsData.thirstIncrease = RthirstIncrease;
                basicNeedsData.drinkingSpeed = RdrinkingSpeed;

                basicNeedsData.hungryThreshold = RhungerThreshold;
                basicNeedsData.hungerMax = RhungerMax;
                basicNeedsData.hungerIncrease = RhunderIncrease;
                basicNeedsData.pregnancyHungerIncrease = RpregnancyHungerIncrease;
                basicNeedsData.youngHungerIncrease = RyoungHungerIncrease;
                basicNeedsData.adultHungerIncrease = RadultHungerIncrease;
                basicNeedsData.oldHungerIncrease = RoldHungerIncrease;
                basicNeedsData.eatingSpeed = ReatingSpeed;
            }).ScheduleParallel();

            //Update BioStatsData
            float RageIncrease = RabbitDefaults.ageIncrease;
            float RageMax = RabbitDefaults.ageMax;
            Entities.WithAll<isRabbitTag>().ForEach((ref BioStatsData bioStatsData) =>
            {
                bioStatsData.ageIncrease = RageIncrease;
                bioStatsData.ageMax = RageMax;
            }).ScheduleParallel();

            //Update EdibleData
            float RnutritionalValueBase = RabbitDefaults.nutritionalValue;
            bool RcanBeEaten = RabbitDefaults.canBeEaten;
            Entities.WithAll<isRabbitTag>().ForEach((ref EdibleData edibleData) =>
            {
                edibleData.canBeEaten = RcanBeEaten;
                edibleData.nutritionalValueBase = RnutritionalValueBase;
            }).ScheduleParallel();

            //Update MovementData
            float RmoveSpeed = RabbitDefaults.moveSpeed;
            float RpregnancyMoveMultiplier = RabbitDefaults.pregnancyMoveMultiplier;
            float RyoungMoveMultiplier = RabbitDefaults.youngMoveMultiplier;
            float RadultMoveMultiplier = RabbitDefaults.adultMoveMultiplier;
            float RoldMoveMultiplier = RabbitDefaults.oldMoveMultiplier;
            Entities.WithAll<isRabbitTag>().ForEach((ref MovementData movementData) =>
            {
                movementData.moveSpeedBase = RmoveSpeed;
                movementData.pregnancyMoveMultiplier = RpregnancyMoveMultiplier;
                movementData.youngMoveMultiplier = RyoungMoveMultiplier;
                movementData.adultMoveMultiplier = RadultMoveMultiplier;
                movementData.oldMoveMultiplier = RadultMoveMultiplier;
            }).ScheduleParallel();

            //Update ReproductiveData
            float RmatingDuration = RabbitDefaults.matingDuration;
            float RpregnancyLength = RabbitDefaults.pregnancyLength;
            float RbirthDuration = RabbitDefaults.birthDuration;
            int RlitterSizeMin = RabbitDefaults.litterSizeMin;
            int RlitterSizeMax = RabbitDefaults.litterSizeMax;
            int RlitterSizeAve = RabbitDefaults.litterSizeAve;
            Entities.WithAll<isRabbitTag>().ForEach((ref ReproductiveData reproductiveData) =>
            {
                reproductiveData.matingDuration = RmatingDuration;
                reproductiveData.pregnancyLengthBase = RpregnancyLength;
                reproductiveData.birthDuration = RbirthDuration;
                reproductiveData.litterSizeMin = RlitterSizeMin;
                reproductiveData.litterSizeMax = RlitterSizeMax;
                reproductiveData.litterSizeAve = RlitterSizeAve;
            }).ScheduleParallel();

            //Update SizeData
            //Update Scale dependent on gender
            float RmaleSize = RabbitDefaults.scaleMale;
            float RfemaleSize = RabbitDefaults.scaleFemale;
            float RyoungSizeMultiplier = RabbitDefaults.youngSizeMultiplier;
            float RadultSizeMultiplier = RabbitDefaults.adultSizeMultiplier;
            float RoldSizeMultiplier = RabbitDefaults.oldSizeMultiplier;
            Entities.WithAll<isRabbitTag>().ForEach((ref SizeData sizeData, in BioStatsData bioStatsData) =>
            {
                if (bioStatsData.gender == BioStatsData.Gender.Male)
                    sizeData.size = RmaleSize;
                else if (bioStatsData.gender == BioStatsData.Gender.Female)
                    sizeData.size = RfemaleSize;
                sizeData.youngSizeMultiplier = RyoungSizeMultiplier;
                sizeData.adultSizeMultiplier = RadultSizeMultiplier;
                sizeData.oldSizeMultiplier = RoldSizeMultiplier;
            }).ScheduleParallel();

            //Update TargetData
            float RsightRadius = RabbitDefaults.sightRadius;
            Entities.WithAll<isRabbitTag>().ForEach((ref TargetData targetData) =>
            {
                targetData.sightRadius = RsightRadius;
            }).ScheduleParallel();



            /*FOX*/

            //Update BasicNeedsData
            float FthirstThreshold = FoxDefaults.thirstyThreshold;
            float FthirstMax = FoxDefaults.thirstMax;
            float FthirstIncrease = FoxDefaults.thirstIncrease;
            float FdrinkingSpeed = FoxDefaults.drinkingSpeed;

            float FhungerThreshold = FoxDefaults.hungryThreshold;
            float FhungerMax = FoxDefaults.hungerMax;
            float FhunderIncrease = FoxDefaults.hungerIncrease;
            float FpregnancyHungerIncrease = FoxDefaults.pregnancyHungerIncrease;
            float FyoungHungerIncrease = FoxDefaults.youngHungerIncrease;
            float FadultHungerIncrease = FoxDefaults.adultHungerIncrease;
            float FoldHungerIncrease = FoxDefaults.oldHungerIncrease;
            float FeatingSpeed = FoxDefaults.eatingSpeed;
            Entities.WithAll<isFoxTag>().ForEach((ref BasicNeedsData basicNeedsData) =>
            {
                basicNeedsData.thirstyThreshold = FthirstThreshold;
                basicNeedsData.thirstMax = FthirstMax;
                basicNeedsData.thirstIncrease = FthirstIncrease;
                basicNeedsData.drinkingSpeed = FdrinkingSpeed;

                basicNeedsData.hungryThreshold = FhungerThreshold;
                basicNeedsData.hungerMax = FhungerMax;
                basicNeedsData.hungerIncrease = FhunderIncrease;
                basicNeedsData.pregnancyHungerIncrease = FpregnancyHungerIncrease;
                basicNeedsData.youngHungerIncrease = FyoungHungerIncrease;
                basicNeedsData.adultHungerIncrease = FadultHungerIncrease;
                basicNeedsData.oldHungerIncrease = FoldHungerIncrease;
                basicNeedsData.eatingSpeed = FeatingSpeed;
            }).ScheduleParallel();

            //Update BioStatsData
            float FageIncrease = FoxDefaults.ageIncrease;
            float FageMax = FoxDefaults.ageMax;
            Entities.WithAll<isFoxTag>().ForEach((ref BioStatsData bioStatsData) =>
            {
                bioStatsData.ageIncrease = FageIncrease;
                bioStatsData.ageMax = FageMax;
            }).ScheduleParallel();

            //Update EdibleData
            float FnutritionalValueBase = FoxDefaults.nutritionalValue;
            bool FcanBeEaten = FoxDefaults.canBeEaten;
            Entities.WithAll<isFoxTag>().ForEach((ref EdibleData edibleData) =>
            {
                edibleData.canBeEaten = FcanBeEaten;
                edibleData.nutritionalValueBase = FnutritionalValueBase;
            }).ScheduleParallel();

            //Update MovementData
            float FmoveSpeed = FoxDefaults.moveSpeed;
            float FpregnancyMoveMultiplier = FoxDefaults.pregnancyMoveMultiplier;
            float FyoungMoveMultiplier = FoxDefaults.youngMoveMultiplier;
            float FadultMoveMultiplier = FoxDefaults.adultMoveMultiplier;
            float FoldMoveMultiplier = FoxDefaults.oldMoveMultiplier;
            Entities.WithAll<isFoxTag>().ForEach((ref MovementData movementData) =>
            {
                movementData.moveSpeedBase = FmoveSpeed;
                movementData.pregnancyMoveMultiplier = FpregnancyMoveMultiplier;
                movementData.youngMoveMultiplier = FyoungMoveMultiplier;
                movementData.adultMoveMultiplier = FadultMoveMultiplier;
                movementData.oldMoveMultiplier = FadultMoveMultiplier;
            }).ScheduleParallel();

            //Update ReproductiveData
            float FmatingDuration = FoxDefaults.matingDuration;
            float FpregnancyLength = FoxDefaults.pregnancyLength;
            float FbirthDuration = FoxDefaults.birthDuration;
            int FlitterSizeMin = FoxDefaults.litterSizeMin;
            int FlitterSizeMax = FoxDefaults.litterSizeMax;
            int FlitterSizeAve = FoxDefaults.litterSizeAve;
            Entities.WithAll<isFoxTag>().ForEach((ref ReproductiveData reproductiveData) =>
            {
                reproductiveData.matingDuration = FmatingDuration;
                reproductiveData.pregnancyLengthBase = FpregnancyLength;
                reproductiveData.birthDuration = FbirthDuration;
                reproductiveData.litterSizeMin = FlitterSizeMin;
                reproductiveData.litterSizeMax = FlitterSizeMax;
                reproductiveData.litterSizeAve = FlitterSizeAve;
            }).ScheduleParallel();

            //Update SizeData
            //Update Scale dependent on gender
            float FmaleSize = FoxDefaults.scaleMale;
            float FfemaleSize = FoxDefaults.scaleFemale;
            float FyoungSizeMultiplier = FoxDefaults.youngSizeMultiplier;
            float FadultSizeMultiplier = FoxDefaults.adultSizeMultiplier;
            float FoldSizeMultiplier = FoxDefaults.oldSizeMultiplier;
            Entities.WithAll<isFoxTag>().ForEach((ref SizeData sizeData, in BioStatsData bioStatsData) =>
            {
                if (bioStatsData.gender == BioStatsData.Gender.Male)
                    sizeData.size = FmaleSize;
                else if (bioStatsData.gender == BioStatsData.Gender.Female)
                    sizeData.size = FfemaleSize;
                sizeData.youngSizeMultiplier = FyoungSizeMultiplier;
                sizeData.adultSizeMultiplier = FadultSizeMultiplier;
                sizeData.oldSizeMultiplier = FoldSizeMultiplier;
            }).ScheduleParallel();

            //Update TargetData
            float FsightRadius = FoxDefaults.sightRadius;
            Entities.WithAll<isFoxTag>().ForEach((ref TargetData targetData) =>
            {
                targetData.sightRadius = FsightRadius;
            }).ScheduleParallel();



            /*GRASS*/

            //Update EdibleData
            float GrassnutritionalValueBase = GrassDefaults.nutritionalValue;
            bool GrasscanBeEaten = GrassDefaults.canBeEaten;
            Entities.WithAll<isGrassTag>().ForEach((ref EdibleData edibleData) =>
            {
                edibleData.canBeEaten = GrasscanBeEaten;
                edibleData.nutritionalValueBase = GrassnutritionalValueBase;
            }).ScheduleParallel();

            //Update SizeData
            float GrassSize = GrassDefaults.scale;
            Entities.WithAll<isGrassTag>().ForEach((ref SizeData sizeData) =>
            {
                sizeData.size = GrassSize;
            }).ScheduleParallel();


            somethingChangedFlag = false;
        }
    }
}
