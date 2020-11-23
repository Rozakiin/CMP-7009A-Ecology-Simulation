using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SpawningSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     float deltaTime = Time.DeltaTime;

        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.

        SpawnRabbitAtPosOnClick();

    }

    private void SpawnRabbitAtPosOnClick()
    {
        //checks for click of the mouse, sends ray out from camera, creates rabbit where it hits
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                CreateRabbitAtPos(targetPosition);
            }
        }
    }
    private void CreateRabbitAtPos(in Vector3 position)
    {
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var entityRabbit = GameObjectConversionUtility.ConvertGameObjectHierarchy(SimulationManager.Instance.rabbit, settings);
        Entity prototypeRabbit = EntityManager.Instantiate(entityRabbit);
        EntityManager.SetName(prototypeRabbit, "ClickRabbit" + prototypeRabbit.Index);
        EntityManager.SetComponentData(prototypeRabbit, new Translation { Value = position }); // set position data (called translation in ECS)
        EntityManager.SetComponentData(prototypeRabbit, new TargetData { currentTarget = position, oldTarget = position, atTarget = true }); // set target data
        EntityManager.DestroyEntity(entityRabbit);
    }
}
