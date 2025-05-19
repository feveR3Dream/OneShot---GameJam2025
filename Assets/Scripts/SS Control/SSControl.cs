using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class SSControl : MonoBehaviour
{

    [Header("References")]
    public SSMovementStats MoveStats; // Retrieve the stats
    [SerializeField] private Collider2D feetColl;
    [SerializeField] private Collider2D bodyColl;


    private Rigidbody2D rb;


    // Movement Vars
    private Vector2 moveVelocity; // Temp
    private bool isFacingRight;


    // Collision Check Vars
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private bool isGrounded;
    private bool bumpedHead;


    // Jump Variable
    public float VerticalVelocity { get; private set; }
    private bool isJumping;
    private bool isFastFalling;
    private bool isFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    private int numberOfJumpsUsed;


    // Apex Variable
    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;


    // Jump Buffer Variable
    private float jumpBufferTimer;
    private bool jumpReleasedDuringBuffer;


    // Coyote Time Variable 
    private float coyoteTimer;


    private void Awake()
    {
        isFacingRight = true;

        rb = GetComponent<Rigidbody2D>();

        rb.mass = 0; 
        rb.gravityScale = 0; 
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        CountTimers();
        JumpChecks();
    }

    private void FixedUpdate()
    {
        GroundCollisionCheck();
        BumpedHead();
        Jump();

        if (isGrounded)
            PlayerMove(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Move );
         else
            PlayerMove(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Move );
    }

    #region Movement

    private void PlayerMove(float acceleration, float deceleration, Vector2 moveInput) 
    {
        // Check movement input val
        if (moveInput != Vector2.zero)
        {
            CheckTurn(moveInput);

            Vector2 targetVelocity = Vector2.zero;

            if (InputManager.Run)
            {
                targetVelocity = new Vector2(moveInput.x, 0) * MoveStats.RunSpeed;
            }
            else
                targetVelocity = new Vector2(moveInput.x, 0) * MoveStats.WalkSpeed;



            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.deltaTime);
            rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);
        }

        else if (moveInput == Vector2.zero)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.deltaTime);
            rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);
        }
    }


    private void CheckTurn(Vector2 moveInput)
    {
        if (isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool isRight)
    {
        if (isRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region Jump
    private void JumpChecks()
    {
        // WHEN WE PRESS THE JUMP BUTTON
        if (InputManager.PressJump)
        {
            jumpBufferTimer = MoveStats.JumpBufferTime;
            jumpReleasedDuringBuffer = false;

        } 

        // WHEN WE RELEASE THE JUMP BUTTON
        if (InputManager.ReleaseJump)
        {
            if (jumpBufferTimer > 0f)
            {
                jumpReleasedDuringBuffer = true;
            }

            if (isJumping && VerticalVelocity > 0f)
            {
                if (isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = MoveStats.TimeForUpwardCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = VerticalVelocity;
                }

            }
        }

        // INITIATE JUMP WITH JUMP BUFFERING AND COYOTE TIME
        if (jumpBufferTimer > 0f && !isJumping && (isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);
            
            if (jumpReleasedDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = VerticalVelocity; 

            }
        }


        // DOUBLE JUMP
        else if (jumpBufferTimer > 0f && isJumping && numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
        {
            isFastFalling = false;
            InitiateJump(1);
        }

        // AIR JUMP AFTER COYOTE TIME LAPSED
        else if (jumpBufferTimer > 0f && isFalling && numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
        {
            InitiateJump(2);
            isFastFalling = false;
        }


        // LANDED
        if ((isJumping || isFalling) && isGrounded && VerticalVelocity <= 0f)
        {
            isJumping = false;
            isFalling = false;
            isFastFalling = false;
            fastFallTime = 0f;
            isPastApexThreshold = false;
            numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }

    }

    private void InitiateJump(int jumpsUsed)
    {
        if (!isJumping)
        {
            isJumping = true;
        }

        jumpBufferTimer = 0f;
        numberOfJumpsUsed += jumpsUsed;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        // APPLY GRAVITY WHILE JUMPING
        if (isJumping)
        {
            // CHECK FOR HEAD BUMP
            if (bumpedHead)
            {
                isFastFalling = true;
            }

            // GRAVITY ON ASCENDING
            if (VerticalVelocity >= 0f)
            {
                // APEX CONTORLS
                apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (apexPoint > MoveStats.ApexThreshold)
                {
                    if (!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if (isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;

                        if (timePastApexThreshold < MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;

                        }
                    }
                }

                // GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
                else
                {
                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;

                    if (isPastApexThreshold)
                    {
                        isPastApexThreshold = false;
                    }
                }

            }

            // GRAVITY ON DESCENDING
            else if (!isFastFalling)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelocity < 0f) 
            {
                if (!isFalling)
                {
                    isFalling = true;
                }
            }
        }

        // JUMP CUT
        if (isFastFalling)
        {
            if (fastFallTime >= MoveStats.TimeForUpwardCancel)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (fastFallTime < MoveStats.TimeForUpwardCancel)
            {
                VerticalVelocity = Mathf.Lerp(fastFallReleaseSpeed, 0f, (fastFallTime / MoveStats.TimeForUpwardCancel));
            }

            fastFallTime += Time.fixedDeltaTime;
        }


        // NORMAL GRAVITY WHILE FALLING
        if (!isGrounded && !isJumping)
        {
            if (!isFalling)
            {
                isFalling = true;
            }

            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }

        // CLAMP FALL SPEED
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);

        rb.velocity = new Vector2(rb.velocity.x, VerticalVelocity);


    }
    #endregion

    #region Collision Checking

    private void GroundCollisionCheck()
    {
        Vector2 boxCastOrigin = new Vector2(feetColl.bounds.center.x, feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f,  Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

        if (groundHit.collider != null)
        {
            isGrounded = true;
        }
        else isGrounded = false;

        #region Debug Visualization
        if (MoveStats.DebugShowGroundedBox)
        {
            Color rayColor;

            if (isGrounded)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Debug.DrawLine(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawLine(new Vector2(boxCastOrigin.x + boxCastOrigin.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawLine(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * MoveStats.GroundDetectionRayLength, rayColor);
        }
        #endregion
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(feetColl.bounds.center.x, bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);

        if (headHit.collider != null)
        {
            bumpedHead = true;
        }
        else bumpedHead = false;

        #region Debug Visualization
        if (MoveStats.DebugShowGroundedBox)
        {
            float headWidth = MoveStats.HeadWidth;

            Color rayColor;

            if (bumpedHead)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Debug.DrawLine(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawLine(new Vector2(boxCastOrigin.x + (boxCastOrigin.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawLine(new Vector2(boxCastOrigin.x - boxCastOrigin.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
        }
        #endregion
    }

    #endregion

    #region Timers
    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if (isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else { coyoteTimer = MoveStats.JumpCoyoteTime; }
    }

    #endregion
}
