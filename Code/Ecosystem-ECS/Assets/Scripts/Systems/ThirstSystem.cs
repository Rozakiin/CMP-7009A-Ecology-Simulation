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


        Entities.ForEach((ref BasicNeedsData basicNeedsData, in StateData stateData) => {

            // Increase thirst
            basicNeedsData.thirst += basicNeedsData.thirstIncrease * deltaTime;

            //If the entityToDrink exists and entity is drinking
            if (basicNeedsData.entityToDrink != Entity.Null && stateData.state == StateData.States.Drinking)
            {
                if (HasComponent<DrinkableData>(basicNeedsData.entityToDrink))
                {
                    basicNeedsData.thirst -= GetComponentDataFromEntity<DrinkableData>(true)[basicNeedsData.entityToDrink].Value * basicNeedsData.drinkingSpeed * deltaTime; //gets nutritionalValue from entityToEat (GetComponentDataFromEntity gives array like access)
                }
            }
        }).ScheduleParallel();
    }
}
