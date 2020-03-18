using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    private GameObject Player;
    Player_State state;
    public float oppositeDirection;
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
        state = Player.GetComponent<Player_State>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            state.enemyColSign = oppositeDirection;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            state.enemyColSign = oppositeDirection;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        state.enemyColSign = 0.0f;
    }
}
