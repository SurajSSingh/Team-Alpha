using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallCheck : MonoBehaviour
{
    private GameObject Player;
    public float oppositeDirection;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall")){
            Player.GetComponent<Player_Controller>().isPlayerWallTouch(true,oppositeDirection);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Player.GetComponent<Player_Controller>().isPlayerWallTouch(false,0.0f);
    }
}
