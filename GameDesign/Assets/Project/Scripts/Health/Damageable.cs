using System;
using System.Xml.Serialization;
using UnityEngine;
using Stats;
using Stats.Structs;

[RequireComponent(typeof(StatsEntityFinal))]
public class Damageable : MonoBehaviour
{
    private float currentHealth = 0;
    public float CurrentHealth {
        get
        {
            return Math.Max(0, currentHealth);
        }
    }
    protected StatsEntityFinal stats;
    
    void Start()
    {
        stats = GetComponent<StatsEntityFinal>();
        lock(maxHealthLock)
        {
            currentHealth = stats.Body.MaxHP;
        }
    }

    //TODO: change with an event subscription later on
    private float lastMaxHealth = 0;
    private object maxHealthLock = new object();

    private void UpdateCurrentHealth(float newMaxHealth)
    {
        lock (maxHealthLock)
        {
            currentHealth = Math.Min(currentHealth, newMaxHealth);
            newMaxHealth = lastMaxHealth;
        }
    }

    private void Update()
    {
        if(stats.Body.MaxHP != lastMaxHealth)
        {
            UpdateCurrentHealth(stats.Body.MaxHP);
        }

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= Math.Max(1, damage - damage * stats.Body.DamageReduction);
        Debug.Log("Taking damage, current health : " + currentHealth + " max HP : " + stats.Body.MaxHP + " damage : " + damage);

        if (currentHealth <= 0)
        {
            Die();
        }
        
    }
    
    void Die()
    {
        Destroy(transform.root.gameObject);
        Debug.Log("Just died damn");
    }
}