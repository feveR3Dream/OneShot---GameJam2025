using System.Collections;
using UnityEngine;

public class BossPhase : MonoBehaviour
{
    public static int MaxPhase { get; private set; }

    [SerializeField] private int maxPhase;
    [SerializeField] float[] shockwaveRange;
    [SerializeField] float[] shockwaveDistances;
    [SerializeField] float[] shockwaveTimes;
    [SerializeField] Shockwave shockwave;
    [SerializeField] ObstacleManager obstacleManager;
    [SerializeField] LayerMask player;
    [SerializeField] int currentPhase;


    private void Awake()
    {
        MaxPhase = maxPhase;
    }

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

        if (currentPhase >= MaxPhase)
        {
            EventDispatcher.Instance.SendEvent(new PlayerWin());
            yield break;
        }

        currentPhase++;
        int thisPhase = currentPhase;

        EventDispatcher.Instance.SendEvent(new BossChangePhase { CurrentPhase = thisPhase });

        yield return new WaitForSeconds(1.0f);
        EventDispatcher.Instance.SendEvent(new CameraShakeEvent { ShakeDuration = 0.25f, ShakeMagnitude = 0.75f });
        SoundManager.PlaySound(SoundType.BOSS_SHOCKWAVE, 0.5f);
        SoundManager.PlaySound(SoundType.BOSS_EVOLVE, 0.25f);
        SoundManager.PlaySound(SoundType.DANGER_HUMMING, 0.25f);
        ParticleManager.instance.SpawnParticle(ParticleType.SHOCKWAVE, this.transform.position, Quaternion.identity);
        obstacleManager.AssignObstacles(thisPhase);
        DoShockwave();
    }


    private void DoShockwave()
    {
        if (currentPhase >= shockwaveRange.Length ||
            currentPhase >= shockwaveDistances.Length ||
            currentPhase >= shockwaveTimes.Length)
        {
            Debug.LogWarning("Current phase exceeds shockwave data array limits.");
            return;
        }

        Collider2D col = Physics2D.OverlapCircle(this.transform.position, shockwaveRange[currentPhase], player.value);
        if (col == null)
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
