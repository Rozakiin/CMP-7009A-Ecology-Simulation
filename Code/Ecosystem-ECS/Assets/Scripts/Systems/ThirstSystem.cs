using Components;
using Unity.Entities;

namespace Systems
{
    public class ThirstSystem : SystemBase
    {
        /* 
         * increases entities' thirst and adjusts how much it increases
         * by various factors. Drinks entities, reduces thirst by entity to drink's
         * value when in Drinking state
         */
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((
                ref BasicNeedsData basicNeedsData,
                in TargetData targetData,
                in StateData stateData
            ) =>
            {
                // Increase thirst
                basicNeedsData.Thirst += basicNeedsData.ThirstIncrease * deltaTime;

                //If the entityToDrink exists and entity is drinking
                if (HasComponent<DrinkableData>(targetData.EntityToDrink) && stateData.IsDrinking)
                    basicNeedsData.Thirst -= GetComponentDataFromEntity<DrinkableData>(true)[targetData.EntityToDrink].Value * basicNeedsData.DrinkingSpeed * deltaTime;
            }).ScheduleParallel();
        }
    }
}