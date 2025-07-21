using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static float currentTimer { get; private set; }

    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Values")]
    [SerializeField] private float maxTimer = 120f;

    [Header("Scripts")]
    [SerializeField] private PlayerDeath playerDeathScript;

    // Boolean
    private bool endGame = false;


    private void Awake()
    {
        currentTimer = maxTimer;
    }

    private void Start()
    {
        if (playerDeathScript == null) Debug.Log("Assign Player Death Script");
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<PlayerWin>(WinGame);
        EventDispatcher.Instance.Subscribe<PlayerDie>(PlayerDie);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<PlayerWin>(WinGame);
        EventDispatcher.Instance.Unsubscribe<PlayerDie>(PlayerDie);
    }

    void Update()
    {
        if (endGame) return;
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0 && playerDeathScript != null)
        {
            endGame = true;
            playerDeathScript.CallPlayerDeath();
        }
    }

    private void PlayerDie(PlayerDie e)
    {
        animator.Play("You Lose Opening Animation");

        Cursor.visible = true;
    }

    private void WinGame(PlayerWin e)
    {
        endGame = true;
        CalculateScore.totalTime = (int)(maxTimer - currentTimer);
        animator.Play("Gameplay Closing Animation");
        StartCoroutine(DelayAnimation("Gameplay Closing Animation", "End Screen"));

        Cursor.visible = true;
    }

    private IEnumerator DelayAnimation(string animationName, string mapName)
    {
        yield return null;

        AnimatorClipInfo[] clipInfo;
        float clipLength = 0f;

        while (true)
        {
            clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0 && clipInfo[0].clip.name == animationName)
            {
                clipLength = clipInfo[0].clip.length;
                break;
            }

            yield return null;
        }

        yield return new WaitForSecondsRealtime(clipLength);

        SceneManager.LoadScene(mapName);
    }
}
