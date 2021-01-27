using Components;
using Unity.Entities;

namespace Systems
{
    public class HungerSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _ecbSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            _ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        /* 
         * increases entities' hunger and adjusts how much it increases
         * by various factors. Eats entities (sets their state to dead + eaten
         * when hunger is over threshold and is state Eating
         */
        protected override void OnUpdate()
        {
            var ecb = _ecbSystem.CreateCommandBuffer().ToConcurrent();

            var deltaTime = Time.DeltaTime;

            Entities.ForEach((
                int entityInQueryIndex,
                ref BasicNeedsData basicNeedsData,
                in TargetData targetData,
                in StateData stateData,
                in BioStatsData bioStatsData
            ) =>
            {
                if (!stateData.IsPregnant)
                {
                    if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Young)
                        basicNeedsData.HungerIncrease = basicNeedsData.YoungHungerIncrease;
                    else if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Adult)
                        basicNeedsData.HungerIncrease = basicNeedsData.AdultHungerIncrease;
                    else if (bioStatsData.AgeGroup == BioStatsData.AgeGroups.Old)
                        basicNeedsData.HungerIncrease = basicNeedsData.OldHungerIncrease;
                }
                else
                {
                    basicNeedsData.HungerIncrease = basicNeedsData.PregnancyHungerIncrease;
                }

                // Increase hunger
                basicNeedsData.Hunger += basicNeedsData.HungerIncrease * deltaTime;

                //If the entityToEat exists and entity is eating, set entityToEat state to dead and eaten.Decrease hunger by nutrition value of entity
                if (HasComponent<EdibleData>(targetData.EntityToEat) && stateData.IsEating)
                {
                    basicNeedsData.Hunger -= GetComponentDataFromEntity<EdibleData>(true)[targetData.EntityToEat].NutritionalValue;
                    if (basicNeedsData.Hunger < 0) basicNeedsData.Hunger = 0;
                    //set beenEaten to true in entityToEat
                    if (HasComponent<StateData>(targetData.EntityToEat))
                        ecb.SetComponent(entityInQueryIndex, targetData.EntityToEat,
                            new StateData
                            {
                                DeathReason = StateData.DeathReasons.Eaten,

                                FlagStateCurrent = StateData.FlagStates.Dead
                            }
                        );
                }
            }).ScheduleParallel();

            // Make sure that the ECB system knows about our job
            _ecbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}