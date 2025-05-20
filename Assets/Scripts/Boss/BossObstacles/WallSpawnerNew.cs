using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawnerNew : MonoBehaviour
{
    [Serializable] public class WallSet
    {
        public GameObject WallPhaseContainer;
        public GameObject[] Walls;
    }

    public List<WallSet> ObstaclesTable = new List<WallSet>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform wallcontainer in gameObject.transform)
        {
            List<GameObject> wallslist = new List<GameObject>();
            foreach (Transform walls in wallcontainer.transform)
            {
                wallslist.Add(walls.gameObject);
            }
            WallSet obstacle = new WallSet()
            {
                WallPhaseContainer = wallcontainer.gameObject,
                Walls = wallslist.ToArray(),
            };
            ObstaclesTable.Add(obstacle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
