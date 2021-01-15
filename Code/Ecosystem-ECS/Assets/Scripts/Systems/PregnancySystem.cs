using Components;
using Unity.Entities;

namespace Systems
{
    public class PregnancySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref ReproductiveData reproductiveData,
                in StateData stateData,
                in BioStatsData bioStatsData
            ) =>
            {

                if (stateData.isPregnant)
                {
                    if (bioStatsData.age - reproductiveData.pregnancyStartTime >= reproductiveData.PregnancyLength)
                    {
                        reproductiveData.pregnant = false;
                    }
                }
            }).ScheduleParallel();
        }
    }
}
