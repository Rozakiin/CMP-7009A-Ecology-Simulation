using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ScaleSystem : SystemBase
{
    protected override void OnUpdate()
    {
        
        Entities.ForEach((
            ref NonUniformScale scale, 
            ref SizeData sizeData, 
            in BioStatsData bioStatsData, 
            in ReproductiveData reproductiveData
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
            
            scale.Value = sizeData.Size;

        }).ScheduleParallel();
    }
}
