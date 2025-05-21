using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FakeThunder : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _impactPrefab; // The prefab to spawn on impact
    [SerializeField] private float _moveSpeed = 10f;
    private float _impactThreshold = 0.8f;
    [SerializeField] private bool _destroyItSelf;
    [SerializeField] private float _timer;

    [SerializeField] private float _fallAcceleration = 30f;
    private float _currentFallSpeed = 0f;

    private bool _hasHit = false;

    private void FixedUpdate()
    {
        if (_hasHit || _target == null) return;

        // Move toward the target
        //transform.position = Vector3.MoveTowards(transform.position, _target.position, _moveSpeed * Time.fixedDeltaTime);
        //transform.position = Vector3.Lerp(transform.position, _target.position, _moveSpeed * Time.fixedDeltaTime);

        _currentFallSpeed += _fallAcceleration * Time.fixedDeltaTime;

        // Move downward toward the target
        Vector3 direction = (_target.position - transform.position).normalized;
        transform.position += direction * _currentFallSpeed * Time.fixedDeltaTime;


        // Check if reached target
        if (Vector3.Distance(transform.position, _target.position) <= _impactThreshold)
        {
            Impact();
        }
    }

    private void Impact()
    {
        _hasHit = true;

        // Spawn impact prefab at the current position
        if (_impactPrefab != null)
        {
            GameObject impact = Instantiate(_impactPrefab, transform.position, Quaternion.identity);
        }

        if (_destroyItSelf)
        {
            gameObject.layer = LayerMask.NameToLayer("Obstacle");

            // Add BoxCollider2D if not already present
            if (gameObject.GetComponent<BoxCollider2D>() == null)
            {
                gameObject.AddComponent<BoxCollider2D>();
            }
        }

        Destroy(_target.gameObject);

        // Destroy the thunder object after impact 
        if (_destroyItSelf)
        {
            Destroy(gameObject, _timer);
        }

    }
}
