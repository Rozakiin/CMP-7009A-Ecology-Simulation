using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ThirstSystem : SystemBase
{
    protected override void OnUpdate()
    {

        float deltaTime = Time.DeltaTime;


        Entities.ForEach((ref BasicNeedsData basicNeedsData, in TargetData targetData , in StateData stateData) => {

            // Increase thirst
            basicNeedsData.thirst += basicNeedsData.thirstIncrease * deltaTime;

            //If the entityToDrink exists and entity is drinking
            if (HasComponent<DrinkableData>(targetData.entityToDrink) && stateData.isDrinking)
            {
                basicNeedsData.thirst -= GetComponentDataFromEntity<DrinkableData>(true)[targetData.entityToDrink].Value * basicNeedsData.drinkingSpeed * deltaTime; //gets nutritionalValue from entityToEat (GetComponentDataFromEntity gives array like access)
            }
        }).ScheduleParallel();
    }
}
