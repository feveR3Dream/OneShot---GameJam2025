using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [Header("Wall Settings")]
    public GameObject wallPrefab;
    public int wallCount = 6;
    public float radius = 5f;

    [Header("Gap Settings")]
    public int gapCount = 2;
    public Vector2 gapAngleSizeRange = new Vector2(30f, 60f); // Min-Max in degrees

    private List<(float startAngle, float endAngle)> gaps = new List<(float, float)>();

    void Start()
    {
        GenerateRandomGaps();

        int spawned = 0;
        int maxAttempts = 500;

        while (spawned < wallCount && maxAttempts-- > 0)
        {
            float angle = Random.Range(0f, 360f);

            if (IsInGap(angle)) continue;

            Vector3 spawnPos = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0f) * radius;

            GameObject wall = Instantiate(wallPrefab, transform.position + spawnPos, Quaternion.identity);
            wall.transform.parent = transform;
            wall.transform.up = (transform.position - wall.transform.position).normalized;

            spawned++;
        }
    }

    void GenerateRandomGaps()
    {
        gaps.Clear();

        for (int i = 0; i < gapCount; i++)
        {
            float startAngle = Random.Range(0f, 360f);
            float gapSize = Random.Range(gapAngleSizeRange.x, gapAngleSizeRange.y);
            float endAngle = (startAngle + gapSize) % 360f;

            gaps.Add((startAngle, endAngle));
        }
    }

    bool IsInGap(float angle)
    {
        foreach (var gap in gaps)
        {
            float start = gap.startAngle;
            float end = gap.endAngle;

            if (start < end)
            {
                if (angle >= start && angle <= end)
                    return true;
            }
            else
            {
                // Gap wraps around 360 → 0
                if (angle >= start || angle <= end)
                    return true;
            }
        }

        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var gap in gaps)
        {
            DrawGapArc(gap.startAngle, gap.endAngle, radius);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void DrawGapArc(float startAngle, float endAngle, float radius)
    {
        const int segments = 30;
        float totalAngle = (endAngle > startAngle)
            ? endAngle - startAngle
            : 360f - (startAngle - endAngle);

        float step = totalAngle / segments;

        Vector3 prevPoint = transform.position + DirFromAngle(startAngle) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = (startAngle + step * i) % 360f;
            Vector3 nextPoint = transform.position + DirFromAngle(angle) * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    Vector3 DirFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
    }
#endif
}