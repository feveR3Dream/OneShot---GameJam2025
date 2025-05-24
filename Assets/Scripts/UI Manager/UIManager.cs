using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI bossPhase;
    [SerializeField] private Animator animator;

    // Booleans
    private bool stopTimer = false;


    private void Awake()
    {
        if (timer == null) Debug.Log("Assign Timer Text");
        if (bossPhase == null) Debug.Log("Assign Phase Text");
        if (animator == null) Debug.Log("Assign World Space Animator");
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossChangePhase>(UpdateBossPhaseUI);
        EventDispatcher.Instance.Subscribe<PlayerDie>(StopTimer);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossChangePhase>(UpdateBossPhaseUI);
        EventDispatcher.Instance.Unsubscribe<PlayerDie>(StopTimer);
    }


    void Start()
    {
        UpdateBossPhaseUI(new BossChangePhase());
    }
    

    void Update()
    {
        if (stopTimer) return;

        UpdateTimer((int)GameplayManager.currentTimer);
    }
    private void UpdateTimer(float currentTimer)
    {
        timer.text = $"Time Left: {currentTimer}";
    }

    private void UpdateBossPhaseUI(BossChangePhase e)
    {
        if ((BossPhase.MaxPhase + 1) - e.CurrentPhase > 1)
        {
            if (e.CurrentPhase == 1)
            {
                animator.Play("Introduction Fade Away Animation");
            }

            bossPhase.text = "Phase: " + e.CurrentPhase + " / " + (BossPhase.MaxPhase + 1);
        }
        else
            bossPhase.text = "Final Phase";
    }

    private void StopTimer(PlayerDie e)
    {
        stopTimer = true;
    }
}
