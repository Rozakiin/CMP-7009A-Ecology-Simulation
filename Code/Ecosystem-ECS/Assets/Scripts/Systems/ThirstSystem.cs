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
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((ref BasicNeedsData basicNeedsData, in TargetData targetData, in StateData stateData) =>
            {
                // Increase thirst
                basicNeedsData.thirst += basicNeedsData.thirstIncrease * deltaTime;

                //If the entityToDrink exists and entity is drinking
                if (HasComponent<DrinkableData>(targetData.entityToDrink) && stateData.isDrinking)
                {
                    basicNeedsData.thirst -= GetComponentDataFromEntity<DrinkableData>(true)[targetData.entityToDrink].Value * basicNeedsData.drinkingSpeed * deltaTime; //gets drink Value from entityToDrink
                }
            }).ScheduleParallel();
        }
    }
}
