using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public Animator transition;

    public float transitonTime = 1f;

    public void PlayGame()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitonTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
