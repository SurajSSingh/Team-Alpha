using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Audio;

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
    public Tilemap checkpoint_tiles;
    public Tile active;
    public AudioClip activateCheckpoint;


    // Start is called before the first frame update
    void Start()
    {
        playerJump = player.jumping;
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
        if (checkpoint_tiles.HasTile(new Vector3Int((int)checkpoint.x,Mathf.FloorToInt(checkpoint.y),0)))
        {
            checkpoint_tiles.SetTile(new Vector3Int((int)checkpoint.x,Mathf.FloorToInt(checkpoint.y),0),active);
            AudioSource.PlayClipAtPoint(activateCheckpoint,checkpoint);
        }
        checkpoint.y += 0.1f;

    }

    public void Respawn()
    {
        rb.transform.position = checkpoint;
        rb.velocity = Vector3.zero;
        timer = 0;
        player.ResetDash();
    }
}