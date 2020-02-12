using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveCheck : MonoBehaviour
{
    private GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
        Enemy = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerAttack"))
        {
            Enemy.GetComponent<Enemy>().isAttacked(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerAttack"))
        {
            Enemy.GetComponent<Enemy>().isAttacked(true);
            Destroy(gameObject);
        }
    }
}
