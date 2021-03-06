﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dive : MonoBehaviour
{
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;
    GroundCheck groundCheck;

    //Positional States
    private bool airborne;

    //Action States
    private bool control;
    private bool jumping;
    private bool doubleJumping;
    private bool diving;
    private bool diveHit;
    private bool stunned;
    private bool immune;

    //Ability Access States
    private bool dive;

    //Timers
    private float landingTimer;
    private float diveAttackTimer;

    //Movement Stats
    private float moveSpeed;
    private float jumpVelocity;
    private float diveSpeed;

    //Timer Values
    private float landingTime;
    private float diveAttackTime;

    //Physics
    private LayerMask diveCollisionMask;

    void Start()
    {
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        attributes = state.attributes;
        groundCheck = gameObject.transform.GetChild(1).gameObject.GetComponent<GroundCheck>();
        ReceiveValues();
        moveSpeed = attributes.moveSpeed;
        jumpVelocity = attributes.jumpVelocity;
        diveSpeed = attributes.diveSpeed;
        landingTime = attributes.landingTime;
        diveAttackTime = attributes.diveAttackTime;
        diveCollisionMask = attributes.diveCollisionMask;
    }
    void Update()
    {
        ReceiveValues();
        if (control && dive) //If player has control and has access to dive ability
        {
            if (Input.GetAxisRaw("Fire2") == 1.0f && airborne) //Player is airborne and presses Z key
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
        CheckEnemyBelow(ref velocity);
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

    public void ManageDiveAttack()
    {
        if (timers.diveAttackTimer <= 0.0f)
        {
            state.diving = false;
            state.diveHit = false;
            state.control = true;
            animator.AnimatorDive();
            animator.AnimatorDiveAttack();
        }
    }

    public void Rebound(ref Vector3 velocity, float sign) //If player hit enemy during dive, rebound and leap off enemy
    {
        if (sign == 0.0f)
        {
            sign = 1.0f;
        }
        velocity.x = sign * moveSpeed * 1.8f;
        velocity.y = jumpVelocity * 0.75f;
        groundCheck.attacking = false;
    }

    private void CheckEnemyBelow(ref Vector3 velocity) //Check if there is an enemy directly below player; if true, trigger dive attack
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector2 rayOrigin1 = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 rayOrigin2 = new Vector2(bounds.max.x, bounds.min.y);
        Vector2 rayOrigin3 = new Vector2(bounds.center.x, bounds.min.y);
        RaycastHit2D hit1 = Physics2D.Raycast(rayOrigin1, Vector2.down, 0.6f, diveCollisionMask);
        RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin2, Vector2.down, 0.6f, diveCollisionMask);
        RaycastHit2D hit3 = Physics2D.Raycast(rayOrigin3, Vector2.down, 0.6f, diveCollisionMask);
        if (hit1 || hit2 || hit3)
        {
            velocity = Vector2.zero;
            DiveAttack();
        }
    }

    private void DiveAttack() //Trigger dive attack animation and inflict damage on enemy
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/MusicSingle/Abilities_Dive", this.transform.position);
        state.diveHit = true;
        state.immune = true;
        timers.diveAttackTimer = diveAttackTime;
        timers.immuneTimer = 1.0f;
        animator.AnimatorDiveAttack();
        groundCheck.attacking = true;
    }

    private void StartDive()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/MusicSingle/Abilities_Dive", this.transform.position);
        diving = true;
        control = false;
        jumping = false;
        jumping = false;
        doubleJumping = false;
        state.immune = true;
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
        groundCheck.attacking = false;
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
        immune = state.immune;
        dive = state.dive;
        landingTimer = timers.landingTimer;
        diveAttackTimer = timers.diveAttackTimer;
    }

    private void SendValues()
    {
        state.diving = diving;
        state.control = control;
        state.jumping = jumping;
        state.doubleJumping = doubleJumping;
        state.immune = immune;
    }
}
