using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEventHandler : MonoBehaviour, OnGunReloadI
{
    private EventSubmission eventSubmission;

    virtual public void OnGunReloadEvent(object sender, GunReloadEventArgs e)
    {
    }

    virtual protected void Start()
    {
        eventSubmission = GetComponentInParent<EventSubmission>();
        eventSubmission.GunReloadEvent += OnGunReloadEvent;
    }

    
}
