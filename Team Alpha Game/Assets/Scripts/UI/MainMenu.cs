using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject controlUI;
    public GameObject selectUI;
    public GameObject creditsUI;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Select(string levelName)
    {
        SceneManager.LoadScene(int.Parse(levelName));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ControlMenu()
    {
        menuUI.SetActive(false);
        controlUI.SetActive(true);
    }

    public void ReturntoMenu()
    {
        menuUI.SetActive(true);
        controlUI.SetActive(false);
        selectUI.SetActive(false);
        creditsUI.SetActive(false);
    }

    public void LevelSelectMenu()
    {
        menuUI.SetActive(false);
        selectUI.SetActive(true);
    }

    public void CreditsMenu()
    {
        menuUI.SetActive(false);
        creditsUI.SetActive(true);
    }
}
