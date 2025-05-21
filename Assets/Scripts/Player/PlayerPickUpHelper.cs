using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpHelper : MonoBehaviour
{
    [SerializeField] private GameObject HelperMain;
    private GameObject pickUpObj;
    private Vector2 targetDir;
    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<PickUpEvent>(DeployHelper);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<PickUpEvent>(DeployHelper);
    }

    private void DeployHelper(PickUpEvent e)
    {
        pickUpObj = e.PickUpObj;
        SetHelper(true);
    }

    private void Update()
    {
        if (HelperMain.activeSelf)
        {
            targetDir = ((Vector2)pickUpObj.transform.position - (Vector2)HelperMain.transform.position).normalized;

            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

            HelperMain.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void SetHelper(bool newValue)
    {
        HelperMain.SetActive(newValue);
    }

}
