using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletSourceBase : MonoBehaviour
{
    protected StarterAssetsInputs starterAssetsInputs;
    protected StatsEntityFinal statsEntityFinal;

    void Start()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        statsEntityFinal = GetComponentInParent<StatsEntityFinal>();
    }

    protected Vector3 GetDirection()
    {
        Vector3 targetPosition = AimRaycastProvider.GetCameraTargetPosition();
        Vector3 direction = (targetPosition - transform.position).normalized;

        return direction;
    }

    protected abstract void ShootBullet();

    //TODO: Manual vs Continous 
    void Update()
    {
        if (starterAssetsInputs.shoot)
        {
            starterAssetsInputs.shoot = false;
            ShootBullet();
        }
    }
}
