﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_End : MonoBehaviour
{

    public ScreenManager respawner;
    private void Start()
    {
        respawner = GameObject.Find("Game UI").GetComponent<ScreenManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other);
        respawner.ResetLevel();
    }
}
