using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public Camera camera;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            vcam.m_Priority += 2;
            StartCoroutine("ZoomOut");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            vcam.m_Priority -= 2;
            StartCoroutine("ZoomIn");
        }
    }

    IEnumerator ZoomOut()
    {
        for(int i=0; i < 30; i++)
        {
            camera.orthographicSize += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    IEnumerator ZoomIn()
    {
        for (int i = 0; i < 30; i++)
        {
            camera.orthographicSize -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

    }
}
