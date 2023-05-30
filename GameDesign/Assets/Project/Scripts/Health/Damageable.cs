using System;
using System.Xml.Serialization;
using UnityEngine;
using Stats;
using Stats.Structs;
using System.Collections.Generic;
using Random = System.Random;

[RequireComponent(typeof(StatsEntityFinal))]
public class Damageable : MonoBehaviour
{
    private float currentHealth = 0;

    [SerializeField] AudioSource soundEffects;

    [SerializeField] List<AudioClip> damageSounds;
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
        if (soundEffects && !soundEffects.isPlaying)
        {
            Random random = new();
            soundEffects.clip = damageSounds[random.Next(damageSounds.Count - 1)];
            soundEffects.Play();
        }

        currentHealth -= Math.Max(1, damage - damage * stats.Body.DamageReduction);
        Debug.Log("Taking damage, current health : " + currentHealth + " max HP : " + stats.Body.MaxHP + " damage : " + damage);


        if (currentHealth <= 0)
        {
            Die();
        }
        
    }
    
    void Die()
    {
        if (soundEffects)
        {
            soundEffects.clip = damageSounds[damageSounds.Count - 1];
            soundEffects.Play();
        }

        if (gameObject.CompareTag("Enemy"))
        {
            ScoreSystem.instance.AddPoints(stats.Body.MaxHP / 10);
            Destroy(transform.root.gameObject);
        }
        
        if (gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<PauseMenu>().TryAgainMenu();
        }

        // Debug.Log("Just died damn");
    }

    private void OnParticleCollision(GameObject other)
    {
        TakeDamage(0.05f);
    }
}