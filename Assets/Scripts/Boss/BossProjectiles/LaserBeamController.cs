using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamController : MonoBehaviour
{
    [SerializeField] private GameObject pickUpPrefab;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Material warningBeam;
    [SerializeField] private Material realBeam;
    [SerializeField] private float setBeamLength;
    [SerializeField] private float setWarningTimer;
    private GameObject target;
    // Start is called before the first frame update
    private void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        EventDispatcher.Instance.Subscribe<BossWhiffed>(FireBeam);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossWhiffed>(FireBeam);
    }

    private void FireBeam(BossWhiffed e)
    {
        lineRenderer.enabled = true;
        StartCoroutine(SetBeamRaycast());
    }

    private IEnumerator SetBeamRaycast()
    {
        Vector2 dir = target.transform.position - transform.position;
        Vector2 point = Vector2.Lerp(transform.position, target.transform.position, Random.Range(0.8f,1f));

        lineRenderer.SetPosition(1, dir * setBeamLength);
        yield return StartCoroutine(SetWarningBeam());
        StartCoroutine(SetRealBeam(dir));

        yield return new WaitForSeconds(0.4f);
        Vector2 randomPos = new Vector2(point.x + Random.Range(-3f, 3f), point.y + Random.Range(-3f, 3f));
        Instantiate(pickUpPrefab, randomPos, Quaternion.identity);
    }

    private IEnumerator SetWarningBeam()
    {
        lineRenderer.SetMaterials(new List<Material> { warningBeam });
        Color color = lineRenderer.material.color;
        float opacity = 0f;
        while (opacity <= 1f)
        {
            opacity += 0.05f;
            Color newColor = new Color(color.r, color.g, color.b, opacity);
            lineRenderer.material.color = newColor;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator SetRealBeam(Vector2 dir)
    {
        lineRenderer.SetMaterials(new List<Material> { realBeam });
        Ray2D ray = new Ray2D(transform.position, dir);
        float angle = Mathf.Atan2(ray.direction.y, ray.direction.x) * Mathf.Rad2Deg;
        // Lingering attack
        for (int i = 0; i < 10; i++)
        {
            RaycastHit2D hit = Physics2D.BoxCast(ray.origin, new Vector2(4, 8), angle, ray.direction, 100f, layerMask);
            
            DebugDrawBoxCast(ray.origin, new Vector2(4, 8), angle, ray.direction, 100f, Color.red);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player") == true)
            {

                //Destroy(hit.collider.gameObject);
                Debug.Log("Hit target");
            }
            yield return new WaitForSeconds(0.15f);
        }
        lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
        lineRenderer.enabled = false;
    }



    void DebugDrawBoxCast(Vector2 origin, Vector2 size, float angleDeg, Vector2 direction, float distance, Color color, float duration = 0.1f)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angleDeg);
        Vector2 halfSize = size * 0.5f;

        // Corners of the box at origin
        Vector2 topLeft = origin + (Vector2)(rotation * new Vector3(-halfSize.x, halfSize.y));
        Vector2 topRight = origin + (Vector2)(rotation * new Vector3(halfSize.x, halfSize.y));
        Vector2 bottomLeft = origin + (Vector2)(rotation * new Vector3(-halfSize.x, -halfSize.y));
        Vector2 bottomRight = origin + (Vector2)(rotation * new Vector3(halfSize.x, -halfSize.y));


        Vector2 offset = direction.normalized * distance;

        // Draw origin box
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);

        // Draw end box
        Debug.DrawLine(topLeft + offset, topRight + offset, color, duration);
        Debug.DrawLine(topRight + offset, bottomRight + offset, color, duration);
        Debug.DrawLine(bottomRight + offset, bottomLeft + offset, color, duration);
        Debug.DrawLine(bottomLeft + offset, topLeft + offset, color, duration);

        // Draw sides
        Debug.DrawLine(topLeft, topLeft + offset, color, duration);
        Debug.DrawLine(topRight, topRight + offset, color, duration);
        Debug.DrawLine(bottomLeft, bottomLeft + offset, color, duration);
        Debug.DrawLine(bottomRight, bottomRight + offset, color, duration);
    }

}
