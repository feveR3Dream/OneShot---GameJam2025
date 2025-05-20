using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShockwave : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float pushForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player") == true)
            {
                collision.TryGetComponent<PlayerController>(out PlayerController playerController);
                StartCoroutine(Knockback(duration, playerController, pushForce));
            }
        }
    }

    IEnumerator Knockback(float duration, PlayerController pc, float PushForce)
    {
        pc.SetCanMove(false);
        pc.GetRigidBody().drag = 1;
        pc.GetRigidBody().AddForce((pc.gameObject.transform.position - this.transform.position) * PushForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        pc.GetRigidBody().drag = 0;
        pc.SetCanMove(true);
    }
}
