using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Animator transition;

    public float transitonTime = 1f;

    public Text livesText;
    public Text dashReady;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject playerUI;

    public static ScreenManager instance;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Update()
    {
        livesText.text = "Lives: " + PlayerManager.instance.lives.ToString();
        dashReady.text = PlayerManager.instance.dashCond;
        if (PlayerManager.instance.dashCond.Contains("Un"))
        {
            dashReady.color = Color.red;
        } else {
            dashReady.color = Color.green;   
        }
    }

    public void CloseScreen(GameObject screen)
    {
        screen.SetActive(false);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex);
        playerUI.SetActive(true);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GameWin()
    {
        playerUI.SetActive(false);
        PlayerManager.instance.StopChecking();
        StartCoroutine(LoadScreen());
        winScreen.SetActive(true);
    }

    public void GameLose()
    {
        playerUI.SetActive(false);
        PlayerManager.instance.StopChecking();
        StartCoroutine(LoadScreen());
        loseScreen.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator LoadScreen()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitonTime);
    }
}
