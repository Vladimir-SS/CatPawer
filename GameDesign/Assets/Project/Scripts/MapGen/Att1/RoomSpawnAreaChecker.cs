using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnAreaChecker : MonoBehaviour
{
    public bool removable = false;
    private BoxCollider col;

    private void Start()
    {
        col = this.GetComponent<BoxCollider>();
        col.size = new Vector3(col.size.x * 0.9f, col.size.y * 0.9f, col.size.z * 0.9f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomSpawnAreaChecker") && this.removable)
        {
            //Debug.Log("getting removed:" + transform.parent + " status " + removable);
            Destroy(transform.parent.gameObject);
        }
    }
}
