using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LineController : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _linewidth = 0.1f;
    [SerializeField] private Transform[] points;

    private void Awake()
    {
        if (_lineRenderer == null)
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
    }

    private void Start()
    {
        SetUpLine();
    }

    void Update()
    {
        UpdateLine();
    }

    private void SetUpLine()
    {
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.startWidth = _linewidth;

    }

    private void UpdateLine()
    {
        for (int i = 0; i < points.Length; i++)
        {
            _lineRenderer.SetPosition(i, points[i].position);
        }
    }

    public void UpdateMaterial(Material material)
    {
        _lineRenderer.material = material;
    }

    public void GenerateLine()
    {
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
            setupPointsScripts.GenerateLine();
        }    
        
        if (GUILayout.Button("Clear Line"))
        {
            setupPointsScripts.ClearLine();
        }
    }

}

#endif