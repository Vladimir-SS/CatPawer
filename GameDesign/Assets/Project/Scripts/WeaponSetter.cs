using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSetter : MonoBehaviour
{
    public void SetWeapon(string weapon)
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.weapon = weapon;
        }
        else
        {
            Debug.Log("MainManager is null");
        }
        
    }
}
