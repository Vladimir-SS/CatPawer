using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRay : IBulletMonoBehaviour 
{

    private LineRenderer lineRenderer;
    //Enemy Layer Mask
    //private static LayerMask enemyLayerMask;
    private static float MaxDistance = 100f;
    private static int TimeToLive = 50;


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    
    async void RemoveBullet()
    {
        await System.Threading.Tasks.Task.Delay(TimeToLive);
        lineRenderer.SetPositions(new Vector3[] { });
        gameObject.SetActive(false);
    }

    override public void Shoot(Vector3 position, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, MaxDistance/*, enemyLayerMask*/))
        {
            hit.collider.gameObject.GetComponent<Damageable>()?.TakeDamage(Damage);
            lineRenderer.SetPositions(new Vector3[] { position, hit.point });
        }
        else
        {
            // positionEnd = starting at position go in direction for MaxDistance
            Vector3 positionEnd = position + direction * MaxDistance;

            lineRenderer.SetPositions(new Vector3[] { position, positionEnd });
        }
        RemoveBullet();
    }
}
