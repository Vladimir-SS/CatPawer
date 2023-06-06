using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBouncy : IBulletMonoBehaviour
{
    [SerializeField] private float velocity;
    private Rigidbody rb;
    [SerializeField] private float TimeToDestroy = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
    }

    private void Dissapear()
    {
        gameObject.SetActive(false);
    }



    public override void Shoot(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.forward = direction;
        Debug.Log(transform.forward);
        rb.velocity = transform.forward * velocity;
        Invoke(nameof(Dissapear), TimeToDestroy);
    }
}
