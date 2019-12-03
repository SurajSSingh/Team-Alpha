using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Fields to play around with
    [SerializeField]
<<<<<<< HEAD
    private float moveSpeed = 10.0f;
<<<<<<< HEAD
    [SerializeField]
    private float jumpForce = 350.0f;
=======
    private float moveSpeed = 8.0f;
    //[SerializeField]
    //private float jumpForce = 250.0f;
>>>>>>> master
=======
    //[SerializeField]
    //private float jumpForce = 250.0f;
>>>>>>> master
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float maxDescendAngle = 120.0f;

    [SerializeField]
    private float rbDrag = 0.5f;
    [SerializeField]
    private float rbMass = 0.5f;
    [SerializeField]
<<<<<<< HEAD
<<<<<<< HEAD
    private float rbGravity = 2.0f;
=======
    //private float rbGravity = -20.0f;
    private float gravity;
>>>>>>> master
=======
    //private float rbGravity = -20.0f;
    private float gravity;
>>>>>>> master

    private bool onGround = false;

    private bool onWall = false;

    [SerializeField]
    private float prevSign = 0.0f;
    private float sign = 0.0f;
    private float wallSign = 0.0f;

    public Vector3 velocity;

    public Vector3 velocity;

    public bool inSession = false;
    public bool playerJump = false;

<<<<<<< HEAD
<<<<<<< HEAD
    public float fastDescent = 1.3f;
    public float terminalVel = -6.0f;
=======
=======
>>>>>>> master
    //public float fastDescent = 5.0f;
    //public float terminalVel = -20.0f;
    public float jumpHeight = 10.0f;
    public float timeToJumpApex = 1.0f;
    public float jumpVelocity;
