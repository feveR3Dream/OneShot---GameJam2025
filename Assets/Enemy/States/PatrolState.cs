using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyState
{
    public PatrolState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {

        // COMMENTED FOR OBSERVATION
        // enemy.ChasePlayer();
    }

    public override void ExitState()
    {

    }

}

