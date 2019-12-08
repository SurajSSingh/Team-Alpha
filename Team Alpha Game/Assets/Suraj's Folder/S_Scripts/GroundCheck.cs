using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject Player;
<<<<<<< HEAD
    private float lessTimer;
    private float countdown;
=======
    private Player_Controller pc;
>>>>>>> master
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
        pc = Player.GetComponent<Player_Controller>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 12){
<<<<<<< HEAD
            Player.GetComponent<Player_Controller>().isPlayerGrounded(true);
=======
            pc.isPlayerGrounded(true);
        }
        if (other.gameObject.CompareTag("Quicksand")){
            pc.isOnQuicksand(true);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            pc.isStepping(true);
>>>>>>> master
        }
        if (other.gameObject.CompareTag("Quicksand")){
            Player.GetComponent<Player_Controller>().isOnQuicksand(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
<<<<<<< HEAD
        Player.GetComponent<Player_Controller>().isPlayerGrounded(false);
        Player.GetComponent<Player_Controller>().isOnQuicksand(false);
=======
        pc.isPlayerGrounded(false);
        pc.isOnQuicksand(false);
>>>>>>> master
    }

    
}
