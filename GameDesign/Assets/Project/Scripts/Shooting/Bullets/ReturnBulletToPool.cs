using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(IBulletMonoBehaviour))]
public class ReturnBulletToPool : MonoBehaviour
{
    protected IBulletMonoBehaviour bullet;
    public IObjectPool<IBulletMonoBehaviour> BulletPool { protected get; set; }

    private void Start()
    {
        bullet = GetComponent<IBulletMonoBehaviour>();
    }

    private void OnDisable()
    {
        BulletPool.Release(bullet);
    }
}