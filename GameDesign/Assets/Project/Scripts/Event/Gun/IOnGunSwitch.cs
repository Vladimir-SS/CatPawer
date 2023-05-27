using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnGunSwitch
{
    void OnGunSwitchEvent(object sender, GunSwitchEventArgs e);
}
