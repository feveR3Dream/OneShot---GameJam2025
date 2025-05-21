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
    [SerializeField] int currentPhase;

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
        StartCoroutine(Evolve());
    }

    IEnumerator Evolve()
    {
        EventDispatcher.Instance.SendEvent(new CameraShakeEvent { ShakeDuration = 0.25f, ShakeMagnitude = 0.75f });
        currentPhase++;
        int thisPhase = currentPhase;
        if (currentPhase > MaxPhase)
        {
            EventDispatcher.Instance.SendEvent(new PlayerWin());
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        EventDispatcher.Instance.SendEvent(new CameraShakeEvent { ShakeDuration = 0.25f, ShakeMagnitude = 0.75f });
        SoundManager.PlaySound(SoundType.BOSS_SHOCKWAVE, 1);
        SoundManager.PlaySound(SoundType.BOSS_EVOLVE, 1);
        SoundManager.PlaySound(SoundType.DANGER_HUMMING, 1);
        ParticleManager.instance.SpawnParticle(ParticleType.SHOCKWAVE, this.transform.position, Quaternion.identity);
        obstacleManager.AssignObstacles(thisPhase);
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
