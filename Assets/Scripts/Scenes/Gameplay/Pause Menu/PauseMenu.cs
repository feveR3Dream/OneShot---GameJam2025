using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Animator animator;

    private bool canInteract = true;
    private bool isPaused = false;

    private void OnEnable()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        menuButton.onClick.AddListener(MainMenu);

        EventDispatcher.Instance.Subscribe<PlayerDie>(DisablePausing);
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveListener(ResumeGame);
        menuButton.onClick.RemoveListener(MainMenu);

        EventDispatcher.Instance.Unsubscribe<PlayerDie>(DisablePausing);
    }

    void Start()
    {
        if (resumeButton == null) Debug.Log("Assign Resume Button");
        if (menuButton == null) Debug.Log("Assign Menu Button");
        if (pauseMenu == null) Debug.Log("Assign Pause Menu");

        if (animator == null) Debug.Log("Assign Animator");
        else animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canInteract)
        {
            isPaused = !isPaused;
            Debug.Log("Paused: " + isPaused);
            StartCoroutine(PauseMenuFunctionality(isPaused));
        }
    }

    private IEnumerator PauseMenuFunctionality(bool isPausing)
    {
        canInteract = false;
        PlayerCrosshairController.Enabled = !isPausing;

        if (isPausing)
        {
            Cursor.visible = true;

            EventDispatcher.Instance.SendEvent(new GamePaused { paused = true }) ;

            animator.Play("Pause Menu Open Animation");

            yield return WaitForClip("Pause Menu Open Animation");

            Time.timeScale = 0f;
            canInteract = true;
        }
        else
        {
            Cursor.visible = false;

            Time.timeScale = 1f; // Resume time BEFORE animation

            EventDispatcher.Instance.SendEvent(new GamePaused { paused = false }) ;

            animator.Play("Pause Menu Close Animation");

            yield return WaitForClip("Pause Menu Close Animation");

            canInteract = true;
        }
    }

    private IEnumerator WaitForClip(string animationName)
    {
        // Wait one frame for animator to update
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
    }

    private void ResumeGame()
    {
        if (canInteract && isPaused)
        {
            isPaused = false;
            StartCoroutine(PauseMenuFunctionality(isPaused));
        }
    }

    private void MainMenu()
    {
        Time.timeScale = 1f;
        animator.Play("Gameplay Closing Animation");
        StartCoroutine(LoadMainMenuAfterAnimation("Gameplay Closing Animation"));

        Cursor.visible = true;
    }

    private void DisablePausing(PlayerDie e)
    {
        canInteract = false;
    }

    private IEnumerator LoadMainMenuAfterAnimation(string animationName)
    {
        yield return WaitForClip(animationName);
        SceneManager.LoadScene("Main Menu");
    }
}
