using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class HungerSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate()
    {
        ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        float deltaTime = Time.DeltaTime;

        Entities.ForEach((
            int entityInQueryIndex, 
            ref BasicNeedsData basicNeedsData, 
            in TargetData targetData, 
            in StateData stateData, 
            in ReproductiveData reproductiveData, 
            in BioStatsData bioStatsData
            ) => {

            if (!stateData.isPregnant)
            {
                if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Young)
                {
                    basicNeedsData.hungerIncrease = basicNeedsData.youngHungerIncrease;
                }
                else if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Adult)
                {
                    basicNeedsData.hungerIncrease = basicNeedsData.adultHungerIncrease;
                }
                else if (bioStatsData.ageGroup == BioStatsData.AgeGroup.Old)
                {
                    basicNeedsData.hungerIncrease = basicNeedsData.oldHungerIncrease;
                }
            }
            else
            {
                basicNeedsData.hungerIncrease = basicNeedsData.pregnancyHungerIncrease;
            }

            // Increase hunger
            basicNeedsData.hunger += basicNeedsData.hungerIncrease * deltaTime;

            //If the entityToEat exists and entity is eating, set entityToEat state to dead and eaten.Decrease hunger by nutritionvalue of entity
            if (targetData.entityToEat != Entity.Null && stateData.isEating)
            {
                if (HasComponent<EdibleData>(targetData.entityToEat))
                {
                        basicNeedsData.hunger -= GetComponentDataFromEntity<EdibleData>(true)[targetData.entityToEat].NutritionalValue;
                        //basicNeedsData.hunger -= GetComponentDataFromEntity<EdibleData>(true)[targetData.entityToEat].NutritionalValue * basicNeedsData.eatingSpeed * deltaTime; //gets nutritionalValue from entityToEat (GetComponentDataFromEntity gives array like access)
                        //set beenEaten to true in entityToEat
                        if (HasComponent<StateData>(targetData.entityToEat))
                        {
                            ecb.SetComponent(entityInQueryIndex, targetData.entityToEat, 
                                new StateData { 
                                    state = StateData.States.Dead, 
                                    deathReason = StateData.DeathReason.Eaten,
                                    
                                    flagState = StateData.FlagStates.Dead
                                }
                            );
                        }
                }
            }
        }).ScheduleParallel();
        // Make sure that the ECB system knows about our job
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
