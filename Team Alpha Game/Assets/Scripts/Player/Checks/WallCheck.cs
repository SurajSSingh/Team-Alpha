using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallCheck : MonoBehaviour
{
    private GameObject Player;
    Player_State state;
    public float oppositeDirection;
    public bool attacking;

    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
        state = Player.GetComponent<Player_State>();
        attacking = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall")){
            state.onWall = true;
            state.wallSign = oppositeDirection;
            state.Wall_State();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        state.onWall = false;
    }
}
