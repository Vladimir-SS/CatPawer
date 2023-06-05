using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyParrentColliderButTrigger : MonoBehaviour
{
    void Start()
    {
        //Copy parrent collider but trigger
        Collider[] colliders = transform.parent.GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            Collider newCollider = gameObject.AddComponent(collider.GetType()) as Collider;
            newCollider.isTrigger = true;
        }
    }
}
