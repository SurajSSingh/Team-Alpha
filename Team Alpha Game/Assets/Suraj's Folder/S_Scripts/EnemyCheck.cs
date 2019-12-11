using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            pc.isPlayerEnemyTouch(true, oppositeDirection);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            pc.isPlayerEnemyTouch(true, oppositeDirection);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        pc.isPlayerEnemyTouch(false, 0.0f);
    }
}
