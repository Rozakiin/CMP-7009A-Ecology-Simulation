using Components;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public class ScaleSystem : SystemBase
    {
        /*
         * update the SizeBase of the entity model base on age for entities with biostatsdata
         */
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref CompositeScale scale,
                ref SizeData sizeData,
                in BioStatsData bioStatsData
            ) =>
            {
                if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Old)
                    sizeData.AgeSizeMultiplier = sizeData.OldSizeMultiplier;
                else if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Adult)
                    sizeData.AgeSizeMultiplier = sizeData.AdultSizeMultiplier;
                else if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Young)
                    sizeData.AgeSizeMultiplier = sizeData.YoungSizeMultiplier;

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