using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class LineController : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _linewidth = 0.1f;
    [SerializeField] private Transform _pointContainer;
    private List<Transform> _points = new();

    private void Awake()
    {
        if (_lineRenderer == null)
        {
            if (GetComponent<LineRenderer>() != null)
            {
                _lineRenderer = GetComponent<LineRenderer>();
            }
            else
            {
                _lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
        }

        _lineRenderer.loop = true;
    }

    private void Start()
    {
        UpdatePoints();
        SetUpLine();
    }

    void Update()
    {
        UpdateLine();
    }




    private void SetUpLine()
    {
        _lineRenderer.positionCount = _points.Count;
        _lineRenderer.startWidth = _linewidth;
    }

    private void UpdatePoints()
    {
        _points.Clear();
        foreach (Transform point in _pointContainer)
        {
            _points.Add(point);
        }
    }

    private void UpdateLine()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            _lineRenderer.SetPosition(i, _points[i].position);
        }
    }

    public void UpdateMaterial(Material material)
    {
        _lineRenderer.material = material;
    }



    public void RefreshLine()
    {
        UpdatePoints();
        SetUpLine();
        UpdateLine();
    }

    public void ClearLine()
    {
        _lineRenderer.positionCount = 0;
    }

}



#if UNITY_EDITOR

[CustomEditor(typeof(LineController))]
public class LineControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var setupPointsScripts = (LineController)target;

        if (GUILayout.Button("Generate Line"))
        {
            setupPointsScripts.RefreshLine();
        }    
        
        if (GUILayout.Button("Clear Line"))
        {
            setupPointsScripts.ClearLine();
        }
    }

}

#endif