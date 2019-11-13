using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Fields to play around with
    [SerializeField]
    private float moveSpeed = 10.0f;
    [SerializeField]
    private float jumpForce = 350.0f;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float rbDrag = 0.5f;
    [SerializeField]
    private float rbMass = 0.5f;
    [SerializeField]
    private float rbGravity = 2.0f;

    private bool onGround = false;
    
    private bool onWall = false;

    [SerializeField]
    private float sign = 0.0f;

    public bool inSession = false;
    public bool playerJump = false;

    public float fastDescent = 1.3f;
    public float terminalVel = -6.0f;

    public float wallSign = 0.0f;
    public float wallJumpForce = 250.0f;
    private bool wantToJump = false;
    private float quicksandMultiplier = 1.0f;

    private int maxWallJumps = 3;
    private int wallJumpsDone = 0;

    private float maxJumpWait = 0.3f;
    private float jumpWaitTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        inSession = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = rbDrag;
        rb.mass = rbMass;
        rb.gravityScale = rbGravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            wantToJump = true;
            jumpWaitTime += Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        if (Mathf.Sign(Input.GetAxis("Horizontal")) != sign) {
            rb.velocity = new Vector2(rb.velocity.x/2,rb.velocity.y);
        }
        if (Mathf.Sign(rb.velocity.y) < 0.0f && rb.velocity.y > terminalVel){
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*fastDescent);
        }
        sign = Mathf.Sign(Input.GetAxis("Horizontal"));
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal")*moveSpeed*quicksandMultiplier,0.0f);
        if (wantToJump)
            jumpWaitTime += Time.deltaTime;

        if (wantToJump && onGround && jumpWaitTime < maxJumpWait){
            // Debug.Log("Spacebar pushed");
            wantToJump = false;
            jumpWaitTime = 0.0f;
            movement = new Vector2(movement.x*quicksandMultiplier, jumpForce);
        }
        else if (wallJumpsDone < maxWallJumps && wantToJump && onWall && jumpWaitTime < maxJumpWait){
            wantToJump = false;
            wallJumpsDone += 1;
            jumpWaitTime = 0.0f;
            movement = new Vector2(wallSign*wallJumpForce, jumpForce/wallJumpsDone);
        }
        else if (!wantToJump && onGround){
            wallJumpsDone = 0;
        }

        if (jumpWaitTime >= maxJumpWait && wantToJump){
            jumpWaitTime = 0.0f;
            wantToJump = false;
        }
        // rb.position += movement;
        rb.AddForce(movement);
        if (onGround)
            playerJump = false;
        else
            playerJump = true;

        var vel = rb.velocity;
        float speed = vel.magnitude;
        //Debug.Log(speed);
        PlayerManager.instance.updateHealth(speed);
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
    } 

    public void quicksandTouch(float multipler)
    {
        quicksandMultiplier = multipler;
    }
}
