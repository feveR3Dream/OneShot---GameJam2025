using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] List<GameObject> groupParents;
    [SerializeField] List<string> ObstacleSetup; // Example: "013", "24", etc.
    [SerializeField] List<GameObject> Obstacle;  // Obstacle prefabs

    private List<List<GameObject>> groupChildren;

    private void Start()
    {
        groupChildren = new List<List<GameObject>>(groupParents.Count);

        foreach (GameObject parent in groupParents)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in parent.transform)
            {
                children.Add(child.gameObject);
            }
            groupChildren.Add(children);
        }
    }

    public void AssignObstacles(int state)
    {
        if (state - 1 < 0 || state - 1 >= groupChildren.Count) return;
        if (state - 1 >= ObstacleSetup.Count) return;

        string setup = ObstacleSetup[state - 1];
        List<int> obstacleIndices = new List<int>();

        // Parse indices from string setup
        foreach (char c in setup)
        {
            if (char.IsDigit(c))
            {
                int index = c - '0';
                if (index >= 0 && index < Obstacle.Count)
                {
                    obstacleIndices.Add(index);
                }
            }
        }

        // Instantiate obstacles
        foreach (GameObject child in groupChildren[state - 1])
        {
            if (obstacleIndices.Count == 0) continue;

            int randomIndex = Random.Range(0, obstacleIndices.Count);
            int obstacleIndex = obstacleIndices[randomIndex];

            Instantiate(Obstacle[obstacleIndex], child.transform.position, child.transform.rotation, child.transform);
        }
    }
}