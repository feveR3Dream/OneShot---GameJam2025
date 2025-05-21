using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakpointSpawner : MonoBehaviour
{
    [SerializeField] GameObject weakpoint;
    [SerializeField] int MaximumWeakpoint;
    [SerializeField] List<GameObject> activeWeakpoints = new();

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossHurt>(Spawn);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossHurt>(Spawn);
    }

    private void Spawn(BossHurt context)
    {
        // Clear previous weakpoints
        foreach (GameObject wp in activeWeakpoints)
        {
            if (wp != null)
                Destroy(wp);
        }
        activeWeakpoints.Clear();

        int count = UnityEngine.Random.Range(1, MaximumWeakpoint + 1);

        for (int i = 0; i < count; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f));
            GameObject spawned = Instantiate(weakpoint, transform.position, rotation);
            activeWeakpoints.Add(spawned);
        }
    }
}
