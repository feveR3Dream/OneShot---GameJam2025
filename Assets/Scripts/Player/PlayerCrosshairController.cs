using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrosshairController : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;
    void Awake()
    {
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    void Update()
    {
        crosshair.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
