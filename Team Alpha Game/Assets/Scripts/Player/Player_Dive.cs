using System.Collections;
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

    //Ability Access States
    private bool dive;

    //Timers
    private float landingTimer;

    //Movement Stats
    private float moveSpeed;
    private float jumpVelocity;
    private float diveSpeed;

    //Timer Values
    private float landingTime;

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
        diveCollisionMask = attributes.diveCollisionMask;
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

    public void Rebound(ref Vector3 velocity, float sign) //If player hit enemy during dive, rebound and leap off enemy
    {
        velocity.x = sign * moveSpeed * 1.5f;
        velocity.y = jumpVelocity * 2.0f;
        state.diveHit = false;
        state.diving = false;
        animator.AnimatorDiveAttack();
        groundCheck.attacking = false;
    }

    private void CheckEnemyBelow(ref Vector3 velocity) //Check if there is an enemy directly below player; if true, trigger dive attack
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        Vector2 rayOrigin1 = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 rayOrigin2 = new Vector2(bounds.max.x, bounds.min.y);
        Vector2 rayOrigin3 = new Vector2(bounds.center.x, bounds.min.y);
        RaycastHit2D hit1 = Physics2D.Raycast(rayOrigin1, Vector2.down, 0.7f, diveCollisionMask);
        RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin2, Vector2.down, 0.7f, diveCollisionMask);
        RaycastHit2D hit3 = Physics2D.Raycast(rayOrigin3, Vector2.down, 0.7f, diveCollisionMask);
        if (hit1 || hit2 || hit3)
        {
            DiveAttack();
        }
    }

    private void DiveAttack() //Trigger dive attack animation and inflict damage on enemy
    {
        state.diveHit = true;
        animator.AnimatorDiveAttack();
    }

    private void StartDive()
    {
        diving = true;
        control = false;
        jumping = false;
        doubleJumping = false;
        groundCheck.attacking = true;
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
