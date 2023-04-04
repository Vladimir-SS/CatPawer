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
        PrepareData();
        //ValidateData();       //not rn
        BuildLayout();

    }

    private void RotateRoomClockwise(DoorMarker pivotPoint, RoomParameters room)
    {
        float aux;
        for (int i = 0; i < 4; i++)
        {
            room.Corners[i] -= pivotPoint.transform.position;

            aux = room.Corners[i].x;
            room.Corners[i].x = room.Corners[i].z;
            room.Corners[i].z = -1 * aux;

            room.Corners[i] += pivotPoint.transform.position;
        }
    }

    /*
     placeableRooms[roomIndex].transform.position = location.transform.position - room.DoorMarkers[i].transform.localPosition;
    placeableRooms[roomIndex].transform.RotateAround(location.transform.position, Vector3.up, angle);
    foreach(Vector3 d in room.Corners)
            {
                GameObject emptyGameObject = new GameObject("EmptyGameObject");
                emptyGameObject.transform.position = d;
            }
     */

    private IEnumerator TransposeRoomToInitialLocation(DoorMarker location, RoomParameters room, int doorIndex, int roomIndex)
    {
        room.DoorMarkers[doorIndex].transform.position = location.transform.position;
        placeableRooms[roomIndex].transform.position = location.transform.position - room.DoorMarkers[doorIndex].transform.localPosition;
        //placeableRooms[roomIndex].transform.RotateAround(location.transform.position, Vector3.up, 90);
        yield return null;
    }

    //needed to check if two rooms collide
    private Tuple<Vector3, Vector3> GetExtemeCornerCoordinates(RoomParameters room)
    {
        Vector3 minPoints = new Vector3(Mathf.Infinity, room.Corners[0].y, Mathf.Infinity);
        Vector3 maxPoints = new Vector3(Mathf.NegativeInfinity, room.Corners[0].y, Mathf.NegativeInfinity);
        for (int i = 0; i < room.Corners.Length; i++)
        {
            Vector3 corner = room.Corners[i];
            if (corner.x < minPoints.x)
            {
                minPoints.x = corner.x;
            }
            if(corner.x > maxPoints.x)
            {
                maxPoints.x = corner.x;
            }
            if (corner.z < minPoints.z)
            {
                minPoints.z = corner.z;
            }
            if (corner.z > maxPoints.z)
            {
                maxPoints.z = corner.z;
            }
        }
        return new Tuple<Vector3, Vector3>(minPoints, maxPoints);
    }

    //should still return False if only their edges collide
    private bool RoomsCollide(RoomParameters room1, RoomParameters room2)
    {
        (Vector3 minPoints1, Vector3 maxPoints1) = GetExtemeCornerCoordinates(room1);
        (Vector3 minPoints2, Vector3 maxPoints2) = GetExtemeCornerCoordinates(room2);
        if ((minPoints1.x < maxPoints2.x) && (minPoints2.x < maxPoints1.x))
        {
            if ((minPoints1.z < maxPoints2.z) && (minPoints2.z < maxPoints1.z))
            {
                return true;
            }
        }
        
        return false;
    }

    private bool CollidesWithAnyRoom(RoomParameters room)
    {
        //check for collisions with starter
        if (RoomsCollide(startRoom.GetComponent<RoomParameters>(), room))
        {
            //Debug.Log("Collided with starter");
            return true;
        }
        //check for collisions with the rest of the rooms    
        for(int i = 1; i < placingData.Count; i++)
        {
            if (RoomsCollide(placeableRooms[placingData[i].PlaceableRoomsIndex].GetComponent<RoomParameters>(), room))
            {
                //Debug.Log("Collided with room of index:" + placingData[i].PlaceableRoomsIndex);
                return true;
            }
        }
        //Debug.Log("no collisions");
        return false;
    }

    private bool AbleToPlaceRoom(DoorMarker location, RoomParameters room, int roomIndex)
    {
        Vector3[] corners = room.Corners;
        placeableRooms[roomIndex].SetActive(true);
        for (int i = 0; i < room.DoorMarkers.Length; i++)
        {
            StartCoroutine(TransposeRoomToInitialLocation(location, room, i, roomIndex));
            room.SetCorners();

            for (int angle = 0; angle < 360; angle += 90)
            {
                Debug.Log("angle " + angle + " Room coll:" + CollidesWithAnyRoom(room));
                RotateRoomClockwise(location, room);
                //placeableRooms[roomIndex].transform.RotateAround(location.transform.position, Vector3.up, 270);
                //if(doesn collide && doors are properly placed)


                //check collisions
                //check to have valid doors
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
        //placingData.Add(new RoomPlacingData(-1, openDoors.Peek(), 0));      // -1: starter room is already in position

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
}
