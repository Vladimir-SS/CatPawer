using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody bulletRigidBody;
    private float maxDistance = 100f;
    private float damage = 0;
    private Vector3 startPosition;
    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    public void Init(float damage, float maxDistance)
    {
        this.damage = damage;
        this.maxDistance = maxDistance;
    }

    void Start()
    {
        const float speed = 10f;
        bulletRigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Bullet Destroyed");
    }
}
