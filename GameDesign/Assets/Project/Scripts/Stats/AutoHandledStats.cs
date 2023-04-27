using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Stats
{
    public abstract class AutoHandledStats : PossibleStats
    {
        protected StatsEntityFinal FinalStats;

        protected abstract StatsEntityFinal FindFinalStats();

        
        private bool isAdded = false;
        protected readonly object balanceLock = new();

        protected void AddMyself()
        {
            lock (balanceLock)
            {
                if (!isAdded)
                {
                    isAdded = true;
                    FinalStats.AddStats(this);
                }
            }
        }

        protected void RemoveMyself()
        {
            lock (balanceLock)
            {
                if (isAdded)
                {
                    isAdded = false;
                    FinalStats.RemoveStats(this);
                }
            }
        }

        void Awake()
        {
            FinalStats = FindFinalStats();
        }

        void Start()
        {
            AddMyself();
        }

        void OnDestroy()
        {
            RemoveMyself();
        }

        private void OnEnable()
        {
            AddMyself();
        }

        private void OnDisable()
        {
            RemoveMyself();
        }
    }


}
