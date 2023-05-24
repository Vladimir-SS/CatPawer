using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    [RequireComponent(typeof(StatsEntityFinal))]
    public class StatsEntity : AutoHandledStats
    {
        protected override StatsEntityFinal FindFinalStats()
        {
            return GetComponent<StatsEntityFinal>();
        }
    }
}






