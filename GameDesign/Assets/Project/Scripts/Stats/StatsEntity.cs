using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsEntityFinal))]
public class StatsEntity : StatsBase
{
    protected override StatsEntityFinal FindFinalStats()
    {
        return GetComponent<StatsEntityFinal>();
    }
}




