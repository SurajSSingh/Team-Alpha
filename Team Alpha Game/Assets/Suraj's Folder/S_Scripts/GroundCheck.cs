using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }
<<<<<<< HEAD

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Player.GetComponent<Player_Controller>().isPlayerGrounded(true);
        }
    }
=======
>>>>>>> master
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.layer == 12){
            Player.GetComponent<Player_Controller>().isPlayerGrounded(true);
        }
        if (other.gameObject.CompareTag("Quicksand")){
<<<<<<< HEAD
<<<<<<< HEAD
            Player.GetComponent<Player_Controller>().isOnQuicksand(true);
=======
            // Player.GetComponent<Player_Controller>().isOnQuicksand(true);
>>>>>>> master
=======
            Player.GetComponent<Player_Controller>().isOnQuicksand(true);
>>>>>>> master
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Player.GetComponent<Player_Controller>().isPlayerGrounded(false);
<<<<<<< HEAD
<<<<<<< HEAD
        Player.GetComponent<Player_Controller>().isOnQuicksand(false);
=======
        // Player.GetComponent<Player_Controller>().isOnQuicksand(false);
>>>>>>> master
=======
        Player.GetComponent<Player_Controller>().isOnQuicksand(false);
>>>>>>> master
    }
}
