using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletProjectile : IBullet
{
    public float speed;
    public Vector3 originalPosition = Vector3.zero;

    public override void Shoot(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.forward = direction;
        originalPosition = position;
    }

    private void Update()
    {
        if (Vector3.Distance(originalPosition, transform.position) > 40)
        {
            gameObject.SetActive(false);
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}

