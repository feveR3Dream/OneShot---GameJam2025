using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ColliderController : MonoBehaviour
{
    [SerializeField] private Transform _pointContainer;
    [SerializeField] private Transform _colliderContainer;
    [SerializeField] private float _boxYSize = 0.25f;
    private List<Transform> _points = new();
    [SerializeField] private bool _wrapAround = false;


    void Start()
    {
    }

    void Update()
    {

    }

    private void UpdatePoints()
    {
        _points.Clear();
        foreach (Transform point in _pointContainer)
        {
            _points.Add(point);
        }
    }

    private void UpdateCollider()
    {
        if (_wrapAround)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                var next = (i + 1) % _points.Count;
                GenerateCollider(_points[i], _points[next]);
            }
            return;
        }

        for (int i = 0; i < _points.Count - 1; i++)
        {
            GenerateCollider(_points[i], _points[i + 1]);
        }

    }


    private void GenerateCollider(Transform point1, Transform point2)
    {
        // Create empty 
        GameObject colliderGameObject = new GameObject("Collider2D");
        colliderGameObject.transform.SetParent(_colliderContainer, false);

        // Get direction and midpoint
        Vector2 direction = point2.position - point1.position;

        Vector2 middlePoint = (Vector2)point1.position + direction / 2f;

        // Set position
        colliderGameObject.transform.position = middlePoint;

        // Set rotaion
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        colliderGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Add Collider
        BoxCollider2D collider = colliderGameObject.AddComponent<BoxCollider2D>();
        float distance = direction.magnitude;
        collider.size = new Vector2(distance, _boxYSize);
    }


    private void RemoveAllColliders()
    {
        var children = new List<Transform>();
        foreach (Transform child in _colliderContainer)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(child.gameObject);
            else
#endif
                Destroy(child.gameObject);
        }
    }





    public void Test()
    {
        RemoveAllColliders();
        UpdatePoints();
        UpdateCollider();
    }


}

#if UNITY_EDITOR

[CustomEditor(typeof(ColliderController))]
public class ColliderControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var colliderScript = (ColliderController)target;

        if (GUILayout.Button("Generate Colliders"))
        {
            colliderScript.Test();
        }
    }

}

#endif