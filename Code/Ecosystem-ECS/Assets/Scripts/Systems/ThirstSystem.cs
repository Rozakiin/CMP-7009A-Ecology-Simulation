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


        Entities.ForEach((ref BasicNeedsData basicNeedsData, in LookingEntityData lookingEntityData , in StateData stateData) => {

            // Increase thirst
            basicNeedsData.thirst += basicNeedsData.thirstIncrease * deltaTime;

            //If the entityToDrink exists and entity is drinking
            if (lookingEntityData.entityToDrink != Entity.Null && stateData.state == StateData.States.Drinking)
            {
                if (HasComponent<DrinkableData>(lookingEntityData.entityToDrink))
                {
                    basicNeedsData.thirst -= GetComponentDataFromEntity<DrinkableData>(true)[lookingEntityData.entityToDrink].Value * basicNeedsData.drinkingSpeed * deltaTime; //gets nutritionalValue from entityToEat (GetComponentDataFromEntity gives array like access)
                }
            }
        }).ScheduleParallel();
    }
}
