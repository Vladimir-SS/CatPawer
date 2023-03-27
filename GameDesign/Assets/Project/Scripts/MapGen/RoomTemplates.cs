using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    //public List<GameObject> mandatoryMultipleDoorRooms;
    public List<GameObject> mandatorySingleDoorRooms;
    public List<GameObject> optionalSingleDoorRooms;

    public static object lockMandatoryMultipleDoorRooms = new object();

    private Queue<GameObject> mandatorySingleDoorRoomsQueue;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
        mandatorySingleDoorRoomsQueue = new Queue<GameObject>(mandatorySingleDoorRooms);
    }

    public GameObject GetNextGameObject()
    {
        // delaying does not seem necessary anymore
        if (Time.time - startTime >= 0.5f)
        {
            startTime = Time.time;
            lock (lockMandatoryMultipleDoorRooms)                   // to remmove??
            {
                if (mandatorySingleDoorRoomsQueue.Count > 0)
                {
                    return mandatorySingleDoorRoomsQueue.Dequeue();
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
}
