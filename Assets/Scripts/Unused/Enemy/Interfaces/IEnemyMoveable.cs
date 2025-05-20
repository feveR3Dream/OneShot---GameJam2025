using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    float MoveSpeed { get; set; }

    Rigidbody2D RB { get; set; }

    void MoveEnemyTowards(Vector2 targetPos);

    void LookAtPlayer(Vector2 playerPos);
}
