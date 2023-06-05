using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyTrigger : MonoBehaviour
{
    private IBulletMonoBehaviour parrent;

    private void Start()
    {
        parrent = GetComponentInParent<IBulletMonoBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLUSION " + other.gameObject.tag);
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.TakeDamage(parrent.Damage);
            parrent.gameObject.SetActive(false);
        }
    }
}
