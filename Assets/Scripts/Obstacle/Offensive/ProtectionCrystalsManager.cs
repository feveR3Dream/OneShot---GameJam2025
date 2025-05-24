using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionCrystalsManager : MonoBehaviour
{
    private List<GameObject> weakchildPoints = new List<GameObject>();
    private List<ObstacleProtectionCrystal> crystals = new List<ObstacleProtectionCrystal>();

    private void CheckWeakSpotVulnerability()
    {
        if (crystals.Count <= 0)
            SetAllWeakSpotsCollider(true);
        else
            SetAllWeakSpotsCollider(false);
    }

    private void SetAllWeakSpotsCollider(bool value)
    {
        foreach (var p in weakchildPoints)
        {
            if (p != null)
                p.GetComponent<CircleCollider2D>().enabled = value;
        }
    }


    public List<GameObject> AddCrystal(ObstacleProtectionCrystal crystal)
    {
        crystals.Add(crystal);
        CheckWeakSpotVulnerability();
        return weakchildPoints;
    }

    public void RemoveCrystal(ObstacleProtectionCrystal crystal)
    {
        crystals.Remove(crystal);
        crystals.RemoveAll(x => x == null);
        CheckWeakSpotVulnerability();
        
    }

    public void SetWeakPoints(List<GameObject> weakpoints)
    {
        List<GameObject> childwp = new List<GameObject>();
        for (int i = 0; i < weakpoints.Count; i++)
        {
            childwp.Add(weakpoints[i].transform.GetChild(0).gameObject);
        }

        weakchildPoints = childwp;
    }
}
