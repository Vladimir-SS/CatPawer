using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BulletProjectile))]
public class ReturnBulletProjectileToPool : MonoBehaviour
{
    protected BulletProjectile bullet;
    public IObjectPool<BulletProjectile> bulletPool { protected get; set; }

    private void Start()
    {
        bullet = GetComponent<BulletProjectile>();
    }

    private void OnDisable()
    {
        bulletPool.Release(bullet);
    }
}