using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using static UnityEngine.UI.GridLayoutGroup;

class RoomAttributes
{
    public Vector3 topLeft = new Vector3();
    public Vector3 bottomLeft = new Vector3();
    public Vector3 topRight = new Vector3();
    public Vector3 bottomRight = new Vector3();

    public RoomAttributes(GameObject room, MapGenerator mapGenerator)
    {
        (Vector3 minPoint, Vector3 maxPoint) = mapGenerator.GetExtemeCornerCoordinates(room.GetComponent<RoomParameters>());
        Vector3[] corners = room.GetComponent<RoomParameters>().Corners;
        foreach (Vector3 corner in corners)
        {
            if (corner.x == minPoint.x && corner.z == minPoint.z)
            {
                bottomLeft = corner;
            }
            else if (corner.x == minPoint.x && corner.z == maxPoint.z)
            {
                topLeft = corner;
            }
            else if (corner.x == maxPoint.x && corner.z == minPoint.z)
            {
                bottomRight = corner;
            }
            else if (corner.x == maxPoint.x && corner.z == maxPoint.z)
            {
                topRight = corner;
            }
        }
    }
}


public class WallPlacer : MonoBehaviour
{
    private List<GameObject> rooms;
    private List<RoomAttributes> roomAttributes = new List<RoomAttributes>();

    private int doorSize;

    private GameObject wall;
    private float wallWidth;

    private List<GameObject> placedWalls = new List<GameObject>();
    private MapGenerator mapGenerator;

    private GameObject wallContainer;

