using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private LayerMask targetLayerMask;
    private Rigidbody2D rb;

    void Start()
    {
        SoundManager.PlaySound(SoundType.SHOOT_SOUND, 0.5f);
        Destroy(gameObject, 3f);
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = projectileSpeed * transform.right;
        projectileSpeed = rb.velocity.magnitude;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if ((targetLayerMask & (1 << collision.gameObject.layer)) != 0 )
                //Destroy player here
            Destroy(gameObject);
        }
    }
}
