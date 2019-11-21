using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Fields to play around with
    [SerializeField]
    private float moveSpeed = 10.0f;
    [SerializeField]
    private float jumpForce = 250.0f;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float rbDrag = 0.5f;
    [SerializeField]
    private float rbMass = 0.5f;
    [SerializeField]
    private float rbGravity = 1.0f;

    private bool onGround = false;
    
    private bool onWall = false;

    [SerializeField]
    private float sign = 0.0f;

    public bool inSession = false;
    public bool playerJump = false;

    public float fastDescent = 1.2f;
    public float terminalVel = -5.0f;

    public float wallSign = 0.0f;
    public float wallJumpForce = 250.0f;

    private bool wantToJump = false;
    private bool onQuicksand = false;

    private int wallJumpCount = 0;
    private int wallJumpMax = 3;

    // Start is called before the first frame update
    void Start()
    {
        inSession = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = rbDrag;
        rb.mass = rbMass;
        rb.gravityScale = rbGravity;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            wantToJump = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Mathf.Sign(Input.GetAxis("Horizontal")) != sign) {
            rb.velocity = new Vector2(rb.velocity.x/2,rb.velocity.y);
        }
        if (Mathf.Sign(rb.velocity.y) < 0.0f && rb.velocity.y > terminalVel){
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*fastDescent);
        }
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

    public void isOnQuicksand(bool touchSand)
    {
        onQuicksand = touchSand;
    } 


}
