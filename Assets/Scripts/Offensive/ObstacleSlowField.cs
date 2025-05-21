using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSlowField : MonoBehaviour
{
    public float speedReduction = 10f;
    public float radius = 3f;
    public LayerMask affectedLayers;
    private CircleCollider2D collider2d;

    private void Start()
    {
        collider2d = GetComponent<CircleCollider2D>();
        collider2d.radius = radius;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsInLayerMask(other.gameObject, affectedLayers))
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.drag = speedReduction*100;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (IsInLayerMask(other.gameObject, affectedLayers))
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.drag = 0f; // Reset drag when leaving the field
            }
        }
    }

    bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