    public void PlaceWalls(List<GameObject> rooms, int doorSize, GameObject baseWall)
    {
        this.rooms = rooms;
        this.wall = baseWall;
        this.doorSize = doorSize;

        SetWallSizes();

        mapGenerator = this.GetComponent<MapGenerator>();

        wallContainer = new GameObject("Walls");

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] == null)
                continue;
            roomAttributes.Add(new RoomAttributes(rooms[i], mapGenerator));
        }

        SortRooms();

        List<Vector3> topLeft = new List<Vector3>();
        List<Vector3> topRight = new List<Vector3>();
        List<Vector3> bottomLeft = new List<Vector3>();
        List<Vector3> bottomRight = new List<Vector3>();

        foreach (RoomAttributes room in roomAttributes)
        {
            topLeft.Add(room.topLeft);
            topRight.Add(room.topRight);
            bottomLeft.Add(room.bottomLeft);
            bottomRight.Add(room.bottomRight);
        }

        PlaceWallsAlongXAxis(topLeft, topRight, bottomLeft, bottomRight);
        PlaceWallsAlongZAxis(topLeft, topRight, bottomLeft, bottomRight);

        ClearExtraWalls();
    }

    private bool WallIsOnRoomEdge(GameObject wall, RoomAttributes room)
    {
        Vector3 p = wall.transform.position;
        if (p.x >= room.bottomLeft.x - wallWidth * 3 / 2 && p.x <= room.bottomRight.x + wallWidth / 2 && p.z >= room.bottomLeft.z - wallWidth / 2 && p.z <= room.topLeft.z + wallWidth / 2)
            return true;
        return false;
    }

    private bool IsWallNearDoor(GameObject wall)
    {
        foreach(GameObject room in rooms)
        {
            DoorMarker[] doors = room.GetComponentsInChildren<DoorMarker>();
            foreach(DoorMarker door in doors)
            {
                if(DistanceXZ(wall.transform.position, door.Position) <= doorSize )
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsWallRemovable(GameObject wall)
    {
        foreach (RoomAttributes room in roomAttributes)
        {
            if (WallIsOnRoomEdge(wall, room))
                return false;
        }
        return true;
    }

    private void ClearExtraWalls()
    {
        foreach(GameObject wall in placedWalls)
        {
            if (wall == null)
                continue;
            if (IsWallRemovable(wall) || IsWallNearDoor(wall))
            {
                Destroy(wall);
            }
        }
    }

    private void PlaceWallsAlongZAxis(List<Vector3> topLeft, List<Vector3> topRight, List<Vector3> bottomLeft, List<Vector3> bottomRight)
    {
        Vector3 t1, t2, b1, b2;
        Vector3 axis = new Vector3(0, 0, 1);
        while (true)
        {
            (t1, b1) = GetFurthestPoints(topLeft, bottomLeft, bottomRight, axis);
            (t2, b2) = GetFurthestPoints(topRight, bottomLeft, bottomRight, axis);

            if (DistanceXZ(t1, b1) > DistanceXZ(t2, b2))
            {
                PlaceWallsBetweenPoints(t1, b1);
                topLeft = UpdateCoveredCorners(t1, b1, topLeft);
                topRight = UpdateCoveredCorners(t1, b1, topRight);
            }
            else
            {
                PlaceWallsBetweenPoints(t2, b2);
                topLeft = UpdateCoveredCorners(t2, b2, topLeft);
                topRight = UpdateCoveredCorners(t2, b2, topRight);
            }
            if (topLeft.Count == 0 && topRight.Count == 0)
            {
                break;
            }
        }
    }

    private void PlaceWallsAlongXAxis(List<Vector3> topLeft, List<Vector3> topRight, List<Vector3> bottomLeft, List<Vector3> bottomRight)
    {
        Vector3 l1, r1, l2, r2;
        Vector3 axis = new Vector3(1, 0, 0);
        while (true)
        {
            (l1, r1) = GetFurthestPoints(topLeft, topRight, bottomRight, axis);
            
            (l2, r2) = GetFurthestPoints(bottomLeft, topRight, bottomRight, axis);
            if (DistanceXZ(l1, r1) > DistanceXZ(l2, r2))
            {
                PlaceWallsBetweenPoints(l1, r1);
                topLeft = UpdateCoveredCorners(l1, r1, topLeft);
                bottomLeft = UpdateCoveredCorners(l1, r1, bottomLeft);

            }
            else
            {
                PlaceWallsBetweenPoints(l2, r2);
                topLeft = UpdateCoveredCorners(l2, r2, topLeft);
                bottomLeft = UpdateCoveredCorners(l2, r2, bottomLeft);
            }
            if (topLeft.Count == 0 && bottomLeft.Count == 0)
            {
                break;
            }
        }
    }

    private Tuple<Vector3, Vector3> GetFurthestPoints(List<Vector3> start, List<Vector3> end1, List<Vector3> end2, Vector3 axis)
    {
        (Vector3 l1, Vector3 r1) = GetLongestSegment(start, end1, axis);
        (Vector3 l2, Vector3 r2) = GetLongestSegment(start, end2, axis);

        if (DistanceXZ(l1, r1) > DistanceXZ(l2, r2))
        {
            return new Tuple<Vector3, Vector3>(l1, r1);
        }
        return new Tuple<Vector3, Vector3>(l2, r2);
    }

    private List<Vector3> UpdateCoveredCorners(Vector3 p1, Vector3 p2, List<Vector3> toCheck)
    {
        return toCheck.Where(x => !IsPointOnLine(p1, p2, x, wallWidth)).ToList();
    }

    private Tuple<Vector3, Vector3> GetLongestSegment(List<Vector3> leftPoints, List<Vector3> rightPoints, Vector3 axis)
    {
        Vector3 furthestLeft = new Vector3();
        Vector3 furthestRight = new Vector3();
        float maxDistance = 0;
        foreach (Vector3 left in leftPoints)
        {
            foreach (Vector3 right in rightPoints)
            {
                if ((axis.x != 0) &&  (Math.Abs(left.z - right.z) >= wallWidth))
                    continue;
                if ((axis.z != 0) && (Math.Abs(left.x - right.x) >= wallWidth))
                    continue;

                float distance = DistanceXZ(left, right);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    furthestLeft = left;
                    furthestRight = right;
                }
            }
        }
        return new Tuple<Vector3, Vector3>(furthestLeft, furthestRight);
    }

    private bool IsPointOnLine(Vector3 p1, Vector3 p2, Vector3 toCheck, float treshold)
    {
        if (Math.Abs(p1.x - p2.x) <= treshold)
        {
            if (Math.Abs(p1.x - toCheck.x) <= treshold)
                return true;
        }
        else if (Math.Abs(p1.z - p2.z) <= treshold)
        {
            if (Math.Abs(p1.z - toCheck.z) <= treshold)
                return true;
        }
        return false;
    }

    private float DistanceXZ(Vector3 point1, Vector3 point2)
    {
        return Mathf.Sqrt(Mathf.Pow(point1.x - point2.x, 2) + Mathf.Pow(point1.z - point2.z, 2));
    }

    private void PlaceWallsBetweenPoints(Vector3 p1, Vector3 p2)
    {
        Vector3 start = new Vector3();
        Vector3 end = new Vector3();
        Vector3 increment = new Vector3();
        Vector3 currentPosition = new Vector3();


        if (Math.Abs(p1.x - p2.x) > wallWidth || Math.Abs(p1.z - p2.z) < wallWidth)
        {
            start = p1 + new Vector3(1, 0, 0) * wallWidth / 2; 
            end = p2 + new Vector3(1, 0, 0) * wallWidth;
        }
        else
        {
            start = p1;
            end = p2 + new Vector3(0, 0, -2) * wallWidth;
        }

        if (Math.Abs(p1.x - p2.x) > wallWidth)
            increment = new Vector3(1, 0, 0) * wallWidth;
        else
            increment = new Vector3(0, 0, -1) * wallWidth;

        start.y = rooms[0].transform.position.y;
        currentPosition = start;

        if (increment.x != 0)
        {
            while (Math.Abs(currentPosition.x - end.x) > wallWidth)
            {
                GameObject wall = Instantiate(this.wall, currentPosition, Quaternion.identity, wallContainer.transform);
                placedWalls.Add(wall);
                currentPosition += increment;
            }
        }
        else
        {
            while (Math.Abs(currentPosition.z - end.z) > wallWidth)
            {
                GameObject wall = Instantiate(this.wall, currentPosition, Quaternion.identity, wallContainer.transform);
                placedWalls.Add(wall);
                currentPosition += increment;
            }
        }
    }

    private float RoomArea(RoomAttributes room)
    {
        return Mathf.Abs((room.bottomRight.x - room.bottomLeft.x) * (room.topLeft.y - room.bottomLeft.y) - (room.bottomRight.y - room.bottomLeft.y) * (room.topLeft.x - room.bottomLeft.x));
    }

    private void SortRooms()
    {
        roomAttributes.Sort((x, y) => RoomArea(y).CompareTo(RoomArea(x)));
    }

    private void SetWallSizes()
    {
        GameObject w = Instantiate(wall);

        MeshRenderer[] meshRenderers = w.GetComponentsInChildren<MeshRenderer>();

        Bounds bounds = meshRenderers[0].bounds;

        for (int i = 1; i < meshRenderers.Length; i++)
        {
            bounds.Encapsulate(meshRenderers[i].bounds);
        }

        wallWidth = bounds.size.x;
        Destroy(w);
    }
}