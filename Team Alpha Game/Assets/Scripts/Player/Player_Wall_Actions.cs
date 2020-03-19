using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Wall_Actions : MonoBehaviour
{
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;
    Speed_Manager speedManager;

    //Velocity
    private Vector3 velocity;

    //Counters
    private int jumpCount;

    //Positional States
    private bool onGround;
    private bool onWall;

    //Action States
    private bool control;
    private bool wantToJump;
    private bool wallSliding;
    private bool wallClimbing;
    private bool wallJumping;
    private bool stunned;

    //Directional States
    private float sign;
    private float wallSign;

    //Timers
    private float jumpBufferTimer;
    private float wallStickTimer;
    private float wallClimbTimer;
    private float wallClimbCooldownTimer;
    private float wallJumpTimer;
    private float wallJumpCooldownTimer;

    //Movement Stats
    private float moveSpeed;
    private float wallSlideSpeed;
    private Vector2 wallClimbVel;
    private Vector2 wallJumpVel;

    //Timer Values
    private float wallStickCooldown;
    private float wallClimbCooldown;
    private float wallClimbTime;
    private float wallJumpCooldown;
    private float wallJumpTime;

    void Start()
    {
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        attributes = state.attributes;
        speedManager = GetComponent<Speed_Manager>();
        velocity = speedManager.velocity;
        jumpCount = state.jumpCount;
        onGround = state.onGround;
        onWall = state.onWall;
        control = state.control;
        wantToJump = state.wantToJump;
        wallSliding = state.wallSliding;
        wallClimbing = state.wallClimbing;
        wallJumping = state.wallJumping;
        stunned = state.stunned;
        sign = state.sign;
        wallSign = state.wallSign;
        jumpBufferTimer = timers.jumpBufferTimer;
        wallStickTimer = timers.wallStickTimer;
        wallClimbTimer = timers.wallClimbTimer;
        wallClimbCooldownTimer = timers.wallClimbCooldownTimer;
        wallJumpTimer = timers.wallJumpTimer;
        wallJumpCooldownTimer = timers.wallJumpCooldownTimer;
        moveSpeed = attributes.moveSpeed;
        wallSlideSpeed = attributes.wallSlideSpeed;
        wallClimbVel = attributes.wallClimbVel;
        wallJumpVel = attributes.wallJumpVel;
        wallStickCooldown = attributes.wallStickCooldown;
        wallClimbCooldown = attributes.wallClimbCooldown;
        wallClimbTime = attributes.wallClimbTime;
        wallJumpCooldown = attributes.wallJumpCooldown;
        wallJumpTime = attributes.wallJumpTime;
    }

    public void WallSlide() //Sets player velocity to wall slide speed
    {
        state.wallClimbing = false;
        state.wallJumping = false;
        state.wallSliding = true;
        animator.AnimatorWallClimb();
        animator.AnimatorWallJump();
        animator.AnimatorWallSlide();
        state.jumpCount = 0;
    }

    public void ManageWallSlide()
    {
        if (state.sign == state.wallSign || state.stunned || state.onGround || !state.onWall || state.airborne)
        {
            DetachFromWall();
        }
        else if (state.wantToJump && state.sign == -state.wallSign && timers.wallClimbCooldownTimer <= 0.0f) //Player facing towards wall and space entered
        {
            WallClimb();
        }
        else if (state.wantToJump && state.sign != -state.wallSign && timers.wallJumpCooldownTimer <= 0.0f) //Player facing away from wall and space entered
        {
            WallJump();
        }
    }

    public void ManageWallClimb() //If player is stunned, cancel wall climb, else continue
    {
        if (!state.stunned)
        {
            if (timers.wallClimbTimer <= 0.0f)
            {
                state.wallClimbing = false;
                animator.AnimatorWallClimb();
                state.control = true;
            }
        }
        else
        {
            state.wallClimbing = false;
            animator.AnimatorWallClimb();
        }
    }

    public void WallClimbMove(ref Vector3 velocity, float wallSign) //Simulate wall climb movement over wallClimbTime duration
    {
        velocity.y = wallClimbVel.y;
        if (timers.wallClimbTimer <= wallClimbTime && timers.wallClimbTimer >= wallClimbTime / 5.0f * 3)
        {
            velocity.x = wallSign * moveSpeed / 4.0f;
        }
        else if (timers.wallClimbTimer > 0.0f && timers.wallClimbTimer < wallClimbTime / 5.0f * 2)
        {
            velocity.x = -wallSign * moveSpeed / 4.0f;
        }
    }

    public void WallClimb()
    {
        DetachFromWall();
        state.wallClimbing = true;
        animator.AnimatorWallClimb();
        state.control = false;
        timers.wallClimbTimer = wallClimbTime;
        timers.wallClimbCooldownTimer = wallClimbCooldown;
    }

    public void ManageWallJump() //If player is stunned, cancel wall jump, else continue
    {
        if (!state.stunned)
        {
            if (timers.wallJumpTimer <= 0.0f)
            {
                state.wallJumping = false;
                animator.AnimatorWallJump();
                state.control = true;
            }
        }
        else
        {
            timers.wallJumpTimer = 0.0f;
            state.wallJumping = false;
            animator.AnimatorWallJump();
        }
    }

    public void WallJumpMove(ref Vector3 velocity, float wallSign) //Simulate wall jump movement over wallJumpTime duration
    {
        if (timers.wallJumpTimer <= wallJumpTime && timers.wallJumpTimer >= 0.0f)
        {
            velocity.x = wallSign * wallJumpVel.x;
            velocity.y = wallJumpVel.y;
        }
    }

    public void WallJump()
    {
        DetachFromWall();
        state.wallJumping = true;
        state.control = false;
        animator.AnimatorWallJump();
        timers.wallJumpTimer = wallJumpTime;
        timers.wallJumpCooldownTimer = wallJumpCooldown;
        animator.ToggleFlip();
        ManageWallJump();
    }

    public void DetachFromWall()
    {
        state.wallSliding = false;
        animator.AnimatorWallSlide();
        state.wantToJump = false;
        timers.wallStickTimer = wallStickCooldown;
    }

    private void ReceiveValues()
    {
        velocity = speedManager.velocity;
        jumpCount = state.jumpCount;
        onGround = state.onGround;
        onWall = state.onWall;
        control = state.control;
        wantToJump = state.wantToJump;
        wallSliding = state.wallSliding;
        wallClimbing = state.wallClimbing;
        wallJumping = state.wallJumping;
        stunned = state.stunned;
        sign = state.sign;
        wallSign = state.wallSign;
        jumpBufferTimer = timers.jumpBufferTimer;
        wallStickTimer = timers.wallStickTimer;
        wallClimbTimer = timers.wallClimbTimer;
        wallClimbCooldownTimer = timers.wallClimbCooldownTimer;
        wallJumpTimer = timers.wallJumpTimer;
        wallJumpCooldownTimer = timers.wallJumpCooldownTimer;
    }
}
