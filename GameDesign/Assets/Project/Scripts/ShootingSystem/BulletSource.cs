using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSource : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    private StatsEntityFinal statsEntityFinal;
    [SerializeField] private GameObject bulletPrefab;

    void Start()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        statsEntityFinal = GetComponentInParent<StatsEntityFinal>();
    }

    private void ShootBullet()
    {
        Debug.Log("Shoot Bullet");
        Vector3 targetPosition = AimRaycastHandler.GetCameraTargetPosition();
        Vector3 direction = (targetPosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(direction));
        BulletProjectile bulletProjectile = bullet.GetComponent<BulletProjectile>();
        bulletProjectile.Init(statsEntityFinal.RangedDamage, statsEntityFinal.AttackRange);
    }

    void Update()
    {
        if (starterAssetsInputs.shoot)
        {
            starterAssetsInputs.shoot = false;
            ShootBullet();
        }
    }
}
