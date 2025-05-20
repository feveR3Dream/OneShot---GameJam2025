using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerShockWave : MonoBehaviour
{
    [SerializeField] private CircleCollider2D collider2d;
    [SerializeField] protected float pushForce;
    private PlayerController pc;
    private bool run;

    private void Start()
    {
        pc = GameObject.FindAnyObjectByType<PlayerController>();
        collider2d = GetComponent<CircleCollider2D>();
        collider2d.enabled = false;
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossChangePhase>(PhaseChangeEventTriggered);
    }
    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossChangePhase>(PhaseChangeEventTriggered);
    }

    private void PhaseChangeEventTriggered(BossChangePhase e)
    {
        
        collider2d.enabled = true;
        if (e.Phase <= 3)
            collider2d.radius += e.Phase + 2;
        if (pc != null)
        {
            if (Vector2.Distance(transform.position, pc.gameObject.transform.position) > collider2d.radius)
                collider2d.enabled = false; // kill itself
            else
            {
                run = true;
                pc.SetCanMove(false);
                pc.GetRigidBody().drag = 1;
                StartCoroutine(PushPlayer());
            }
        }
        
    }

    private void DisableCollider()
    {
        collider2d.enabled = false;
        run = false;
        if (pc != null)
        {
            pc.GetRigidBody().drag = 0;
            pc.SetCanMove(true);
        }
    }

    private IEnumerator PushPlayer()
    {
        while (pc != null && run)
        {
            pc.GetRigidBody().AddForce((pc.gameObject.transform.position - this.transform.position) * pushForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
            if (collision.CompareTag("Player") == true)
                DisableCollider();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, collider2d.radius);
    }
}
