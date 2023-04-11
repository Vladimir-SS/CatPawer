using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBullet : MonoBehaviour
{
    public float damage;

    public abstract void Shoot(Vector3 position, Vector3 direction);
}
