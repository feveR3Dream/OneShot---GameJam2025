using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEyeController : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private float Cooldown;

    private Rigidbody2D rb;
    private Vector2 targetDir;
    private float timeBeforeFire;
    private bool disable = false;

    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        timeBeforeFire = Cooldown;
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
        if (timeBeforeFire <= 0)
            FireBeam();
        else
            timeBeforeFire -= Time.deltaTime;
    }

    private void FireBeam()
    {
        GameObject bullet = Instantiate(beamPrefab, firingPoint.transform.position, firingPoint.transform.rotation);
        timeBeforeFire = Cooldown;
    }

    private void LookAtPlayer()
    {
        targetDir = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        rb.rotation = angle;
    }
}
