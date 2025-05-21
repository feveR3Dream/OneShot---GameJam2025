using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 shakeOffset;
    private Camera m_Camera;
    float shakeDuration = 0f;
    float shakeMagnitude = 0.1f;

    void Start()
    { 
        m_Camera = Camera.main;
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<CameraShakeEvent>(Shake);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<CameraShakeEvent>(Shake);
    }

    void LateUpdate()
    {
        if (shakeDuration > 0)
        {
            shakeOffset = new Vector3(
                 Random.Range(-1f, 1f) * shakeMagnitude,
                 Random.Range(-1f, 1f) * shakeMagnitude,
                 0f
             );

            shakeMagnitude *= 0.9f;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }

        m_Camera.transform.localPosition += shakeOffset;
    }

    private void Shake(CameraShakeEvent e)
    {
        shakeMagnitude = e.ShakeMagnitude;
        shakeDuration = e.ShakeDuration;
    }
}
