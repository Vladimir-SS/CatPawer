using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSubmission : MonoBehaviour
{
    // GunReloadEvent
    public delegate void GunReloadEventHandler(object sender, GunReloadEventArgs e);
    public event GunReloadEventHandler GunReloadEvent;
    public void TriggerGunReloadEvent(object sender, GunReloadEventArgs e)
    {
        GunReloadEvent?.Invoke(sender, e);
    }
    
    
}
