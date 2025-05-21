using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BossPhase : MonoBehaviour
{
    [SerializeField] int MaxPhase;
    [SerializeField] float[] shockwaveRange;
    [SerializeField] float[] shockwaveDistances;
    [SerializeField] float[] shockwaveTimes;
    [SerializeField] Shockwave shockwave;
    [SerializeField] ObstacleManager obstacleManager;
    [SerializeField] LayerMask player;
    int currentPhase;

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossHurt>(Hurt);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossHurt>(Hurt);
    }

    private void Hurt(BossHurt context)
    {
        EventDispatcher.Instance.SendEvent(new CameraShakeEvent { ShakeDuration = 0.25f, ShakeMagnitude = 0.75f });
        currentPhase++;

        if (currentPhase > MaxPhase)
        {
            EventDispatcher.Instance.SendEvent(new PlayerWin());
            return;
        }
        currentPhase = Mathf.Clamp(currentPhase, 0, MaxPhase - 1);
        obstacleManager.AssignObstacles(currentPhase);
        DoShockwave();
    }

    private void DoShockwave()
    {
        Collider2D col = Physics2D.OverlapCircle(this.transform.position, shockwaveRange[currentPhase], player.value);
        if(col == null)
        {
            return;
        }

        shockwave.Recalibrate(shockwaveDistances[currentPhase], shockwaveTimes[currentPhase]);
        if (col.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            shockwave.SetController(playerController);
            shockwave.TriggerShockwave(playerController);
        }
    }

    public int GetPhase()
    {
        return currentPhase;
    }
}
