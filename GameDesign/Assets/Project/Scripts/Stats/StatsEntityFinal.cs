using Stats.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class StatsEntityFinal : MonoBehaviour
    {
        [field: SerializeField] public Structs.Final.Body Body { get; protected set; }
        [field: SerializeField] public Gun Gun { get; protected set; }
        [field: SerializeField] public Structs.Final.Combat Combat { get; protected set; }

        private Combat CombatBase;
        private Body BodyBase;
        private Gun GunBase;




        private readonly object changeLock = new();

        private void RecalculateStats()
        {
            Body = BodyBase;
            Combat = CombatBase;
            Gun = GunBase;
        }

        public void AddStats(PossibleStats stats)
        {
            lock (changeLock)
            {
                BodyBase += stats.Body;
                CombatBase += stats.Combat;
                GunBase += stats.Gun;


                RecalculateStats();
            }
        }

        public void RemoveStats(PossibleStats stats)
        {
            lock (changeLock)
            {
                BodyBase -= stats.Body;
                CombatBase -= stats.Combat;
                GunBase -= stats.Gun;

                RecalculateStats();
            }
        }
    }
}
