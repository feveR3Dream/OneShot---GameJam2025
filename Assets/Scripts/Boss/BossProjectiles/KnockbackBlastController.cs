using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackBlastController : MonoBehaviour, I_ProjectileHostile
{
    private GameObject owner;
    private bool available = false;
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
                EventDispatcher.Instance.SendEvent(new BossDamaged());
            }
        }
    }
}
