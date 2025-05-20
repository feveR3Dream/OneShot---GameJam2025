using System.Collections;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask weakspot;


    [Header("Values")]
    [SerializeField] private float slowPercent; // FOR TUNG
    [SerializeField] public float currentSpeed;

    public GunController owner;

    // Booleans
    private bool isBullet = true;
    private void Start()
    {
        Invoke("WhiffedShot", 3f);
    }

    private void FixedUpdate()
    {
        if (!isBullet) return;

        _rb.velocity = currentSpeed * transform.right;
        currentSpeed = _rb.velocity.magnitude;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBullet) return;

        Debug.Log("collided!");
        if ((weakspot & (1 << collision.gameObject.layer)) != 0)
        {
            owner.SetFire(true);
            Debug.Log("is weak");
            EventDispatcher.Instance.SendEvent(new BossHurt());
            //hurt boss
            Destroy(gameObject);
        }
/*        else
        {
            EventDispatcher.Instance.SendEvent(new BossWhiffed());
            Debug.Log("is not weak");
            Destroy(gameObject);
        }*/

        EventDispatcher.Instance.SendEvent(new BossWhiffed());
        Destroy(gameObject);
    }

    private void WhiffedShot()
    {
        Destroy(gameObject);
        EventDispatcher.Instance.SendEvent(new BossWhiffed());
    }
    public void IsBullet(bool bullet)
    {
        isBullet = bullet;
    }
}
/*
{
    [Header("References")]
    [SerializeField] private GunController gunController;
    [SerializeField] private LayerMask targetLayer;

    // Coroutine
    private Coroutine deleteCoroutine = null;

    // Check Pram
    private bool _isWeakSpot;
    private int weakSpotLayer;
    private int pierceAmount;

    private void Start()
    {
        weakSpotLayer = LayerMask.NameToLayer("WeakSpot");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            _isWeakSpot = collision.gameObject.layer == weakSpotLayer;

            DeleteGOCoroutine();

            if (_isWeakSpot)
            {
                // Activate next boss phase
                Debug.Log("Hit a weak spot!");
            }
        }

        else
        {
            if (pierceAmount >= 1)
                pierceAmount--;
            else 
                DeleteGOCoroutine();
        }
    }

    private void DeleteGOCoroutine() // Instant Delete
    {
        if (deleteCoroutine != null)
        {
            Debug.Log("Stop");
            StopCoroutine(deleteCoroutine);
            deleteCoroutine = null;
        }

        deleteCoroutine = StartCoroutine(AutoDelete(0f)); 
    }

    private IEnumerator AutoDelete(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        EventDispatcher.Instance.SendEvent(new BulletSpawn { timer = 1f });
        Destroy(gameObject);
       
    }

    public void Initiate(int pierceVal)
    {
        pierceAmount = pierceVal;
        deleteCoroutine = StartCoroutine(AutoDelete(3f));
    }

}
*/