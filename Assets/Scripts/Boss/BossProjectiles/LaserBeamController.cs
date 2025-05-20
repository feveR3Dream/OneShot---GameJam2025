using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamController : MonoBehaviour, I_ProjectileHostile
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
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }

    private IEnumerator SetBeamRaycast()
    {
        Vector2 dir = target.transform.position - transform.position;
        Vector2 point = Vector2.Lerp(transform.position, target.transform.position, Random.Range(0.8f,1f));

        lineRenderer.SetPosition(1, dir * setBeamLength);
        yield return StartCoroutine(SetWarningBeam());
        StartCoroutine(SetRealBeam(dir));

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
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player") == true)
        {
            Debug.Log("Hit target");
            Debug.DrawLine(ray.origin, hit.point);
        }
        yield return null;
    }

    private void SetPickUp()
    {

    }

    public void Fire()
    {
        StartCoroutine(SetBeamRaycast());
    }

    public void SetOwner(GameObject obj)
    {

    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }
}
