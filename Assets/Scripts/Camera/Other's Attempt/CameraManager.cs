using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;


    [Header("Values")]
    [SerializeField] private float lengthPercent; // Who?
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float distanceThreshold;

    // Camera
    private Camera m_Camera;

    // Boolean
    private bool functionable = true;

    void Start()
    {
        m_Camera = Camera.main;

        m_Camera.transform.position = new Vector3(boss.position.x, boss.position.y, m_Camera.transform.position.z);

        if (player == null || boss == null)
        {
            functionable = false;
            Debug.Log("Player or Boss are missing");
        }
    }


    void Update()
    {
        if (!functionable) return;

        PivotToPlayer();
    }

    private void PivotToPlayer()
    {
        Vector3 bossPos = boss.position;
        Vector3 playerPos = player.position;

        Vector3 targetDir = playerPos - bossPos;
        float distance = targetDir.magnitude;

        if (distance >= distanceThreshold)
        {
            targetDir = targetDir.normalized * distanceThreshold;
        }

        Vector3 newPos = bossPos + targetDir;
        Vector3 middlePos = Vector3.Lerp(bossPos, newPos, lengthPercent);

        Vector3 lerped = Vector3.Lerp(m_Camera.transform.position, new Vector3(middlePos.x, middlePos.y, m_Camera.transform.position.z), lerpSpeed * Time.deltaTime);
        m_Camera.transform.position = lerped;
    }

}
