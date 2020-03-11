using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jump : MonoBehaviour
{
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;
    Speed_Manager speed_Manager;

    //Counters
    private int jumpCount;

    //Positional States
    private bool onGround;
    private bool onQuicksand;
    private bool onWall;
    private bool airborne;

    //Action States
    private bool control;
    private bool wantToJump;
    private bool jumping;
    private bool doubleJumping;

    //Ability Access States
    private bool doubleJump;

    //Timers
    private float displacementTimer;
    private float jumpBufferTimer;
    private float jumpTimer;

    //Timer Values
    private float jumpBufferTime;
    private float jumpTime;

    void Start()
    {
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        attributes = state.attributes;
        jumpCount = state.jumpCount;
        onGround = state.onGround;
        onQuicksand = state.onQuicksand;
        onWall = state.onWall;
        airborne = state.airborne;
        control = state.control;
        wantToJump = state.wantToJump;
        jumping = state.jumping;
        doubleJumping = state.doubleJumping;
        displacementTimer = timers.displacementTimer;
        jumpBufferTimer = timers.jumpBufferTimer;
        jumpTimer = timers.jumpTimer;
        jumpBufferTime = attributes.jumpBufferTime;
        jumpTime = attributes.jumpTime;
    }

    void Update()
    {
        ReceiveValues();
        if (control)
        {
            JumpBuffer();
            if (jumpBufferTimer <= 0.0f)
            {
                wantToJump = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                wantToJump = true;
                jumpBufferTimer = jumpBufferTime;
            }
        }
        SendValues();
    }

    public void Jump()
    {
        state.jumping = true;
        state.onGround = false;
        state.wantToJump = false;
        state.jumpCount -= 1;
        timers.jumpTimer = jumpTime;
        animator.AnimatorJump();
    }

    public void DoubleJump()
    {
        state.jumping = true;
        state.wantToJump = false;
        state.jumpCount -= 1;
        state.doubleJumping = true;
        animator.AnimatorJump();
        animator.AnimatorDoubleJump();
    }

    public void Unground() //Checks if player is in the air for more than 0.2 seconds. If true, player is considered airborne
    {
        if (!state.onGround)
        {
            if (timers.displacementTimer <= 0.0f)
            {
                state.Airborne_State();
            }
        }
    }

    private void JumpBuffer()
    {
        if (wantToJump && jumpBufferTimer <= 0.0f) 
        {
            wantToJump = false;
        }
    }

    private void ReceiveValues()
    {
        jumpCount = state.jumpCount;
        onGround = state.onGround;
        onQuicksand = state.onQuicksand;
        onWall = state.onWall;
        airborne = state.airborne;
        control = state.control;
        wantToJump = state.wantToJump;
        jumping = state.jumping;
        doubleJumping = state.doubleJumping;
        displacementTimer = timers.displacementTimer;
        jumpBufferTimer = timers.jumpBufferTimer;
        jumpTimer = timers.jumpTimer;
    }

    private void SendValues()
    {
        state.jumpCount = jumpCount;
        state.onGround = onGround;
        state.onQuicksand = onQuicksand;
        state.onWall = onWall;
        state.airborne = airborne;
        state.control = control;
        state.wantToJump = wantToJump;
        state.jumping = jumping;
        state.doubleJumping = doubleJumping;
        timers.displacementTimer = displacementTimer;
        timers.jumpBufferTimer = jumpBufferTimer;
        timers.jumpTimer = jumpTimer;
    }
}
