using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask targetLayer;

    // Coroutine
    private Coroutine deleteCoroutine = null;

    // Check Pram
    private bool _isWeakSpot;
    private int weakSpotLayer;

    private void Start()
    {
        weakSpotLayer = LayerMask.NameToLayer("WeakSpot");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {

            _isWeakSpot = collision.gameObject.layer == weakSpotLayer;
            //Debug.Log(LayerMask.LayerToName(collision.gameObject.layer));

            if (deleteCoroutine != null)
            {
                Debug.Log("Stop");
                StopCoroutine(deleteCoroutine);
                deleteCoroutine = null;
            }

            if (_isWeakSpot)
            {
                Debug.Log("Hit a weak spot!");
            }
            deleteCoroutine = StartCoroutine(AutoDelete(0f)); // Instant Delete

        }
    }

    private IEnumerator AutoDelete(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        EventDispatcher.Instance.SendEvent(new BulletSpawn { timer = 1f });
        Destroy(gameObject);
       
    }

    public void Initiate()
    {
        deleteCoroutine = StartCoroutine(AutoDelete(3f));
    }

}
