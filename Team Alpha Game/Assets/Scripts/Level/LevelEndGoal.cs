using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndGoal : MonoBehaviour
{
    public LevelMusicScript levelMusic;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            levelMusic.PlayWin();
            ScreenManager.instance.GameWin();
        }
    }
}
