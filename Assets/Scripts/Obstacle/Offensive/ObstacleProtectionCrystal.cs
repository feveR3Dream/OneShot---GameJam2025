using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleProtectionCrystal : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    private List<LineRenderer> lines = new List<LineRenderer>();
    private List<GameObject> weakchildPoints = new List<GameObject>();
    private ProtectionCrystalsManager crystalsManager;

    private void Start()
    {
        crystalsManager = GameObject.Find("Boss").GetComponentInChildren<ProtectionCrystalsManager>();
        weakchildPoints = crystalsManager.AddCrystal(this);
        SetLines(weakchildPoints);
    }

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
        crystalsManager.RemoveCrystal(this);
        var temp = new ShootIndicator
        {
            color = Color.yellow,
            timer = 1f,
        };
        EventDispatcher.Instance.SendEvent(temp);
    }

    private void SetLines(List<GameObject> weakpoints)
    {
        for (int i = 0; i < weakpoints.Count; i++)
        {
            GameObject newLine = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);
            lines.Add(newLine.GetComponent<LineRenderer>());
        }
    }

}
