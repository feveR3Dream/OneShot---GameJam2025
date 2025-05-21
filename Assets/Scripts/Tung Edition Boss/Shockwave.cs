using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] AnimationCurve movementAnimationCurve;
    [SerializeField] float endTime;
    [SerializeField] float distance;
    PlayerController controller;
    Vector3 startPos;
    Vector3 endPos;
    float current;
    bool triggerShockwave;

    private void Update()
    {
        if (triggerShockwave && controller != null)
        {
            DoShockwave(controller);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            controller = playerController;
            TriggerShockwave(controller);
        }
    }

    void DoShockwave(PlayerController playerController)
    {
        current += Time.deltaTime;
        playerController.transform.position = Vector3.Lerp(startPos, endPos, current/endTime);
        if(current > endTime)
        {
            playerController.SetCanMove(true);
            playerController.SetCollider2D(true);
            triggerShockwave = false;
        }
    }

    public void TriggerShockwave(PlayerController playerController)
    {
        triggerShockwave = true;
        SoundManager.PlaySound(SoundType.BOSS_SHOCKWAVE, 1f);
        current = 0;
        playerController.SetCanMove(false);
        playerController.SetCollider2D(false);
        startPos = playerController.transform.position;
        endPos = this.transform.position + (startPos - this.transform.position).normalized * distance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(endPos, Vector3.one);
    }

    public void Recalibrate(float newDistance, float newTime)
    {
        distance = newDistance;
        endTime = newTime;
    }

    public void SetController(PlayerController playerController)
    {
        controller = playerController;
    }
}
