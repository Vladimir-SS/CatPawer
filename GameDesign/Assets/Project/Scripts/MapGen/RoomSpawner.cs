using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public bool spawned = false;
    private Vector3 spawnPosition;

    private RoomTemplates templates;
    private GameObject room = null;
    
    private Component[] placedRoomSpawnCheckers;
    private RoomSpawnAreaChecker placedRoomSpawnAreaChecker;

    void Start()
    {
        StartCoroutine(SpawnRoom());
    }

    public IEnumerator SpawnRoom()
    {
        if (spawned)
            yield break;

        templates = GameObject.Find("Room Templates").GetComponent<RoomTemplates>();
        while(room == null)
        {
            yield return new WaitForSeconds(0.1f);
            room = templates.GetNextGameObject(); 
        }

        placedRoomSpawnCheckers = room.GetComponentsInChildren<RoomSpawnChecker>();
        placedRoomSpawnAreaChecker = room.GetComponentInChildren<RoomSpawnAreaChecker>();

        placedRoomSpawnAreaChecker.removable = true;

        // aligns a spawnchecker from the placeable prefab to this spawn point 
        this.spawnPosition = this.transform.position - placedRoomSpawnCheckers[0].transform.localPosition;
        StartCoroutine(TryPlacePrefab(room, 0));
    }

    IEnumerator TryPlacePrefab(GameObject room, int roomSpawnCheckerIndex)
    {
        if (this.spawned || roomSpawnCheckerIndex > placedRoomSpawnCheckers.Length)
            yield break;

        GameObject placedPrefab;
        for (int angle = 90; angle <= 360; angle += 90)
        {
            placedPrefab = Instantiate(room, this.spawnPosition, Quaternion.identity);
            placedPrefab.transform.RotateAround(this.transform.position, Vector3.up, angle);

            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;

            if (IsPrefabStillPlaced(placedPrefab))
            {
                RoomSpawnAreaChecker script = placedPrefab.GetComponentInChildren<RoomSpawnAreaChecker>();
                script.removable = false;
                break;
            }
        }
       
        if(!this.spawned)
            StartCoroutine(TryPlacePrefab(room, roomSpawnCheckerIndex++));
    }

    bool IsPrefabStillPlaced(GameObject placedPrefab)
    {
        // try-catch IS necessary
        try
        {
            this.spawned = placedPrefab.activeSelf;
        }
        catch (MissingReferenceException)
        {
            this.spawned = false;
        }
        return this.spawned;
    }

    private void OnTriggerEnter(Collider other)
    {
        //prevents rooms trying to spawn  on spawners that are matched to spawnCheckers
        if (other.CompareTag("RoomSpawnChecker"))
        {
            this.spawned = true;
        }
    }
}
