using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Fields to play around with
    [SerializeField]
    private float moveSpeed = 8.0f;
    //[SerializeField]
    //private float jumpForce = 250.0f;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float maxDescendAngle = 120.0f;

    [SerializeField]
    private float rbDrag = 0.5f;
    [SerializeField]
    private float rbMass = 0.5f;
    [SerializeField]
    //private float rbGravity = -20.0f;
    private float gravity;

    private bool onGround = false;
    
    private bool onWall = false;

<<<<<<< Updated upstream
=======
    private bool onSlope = false;

    private bool againstCeiling = false;

    private bool descending = false;

>>>>>>> Stashed changes
    [SerializeField]
    private float sign = 0.0f;

    public Vector3 velocity;
<<<<<<< Updated upstream
=======
    public Vector2 terminalVel;
    public Vector2 airTerminalVel;
>>>>>>> Stashed changes

    public bool inSession = false;

<<<<<<< Updated upstream
    //public float fastDescent = 5.0f;
    //public float terminalVel = -20.0f;
=======
    public LayerMask collisionMask;

    public bool jumping = false;
>>>>>>> Stashed changes
    public float jumpHeight = 10.0f;
    public float timeToJumpApex = 1.0f;
    public float jumpVelocity;

    public float wallSign = 0.0f;
    public float wallJumpForce = 250.0f;

<<<<<<< Updated upstream
=======
    private bool playerOnQuicksand = false;
    public bool wantToJump = false;
    private float timeTillJumpExpire = 0.4f;

    public bool dashing = false;
    public bool dashReady = true;
    public float dashSpeed;
    public float dashCooldown;
    public float dashTime;
    public float dashTimer;

    public bool wallSliding = false;
    public bool wallClimbing = false;
    public bool wallJumping = false;
    public bool wantToWallJump = false;
    public Vector2 wallClimb;
    public Vector2 wallJump;
    public float jumpDirX;
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

    public Animator animator;
    public AudioClip runSound;
    public AudioClip mudRunSound;
    public AudioClip groundJumpSound;
    public AudioClip wallJumpSound;
    public AudioClip knockbackSound;
    public AudioClip dashSound;
    public float soundTimeDelay = 0.5f;
    

>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        inSession = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
<<<<<<< Updated upstream
        rb.drag = rbDrag;
        rb.mass = rbMass;
        gravity = 3.5f;
        jumpVelocity = 22.0f;
        //rb.gravityScale = rbGravity;
=======
        rb.drag = 0.5f;
        rb.mass = 1.5f;
        gravity = 7.5f;
        jumpVelocity = 16.0f;
        wantToJump = false;
        dashSpeed = 35.0f;
        dashTime = 0.2f;
        dashTimer = 0.0f;
        dashCooldown = 4.0f;
        wallClimb = new Vector2(32.0f, 6.0f);
        wallJump = new Vector2(14.0f, 12.0f);
        wallSlideSpeed = -3.0f;
        wallClimbTime = 0.3f;
        wallClimbTimer = 0.0f;
        wallJumpTime = 0.8f;
        wallJumpTimer = 0.0f;
        jumpDirX = 0.0f;
        wallClimbCooldown = 0.0f;
        stunTime = 1.0f;
        knockbackSpeed = 8.0f;
        immuneTimer = 0.0f;
        immuneTime = 2.0f;
        reboundHeight = 20.0f;
        terminalVel = new Vector2(dashSpeed, dashSpeed/1.5f);
        airTerminalVel = new Vector2(dashSpeed / 2.0f, dashSpeed / 1.5f);
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velocity.x = input.x * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        if (onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            velocity.y = 0;
            DescendSlope(ref velocity);
        }
        if (velocity.y > 0)
        {
            velocity.y = velocity.y / 1.05f;
        }
<<<<<<< Updated upstream
        if ((Input.GetKeyDown(KeyCode.Space) && onGround)){
            velocity.y = jumpVelocity;
        }
        if (Mathf.Sign(Input.GetAxis("Horizontal")) != sign)
