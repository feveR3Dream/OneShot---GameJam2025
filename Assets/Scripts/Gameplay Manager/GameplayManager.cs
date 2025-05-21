using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Image screenFader;

    // Timer
    private float timer = 0f;


    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<PlayerWin>(WinGame);
        EventDispatcher.Instance.Subscribe<PlayerDie>(PlayerDie);

        retryButton.onClick.AddListener(RetryGame);
        menuButton.onClick.AddListener(MainMenu);
    }    
    
    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<PlayerWin>(WinGame);
        EventDispatcher.Instance.Unsubscribe<PlayerDie>(PlayerDie);

        retryButton.onClick.RemoveListener(RetryGame);
        menuButton.onClick.RemoveListener(MainMenu);
    }


    void Start()
    {
        if (retryButton == null) Debug.Log("Assign Retry Button");
        if (menuButton == null) Debug.Log("Assign Menu Button");
        if (screenFader == null) Debug.Log("Assign Screen Fader");

        if (animator == null)
            Debug.Log("Assign Animator");
    }


    void Update()
    {
        timer += Time.deltaTime;
    }

    private void PlayerDie(PlayerDie e)
    {
        animator.Play("You Lose Opening Animation");
    }

    private void WinGame(PlayerWin e)
    {
        animator.Play("Gameplay Closing Animation");
        StartCoroutine(DelayAnimation("Gameplay Closing Animation", "End Screen"));
    }


    private void RetryGame()
    {
        animator.Play("You Lose Closing Animation");
        StartCoroutine(DelayAnimation("You Lose Closing Animation", "DaBestSceneEver"));
    }

    private void MainMenu()
    {
        animator.Play("Gameplay Closing Animation");
        StartCoroutine(DelayAnimation("Gameplay Closing Animation", "Main Menu"));
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

        // Fade to black
        if (screenFader != null)
        {
            float fadeDuration = 0.5f;
            float elapsed = 0f;

            Color startColor = screenFader.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

            while (elapsed < fadeDuration)
            {
                screenFader.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            screenFader.color = endColor;
        }

        SceneManager.LoadScene(mapName);
    }


}
