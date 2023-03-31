using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject startRoom;
    public List<GameObject> placeableRooms;

    private Queue<Tuple<RoomParameters, int>> multipleDoorRooms = new Queue<Tuple<RoomParameters, int>>();
    private Queue<Tuple<RoomParameters, int>> specialRooms = new Queue<Tuple<RoomParameters, int>>();
    private Queue<Tuple<RoomParameters, int>> singleDoorRooms = new Queue<Tuple<RoomParameters, int>>();

    private Queue<DoorMarker> openDoors = new Queue<DoorMarker>();
    private List<RoomPlacingData> placingData;
    
    private IEnumerator Start()
    {
        yield return StartCoroutine(InstantiateAll());
        PrepareData();
        //ValidateData();       //not rn
        BuildLayout();

    }

    private IEnumerator InstantiateAll()
    {
        startRoom = Instantiate(startRoom, this.transform.position, Quaternion.identity);

        for (int i = 0; i<placeableRooms.Count; i++)
        {
            placeableRooms[i] = Instantiate(placeableRooms[i].gameObject, this.transform.position, Quaternion.identity);
            //placeableRooms[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.2f);
    }

    private void PrepareData()
    {
        //sort out the rooms
        RoomParameters script;
        for(int i=0;i<placeableRooms.Count;i++)
        {
            script = placeableRooms[i].GetComponent<RoomParameters>();
            if (script.isSpecialRoom)
            {
                specialRooms.Enqueue(new Tuple<RoomParameters, int>(script, i));
            }
            else if (script.DoorMarkers.Length > 1)
            {
                multipleDoorRooms.Enqueue(new Tuple<RoomParameters, int>(script, i));
            }
            else
            {
                singleDoorRooms.Enqueue(new Tuple<RoomParameters, int>(script, i));
            }
        }
    }

    private void ValidateData()
    {
        //check that we have all we need
        if (multipleDoorRooms.Count < 3)
        {
            Debug.LogException(new MissingComponentException("Not enough multiple door rooms provided in MapGenerator > placeableRooms!"));
        }
        if (singleDoorRooms.Count == 0)
        {
            Debug.LogException(new MissingComponentException("No single-door room provided in MapGenerator > placeableRooms!"));
        }
        /*
         while(multipleDoorRooms.Count > 0)
        {
            Tuple<RoomParameters, int> t = multipleDoorRooms.Dequeue();
            Debug.Log("mult:" + t.Item1.name + " - " + t.Item2);
        }
        while (singleDoorRooms.Count > 0)
        {
            Tuple<RoomParameters, int> t = singleDoorRooms.Dequeue();
            Debug.Log("single:" + t.Item1.name + " - " + t.Item2);
        }
        */
    }

    private void PlaceRegularRoomCollection(Queue<Tuple<RoomParameters, int>> rooms)
    {
        DoorMarker currentDoor = openDoors.Dequeue();
        while(rooms.Count > 0)
        {
            //who gets requeued? rooms or doors
            //
        }
    }

    private void BuildLayout()
    {
        int maxReQueues = 5;
        float elevation = this.transform.position.y;
        //set the starter room at the position of this
        openDoors = new Queue<DoorMarker>(startRoom.GetComponent<RoomParameters>().DoorMarkers);
        placingData.Add(new RoomPlacingData(-1, openDoors.Peek(), 0));

        Queue<Tuple<RoomParameters, int>> firstRoomCollection = multipleDoorRooms;
        Queue<Tuple<RoomParameters, int>> secondRoomCollection = specialRooms;
        Queue<Tuple<RoomParameters, int>> finalRoomCollection = singleDoorRooms;

        //sets multiple door rooms
        while (multipleDoorRooms.Count > 0)
        {
            /*
            TryPlaceRoom();
            if (placed)
                addopenDoors;
            else
            {
                openDoors.Enqueue(openDoors.Dequeue());
                multipleDoorRooms.Enqueue(multipleDoorRooms.Dequeue());
            }
            */
        }
    }

    private void GetLayout()
    {
        //Instantiate(placeablePrefabs[0], this.transform.position, Quaternion.identity);
        //place starter room at this.position :) 
        //for (i=1; I, placingData....) -> place all other prefabs
        //placingData[0] = new RoomPlacingData(-1, starterRoom.DoorMarkers[0], 0);
        //RoomParameters script = starterRoom.GetComponent<RoomParameters>();
        //print(script.DoorMarkers[0]);
        //float elevation = this.transform.position.y;

        //ist<DoorMarker> openDoors = new List<DoorMarker>(starterRoom.DoorMarkers);
        //place multiple-door rooms

    }
}
