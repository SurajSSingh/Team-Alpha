using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Animator transition;
    public GameObject player;

    public float transitonTime = 1f;

    public Image DashComponent;
    public Image DiveComponent;
    public Image fadinText1;
    public Image fadinText2;
    public Sprite dashImage;
    public Sprite diveImage;
    public Sprite emptyImage;
    public Text livesText;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject playerUI;

    public LevelMusicScript levelMusic;

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
        bool dashReady = player.GetComponent<Player_Controller>().dashReady;
        bool diveReady = player.GetComponent<Player_Controller>().diving;
        if (dashReady)
        {
            DashComponent.sprite = dashImage;
        } else {
            DashComponent.sprite = emptyImage;
        }
        if (diveReady)
        {
            DiveComponent.sprite = diveImage;
        } else
        {
            DiveComponent.sprite = emptyImage;
        }
    }

    public void CloseScreen(GameObject screen)
    {
        screen.SetActive(false);
    }

    public void ResetLevel()
    {
        levelMusic.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene ().buildIndex);
        playerUI.SetActive(true);
    }

    public void NextLevel()
    {
        levelMusic.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GameWin()
    {
        playerUI.SetActive(false);
        PlayerManager.instance.StopChecking();
        StartCoroutine(LoadWinScreen());
    }

    public void GameLose()
    {
        playerUI.SetActive(false);
        PlayerManager.instance.StopChecking();
        StartCoroutine(LoadLooseScreen());
    }

    public void ReturnToMenu()
    {
        levelMusic.Stop();
        SceneManager.LoadScene(0);
    }

    IEnumerator LoadLooseScreen()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitonTime);

        loseScreen.SetActive(true);
        fadinText1.CrossFadeAlpha(0, 3, false);
        fadinText1.enabled = false;
    }

    IEnumerator LoadWinScreen()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitonTime);

        winScreen.SetActive(true);
        fadinText2.CrossFadeAlpha(0, 3, false);
        fadinText2.enabled = false;
    }
}
