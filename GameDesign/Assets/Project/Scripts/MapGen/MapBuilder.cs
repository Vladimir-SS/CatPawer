using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    public float collisionTollerance;
    public int seed = 123;
    public GameObject startRoom;
    public List<GameObject> placeableRooms;

    public GameObject wall;
    private MapGenerator mapGenerator;
    private WallPlacer wallPlacer;


    IEnumerator Start()
    {
        mapGenerator = this.GetComponent<MapGenerator>();
        yield return StartCoroutine(mapGenerator.GenerateMap(seed, collisionTollerance, startRoom, placeableRooms));
        
        yield return(StartCoroutine(ClearRooms()));

        wallPlacer = this.GetComponent<WallPlacer>();

        // a necessary artifice; corners position gets mangled
        startRoom.GetComponent<RoomParameters>().SetCorners(mapGenerator.GetStartRoomCorners());
        placeableRooms.Add(startRoom);

        wallPlacer.PlaceWalls(placeableRooms, 1, wall);
    }

    private IEnumerator ClearRooms()
    {
        for (int i = 0; i < placeableRooms.Count; i++)
        {
            if (!placeableRooms[i].activeSelf)
            {
                GameObject.Destroy(placeableRooms[i]);
                placeableRooms.RemoveAt(i);
                i--;
            }
        }
        yield return null;
    }
}