<<<<<<< HEAD
>>>>>>> master
=======
>>>>>>> master

    public float wallJumpForce = 250.0f;
    private bool wantToJump = false;
    private float quicksandMultiplier = 1.0f;

    // private bool wantToJump = false;
    private bool onQuicksand = false;

    private int wallJumpCount = 0;
    private int wallJumpMax = 3;

    private bool wantToJump = false;
    private bool onQuicksand = false;

    private int wallJumpCount = 0;
    private int wallJumpMax = 3;

    private bool playerOnQuicksand = false;
    private bool wantToJump = false;
    private float timeTillJumpExpire = 0.75f;

    private bool dashing = false;
    private bool dashReady = true;
    private float dashSpeed = 35.0f;
    private float dashCooldown = 8.0f;
    private float dashTimer = 0.2f;

    private bool wallSliding = false;
    private Vector2 wallClimb;
    private Vector2 wallJump;
    private float wallSlideSpeed = -1.5f;
    private float wallStickTime = 0.1f;
    private float wallStickTimer;

    // Start is called before the first frame update
    void Start()
    {
        inSession = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = rbDrag;
        rb.mass = rbMass;
        gravity = 3.5f;
        jumpVelocity = 22.0f;
<<<<<<< HEAD
=======
        dashTimer = 0.0f;
        dashCooldown = 4.0f;
        wallClimb = new Vector2(10.0f, 16.0f);
        wallJump = new Vector2(18.0f, 22.0f);
        wallSlideSpeed = -1.5f;
        wallStickTime = 0.1f;
>>>>>>> master
        //rb.gravityScale = rbGravity;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            wantToJump = true;
        }
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space)){
    //         wantToJump = true;
    //         jumpWaitTime += Time.deltaTime;
    //     }
    // }
    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
<<<<<<< HEAD
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
<<<<<<< HEAD
        sign = Mathf.Sign(Input.GetAxis("Horizontal"));
        float quicksandMult = onQuicksand? 0.5f:1.0f;
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal")*moveSpeed*quicksandMult,0.0f);
        if (wantToJump && onGround){
            // Debug.Log("Spacebar pushed");
            wantToJump = false;
            movement = new Vector2(movement.x, jumpForce*quicksandMult);
        }
        else if (wallJumpCount < wallJumpMax && wantToJump && onWall){
            wantToJump = false;
            wallJumpCount += 1;
            movement = new Vector2(wallSign*wallJumpForce*quicksandMult, jumpForce/wallJumpCount);
        }
        else if (onGround && !wantToJump){
            wallJumpCount = 0;
=======
        if ((Input.GetKeyDown(KeyCode.Space) && onGround)){
            velocity.y = jumpVelocity;
        }
        if (Mathf.Sign(Input.GetAxis("Horizontal")) != sign)
        {
            velocity.x = velocity.x / 2;
        }
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

    // private void DescendSlope(ref Vector3 velocity)
    // {
    //     float directionX = Mathf.Sign(velocity.x);
    //     Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
    //     Vector2 rayOrigin = (directionX == -1) ? new Vector2(bounds.max.x, bounds.min.y) : new Vector2(bounds.min.x, bounds.min.y);
    //     RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity);
    //     if (hit)
    //     {
    //         float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
    //         Debug.Log(slopeAngle);
    //         if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
    //         {
    //             Debug.Log("ok?");
    //             if (Mathf.Sign(hit.normal.x) == directionX)
    //             {
    //                 Debug.Log("OK");
    //                 if (hit.distance - 0.15 <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
    //                 {
    //                     Debug.Log("...");
    //                     float moveDistance = Mathf.Abs(velocity.x);
    //                     float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
    //                     velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
    //                     velocity.y -= descendVelocityY;
    //                 }
    //             }
    //         }
    //     }
    //     if ((Input.GetKeyDown(KeyCode.Space) && onGround)){
    //         velocity.y = jumpVelocity;
    //     }
    //     if (Mathf.Sign(Input.GetAxis("Horizontal")) != sign)
    //     {
    //         velocity.x = velocity.x / 2;
    //     }
    //     sign = Mathf.Sign(Input.GetAxis("Horizontal"));
    // //    if (Mathf.Sign(Input.GetAxis("Horizontal")) != sign) {
    // //        rb.velocity = new Vector2(rb.velocity.x/2,rb.velocity.y);
    // //    }
    // //    if (Mathf.Sign(rb.velocity.y) < 0.0f && rb.velocity.y > terminalVel){
    // //        rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*fastDescent);
    // //    }
    // //    sign = Mathf.Sign(Input.GetAxis("Horizontal"));
    // //    Vector2 movement = new Vector2(Input.GetAxis("Horizontal")*moveSpeed,0.0f);
    // //    if (Input.GetKeyDown(KeyCode.Space) && onGround){
    // //        // Debug.Log("Spacebar pushed");
    // //        movement = new Vector2(movement.x, jumpForce);
    // //    }
    // //    if ((Input.GetKeyDown(KeyCode.Space) && onWall)){
    // //        movement = new Vector2(wallSign*wallJumpForce, jumpForce);
    // //    }
    // //    // rb.position += movement;
    // //    rb.AddForce(movement);
    // //    if (onGround)
    // //        playerJump = false;
    // //   else
    // //        playerJump = true;
    // //
    // //    var vel = rb.velocity;
    // //    float speed = vel.magnitude;
    // //    //Debug.Log(speed);
    //     PlayerManager.instance.updateHealth(velocity.magnitude);
    //     transform.Translate(velocity * Time.deltaTime);
    // }

    private void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        Vector2 rayOrigin = (directionX == -1) ? new Vector2(bounds.max.x, bounds.min.y) : new Vector2(bounds.min.x, bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity);
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            Debug.Log(slopeAngle);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                Debug.Log("ok?");
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    Debug.Log("OK");
                    if (hit.distance - 0.15 <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        Debug.Log("...");
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;
                    }
                }
            }
>>>>>>> master
        }
=======
        dashCooldown -= Time.deltaTime;
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
            velocity.x = input.x * moveSpeed;
            velocity.y -= gravity * Time.deltaTime;
        }
        if (onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            velocity.y = 0;
            DescendSlope(ref velocity);
        }
        else if (velocity.y > 0)
        {
            velocity.y = velocity.y / 1.05f;
        }
        if (Input.GetKeyDown(KeyCode.Z) && dashReady)
        {
            dashReady = false;
            dashing = true;
            dashCooldown = 4.0f;
        }
        if (Input.GetKeyDown(KeyCode.Space)){
            wantToJump = true;
        }
        if (wantToJump && onGround && !playerOnQuicksand){
            velocity.y = jumpVelocity;
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
                    wallStickTimer = 0.15f;
                }
            }
            else
            {
                wallStickTimer = 0.15f;
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
        prevSign = sign;
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
>>>>>>> master
    }

    // Used to change on ground value
    public void isPlayerGrounded(bool grounded)
    {

        onGround = grounded;
    }

    public void isPlayerWallTouch(bool wallTouching, float sign)
    {
        wallSign = sign;
        onWall = wallTouching;
<<<<<<< HEAD
<<<<<<< HEAD
    } 

    public void isOnQuicksand(bool touchSand)
    {
        onQuicksand = touchSand;
    } 


=======
    }
>>>>>>> master
}
=======
    }

    public void isOnQuicksand(bool onQuicksand){
        playerOnQuicksand = onQuicksand;
    }
}
>>>>>>> master
