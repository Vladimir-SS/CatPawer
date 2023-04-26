using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsEntityFinal : PossibleStats
{
    //TODO: Remove [field: SerializeField] when you finished debugging
    [field: SerializeField] public float MeleeDamage { get; protected set; } = 0;
    [field: SerializeField] public float RangedDamage { get; protected set; } = 0;

    private void RecalculateStats()
    {
        RangedDamage = RangedDamageBase + (RangedDamageBase * DamagePercentageBase);
    }
    
    public void AddStats(StatsBase stats)
    {
        MaxHealth += stats.MaxHealth;
        AttackSpeed += stats.AttackSpeed;
        RangedDamageBase += stats.RangedDamageBase;
        Armor += stats.Armor;
        AttackRange += stats.AttackRange;
        MoveSpeed += stats.MoveSpeed;
        DamagePercentageBase += stats.DamagePercentageBase;

        RecalculateStats();
    }

    public void RemoveStats(StatsBase stats)
    {
        MaxHealth -= stats.MaxHealth;
        AttackSpeed -= stats.AttackSpeed;
        RangedDamageBase -= stats.RangedDamageBase;
        Armor -= stats.Armor;
        AttackRange -= stats.AttackRange;
        MoveSpeed -= stats.MoveSpeed;
        DamagePercentageBase -= stats.DamagePercentageBase;

        RecalculateStats();
    }
}