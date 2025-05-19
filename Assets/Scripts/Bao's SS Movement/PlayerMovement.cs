using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; //Downwards force (gravity) 
    [HideInInspector] public float gravityScale; //Gravity multiplier

    [Space(5)]
    public float fallGravityMult; //Multiplier to the player's gravityScale when falling.
    public float maxFallSpeed; //Maximum fall speed 
    [Space(5)]
    public float fastFallGravityMult; //fast fall multiplier when player fall down with multiplier

    public float maxFastFallSpeed; //Maximum fall speed

    [Space(20)]

    [Header("Run")]
    public float runMaxSpeed; //Target speed we want the player to reach.
    public float runAcceleration; //The speed at which our player accelerates to max speed
    [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    public float runDecceleration; //The speed at which our player decelerates from their current speed
    [HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
    [Space(5)]
    [Range(0f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
    [Range(0f, 1)] public float deccelInAir;
    [Space(5)]
    public bool doConserveMomentum = true;

    [Space(20)]

    [Header("Jump")]
    public float jumpHeight;
    public float jumpTimeToApex; //Time between applying the jump force and reaching the desired jump height. 
    [HideInInspector] public float jumpForce;

    [Header("Both Jumps")]
    public float jumpCutGravityMult; //Multiplier to determine how high the player jump
    [Range(0f, 1)] public float jumpHangGravityMult; //Multiplier to gravity to keep player hang when at the peak of the jump
    public float jumpHangTimeThreshold; //Speeds (close to 0) where the player will experience extra "jump hang". 
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce;
    [Space(5)]
    [Range(0f, 1f)] public float wallJumpRunLerp; //Reduces the effect of player's movement while wall jumping.
    [Range(0f, 1.5f)] public float wallJumpTime; //Time after wall jumping the player's movement is slowed for.
    public bool doTurnOnWallJump;

    [Space(20)]

    [Header("Slide")]
    public float slideSpeed;
    public float slideAccel;

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime;
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.

    [Space(20)]

    [Header("Dash")]
    public int dashAmount;
    public float dashSpeed;
    public float dashSleepTime; //Duration for which the game freezes when we press dash but before we read directional input and apply a force
    [Space(5)]
    public float dashAttackTime;
    [Space(5)]
    public float dashEndTime; //Time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state)
    public Vector2 dashEndSpeed; //Slows down player, makes dash feel more responsive (used in Celeste)
    [Range(0f, 1f)] public float dashEndRunLerp; //Slows the affect of player movement while dashing
    [Space(5)]
    public float dashRefillTime;
    [Space(5)]
    [Range(0.01f, 0.5f)] public float dashInputBufferTime;

    #region Ref
    //Components
    public Rigidbody2D rb2D { get; private set; }

    //Variables control the various actions the player can perform at any time.
    //These are fields which can are public allowing for other sctipts to read them
    //but can only be privately written to.
    public bool IsFacingRight;
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsSliding { get; private set; }
    public bool IsDashing { get; private set; }

    //Timers (also all fields, could be private and a method returning a bool could be used)
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    //Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    //Wall Jump
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;
    private float _wallJumpLockCounter;
    private float _wallJumpLockTime = 0.15f;

    //Dash
    private int _dashesLeft;
    private bool _dashRefilling;
    [HideInInspector] public Vector2 _lastDashDir;
    private bool _isDashAttacking;
    private TrailRenderer _trailRenderer;

    [HideInInspector] public Vector2 _moveInput;
    public float LastPressedJumpTime { get; private set; }

    public float LastPressedDashTime { get; private set; }

    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;

    #endregion

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }
    void Start()
    {
        IsFacingRight = true;
        Debug.Log($"[RESPAWN] moveInput: {_moveInput}, lastDashDir: {_lastDashDir}, isFacingRight: {IsFacingRight}");

    }

    void Update()
    {
        #region Timer
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        #endregion

        #region Input

        HandleInput();
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C))
        {
            OnJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.C))
        {
            OnJumpUpInput();
        }

        if (_moveInput.x != 0)
        {
            CheckDirectionToFace(_moveInput.x > 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.RightShift))
        {
            OnDashInput();
        }
        #endregion

        #region Collision
        if (!IsDashing && !IsJumping)
        {
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping)
            {
                LastOnGroundTime = coyoteTime;
            }

            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight) || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
            {
                LastOnWallRightTime = coyoteTime;
            }

            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight) || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
            {
                LastOnWallLeftTime = coyoteTime;
            }

            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }

        #endregion

        #region Check
        if (IsJumping && rb2D.velocity.y < 0)
        {
            IsJumping = false;
            if (!IsWallJumping)
            {
                _isJumpFalling = true;
            }
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > wallJumpTime)
        {
            IsWallJumping = false;
        }

        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;
            if (!IsJumping)
            {
                _isJumpFalling = false;
            }
        }

        JumpInGeneral();

        #endregion

        #region slide check

        if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
        {
            IsSliding = true;
        }
        else
        {
            IsSliding = false;
        }

        #endregion

        #region dash check

        if (CanDash() && LastPressedDashTime > 0)
        {
            Sleep(dashSleepTime);
            if (_moveInput != Vector2.zero)
            {
                _lastDashDir = _moveInput;
            }
            else
            {
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;
            }

            IsDashing = true;
            IsJumping = false;
            IsWallJumping = false;
            _isJumpCut = false;

            StartCoroutine(nameof(StartDash), _lastDashDir);
        }

        #endregion

        #region gravity

        if (!_isDashAttacking)
        {
            if (IsSliding)
            {
                SetGravityScale(0);
            }
            else if (rb2D.velocity.y < 0 && _moveInput.y < 0)
            {
                SetGravityScale(gravityScale * fastFallGravityMult);
                rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Max(rb2D.velocity.y, -maxFallSpeed));
            }
            else if (_isJumpCut)
            {
                SetGravityScale(gravityScale * jumpCutGravityMult);
                rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Max(rb2D.velocity.y, -maxFallSpeed));
            }
            else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(rb2D.velocity.y) < jumpHangTimeThreshold)
            {
                SetGravityScale(gravityScale * jumpHangGravityMult);
            }
            else if (rb2D.velocity.y < 0)
            {
                SetGravityScale(gravityScale * fallGravityMult);
                rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Max(rb2D.velocity.y, -maxFallSpeed));
            }
            else
            {
                SetGravityScale(gravityScale);
            }
        }
        else
        {
            SetGravityScale(0);
        }

        #endregion
    }

    private void FixedUpdate()
    {
        if (!IsDashing)
        {
            if (IsWallJumping)
            {
                Run(wallJumpRunLerp);
            }
            else
            {
                Run(1);
            }
        }
        else if (_isDashAttacking)
        {
            Run(dashEndRunLerp);
        }


        if (IsSliding)
        {
            Slide();
        }
    }

    private void HandleInput()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        Debug.Log($"x:{_moveInput.x}  y:{_moveInput.y}");
    }

    private void Run(float lerpAmount)
    {
        float targetSpeed = _moveInput.x * runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb2D.velocity.x, targetSpeed, lerpAmount);
        float accelRate;
        if (LastOnGroundTime > 0)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        }
        else
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;
        }

        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(rb2D.velocity.y) < jumpHangTimeThreshold)
        {
            accelRate *= jumpHangAccelerationMult;
            targetSpeed *= jumpHangMaxSpeedMult;
        }

        if (doConserveMomentum && Mathf.Abs(rb2D.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb2D.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            accelRate = 0;
        }
        float speedDiff = targetSpeed - rb2D.velocity.x;

        float movement = speedDiff * accelRate;
        rb2D.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        IsFacingRight = !IsFacingRight;
    }
    private void JumpInGeneral()
    {
        if (!IsDashing)
        {
            if (CanJump() && LastPressedJumpTime > 0f)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                JumpAction();
            }
            else if (CanWallJump() && LastPressedJumpTime > 0f)
            {
                IsWallJumping = true;
                IsJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                _wallJumpStartTime = Time.time;
                _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
                WallJumpAction(_lastWallJumpDir);
            }
        }
    }

    private void OnJumpInput()
    {
        LastPressedJumpTime = jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
        {
            _isJumpCut = true;
        }
    }

    public void OnDashInput()
    {
        LastPressedDashTime = dashInputBufferTime;
    }
    private void JumpAction()
    {
        Debug.Log("jump");
        LastPressedJumpTime = 0f;
        LastOnGroundTime = 0f;
        float force = jumpForce;
        if (rb2D.velocity.y < 0)
        {
            force -= rb2D.velocity.y;
        }
        rb2D.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void WallJumpAction(int dir)
    {
        Debug.Log("WallJump");
        LastPressedJumpTime = 0f;
        LastOnGroundTime = 0f;
        LastOnWallRightTime = 0f;
        LastOnWallLeftTime = 0f;
        Vector2 force = new Vector2(wallJumpForce.x, wallJumpForce.y);
        force.x *= dir;

        if (Mathf.Sign(rb2D.velocity.x) != Mathf.Sign(force.x))
        {
            force.x -= rb2D.velocity.x;
        }

        if (rb2D.velocity.y < 0)
        {
            force.y -= rb2D.velocity.y;
        }
        rb2D.AddForce(force, ForceMode2D.Impulse);
    }

    private IEnumerator StartDash(Vector2 dir)
    {
        if (_trailRenderer != null) { _trailRenderer.emitting = true; }
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        _dashesLeft--;
        _isDashAttacking = true;

        SetGravityScale(0);

        while (Time.time - startTime <= dashAttackTime)
        {
            rb2D.velocity = dir.normalized * dashSpeed;
            yield return null;
        }

        startTime = Time.time;
        _isDashAttacking = false;

        SetGravityScale(gravityScale);
        rb2D.velocity = dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= dashEndTime)
        {
            yield return null;
        }

        IsDashing = false;
        if (_trailRenderer != null) { _trailRenderer.emitting = false; }

    }

    private IEnumerator RefillDash(int amount)
    {
        _dashRefilling = true;
        yield return new WaitForSeconds(dashRefillTime);
        _dashRefilling = false;
        _dashesLeft = Mathf.Min(dashAmount, _dashesLeft + 1);
    }

    private void Slide()
    {
        float speedDif = slideSpeed - rb2D.velocity.y;
        float movement = speedDif * slideAccel;
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
        rb2D.AddForce(movement * Vector2.up);
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
        {
            Turn();
        }
    }

    #region GeneralMethod

    public void SetGravityScale(float scale)
    {
        rb2D.gravityScale = scale;
    }

    private void Sleep(float duration)
    {
        StartCoroutine(nameof(PerformSleep), duration);
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    private bool CanDash()
    {
        if (!IsDashing && _dashesLeft < dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
        {
            StartCoroutine(nameof(RefillDash), 1);
        }
        return _dashesLeft > 0;
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0f && !IsJumping;
    }

    private bool CanWallJump()
    {
        return LastPressedJumpTime > 0f && LastOnWallTime > 0f && LastOnGroundTime <= 0f && (!IsWallJumping || (LastOnWallRightTime > 0f && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0f && _lastWallJumpDir == -1));
    }

    private bool CanJumpCut()
    {
        return IsJumping && rb2D.velocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return IsWallJumping && rb2D.velocity.y > 0;
    }

    public bool CanSlide()
    {
        if (LastOnWallTime > 0f && !IsJumping && !IsWallJumping && LastOnGroundTime <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    }
}
