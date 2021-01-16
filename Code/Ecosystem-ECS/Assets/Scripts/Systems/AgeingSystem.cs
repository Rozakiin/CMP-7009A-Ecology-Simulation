using Components;
using Unity.Entities;

namespace Systems
{
    public class AgeingSystem : SystemBase
    {
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
