using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer ren;
    private bool attacked;
    public AudioClip deathSound;
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
            AudioSource.PlayClipAtPoint(deathSound,this.transform.position,2.0f);
            Destroy(gameObject);
        }
    }

    public void isAttacked(bool isAttacked)
    {
        attacked = isAttacked;
    }
}