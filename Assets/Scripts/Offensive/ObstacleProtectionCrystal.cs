using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleProtectionCrystal : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    private BossWeakpointSpawner weakPointsManager;
    private List<GameObject> weakPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> weakchildPoints = new List<GameObject>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    // Start is called before the first frame update
    void Start()
    {
        weakPointsManager = GameObject.Find("Weakspot Controller").GetComponent<BossWeakpointSpawner>();
        weakPoints = weakPointsManager.GetWeakPoints();
        for (int i = 0; i < weakPoints.Count; i++)
        {
            weakchildPoints.Add(weakPoints[i].transform.GetChild(0).gameObject);
            GameObject newLine = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);
            lines.Add(newLine.GetComponent<LineRenderer>());
            weakchildPoints[i].GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < weakchildPoints.Count; i++)
        {
            if (weakchildPoints[i] != null)
            {
                lines[i].gameObject.transform.position = weakchildPoints[i].transform.position;
                lines[i].SetPosition(0, transform.position);
                lines[i].SetPosition(1, weakchildPoints[i].transform.position);
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < weakchildPoints.Count; i++)
        {
            if (weakchildPoints[i] == null) continue;

            weakchildPoints[i].GetComponent<CircleCollider2D>().enabled = true;

            var temp = new ShootIndicator
            {
                color = Color.yellow,
                timer = 1f,
            };
            EventDispatcher.Instance.SendEvent(temp);
        }
    }
}
