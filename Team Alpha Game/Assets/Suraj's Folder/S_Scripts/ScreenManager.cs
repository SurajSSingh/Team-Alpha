using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Text livesText;
    public GameObject winScreen;
    public GameObject loseScreen;

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
    }

    public void CloseScreen(GameObject screen)
    {
        screen.SetActive(false);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex);
    }

    public void GameWin()
    {
        PlayerManager.instance.StopChecking();
        winScreen.SetActive(true);
    }

    public void GameLose()
    {
        PlayerManager.instance.StopChecking();
        loseScreen.SetActive(true);
    }
}
