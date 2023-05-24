using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class ItemStats : AutoHandledStats
    {
        protected override StatsEntityFinal FindFinalStats()
        {
            return GetComponentInParent<StatsEntityFinal>();
        }
    }
}