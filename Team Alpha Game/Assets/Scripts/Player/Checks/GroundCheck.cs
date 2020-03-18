using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject Player;
    Player_State state;
    Player_Timers timers;
    Player_Attributes attributes;

    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
        state = Player.GetComponent<Player_State>();
        timers = Player.GetComponent<Player_Timers>();
        attributes = state.attributes;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        timers.displacementTimer = attributes.displacementTime;
        CheckOverlaps(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        timers.displacementTimer = attributes.displacementTime;
        if (other.gameObject.CompareTag("Ground"))
        {
            state.onGround = true;
        }
        if (other.gameObject.CompareTag("Quicksand"))
        {
            state.onGround = true;
            state.onQuicksand = true;
        }
        if (other.gameObject.CompareTag("EnemyHead"))
        {
        }
        if (other.gameObject.CompareTag("Slope"))
        {
            state.onGround = true;
            state.onSlope = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        timers.displacementTimer = attributes.displacementTime;
        state.onGround = false;
        state.onQuicksand = false;
        state.onSlope = false;
    }

    private void CheckOverlaps(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            state.Grounded_State();
        }
        if (other.gameObject.CompareTag("Quicksand"))
        {
            state.Grounded_State();
            state.onQuicksand = true;
        }
        if (other.gameObject.CompareTag("Spikes"))
        {
            state.Grounded_State();
            GetComponentInParent<Speed_Manager>().InstantChangeHealth(-10);
        }
        if (other.gameObject.CompareTag("EnemyHead"))
        {
        }
        if (other.gameObject.CompareTag("Slope"))
        {
            state.Grounded_State();
            state.onSlope = true;
        }
    }
}
