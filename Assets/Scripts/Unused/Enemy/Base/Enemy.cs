using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    public float MaxHealth { get; set; } = 100f;

    public float CurrentHealth { get; set; }

    public float MoveSpeed { get; set; } = 1f;

    public Rigidbody2D RB { get; set; }

    [Header("State Machine")]
    public EnemyStateMachine stateMachine;

    [Header("Reference")]
    [HideInInspector] public Transform mainTarget;

    [Header("Values")]
    [SerializeField] protected float maxHealth = 100f; // I can set this for each enemy in the inspector.
    public float distancingValue; // Distancing purpose, customizable for each individual enemies [TOP DOWN]
    private bool isKnockedBack = false; // New flag to track knockback state


    [Header("State Machines")]
    public PatrolState patrolState;

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
        patrolState = new PatrolState(this, stateMachine);

        MaxHealth = maxHealth;
    }


    public virtual void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.gravityScale = 0f;
        RB.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        RB.interpolation = RigidbodyInterpolation2D.Interpolate;

        CurrentHealth = MaxHealth;

        // SET A DEFAULT STATE
        // stateMachine.StartState(Insert State Here);
    }

    private void Update()
    {
        stateMachine.currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdateState();
    }

    #region Health / Die Functions

    public void Damaged(float damageAmount, Vector2 hitPos)
    {
        CurrentHealth -= damageAmount;

        #region [TOP DOWN / ACTION] - Knockback
        Vector2 knockBackDir = (hitPos - (Vector2)this.transform.position).normalized * -1f;

        isKnockedBack = true; // Disable movement

        if (RB != null)
        {
            RB.velocity = Vector2.zero; // Stop current movement before applying force
            RB.AddForce(knockBackDir, ForceMode2D.Impulse);
        }

        StartCoroutine(RecoverFromKnockback(0.1f)); // Recover after 0.5 seconds
        #endregion

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator RecoverFromKnockback(float delay)
    {
        yield return new WaitForSeconds(delay);
        isKnockedBack = false; // Allow movement again
    }

    public void Die() // Count points
    {
        Destroy(this.gameObject);
    }

    #endregion


    #region [TOP DOWN / SHOOTER] - Enemy Movement / Look / Distancing Functions
    public void MoveEnemyTowards(Vector2 targetPos)
    {
        if (isKnockedBack) return; // Stop movement if knocked back

        Vector2 direction = (targetPos - (Vector2)transform.position).normalized; // Get direction to player
        RB.velocity = direction * MoveSpeed; // Move in that direction
    }

    public void KeepDistance(float distancingValue) // Make sure to override this for enemies that need to keep distance from each other
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, distancingValue, LayerMask.GetMask("Enemy"));

        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                Vector2 backwardDir = (colliders[i].transform.position - this.transform.position).normalized;

                colliders[i].gameObject.GetComponent<Rigidbody2D>().AddForce(backwardDir * 37.5f, ForceMode2D.Force); // 37.5 is a decent number for now.
            }
        }
    }

    public void LookAtPlayer(Vector2 playerPos)
    {
        Vector2 lookDir = playerPos - (Vector2)this.transform.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; // Will need to adjust when you get home

        RB.rotation = angle;
    }
    #endregion 

    public virtual void PatrolPlayer() { }
    public virtual void ChasePlayer() { } 
    public virtual void ShootPlayer() { } 

}
