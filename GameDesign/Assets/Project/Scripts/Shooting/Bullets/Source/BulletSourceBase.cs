using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Stats;

public abstract class BulletSourceBase : MonoBehaviour, IBulletSource
{
    protected StarterAssetsInputs starterAssetsInputs;
    protected StatsEntityFinal statsEntityFinal;
    protected EventSubmission eventSubmission;

    protected float nextFire;
    protected uint bulletsInMagazine;
    protected bool isReloading;
    public bool isManual;

    public uint BulletsInMagazine => bulletsInMagazine;

    void Awake()
    {
        nextFire = 0f;
        bulletsInMagazine = 0;
    }

    void Start()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        statsEntityFinal = GetComponentInParent<StatsEntityFinal>();
        eventSubmission = GetComponentInParent<EventSubmission>();

        bulletsInMagazine = statsEntityFinal.Gun.MagazineCapacity;
    }

    protected Vector3 GetDirection()
    {
        Vector3 targetPosition = AimRaycastProvider.GetCameraTargetPosition();
        Vector3 direction = (targetPosition - transform.position).normalized;

        return direction;
    }

    protected GameObject GetHitObject()
    {
        return AimRaycastProvider.GetHitObject();
    }

    protected abstract void ShootBullet();

    protected async void reload()
    {
        
        isReloading = true;
        var reloadSpeed = statsEntityFinal.Gun.ReloadSpeed;
        eventSubmission.TriggerGunReloadEvent(this, new GunReloadEventArgs(reloadSpeed));
        await Task.Delay((int)(reloadSpeed * 1000));
        bulletsInMagazine = statsEntityFinal.Gun.MagazineCapacity;
        isReloading = false;
    }

    //TODO: Manual vs Continous 
    void Update()
    {
        if (starterAssetsInputs.reload)
        {
            if (!isReloading)
                reload();
            starterAssetsInputs.reload = false;
        }

        if (starterAssetsInputs.shootHold && starterAssetsInputs.aim && !isReloading)
        {
            if (isManual)
                starterAssetsInputs.shootHold = false;
            // TODO : FireRateFinal
            if (Time.time > nextFire && bulletsInMagazine != 0)
            {
                nextFire = Time.time + statsEntityFinal.Gun.FireRate;
                --bulletsInMagazine;
                ShootBullet();
            }
        }
    }
}
