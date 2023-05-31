using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public GameObject loadingScreenImage;
    public Slider loadingBarSlider;
    private static GameObject loadingScreen;
    private static Slider loadingBar;


    private List<GameObject> placeableRooms;
    private GameObject startRoom;

    public GameObject mapBuilderObject;

    void Start()
    {
        if (loadingScreenImage != null && loadingBarSlider != null)
        {
            loadingScreen = loadingScreenImage;
            loadingBar = loadingBarSlider;
        }
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

    private IEnumerator LoadLevel()
    {
        print(3);
        yield return new WaitForSeconds(1f);
        ClearLevel();

        loadingBar.value = 0.1f;
        SceneManager.LoadScene(levelSceneName, LoadSceneMode.Additive);
        loadingBar.value = 0.5f;
        player.GetComponentInChildren<CharacterController>().transform.position = new Vector3(0, 2, 0);
        loadingBar.value = 0.8f;

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(levelSceneName));

        loadingBar.value = 0.9f;

        player.SetActive(false);
        loadingScreen.SetActive(false);
    }

    public IEnumerator GetNextLevel()
    {
        print(0);
        loadingBar.value = 0;
        print(1);

        loadingScreen.SetActive(true);
        print(2);
        
        yield return StartCoroutine(LoadLevel());
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
