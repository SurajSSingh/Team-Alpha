﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject Player;
    Player_State state;
    Player_Timers timers;
    Player_Attributes attributes;
    Player_Collisions collisions;
    public bool attacking;

    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
        state = Player.GetComponent<Player_State>();
        timers = Player.GetComponent<Player_Timers>();
        attributes = state.attributes;
        collisions = Player.GetComponent<Player_Collisions>();
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckOverlaps(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.onGround = true;
        }
        if (other.gameObject.CompareTag("Quicksand"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.onGround = true;
            state.onQuicksand = true;
        }
        if (other.gameObject.CompareTag("Slope"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.onGround = true;
            state.onSlope = true;
        }
        if (other.gameObject.CompareTag("Spikes"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.onGround = true;
            if (state.sign == 0.0f)
            {
                state.enemyColSign = -1.0f;
            }
            else
            {
                state.enemyColSign = -state.sign;
            }
            collisions.StartKnockback();
            GetComponentInParent<Speed_Manager>().InstantChangeHealth(-10);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Quicksand") || other.gameObject.CompareTag("Slope"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.onGround = false;
            state.onQuicksand = false;
            state.onSlope = false;
        }
        if (other.gameObject.CompareTag("Spikes"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.onGround = false;
        }
    }

    private void CheckOverlaps(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.Grounded_State();
        }
        if (other.gameObject.CompareTag("Quicksand"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.Grounded_State();
            state.onQuicksand = true;
        }
        if (other.gameObject.CompareTag("Slope"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.Grounded_State();
            state.onSlope = true;
        }
        if (other.gameObject.CompareTag("Spikes"))
        {
            timers.displacementTimer = attributes.displacementTime;
            state.Grounded_State();
        }
    }
}
