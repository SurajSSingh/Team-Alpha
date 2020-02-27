using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    GameObject playerManagerComponent;
    Image healthBarComponent;
    float maxHealth = 1000f;
    float currenthealth;


    // Start is called before the first frame update
    void Start()
    {
        healthBarComponent = GetComponent<Image>();

        playerManagerComponent = GameObject.Find("PlayerManager");

        currenthealth = playerManagerComponent.GetComponent<PlayerManager>().currHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currenthealth = playerManagerComponent.GetComponent<PlayerManager>().currHealth;
        healthBarComponent.fillAmount = currenthealth / maxHealth;
    }

    void currHealthListener(float health)
    {
        healthBarComponent.fillAmount = health / maxHealth;
    }
}
