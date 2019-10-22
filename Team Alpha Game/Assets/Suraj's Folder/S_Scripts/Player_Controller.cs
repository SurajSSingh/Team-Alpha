using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Fields to play around with
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float jumpForce = 40.0f;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float rbDrag = 0.0f;
    [SerializeField]
    private float rbMass = 0.1f;
    [SerializeField]
    private float rbGravity = 1.0f;

    private bool onGround = false;
    private bool onWall = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.drag = rbDrag;
        rb.mass = rbMass;
        rb.gravityScale = rbGravity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal")*moveSpeed,0.0f);
        if (Input.GetKeyDown(KeyCode.Space) && onGround){
            // Debug.Log("Spacebar pushed");
            movement = new Vector2(movement.x, jumpForce);
        }
        Debug.Log(movement);
        // rb.position += movement;
        rb.AddForce(movement);

    }

    // Used to change on ground value
    public void isPlayerGrounded(bool grounded)
    {
        onGround = grounded;
    }   
}
