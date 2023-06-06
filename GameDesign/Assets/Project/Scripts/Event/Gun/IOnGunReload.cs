using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnGunReload
{
    void OnGunReloadEvent(object sender, GunReloadEventArgs e);
}
