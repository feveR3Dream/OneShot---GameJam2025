using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }
    [SerializeField] GameObject[] _dictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SpawnParticle(int index, Vector3 position, Quaternion rotation)
    {
        if (index >= 0 && index < _dictionary.Length)
        {
            Instantiate(_dictionary[index], position, rotation);
        }
    }
}