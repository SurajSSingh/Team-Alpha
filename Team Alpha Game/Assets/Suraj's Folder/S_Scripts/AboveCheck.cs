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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            Enemy.GetComponent<Enemy>().isAttacked(true);
            Destroy(gameObject);
        }
    }
}
