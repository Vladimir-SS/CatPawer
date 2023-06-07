using Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPercentage : MonoBehaviour
{
    [SerializeField] private float percentage = 10f;
    
    void Start()
    {
        Damageable parrentDmg = GetComponentInParent<Damageable>();
        StatsEntityFinal parrentStats = GetComponentInParent<StatsEntityFinal>();

        if (parrentDmg != null && parrentStats != null)
        {
            parrentDmg.Heal(parrentStats.Body.MaxHP * (percentage / 100));
        }
    }

}
