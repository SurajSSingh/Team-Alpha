﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Player_Controller : MonoBehaviour
{
    public PlayerObject po;
    // Fields to play around with
    [SerializeField]
    private SpriteRenderer ren;
    //[SerializeField]
    //private float moveSpeed;
    //[SerializeField]
    //private float jumpForce = 250.0f;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float maxDescendAngle = 120.0f;
    [SerializeField]
    private float rbDrag;
    [SerializeField]
    private float rbMass;
    [SerializeField]
    //private float rbGravity = -20.0f;
    private float gravity;

    public bool onGround = false;
    
    public bool onWall = false;

    public bool onSlope = false;

    private bool againstCeiling = false;

    private bool descending = false;

    [SerializeField]
    private Vector2 prevDir;
    public float sign = 0.0f;
    private float prevSign = 0.0f;
    private float wallSign = 0.0f;

    public Vector3 velocity;
    public Vector2 terminalVel;
    public Vector2 airTerminalVel;

    public bool inSession = false;

    public LayerMask collisionMask;

    public bool jumping = false;
    public bool doubleJump = false;
    public float jumpTimer;
    public float jumpTime;
    public float jumpHeight = 10.0f;
    public float timeToJumpApex = 1.0f;
    public float jumpVelocity;
    public int jumpCount = 2;

    public float wallJumpForce = 250.0f;

    private bool playerOnQuicksand = false;
    public bool wantToJump = false;
    private float timeTillJumpExpire = 0.25f;

    public bool attacking = false;
    public bool dashAttacking = false;
    public bool diving = false;
    public bool diveHit = false;
    public bool landing = false;
    public bool airborne = false;
    public bool pivoting = false;
    public float attackTimer;
    public float dashAttackTimer;
    public float displacementTimer;
    public float pivotTimer;
    public float diveWindUpTimer;

    public bool dashing = false;
    public bool dashReady = true;
    public bool dashAttack = true;
    public Vector2 dashDir;
    public float dashSpeed;
    public float dashCooldown;
    public float dashTime;
    public float dashTimer;
    public float momentumTimer;
    public float momentumFactor;
    public float runSpeedLimit;
    public bool fastRunning = false;

    public bool wallSliding = false;
    public bool wallClimbing = false;
    public bool wallJumping = false;
    public bool wantToWallJump = false;
    public Vector2 wallClimb;
    public Vector2 wallJump;
    public float jumpDirX;
    public float wallStickCooldown;
    public float wallClimbCooldown;
    public float wallSlideSpeed;
    public float wallClimbTime;
    public float wallClimbTimer;
    public float wallJumpTime;
    public float wallJumpTimer;

    public float stunTimer;
    public float stunTime;
    public float knockbackSpeed;
    public float immuneTimer;
    public float immuneTime;
    public float enemyColSign = 0.0f;
    public float reboundHeight;
    public bool stunned = false;
    public bool stepping = false;
    public bool inMist = false;

    public Animator animator;
    public AudioClip runSound;
    public AudioClip mudRunSound;
    public AudioClip groundJumpSound;
    public AudioClip wallJumpSound;
    public AudioClip knockbackSound;
    public AudioClip dashSound;
    public float soundTimeDelay = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        inSession = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = 0.5f;
        rb.mass = 1.5f;
        gravity = 6.0f;
        jumpVelocity = 16.0f;
        jumpTimer = 0.0f;
        //moveSpeed = 6.0f;
        wantToJump = false;
        dashSpeed = 35.0f;
        dashTime = 0.2f;
        dashTimer = 0.0f;
        dashCooldown = 4.0f;
        wallClimb = new Vector2(32.0f, 6.0f);
        wallJump = new Vector2(7.0f, 8.0f);
        wallSlideSpeed = -3.0f;
        wallClimbTime = 0.3f;
        wallClimbTimer = 0.0f;
        wallJumpTime = 0.8f;
        wallJumpTimer = 0.0f;
        jumpDirX = 0.0f;
        wallStickCooldown = 0.0f;
        wallClimbCooldown = 0.0f;
        stunTime = 1.0f;
        knockbackSpeed = 8.0f;
        immuneTimer = 0.0f;
        immuneTime = 2.0f;
        reboundHeight = 20.0f;
        momentumTimer = 2.5f;
        momentumFactor = 1.8f;
        runSpeedLimit = 12.0f;
        prevSign = 1.0f;
        terminalVel = new Vector2(dashSpeed, dashSpeed/1.5f);
        airTerminalVel = new Vector2(dashSpeed / 2.0f, dashSpeed / 1.5f);
        doubleJump = true;
        fastRunning = false;
        displacementTimer = 0.2f;
        pivotTimer = 0.5f;
        attackTimer = 0.8f;
        dashAttackTimer = 0.5f;
        diveWindUpTimer = 0.4f;
    }

    // Update is called once per frame
    void Update()
    {

        if ((velocity.x < -0.1 || velocity.x > 0.1) && soundTimeDelay > 0.75f) 
        {
            soundTimeDelay = 0.0f;
            if (playerOnQuicksand)
            {
                AudioSource.PlayClipAtPoint(mudRunSound,this.transform.position,2.0f);
            }
            else 
            {
                AudioSource.PlayClipAtPoint(runSound,this.transform.position,2.0f);
            }
        }
        else 
        {
            soundTimeDelay += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        UpdateSign();
        dashCooldown -= Time.deltaTime;
        wallClimbCooldown -= Time.deltaTime;
        wallStickCooldown -= Time.deltaTime;
        immuneTimer -= Time.deltaTime;
        animator.SetBool("Grounded", onGround);
        if (jumping || !onGround && !onWall)
        {
            displacementTimer -= Time.deltaTime;
            if (displacementTimer <= 0.0f)
            {
                airborne = true;
                animator.SetBool("Airborne", airborne);
                displacementTimer = 0.2f;
            }
        }
        if (airborne && velocity.y <= 0.0f)
        {
            jumping = false;
            animator.SetBool("Jumping", jumping);
        }
        if ((!stunned || immuneTimer <= 1.0f) && !wallClimbing && !wallJumping && !attacking && !diving)
        {
            if (onGround)
            {
                landing = true;
                jumping = false;
                airborne = false;
                animator.SetBool("Jumping", jumping);
                animator.SetBool("Double Jump", jumping);
                animator.SetBool("Airborne", airborne);
                wallJumping = false;
                wallClimbing = false;
                if (doubleJump)
                {
                    jumpCount = 2;
                }
                else
                {
                    jumpCount = 1;
                }
                rb.velocity = new Vector2(rb.velocity.x, 0);
                descending = velocity.y < 0.0f ? true : false;
                velocity.y = 0;
                if (onSlope && descending)
                {
                    DescendSlope(ref velocity);
                }
            }
            if (dashCooldown <= 0.0f)
            {
                dashReady = true;
            }
            if (dashing)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    dashAttacking = true;
                    DashAttack();
                    animator.SetBool("Dash Attack", dashAttacking);
                }
                else
                {
                    Dash(dashDir);
                }
            }
            else
            {
                if (input.x != 0.0f)
                {
                    if (!onGround && !onWall)
                    {
                        if (Input.GetKeyDown(KeyCode.Z))
                        {
                            diving = true;
                            animator.SetBool("Dive", diving);
                        }
                        else if (Mathf.Abs(velocity.x) >= po.moveSpeed) //influence if airborne velocity x greater than movespeed
                        {
                            velocity.x += input.x * 0.04f;
                        }
                        else
                        {
                            velocity.x += input.x * 0.2f; //influence if airborne velocity x less than movespeed
                        }
                    }
                    else
                    {
                        velocity.x = input.x * po.moveSpeed;
                        if (playerOnQuicksand)
                        {
                            velocity.x = velocity.x / 4;
                        }
                    }
                    if (momentumTimer <= 0.0f)
                    {
                        fastRunning = true;
                        velocity.x = input.x * po.moveSpeed * momentumFactor;
                    }
                    else if (fastRunning && momentumTimer > 0.0f)
                    {
                        pivotTimer = 0.5f;
                        pivoting = true;
                        animator.SetBool("Pivoting", pivoting);
                        fastRunning = false;
                    }
                }
                else if (onGround) //ground momentum simulation
                {
                    if (pivoting)
                    {
                        velocity.x = velocity.x / 1.2f;
                        pivotTimer -= Time.deltaTime;
                        if (pivotTimer <= 0.0f)
                        {
                            pivoting = false;
                            animator.SetBool("Pivoting", pivoting);
                            pivotTimer = 1.2f;
                        }
                    }
                    else
                    {
                        velocity.x = 0.0f;
                    }
                }
                else if (Mathf.Abs(velocity.x) > 1.0f) //airborne movement dampening
                {
                    velocity.x = velocity.x / 1.01f;
                }
                if (velocity.y < 0)
                {
                    velocity.y -= Mathf.Pow(gravity, 2) * Time.deltaTime;
                }
                else
                {
                    velocity.y -= gravity * Time.deltaTime;
                }
            }
            if (stepping == true)
            {
                stepping = false;
                velocity.y = reboundHeight;
            }
            if (velocity.y > 0 && !dashing)
            {
                velocity.y = velocity.y / 1.05f;
            }
            if (Input.GetKeyDown(KeyCode.E) && dashReady && input != Vector2.zero && !wallJumping)
            {
                dashReady = false;
                dashing = true;
                dashCooldown = 4.0f;
                dashDir = input;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                wantToJump = true;
            }
            if (Input.GetKeyDown(KeyCode.Z) && onGround)
            {
                attacking = true;
                animator.SetBool("Attack", attacking);
            }
            if (wantToJump && doubleJump && airborne && jumpCount == 1)
            {
                animator.SetBool("Double Jump", true);
                velocity.y = jumpVelocity;
                jumpCount -= 1;
                Jump();
                wantToJump = false;
            }
            if (wantToJump && onGround)
            {
                velocity.y = playerOnQuicksand ? jumpVelocity/4 : jumpVelocity;
                jumpCount -= 1;
                Jump();
                AudioSource.PlayClipAtPoint(groundJumpSound,this.transform.position,2.0f);
            }
            if (onWall && !onGround && velocity.y <= 2.0f && wallJumpTimer <= 0.65f && wallStickCooldown <= 0.0f)
            {
                wallClimbing = false;
                wallJumping = false;
                wallSliding = true;
                animator.SetBool("Wall Sliding", wallSliding);
                jumpCount = 0;
                velocity.x = 0;
                if (velocity.y < wallSlideSpeed)
                {
                    velocity.y = wallSlideSpeed;
                }
                rb.velocity = new Vector2(velocity.x, 0);
            }
            if (wallSliding && sign == wallSign)
            {
                DetachFromWall();
                velocity.x = po.moveSpeed;
            }
            if (wallSliding && wantToJump && !wallJumping)
            {
                if (sign == -wallSign && wallClimbCooldown <= 0.0f)
                {
                    DetachFromWall();
                    Jump();
                    wallClimbing = true;
                    wallClimbTimer = wallClimbTime;
                    wallClimbCooldown = 1.0f;
                    jumpDirX = -wallSign;
                    velocity.x = wallSign * wallClimb.x;
                    velocity.y = wallClimb.y;
                    animator.SetBool("Wall Climbing", wallClimbing);
                }
                else if (sign == 0.0f)
                {
                    DetachFromWall();
                    Jump();
                    wallJumping = true;
                    wallJumpTimer = wallJumpTime;
                    jumpDirX = wallSign;
                    ToggleFlip();
                    WallJump();
                    animator.SetBool("Wall Jumping", wallJumping);
                }
            }
            if (wantToJump && timeTillJumpExpire <= 0.0f)
            {
                wantToJump = false;
                timeTillJumpExpire = 0.25f;
            }
            else if (wantToJump)
            {
                timeTillJumpExpire -= Time.deltaTime;
            }
            if (againstCeiling)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                velocity.y = -3.0f;
            }
        }
        else if (stunned)
        {
            Knockback();
        }
        else if (wallClimbing)
        {
            WallClimb();
        }
        else if (wallJumping)
        {
            WallJump();
        }
        else if (attacking)
        {
            Attack();
        }
        else if (diving)
        {
            Dive();
        }
        prevDir = input;
        LimitSpeed();
        PlayerManager.instance.updateHealth(velocity.magnitude, inMist);
        animator.SetFloat("SpeedX", Mathf.Abs(velocity.x));
        animator.SetFloat("SpeedY", velocity.y);
        transform.Translate(velocity * Time.deltaTime);
    }

    private void Attack()
    {
        attackTimer -= Time.deltaTime;
        velocity.x = 0.2f;
        if (fastRunning)
        {
            velocity.x = velocity.x * momentumFactor;
        }
        if (attackTimer <= 0.0f)
        {
            attacking = false;
            attackTimer = 0.8f;
            animator.SetBool("Attack", attacking);
        }
    }

    private void DashAttack()
    {
        dashAttackTimer -= Time.deltaTime;
        velocity.x = velocity.x / 0.5f;
        if (dashAttackTimer <= 0.0f)
        {
            dashing = false;
            dashAttacking = false;
            dashAttackTimer = 0.5f;
            animator.SetBool("Dash Attack", dashAttacking);
        }
    }

    private void Dive()
    {
        diveWindUpTimer -= Time.deltaTime;
        velocity.x = 0;
        jumping = false;
        animator.SetBool("Jumping", jumping);
        dashing = false;
        animator.SetBool("Dashing", dashing);
        if (diveWindUpTimer <= 0.0f)
        {
            velocity.y = -17.5f;
            if (landing)
            {
                diving = false;
                animator.SetBool("Dive", diving);
            }
            else if (diveHit)
            {
                diving = false;
                animator.SetBool("Dive", diving);
                animator.SetBool("Dive Attack", diveHit);
                Rebound();
            }
        }
    }

    private void Rebound()
    {
        jumpCount = 0;
        jumping = true;
        animator.SetBool("Jumping", jumping);
        velocity.y = jumpVelocity * 1.2f;
        velocity.x = po.moveSpeed * sign;
    }

    private void Jump()
    {
        jumping = true;
        wantToJump = false;
        animator.SetBool("Jumping", jumping);
    }

    private void UpdateSign()
    {
        sign = Input.GetAxisRaw("Horizontal");
        if (sign != 0.0f)
        {
            if (sign == 1.0f)
            {
                ren.flipX = false;
            }
            else //sign == -1.0f
            {
                ren.flipX = true;
            }
        }
        if (sign == prevSign && sign != 0.0f && !onWall)
        {
            momentumTimer -= Time.deltaTime;
            animator.SetFloat("Momentum Timer", momentumTimer);
        }
        else
        {
            if (sign == 0.0f && onGround)
            {
                animator.SetBool("Running", false);
            }
            momentumTimer = 2.5f;
            fastRunning = false;
        }
        prevSign = sign;
    }

    private Vector2 GenerateRayOrigins(Vector2 direction)
    {
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        Vector2 rayOrigin = Vector2.zero;
        if (direction.x == 0.0f && direction.y < 0.0f) //Down
        {
            rayOrigin = new Vector2(bounds.center.x, bounds.max.y);
        }
        else if (direction.x == 0.0f && direction.y > 0.0f) //Up
        {
            rayOrigin = new Vector2(bounds.center.x, bounds.min.y);
        }
        else if (direction.x < 0.0f && direction.y == 0.0f) //Left
        {
            rayOrigin = new Vector2(bounds.min.x, bounds.center.y);
        }
        else if (direction.x > 0.0f && direction.y == 0.0f) //Right
        {
            rayOrigin = new Vector2(bounds.max.x, bounds.center.y);
        }
        else if (direction.x < 0.0f && direction.y < 0.0f) //DownLeft
        {
            rayOrigin = new Vector2(bounds.min.x, bounds.min.y);
        }
        else if (direction.x > 0.0f && direction.y < 0.0f) //DownRight
        {
            rayOrigin = new Vector2(bounds.max.x, bounds.min.y);
        }
        else if (direction.x < 0.0f && direction.y > 0.0f) //UpLeft
        {
            rayOrigin = new Vector2(bounds.min.x, bounds.max.y);
        }
        else if (direction.x > 0.0f && direction.y > 0.0f) //UpRight
        {
            rayOrigin = new Vector2(bounds.max.x, bounds.max.y);
        }
        return rayOrigin;
    }

    private void Dash(Vector2 direction)
    {
        dashReady = false;
        momentumTimer = 2.5f;
        AngleCheck(direction);
        if (direction != Vector2.zero)
        {
            Vector2 rayOrigin = GenerateRayOrigins(direction);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 7.0f, collisionMask);
            if (hit && hit.distance - 0.15 <= velocity.magnitude * Time.deltaTime * 1.65f)
            {
                velocity = velocity/1.8f;
                AudioSource.PlayClipAtPoint(dashSound,this.transform.position,2.0f);
            }
        }
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0.0f)
        {
            dashing = false;
            dashTimer = dashTime;
            velocity = velocity / 2.0f;
        }
    }

    private void DescendSlope(ref Vector3 velocity)
    {
        float dirX = Mathf.Sign(velocity.x);
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        Vector2 botLeft = new Vector2(bounds.min.x + velocity.x * Time.deltaTime, bounds.min.y);
        Vector2 botRight = new Vector2(bounds.max.x + velocity.x * Time.deltaTime, bounds.min.y);
        Vector2 rayOrigin = (dirX == -1) ? botRight : botLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == dirX)
                {
                    if (hit.distance - 0.15 <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {

                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x += Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * dirX - 0.15f;
                        velocity.y -= descendVelocityY - 0.15f;
                    }
                }
            }
        }
    }

    private void LimitSpeed()
    {
        float dirX = Mathf.Sign(velocity.x);
        float dirY = Mathf.Sign(velocity.y);
        if (Mathf.Abs(velocity.x) > terminalVel.x)
        {
            velocity.x = terminalVel.x * dirX;
        }
        if (Mathf.Abs(velocity.y) > terminalVel.y)
        {
            velocity.y = terminalVel.y * dirY;
        }
        if (Mathf.Abs(rb.velocity.x) > terminalVel.x)
        {
            rb.velocity = new Vector2(terminalVel.x * dirX, rb.velocity.y);
        }
        if (Mathf.Abs(rb.velocity.y) > terminalVel.y)
        {
            rb.velocity = new Vector2(rb.velocity.x, terminalVel.y * dirY);
        }
        if (!onGround && Mathf.Abs(velocity.x) > airTerminalVel.x)
        {
            velocity.x = airTerminalVel.x * dirX;
        }
    }

    private void AngleCheck(Vector2 direction)
    {
        if ((Mathf.Abs(direction.x) == 1 && Mathf.Abs(direction.y) == 1))
        {
            velocity = direction * (dashSpeed / 2);
        }
        else if (direction.x == 0 && direction.y != 0) 
        {
            velocity = direction * (dashSpeed * 0.75f);
        }
        else if (direction.x != 0 && direction.y == 0)
        {
            velocity = direction * dashSpeed;
        }
    }

    private void DetachFromWall()
    {
        wallSliding = false;
        animator.SetBool("Wall Sliding", wallSliding);
        wantToJump = false;
        wallStickCooldown = 0.3f;
        AudioSource.PlayClipAtPoint(wallJumpSound,this.transform.position,2.0f);
    }

    private void Knockback()
    {
        stunTimer -= Time.deltaTime;
        animator.SetBool("Stunned", stunned);
        dashing = false;
        animator.SetBool("Dashing", dashing);
        wallSliding = false;
        animator.SetBool("Wall Sliding", wallSliding);
        attacking = false;
        animator.SetBool("Attack", attacking);
        dashAttacking = false;
        animator.SetBool("Dash Attack", dashAttacking);
        jumping = false;
        animator.SetBool("Jumping", jumping);
        if (stunTimer <= 1.0f && stunTimer >= 0.5f)
        {
            velocity.x = enemyColSign * knockbackSpeed;
            velocity.y = 1.0f;
            AudioSource.PlayClipAtPoint(knockbackSound,this.transform.position);
        }
        else if (stunTimer <= 0.5f && stunTimer >= 0.0f)
        {
            velocity = Vector2.zero;
        }
        else
        {
            stunned = false;
            animator.SetBool("Stunned", stunned);
        }
    }

    private void WallClimb()
    {
        if (!stunned)
        {
            wallClimbTimer -= Time.deltaTime;
            if (wallClimbTimer <= wallClimbTime && wallClimbTimer >= wallClimbTime / 2)
            {
                velocity.x = -jumpDirX * po.moveSpeed / 4;
            }
            else if (wallClimbTimer > 0.0f && wallClimbTimer < wallClimbTime / 2)
            {
                velocity.x = jumpDirX * po.moveSpeed / 4;
            }
            if (wallClimbTimer <= 0.0f)
            {
                wallClimbing = false;
                animator.SetBool("Wall Climbing", wallClimbing);
            }
        }
        else
        {
            wallClimbTimer = 0.0f;
            wallClimbing = false;
            animator.SetBool("Wall Climbing", wallClimbing);
        }
    }

    private void WallJump()
    {
        if (!stunned)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= wallJumpTime && wallJumpTimer >= wallJumpTime - 0.1f)
            {
                velocity.x = jumpDirX * wallJump.x;
                velocity.y = wallJump.y;
            }
            if (wallJumpTimer <= 0.0f)
            {
                wallJumping = false;
                animator.SetBool("Wall Jumping", wallJumping);
            }
        }
        else
        {
            wallJumpTimer = 0.0f;
            wallJumping = false;
            animator.SetBool("Wall Jumping", wallJumping);
        }
    }

    private void ToggleFlip() //Flips the direction that the character is currently facing
    {
        if (ren.flipX)
        {
            ren.flipX = false;
        }
        else
        {
            ren.flipX = true;
        }
    }

    // Used to change on ground value
    public void isPlayerGrounded(bool grounded)
    {

        onGround = grounded;
    } 

    public void isPlayerEnemyTouch(bool isAttacked, float sign)
    {
        if (immuneTimer <= 0.0f)
        {
            stunned = isAttacked;
            enemyColSign = sign;
            stunTimer = stunTime;
            immuneTimer = immuneTime;
        }
    }

    public void isStepping(bool isStepping)
    {
        stepping = isStepping;
    }

    public void isPlayerWallTouch(bool wallTouching, float sign)
    {
        wallSign = sign;
        onWall = wallTouching;
    }

    public void isPlayerCeilingTouch(bool ceilingTouching)
    {
        againstCeiling = ceilingTouching;
    }

    public void isOnQuicksand(bool onQuicksand){
        playerOnQuicksand = onQuicksand;
    }

    public void isOnSlope(bool onslope)
    {
        onSlope = onslope;
    }

    public void ResetDash()
    {
        dashing = false;
        dashCooldown = 0.0f;
        dashReady = true;
        dashTimer = dashTime;
        velocity = Vector2.zero;
    }
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

    public void ChangeHealth(float value)
    {
        PlayerManager.instance.ChangeHealth(value);
    }
}
