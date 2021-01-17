using Components;
using Unity.Entities;

namespace Systems
{
    public class DeathSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            //catch to not run if paused
            if (MonoBehaviourTools.UI.UITimeControl.instance.GetPause())
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
                if (UtilTools.ComponentTools.ContainsState(StateData.FlagStates.Dead, stateData.flagState))
                {
                    rabbitsDeadTotal++;
                    switch (stateData.deathReason)
                    {
                        case StateData.DeathReason.Age:
                            rabbitsDeadAge++;
                            break;
                        case StateData.DeathReason.Eaten:
                            rabbitsDeadEaten++;
                            break;
                        case StateData.DeathReason.Hunger:
                            rabbitsDeadHunger++;
                            break;
                        case StateData.DeathReason.Thirst:
                            rabbitsDeadThirst++;
                            break;
                        default:
                            throw new System.NotImplementedException();
                    }
                    EntityManager.DestroyEntity(entity);
                }
            }).WithStructuralChanges().Run();
            SimulationManager.Instance.rabbitPopulation = rabbitsTotal;
            SimulationManager.Instance.rabbitPopulation -= rabbitsDeadTotal;
            SimulationManager.Instance.numberOfRabbitsDeadTotal += rabbitsDeadTotal;
            SimulationManager.Instance.numberOfRabbitsDeadAge += rabbitsDeadAge;
            SimulationManager.Instance.numberOfRabbitsDeadEaten += rabbitsDeadEaten;
            SimulationManager.Instance.numberOfRabbitsDeadHunger += rabbitsDeadHunger;
            SimulationManager.Instance.numberOfRabbitsDeadThirst += rabbitsDeadThirst;


            int foxesDeadTotal = 0;
            int foxesDeadAge = 0;
            int foxesDeadEaten = 0;
            int foxesDeadHunger = 0;
            int foxesDeadThirst = 0;

            int foxesTotal = 0;
            Entities.WithAll<IsFoxTag>().ForEach((Entity entity, in StateData stateData) =>
            {
                foxesTotal++;
                if (UtilTools.ComponentTools.ContainsState(StateData.FlagStates.Dead, stateData.flagState))
                {
                    foxesDeadTotal++;
                    switch (stateData.deathReason)
                    {
                        case StateData.DeathReason.Age:
                            foxesDeadAge++;
                            break;
                        case StateData.DeathReason.Eaten:
                            foxesDeadEaten++;
                            break;
                        case StateData.DeathReason.Hunger:
                            foxesDeadHunger++;
                            break;
                        case StateData.DeathReason.Thirst:
                            foxesDeadThirst++;
                            break;
                        default:
                            throw new System.NotImplementedException();
                    }
                    EntityManager.DestroyEntity(entity);
                }
            }).WithStructuralChanges().Run();
            SimulationManager.Instance.foxPopulation = foxesTotal;
            SimulationManager.Instance.foxPopulation -= foxesDeadTotal;
            SimulationManager.Instance.numberOfFoxesDeadTotal += foxesDeadTotal;
            SimulationManager.Instance.numberOfFoxesDeadAge += foxesDeadAge;
            SimulationManager.Instance.numberOfFoxesDeadEaten += foxesDeadEaten;
            SimulationManager.Instance.numberOfFoxesDeadHunger += foxesDeadHunger;
            SimulationManager.Instance.numberOfFoxesDeadThirst += foxesDeadThirst;


            int grassEaten = 0;
            int grassTotal = 0;

            Entities.WithAll<IsGrassTag>().ForEach((Entity entity, in StateData stateData) =>
            {
                grassTotal++;
                if (UtilTools.ComponentTools.ContainsState(StateData.FlagStates.Dead, stateData.flagState))
                {
                    grassEaten++;
                    EntityManager.DestroyEntity(entity);
                }
            }).WithStructuralChanges().Run();
            SimulationManager.Instance.grassPopulation = grassTotal;
            SimulationManager.Instance.grassPopulation -= grassEaten;
            SimulationManager.Instance.numberOfGrassEaten += grassEaten;
        }
    }
}
