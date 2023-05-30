using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPoolProviderBase : IBulletProvider
{
    protected IObjectPool<IBulletMonoBehaviour> _bulletPool;
    [SerializeField] protected IBulletMonoBehaviour _bulletPrefab;

    public IObjectPool<IBulletMonoBehaviour> BulletPool
    {
        get
        {
            if (_bulletPool == null)
            {
                _bulletPool = new ObjectPool<IBulletMonoBehaviour>(CreateBullet, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, defaultCapacity: 10);
            }
            return _bulletPool;
        }

    }

    public override IBullet GetBullet()
    {
        return BulletPool.Get();
    }

    void OnTakeFromPool(IBulletMonoBehaviour bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    void OnReturnToPool(IBulletMonoBehaviour bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    void OnDestroyPoolObject(IBulletMonoBehaviour bullet)
    {
        Destroy(bullet);
    }

    IBulletMonoBehaviour CreateBullet()
    {
        IBulletMonoBehaviour clone =  Instantiate(_bulletPrefab);
        var returnToPool = clone.gameObject.AddComponent<ReturnBulletToPool>();
        returnToPool.BulletPool = BulletPool;

        return clone;
    }

    
}
