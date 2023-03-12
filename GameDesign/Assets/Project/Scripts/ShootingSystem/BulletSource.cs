using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSource : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private GameObject bulletPrefab;

    void Start()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    private void ShootBullet()
    {
        Vector3 targetPosition = AimRaycastHandler.GetCameraTargetPosition();
        Vector3 direction = (targetPosition - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(direction));
    }
    
    void Update()
    {
        if (starterAssetsInputs.shoot)
        {
            ShootBullet();
            starterAssetsInputs.shoot = false;
        }
    }
}
