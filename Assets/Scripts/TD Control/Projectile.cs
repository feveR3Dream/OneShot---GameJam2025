using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class Projectile : MonoBehaviour
{

    [SerializeField] private LayerMask bossLayer;
    [SerializeField] private float hitRadius = 1f;

    public void HitBoss()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, hitRadius, bossLayer);

        if (hit != null )
        {
            Debug.Log("Boss hit: " + hit.name);
            this.gameObject.SetActive(false);
        }

        else
            StartCoroutine(DisableAfterSeconds(5f));
    }

    private IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
