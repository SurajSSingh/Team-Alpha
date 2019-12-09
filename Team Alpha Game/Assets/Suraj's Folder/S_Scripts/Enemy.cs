using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer ren;
    private bool attacked;
    // Start is called before the first frame update
    void Start()
    {
        ren.enabled = true;
        attacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ren.enabled && attacked)
        {
            Destroy(gameObject);
        }
    }

    public void isAttacked(bool isAttacked)
    {
        attacked = isAttacked;
    }
}