using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatsBase : PossibleStats
{
    protected StatsEntityFinal FinalStats;

    protected abstract StatsEntityFinal FindFinalStats();

    void Awake()
    {
        FinalStats = FindFinalStats();
    }

    void Start()
    {
        FinalStats.AddStats(this);
    }

    void OnDestroy()
    {
        FinalStats.RemoveStats(this);
    }
}
