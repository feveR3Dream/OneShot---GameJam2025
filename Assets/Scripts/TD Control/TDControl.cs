using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class TDControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D bodyColl;

    [Header("Values")]
    [SerializeField] private float lineRenderDistance;

    [Header("Player Movement Info")]
    public float Acceleration;
    public float Deceleration;
    public float MovementSpeed;

    private Rigidbody2D rb;
    [SerializeField] private Transform Boss;

    // Movement Vars
    private Vector2 moveVelocity;
    private Vector2 targetDir;




    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossDamaged>(PlayerKnockback);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossDamaged>(PlayerKnockback);
    }


    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossDamaged>(PlayerKnockback);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossDamaged>(PlayerKnockback);
    }


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
        //AutoLookAtBoss();
        PlayerLook();
    }

    private void Update()
    {
        Debug.DrawLine((Vector2)transform.position + targetDir / 1.5f, (Vector2)transform.position + targetDir * lineRenderDistance, Color.red);
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
        rb.rotation = angle;
    }

    private void PlayerLook()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        targetDir = (mousePos - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        rb.rotation = angle;


    }

    private void PlayerKnockback(BossDamaged e)
    {
        Vector2 pushdirection = gameObject.transform.position - Boss.position;
        Vector2 multipliedforce = pushdirection.normalized * 10f;

        rb.AddForce(multipliedforce, ForceMode2D.Impulse);
        Debug.Log("Kncokback applied");

    }
}
