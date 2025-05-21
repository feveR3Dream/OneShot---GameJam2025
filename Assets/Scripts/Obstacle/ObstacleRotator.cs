using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour
{
    [SerializeField] List<GameObject> rings;
    [SerializeField] int pointPool;
    bool isActive = true;

    private List<(GameObject obj, float speed, int dir)> rotations = new();
    private readonly Dictionary<float, int> options = new()
    {
        { 30f, 1},
        { 20f, 2},
        { 15f, 4},
        { 10f, 6}
    };

    void Start()
    {
        int pool = pointPool;
        var times = new List<float>(options.Keys);

        for (int i = 0; i < rings.Count; i++)
        {
            List<float> valid = times.FindAll(t => options[t] <= pool);
            float choice;

            if (valid.Count == 0)
            {
                // Not enough points, default to 10s (1 point)
                choice = 10f;
            }
            else
            {
                choice = (i == rings.Count - 1) ? valid[valid.Count - 1] : valid[Random.Range(0, valid.Count)];
                pool -= options[choice];
            }

            float degPerSec = 360f / choice;
            int dir = Random.value > 0.5f ? 1 : -1;

            rotations.Add((rings[i], degPerSec, dir));
        }
    }

    void FixedUpdate()
    {
        if(!isActive) return;

        foreach (var r in rotations)
        {
            if (r.obj != null)
                r.obj.transform.Rotate(0f, 0f, r.speed * r.dir * Time.fixedDeltaTime);
        }
    }

    public void Halt()
    {
        isActive = false;
    }

    public void Continue()
    {
        isActive = true;
    }
}