using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserPointer : MonoBehaviour
{
    [SerializeField] private Transform firingPoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask layerMask;
    private LayerMask weakspot;
    private LayerMask hittable;
    private LayerMask boss;

    [SerializeField] private int pierceNumbers = 0;
    private int pierceNumberTemp = 0;
    private bool canDisplayPointer = true;

    private void Start()
    {
        weakspot = LayerMask.GetMask("Weakspot");
        hittable = LayerMask.GetMask("Hittable");
        boss = LayerMask.GetMask("BossShockwave");
    }
    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        if (canDisplayPointer)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector2 realPos = (Vector2) screenPos;

            RaycastHit2D[] targets = Physics2D.RaycastAll(firingPoint.position, firingPoint.right, Mathf.Infinity, layerMask);
            pierceNumberTemp = pierceNumbers;
            foreach (RaycastHit2D hit in targets)
            { 
                if (hit.collider != null)
                {
                    if (pierceNumberTemp > 0)
                        pierceNumberTemp--;
                    else
                    {
                        realPos = hit.point;
                        break;
                    }
                        

                    if ((weakspot & (1 << hit.collider.gameObject.layer)) != 0 || (hittable & (1 << hit.collider.gameObject.layer)) != 0 || (boss & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        realPos = hit.point;
                        break;
                    }
                        
                }
            }
            
            lineRenderer.SetPosition(0, firingPoint.position);
            lineRenderer.SetPosition(1, realPos);
        }
    }

    public void SetLine(bool newValue)
    {
        canDisplayPointer = newValue;
        lineRenderer.enabled = newValue;
        pierceNumbers = PierceManager.Instance.GetPierceStack();
    }
}
