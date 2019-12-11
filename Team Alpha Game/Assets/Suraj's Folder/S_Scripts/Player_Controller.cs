using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Player_Controller : MonoBehaviour
{
    // Fields to play around with
    [SerializeField]
    private float moveSpeed = 10.0f;
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

    private bool onGround = false;
    
    private bool onWall = false;

    private bool againstCeiling = false;

    [SerializeField]
    private Vector2 prevDir;
    private float sign = 0.0f;
    private float wallSign = 0.0f;

    public Vector3 velocity;
    public Vector2 terminalVel;

    public bool inSession = false;
    public bool playerJump = false;

    public LayerMask collisionMask;

    //public float fastDescent = 5.0f;
    //public float terminalVel = -20.0f;
    public float jumpHeight = 10.0f;
    public float timeToJumpApex = 1.0f;
    public float jumpVelocity;

    public float wallJumpForce = 250.0f;

    private bool playerOnQuicksand = false;
    private bool wantToJump = false;
    private float timeTillJumpExpire = 0.4f;

    public bool dashing = false;
    public bool dashReady = true;
    public float dashSpeed;
    public float dashCooldown;
    public float dashTime;
    public float dashTimer;

    public bool wallSliding = false;
    public Vector2 wallClimb;
    public Vector2 wallJump;
    public float wallSlideSpeed;
    public float wallStickTime;
    public float wallStickTimer;
    public float wallStickCooldown;

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
    

    // Start is called before the first frame update
    void Start()
    {
        inSession = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = 0.5f;
        rb.mass = 1.5f;
        gravity = 9.8f;
        jumpVelocity = 22.0f;
        dashSpeed = 35.0f;
        dashTime = 0.2f;
        dashTimer = 0.0f;
        dashCooldown = 4.0f;
        wallClimb = new Vector2(32.0f, 16.0f);
        wallJump = new Vector2(18.0f, 22.0f);
        wallSlideSpeed = -3f;
        wallStickTime = 0.1f;
        wallStickCooldown = 0.0f;
        stunTime = 1.0f;
        knockbackSpeed = 8.0f;
        immuneTimer = 0.0f;
        immuneTime = 2.0f;
        reboundHeight = 20.0f;
        terminalVel = new Vector2(dashSpeed, dashSpeed);
    }

    // Update is called once per frame
    void Update()
    {

        if ((velocity.x < -0.001 || velocity.x > 0.001) && soundTimeDelay > 0.5f) 
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
        animator.SetFloat("Horizontal",Input.GetAxisRaw("Horizontal"));
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        dashCooldown -= Time.deltaTime;
        immuneTimer -= Time.deltaTime;
        if (!stunned || immuneTimer <= 1.0f)
        {
            if (onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                velocity.y = 0;
                DescendSlope(ref velocity);
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
                velocity.x = playerOnQuicksand ? input.x * moveSpeed / 4 : input.x * moveSpeed;
                velocity.y -= gravity * Time.deltaTime;
            }
            if (stepping)
            {
                stepping = false;
                velocity.y = reboundHeight;
            }
            if (velocity.y > 0)
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
            if (Input.GetKeyDown(KeyCode.E) && dashReady)
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
                wantToJump = false;
                AudioSource.PlayClipAtPoint(groundJumpSound,this.transform.position,2.0f);
            }
            sign = Mathf.Sign(Input.GetAxis("Horizontal"));
            if (!wallSliding)
            {
                wallStickCooldown -= Time.deltaTime;
            }
            if (onWall && !onGround && wallStickCooldown <= 0.0f)
            {
                wallSliding = true;
                velocity.x = 0;
                if (velocity.y < wallSlideSpeed)
                    velocity.y = wallSlideSpeed;
                if (wallStickTimer > 0.0f)
                {
                    velocity.x = 0;
                    if (sign == wallSign)
                    {
                        wallStickTimer -= Time.deltaTime;
                    }
                    else
                    {
                        wallStickTimer = wallStickTime;
                    }
                }
                else
                {
                    wallStickTimer = wallStickTime;
                }
                rb.velocity = new Vector2(velocity.x, 0);
            }
            if (wallSliding && wantToJump)
            {
                if (sign != wallSign && input.x == sign)
                {
                    velocity.x = wallSign * wallClimb.x;
                    velocity.y = wallClimb.y;
                    DetachFromWall();
                }
                else if (sign == wallSign && wallStickTimer <= 0.0f)
                {
                    velocity.x = wallSign * wallJump.x;
                    velocity.y = wallJump.y;
                    DetachFromWall();
                }
                else if (input.x == 0.0f)
                {
                    velocity.x = wallSign * 20.0f;
                    velocity.y = -1.0f;
                    DetachFromWall();
                }
            }
            if (wantToJump && timeTillJumpExpire <= 0.0f)
            {
                wantToJump = false;
                timeTillJumpExpire = 0.4f;
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
        else
        {
            Knockback();
        }
        prevDir = input;
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
        if (velocity.x > terminalVel.x)
        {
            velocity.x = terminalVel.x;
        }
        if (velocity.y > terminalVel.y)
        {
            velocity.y = terminalVel.y;
        }
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
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, Mathf.Infinity, collisionMask);
            if (hit && hit.distance - 0.15 <= velocity.magnitude * Time.deltaTime * 1.65f)
            {
                // Debug.Log("Should Dash");
                velocity = velocity*3;
                AudioSource.PlayClipAtPoint(dashSound,this.transform.position,2.0f);
            }
        }
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0.0f)
        {
            dashing = false;
            dashTimer = dashTime;
        }
    }

    private void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        Vector2 botLeft = new Vector2(bounds.min.x + velocity.x * Time.deltaTime, bounds.min.y);
        Vector2 botRight = new Vector2(bounds.max.x + velocity.x * Time.deltaTime, bounds.min.y);
        Vector2 rayOrigin = (directionX == -1) ? botRight : botLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - 0.15 <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {

                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x += Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;
                    }
                }
            }
        }
    }

    private void AngleCheck(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) == 1 && Mathf.Abs(direction.y) == 1)
        {
            velocity = direction * (dashSpeed / 2);
        }
        else if (direction.x == 0 && direction.y != 0 || direction.y == 0 && direction.x != 0)
        {
            velocity = direction * dashSpeed;
        }
    }

    private void DetachFromWall()
    {
        wallSliding = false;
        wantToJump = false;
        wallStickCooldown = 0.6f;
        AudioSource.PlayClipAtPoint(wallJumpSound,this.transform.position,2.0f);
    }

    private void Knockback()
    {
        // Debug.Log("yes");
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
        stepping = true;
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

    public void ResetDash()
    {
        dashing = false;
        dashCooldown = 0.0f;
        dashReady = true;
        dashTimer = dashTime;
        velocity = Vector2.zero;
    }
}
