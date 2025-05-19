using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy // Framework
{
    public override void Start()
    {
        base.Start();
    }

    public override void ChasePlayer() // Example
    {
        // COMMENT FOR OBSERVATION
        //MoveEnemyTowards(mainTarget.position);
        //LookAtPlayer(mainTarget.position);
        //KeepDistance(distancingValue);
    }

    void OnDestroy()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, distancingValue);
    }

}
