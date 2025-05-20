using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask weakspot;


    [Header("Values")]
    [SerializeField] private float slowPercent; // FOR TUNG
    [SerializeField] public float currentSpeed;

    // Booleans
    private bool isBullet = true;


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
            Debug.Log("is weak");
            //hurt boss
        }
        else
        {
            Debug.Log("is not weak");
            //check if boss or obstacle
            //if obstacle -> check pierce -> if yes, pierce, if no, big lazer -> spawn thingy
            //if boss -> EVENT DISPATCHER -> big lazer -> spawn thingy
        }
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