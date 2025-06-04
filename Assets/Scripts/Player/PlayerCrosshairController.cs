using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrosshairController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject crosshair;

    // Values
    public static bool Enabled = true;

    private void Awake()
    {
        Enabled = true;
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    private void Update()
    {
        crosshair.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

        crosshair.SetActive(Enabled);
    }
}
