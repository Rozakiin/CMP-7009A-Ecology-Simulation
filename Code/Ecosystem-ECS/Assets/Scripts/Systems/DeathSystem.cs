using Components;
using Unity.Entities;

namespace Systems
{
    public class DeathSystem : SystemBase
    {
        /*
         * Counts the number of entities that have died,
         * tallies the ways they died and updates the count
         * in SimulationController. Then deletes the entity.
         */
        protected override void OnUpdate()
        {
            //catch to not run if paused
            if (MonoBehaviourTools.UI.UITimeControl.Instance.GetPause())
            {
                return;
            }
            // Checks What entity is dead, increment the dead count, decrement the living count, store how they died and destory the entity
            // Also counts the total number of each entity

            int rabbitsDeadTotal = 0;
            int rabbitsDeadAge = 0;
            int rabbitsDeadEaten = 0;
            int rabbitsDeadHunger = 0;
            int rabbitsDeadThirst = 0;

            int rabbitsTotal = 0;
            Entities.WithAll<IsRabbitTag>().ForEach((Entity entity, in StateData stateData) =>
            {
                rabbitsTotal++;
                if (UtilTools.ComponentTools.ContainsState(StateData.FlagStates.Dead, stateData.FlagStateCurrent))
                {
                    rabbitsDeadTotal++;
                    switch (stateData.DeathReason)
                    {
                        case StateData.DeathReasons.Age:
                            rabbitsDeadAge++;
                            break;
                        case StateData.DeathReasons.Eaten:
                            rabbitsDeadEaten++;
                            break;
                        case StateData.DeathReasons.Hunger:
                            rabbitsDeadHunger++;
                            break;
                        case StateData.DeathReasons.Thirst:
                            rabbitsDeadThirst++;
                            break;
                        default:
                            throw new System.NotImplementedException();
                    }
                    EntityManager.DestroyEntity(entity);
                }
            }).WithStructuralChanges().Run();
            SimulationManager.Instance.RabbitPopulation = rabbitsTotal;
            SimulationManager.Instance.RabbitPopulation -= rabbitsDeadTotal;
            SimulationManager.Instance.NumberOfRabbitsDeadTotal += rabbitsDeadTotal;
            SimulationManager.Instance.NumberOfRabbitsDeadAge += rabbitsDeadAge;
            SimulationManager.Instance.NumberOfRabbitsDeadEaten += rabbitsDeadEaten;
            SimulationManager.Instance.NumberOfRabbitsDeadHunger += rabbitsDeadHunger;
            SimulationManager.Instance.NumberOfRabbitsDeadThirst += rabbitsDeadThirst;


            int foxesDeadTotal = 0;
            int foxesDeadAge = 0;
            int foxesDeadEaten = 0;
            int foxesDeadHunger = 0;
            int foxesDeadThirst = 0;

            int foxesTotal = 0;
            Entities.WithAll<IsFoxTag>().ForEach((Entity entity, in StateData stateData) =>
            {
                foxesTotal++;
                if (UtilTools.ComponentTools.ContainsState(StateData.FlagStates.Dead, stateData.FlagStateCurrent))
                {
                    foxesDeadTotal++;
                    switch (stateData.DeathReason)
                    {
                        case StateData.DeathReasons.Age:
                            foxesDeadAge++;
                            break;
                        case StateData.DeathReasons.Eaten:
                            foxesDeadEaten++;
                            break;
                        case StateData.DeathReasons.Hunger:
                            foxesDeadHunger++;
                            break;
                        case StateData.DeathReasons.Thirst:
                            foxesDeadThirst++;
                            break;
                        default:
                            throw new System.NotImplementedException();
                    }
                    EntityManager.DestroyEntity(entity);
                }
            }).WithStructuralChanges().Run();
            SimulationManager.Instance.FoxPopulation = foxesTotal;
            SimulationManager.Instance.FoxPopulation -= foxesDeadTotal;
            SimulationManager.Instance.NumberOfFoxesDeadTotal += foxesDeadTotal;
            SimulationManager.Instance.NumberOfFoxesDeadAge += foxesDeadAge;
            SimulationManager.Instance.NumberOfFoxesDeadEaten += foxesDeadEaten;
            SimulationManager.Instance.NumberOfFoxesDeadHunger += foxesDeadHunger;
            SimulationManager.Instance.NumberOfFoxesDeadThirst += foxesDeadThirst;


            int grassEaten = 0;
            int grassTotal = 0;

            Entities.WithAll<IsGrassTag>().ForEach((Entity entity, in StateData stateData) =>
            {
                grassTotal++;
                if (UtilTools.ComponentTools.ContainsState(StateData.FlagStates.Dead, stateData.FlagStateCurrent))
                {
                    grassEaten++;
                    EntityManager.DestroyEntity(entity);
                }
            }).WithStructuralChanges().Run();
            SimulationManager.Instance.GrassPopulation = grassTotal;
            SimulationManager.Instance.GrassPopulation -= grassEaten;
            SimulationManager.Instance.NumberOfGrassEaten += grassEaten;
        }
    }
}
