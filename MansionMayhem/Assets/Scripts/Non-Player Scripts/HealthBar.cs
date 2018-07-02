using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Values needed for health bar
    private float maxHealth;
    private float currentHealth;
    private GameObject healthBarCanvas;
    private Slider healthBar;
    private Owner ownerType;

    void Start()
    {
        healthBarCanvas = transform.Find("HealthBarCanvas").gameObject;
        healthBar = healthBarCanvas.transform.GetChild(0).GetComponent<Slider>();
    }

	// Use this for initialization
	public void HealthBarInstantiate()
    {
        if (GetComponent<EnemyManager>())
        {
            ownerType = Owner.Enemy;

            // Set the healthBar Max value
            maxHealth = GetComponent<EnemyManager>().maxHealth;
        }
        else if (GetComponent<AllyManager>())
        {
            ownerType = Owner.Ally;

            // Set the healthBar Max value
            maxHealth = GetComponent<AllyManager>().maxHealth;
        }
        else
        {
            ownerType = Owner.None;
        }
        // Set the healthBar Max value
        //Debug.Log("HealthBar" + healthBar + "maxhealth" + maxHealth);
        healthBar.maxValue = maxHealth;

        // Turn off health bar canvas at the beginning
        //healthBarCanvas.SetActive(false);

    }
	
	// Update is called once per frame
	void Update()
    {
        // Get current health and update the bar
        if (ownerType == Owner.Enemy)
        {
            currentHealth = GetComponent<EnemyManager>().CurrentHealth;
        }
        if (ownerType == Owner.Ally)
        {
            currentHealth = GetComponent<AllyManager>().CurrentHealth;
        }

        healthBar.value = currentHealth;
        // Turn on the HealthBar only if you damage the enemy
        if(maxHealth!=currentHealth)
        {
            healthBarCanvas.SetActive(true);
        }
	}
}
