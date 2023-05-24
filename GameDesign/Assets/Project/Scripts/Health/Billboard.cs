using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;
    void Update()
    {
        if(cam == null)
            cam = FindObjectOfType<Camera>();

        if (cam == null)
        {
            Debug.Log("no cam");
            return;
        }
        
        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
