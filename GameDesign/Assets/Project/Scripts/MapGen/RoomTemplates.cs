using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public List<GameObject> mandatoryMultipleDoorRooms;
    public List<GameObject> mandatorySingleDoorRooms;
    public List<GameObject> optionalSingleDoorRooms;

    public static object lockQueues = new object();

    private Queue<GameObject> multipleDoorRoomsQueue;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
        multipleDoorRoomsQueue = new Queue<GameObject>(mandatoryMultipleDoorRooms);
    }

    public GameObject GetNextRoom()
    {
        // delaying does not seem necessary anymore
        if (Time.time - startTime >= 0.5f)
        {
            startTime = Time.time;
            lock (lockQueues)                   // to remmove??
            {
                if (multipleDoorRoomsQueue.Count > 0)
                {
                    return multipleDoorRoomsQueue.Dequeue();
                }
                else
                {
                    return null;
                }
            }
        }
        else
            return null;
    }

    public void ReQueueRoom(GameObject room)
    {
        print("req");
        multipleDoorRoomsQueue.Enqueue(room);
    }
}
