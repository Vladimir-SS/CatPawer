using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMarker : MonoBehaviour
{
    private bool _isOpen = false;
    private Vector3 _position;

    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            _isOpen = value;
        }
    }

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
        }
    }
}
