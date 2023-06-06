using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletProjectile : IBulletMonoBehaviour
{
    public float speed;
    public Vector3 originalPosition = Vector3.zero;
    public string ShooterTag { get; set; } = "Player";

    override public void Shoot(Vector3 position, Vector3 direction)
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

        transform.position += speed * Time.deltaTime * transform.forward;
    }

   private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ShooterTag))
        {
            var target = other.GetComponent<Damageable>();
            if (target != null)
            {
                target.TakeDamage(Damage);
            }

            gameObject.SetActive(false);
        }
    }
    
}

