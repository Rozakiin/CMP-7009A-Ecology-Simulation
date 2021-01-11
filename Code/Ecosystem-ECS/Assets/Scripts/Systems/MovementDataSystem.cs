using Unity.Entities;
using Unity.Jobs;

public class MovementDataSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((
            ref MovementData movementData,
            in BioStatsData bioStatsData,
            in StateData stateData
            ) =>
        {
            if (!stateData.isPregnant)
            {
                if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Young)
                {
                    movementData.moveMultiplier = movementData.youngMoveMultiplier;
                }
                else if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Adult)
                {
                    movementData.moveMultiplier = movementData.adultMoveMultiplier;
                }
                else if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Old)
                {
                    movementData.moveMultiplier = movementData.oldMoveMultiplier;
                }
            }
            else
            {
                movementData.moveMultiplier = movementData.pregnancyMoveMultiplier;
            }
        }).ScheduleParallel();
    }
}
