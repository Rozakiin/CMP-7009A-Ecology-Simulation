using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

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
            //Update Speed
            float speed = RabbitDefaults.moveSpeed;
            Entities.WithAll<isRabbitTag>().ForEach((ref MovementData movementData) =>
            {

                movementData.moveSpeedBase = speed;

            }).ScheduleParallel();

            //Update Scale dependent on gender
            float malesize = RabbitDefaults.scaleMale;
            float femalesize = RabbitDefaults.scaleFemale;

            Entities.WithAll<isRabbitTag>().ForEach((ref SizeData sizeData, in BioStatsData bioStatsData) =>
            {
                if(bioStatsData.gender == BioStatsData.Gender.Male)
                    sizeData.size = malesize;
                else if (bioStatsData.gender == BioStatsData.Gender.Female)
                    sizeData.size = femalesize;
            }).ScheduleParallel();

            somethingChangedFlag = false;
        }
    }
}
