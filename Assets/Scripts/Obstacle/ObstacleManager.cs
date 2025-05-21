using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] List<GameObject> groupParents;
    List<List<GameObject>> groupChildren;
    [SerializeField] GameObject Obstacle;

    private void Start()
    {
        groupChildren = new List<List<GameObject>>(groupParents.Count);

        for (int i = 0; i < groupParents.Count; i++)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform childTransform in groupParents[i].transform)
            {
                children.Add(childTransform.gameObject);
            }
            groupChildren.Add(children);
        }
    }

    public void AssignObstacles(int state)
    {
        foreach (GameObject child in groupChildren[state - 1])
        {
            Instantiate(Obstacle, child.transform.position, child.transform.rotation, child.transform);
        }
    }
}