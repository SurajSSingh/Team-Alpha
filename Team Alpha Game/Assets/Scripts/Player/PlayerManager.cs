using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class FloatUnityEvent : UnityEvent<float> { };

public class PlayerManager : MonoBehaviour
{
    Speed_Manager speedManager;
    Player_State state;
    Player_Animator animator;
    Player_Timers timers;
    Player_Attributes attributes;

    GameObject player;

    //Timer Values
    private float deathTime;

    public static PlayerManager instance;

    public float currHealth = 250f;

    public FloatUnityEvent playerHealth = new FloatUnityEvent();

    public GameObject respawner;

    public int lives = 1;

    private bool checkingSpeed = true;

    public LevelMusicScript levelMusic;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        speedManager = player.GetComponent<Speed_Manager>();
        state = player.GetComponent<Player_State>();
        animator = player.GetComponent<Player_Animator>();
        timers = player.GetComponent<Player_Timers>();
        attributes = state.attributes;
        deathTime = attributes.deathTime;
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Update()
    {
        levelMusic.UpdateHealth(currHealth);
    }

    public void updateHealth(float speed, bool inMist)
    {
        if (lives > 0 && checkingSpeed)
        {
            if (inMist)
            {
                currHealth -= 5;
                levelMusic.UpdateMist(true);
            }
            else
            {
                levelMusic.UpdateMist(false);
            }
            if (speed < 0.1)
            {
                if (currHealth >= 0)
                {
                    if (currHealth <= 50){
                        currHealth -= 2;
                        playerHealth.Invoke(currHealth);
                    }
                    else if (currHealth <= 100){
                            currHealth -= 4;
                            playerHealth.Invoke(currHealth);
                        }
                    else{
                        currHealth -= 5;
                        playerHealth.Invoke(currHealth);
                    }
                }
            }
            else if (speed < 2)
            {
                if (currHealth <= 50){
                        currHealth -= 1;
                        playerHealth.Invoke(currHealth);
                    }
                else if (currHealth <= 100){
                        currHealth -= 2;
                        playerHealth.Invoke(currHealth);
                    }
                else{
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
                } else if (currHealth <= 1000)
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
                } else if (currHealth <= 1000)
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
                } else if (currHealth <= 1000)
                {
                    currHealth += 5;
                    playerHealth.Invoke(currHealth);
                }
            }

            if (currHealth <= 0)
            {
                Death();
            }
            if (state.death)
            {
                if (timers.deathTimer <= 0.0f)
                {
                    respawner.GetComponent<Respawn_Manager>().Respawn();
                }
            }
        }
        else if (lives <= 0)
        {
            currHealth = 0;
            ScreenManager.instance.GameLose();
        }

    }

    public void ChangeHealth(float value)
    {
        currHealth += value;
    }

    public void StopChecking()
    {
        checkingSpeed = false;
    }

    private void Death()
    {
        state.death = true;
        state.control = false;
        timers.deathTimer = deathTime;
        animator.AnimatorDeath();
        lives -= 1;
    }
}
