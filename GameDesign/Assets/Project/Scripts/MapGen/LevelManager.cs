using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int seed = 123;
    public int rounds = 0;

    [SerializeField] private float collisionTollerance;

    [SerializeField] private int numberOfRooms = 5;

    private GameObject player;
    private GameObject playerCopy;

    [SerializeField] private List<GameObject> allStartRooms;
    [SerializeField] private List<GameObject> allEndRooms;
    [SerializeField] private List<GameObject> allSpecialRooms;
    [SerializeField] private List<GameObject> allPlaceableRooms;

    [SerializeField] private GameObject wall;

    [SerializeField] private SceneAsset sourceSceneAsset;

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

            StartCoroutine(GetNextLevel(numberOfRooms));
        }
    }

    public IEnumerator GetNextLevel(int numberOfRooms)
    {
        yield return new WaitForSeconds(1);
        ClearLevel();
        rounds++;
        //UnityEngine.SceneManagement.Scene activeScene = SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.Scene gameScene = SceneManager.GetSceneByName("Game");
        SceneManager.LoadScene("GameTemplate", LoadSceneMode.Additive);

        UnityEngine.SceneManagement.Scene sceneToMerge = SceneManager.GetSceneByName("GameTemplate");

        //if (sceneToMerge == null) 
        //{
        //    sceneToMerge = SceneManager.GetSceneByName("GameTemplate");
        //}

        //print(activeScene.name);
        //print(sceneToMerge.name);
        player.transform.position = new Vector3(1, 2, 0);
        player.SetActive(false);
        try
        {
            if (gameScene.IsValid() && sceneToMerge.IsValid())
            {
                print("merging");
                SceneManager.MergeScenes(sceneToMerge, gameScene);
            }
            else
            {
                Debug.LogError("Invalid scenes for merging.");
            }
        }
        catch (ArgumentException e) { }
        SceneManager.UnloadSceneAsync(sceneToMerge);

        //SceneManager.MergeScenes(SceneManager.GetSceneByName("Game 1"), SceneManager.GetActiveScene());
        //// Find the template scene
        //string templateSceneName = "Game";
        //string newSceneName = "new_Game";

        //player = FindObject("PlayerCat");
        //print(player.name);

        //// Load the "NewGame" scene
        //SceneManager.LoadScene("Game 1", LoadSceneMode.Single);
        //    print("Loaded scene: " + SceneManager.GetActiveScene().name);

        //    // Place the gameObject into the new scene
        //    SceneManager.MoveGameObjectToScene(player, SceneManager.GetActiveScene());
        //    print("Placed object: " + player.name);

        //GameObject obj = GameObject.Find("LevelManager");
        //obj.GetComponent<LevelManager>().seed = 123;

        //CreateLevel(numberOfRooms);
        //SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        //// Find the desired object in the loaded scene
        //GameObject obj = GameObject.Find("LevelManager");

        //obj.GetComponent<LevelManager>().player = GameObject.Find("PlayerCat");

        //SceneManager.sceneLoaded -= OnSceneLoaded;
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
        List<GameObject> placeableRooms = new List<GameObject>();
        player = FindObject("PlayerCat");
        do
        {
            //ClearLevel();
            (startRoom, placeableRooms) = SelectRooms(numberOfRooms);

            mapBuilderObject.GetComponent<MapBuilder>().SetMapBuilderParameters(seed, collisionTollerance, placeableRooms, startRoom, wall, player);
            mapBuilderObject.GetComponent<MapBuilder>().StartCoroutine(mapBuilderObject.GetComponent<MapBuilder>().StartBuild());
        } while (GameObject.FindGameObjectsWithTag("Room").Length < numberOfRooms);
        // remove loading screen
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
