using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Renamer : MonoBehaviour
{
    [Space(3)]
    [Header("Rename")]
    [SerializeField] private Transform[] parent;
    [SerializeField] private string baseName = "Point";


    public void RenameChildren()
    {
        foreach (Transform t in parent)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                child.name = $"{baseName} {i}";
            }
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(Renamer))]
public class RenamerrEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var setupPointsScripts = (Renamer)target;

        if (GUILayout.Button("Rename"))
        {
            setupPointsScripts.RenameChildren();
        }
    }

}

#endif
