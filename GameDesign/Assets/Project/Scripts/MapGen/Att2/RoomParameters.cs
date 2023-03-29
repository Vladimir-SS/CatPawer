using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomParameters : MonoBehaviour
{
    private List<Vector3> _corners;
    private List<DoorMarker> _doors;
    public bool isSpecialRoom = false;  //true if it is boss or upgrade room
    //wtf is the location supposed to be?
    
    void Start()
    {
        SetSizes();
    }

    private void SetSizes()
    {
        List<Vector3> vertices = new List<Vector3>();

        MeshFilter[] meshFilters = this.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] meshVertices = mesh.vertices;

            // Transform the vertices from local space to world space
            for (int i = 0; i < meshVertices.Length; i++)
            {
                Vector3 worldVertex = meshFilter.transform.TransformPoint(meshVertices[i]);
                vertices.Add(worldVertex);
            }
        }

        for (int i = 0; i < 4; i++)
            this._corners.Add(vertices[i]);
        // Get the coordinates of the 4 corners
        /* Vector3 topLeft = vertices[0];
        Vector3 topRight = vertices[1];
        Vector3 bottomRight = vertices[2];
        Vector3 bottomLeft = vertices[3];*/
    }
}
