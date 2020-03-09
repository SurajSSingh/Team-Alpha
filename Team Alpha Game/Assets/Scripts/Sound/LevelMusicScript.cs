﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicScript : MonoBehaviour
{
    FMOD.Studio.EventInstance levelMusic;
    float safeSection = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        levelMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
        levelMusic.start();
    }

    public void PlayWin()
    {
        levelMusic.setParameterByName("Finished",1.0f);
    }

    public void NextSafeSection()
    {
        safeSection = 1.0f;
    }

    public void UpdateDistance(float distance)
    {
        levelMusic.setParameterByName("Distance", distance);
    }

    public void UpdateHealth (float health)
    {
        levelMusic.setParameterByName("Health", health);
    }

    public void SwapZones(bool isDanger)
    {
        if (isDanger)
        {
            levelMusic.setParameterByName("Zone", 2.0f);
        }
        else
        {
            levelMusic.setParameterByName("Zone", safeSection);
        }
    }

    public void Stop()
    {
        FMODUnity.RuntimeManager.GetBus("Bus:/").stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
