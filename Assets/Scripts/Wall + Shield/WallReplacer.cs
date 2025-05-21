using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallReplacer : MonoBehaviour
{

    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _wallRef;

    void Start()
    {
        _visual.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (_wallRef == null)
        {
            _visual.SetActive(true);
        }
    }

}
