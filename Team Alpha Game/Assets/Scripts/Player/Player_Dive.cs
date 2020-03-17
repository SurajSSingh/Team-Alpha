using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dive : MonoBehaviour
{
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;

    //Positional States
    private bool airborne;

    //Action States
    private bool control;
    private bool jumping;
    private bool doubleJumping;
    private bool diving;
    private bool diveHit;
    private bool stunned;

    //Ability Access States
    private bool dive;

    //Timers
    private float landingTimer;

    //Movement Stats
    private float diveSpeed;

    //Timer Values
    private float landingTime;

    void Start()
    {
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        attributes = state.attributes;
        ReceiveValues();
        diveSpeed = attributes.diveSpeed;
        landingTime = attributes.landingTime;
    }
    void Update()
    {
        ReceiveValues();
        if (control && dive) //If player has control and has access to dive ability
        {
            if (Input.GetKeyDown(KeyCode.Z) && airborne) //Player is airborne and presses Z key
            {
                StartDive();
            }
        }
        SendValues();
    }

    public void Dive(ref Vector3 velocity) //Player dives straight downward at high speed
    {
        velocity.x = 0;
        velocity.y = diveSpeed;
    }

    public void ManageDive() //If player is stunned, cancel dive; else if player lands on ground, play landing animation
    {
        animator.AnimatorDive();
        if (state.stunned)
        {
            state.diving = false;
            state.control = true;
            animator.AnimatorDive();
        }
        else if (!state.stunned && state.onGround)
        {
            state.diving = false;
            animator.AnimatorDive();
            animator.AnimatorGrounded();
            Land();
        }
    }

    private void StartDive()
    {
        diving = true;
        control = false;
        jumping = false;
        doubleJumping = false;
    }

    public void ManageLanding()
    {
        if (timers.landingTimer <= 0.0f)
        {
            state.landing = false;
            animator.AnimatorLanding();
            state.control = true;
        }
    }

    private void Land()
    {
        state.landing = true;
        animator.AnimatorLanding();
        timers.landingTimer = landingTime;
    }

    private void ReceiveValues()
    {
        airborne = state.airborne;
        control = state.control;
        jumping = state.jumping;
        doubleJumping = state.doubleJumping;
        diving = state.diving;
        diveHit = state.diveHit;
        stunned = state.stunned;
        dive = state.dive;
        landingTimer = timers.landingTimer;
    }

    private void SendValues()
    {
        state.diving = diving;
        state.control = control;
        state.jumping = jumping;
        state.doubleJumping = doubleJumping;
    }
}
