using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusicShift : MonoBehaviour
{
    public bool changeDanger = false;
    bool activated = false;
    public LevelMusicScript lms;

    private void Start()
    {
        if(lms == null)
        {
            lms = this.gameObject.GetComponentInParent<LevelMusicScript>();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (changeDanger)
        {
            lms.SwapZones(true);
        }
        else if(!activated)
        {
            activated = true;
            lms.NextSafeSection();
            lms.SwapZones(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (changeDanger)
        {
            lms.SwapZones(false);
        }
    }
}
