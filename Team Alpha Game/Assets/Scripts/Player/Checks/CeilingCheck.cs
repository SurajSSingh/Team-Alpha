using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingCheck : MonoBehaviour
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
            pc.isPlayerCeilingTouch(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            pc.isPlayerCeilingTouch(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        pc.isPlayerCeilingTouch(false);
    }
}
