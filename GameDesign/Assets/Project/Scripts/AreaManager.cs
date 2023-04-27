using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private GameObject trashbin;
    [SerializeField] private GameObject spawnPoints;
    [SerializeField] private GameObject EnemyHealthBarPrefab;
    
    public GameObject[] enemyPrefabs;
    private bool enemiesSpawned = false;
    [SerializeField] private int difficulty = 1; // 0 - easy, 1 - normal, 2 - hard

    // Percentage of the default stats that will be added or removed from the enemy per difficulty level
    [SerializeField] private float healthBuffPercentage = 0.5f;
    [SerializeField] private float speedBuffPercentage = 0.1f;
    [SerializeField] private float damageBuffPercentage = 0.25f;

    void Update()
    {
        if (enemiesSpawned)
            if (CheckIfAllEnemiesAreDead())
            {
                SpawnLoot();
                OpenDoor();
            }

    }

    void AddHealthBar(GameObject go)
    {
        GameObject body = go.transform.Find("Body").gameObject;

        var height = body.GetComponent<Collider>().bounds.size.y;
        Vector3 offset = new (0, height, 0);

        Instantiate(EnemyHealthBarPrefab, body.transform.position + offset, Quaternion.identity, body.transform);
    }

    public void SetDifficutly(int diff)
    { difficulty = diff; }
    
    public void SpawnEnemies()
    {
        foreach (Transform child in spawnPoints.transform)
        {

            if (child != null)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                var enemy = Instantiate(enemyPrefabs[randomIndex], child.transform.position, Quaternion.identity);
                AddHealthBar(enemy);

                var defaultStats = enemy.gameObject.transform.Find("Body").GetComponent<StatsEntity>();
                Debug.Log(defaultStats.MaxHealth);
                // create item 
                ItemStats itemStats = enemy.AddComponent<ItemStats>();

                //itemStats.MaxHealth = defaultStats.MaxHealth * (difficulty - 1) * healthBuffPercentage;
                //itemStats.DamagePercentageBase = defaultStats.DamagePercentageBase * (difficulty - 1) * damageBuffPercentage;
                //itemStats.MoveSpeed = defaultStats.MoveSpeed * (difficulty - 1) * speedBuffPercentage;
                //itemStats.Armor = difficulty == 3 ? defaultStats.Armor + 100 : defaultStats.Armor;
            }
        }
        enemiesSpawned = true;
    }

    bool CheckIfAllEnemiesAreDead()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }

    void SpawnLoot()
    {
        trashbin.SetActive(true);
    }

    void OpenDoor()
    {
        // Open door
    }
}
