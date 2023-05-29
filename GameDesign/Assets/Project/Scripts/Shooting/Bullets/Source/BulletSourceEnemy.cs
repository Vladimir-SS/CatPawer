using Stats;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(IBulletProvider))]
public class BulletSourceEnemy : MonoBehaviour, IBulletSource
{
    [SerializeField] private IBulletProvider bulletProvider;
    private StatsEntityFinal statsEntityFinal;

    private bool canShoot = true;

    void Awake()
    {
        statsEntityFinal = GetComponentInParent<StatsEntityFinal>();
        bulletProvider = GetComponent<IBulletProvider>();
    }

    async void blockShooting()
    {
        canShoot = false;
        var fireRate = statsEntityFinal.Gun.FireRate;
        await Task.Delay((int)(fireRate * 1000));
        canShoot = true;
    }

    public void Shoot(Vector3 shootPosition)
    {
        if (canShoot)
        {
            blockShooting();
            IBullet bullet = bulletProvider.GetBullet();
            bullet.Damage = statsEntityFinal.Combat.Damage;
            bullet.Shoot(transform.position, shootPosition);
        }
    }
}
