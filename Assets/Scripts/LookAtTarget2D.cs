using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget2D : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject InnerEye;
    [SerializeField] private float distMulti = 0.4f;
    [SerializeField] private float maxDist = 0.25f;
    [SerializeField] private float distCap = 25f;

    void Update()
    {
        if (_target == null && PlayerManager.Instance != null && PlayerManager.Instance.Player != null)
        {
            _target = PlayerManager.Instance.Player.transform;
        }

        if (_target == null) return;

        LerpPositionAtTarget();
        LookAtTarget();
    }

    private void LookAtTarget()
    {
        Vector2 direction = _target.position - InnerEye.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        InnerEye.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void LerpPositionAtTarget()
    {
        float distance = Vector2.Distance(transform.position, _target.position * 0.5f) * 0.01f;
        Vector2 targetpos = _target.position * distMulti;
        if (distance >= maxDist)
            targetpos = InnerEye.transform.position + InnerEye.transform.right * distCap;
            
        Vector2 pos = Vector2.Lerp(transform.position, targetpos, 0.1f);
        InnerEye.transform.position = pos;
    }
}
