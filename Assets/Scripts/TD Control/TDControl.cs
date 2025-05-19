using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class TDControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D bodyColl;

    [Header("Player Movement Info")]
    public float Acceleration;
    public float Deceleration;
    public float MovementSpeed;

    private Rigidbody2D rb;
    [SerializeField] private Transform Boss;
    [SerializeField] private Transform AimPos1;
    [SerializeField] private Transform AimPos2;

    // Movement Vars
    private Vector2 moveVelocity; // Temp


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.mass = 0;
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate()
    {
       PlayerMove(Acceleration, Deceleration, InputManager.Move);
    }

    private void LateUpdate()
    {
        AutoLookAtBoss();
    }

    #region Movement

    private void PlayerMove(float acceleration, float deceleration, Vector2 moveInput)
    {
        // Check movement input val
        if (moveInput != Vector2.zero)
        {

            Vector2 targetVelocity = Vector2.zero;
            targetVelocity = new Vector2(moveInput.x, moveInput.y) * MovementSpeed;

            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.deltaTime);
            rb.velocity = new Vector2(moveVelocity.x, moveVelocity.y);
        }

        else if (moveInput == Vector2.zero)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.deltaTime);
            rb.velocity = new Vector2(moveVelocity.x, moveVelocity.y);
        }
    }

    #endregion

    private void AutoLookAtBoss()
    {
        Vector2 direction = Boss.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void AimTuning()
    {

    }

}
