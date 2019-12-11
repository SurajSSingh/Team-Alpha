using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Manager : MonoBehaviour
{
    [SerializeField] 
    private Player_Controller player;
    [SerializeField] 
    private Rigidbody2D rb;
    [SerializeField] 
    private bool playerJump;
    [SerializeField] 
    private bool inSession;
    [SerializeField] 
    private float timer = 0.0f;

    private Vector3 checkpoint = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        playerJump = player.playerJump;
        inSession = player.inSession;
    }

    // Update is called once per frame
    void Update()
    {
        if (inSession == true)
        {
            if (playerJump == true)
            {
                timer += Time.deltaTime;
                if (timer >= 4)
                    Respawn();
            }
        }
        else if (rb.velocity.y <= -30)
        {
            Respawn();
        }
        else
        {
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(checkpoint);
        checkpoint = other.gameObject.transform.position;
        checkpoint.y += 2;
    }

    public void Respawn()
    {
        rb.transform.position = checkpoint;
        rb.velocity = Vector3.zero;
        timer = 0;
        player.ResetDash();
    }
}