=======
    }

    void FixedUpdate()
    {
        animator.SetFloat("Horizontal",Input.GetAxisRaw("Horizontal"));
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        dashCooldown -= Time.deltaTime;
        wallClimbCooldown -= Time.deltaTime;
        immuneTimer -= Time.deltaTime;
        if ((!stunned || immuneTimer <= 1.0f) && !wallClimbing)
        {
            if (onGround)
            {
                jumping = false;
                wallJumping = false;
                wallClimbing = false;
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
                Dash(input);
            }
            else
            {
                if (!wallJumping)
                {
                    if (input.x != 0.0f)
                    {
                        if (!onGround && !onWall)
                        {
                            if (Mathf.Abs(velocity.x) >= moveSpeed) //influence if airborne velocity x greater than movespeed
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
                            velocity.x = playerOnQuicksand ? input.x * moveSpeed / 4 : input.x * moveSpeed;
                        }
                    }
                    else if (onGround) //ground momentum simulation
                    {
                        velocity.x = velocity.x / 1.2f;
                    }
                    else if (velocity.x > 1.0f) //airborne movement dampening
                    {
                        velocity.x = velocity.x / 1.01f;
                    }
                }
                else
                {
                    WallJump();
                }
                if (velocity.y < 0)
                {
                    velocity.y -= Mathf.Pow(gravity, 2) * Time.deltaTime;
                }
                else
                {
                    velocity.y -= gravity * Time.deltaTime;
                }
                velocity.y -= gravity * Time.deltaTime;
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
            if (dashReady)
            {
                PlayerManager.instance.updateDash("Dash Available");
            }
            else
            {
                PlayerManager.instance.updateDash("Dash Unavailable");
            }
            if (Input.GetKeyDown(KeyCode.E) && dashReady && input != Vector2.zero && !wallJumping)
            {
                dashReady = false;
                dashing = true;
                dashCooldown = 4.0f;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                wantToJump = true;
            }
            if (wantToJump && onGround)
            {
                velocity.y = playerOnQuicksand ? jumpVelocity/4 : jumpVelocity;
                jumping = true;
                wantToJump = false;
                AudioSource.PlayClipAtPoint(groundJumpSound,this.transform.position,2.0f);
            }
            sign = Mathf.Sign(Input.GetAxis("Horizontal"));
            if (onWall && !onGround && velocity.y <= 2.0f && wallJumpTimer <= 0.65f)
            {
                wallClimbing = false;
                wallJumping = false;
                wallSliding = true;
                velocity.x = 0;
                if (velocity.y < wallSlideSpeed)
                {
                    velocity.y = wallSlideSpeed;
                }
                rb.velocity = new Vector2(velocity.x, 0);
            }
            if (wallSliding && input.x == wallSign)
            {
                DetachFromWall();
                velocity.x = moveSpeed;
            }
            if (wallSliding && wantToJump && !wallJumping)
            {
                if (input.x == -wallSign && wallClimbCooldown <= 0.0f)
                {
                    DetachFromWall();
                    jumping = true;
                    wallClimbing = true;
                    wallClimbTimer = wallClimbTime;
                    wallClimbCooldown = 1.0f;
                    jumpDirX = -wallSign;
                    velocity.x = wallSign * wallClimb.x;
                    velocity.y = wallClimb.y;
                }
                else if (input.x == 0.0f)
                {
                    DetachFromWall();
                    jumping = true;
                    wallJumping = true;
                    wallJumpTimer = wallJumpTime;
                    jumpDirX = wallSign;
                    velocity.x = jumpDirX * wallJump.x;
                    velocity.y = wallJump.y;
                }
            }
            if (wantToJump && timeTillJumpExpire <= 0.0f)
            {
                wantToJump = false;
                timeTillJumpExpire = 0.15f;
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
>>>>>>> Stashed changes
        {
            velocity.x = velocity.x / 2;
        }
<<<<<<< Updated upstream
        sign = Mathf.Sign(Input.GetAxis("Horizontal"));
    //    if (Mathf.Sign(Input.GetAxis("Horizontal")) != sign) {
    //        rb.velocity = new Vector2(rb.velocity.x/2,rb.velocity.y);
    //    }
    //    if (Mathf.Sign(rb.velocity.y) < 0.0f && rb.velocity.y > terminalVel){
    //        rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*fastDescent);
    //    }
    //    sign = Mathf.Sign(Input.GetAxis("Horizontal"));
    //    Vector2 movement = new Vector2(Input.GetAxis("Horizontal")*moveSpeed,0.0f);
    //    if (Input.GetKeyDown(KeyCode.Space) && onGround){
    //        // Debug.Log("Spacebar pushed");
    //        movement = new Vector2(movement.x, jumpForce);
    //    }
    //    if ((Input.GetKeyDown(KeyCode.Space) && onWall)){
    //        movement = new Vector2(wallSign*wallJumpForce, jumpForce);
    //    }
    //    // rb.position += movement;
    //    rb.AddForce(movement);
    //    if (onGround)
    //        playerJump = false;
    //   else
    //        playerJump = true;
    //
    //    var vel = rb.velocity;
    //    float speed = vel.magnitude;
    //    //Debug.Log(speed);
        PlayerManager.instance.updateHealth(velocity.magnitude);
        transform.Translate(velocity * Time.deltaTime);
    }
=======
        else if (wallClimbing)
        {
            WallClimb();
        }
        prevDir = input;
        LimitSpeed();
        PlayerManager.instance.updateHealth(velocity.magnitude);
        transform.Translate(velocity * Time.deltaTime);
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
        else if (direction.x > 0.0f && direction.y > 0.0f) // UpRight
        {
            rayOrigin = new Vector2(bounds.max.x, bounds.max.y);
        }
        return rayOrigin;
    }

    private void Dash(Vector2 input)
    {
        dashReady = false;
        Vector2 direction;
        if (input == Vector2.zero)
        {
            direction = prevDir;
        }
        else
        {
            direction = input;
        }
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
>>>>>>> Stashed changes

    private void DescendSlope(ref Vector3 velocity)
    {
        float dirX = Mathf.Sign(velocity.x);
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
<<<<<<< Updated upstream
        Vector2 rayOrigin = (directionX == -1) ? new Vector2(bounds.max.x, bounds.min.y) : new Vector2(bounds.min.x, bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity);
=======
        Vector2 botLeft = new Vector2(bounds.min.x + velocity.x * Time.deltaTime, bounds.min.y);
        Vector2 botRight = new Vector2(bounds.max.x + velocity.x * Time.deltaTime, bounds.min.y);
        Vector2 rayOrigin = (dirX == -1) ? botRight : botLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
>>>>>>> Stashed changes
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            Debug.Log(slopeAngle);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
<<<<<<< Updated upstream
                Debug.Log("ok?");
                if (Mathf.Sign(hit.normal.x) == directionX)
=======
                if (Mathf.Sign(hit.normal.x) == dirX)
>>>>>>> Stashed changes
                {
                    Debug.Log("OK");
                    if (hit.distance - 0.15 <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        Debug.Log("...");
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
<<<<<<< Updated upstream
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
=======
                        velocity.x += Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * dirX;
>>>>>>> Stashed changes
                        velocity.y -= descendVelocityY;
                    }
                }
            }
        }
    }

<<<<<<< Updated upstream
=======
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
        wantToJump = false;
        AudioSource.PlayClipAtPoint(wallJumpSound,this.transform.position,2.0f);
    }

    private void Knockback()
    {
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
        }
    }

    private void WallClimb()
    {
        if (!stunned)
        {
            wallClimbTimer -= Time.deltaTime;
            if (wallClimbTimer <= wallClimbTime && wallClimbTimer >= wallClimbTime / 2)
            {
                velocity.x = -jumpDirX * moveSpeed / 4;
            }
            else if (wallClimbTimer > 0.0f && wallClimbTimer < wallClimbTime / 2)
            {
                velocity.x = jumpDirX * moveSpeed / 4;
            }
            if (wallClimbTimer <= 0.0f)
            {
                wallClimbing = false;
            }
        }
        else
        {
            wallClimbTimer = 0.0f;
            wallClimbing = false;
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
            }
        }
        else
        {
            wallJumpTimer = 0.0f;
            wallJumping = false;
        }
    }

>>>>>>> Stashed changes
    // Used to change on ground value
    public void isPlayerGrounded(bool grounded)
    {
        onGround = grounded;
    } 

<<<<<<< Updated upstream
=======
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

>>>>>>> Stashed changes
    public void isPlayerWallTouch(bool wallTouching, float sign)
    {
        wallSign = sign;
        onWall = wallTouching;
    }
<<<<<<< Updated upstream
=======

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
>>>>>>> Stashed changes
}
