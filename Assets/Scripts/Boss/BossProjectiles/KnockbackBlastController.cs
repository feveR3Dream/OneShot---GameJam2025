using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackBlastController : MonoBehaviour, I_ProjectileHostile
{
    private GameObject owner;
    private bool available = false;
    private bool touched = false;
    public void SetOwner(GameObject obj)
    {
        owner = obj;
    }
    private void OnEnable()
    {
        available = true;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && available)
        {
            if (collider.gameObject.CompareTag("Player") == true)
            {
                Vector2 pushdirection = collider.gameObject.transform.position - owner.transform.position;
                Vector2 multipliedforce = pushdirection.normalized * 10f;

                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(multipliedforce, ForceMode2D.Impulse);
            }
        }
    }
}
