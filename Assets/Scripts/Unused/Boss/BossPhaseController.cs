using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossPhaseController : MonoBehaviour
{
    public GameObject Weakspot;
    public CircleCollider2D Circle;
    private int Phase;

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossHurt>(Hurt);   
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossHurt>(Hurt);
    }
    
    private void ChangeWeakspot(Vector2 projectilepos)
    {
        // World position of Circle center
        Vector2 center = Circle.transform.position + (Vector3)(Circle.offset * Circle.transform.lossyScale);
        float radius = Circle.radius * Mathf.Max(Circle.transform.lossyScale.x, Circle.transform.lossyScale.y);
        float inwardOffset = Random.Range(0.1f, 0.4f);
        float adjustedRadius = radius - inwardOffset;

        // Incoming direction from projectile to center
        Vector2 incomingDir = (center - projectilepos).normalized;

        // Flip direction
        Vector2 targetDir = incomingDir;

        // New position slightly inside edge in that direction
        Vector2 newPosition = center + targetDir * adjustedRadius;

        Weakspot.transform.position = newPosition;
    }

    private void PhaseIncrease()
    {
        Phase += 1;
        EventDispatcher.Instance.SendEvent(new BossChangePhase { Phase = this.Phase});
        switch (Phase)
        {
            default:
                //LOLOLOL
                break;
        }
    }

    private void Hurt(BossHurt e)
    {
        //Some other visual shit here
        PhaseIncrease();
        ChangeWeakspot(e.projectilePos);
    }

}
