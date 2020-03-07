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

    
    void FixedUpdate()
    {
        ReceiveValues();
        if (control)
        {
            //conditions to wall slide:
            //against a wall
            //not on ground
            //y velocity is less than 2
            //0.3 or more seconds have elapsed since player last detached from wall
            if (onWall && !onGround && velocity.y <= 2.0f && wallStickTimer <= 0.0f)
            {
                WallSlide();
            }
            if (wallSliding)
            {
                ManageWallSlide();
            }
        }
        else if (wallClimbing)
        {
            ManageWallClimb();
        }
        else if (wallJumping)
        {
            ManageWallJump();
        }
        SendValues();
    }

    public void WallSlide() //Sets player velocity to wall slide speed
    {
        wallClimbing = false;
        wallJumping = false;
        wallSliding = true;
        animator.AnimatorWallClimb();
        animator.AnimatorWallJump();
        animator.AnimatorWallSlide();
        jumpCount = 0;
    }

    public void ManageWallSlide()
    {
        if (sign == wallSign || stunned || onGround)
        {
            DetachFromWall();
        }
        else if (wantToJump && sign == -wallSign && wallClimbCooldownTimer <= 0.0f) //Player facing towards wall and space entered
        {
            WallClimb();
        }
        else if (wantToJump && sign == wallSign && wallJumpCooldownTimer <= 0.0f) //Player facing away from wall and space entered
        {
            WallJump();
        }
    }

    public void ManageWallClimb() //If player is stunned, cancel wall climb, else continue
    {
        if (!stunned)
        {
            if (wallClimbTimer <= 0.0f)
            {
                wallClimbing = false;
                animator.AnimatorWallClimb();
                control = true;
            }
        }
        else
        {
            wallClimbing = false;
            animator.AnimatorWallClimb();
        }
    }

    public void WallClimbMove() //Simulate wall climb movement over wallClimbTime duration
    {
        if (wallClimbTimer <= wallClimbTime && wallClimbTimer >= wallClimbTime / 2.0f)
        {
            velocity.x = wallSign * moveSpeed / 4.0f;
        }
        else if (wallClimbTimer > 0.0f && wallClimbTimer < wallClimbTime / 2.0f)
        {
            velocity.x = -wallSign * moveSpeed / 4.0f;
        }
    }

    public void WallClimb()
    {
        DetachFromWall();
        wallClimbing = true;
        animator.AnimatorWallClimb();
        control = false;
        wallClimbTimer = wallClimbTime;
        wallClimbCooldownTimer = wallClimbCooldown;
        velocity.x = -wallSign * wallClimbVel.x;
        velocity.y = wallClimbVel.y;
    }

    public void ManageWallJump() //If player is stunned, cancel wall jump, else continue
    {
        if (!stunned)
        {
            if (wallJumpTimer <= 0.0f)
            {
                wallJumping = false;
                animator.AnimatorWallJump();
                control = true;
            }
        }
        else
        {
            wallJumpTimer = 0.0f;
            wallJumping = false;
            animator.AnimatorWallJump();
        }
    }

    public void WallJumpMove() //Simulate wall jump movement over wallJumpTime duration
    {
        if (wallJumpTimer <= wallJumpTime && wallJumpTimer >= 0.0f)
        {
            velocity.x = -wallSign * wallJumpVel.x;
            velocity.y = wallJumpVel.y;
        }
    }

    public void WallJump()
    {
        DetachFromWall();
        wallJumping = true;
        control = false;
        animator.AnimatorWallJump();
        wallJumpTimer = wallJumpTime;
        animator.ToggleFlip();
        ManageWallJump();
    }

    public void DetachFromWall()
    {
        wallSliding = false;
        animator.AnimatorWallSlide();
        wantToJump = false;
        wallStickTimer = wallStickCooldown;
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

    private void SendValues()
    {
        speedManager.velocity = velocity;
        state.jumpCount = jumpCount;
        state.onGround = onGround;
        state.onWall = onWall;
        state.control = control;
        state.wantToJump = wantToJump;
        state.wallSliding = wallSliding;
        state.wallClimbing = wallClimbing;
        state.wallJumping = wallJumping;
        state.stunned = stunned;
        state.sign = sign;
        state.wallSign = wallSign;
        timers.jumpBufferTimer = jumpBufferTimer;
        timers.wallStickTimer = wallStickTimer;
        timers.wallClimbTimer = wallClimbTimer;
        timers.wallClimbCooldownTimer = wallClimbCooldownTimer;
        timers.wallJumpTimer = wallJumpTimer;
        timers.wallJumpCooldownTimer = wallJumpCooldownTimer;
    }
}
