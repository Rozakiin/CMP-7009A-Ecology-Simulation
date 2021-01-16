using Components;
using Unity.Entities;

namespace Systems
{
    public class DeathSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            // Checks What entity is dead, increment the dead count, decrement the living count, store how they died and destory the entity
            // Also counts the total number of each entity

            int rabbitsDeadTotal = 0;
            int rabbitsDeadAge = 0;
            int rabbitsDeadEaten = 0;
            int rabbitsDeadHunger = 0;
            int rabbitsDeadThirst = 0;

            int rabbitsTotal = 0;
            Entities.WithAll<isRabbitTag>().ForEach((Entity entity, in StateData stateData) =>
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
            SimulationManager.instance.rabbitPopulation = rabbitsTotal;
            SimulationManager.instance.rabbitPopulation -= rabbitsDeadTotal;
            SimulationManager.instance.numberOfRabbitsDeadTotal += rabbitsDeadTotal;
            SimulationManager.instance.numberOfRabbitsDeadAge += rabbitsDeadAge;
            SimulationManager.instance.numberOfRabbitsDeadEaten += rabbitsDeadEaten;
            SimulationManager.instance.numberOfRabbitsDeadHunger += rabbitsDeadHunger;
            SimulationManager.instance.numberOfRabbitsDeadThirst += rabbitsDeadThirst;


            int foxesDeadTotal = 0;
            int foxesDeadAge = 0;
            int foxesDeadEaten = 0;
            int foxesDeadHunger = 0;
            int foxesDeadThirst = 0;

            int foxesTotal = 0;
            Entities.WithAll<isFoxTag>().ForEach((Entity entity, in StateData stateData) =>
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
            SimulationManager.instance.foxPopulation = foxesTotal;
            SimulationManager.instance.foxPopulation -= foxesDeadTotal;
            SimulationManager.instance.numberOfFoxesDeadTotal += foxesDeadTotal;
            SimulationManager.instance.numberOfFoxesDeadAge += foxesDeadAge;
            SimulationManager.instance.numberOfFoxesDeadEaten += foxesDeadEaten;
            SimulationManager.instance.numberOfFoxesDeadHunger += foxesDeadHunger;
            SimulationManager.instance.numberOfFoxesDeadThirst += foxesDeadThirst;


            int grassEaten = 0;
            int grassTotal = 0;

            Entities.WithAll<isGrassTag>().ForEach((Entity entity, in StateData stateData) =>
            {
                grassTotal++;
                if (UtilTools.ComponentTools.ContainsState(StateData.FlagStates.Dead, stateData.flagState))
                {
                    grassEaten++;
                    EntityManager.DestroyEntity(entity);
                }
            }).WithStructuralChanges().Run();
            SimulationManager.instance.grassPopulation = grassTotal;
            SimulationManager.instance.grassPopulation -= grassEaten;
            SimulationManager.instance.numberOfGrassEaten += grassEaten;
        }
    }
}
