using Components;
using Unity.Entities;

namespace Systems
{
    public class AgeingSystem : SystemBase
    {
        /* 
         * increases the age of the entities by ageIncrease * deltaTime,
         * changes the ageGroup of the entity if they have reached the required age
         */
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((ref BioStatsData bioStatsData) =>
            {
                // Increase age
                bioStatsData.Age += bioStatsData.AgeIncrease * deltaTime;

                if (bioStatsData.Age >= bioStatsData.OldEntryTimer)
                {
                    bioStatsData.AgeGroup = BioStatsData.AgeGroups.Old;
                }
                else if (bioStatsData.Age >= bioStatsData.AdultEntryTimer)
                {
                    bioStatsData.AgeGroup = BioStatsData.AgeGroups.Adult;
                }
            }).ScheduleParallel();
        }
    }
}
