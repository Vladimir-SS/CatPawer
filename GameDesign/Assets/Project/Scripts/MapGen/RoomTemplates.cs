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

    void Start()
    {
        mandatorySingleDoorRoomsQueue = new Queue<GameObject>(mandatorySingleDoorRooms);
    }

    public GameObject GetNextGameObject()
    {
        lock (lockMandatoryMultipleDoorRooms)
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
}
