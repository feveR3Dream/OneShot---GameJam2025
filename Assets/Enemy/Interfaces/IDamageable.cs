using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{

    void Damaged(float damageAmount, Vector2 hitPos);

    void Die();

    float MaxHealth { get; set; }

    float CurrentHealth { get; set; }
}
