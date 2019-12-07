using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject Player;
    private Player_Controller pc;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
        pc = Player.GetComponent<Player_Controller>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 12){
            pc.isPlayerGrounded(true);
        }
        if (other.gameObject.CompareTag("Quicksand")){
            pc.isOnQuicksand(true);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            pc.isStepping(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        pc.isPlayerGrounded(false);
        pc.isOnQuicksand(false);
    }
}
