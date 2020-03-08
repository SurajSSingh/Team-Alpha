using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicBoxScript : MonoBehaviour
{
    public List<string> keepScenes;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!keepScenes.Contains(SceneManager.GetActiveScene().name))
        {
            Debug.Log("Destroy");
            Destroy(this.gameObject);
        }
    }
}
