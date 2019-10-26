using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatUnityEvent : UnityEvent<float> { };

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float currHealth = 100f;

    public FloatUnityEvent playerHealth = new FloatUnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateHealth(float speed)
    {
        if (speed < 1.0)
        {
            currHealth -= 10;
            playerHealth.Invoke(currHealth);
        }
        else if (speed < 5)
        {
            currHealth -= 5;
            playerHealth.Invoke(currHealth);
        }
        else if (speed > 8)
        {
            currHealth += 5;
            playerHealth.Invoke(currHealth);
        }
        else if (speed > 10)
        {
            currHealth += 10;
            playerHealth.Invoke(currHealth);
        }
    }
}
