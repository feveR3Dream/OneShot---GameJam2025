using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button beginButton;
    [SerializeField] private Button exitButton;


    // Animation Vars
    private Animator animator;
    private AnimatorClipInfo[] currentClipInfo;
    private float currentClipLength;


    void Start()
    {
        if (beginButton == null)
            Debug.Log("Assign Begin Button");

        if (exitButton == null)
            Debug.Log("Assign Exit Button");

        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.Log("Assign Animator");

    }

    private void OnEnable()
    {
        beginButton.onClick.AddListener(BeginGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDisable()
    {
        beginButton.onClick.RemoveListener(BeginGame);  
        exitButton.onClick.RemoveListener(ExitGame);
    }

    private void BeginGame()
    {
        animator.Play("Main Menu Closing Animation");
        StartCoroutine(WaitForClosingAnimation());
    }

    private IEnumerator WaitForClosingAnimation()
    {
        // Wait until the Animator updates (wait a frame)
        yield return null;

        // Wait until the current animation is actually "Main Menu Closing Animation"
        AnimatorClipInfo[] clipInfo;
        float clipLength = 0f;

        // Wait until the correct animation is playing
        while (true)
        {
            clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0 && clipInfo[0].clip.name == "Main Menu Closing Animation")
            {
                clipLength = clipInfo[0].clip.length;
                break;
            }

            yield return null; // Keep waiting until it's the correct one
        }

        Debug.Log("Animation Timer: " + clipLength);

        yield return new WaitForSeconds(clipLength);

        SceneManager.LoadScene("TopdownGameJam");
    }



    private IEnumerator DelayBeginGame()
    {
        animator.Play("Main Menu Closing Animation");

        #region Get Clip Animation Time
        currentClipInfo = this.animator.GetCurrentAnimatorClipInfo(0);
        currentClipLength = currentClipInfo[0].clip.length;
        #endregion

        Debug.Log("Animation Timer: " + currentClipLength);

        yield return new WaitForSeconds(currentClipLength); 

        SceneManager.LoadScene("TopdownGameJam");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
