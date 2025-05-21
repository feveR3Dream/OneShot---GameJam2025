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
    //[SerializeField] private Animator animator;


    void Start()
    {
        if (retryButton == null) Debug.Log("Assign Retry Button");
        if (menuButton == null) Debug.Log("Assign Menu Button");
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
        SceneManager.LoadScene("DaBestSceneEver");
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
