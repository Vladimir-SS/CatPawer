using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBulletMonoBehaviour : MonoBehaviour, IBullet
{
    virtual public float Damage { get; set; }

    abstract public void Shoot(Vector3 position, Vector3 direction);
}
