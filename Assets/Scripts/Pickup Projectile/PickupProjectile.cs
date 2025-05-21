using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PickupProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject visualProjectile;


    [Header("Values")]
    [SerializeField] private float radius;
    [SerializeField] private float lerpToPlayerSpeed;

    private void Start()
    {
        SoundManager.PlaySound(SoundType.PICKUP_SOUND, 1f);
    }

    private void Update()
    {
        DetectPlayer();
    }


    private void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(this.transform.position, radius, LayerMask.GetMask("Player"));

        Vector2 visualMove = Vector2.Lerp(visualProjectile.transform.position, 
        playerCollider != null ? playerCollider.transform.position : this.transform.position
        , lerpToPlayerSpeed * Time.deltaTime);

        visualProjectile.transform.position = visualMove;

        if (playerCollider != null)
        {
            if (Vector2.Distance(playerCollider.transform.position, (Vector2) visualProjectile.transform.position) <= 1f) // Temporary value
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
