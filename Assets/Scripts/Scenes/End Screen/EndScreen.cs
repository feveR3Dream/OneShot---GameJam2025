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
    [SerializeField] private TextMeshProUGUI deathConcludeText;
    [SerializeField] private Animator animator;


    void Start()
    {
        if (retryButton == null) Debug.Log("Assign Retry Button");
        if (menuButton == null) Debug.Log("Assign Menu Button");
        if (timerConcludeText == null) Debug.Log("Assign Timer Text");
        if (deathConcludeText == null) Debug.Log("Assign Death Text");
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

    private void Update()
    {
        
    }


    private void RetryGame()
    {
        SceneManager.LoadScene("TopdownGameJam");
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
