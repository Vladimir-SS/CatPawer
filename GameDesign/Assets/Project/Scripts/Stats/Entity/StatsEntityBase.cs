using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsEntityBase : StatsEnity
{
    void Awake()
    {
        gameObject.AddComponent<StatsEntityFinal>();
    }
}