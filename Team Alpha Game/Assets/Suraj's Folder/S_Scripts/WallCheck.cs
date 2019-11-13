﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    private GameObject Player;
    public float oppositeDirection = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Quicksand")){
            //Debug.Log(other.transform.position-Player.transform.position);
            Player.GetComponent<Player_Controller>().isPlayerWallTouch(true,oppositeDirection);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Player.GetComponent<Player_Controller>().isPlayerWallTouch(false,0.0f);
    }
}
