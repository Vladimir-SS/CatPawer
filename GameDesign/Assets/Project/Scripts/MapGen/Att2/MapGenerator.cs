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

    private Queue<DoorMarker> openDoors;
    private List<RoomPlacingData> placingData = new List<RoomPlacingData>();
    
    private IEnumerator Start()
    {
        yield return StartCoroutine(InstantiateAll());
        print("isnt");
        PrepareData();
        print("prepped");
        //ValidateData();       //not rn
        BuildLayout();

    }

    private void RotateRoomCounterClockwise(DoorMarker pivotPoint, RoomParameters room)
    {
        float aux;
        for (int i = 0; i < 4; i++)
        {
            room.Corners[i] -= pivotPoint.transform.position;

            aux = room.Corners[i].y;
            room.Corners[i].y = room.Corners[i].x;
            room.Corners[i].x = -1 * aux;

            room.Corners[i] += pivotPoint.transform.position;
        }
    }

    private bool AbleToPlaceRoom(DoorMarker location, RoomParameters room, int roomIndex)
    {
        for (int i = 0; i < room.DoorMarkers.Length; i++)
        {
            placeableRooms[roomIndex].SetActive(true);
            room.DoorMarkers[i].transform.position = location.transform.position;
            print("after1");
            placeableRooms[roomIndex].transform.position = location.transform.position - room.DoorMarkers[i].transform.localPosition;
            print(placeableRooms[roomIndex].GetComponent<RoomParameters>());
            for (int angle = 90; angle < 360; angle += 90)
            {
                RotateRoomCounterClockwise(location, room);
                //if(doesn collide && doors are properly placed)
                //placedPrefab = Instantiate(room, this.spawnPosition, Quaternion.identity);
               // placeableRooms[roomIndex].transform.position = location.transform.position;

                //placeableRooms[roomIndex].transform.RotateAround(location.transform.position, Vector3.up, angle);

                //check collisions
                //check to have valid doors
                break;
            }
            break;
        }
        return false;
    }

    private void PlaceRegularRoomCollection(Queue<Tuple<RoomParameters, int>> rooms)
    {
        DoorMarker currentDoor = openDoors.Dequeue();
        while (rooms.Count > 0)
        {
            AbleToPlaceRoom(currentDoor, rooms.Dequeue().Item1, rooms.Dequeue().Item2);
            break;
            //TryPlaceRoom(at currentDoor);
            /*
             if(placed)
            {
                add to open doors
                currentDoor =  openDoors.Dequeue();
            }
            else
            {
                check for loops
                requeue room;
            }*/
        }
    }

    private void BuildLayout()
    {
        int maxReQueues = 5;
        float elevation = this.transform.position.y;
        //set the starter room at the position of this

        openDoors = new Queue<DoorMarker>(startRoom.GetComponentInChildren<RoomParameters>().DoorMarkers);
        placingData.Add(new RoomPlacingData(-1, openDoors.Peek(), 0));      // -1: starter room is already in position

        Queue<Tuple<RoomParameters, int>> firstRoomCollection = multipleDoorRooms;
        Queue<Tuple<RoomParameters, int>> secondRoomCollection = specialRooms;
        Queue<Tuple<RoomParameters, int>> finalRoomCollection = singleDoorRooms;

        //sets multiple door rooms
        while (multipleDoorRooms.Count > 0)
        {
            PlaceRegularRoomCollection(multipleDoorRooms);
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
    private IEnumerator InstantiateAll()
    {
        startRoom = Instantiate(startRoom, this.transform.position, Quaternion.identity);

        //startRoom.SetActive(false);

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
            placeableRooms[i].SetActive(false);
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
