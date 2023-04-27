using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IBulletProvider))]
public class BulletSourceBulletProvider : BulletSourceBase
{
    protected IBulletProvider bulletProvider;

    void Awake()
    {
        bulletProvider = GetComponent<IBulletProvider>();
    }

    protected override void ShootBullet()
    {
        BulletProjectile bullet = (BulletProjectile)bulletProvider.GetBullet();
        bullet.damageFromProvider = statsEntityFinal.Combat.Damage;
        bullet.Shoot(transform.position, GetDirection());
    }   
}