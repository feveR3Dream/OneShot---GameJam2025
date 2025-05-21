using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEyeController : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private GameObject firingPoint;
    private GameObject player;
    [SerializeField] private LineRenderer lineRenderer;

    private Rigidbody2D rb;
    private Vector2 targetDir;
    private float timeBeforeFire;
    private bool disable = false;

    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.enabled = false;
        timeBeforeFire = 3;
        if (player == null)
        {
            disable = true;
        }
    }

    private void FixedUpdate()
    {
        if (disable) return;
        CooldownProcessor();
        LookAtPlayer();
    }

    private void CooldownProcessor()
    {
        if (timeBeforeFire <= 1.5f)
            FireWarningBeam();
        if (timeBeforeFire <= 0)
            FireBeam();
        else
            timeBeforeFire -= Time.deltaTime;

    }

    private void FireWarningBeam()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firingPoint.transform.position);
        lineRenderer.SetPosition(1, firingPoint.transform.position + firingPoint.transform.right * 10f);
    }

    private void FireBeam()
    {
        lineRenderer.enabled = false;
        GameObject bullet = Instantiate(beamPrefab, firingPoint.transform.position, firingPoint.transform.rotation);
        timeBeforeFire = Random.Range(2,5);
    }

    private void LookAtPlayer()
    {
        targetDir = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        rb.rotation = angle;
    }
}
