﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PregnancySystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((
            ref ReproductiveData reproductiveData, 
            ref StateData stateData,
            in BioStatsData bioStatsData
            ) => {
            
            if (stateData.isPregnant)
            {
                if (bioStatsData.age - reproductiveData.pregnancyStartTime >= reproductiveData.PregnancyLength)
                {
                    reproductiveData.birthStartTime = bioStatsData.age;
                    reproductiveData.pregnant = false;
                    stateData.flagState ^= StateData.FlagStates.Pregnant;
                }
            }
        }).Schedule();
    }
}
