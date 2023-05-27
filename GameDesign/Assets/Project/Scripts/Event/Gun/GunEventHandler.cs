using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEventHandler : MonoBehaviour, IOnGunReload, IOnGunSwitch
{
    private EventSubmission eventSubmission;

    virtual public void OnGunReloadEvent(object sender, GunReloadEventArgs e)
    {
    }

    virtual public void OnGunSwitchEvent(object sender, GunSwitchEventArgs e)
    {
    }

    virtual protected void Start()
    {
        eventSubmission = GetComponentInParent<EventSubmission>();
        eventSubmission.GunReloadEvent += OnGunReloadEvent;
        eventSubmission.GunSwitchEvent += OnGunSwitchEvent;
    }

    
}
