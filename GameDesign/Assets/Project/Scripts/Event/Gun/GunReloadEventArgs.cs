using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunReloadEventArgs : System.EventArgs
{
    public float ReloadTime { get; private set; }

    public GunReloadEventArgs(float ReloadTime)
    {
        this.ReloadTime = ReloadTime;
    }   
}
