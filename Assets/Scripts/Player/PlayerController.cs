using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private float Acceleration;
    [SerializeField] private float Deceleration;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float lineRenderDistance;

    // Movement Vars
    private Vector2 moveVelocity;
    private Vector2 targetDir;
    [SerializeField] private bool _canMove = true;



    private void FixedUpdate()
    {
        PlayerLook();
        if (_canMove)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            PlayerMove(Acceleration, Deceleration, moveInput);
        }
    }
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

    private void PlayerLook()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        targetDir = (mousePos - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        rb.rotation = angle;
    }

    public Rigidbody2D GetRigidBody()
    {
        return rb;
    }

    public void SetCanMove(bool newValue)
    {
        _canMove = newValue;
    }

    public void SetCollider2D(bool newValue)
    {
        circleCollider.enabled = newValue;
    }

    /*
    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossDamaged>(PlayerKnockback);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossDamaged>(PlayerKnockback);
    } 
    
    private void PlayerKnockback(BossDamaged e)
    {
        _canMove = false;
        Vector2 pushdirection = gameObject.transform.position - Boss.position;
        Vector2 multipliedforce = pushdirection.normalized * 0.25f;

        rb.AddForce(multipliedforce, ForceMode2D.Force);
        Debug.Log("Kncokback applied");
        StartCoroutine(KnockbackRecovery(0.5f));
    }

    private IEnumerator KnockbackRecovery(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        _canMove = true;
    }
    */
}
