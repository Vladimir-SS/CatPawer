
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    public float Damage { get; set; }

    public void Shoot(Vector3 position, Vector3 direction);
}
