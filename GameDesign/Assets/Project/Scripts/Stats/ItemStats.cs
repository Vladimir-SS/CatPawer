using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : StatsBase
{
    protected override StatsEntityFinal FindFinalStats()
    {
        return GetComponentInParent<StatsEntityFinal>();
    }
}
