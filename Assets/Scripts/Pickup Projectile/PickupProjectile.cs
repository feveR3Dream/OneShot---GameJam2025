using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PickupProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;


    [Header("Values")]
    [SerializeField] private float radius;
    [SerializeField] private float lerpToPlayerSpeed;


    // References
    private GameObject visualProjectile;


    private void Start() // Spawn bullet visual here
    {
        visualProjectile = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation, this.transform);
        visualProjectile.GetComponent<Projectile>().IsBullet(false);
    }


    private void Update()
    {
        if (this.gameObject == null) return;

        DetectPlayer();
    }


    private void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(this.transform.position, radius, LayerMask.GetMask("Player"));

        Vector2 temp = Vector2.Lerp(visualProjectile.transform.position, 
        playerCollider != null ? playerCollider.transform.position : this.transform.position, 
        lerpToPlayerSpeed * Time.deltaTime);

        visualProjectile.transform.position = temp;

        if (playerCollider != null)
        {
            if (Vector2.Distance(playerCollider.transform.position, (Vector2) visualProjectile.transform.position) <= 0.5f) // Temporary value
            {
                playerCollider.gameObject.GetComponent<GunController>().SetFire(true);
                Destroy(visualProjectile); 
                Destroy(this.gameObject); 
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
