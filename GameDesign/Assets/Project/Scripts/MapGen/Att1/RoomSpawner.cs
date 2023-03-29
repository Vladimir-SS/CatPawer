using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public bool spawned = false;
    private bool hitAreaChecker = false;
    private Vector3 spawnPosition;

    private RoomTemplates templates;
    private GameObject room = null;
    private GameObject placedPrefab;

    private RoomSpawnChecker[] placedRoomSpawnCheckers;
    private RoomSpawnAreaChecker placedRoomSpawnAreaChecker;

    void Start()
    {
        //StartCoroutine(SpawnRoom());
    }

    public IEnumerator SpawnRoom()
    {
        // if & delay ARE necessary in this order!
        // Otherwise, single-door rooms (that are not starters) might "ask" for a room to spawn despite being unable to do so
        // removes the current room if the spawn point hit a areaChecker but not a spawnChecher:
        // the spawn point did not match to another door & is currently placed in a wall somewhere
        yield return new WaitForSeconds(0.1f);
        if (spawned)
            yield break;

        templates = GameObject.Find("Room Templates").GetComponent<RoomTemplates>();
        while (room == null)
        {
            yield return new WaitForSeconds(0.1f);
            room = templates.GetNextRoom(); 
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
        {
            if (!this.spawned)
            {
                Destroy(placedPrefab);
                templates.ReQueueRoom(room);
                yield return null;
                yield return null;
                yield return null;
                this.room = null;
                StartCoroutine(SpawnRoom());
            }
            yield break;
        }
            

        for (int angle = 90; angle <= 360; angle += 90)
        {
            placedPrefab = Instantiate(room, this.spawnPosition, Quaternion.identity);
            placedPrefab.transform.RotateAround(this.transform.position, Vector3.up, angle);

            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
 
            if (IsPrefabStillPlaced(placedPrefab) && ProperSpawnPointPlacement(placedPrefab))
            {
                
                this.spawned = true;
                MakePrefabUnremovable(placedPrefab);
                yield break;
            }
            else
                this.spawned = false;
        }
        if (!this.spawned)
        {
            StartCoroutine(TryPlacePrefab(room, ++roomSpawnCheckerIndex));
        }
            
    }

    bool IsPrefabStillPlaced(GameObject placedPrefab)
    {
        // try-catch IS necessary
        try
        {  
            return placedPrefab.activeSelf;
        }
        catch 
        {
            return false;
        }
        
    }

    private void MakePrefabUnremovable(GameObject prefab)
    {
        RoomSpawnAreaChecker script = prefab.GetComponentInChildren<RoomSpawnAreaChecker>();
        script.removable = false;
    }

    private bool ProperSpawnPointPlacement(GameObject prefab)
    {
        RoomSpawner[] placedPrefabSpawners;
        placedPrefabSpawners = prefab.GetComponentsInChildren<RoomSpawner>();
        foreach (RoomSpawner rs in placedPrefabSpawners)
            if (rs.hitAreaChecker && !rs.spawned)
                return false;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //prevents rooms trying to spawn  on spawners that are matched to spawnCheckers
        if (other.CompareTag("RoomSpawnChecker"))
        {
            this.spawned = true;
        }
        if (other.CompareTag("RoomSpawnAreaChecker"))
        {
            this.hitAreaChecker = true;
        }
    }

    private void TryWithNewPrefab()
    {

    }
}
