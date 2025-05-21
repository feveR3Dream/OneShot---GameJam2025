using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LineControllerPro : MonoBehaviour
{
    [SerializeField] private float _linewidth = 0.1f;

    [Serializable]
    public class Shapes
    {
        public string _name;
        public LineRenderer lineRenderer;
        public Transform pointContainer;
        public List<Transform> points = new();
    }

    [SerializeField] private Shapes[] shapes;

    private void Awake()
    {

    }

    private void Start()
    {
        foreach (Shapes shape in shapes)
        {
            UpdatePoints(shape);
            SetUpLine(shape);
        }
    }

    void Update()
    {
        foreach (Shapes shape in shapes)
        {
            UpdateLine(shape);
        }
    }


    private void SetUpLine(Shapes shape)
    {
        shape.lineRenderer.positionCount = shape.points.Count;
        shape.lineRenderer.startWidth = _linewidth;
    }

    private void UpdatePoints(Shapes shape)
    {
        shape.points.Clear();
        foreach (Transform point in shape.pointContainer)
        {
            shape.points.Add(point);
        }
    }

    private void UpdateLine(Shapes shape)
    {
        for (int i = 0; i < shape.points.Count; i++)
        {
            shape.lineRenderer.SetPosition(i, shape.points[i].position);
        }
    }

    public void UpdateMaterial(Shapes shape, Material material)
    {
        shape.lineRenderer.material = material;
    }



    public void RefreshLine()
    {
        foreach (Shapes shape in shapes)
        {
            UpdatePoints(shape);
            SetUpLine(shape);
            UpdateLine(shape);

        }
    }

    public void ClearAllLine()
    {
        foreach (Shapes shape in shapes)
        {
            shape.lineRenderer.positionCount = 0;
        }
    }

}



#if UNITY_EDITOR

[CustomEditor(typeof(LineControllerPro))]
public class LineControllerProEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var setupPointsScripts = (LineControllerPro)target;

        if (GUILayout.Button("Generate Line"))
        {
            setupPointsScripts.RefreshLine();
        }

        if (GUILayout.Button("Clear Line"))
        {
            setupPointsScripts.ClearAllLine();
        }
    }

}

#endif
