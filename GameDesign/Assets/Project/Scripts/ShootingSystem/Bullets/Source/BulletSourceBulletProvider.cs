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
        IBullet bullet = bulletProvider.GetBullet();
        bullet.damage = statsEntityFinal.RangedDamage;
        bullet.Shoot(transform.position, GetDirection());
    }
}
