using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject controlUI;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
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

    public void ReturnfromControl()
    {
        menuUI.SetActive(true);
        controlUI.SetActive(false);
    }
}
