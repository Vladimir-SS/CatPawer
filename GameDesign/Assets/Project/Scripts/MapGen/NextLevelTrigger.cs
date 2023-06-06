using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask triggerLayers;
    private void OnTriggerEnter(Collider other)
    {
        if (triggerLayers == (triggerLayers | (1 << other.gameObject.layer)))
        {
            Debug.Log("Trigger----------------------------");
            Debug.Log(GameObject.Find("LevelManager"));
            Debug.Log(GameObject.Find("LevelManager").GetComponent<LevelManager>());
            GameObject.Find("LevelManager").GetComponent<LevelManager>().NextLevel();
            Debug.Log("Trigger----------------------------");
        }
    }
}
