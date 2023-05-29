using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBinOpener : MonoBehaviour
{
    [SerializeField] private Animator trashbin;

    private bool isOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            trashbin.Play("TrashBinOpen", 0, 0.0f);
            isOpened = true;
        }
    }
}
