using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public abstract void EnterState();
    public abstract void ExitState();

    public abstract void UpdateState();
    public abstract void FixedUpdateState();
}
