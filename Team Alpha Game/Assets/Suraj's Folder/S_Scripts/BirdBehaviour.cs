using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    private float leftBounds;
    private float rightBounds;
    [SerializeField]
    private Vector2 spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = spawnPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
