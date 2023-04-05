using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BulletSourceBase : MonoBehaviour
{
    protected StarterAssetsInputs starterAssetsInputs;
    protected StatsEntityFinal statsEntityFinal;
    protected GunStats gunStats;
    
    protected float nextFire;
    protected uint bulletsInMagazine;
    protected bool isReloading;
    public bool isManual;

    void Awake()
    {
        nextFire = 0f;
        bulletsInMagazine = 0;
    }

    void Start()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        statsEntityFinal = GetComponentInParent<StatsEntityFinal>();
        gunStats = GetComponentInParent<GunStats>();

        bulletsInMagazine = gunStats.MagazineCapacityBase;
    }

    protected Vector3 GetDirection()
    {
        Vector3 targetPosition = AimRaycastProvider.GetCameraTargetPosition();
        Vector3 direction = (targetPosition - transform.position).normalized;

        return direction;
    }

    protected abstract void ShootBullet();

    protected async void reload()
    {
        isReloading = true;
        await Task.Delay((int)(gunStats.ReloadSpeedBase * 1000));
        bulletsInMagazine = gunStats.MagazineCapacityBase;
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
            if(isManual)
                starterAssetsInputs.shootHold = false;
            // TODO : FireRateFinal
            if (Time.time > nextFire && bulletsInMagazine != 0)
            {
                nextFire = Time.time + gunStats.FireRateBase;
                --bulletsInMagazine;
                ShootBullet();
            }
        }
    }
}
