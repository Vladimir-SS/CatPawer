using Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoUI : GunEventHandler
{
    private BulletSourceBase bulletSourceBase = null;
    private StatsEntityFinal statsEntityFinal = null;
    private TextMeshProUGUI textMesh = null;

    override protected void Start()
    {
        base.Start();
        statsEntityFinal = GetComponentInParent<StatsEntityFinal>();
        textMesh = GetComponent<TextMeshProUGUI>();

        InventoryHandler inventory = GetComponentInParent<InventoryHandler>();
        if (inventory.Gun)
            NewGun(inventory.Gun);
    }

    private void NewGun(GameObject gun)
    {
        bulletSourceBase = gun.transform.Find("ShootingAnchor").GetComponent<BulletSourceBase>();
    }

    public override void OnGunSwitchEvent(object sender, GunSwitchEventArgs e)
    {
        NewGun(e.GunObject);
    }

    private string GetAmmo()
    {
        if (null == bulletSourceBase)
            return "?";

        return bulletSourceBase.BulletsInMagazine.ToString();
    }

    private void Update()
    {
        string s = GetAmmo() + " / " + statsEntityFinal.Gun.MagazineCapacity;
        textMesh.text = s;
        
    }
}
