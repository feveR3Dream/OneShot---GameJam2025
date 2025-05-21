using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget2D : MonoBehaviour
{
    [SerializeField] private Transform _target;

    void Update()
    {
        if (_target == null && PlayerManager.Instance != null && PlayerManager.Instance.Player != null)
        {
            _target = PlayerManager.Instance.Player.transform;
        }

        if (_target == null) return;

        Vector2 direction = _target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
