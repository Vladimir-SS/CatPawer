using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitchEventArgs : System.EventArgs
{
    public GameObject GunObject { get; private set; }
    public GunSwitchEventArgs(GameObject gunObject)
    {
        GunObject = gunObject;
    }
}
