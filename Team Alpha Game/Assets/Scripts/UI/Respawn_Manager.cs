using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Audio;

public class Respawn_Manager : MonoBehaviour
{
    GameObject pc;
    Speed_Manager speedManager;
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;
    PlayerManager playerManager;
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
    public Tile inactive;
    public Tile active;
    public AudioClip activateCheckpoint;


    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("Player");
        speedManager = pc.GetComponent<Speed_Manager>();
        state = pc.GetComponent<Player_State>();
        animator = pc.GetComponent<Player_Animator>();
        timers = pc.GetComponent<Player_Timers>();
        playerManager = pc.GetComponent<PlayerManager>();
        attributes = state.attributes;
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(checkpoint);
        //if (other.gameObject.CompareTag("Player"))
        //{
        //    other.gameObject.GetComponent<Speed_Manager>().InstantChangeHealth(50);
        //}
        checkpoint = other.gameObject.transform.position;
        if (checkpoint_tiles.HasTile(new Vector3Int((int)checkpoint.x,Mathf.FloorToInt(checkpoint.y),0)) &&
            checkpoint_tiles.GetTile(new Vector3Int((int)checkpoint.x, Mathf.FloorToInt(checkpoint.y), 0)) == inactive &&
            other.gameObject.CompareTag("Player"))
        {
            checkpoint_tiles.SetTile(new Vector3Int((int)checkpoint.x,Mathf.FloorToInt(checkpoint.y),0),active);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Music/Checkpoint",this.transform.position);
            //AudioSource.PlayClipAtPoint(activateCheckpoint,checkpoint);
            other.gameObject.GetComponent<Speed_Manager>().InstantChangeHealth(50);
        }
        checkpoint.y += 0.1f;

    }

    public void Respawn()
    {
        state.death = false;
        animator.AnimatorDeath();
        rb.transform.position = checkpoint;
        rb.velocity = Vector3.zero;
        playerManager.currHealth = 1000;
        state.Reset_State();
        timers.immuneTimer = 3.0f;
    }
}
