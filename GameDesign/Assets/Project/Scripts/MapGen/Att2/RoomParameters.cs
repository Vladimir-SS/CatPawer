using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomParameters : MonoBehaviour
{
    private Vector3[] _corners = new Vector3[4];
    private DoorMarker[] _doors;
    public bool isSpecialRoom = false;  //true if it is boss or upgrade room

    void Start()
    {
        SetDoors();
        SetCorners();
    }

    private void SetCorners()
    {
        MeshRenderer[] meshRenderers = this.GetComponentsInChildren<MeshRenderer>();

        Bounds bounds = meshRenderers[0].bounds;

        for (int i = 1; i < meshRenderers.Length; i++)
        {
            bounds.Encapsulate(meshRenderers[i].bounds);
        }

        _corners[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        _corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        _corners[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        _corners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);

        
        //uncomment for quick check
        /*
        for (int i = 0; i < 4; i++)
        {
            GameObject emptyGameObject = new GameObject("EmptyGameObject");
            emptyGameObject.transform.position = _corners[i];
        }
        */
    }

    private void SetDoors()
    {
        _doors = this.GetComponentsInChildren<DoorMarker>();
    }

    public Vector3[] Corners
    {
        get => _corners;
    }

    public DoorMarker[] DoorMarkers
    {
        get => _doors;
    }
}
