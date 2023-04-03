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

    /*
     placeableRooms[roomIndex].transform.position = location.transform.position - room.DoorMarkers[i].transform.localPosition;
    placeableRooms[roomIndex].transform.RotateAround(location.transform.position, Vector3.up, angle);
     */

    private void TransposeRoomToLocation(Vector3[] corners, DoorMarker location, RoomParameters room, int doorIndex)
    {
        Vector3 v = new Vector3(location.transform.position.x - room.DoorMarkers[doorIndex].transform.position.x, location.transform.position.y, location.transform.position.z - room.DoorMarkers[doorIndex].transform.position.z);
        room.DoorMarkers[doorIndex].transform.position += v;
        Vector3 delta = new Vector3(location.transform.position.x - room.DoorMarkers[doorIndex].transform.position.x, location.transform.position.y, location.transform.position.z - room.DoorMarkers[doorIndex].transform.position.z);

        for (int j = 0; j < 4; j++)
        {
            corners[j].x = corners[j].x + delta.x;
            corners[j].z = corners[j].z + delta.z;
        }
    }

    private bool AbleToPlaceRoom(DoorMarker location, RoomParameters room, int roomIndex)
    {
        Vector3[] corners = room.Corners;
        placeableRooms[roomIndex].SetActive(true);
        for (int i = 0; i < room.DoorMarkers.Length; i++)
        {
            TransposeRoomToLocation(corners, location, room, i);
            
            for (int angle = 0; angle <= 360; angle += 90)
            {
                RotateRoomCounterClockwise(location, room);
                //if(doesn collide && doors are properly placed)
                

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
            placeableRooms[i] = Instantiate(placeableRooms[i].gameObject, this.transform.position + new Vector3(10, 1, 110), Quaternion.identity);
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
}
