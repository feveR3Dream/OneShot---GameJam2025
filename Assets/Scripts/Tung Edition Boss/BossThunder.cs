using UnityEngine;

public class BossThunder : MonoBehaviour
{
    [SerializeField] GameObject thunder;     // First prefab
    [SerializeField] GameObject pickup;   // Second prefab

    [SerializeField] int spotCount;         // Number of spots
    [SerializeField] float minRadius = 3f;      // Donut inner edge
    [SerializeField] float maxRadius = 6f;      // Donut outer edge

    private Vector2[] spawnPositions;

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossWhiffed>(FireAction);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossWhiffed>(FireAction);
    }

    public void FireAction(BossWhiffed context)
    {
        spawnPositions = new Vector2[spotCount];

        for (int i = 0; i < spotCount; i++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minRadius, maxRadius);
            Vector2 position = direction * distance;

            spawnPositions[i] = position;
            Instantiate(thunder, position, Quaternion.identity);
        }

        // Pick one spot to spawn the centerPrefab
        int randomIndex = Random.Range(0, spotCount);
        Instantiate(pickup, spawnPositions[randomIndex], Quaternion.identity);
    }
}