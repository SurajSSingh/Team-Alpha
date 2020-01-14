using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class FloatUnityEvent : UnityEvent<float> { };

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float currHealth = 250f;

    public FloatUnityEvent playerHealth = new FloatUnityEvent();

    public GameObject respawner;

    public int lives = 3;

    public string dashCond;

    private bool checkingSpeed = true;

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
        if (lives > 0 && checkingSpeed)
        {
            if (speed < 0.1)
            {
                if (currHealth >= 0)
                {
                    if (currHealth <= 50)
                    {
                        currHealth -= 2;
                        playerHealth.Invoke(currHealth);
                    }
                    else if (currHealth <= 100)
                    {
                        currHealth -= 4;
                        playerHealth.Invoke(currHealth);
                    }
                    else
                    {
                        currHealth -= 5;
                        playerHealth.Invoke(currHealth);
                    }
                }
            }
            else if (speed < 2)
            {
                if (currHealth <= 50)
                {
                    currHealth -= 1;
                    playerHealth.Invoke(currHealth);
                }
                else if (currHealth <= 100)
                {
                    currHealth -= 2;
                    playerHealth.Invoke(currHealth);
                }
                else
                {
                    currHealth -= 4;
                    playerHealth.Invoke(currHealth);
                }
            }
            else if (speed > 8)
            {
                if (currHealth <= 200)
                {
                    currHealth += 2;
                    playerHealth.Invoke(currHealth);
                }
                else if (currHealth <= 1000)
                {
                    currHealth += 1;
                    playerHealth.Invoke(currHealth);
                }
            }
            else if (speed > 10)
            {
                if (currHealth <= 400)
                {
                    currHealth += 5;
                    playerHealth.Invoke(currHealth);
                }
                else if (currHealth <= 1000)
                {
                    currHealth += 2;
                    playerHealth.Invoke(currHealth);
                }
            }
            else if (speed > 12)
            {
                if (currHealth <= 600)
                {
                    currHealth += 10;
                    playerHealth.Invoke(currHealth);
                }
                else if (currHealth <= 1000)
                {
                    currHealth += 5;
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

    public void updateDash(string dash)
    {
        dashCond = dash;
    }

    public void StopChecking()
    {
        checkingSpeed = false;
    }
}