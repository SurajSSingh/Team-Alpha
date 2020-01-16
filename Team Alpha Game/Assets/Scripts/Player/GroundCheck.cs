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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            pc.isPlayerGrounded(true);
        }
        if (other.gameObject.CompareTag("Quicksand"))
        {
            pc.isPlayerGrounded(true);
            pc.isOnQuicksand(true);
        }
        if (other.gameObject.CompareTag("EnemyHead"))
        {
            pc.isStepping(true);
        }
        if (other.gameObject.CompareTag("Slope"))
        {
            pc.isPlayerGrounded(true);
            pc.isOnSlope(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            pc.isPlayerGrounded(true);
        }
        if (other.gameObject.CompareTag("Quicksand"))
        {
            pc.isPlayerGrounded(true);
            pc.isOnQuicksand(true);
        }
        if (other.gameObject.CompareTag("EnemyHead"))
        {
            pc.isStepping(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        pc.isPlayerGrounded(false);
        pc.isOnQuicksand(false);
        pc.isStepping(false);
        pc.isOnSlope(false);
    }
}
