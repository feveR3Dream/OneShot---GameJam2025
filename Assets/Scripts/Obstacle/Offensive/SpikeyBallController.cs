using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyBallController : MonoBehaviour
{
    private GameObject _player;
    private GameObject _boss;
    public GameObject Owner;

    private Rigidbody2D _rb;

    [SerializeField] private float speed = 45f;
    [SerializeField] private float returnSpeed = 60f;
    [SerializeField] private int returnNumber = 3;
    [SerializeField] private LayerMask projectile;

    private bool _canLaunch = false;
    private bool _canReturn = false;

    private Vector2 _launchDirection;
    private Vector2 _targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _boss = GameObject.Find("Boss");
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null) return;
        CheckPlayer();
    }

    private void FixedUpdate()
    {
        if (_canLaunch && !_canReturn)
        {
            _rb.velocity = speed * _launchDirection;

            if (HasReachedTarget(_targetPosition))
            {
                SetReturn();
            }
        }

        else if (_canReturn)
        {
            _rb.velocity = returnSpeed * _launchDirection;

            if (HasReachedTarget(_targetPosition))
            {
                SetReset();
            }
        }
    }

    private void CheckPlayer()
    {
        if (_launchDirection != Vector2.zero || _canLaunch || _canReturn) return;

        Vector2 toPlayer = (Vector2)_player.transform.position - (Vector2)transform.position;
        Vector2 fromBoss = (Vector2)transform.position - (Vector2)_boss.transform.position;
        Vector2 bossToBall = fromBoss.normalized;
        Vector2 bossToPlayer = ((Vector2)_player.transform.position - (Vector2)_boss.transform.position).normalized;

        bool sameSide = Vector2.Dot(bossToBall, bossToPlayer) > 0.99f;
        bool farEnough = toPlayer.magnitude > 1.5f;
        // change 1.5f to whatever buffer you like (so it doesn’t launch if player is literally on top of the ball)

        if (sameSide && farEnough)
        {
            _launchDirection = toPlayer.normalized;
            _targetPosition = (Vector2)_player.transform.position + _launchDirection * Random.Range(0f, 20f);
            _canLaunch = true;
        }
    }

    private void SetReturn()
    {
        _canLaunch = false;
        _canReturn = true;
        _launchDirection = (Owner.transform.position - transform.position).normalized;
        _targetPosition = Owner.transform.position;
    }

    private void SetReset()
    {
        returnNumber--;
        if (returnNumber == 0)
            Destroy(gameObject);

        _rb.velocity = Vector2.zero;
        _launchDirection = Vector2.zero;
        _targetPosition = Vector2.zero;
        _canReturn = false;
        _canLaunch = false;
    }

    private bool HasReachedTarget(Vector2 target)
    {
        Vector2 toTarget = target - (Vector2)transform.position;
        return toTarget.magnitude < 0.25f || Vector2.Dot(toTarget.normalized, _launchDirection) < 0.0f;
    }
}
