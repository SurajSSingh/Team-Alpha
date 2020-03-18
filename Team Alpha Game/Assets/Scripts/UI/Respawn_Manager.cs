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
        
        Vector3Int position = checkpoint_tiles.GetComponentInParent<Grid>().WorldToCell(checkpoint);
        Debug.Log(position);
        if (other.gameObject.CompareTag("Player"))
        {
            if (checkpoint_tiles.GetTile(position) == inactive)
            {
                checkpoint_tiles.SetTile(position, active);
                FMODUnity.RuntimeManager.PlayOneShot("event:/MusicSingle/Checkpoint", this.transform.position);
                other.gameObject.GetComponent<Speed_Manager>().InstantChangeHealth(50);
            }
            else if (checkpoint_tiles.GetTile(position + new Vector3Int(-1, 0, 0)) == inactive)
            {
                checkpoint_tiles.SetTile(position + new Vector3Int(-1, 0, 0), active);
                FMODUnity.RuntimeManager.PlayOneShot("event:/MusicSingle/Checkpoint", this.transform.position);
                other.gameObject.GetComponent<Speed_Manager>().InstantChangeHealth(50);
            }
            else if (checkpoint_tiles.GetTile(position + new Vector3Int(1, 0, 0)) == inactive)
            {
                checkpoint_tiles.SetTile(position + new Vector3Int(1, 0, 0), active);
                FMODUnity.RuntimeManager.PlayOneShot("event:/MusicSingle/Checkpoint", this.transform.position);
                other.gameObject.GetComponent<Speed_Manager>().InstantChangeHealth(50);
            }


        }
        

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
