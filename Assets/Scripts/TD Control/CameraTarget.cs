using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using static UnityEngine.GraphicsBuffer;

public class CameraTarget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera m_Camera;
    //[SerializeField] private Transform orgPivot; // Make sure the camera lies within this GameObject in the scene
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 5f;

    private void LateUpdate()
    {
        GetMidPoint();
        CameraControl();
    }

    private void GetMidPoint()
    {
        Vector3 midpoint = (player.position + boss.position) / 2f;
        Vector3 desiredPosition = new Vector3(midpoint.x, midpoint.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, speed);
    }

    private void CameraControl()
    {
        float distance = Vector3.Distance(player.position, boss.position);
        float targetZoom = Mathf.Clamp(distance * 0.5f, minZoom, maxZoom);
        m_Camera.orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, targetZoom, Time.deltaTime * speed);
    }
}
