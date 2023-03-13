using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(StatsEntityBase))]
public class StatsEntityBuff : StatsEnity
{
    public float DamageBuff;

    private StatsEntityFinal _finalStats;
    private StatsEntityBase _baseStats;

    void Awake()
    {
        _baseStats = GetComponent<StatsEntityBase>();
    }

    private void Start()
    {
        _finalStats = GetComponent<StatsEntityFinal>();
        RecalculateFinalStats();
    }

    private void RecalculateFinalStats()
    {
    }
}
