using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    private float collisionTollerance;
    private int seed;

    private GameObject startRoom;
    private GameObject wall;
    private List<GameObject> placeableRooms;
    private GameObject player;

    private MapGenerator mapGenerator;
    private WallPlacer wallPlacer;
    private List<GameObject> rooms;



    public IEnumerator StartBuild()
    {
        yield return StartCoroutine(mapGenerator.GenerateMap(seed, collisionTollerance, startRoom, placeableRooms));

        yield return (StartCoroutine(ClearRooms()));

        // a necessary artifice; corners position gets mangled
        startRoom.GetComponent<RoomParameters>().SetCorners(mapGenerator.GetStartRoomCorners());
        placeableRooms.Add(startRoom);

        wallPlacer.PlaceWalls(placeableRooms, 2, wall);

        //player.SetActive(true);

        this.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();

        Debug.Log("Map built");
        player.SetActive(true);
        LevelManager.menuCanvas.SetActive(false);
        LevelManager.loadingScreen.SetActive(false);

    }

    public void SetMapBuilderParameters(int seed, float collisionTollerance, 
                                List<GameObject> placeableRooms, GameObject startRoom, GameObject wall, GameObject player)
    {
        this.seed = seed;
        this.collisionTollerance = collisionTollerance;
        this.startRoom = startRoom;
        this.placeableRooms = placeableRooms;
        this.wall = wall;
        this.player = player;

        this.mapGenerator = this.GetComponent<MapGenerator>();
        this.wallPlacer = this.GetComponent<WallPlacer>();
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
