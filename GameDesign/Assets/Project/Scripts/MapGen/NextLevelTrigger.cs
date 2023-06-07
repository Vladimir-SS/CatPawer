using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ScoreSystem.instance.NextLevel();
            LevelManager manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            manager.StartCoroutine(manager.GetNextLevel());
        }
    }
}