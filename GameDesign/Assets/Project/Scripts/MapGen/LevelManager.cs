using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int seed = 123;

    [SerializeField] private float collisionTollerance;

    [SerializeField] private int numberOfRooms = 5;

    [SerializeField] private GameObject player;


    //[SerializeField] private MapBuilder mapBuilder;
    //[SerializeField] private MapGenerator mapGenerator;
    //[SerializeField] private WallPlacer wallPlacer;

    [SerializeField] private List<GameObject> allStartRooms;
    [SerializeField] private List<GameObject> allEndRooms;
    [SerializeField] private List<GameObject> allSpecialRooms;
    [SerializeField] private List<GameObject> allPlaceableRooms;

    [SerializeField] private GameObject wall;

    public GameObject mapBuilderObject;
    private MapGenerator mapGenerator;
    private WallPlacer wallPlacer;


    // Start is called before the first frame update
    void Start()
    {
        //mapBuilder = this.GetComponent<MapBuilder>();
        //mapBuilder = FindObjectInScene("MapBuilder");
        mapGenerator = this.GetComponent<MapGenerator>();
        wallPlacer = this.GetComponent<WallPlacer>();
        Debug.Log(mapBuilderObject);
        // ini seed
        // create first level

        CreateLevel(numberOfRooms);
    }

    private void CreateLevel(int numberOfRooms)
    {
        //GameObject wall = this.GetComponent<GameObject>();

        System.Random rng = new System.Random(seed);

        // get the start room
        GameObject startRoom = allStartRooms[rng.Next(allStartRooms.Count)];
        // get the end room
        GameObject endRoom = allEndRooms[rng.Next(allEndRooms.Count)];
        // get the special room
        GameObject specialRoom = allSpecialRooms[rng.Next(allSpecialRooms.Count)];

        // get all other rooms
        List<GameObject> placeableRooms = new List<GameObject>();
        for(int i = 0; i < numberOfRooms; i++)
        {
            placeableRooms.Add(allPlaceableRooms[rng.Next(allPlaceableRooms.Count)]);
        }

        placeableRooms.Add(endRoom);
        placeableRooms.Add(specialRoom);
        //Debug.Log(mapBuilder);
        // start map generation
        mapBuilderObject.GetComponent<MapBuilder>().SetMapBuilderParameters(seed, collisionTollerance, 
            placeableRooms, startRoom, wall, player);
        mapBuilderObject.GetComponent<MapBuilder>().StartCoroutine(mapBuilderObject.GetComponent<MapBuilder>().StartBuild());
        // ini seed
        // clear current level
        // while(fail)
        // create next level
    }

    private void ClearLevel()
    {
        // destroy all rooms
        // destroy all walls
        // destroy all enemies
        // destroy all items
    }

    public void GetNextLevel(int numberOfRooms)
    {
        // ini seed
        // clear current level

        // create next level
    }

    public static GameObject FindObjectInScene(string search)
    {
        var scene = SceneManager.GetActiveScene();
        var sceneRoots = scene.GetRootGameObjects();

        GameObject result = null;
        foreach (var root in sceneRoots)
        {
            if (root.name.Equals(search)) return root;
        }

        return result;
    }
}
