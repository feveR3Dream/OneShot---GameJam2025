using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private TextMeshProUGUI timerConcludeText;
    [SerializeField] private Animator animator;


    void Start()
    {
        if (retryButton == null) Debug.Log("Assign Retry Button");
        if (menuButton == null) Debug.Log("Assign Menu Button");
        if (animator == null) Debug.Log("Assign Animator");
        if (timerConcludeText == null) Debug.Log("Assign Timer Text");
        else timerConcludeText.text = "Time: " + CalculateScore.totalTime;
        
    }

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

    private void RetryGame()
    {
        animator.Play("End Screen Closing Animation");
        StartCoroutine(DelayAnimation("End Screen Closing Animation", "DaBestSceneEver"));
    }

    private void MainMenu()
    {
        animator.Play("End Screen Closing Animation");
        StartCoroutine(DelayAnimation("End Screen Closing Animation", "Main Menu"));
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
