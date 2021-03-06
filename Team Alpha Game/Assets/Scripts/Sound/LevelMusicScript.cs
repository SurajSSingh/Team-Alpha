﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicScript : MonoBehaviour
{
    FMOD.Studio.EventInstance levelMusic;
    int currentIdx = 0;
    int maxIdx = 3;
    public string MusicName;
    public List<float> sectionNum;


    // Start is called before the first frame update
    void Awake()
    {
        if(MusicName != null && MusicName != "")
        {
            levelMusic = FMODUnity.RuntimeManager.CreateInstance("event:/"+MusicName);
        }
        else
        {
            levelMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
        }
        levelMusic.start();
    }

    public void PlayWin()
    {
        levelMusic.setParameterByName("Finished",1.0f);
    }

    public void NextSafeSection()
    {
        if(currentIdx < maxIdx)
        {
            currentIdx += 1;
        }
        
    }

    public void UpdateMist(bool inMist)
    {
        if (inMist)
        {
            levelMusic.setParameterByName("InMist", 1.0f);
        }
        else
        {
            levelMusic.setParameterByName("InMist", 0.0f);
        }
        
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
            levelMusic.setParameterByName("Zone", sectionNum[currentIdx]);
        }
    }

    public void ChangeVolume(float volume)
    {
        levelMusic.setParameterByName("Vol", volume);
    }

    public void Stop()
    {
        Debug.Log("Stop Music");
        FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        levelMusic.release();
    }
}
