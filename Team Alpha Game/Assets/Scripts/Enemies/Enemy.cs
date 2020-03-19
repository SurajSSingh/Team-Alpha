using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer ren;
    private bool attacked;
    public AudioClip deathSound;
    public GameObject player;
    GroundCheck groundCheck;
    WallCheck leftWallCheck;
    WallCheck rightWallCheck;
    // Start is called before the first frame update
    void Start()
    {
        ren = this.GetComponent<SpriteRenderer>();
        ren.enabled = true;
        attacked = false;
        groundCheck = player.transform.GetChild(1).gameObject.GetComponent<GroundCheck>();
        leftWallCheck = player.transform.GetChild(2).gameObject.GetComponent<WallCheck>();
        rightWallCheck = player.transform.GetChild(3).gameObject.GetComponent<WallCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ren.enabled && attacked)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/MusicSingle/Checkpoint", this.transform.position);
            Destroy(gameObject);
        }
    }

    public void isAttacked(bool isAttacked)
    {
        attacked = isAttacked;
    }
}