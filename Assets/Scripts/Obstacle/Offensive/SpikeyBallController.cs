using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyBallController : MonoBehaviour
{
    private GameObject _player;
    private GameObject _boss;
    public GameObject Owner;

    private Rigidbody2D _rb;

    [SerializeField] private float speed = 35f;
    [SerializeField] private float returnSpeed = 50f;
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
        CheckerViet();
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

    private void CheckerViet()
    {
        if (_launchDirection != Vector2.zero || _canLaunch || _canReturn) return;

        Vector2 toPlayer = _player.transform.position - transform.position;
        Vector2 fromBoss = transform.position - _boss.transform.position;

        bool aligned = Vector2.Dot(toPlayer.normalized, fromBoss.normalized) > 0.99f;

        float loosePercentage = 0.25f;
        float diff = Mathf.Abs(fromBoss.magnitude - toPlayer.magnitude);
        float avg = (fromBoss.magnitude + toPlayer.magnitude) / 2f;
        bool isBetween = diff / avg < loosePercentage;

        if (aligned && isBetween)
        {
            _launchDirection = toPlayer.normalized;
            _targetPosition = _player.transform.position;
            _canLaunch = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((projectile & (1 << collision.gameObject.layer)) != 0)
        {
            SetReturn();
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
