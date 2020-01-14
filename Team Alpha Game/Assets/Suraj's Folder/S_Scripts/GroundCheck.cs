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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Player.GetComponent<Player_Controller>().isPlayerGrounded(true);
        }
        if (other.gameObject.CompareTag("Slope"))
        {
            pc.isPlayerGrounded(true);
            pc.isOnSlope(true);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground")){
            Player.GetComponent<Player_Controller>().isPlayerGrounded(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
<<<<<<< Updated upstream
        Player.GetComponent<Player_Controller>().isPlayerGrounded(false);
=======
        pc.isPlayerGrounded(false);
        pc.isOnQuicksand(false);
        pc.isStepping(false);
        pc.isOnSlope(false);
>>>>>>> Stashed changes
    }
}
