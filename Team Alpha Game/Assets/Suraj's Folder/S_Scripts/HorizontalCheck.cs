using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HorizontalCheck : MonoBehaviour
{
    private GameObject Player;
    private Player_Controller pc;
    public float oppositeDirection;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
        pc = Player.GetComponent<Player_Controller>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall")){
            pc.isPlayerWallTouch(true, oppositeDirection);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            pc.isPlayerEnemyTouch(true, oppositeDirection);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            pc.isPlayerWallTouch(true, oppositeDirection);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            pc.isPlayerEnemyTouch(true, oppositeDirection);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        pc.isPlayerWallTouch(false,0.0f);
        pc.isPlayerEnemyTouch(false,0.0f);
    }
}
