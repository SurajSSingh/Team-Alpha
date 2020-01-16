using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBehaviour : MonoBehaviour
{
    private Vector2 leftBounds;
    private Vector2 rightBounds;
    [SerializeField]
    private Vector2 spawnPoint;
    [SerializeField]
    private Rigidbody2D rb;
    private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = spawnPoint;
        leftBounds = new Vector2(spawnPoint.x - 5.0f, spawnPoint.y);
        rightBounds = new Vector2(spawnPoint.x + 3.0f, spawnPoint.y);
        moveSpeed = 4.0f;
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x >= 0.0f)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        if (transform.position.x <= leftBounds.x)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else if (transform.position.x >= rightBounds.x)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }
}
