using StarterAssets;
using Stats.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideGun : MonoBehaviour
{
    public GameObject gun;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }
    
    void Update()
    {
        if (gun == null)
            return;

        if (starterAssetsInputs.sprint && gun.activeSelf)
            gun.SetActive(false);
        else if (!starterAssetsInputs.sprint && !gun.activeSelf)
            gun.SetActive(true);
    }
}
