﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class DeathSystem : SystemBase
{
    //private EndSimulationEntityCommandBufferSystem ecbSystem;

    protected override void OnCreate()
    {
        //ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        //var ecb = ecbSystem.CreateCommandBuffer().ToConcurrent();

        // Checks What entity is dead, increment the dead count, decrement the living count, store how they died and destory the entity

        int rabbitsDeadTotal = 0;
        int rabbitsDeadAge = 0;
        int rabbitsDeadEaten = 0;
        int rabbitsDeadHunger = 0;
        int rabbitsDeadThirst = 0;
        Entities.WithAll<isRabbitTag>().ForEach((Entity entity, int entityInQueryIndex, in StateData stateData) => {
            if(stateData.state == StateData.States.Dead)
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
                //ecb.DestroyEntity(entityInQueryIndex, entity);
            }
        }).WithStructuralChanges().Run();
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
        Entities.WithAll<isFoxTag>().ForEach((Entity entity, int entityInQueryIndex, in StateData stateData) => {
            if (stateData.state == StateData.States.Dead)
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
                //ecb.DestroyEntity(entityInQueryIndex, entity);
            }
        }).WithStructuralChanges().Run();
        SimulationManager.Instance.foxPopulation -= foxesDeadTotal;
        SimulationManager.Instance.numberOfFoxesDeadTotal += foxesDeadTotal;
        SimulationManager.Instance.numberOfFoxesDeadAge += foxesDeadAge;
        SimulationManager.Instance.numberOfFoxesDeadEaten += foxesDeadEaten;
        SimulationManager.Instance.numberOfFoxesDeadHunger += foxesDeadHunger;
        SimulationManager.Instance.numberOfFoxesDeadThirst += foxesDeadThirst;


        int grassEaten = 0;
        Entities.WithAll<isGrassTag>().ForEach((Entity entity, int entityInQueryIndex, in StateData stateData) => {
            if (stateData.state == StateData.States.Dead)
            {
                grassEaten++;
                EntityManager.DestroyEntity(entity);
                //ecb.DestroyEntity(entityInQueryIndex, entity);
            }
        }).WithStructuralChanges().Run();
        SimulationManager.Instance.grassPopulation -= grassEaten;
        SimulationManager.Instance.numberOfGrassEaten += grassEaten;


        // Make sure that the ECB system knows about our job
        //ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}