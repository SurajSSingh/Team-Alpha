using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    private GameObject Enemy;
    GameObject player;
    GroundCheck groundCheck;
    WallCheck leftWallCheck;
    WallCheck rightWallCheck;
    void Start()
    {
        Enemy = gameObject.transform.parent.gameObject;
        player = GameObject.Find("Player");
        groundCheck = player.transform.GetChild(1).gameObject.GetComponent<GroundCheck>();
        leftWallCheck = player.transform.GetChild(2).gameObject.GetComponent<WallCheck>();
        rightWallCheck = player.transform.GetChild(3).gameObject.GetComponent<WallCheck>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerAttack") || other.gameObject.CompareTag("PlayerCP"))
        {
            FindDamageSource(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerAttack") || other.gameObject.CompareTag("PlayerCP"))
        {
            FindDamageSource(other);
        }
    }

    private void FindDamageSource(Collider2D other)
    {
        if (groundCheck.attacking || leftWallCheck.attacking || rightWallCheck.attacking)
        {
            Enemy.GetComponent<Enemy>().isAttacked(true);
            Destroy(gameObject);
        }
    }
}
