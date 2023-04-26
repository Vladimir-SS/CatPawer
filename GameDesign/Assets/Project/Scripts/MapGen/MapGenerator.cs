using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //allows rooms to be placed even if they slightly overlap. reduces headaches with doorMarker placement
    public float collisionTollerance;
    public int seed = 123;
    public GameObject startRoom;
    public List<GameObject> placeableRooms;

    private List<Tuple<RoomParameters, int>> multipleDoorRooms = new List<Tuple<RoomParameters, int>>();
    private List<Tuple<RoomParameters, int>> specialRooms = new List<Tuple<RoomParameters, int>>();
    private List<Tuple<RoomParameters, int>> singleDoorRooms = new List<Tuple<RoomParameters, int>>();

    private LinkedList<DoorMarker> openDoors = new LinkedList<DoorMarker>();
    private List<RoomPlacingData> placingData = new List<RoomPlacingData>();
    
    private IEnumerator Start()
    {
        yield return StartCoroutine(InstantiateAll());
        PrepareData();
        //ValidateData();     
        if (BuildLayout())
        {
            ShowRooms();
            TryCleanup();  // should delete the doormarkers too, but not yet
        }
    }

    private void RotateRoomClockwise(DoorMarker pivotPoint, RoomParameters room)
    {
        float aux;
        Vector3 p;
        for (int i = 0; i < 4; i++)
        {
            room.Corners[i] -= pivotPoint.transform.position;

            aux = room.Corners[i].x;
            room.Corners[i].x = room.Corners[i].z;
            room.Corners[i].z = -1 * aux;

            room.Corners[i] += pivotPoint.transform.position;
        }
        for (int i=0;i<room.DoorMarkers.Length; i++)
        {
            room.DoorMarkers[i].Position -= pivotPoint.transform.position;

            p = new Vector3(room.DoorMarkers[i].Position.z, room.DoorMarkers[i].Position.y, -1 * room.DoorMarkers[i].Position.x);
            room.DoorMarkers[i].Position = p;

            room.DoorMarkers[i].Position += pivotPoint.transform.position;
        }
    }

    private IEnumerator TransposeRoomToInitialLocation(DoorMarker location, RoomParameters room, int doorIndex, int roomIndex)
    {
        placeableRooms[roomIndex].transform.position = location.transform.position - room.DoorMarkers[doorIndex].transform.localPosition;
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
        if ((minPoints1.x < maxPoints2.x - collisionTollerance) && (minPoints2.x < maxPoints1.x - collisionTollerance))
        {
            if ((minPoints1.z < maxPoints2.z - collisionTollerance) && (minPoints2.z < maxPoints1.z - collisionTollerance))
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
                Debug.Log("Collided with room of index:" + placingData[i].PlaceableRoomsIndex);
                return true;
            }
        }
        //Debug.Log("no collisions");
        return false;
    }

    private Vector3[] GetDoorInitialPositions(RoomParameters room, int roomIndex)
    {
        Vector3[] rez = new Vector3[room.DoorMarkers.Length];
        for (int k = 0; k < room.DoorMarkers.Length; k++)
        {
            rez[k] = placeableRooms[roomIndex].GetComponentInChildren<RoomParameters>().DoorMarkers[k].transform.position;
        }
        
        return rez;
    }

    private bool PrefabIsProperlyDistanced(DoorMarker currentDoor, Vector3 maxCheckerPoint, Vector3 minCheckerPoint, RoomParameters placedRoomScript)
    {
        (Vector3 minPoints, Vector3 maxPoints) = GetExtemeCornerCoordinates(placedRoomScript);
        //checks that the door is close to a prefab
        if ((minCheckerPoint.x < maxPoints.x) && (minPoints.x < maxCheckerPoint.x) && (minCheckerPoint.z < maxPoints.z) && (minPoints.z < maxCheckerPoint.z))
        {
            bool isClose = false;
            // if the door is very close to a prefab, it needs to also be close/overlap another door in the prefab
            // if not, the door is badly placed and it would not be possible to place anything between them OR the door would just face a wall
            foreach (DoorMarker placedDoor in placedRoomScript.DoorMarkers)
            {
                if (Vector3.Distance(currentDoor.Position, placedDoor.Position) < collisionTollerance)
                {
                    isClose = true;
                    break;
                }
            }
            if (!isClose)
            {
                return false;
            }
        }
        return true;
    }

    private bool AnyDoorGetsCovered(RoomParameters currentRoom, RoomParameters roomToCheckAgainst)
    {
        List<DoorMarker> doors = new List<DoorMarker>(roomToCheckAgainst.GetComponent<RoomParameters>().DoorMarkers);
        Vector3 maxCheckerPoint, minCheckerPoint;
        foreach (DoorMarker door in doors)
        {
            maxCheckerPoint = door.Position + new Vector3(2.0f, door.Position.y, 2.0f);
            minCheckerPoint = door.Position + new Vector3(-2.0f, door.Position.y, -2.0f);
            if (!PrefabIsProperlyDistanced(door, maxCheckerPoint, minCheckerPoint, currentRoom))
                return true;
        }
        return false;
    }

    private bool PrefabDoesntSpawnOverDoor(RoomParameters currentRoom)
    {
        if (AnyDoorGetsCovered(currentRoom, startRoom.GetComponent<RoomParameters>()))
            return false;

        for (int i = 1; i < placingData.Count; i++)
        {
            RoomParameters placedRoom = placeableRooms[placingData[i].PlaceableRoomsIndex].GetComponent<RoomParameters>();
            if (AnyDoorGetsCovered(currentRoom, placedRoom))
                return false;
        }
        return true;
    }

    //checks that the doors are not placed too close to other prefabs, but allows doors to overlap
    private bool DoorsAreProperlyPlaced(RoomParameters currentRoom)
    {
        List<DoorMarker> doors = new List<DoorMarker>(currentRoom.DoorMarkers);
        Vector3 maxCheckerPoint, minCheckerPoint;
        foreach(DoorMarker door in doors)
        {
            maxCheckerPoint = door.Position + new Vector3(2.0f, door.Position.y, 2.0f);
            minCheckerPoint = door.Position + new Vector3(-2.0f, door.Position.y, -2.0f);

            if (!PrefabIsProperlyDistanced(door, maxCheckerPoint, minCheckerPoint, startRoom.GetComponent<RoomParameters>()))
                return false;

            for (int i = 1; i < placingData.Count; i++)
            {
                if (!PrefabIsProperlyDistanced(door, maxCheckerPoint, minCheckerPoint, placeableRooms[placingData[i].PlaceableRoomsIndex].GetComponent<RoomParameters>()))
                    return false;
            }
        }
        if(PrefabDoesntSpawnOverDoor(currentRoom))
            return true;
        return false;
    }

    private List<DoorMarker> RemoveClosedDoors(List<DoorMarker> doors)
    {
        for (var node = openDoors.First; node != null; node = node.Next)
        {
            for (int i = 0; i < doors.Count; i++)
            {
                //if(node.Value.Position == doors[i].Position)
                if ((Vector3.Distance(node.Value.Position, doors[i].Position) < collisionTollerance) || (node.Value.Position == doors[i].Position))
                {
                    openDoors.Remove(node);
                    doors.RemoveAt(i);
                }
            }
        }
        return doors;
    }

    private void UpdateOpenDoors(RoomParameters placedRoom)
    {
        List<DoorMarker> doors = new List<DoorMarker>(placedRoom.DoorMarkers);
        doors = RemoveClosedDoors(doors);
        for (int i = 0; i < doors.Count; i++)
        {
            openDoors.AddLast(doors[i]);
        }
    }

    private bool AbleToPlaceRoom(DoorMarker location, RoomParameters room, int roomIndex)
    {
        for (int i = 0; i < room.DoorMarkers.Length; i++)
        {
            
            StartCoroutine(TransposeRoomToInitialLocation(location, room, i, roomIndex));
            
            room.SetCorners();

            room.SetDoors(GetDoorInitialPositions(room, roomIndex));
            
            for (int angle = 0; angle < 360; angle += 90)
            {
                //Debug.Log(CollidesWithAnyRoom(room));
                //Debug.Log(DoorsAreProperlyPlaced(room));              
                if (!CollidesWithAnyRoom(room) && DoorsAreProperlyPlaced(room))
                {
                    placingData.Add(new RoomPlacingData(roomIndex, room.DoorMarkers[i], angle));
                    
                    placeableRooms[roomIndex].transform.RotateAround(room.DoorMarkers[i].Position, Vector3.up, angle);
                    return true;
                }
                RotateRoomClockwise(location, room);
            }
        }
        return false;
    }

    private bool PlaceRegularRoomCollection(Queue<Tuple<RoomParameters, int>> rooms)
    {
        //all this room/door swapping was not tested, but it should help place rooms on any door
        int nrRoomReQueues = 0, nrDoorReQueues = 0;
        if (openDoors.Count == 0)
            return false;
        DoorMarker currentDoor = openDoors.First.Value;
        while (rooms.Count > 0)
        {
            Tuple<RoomParameters, int> r = rooms.Dequeue();
            if (AbleToPlaceRoom(currentDoor, r.Item1, r.Item2))
            {
                UpdateOpenDoors(r.Item1);
                currentDoor = openDoors.First.Value;
                nrRoomReQueues = 0;
                nrDoorReQueues = 0;
            }
            else
            {
                Debug.Log("req:" + r.Item1.name);
                if (nrDoorReQueues >= openDoors.Count + 1)
                    return false;
                if (nrRoomReQueues >= rooms.Count + 1)
                {
                    openDoors.RemoveFirst();
                    openDoors.AddLast(currentDoor);
                    currentDoor = openDoors.First.Value;
                    nrDoorReQueues++;
                }
                else
                {
                    nrRoomReQueues++;
                    rooms.Enqueue(r);
                }
            }
        }
        return true;
    }

    private bool PlaceSpecialRooms(Queue<Tuple<RoomParameters, int>> rooms)
    {
        if (openDoors.Count == 0)
            return false;
        DoorMarker currentDoor = openDoors.Last.Value;
        int doorReQueues = 0;
        try
        {
            while (rooms.Count > 0)
            {
                Tuple<RoomParameters, int> r = rooms.Peek();
                if (AbleToPlaceRoom(currentDoor, r.Item1, r.Item2))
                {
                    rooms.Dequeue();
                    doorReQueues = 0;
                }
                else
                {
                    doorReQueues++;
                    openDoors.AddFirst(currentDoor);
                }
                if (doorReQueues >= rooms.Count + 1)
                    return false;
                openDoors.RemoveLast();
                currentDoor = openDoors.Last.Value;
            }
        }
        catch (NullReferenceException)
        {

        }
        return true;
    }

    private bool AbleToPlaceEndRoom(DoorMarker location, RoomParameters room, int roomIndex)
    {
        //it sure is cool that i gotta use another funct to do pretty much what another one does :] 
        room.IniDoors();
        StartCoroutine(TransposeRoomToInitialLocation(location, room, 0, roomIndex));
        room.SetCorners();
        room.SetDoors(GetDoorInitialPositions(room, roomIndex));

        for (int angle = 0; angle < 360; angle += 90)
        {
            if (!CollidesWithAnyRoom(room) && DoorsAreProperlyPlaced(room))
            {
                placingData.Add(new RoomPlacingData(roomIndex, room.DoorMarkers[0], angle));

                placeableRooms[roomIndex].transform.RotateAround(location.Position, Vector3.up, angle);
                return true;
            }
            RotateRoomClockwise(location, room);
        }
        return false;
    }

    private bool PlacePatchingRooms(List<Tuple<RoomParameters, int>> rooms)
    {
        bool placed;
        for (LinkedListNode<DoorMarker> node = openDoors.First; node != null; node = node.Next)
        {
            placed = false;
            ShuffleList(rooms);
            for(int i = 0; i < rooms.Count; i++)
            {
                Tuple<RoomParameters, int> currentRoom = rooms[i];
                GameObject clone = Instantiate(placeableRooms[currentRoom.Item2], this.transform.position, Quaternion.identity);
                placeableRooms.Add(clone);

                RoomParameters script = clone.GetComponent<RoomParameters>();

                if (AbleToPlaceEndRoom(node.Value, script, placeableRooms.Count - 1))
                {
                    placed = true;
                    break;
                }
            }
            if (!placed)
                return false;
        }
        return true;
    }

    // in case the placing of a prefab covers more than one door, in open doors there will be added two doors that are covered
    private void FilterOpenDoors()
    {
        for (var node1 = openDoors.First; node1 != null; node1 = node1.Next)
        {
            for (var node2 = node1.Next; node2 != null; node2 = node2.Next)
            {
                if (Vector3.Distance(node1.Value.Position, node2.Value.Position) < collisionTollerance)
                {
                    openDoors.Remove(node1);
                    openDoors.Remove(node2);
                }
            }
        }
    }

    public void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random(seed);
        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = rng.Next(i + 1);
            T temp = list[j];
            list[j] = list[i];
            list[i] = temp;
        }
    }

    private bool BuildLayout()
    {
        //set the starter room at the position of this
        DoorMarker[] d = startRoom.GetComponent<RoomParameters>().DoorMarkers;
        for(int i = 0; i < d.Length; i++)
        {
            d[i].Position = startRoom.GetComponent<RoomParameters>().DoorMarkers[i].transform.position;
            openDoors.AddLast(d[i]);
        }
        placingData.Add(new RoomPlacingData(-1, openDoors.First.Value, 0));      // -1: starter room is already in position

        Queue<Tuple<RoomParameters, int>> firstRoomCollection = new Queue<Tuple<RoomParameters, int>>();
        Queue<Tuple<RoomParameters, int>> secondRoomCollection = new Queue<Tuple<RoomParameters, int>>();
        List<Tuple<RoomParameters, int>> finalRoomCollection = singleDoorRooms;

        List<RoomPlacingData> prevPlacingData = new List<RoomPlacingData>();
        LinkedList<DoorMarker> prevOpenDoors = new LinkedList<DoorMarker>();

        //hope y'all like noodles
        int maxAtt = 4;
        for(int att1=0;att1<maxAtt;att1++)
        {
            //shuflle first
            ShuffleList(multipleDoorRooms);
            firstRoomCollection = new Queue<Tuple<RoomParameters, int>>(multipleDoorRooms);
            if (!PlaceRegularRoomCollection(firstRoomCollection)) 
                continue;
            prevPlacingData = placingData;
            prevOpenDoors = openDoors;
            FilterOpenDoors();
            for (int att2 = 0; att2 < maxAtt; att2++)
            {
                //shuffle second 
                ShuffleList(specialRooms);
                secondRoomCollection = new Queue<Tuple<RoomParameters, int>>(specialRooms);
                prevPlacingData = placingData;
                prevOpenDoors = openDoors;
                if (!PlaceSpecialRooms(secondRoomCollection))
                    continue;
                for (int att3 = 0; att3 < maxAtt; att3++)
                {
                    //these get shuffled somewhere else
                    if (PlacePatchingRooms(finalRoomCollection))
                        return true;
                }
                placingData = prevPlacingData;
                openDoors = prevOpenDoors;
            }
            placingData = prevPlacingData;
            openDoors = prevOpenDoors;
        }
        return false;
    }

    private void ShowRooms()
    {
        for(int i = 1; i < placingData.Count; i++)
        {
            placeableRooms[placingData[i].PlaceableRoomsIndex].SetActive(true);
        }
    }

    // returns false if there are multiple door rooms not set as active;
    // if there are multiple door rooms not placed, layout builder failed;
    // there should always be single-door rooms to remove
    private bool TryCleanup()
    {
        //removes extra finishing rooms
        for(int i = 0; i < placeableRooms.Count; i++)
        {
            if (!placeableRooms[i].activeSelf)
            {
                //if (placeableRooms[i].GetComponent<RoomParameters>().DoorMarkers.Length > 1)
                 //   return false;
                GameObject.Destroy(placeableRooms[i]);
            }
        }

        multipleDoorRooms.Clear();
        multipleDoorRooms = null;
        specialRooms.Clear();
        specialRooms = null;
        singleDoorRooms.Clear();
        singleDoorRooms = null;

        openDoors.Clear();
        openDoors = null;

        return true;
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
        multipleDoorRooms = new List<Tuple<RoomParameters, int>>();
        specialRooms = new List<Tuple<RoomParameters, int>>();
        singleDoorRooms = new List<Tuple<RoomParameters, int>>();
        for(int i=0;i<placeableRooms.Count;i++)
        {
            placeableRooms[i].SetActive(false);
            script = placeableRooms[i].GetComponent<RoomParameters>();
            if (script.isSpecialRoom)
            {
                specialRooms.Add(new Tuple<RoomParameters, int>(script, i));
            }
            else if (script.DoorMarkers.Length > 1)
            {
                multipleDoorRooms.Add(new Tuple<RoomParameters, int>(script, i));
            }
            else
            {
                singleDoorRooms.Add(new Tuple<RoomParameters, int>(script, i));
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
    }
}
