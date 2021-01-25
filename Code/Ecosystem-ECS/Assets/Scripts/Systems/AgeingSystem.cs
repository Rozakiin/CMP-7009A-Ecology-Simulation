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
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((ref BioStatsData bioStatsData) =>
            {
                // Increase age
                bioStatsData.age += bioStatsData.ageIncrease * deltaTime;

                if (bioStatsData.age >= bioStatsData.oldEntryTimer)
                {
                    bioStatsData.ageGroup = BioStatsData.AgeGroup.Old;
                }
                else if (bioStatsData.age >= bioStatsData.adultEntryTimer)
                {
                    bioStatsData.ageGroup = BioStatsData.AgeGroup.Adult;
                }
            }).ScheduleParallel();
        }
    }
}
