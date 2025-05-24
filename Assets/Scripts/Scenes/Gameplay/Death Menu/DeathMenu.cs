using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;


    private void OnEnable()
    {
        retryButton.onClick.AddListener(RetryGame);
        menuButton.onClick.AddListener(MainMenu);
    }

    private void OnDisable()
    {
        retryButton.onClick.RemoveListener(RetryGame);
        menuButton.onClick.RemoveListener(MainMenu);
    }


    void Start()
    {
        if (retryButton == null) Debug.Log("Assign Retry Button");
        if (menuButton == null) Debug.Log("Assign Menu Button");
        if (animator == null)
            Debug.Log("Assign Animator");
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

        SceneManager.LoadScene(mapName);
    }


}
