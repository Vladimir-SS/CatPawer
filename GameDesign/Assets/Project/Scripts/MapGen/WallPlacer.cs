using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using static UnityEngine.UI.GridLayoutGroup;

class RoomAttributes
{
    public Vector3 topLeft = new Vector3();
    public Vector3 bottomLeft = new Vector3();
    public Vector3 topRight = new Vector3();
    public Vector3 bottomRight = new Vector3();

    public bool isTopLeftCovered = false;
    public bool isBottomLeftCovered = false;
    public bool isTopRightCovered = false;
    public bool isBottomRightCovered = false;

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
    private float doorSize;

    private GameObject wall;
    private float wallHeight;
    private float wallWidth;
    private float wallHeightOffset;

    private List<GameObject> placedWalls = new List<GameObject>();
    private MapGenerator mapGenerator;


    // sort rooms descending by size
    // place walls
    // if walls continue to another room, place until the end of the room


    public void PlaceWalls(List<GameObject> rooms, float doorSize, GameObject baseWall , float wallHeightOffset)
    {
        this.rooms = rooms;
        this.doorSize = doorSize;
        this.wall = baseWall;
        this.wallHeightOffset = wallHeightOffset;

        SetWallSizes();

        mapGenerator = this.GetComponent<MapGenerator>();

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] == null)
                continue;
            roomAttributes.Add(new RoomAttributes(rooms[i], mapGenerator));
        }

        SortRooms();

        //foreach(RoomAttributes roomAttribute in roomAttributes)
        //{
        //    GameObject emptyGameObject = new GameObject("TR");
        //    emptyGameObject.transform.position = roomAttribute.topRight;

        //    GameObject emptyGameObject2 = new GameObject("TL");
        //    emptyGameObject2.transform.position = roomAttribute.topLeft;

        //    GameObject emptyGameObject3 = new GameObject("BR");
        //    emptyGameObject3.transform.position = roomAttribute.bottomRight;

        //    GameObject emptyGameObject4 = new GameObject("BL");
        //    emptyGameObject4.transform.position = roomAttribute.bottomLeft;

        //}

        foreach(RoomAttributes room in roomAttributes)
        {
            PlaceCorners(room);

            PlaceWallsAlongXAxis(room.topLeft, room.topRight);
                PlaceWallsAlongXAxis(room.bottomLeft, room.bottomRight);
        }
        // ClearDuplicateWalls();
    }

    private void PlaceWallsAlongXAxis(Vector3 c1, Vector3 c2)
    {
        PlaceWallsBetweenPoints(c1, c2);
        Vector3 leftmost = new Vector3();
        Vector3 rightmost = new Vector3();
        if(c1.x < c2.x)
        {
            leftmost = c1;
            rightmost = c2;
        }
        else
        {
            leftmost = c2;
            rightmost = c1;
        }
        //Vector3 followingPoint = GetFollowingPointToTheRight(rightmost);

        //if (IntersectsAnyCorner(rightmost))
        //{
        //    print("right");
            
        //    SetIntersectedCornerCovered(rightmost);            
            

        //    PlaceWallsAlongXAxis(followingPoint, rightmost);
        //}

        Vector3 followingPoint = GetFollowingPointToTheLeft(leftmost);
        if (IntersectsAnyCorner(leftmost))
        {
            print("left");
            
            SetIntersectedCornerCovered(leftmost);

            PlaceWallsAlongXAxis(followingPoint, leftmost);
        }
    }

    private Vector3 GetFollowingPointToTheRight(Vector3 point)
    {
        Vector3 closestPoint = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        foreach (RoomAttributes room in roomAttributes)
        {
            if (Vector3.Distance(room.topRight, point) < Vector3.Distance(closestPoint, point))
            {
                closestPoint = room.topRight;
            }
            
            if (Vector3.Distance(room.bottomRight, point) < Vector3.Distance(closestPoint, point))
            {
                closestPoint = room.bottomRight;
            }

        }
        return closestPoint;
    }

    private Vector3 GetFollowingPointToTheLeft(Vector3 point)
    {
        Vector3 closestPoint = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity); 
        foreach (RoomAttributes room in roomAttributes)
        {

            if (Vector3.Distance(room.topLeft, point) < Vector3.Distance(closestPoint, point))
            {
                closestPoint = room.topLeft;
            }

            if (Vector3.Distance(room.bottomLeft, point) < Vector3.Distance(closestPoint, point))
            {
                closestPoint = room.bottomLeft;
            }
        }
        return closestPoint;
    }

    private void SetIntersectedCornerCovered(Vector3 point)
    {
        foreach(RoomAttributes room in roomAttributes)
        {
            if (DistanceXZ(point, room.topLeft) <= wallWidth)
                room.isTopLeftCovered = true;
            else if (DistanceXZ(point, room.topRight) <= wallWidth)
                room.isTopRightCovered = true;
            else if (DistanceXZ(point, room.bottomLeft) <= wallWidth)
                room.isBottomLeftCovered = true;
            else if (DistanceXZ(point, room.bottomRight) <= wallWidth)
                room.isBottomRightCovered = true;
        }
    }

    private float DistanceXZ(Vector3 point1, Vector3 point2)
    {
        return Mathf.Sqrt(Mathf.Pow(point1.x - point2.x, 2) + Mathf.Pow(point1.z - point2.z, 2));
    }

    private bool IntersectsAnyCorner(Vector3 point)
    {
        foreach(RoomAttributes room in roomAttributes)
        {
            if ((DistanceXZ(point, room.topLeft) <= wallWidth) && !room.isTopLeftCovered)
                return true;
            if((DistanceXZ(point, room.bottomLeft) <= wallWidth) && !room.isBottomLeftCovered)
                return true;
            if((DistanceXZ(point, room.topRight) <= wallWidth) && !room.isTopRightCovered)
                return true;
            if((DistanceXZ(point, room.bottomRight) <= wallWidth) && !room.isBottomRightCovered)
                return true;

        }
        return false;
    }

    private void PlaceWallsBetweenPoints(Vector3 p1, Vector3 p2)
    {
        Vector3 start = new Vector3();
        Vector3 end = new Vector3();
        Vector3 increment = new Vector3();
        Vector3 currentPosition = new Vector3();
        if((p1.x < p2.x) || (p1.z < p2.z))
        {
            start = p1;
            end = p2;
        }
        else
        {
            start = p1;
            end = p2;
        }

        if (p1.x < p2.x)
            increment = new Vector3(1, 0, 0) * wallWidth;
        else
            increment = new Vector3(0, 0, 1) * wallWidth;

        currentPosition = start + increment;

        while (currentPosition.x < end.x || currentPosition.z < end.z)
        {
            GameObject wall = Instantiate(this.wall, currentPosition, Quaternion.identity);
            placedWalls.Add(wall);
            currentPosition += increment;
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

    private void PlaceCorners(RoomAttributes room)
    {
        if(!room.isTopLeftCovered)
        {
            GameObject corner = Instantiate(wall, room.topLeft, Quaternion.identity);
            placedWalls.Add(corner);
            room.isTopLeftCovered = true;
        }
        if (!room.isTopRightCovered) 
        { 
            GameObject corner = Instantiate(wall, room.topRight, Quaternion.identity);
            placedWalls.Add(corner);
            room.isTopRightCovered = true;
        }
        if(!room.isBottomLeftCovered)
        {
            GameObject corner = Instantiate(wall, room.bottomLeft, Quaternion.identity);
            placedWalls.Add(corner);
            room.isBottomLeftCovered = true;
        }
        if(!room.isBottomRightCovered)
        {
            GameObject corner = Instantiate(wall, room.bottomRight, Quaternion.identity);
            placedWalls.Add(corner);
            room.isBottomRightCovered = true;
        }
        //GameObject corner1 = Instantiate(wall, room.bottomRight, Quaternion.identity);
        //GameObject corner2 = Instantiate(wall, room.topRight, Quaternion.identity);
        //GameObject corner3 = Instantiate(wall, room.topLeft, Quaternion.identity);
        //GameObject corner4 = Instantiate(wall, room.bottomLeft, Quaternion.identity);

        //placedWalls.Add(corner1);
        //placedWalls.Add(corner2);
        //placedWalls.Add(corner3);
        //placedWalls.Add(corner4);

        room.isTopRightCovered = true;
        room.isBottomLeftCovered = true;
        room.isBottomRightCovered = true;
        //GameObject emptyGameObject = new GameObject("EmptyGameObject");
        //emptyGameObject.transform.position = room.topLeft;
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

        wallHeight = bounds.size.y;
        wallWidth = bounds.size.x;
        Destroy(w);
    }

    private void ClearDuplicateWalls()
    {
        for(int i = 0; i < placedWalls.Count; i++)
        {
            for(int j = i + 1; j < placedWalls.Count; j++)
            {
                //if (placedWalls[i].transform.position.x == placedWalls[j].transform.position.x)
                if (Mathf.Abs(placedWalls[i].transform.position.x - placedWalls[j].transform.position.x) < wallWidth / 2)
                {
                    if (Mathf.Abs(placedWalls[i].transform.position.z - placedWalls[j].transform.position.z) < wallWidth / 2)
                    {
                        Destroy(placedWalls[j]);
                        placedWalls.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
    }
}
