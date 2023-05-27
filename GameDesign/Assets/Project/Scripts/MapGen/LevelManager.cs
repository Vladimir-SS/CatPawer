using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int seed = 123;

    [SerializeField] private float collisionTollerance;

    [SerializeField] private int numberOfRooms = 5;

    [SerializeField] private string levelSceneName = "LevelTemplate";

    private GameObject player;

    [SerializeField] private List<GameObject> allStartRooms;
    [SerializeField] private List<GameObject> allEndRooms;
    [SerializeField] private List<GameObject> allSpecialRooms;
    [SerializeField] private List<GameObject> allPlaceableRooms;

    [SerializeField] private GameObject wall;

    [SerializeField] private SceneAsset sourceSceneAsset;

    private List<GameObject> placeableRooms;
    private GameObject startRoom;

    public GameObject mapBuilderObject;

    void Start()
    {
        seed = UnityEngine.Random.Range(0, 100000);
        CreateLevel(numberOfRooms);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(GetNextLevel());
        }
    }

    public IEnumerator GetNextLevel()
    {
        yield return new WaitForSeconds(1);
        ClearLevel();

        SceneManager.LoadScene(levelSceneName, LoadSceneMode.Additive);

        UnityEngine.SceneManagement.Scene sceneToMerge = SceneManager.GetSceneByName(levelSceneName);

        player.GetComponentInChildren<CharacterController>().transform.position = new Vector3(0, 2, 0);

        player.SetActive(false);

        SceneManager.UnloadSceneAsync(sceneToMerge);
    }

    private Tuple<GameObject, List<GameObject>> SelectRooms(int numberOfRooms)
    {
        System.Random rng = new System.Random(seed);

        // get the start room
        GameObject startRoom = allStartRooms[rng.Next(allStartRooms.Count)];
        // get the end room
        GameObject endRoom = allEndRooms[rng.Next(allEndRooms.Count)];
        // get the special room
        GameObject specialRoom = allSpecialRooms[rng.Next(allSpecialRooms.Count)];

        // get all other rooms
        List<GameObject> placeableRooms = new List<GameObject>();
        for (int i = 0; i < numberOfRooms; i++)
        {
            placeableRooms.Add(allPlaceableRooms[rng.Next(allPlaceableRooms.Count)]);
        }
        seed++;
        placeableRooms.Add(endRoom);
        placeableRooms.Add(specialRoom);

        return new Tuple<GameObject, List<GameObject>>(startRoom, placeableRooms);
    }

    private void CreateLevel(int numberOfRooms)
    {
        player = FindObject("PlayerCat");

        (startRoom, placeableRooms) = SelectRooms(numberOfRooms);

        mapBuilderObject.GetComponent<MapBuilder>().SetMapBuilderParameters(seed, collisionTollerance, placeableRooms, startRoom, wall, player);
        mapBuilderObject.GetComponent<MapBuilder>().StartCoroutine(mapBuilderObject.GetComponent<MapBuilder>().StartBuild());
        
        if(GameObject.FindGameObjectsWithTag("Room").Length < numberOfRooms + 2)
        {
            StartCoroutine(GetNextLevel());
        }
    }

    private void ClearLevel()
    {
        var scene = SceneManager.GetActiveScene();
        var sceneRoots = scene.GetRootGameObjects();

        foreach (var root in sceneRoots)
        {
            if (!root.name.Equals("PlayerCat") && !root.CompareTag("Persistent"))
            {
                Destroy(root);
            }
        }
    }

    public static GameObject FindObject(string search)
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
