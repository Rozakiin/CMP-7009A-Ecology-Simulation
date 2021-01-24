using Components;
using Unity.Entities;

namespace Systems
{
    public class HungerSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
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
                in BioStatsData bioStatsData
            ) =>
            {
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
                if (HasComponent<EdibleData>(targetData.entityToEat) && stateData.isEating)
                {
                    basicNeedsData.hunger -= GetComponentDataFromEntity<EdibleData>(true)[targetData.entityToEat]
                        .NutritionalValue;
                    if (basicNeedsData.hunger < 0) basicNeedsData.hunger = 0;
                    //set beenEaten to true in entityToEat
                    if (HasComponent<StateData>(targetData.entityToEat))
                    {
                        ecb.SetComponent(entityInQueryIndex, targetData.entityToEat,
                            new StateData
                            {
                                deathReason = StateData.DeathReason.Eaten,

                                flagState = StateData.FlagStates.Dead
                            }
                        );
                    }
                }
            }).ScheduleParallel();

            // Make sure that the ECB system knows about our job
            ecbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}
