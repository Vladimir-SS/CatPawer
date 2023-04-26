using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleStats : MonoBehaviour
{
    [field: SerializeField] public float MaxHealth { get; protected set; } = 0;

    [field: SerializeField] public float AttackSpeed { get; protected set; } = 0;

    [field: SerializeField] public float RangedDamageBase { get; protected set; } = 0;

    [field: SerializeField] public float Armor { get; protected set; } = 0;

    [field: SerializeField] public float AttackRange { get; protected set; } = 0;

    [field: SerializeField] public float MoveSpeed { get; protected set; } = 0;

    [field: SerializeField] public float DamagePercentageBase { get; protected set; } = 0;
}