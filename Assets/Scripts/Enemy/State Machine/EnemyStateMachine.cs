using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState;

    public void StartState(EnemyState newState)
    {
        currentState = newState;
        currentState.EnterState();
    }

    public void SwitchState(EnemyState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}

