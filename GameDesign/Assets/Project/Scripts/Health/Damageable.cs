
using System;
using System.Xml.Serialization;
using UnityEngine;

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
        currentHealth = stats.MaxHealth;
    }

    //TODO: change with an event subscription later on
    private float lastMaxHealth = 0;
    private void UpdateCurrentHealth(float newMaxHealth)
    {
        var delta = newMaxHealth - lastMaxHealth;
        lastMaxHealth = newMaxHealth;
        currentHealth += delta;
    }

    private void Update()
    {
        if(stats.MaxHealth != lastMaxHealth)
        {
            UpdateCurrentHealth(stats.MaxHealth);
        }

        // TODO: DELETE
        //test bar
        currentHealth -= Time.deltaTime * 0.1f;

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        
    }
    
    void Die()
    {
        Destroy(transform.root.gameObject);
    }
}