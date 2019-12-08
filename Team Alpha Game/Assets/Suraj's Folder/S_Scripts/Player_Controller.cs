using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float rbDrag = 0.5f;
    [SerializeField]
    private float rbMass = 0.5f;
    [SerializeField]
    //private float rbGravity = -20.0f;
    private float gravity;

    private bool onGround = false;
    
    private bool onWall = false;

    [SerializeField]
    private float prevSign = 0.0f;
    private float sign = 0.0f;
    private float wallSign = 0.0f;

    public Vector3 velocity;

    public bool inSession = false;
    public bool playerJump = false;

    //public float fastDescent = 5.0f;
    //public float terminalVel = -20.0f;
    public float jumpHeight = 10.0f;
    public float timeToJumpApex = 1.0f;
    public float jumpVelocity;

    public float wallJumpForce = 250.0f;

    private bool playerOnQuicksand = false;
    private bool wantToJump = false;
    private float timeTillJumpExpire = 0.75f;

    private bool dashing = false;
    private bool dashReady = true;
    private float dashSpeed = 100.0f;
    private float dashCooldown = 3.0f;
    private float dashTimer = 0.2f;

    private bool wallSliding = false;
    private Vector2 wallClimb;
    private Vector2 wallJump;
    private float wallSlideSpeed;
    private float wallStickTime;
    private float wallStickTimer;

    private float stunTimer;
    private float stunTime;
    private float knockbackSpeed;
    private float immuneTimer;
    private float immuneTime;
    private float enemyColSign = 0.0f;
    private float reboundHeight;
    private bool stunned = false;
    private bool stepping = true;

    // Start is called before the first frame update
    void Start()
    {
        inSession = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = rbDrag;
        rb.mass = rbMass;
        gravity = 3.5f;
        jumpVelocity = 22.0f;
        dashTimer = 0.0f;
        dashCooldown = 4.0f;
        wallClimb = new Vector2(10.0f, 16.0f);
        wallJump = new Vector2(18.0f, 22.0f);
        wallSlideSpeed = -1.5f;
        wallStickTime = 0.15f;
        stunTime = 1.0f;
        knockbackSpeed = 4.0f;
        immuneTimer = 0.0f;
        immuneTime = 2.0f;
        reboundHeight = 20.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
            if (stepping)
            {
                stepping = false;
                velocity.y = reboundHeight;
            }
            else
            {
                velocity.x = playerOnQuicksand ? input.x*moveSpeed/4 : input.x * moveSpeed;
                velocity.y -= gravity * Time.deltaTime;
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
            }
            wallSliding = false;
            sign = Mathf.Sign(Input.GetAxis("Horizontal"));
            if (onWall && !onGround)
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
                if (sign != wallSign)
                {
                    velocity.x = -sign * wallClimb.x;
                    velocity.y = wallClimb.y;
                    wallSliding = false;
                    wantToJump = false;
                }
                if (sign == wallSign && wallStickTimer <= 0.0f)
                {
                    velocity.x = wallSign * wallJump.x;
                    velocity.y = wallJump.y;
                    wantToJump = false;
                }
            }
            if (wantToJump && timeTillJumpExpire <= 0.0f)
            {
                wantToJump = false;
                timeTillJumpExpire = 0.75f;
            }
            else if (wantToJump)
            {
                timeTillJumpExpire -= Time.deltaTime;
            }
        }
        else
        {
            Knockback();
        }
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

    private void Dash(Vector2 input)
    {
        dashReady = false;
        velocity = input * dashSpeed;
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0.0f)
        {
            dashing = false;
            dashTimer = 0.2f;
        }
    }
    private void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        Vector2 botLeft = new Vector2(bounds.min.x + velocity.x, bounds.min.y);
        Vector2 botRight = new Vector2(bounds.max.x + velocity.x, bounds.min.y);
        Debug.Log(botLeft);
        Debug.Log(botRight);
        Vector2 rayOrigin = (directionX == -1) ? botRight : botLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity);
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

    private void Knockback()
    {
        if (stunTimer <= 1.0f && stunTimer >= 0.5f)
        {
            velocity.x = enemyColSign * knockbackSpeed;
            velocity.y = 1.0f;
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

    public void isOnQuicksand(bool onQuicksand){
        playerOnQuicksand = onQuicksand;
    }
}
