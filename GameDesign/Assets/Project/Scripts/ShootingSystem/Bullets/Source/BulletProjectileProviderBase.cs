using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletProjectileProviderBase : IBulletProvider
{
    protected IObjectPool<BulletProjectile> _bulletPool;
    [SerializeField] protected BulletProjectile _bulletPrefab;

    public IObjectPool<BulletProjectile> BulletPool
    {
        get
        {
            if (_bulletPool == null)
            {
                _bulletPool = new ObjectPool<BulletProjectile>(CreateBullet, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, defaultCapacity: 10);
            }
            return _bulletPool;
        }

    }

    public override IBullet GetBullet()
    {
        return BulletPool.Get();
    }

    void OnTakeFromPool(BulletProjectile bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    void OnReturnToPool(BulletProjectile bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyPoolObject(BulletProjectile bullet)
    {
        Destroy(bullet.gameObject);
    }

    BulletProjectile CreateBullet()
    {
        var clone =  Instantiate(_bulletPrefab);
        var returnToPool = clone.gameObject.AddComponent<ReturnBulletProjectileToPool>();
        returnToPool.bulletPool = BulletPool;

        return clone;
    }

    
}
