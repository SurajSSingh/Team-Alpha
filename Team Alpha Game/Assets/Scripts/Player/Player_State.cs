using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State : MonoBehaviour
{
    Player_Con pc;
    Player_Animator animator;
    Player_Timers timers;
    Player_Dash pDash;
    Player_Jump pJump;
    Player_Wall_Actions wallActions;
    Speed_Manager speedManager;
    public Player_Attributes attributes;

    //Velocity
    private Vector3 velocity;

    //Counters
    public int jumpCount = 2;

    //Positional States
    public bool onGround = false;
    public bool onWall = false;
    public bool onSlope = false;
    public bool onQuicksand = false; //(* used to be playerOnQuicksand)
    public bool againstCeiling = false;
    public bool airborne = false; //true when player is not on ground or wall
    public bool inMist = false; //true when player is in the mist

    //Action States
    public bool descending = false; //true when airborne and velocity.y < 0
    public bool inSession = false; //when true, active level is in session
    public bool control = false; //when true, player can move and perform actions
    public bool jumping = false; //true when player jumps off ground
    public bool doubleJumping = false; //true when player performs a second jump while airborne
    public bool wantToJump = false; //true when player presses space key
    public bool attacking = false; //true when using normal attack / no control while true
    public bool diving = false; //true when using dive / no control while true
    public bool diveHit = false; //true when player collides with enemy below during dive, ends dive
    public bool pivoting = false; //true when player changes direction or stops movement during sprint / no control while true
    public bool dashing = false; //true when player is dashing / no control while true
    public bool dashReady = false; //true when dash is ready to use
    public bool dashAttacking = false; //true when attack input is pressed during dash / no control while true
    public bool sprinting = false; //(* used to be fastRunning) true when grounded and moving in same direction for longer than momentumTime
    public bool wallSliding = false; //true when player is on wall and performing wall slide
    public bool wallClimbing = false; //true when player is performing a wall climb / no control while true
    public bool wallJumping = false; //true when player is performing a wall jump / no control while true
    public bool attacked = false; //true when player collides with enemy
    public bool stunned = false; //true if player is not immune when attacked; no control while true
    public bool immune = false; //when true, player is immune to further enemy damage and collisions


    //Directional States
    public Vector2 input; //Direction player wants to move
    public float sign = 0.0f; //Direction player is facing
    public float prevSign = 1.0f; //Direction player was previously facing last frame
    public float wallSign = 0.0f; //Direction player is facing relative to wall (wallSign = facing away / -wallSign = facing towards)
    public float pivotSign = 1.0f; //(*new) Direction player is pivoting towards
    public Vector2 dashDir; //Direction player is dashing towards
    public float jumpDirX = 0.0f; //Direction player is jumping (used for wall climb/jump)
    public float enemyColSign = 0.0f; //Direction player is knocked back when colliding with an enemy (enemyColSign = facing away / -enemyColSign = facing towards)

    //Ability Access States (true when unlocked)
    public bool dive = false; //(*new)
    public bool dash = false; //(*new)
    public bool doubleJump = false;

    //Timers
    private float jumpTimer;
    private float displacementTimer;
    private float pivotTimer;
    private float momentumTimer;
    private float wallStickTimer;

    //Timer Values
    private float jumpTime;
    private float displacementTime;
    private float pivotTime;
    private float momentumTime;
    private float wallStickCooldown;

    void Start()
    {
        pc = GetComponent<Player_Con>();
        animator = GetComponent<Player_Animator>();
        timers = GetComponent<Player_Timers>();
        pDash = GetComponent<Player_Dash>();
        pJump = GetComponent<Player_Jump>();
        wallActions = GetComponent<Player_Wall_Actions>();
        speedManager = GetComponent<Speed_Manager>();
        ReceiveValues();
        jumpTime = attributes.jumpTime;
        displacementTime = attributes.displacementTime;
        pivotTime = attributes.pivotTime;
        momentumTime = attributes.momentumTime;
        wallStickCooldown = attributes.wallStickCooldown;
        jumpCount = 2;
        sign = 0.0f;
        prevSign = 1.0f;
        wallSign = 0.0f;
        jumpDirX = 0.0f;
        enemyColSign = 0.0f;
        control = true;
        airborne = true;
        //Lines below for testing purposes
        dive = true;
        dash = true;
        doubleJump = true;
    }

    void FixedUpdate()
    {
        ReceiveValues();
        Momentum_State();
        pJump.Unground(); //Delay for transition from grounded to airborne
        if (airborne) 
        {
            if (wantToJump && jumpCount == 1 && doubleJump && jumpTimer <= 0.0f) //If player used 1st jump, pressed space and has access to double jump ability
            {
                pJump.DoubleJump();
            }
            if (velocity.y < 0.0f && !doubleJumping) //Triggers descending animation
            {
                Descend_State();
            }
            else
            {
                descending = false;
            }
        }
        else if (onGround)
        {
            if (momentumTimer > 0.0f || input.x == 0.0f || input.x != prevSign) //If player changes direction or stops moving while sprinting
            {
                if (sprinting)
                {
                    Pivot();
                }
            }
            if (onSlope) //If player is standing on a slope
            {
                Slope_State();
            }
            if (momentumTimer <= 0.0f) //If player has been running in same direction for more than momentumTime, sprint
            {
                Sprint();
            }
            if (wantToJump) //Player inputted space up to 0.25 seconds ago
            {
                pJump.Jump();
            }
        }
        if (onWall)
        {
            Wall_State();
            if (!onGround && velocity.y <= 2.0f && wallStickTimer <= 0.0f && !wallSliding) 
            //If player is on wall, not touching ground, has low vertical speed and the wall stick cooldown is over, attach to wall to perform a wall slide
            {
                wallActions.WallSlide();
            }
            if (wallSliding)
            {
                Slide_State();
                wallActions.ManageWallSlide();
            }
        }
        if (againstCeiling)
        {
            Ceiling_State();
        }
        else if (wallClimbing)
        {
            wallActions.ManageWallClimb();
        }
        else if (wallJumping)
        {
            wallActions.ManageWallJump();
        }
        else if (pivoting)
        {
            ManagePivot();
        }
        else if (dashing)
        {
            pDash.ManageDash();
        }
        else if (attacking)
        {

        }
        else if (diving)
        {

        }
        else if (stunned)
        {
            Stunned_State();
        }
        SendValues();
    }

    //States
    public void Reset_State()
    {
        onGround = false;
        onWall = false;
        onSlope = false;
        onQuicksand = false;
        againstCeiling = false;
        airborne = true;
        descending = false;
        control = true;
        jumping = false;
        doubleJumping = false;
        wantToJump = false;
        attacking = false;
        diving = false;
        diveHit = false;
        pivoting = false;
        dashing = false;
        dashReady = false;
        dashAttacking = false;
        sprinting = false;
        wallSliding = false;
        wallClimbing = false;
        wallJumping = false;
        attacked = false;
        stunned = false;
        immune = false;
        jumpCount = 0;
        animator.AnimatorGrounded();
        animator.AnimatorAirborne();
        animator.AnimatorJump();
        animator.AnimatorDoubleJump();
        animator.AnimatorAttack();
        animator.AnimatorDive();
        animator.AnimatorDiveAttack();
        animator.AnimatorMomentum();
        animator.AnimatorPivot();
        animator.AnimatorDash();
        animator.AnimatorDashAttack();
        animator.AnimatorSpeedX(velocity.x);
        animator.AnimatorSpeedY(velocity.y);
        animator.AnimatorWallSlide();
        animator.AnimatorWallClimb();
        animator.AnimatorWallJump();
        animator.AnimatorStunned();
    }

//Positional States

    public void Airborne_State()
    {
        airborne = true;
        onGround = false;
        onSlope = false;
        onQuicksand = false;
        animator.AnimatorAirborne();
        animator.AnimatorGrounded();
    }

    public void Grounded_State() //Sets all mutually exclusive states and animations when grounded to false
    {
        onGround = true;
        descending = false;
        airborne = false;
        diving = false;
        wallJumping = false;
        wallClimbing = false;
        animator.AnimatorGrounded();
        animator.AnimatorAirborne();
        animator.AnimatorWallClimb();
        animator.AnimatorWallJump();
        animator.AnimatorDive();
        if (doubleJump) //If player has access to double jump ability, reset to 2 jumps when landing on ground, else 1
        {
            jumpCount = 2;
        }
        else
        {
            jumpCount = 1;
        }
    }

    public void Wall_State() //Sets all mutually exclusive states and animations when against a wall to false
    {
        sprinting = false;
        pivoting = false;
        dashing = false;
        momentumTimer = momentumTime;
        animator.AnimatorMomentum();
        animator.AnimatorPivot();
        animator.AnimatorDash();
    }

    public void Ceiling_State() //Sets all mutually exclusive states and animations when against a ceiling to false; additionally disables jumping and remaining jumps
    {
        jumping = false;
        doubleJumping = false;
        jumpCount = 0;
        animator.AnimatorJump();
        animator.AnimatorDoubleJump();
    }

//Action States
    private void Descend_State()
    {
        descending = true;
        jumping = false;
        doubleJumping = false;
        animator.AnimatorJump();
        animator.AnimatorDoubleJump();
    }

    private void Momentum_State() //If player is running in same direction, reduce momentumTimer
    {
        if (sign == prevSign && sign != 0.0f && !onWall)
        {
            momentumTimer -= Time.deltaTime;
        }
        else
        {
            sprinting = false;
            momentumTimer = momentumTime;
            animator.AnimatorMomentum();
        }
    }

    private void Slide_State() //Sets all mutually exclusive states and animations when wall sliding to false
    {
        control = true;
        jumping = false;
        doubleJumping = false;
        airborne = false;
        wallClimbing = false;
        wallJumping = false;
        diving = false;
        animator.AnimatorJump();
        animator.AnimatorDoubleJump();
        animator.AnimatorAirborne();
        animator.AnimatorWallClimb();
        animator.AnimatorWallJump();
        animator.AnimatorDive();
    }

    private void Slope_State()
    {
        pivoting = false;
        animator.AnimatorPivot();
    }

    private void Stunned_State() //Sets all mutually exclusive states and animations when stunned to false
    {
        control = false;
        descending = false;
        jumping = false;
        doubleJumping = false;
        attacking = false;
        diving = false;
        pivoting = false;
        dashing = false;
        dashAttacking = false;
        sprinting = false;
        wallSliding = false;
        wallClimbing = false;
        wallJumping = false;
        animator.AnimatorStunned();
        animator.AnimatorJump();
        animator.AnimatorDoubleJump();
        animator.AnimatorAttack();
        animator.AnimatorDive();
        animator.AnimatorPivot();
        animator.AnimatorDash();
        animator.AnimatorDashAttack();
        animator.AnimatorMomentum();
        animator.AnimatorWallSlide();
        animator.AnimatorWallClimb();
        animator.AnimatorWallJump();
    }
    private void Sprint()
    {
        sprinting = true;
        animator.AnimatorMomentum();
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

    private void ManagePivot()
    {
        if (pivotTimer <= 0.0f || !onGround || onWall || onSlope)
        {
            pivoting = false;
            animator.AnimatorPivot();
            control = true;
        }
    }

    private void ReceiveValues()
    {
        velocity = speedManager.velocity;
        jumpTimer = timers.jumpTimer;
        displacementTimer = timers.displacementTimer;
        pivotTimer = timers.pivotTimer;
        momentumTimer = timers.momentumTimer;
        wallStickTimer = timers.wallStickTimer;
    }

    private void SendValues()
    {
        speedManager.velocity = velocity;
        timers.jumpTimer = jumpTimer;
        timers.displacementTimer = displacementTimer;
        timers.momentumTimer = momentumTimer;
    }

    //Triggers

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mist"))
        {
            inMist = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mist"))
        {
            inMist = false;
        }
    }
}
