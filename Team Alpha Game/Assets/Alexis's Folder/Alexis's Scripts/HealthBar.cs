using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    GameObject playerManagerComponent;
    Image healthBarComponent;
    float maxHealth = 100f;


    // Start is called before the first frame update
    void Start()
    {
        healthBarComponent = GetComponent<Image>();

        playerManagerComponent = GameObject.Find("PlayerManager");

        playerManagerComponent.GetComponent<PlayerManager>().playerHealth.AddListener(currHealthListener);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void currHealthListener(float health)
    {
        healthBarComponent.fillAmount = health / maxHealth;
    }
}
