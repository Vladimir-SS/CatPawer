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

    public bool topLeftCovered = false;
    public bool bottomLeftCovered = false;
    public bool topRightCovered = false;
    public bool bottomRightCovered = false;

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

        GameObject w = Instantiate(wall);
        wallHeight = w.GetComponent<MeshRenderer>().bounds.size.y;
        wallWidth = w.GetComponent<MeshRenderer>().bounds.size.x;
        Destroy(w);

        mapGenerator = this.GetComponent<MapGenerator>();

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] == null)
                continue;
            Vector3[] corners = rooms[i].GetComponent<RoomParameters>().Corners;
            //for (int j = 0; j < corners.Length; j++)
            //{
            //    GameObject emptyGameObject = new GameObject(rooms[i].name);
            //    emptyGameObject.transform.position = corners[j];
            //}
            roomAttributes.Add(new RoomAttributes(rooms[i], mapGenerator));
        }

        SortRooms();

        foreach(RoomAttributes room in roomAttributes)
        {
            PlaceCorners(room);
            //PlaceFillerWalls();
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
        // place corner 1
        GameObject corner1 = Instantiate(wall, room.bottomRight, Quaternion.identity);

        //GameObject emptyGameObject = new GameObject("EmptyGameObject");
        //emptyGameObject.transform.position = room.topLeft;
        placedWalls.Add(corner1);
        // place corner 2
        GameObject corner2 = Instantiate(wall, room.topRight, Quaternion.identity);
        placedWalls.Add(corner2);
        // place corner 3
        GameObject corner3 = Instantiate(wall, room.topLeft, Quaternion.identity);
        placedWalls.Add(corner3);
        // place corner 4
        GameObject corner4 = Instantiate(wall, room.bottomLeft, Quaternion.identity);
        placedWalls.Add(corner4);
    }
}
