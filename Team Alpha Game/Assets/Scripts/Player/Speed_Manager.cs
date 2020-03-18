using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed_Manager : MonoBehaviour
{
    //This script is essentially a player controller script

    Rigidbody2D rb;
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;
    Player_Wall_Actions wallActions;
    Player_Jump pJump;
    Player_Dash pDash;
    Player_Dive pDive;
    Player_Collisions pCol;
    Player_Attack pAttack;
    Player_Move move;
    PlayerManager playerManager;

    //Velocity
    public Vector3 velocity;

    //Positional States
    private bool onGround;
    private bool onWall;
    private bool onSlope;
    private bool onQuicksand;
    private bool againstCeiling;
    private bool airborne;
    private bool inMist;

    //Action States
    private bool descending;
    private bool inSession;
    private bool control;
    private bool jumping;
    private bool doubleJumping;
    private bool wantToJump;
    private bool attacking;
    private bool diving;
    private bool diveHit;
    private bool pivoting;
    private bool dashing;
    private bool dashAttacking;
    private bool sprinting;
    private bool wallSliding;
    private bool wallClimbing;
    private bool wallJumping;
    private bool stunned;
    private bool death;

    //Directional States
    private Vector2 input;
    private float sign;
    private float prevSign;
    private float wallSign;
    private float pivotSign;
    private Vector2 dashDir;
    private float jumpDirX;
    private float enemyColSign;

    //Timers
    private float pivotTimer;
    private float momentumTimer;
    private float wallStickTimer;

    //Movement Stats
    private float moveSpeed;
    private float jumpVelocity;
    private float maxDescendAngle;
    private float dashSpeed;
    private float momentumFactor;
    private float wallSlideSpeed;

    //Timer Values
    private float pivotTime;
    private float momentumTime;
    private float wallStickCooldown;

    //Physics
    private Vector2 terminalVel;
    private Vector2 airTerminalVel;
    LayerMask slopeCollisionMask;
    private float gravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<Player_State>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        attributes = state.attributes;
        wallActions = GetComponent<Player_Wall_Actions>();
        pJump = GetComponent<Player_Jump>();
        pDash = GetComponent<Player_Dash>();
        pDive = GetComponent<Player_Dive>();
        pCol = GetComponent<Player_Collisions>();
        pAttack = GetComponent<Player_Attack>();
        move = GetComponent<Player_Move>();
        playerManager = GetComponent<PlayerManager>();
        rb.drag = 0.5f;
        rb.mass = 1.5f;
        ReceiveValues();
        moveSpeed = attributes.moveSpeed;
        jumpVelocity = attributes.jumpVelocity;
        maxDescendAngle = attributes.maxDescendAngle;
        dashSpeed = attributes.dashSpeed;
        momentumFactor = attributes.momentumFactor;
        wallSlideSpeed = attributes.wallSlideSpeed;
        pivotTime = attributes.pivotTime;
        momentumTime = attributes.momentumTime;
        wallStickCooldown = attributes.wallStickCooldown;
        terminalVel = new Vector2(dashSpeed / 2.0f, dashSpeed / 1.5f);
        airTerminalVel = new Vector2(dashSpeed / 3.0f, dashSpeed);
        slopeCollisionMask = attributes.slopeCollisionMask;
        gravity = attributes.gravity;
    }

    void FixedUpdate()
    {
        ReceiveValues();
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        UpdateSign();
        if (onGround)
        {
            velocity.y = 0;
        }
        if (control) //If player currently has control over character
        {
            if (onGround)
            {
                if (input.x != 0.0f) //If player input a direction
                {
                    if (sprinting) //If player continuously moved in same direction for momentumTime(2.5 seconds), sprint
                    {
                        move.Sprint(ref velocity, input.x, onSlope);
                    }
                    else
                    {
                        velocity.x = input.x * moveSpeed;
                    }
                    if (onQuicksand)
                    {
                        velocity.x = velocity.x / 2.0f;
                    }
                }
                else //No input
                {
                    velocity.x = 0;
                }
                if (jumping) //If on quicksand, cut jump speed by 4, else jump normally
                {
                    velocity.y = onQuicksand ? jumpVelocity / 2.0f : jumpVelocity;
                    jumping = false;
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
                else //No input
                {
                    if (Mathf.Abs(velocity.x) > 1.0f) //Airborne movement influence
                    {
                        velocity.x = velocity.x / 1.01f;
                    }
                }
                if (Mathf.Abs(velocity.x) > 1.0f) //Airborne horizontal movement dampening
                {
                    velocity.x = velocity.x / 1.01f;
                }
                if (doubleJumping) //Second jump
                {
                    velocity.y = jumpVelocity * 2.0f;
                }
                if (velocity.y <= 0.0f) //Fast descending
                {
                    velocity.y -= Mathf.Pow(gravity, 2) * Time.deltaTime;
                }
                else //Dampen rising speed
                {
                    velocity.y = velocity.y / 1.1f;
                    velocity.y -= gravity * Time.deltaTime;
                }
            }
            if (onWall)
            {
                if (sign == -wallSign)
                {
                    if (onGround || wallStickTimer > 0.0f)
                    {
                        velocity.x = -velocity.x;
                    }
                }
                if (wallSliding)
                {
                    if (velocity.y > wallSlideSpeed)
                    {
                        velocity.y = wallSlideSpeed;
                    }
                }
            }
        }
        else if (wallClimbing)
        {
            wallActions.WallClimbMove(ref velocity, wallSign);
        }
        else if (wallJumping)
        {
            wallActions.WallJumpMove(ref velocity, wallSign);
        }
        else if (dashing)
        {
            pDash.Dash(ref velocity, dashDir);
        }
        else if (pivoting)
        {
            move.Pivot(ref velocity, pivotSign, pivotTimer);
        }
        else if (diving)
        {
            pDive.Dive(ref velocity);
        }
        else if (diveHit)
        {
            pDive.Rebound(ref velocity, sign);
        }
        else if (attacking)
        {
            pAttack.Attack(ref velocity, sign);
        }
        else if (stunned)
        {
            pCol.Knockback(ref velocity, enemyColSign);
        }
        if (againstCeiling)
        {
            velocity.y = -3.0f;
        }
        if (onWall && wallStickTimer > 0.0f)
        {
            velocity.x = wallSign * 3.0f;
        }
        if (onSlope && input.x != 0.0f && velocity.y <= 0.0f)
        {
            move.DescendSlope(ref velocity);
        }
        if (!dashing && !doubleJumping)
        {
            LimitSpeed();
        }
        if (doubleJumping)
        {
            doubleJumping = false;
        }
        if (death)
        {
            velocity = Vector2.zero;
        }
        SendValues();
        animator.AnimatorSpeedX(Mathf.Abs(velocity.x));
        animator.AnimatorSpeedY(velocity.y);
        PlayerManager.instance.updateHealth(velocity.magnitude, inMist);
        transform.Translate(velocity * Time.deltaTime);
    }
    private void UpdateSign() //Update sprite flip depending on player input direction and manage momentum by comparing current input direction to previous input direction
    {
        sign = input.x;
        if (control)
        {
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
        }
        prevSign = sign;
    }

    private void LimitSpeed() //Limits speed if player speed exceeds limit, maintaining x-to-y velocity ratio if declining on a slope
    {
        float max; //Determine if x or y velocity is larger
        string larger; //Larger velocity value
        if (velocity.x >= velocity.y)
        {
            max = velocity.x;
            larger = "x";
        }
        else
        {
            max = velocity.y;
            larger = "y";
        }
        float dirX = Mathf.Sign(velocity.x);
        float dirY = Mathf.Sign(velocity.y);
        if (Mathf.Abs(velocity.x) > terminalVel.x)
        {
            velocity.x = terminalVel.x * dirX;
        }
        if (Mathf.Abs(velocity.y) > airTerminalVel.y)
        {
            velocity.y = terminalVel.y * dirY;
        }
        if (!onGround && Mathf.Abs(velocity.x) > airTerminalVel.x)
        {
            velocity.x = airTerminalVel.x * dirX;
        }
        else if (onSlope && velocity.x != 0.0f && velocity.y != 0.0f)
        {
            float ratio;
            if (larger == "x")
            {
                ratio = max / velocity.x;
                velocity.y = velocity.y / ratio;
            }
            else //larger == "y"
            {
                ratio = max / velocity.y;
                velocity.x = velocity.x / ratio;
            }
        }
    }

    private void ReceiveValues()
    {
        onGround = state.onGround;
        onWall = state.onWall;
        onSlope = state.onSlope;
        onQuicksand = state.onQuicksand;
        againstCeiling = state.againstCeiling;
        airborne = state.airborne;
        inMist = state.inMist;
        descending = state.descending;
        inSession = state.inSession;
        control = state.control;
        jumping = state.jumping;
        doubleJumping = state.doubleJumping;
        wantToJump = state.wantToJump;
        attacking = state.attacking;
        diving = state.diving;
        diveHit = state.diveHit;
        pivoting = state.pivoting;
        dashing = state.dashing;
        dashAttacking = state.dashAttacking;
        sprinting = state.sprinting;
        wallSliding = state.wallSliding;
        wallClimbing = state.wallClimbing;
        wallJumping = state.wallJumping;
        stunned = state.stunned;
        death = state.death;
        input = state.input;
        sign = state.sign;
        prevSign = state.prevSign;
        wallSign = state.wallSign;
        pivotSign = state.pivotSign;
        dashDir = state.dashDir;
        jumpDirX = state.jumpDirX;
        enemyColSign = state.enemyColSign;
        pivotTimer = timers.pivotTimer;
        momentumTimer = timers.momentumTimer;
        wallStickTimer = timers.wallStickTimer;
    }

    private void SendValues()
    {
        state.sign = sign;
        state.prevSign = prevSign;
        state.input = input;
        state.jumping = jumping;
        state.doubleJumping = doubleJumping;
        timers.momentumTimer = momentumTimer;
    }
}
