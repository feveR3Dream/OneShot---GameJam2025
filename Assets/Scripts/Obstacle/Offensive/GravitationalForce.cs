using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalForce : MonoBehaviour
{
    public float gravityForce = 9.8f;
    public float gravityRadius = 5f;
    public LayerMask affectedLayers;
    public bool Pull;

    void FixedUpdate()
    {
        // Find all colliders within the radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, gravityRadius, affectedLayers);
        foreach (Collider2D col in colliders)
        {
            Rigidbody2D rb = col.attachedRigidbody;
            if (rb != null)
            {
                Vector2 direction;
                if (Pull)
                    direction = (Vector2)transform.position - rb.position;
                else
                    direction =  rb.position - (Vector2)transform.position;
                float distance = direction.magnitude;

                // Optional: scale force by distance
                float forceMagnitude = (gravityForce*100) / Mathf.Max(distance, 0.1f); // Avoid division by 0
                Vector2 force = direction.normalized * forceMagnitude;

                rb.AddForce(force);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);
    }
}
