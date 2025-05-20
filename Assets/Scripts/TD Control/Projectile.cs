using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class Projectile : MonoBehaviour
{
    //async Task CountDownDestroy()
    //{
    //    await Task.Delay(2000); // Wait 2000 milliseconds = 2 seconds
    //    if (gameObject != null)
    //    {
    //        EventDispatcher.Instance.SendEvent(new BulletSpawn());
    //        Destroy(gameObject);
    //    }
    //}

    private Coroutine deleteCoroutine = null;
    private bool isWeakSpot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Boss" || (LayerMask.LayerToName(collision.gameObject.layer) == "WeakSpot"))
        {
            if (deleteCoroutine != null)
            {
                Debug.Log("Stop");
                StopCoroutine(deleteCoroutine);
                deleteCoroutine = null;
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
