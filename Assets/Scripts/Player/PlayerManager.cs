using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public GameObject Player;

    private string _playerTag = "Player";

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }

    private System.Collections.IEnumerator FindPlayerCoroutine()
    {
        while (Player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag(_playerTag);
            if (found != null)
            {
                Player = found;
            }

            yield return new WaitForSeconds(0.2f); 
        }
    }
}
