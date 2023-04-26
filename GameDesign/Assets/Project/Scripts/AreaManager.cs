using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private GameObject trashbin;
    [SerializeField] private GameObject EnemyHealthBarPrefab;
    
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnPoints;


    void Start()
    {
        SpawnEnemies();
    }

    void AddHealthBar(GameObject go)
    {
        GameObject body = go.transform.Find("Body").gameObject;

        var height = body.GetComponent<Collider>().bounds.size.y;
        Vector3 offset = new (0, height, 0);

        Instantiate(EnemyHealthBarPrefab, body.transform.position + offset, Quaternion.identity, body.transform);
    }

    void SpawnEnemies()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            var enemy = Instantiate(enemyPrefabs[randomIndex], spawnPoint.transform.position, Quaternion.identity);
            AddHealthBar(enemy);
        }
    }

    void Update()
    {
        if (AreAllEnemiesDead())
        {
            SpawnLoot();
            OpenDoor();
        }
    }

    bool AreAllEnemiesDead()
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
