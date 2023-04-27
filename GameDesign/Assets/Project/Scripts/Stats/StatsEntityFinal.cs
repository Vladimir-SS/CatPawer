using Stats.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    public class StatsEntityFinal : MonoBehaviour
    {
        [field: SerializeField] public Structs.Body Body { get; protected set; } = new Structs.Body { MaxHP = 0, Armor = 0, Speed = 0 };
        private Structs.Combat CombatBase;
        [field: SerializeField] public Structs.Gun Gun { get; protected set; }

        [Serializable]
        public struct CalculatedCombat
        {
            public float Damage;
        }

        [field: SerializeField] public CalculatedCombat Combat;



        private readonly object changeLock = new();

        private void RecalculateStats()
        {
            Combat.Damage = CombatBase.DamageBase + CombatBase.DamageBase * (CombatBase.DamageBonus / 100);
        }

        public void AddStats(PossibleStats stats)
        {
            lock (changeLock)
            {
                Body += stats.Body;
                CombatBase += stats.Combat;
                Gun += stats.Gun;


                RecalculateStats();
            }
        }

        public void RemoveStats(PossibleStats stats)
        {
            lock (changeLock)
            {
                Body -= stats.Body;
                CombatBase -= stats.Combat;
                Gun -= stats.Gun;

                RecalculateStats();
            }
        }
    }
}
