using Components;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public class ScaleSystem : SystemBase
    {
        /*
         * update the size of the entity model base on age for entities with biostatsdata
         */
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref CompositeScale scale,
                ref SizeData sizeData,
                in BioStatsData bioStatsData
            ) =>
            {
                if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Old)
                {
                    sizeData.ageSizeMultiplier = sizeData.oldSizeMultiplier;
                }
                else if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Adult)
                {
                    sizeData.ageSizeMultiplier = sizeData.adultSizeMultiplier;
                }
                else if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Young)
                {
                    sizeData.ageSizeMultiplier = sizeData.youngSizeMultiplier;
                }

                scale.Value.c0.x = sizeData.Size;
                scale.Value.c1.y = sizeData.Size;
                scale.Value.c2.z = sizeData.Size;
            }).ScheduleParallel();

            Entities.WithNone<BioStatsData>().ForEach((
                ref CompositeScale scale,
                in SizeData sizeData
            ) =>
            {
                scale.Value.c0.x = sizeData.Size;
                scale.Value.c1.y = sizeData.Size;
                scale.Value.c2.z = sizeData.Size;
            }).ScheduleParallel();
        }
    }
}