using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Con : MonoBehaviour
{
    Rigidbody2D rb;
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;
    Speed_Manager speedManager;

    //Velocity
    private Vector3 velocity;

    //Positional States
    private bool onGround;
    private bool onWall;
    private bool onSlope;
    private bool onQuicksand;
    private bool airborne;

    //Action States
    private bool control;
    private bool pivoting;
    private bool dashing;
    private bool sprinting;

    //Directional States
    private Vector2 input;
    private float sign;
    private float prevSign;
    private float pivotSign;

    //Timers
    private float pivotTimer;
    private float momentumTimer;

    //Movement Stats
    private float moveSpeed;
    private float momentumFactor;

    //Timer Values
    private float pivotTime;
    private float momentumTime;

    //Physics
    private float gravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        attributes = state.attributes;
        speedManager = GetComponent<Speed_Manager>();
        velocity = speedManager.velocity;
        onGround = state.onGround;
        onWall = state.onWall;
        onSlope = state.onSlope;
        onQuicksand = state.onQuicksand;
        airborne = state.airborne;
        control = state.control;
        pivoting = state.pivoting;
        dashing = state.dashing;
        sprinting = state.sprinting;
        input = state.input;
        sign = state.sign;
        prevSign = state.prevSign;
        pivotSign = state.pivotSign;
        pivotTimer = timers.pivotTimer;
        momentumTimer = timers.momentumTimer;
        moveSpeed = attributes.moveSpeed;
        momentumFactor = attributes.momentumFactor;
        pivotTime = attributes.pivotTime;
        momentumTime = attributes.momentumTime;
        rb.drag = attributes.rbDrag;
        rb.mass = attributes.rbMass;
        gravity = attributes.gravity;
    }

    void FixedUpdate()
    {
        ReceiveValues();
        UpdateSign();
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (control)
        {
            if (onGround)
            {
                if (input.x != 0.0f) //If player input a direction
                {
                    if (momentumTimer > 0.0f && sprinting) //If player changes direction and is currently sprinting, trigger pivoting animation
                    {
                        Pivot();
                    }
                    else if (momentumTimer <= 0.0f) //If player continuously moved in same direction for momentumTime(2.5 seconds), sprint
                    {
                        Sprint();
                    }
                    else
                    {
                        velocity.x = input.x * moveSpeed;
                    }
                    if (onQuicksand)
                    {
                        velocity.x = velocity.x / 4.0f;
                    }
                }
                else //no input
                {
                    velocity.x = 0;
                }
            }
            else if (airborne)
            {
                if (input.x != 0.0f) //If player input a direction
                {
                    if (Mathf.Abs(velocity.x) >= moveSpeed) //Influence if airborne velocity x greater than movespeed
                    {
                        velocity.x += input.x * 0.04f;
                    }
                    else
                    {
                        velocity.x += input.x * 0.2f; //Influence if airborne velocity x less than movespeed
                    }
                }
                else //no input
                {
                    if (Mathf.Abs(velocity.x) > 1.0f) //airborne movement dampening
                    {
                        velocity.x = velocity.x / 1.01f;
                    }
                }
                if (velocity.y < 0.0f) //fast descending
                {
                    velocity.y -= Mathf.Pow(gravity, 2) * Time.deltaTime;
                }
                else if (velocity.y > 0 && !dashing) //dampen rising speed
                {
                    velocity.y = velocity.y / 1.05f;
                }
                else
                {
                    velocity.y -= gravity * Time.deltaTime;
                }
            }
        }
        else if (pivoting)
        {
            ManagePivot();
        }
        SendValues();
    }

    private void UpdateSign()
    {
        sign = Input.GetAxisRaw("Horizontal");
        if (sign != 0.0f) //If player is facing opposite direction, flip sprite horizontally
        {
            if (sign == 1.0f)
            {
                animator.SetFlip(false);
            }
            else //sign == -1.0f
            {
                animator.SetFlip(true);
            }
        }
        if (sign == prevSign && sign != 0.0f && !onWall) //If player is running in same direction, reduce momentumTimer
        {
            momentumTimer -= Time.deltaTime;
            animator.AnimatorMomentum();
        }
        else
        {
            momentumTimer = momentumTime;
            sprinting = false;
        }
        prevSign = sign;
    }


    private void ManagePivot()
    {
        velocity.x = pivotSign * velocity.x / 1.2f;
        if (pivotTimer <= 0.0f || !onGround || onWall)
        {
            pivoting = false;
            animator.AnimatorPivot();
            control = true;
        }
    }

    private void Pivot()
    {
        pivotTimer = pivotTime;
        pivoting = true;
        animator.AnimatorPivot();
        sprinting = false;
        control = false;
        pivotSign = prevSign;
    }

    private void Sprint()
    {
        sprinting = true;
        animator.AnimatorMomentum();
        velocity.x = sign * input.x * moveSpeed * momentumFactor;
        if (onSlope)
        {
            velocity.y = velocity.y * momentumFactor;
        }
    }

    private void ReceiveValues()
    {
        velocity = speedManager.velocity;
        onGround = state.onGround;
        onWall = state.onWall;
        onSlope = state.onSlope;
        onQuicksand = state.onQuicksand;
        airborne = state.airborne;
        control = state.control;
        pivoting = state.pivoting;
        dashing = state.dashing;
        sprinting = state.sprinting;
        input = state.input;
        sign = state.sign;
        prevSign = state.prevSign;
        pivotSign = state.pivotSign;
        pivotTimer = timers.pivotTimer;
        momentumTimer = timers.momentumTimer;
    }

    private void SendValues()
    {
        state.onGround = onGround;
        state.onWall = onWall;
        state.onSlope = onSlope;
        state.onQuicksand = onQuicksand;
        state.airborne = airborne;
        state.control = control;
        state.pivoting = pivoting;
        state.dashing = dashing;
        state.sprinting = sprinting;
        state.input = input;
        state.sign = sign;
        state.prevSign = prevSign;
        timers.pivotTimer = pivotTimer;
        timers.momentumTimer = momentumTimer;
    }
}
