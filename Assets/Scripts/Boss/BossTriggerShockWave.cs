using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerShockWave : BossShockwave
{
    private PlayerController pc;
    private bool run;
    private IEnumerator PushPlayer()
    {
        while (pc != null && run)
        {
            pc.GetRigidBody().AddForce((pc.gameObject.transform.position - this.transform.position) * pushForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.2f);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
            if (collision.CompareTag("Player") == true)
            {
                run = true;
                pc = collision.GetComponent<PlayerController>();
                pc.SetCanMove(false);
                pc.GetRigidBody().drag = 1;
                StartCoroutine(PushPlayer());
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
            if (collision.CompareTag("Player") == true)
            {
                run = false;
                pc.SetCanMove(true);
                pc.GetRigidBody().drag = 0;
                pc = null;
            }
    }
}
