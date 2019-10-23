using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_End : MonoBehaviour
{

    public GameObject respawner;
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        respawner.GetComponent<Respawn_Manager>().Respawn();
    }
}
