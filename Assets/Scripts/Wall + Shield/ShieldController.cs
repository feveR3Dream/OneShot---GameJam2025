using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ShieldController : MonoBehaviour
{

    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private float _XSize;
    [SerializeField] private float _YSize;

    [Header("Test")]
    [SerializeField] private Transform _spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnShieldAtLocation(Transform spawnPosition)
    {
        GameObject shield = Instantiate(_shieldPrefab.gameObject, spawnPosition.position, spawnPosition.rotation);
        shield.transform.localScale = new Vector3 (_XSize, _YSize, 1f);
    }

    public void Test()
    {
        SpawnShieldAtLocation(_spawnPoint);
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(ShieldController))]
public class ShieldControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var script = (ShieldController)target;

        if (GUILayout.Button("Generate Shield"))
        {
            script.Test();
        }
    }

}

#endif
