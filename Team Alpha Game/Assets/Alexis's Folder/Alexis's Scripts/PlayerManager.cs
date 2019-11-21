using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FloatUnityEvent : UnityEvent<float> { };

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float currHealth = 1000f;

    public FloatUnityEvent playerHealth = new FloatUnityEvent();

    public GameObject respawner;

    public int lives = 3;

    private bool checkingSpeed = true;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

<<<<<<< HEAD
    // Update is called once per frame
    void Update()
    {
       
    }

=======
>>>>>>> Conflict_Branch
    public void updateHealth(float speed)
    {
        if (lives > 0 && checkingSpeed)
        {
            if (speed < 0.1)
            {
                if (currHealth >= 0)
                {
                    currHealth -= 10;
                    playerHealth.Invoke(currHealth);
                }
            }
            else if (speed < 2)
            {
                if (currHealth >= 0)
                {
                    currHealth -= 5;
                    playerHealth.Invoke(currHealth);
                }
            }
            else if (speed > 4)
            {
                if (currHealth <= 1000)
                {
                    currHealth += 5;
                    playerHealth.Invoke(currHealth);
                }
            }
            else if (speed > 10)
            {
                if (currHealth <= 1000)
                {
                    currHealth += 10;
                    playerHealth.Invoke(currHealth);
                }
            }

            if (currHealth <= 0)
            {
                respawner.GetComponent<Respawn_Manager>().Respawn();
                lives -= 1;
                currHealth = 1000;
            }
        }
        else if (lives <= 0)
        {
            ScreenManager.instance.GameLose();
        }

    }

    public void StopChecking()
    {
        checkingSpeed = false;
    }
}
