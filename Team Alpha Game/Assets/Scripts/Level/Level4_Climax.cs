using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4_Climax : MonoBehaviour
{
    public GameObject trigger;
    public GameObject endObject;
    public MovingMist mover;
    // Start is called before the first frame update
    void Start()
    {
        trigger.SetActive(false);
        endObject.SetActive(true);
        mover.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/MusicSingle/Speed", this.transform.position);
            trigger.SetActive(true);
            endObject.SetActive(false);
            mover.gameObject.SetActive(true);
            mover.StartFog();
        }
    }
}
