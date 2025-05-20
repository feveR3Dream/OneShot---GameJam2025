using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Projectile : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] public float currentSpeed;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] LayerMask weakspot;

    private void FixedUpdate()
    {
        _rb.velocity = currentSpeed * transform.right;
        currentSpeed = _rb.velocity.magnitude;
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((weakspot & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("is weak");
        }
        else
        {
            Debug.Log("is not weak");
        }
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