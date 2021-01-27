using Components;
using Unity.Entities;

namespace Systems
{
    public class MovementDataSystem : SystemBase
    {
        /*
         * changes the entities' movement multiplier based on age group or state
         */
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref MovementData movementData,
                in BioStatsData bioStatsData,
                in StateData stateData
            ) =>
            {
                if (!stateData.IsPregnant)
                {
                    if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Young)
                        movementData.MoveMultiplier = movementData.YoungMoveMultiplier;
                    else if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Adult)
                        movementData.MoveMultiplier = movementData.AdultMoveMultiplier;
                    else if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Old)
                        movementData.MoveMultiplier = movementData.OldMoveMultiplier;
                }
                else
                {
                    movementData.MoveMultiplier = movementData.PregnancyMoveMultiplier;
                }

                //temp fix set movement to 0 when mating or giving birth
                if (stateData.IsMating | stateData.IsGivingBirth) movementData.MoveMultiplier = 0;
            }).ScheduleParallel();
        }
    }
}