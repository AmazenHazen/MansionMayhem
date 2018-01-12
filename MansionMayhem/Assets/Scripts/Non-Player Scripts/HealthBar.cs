using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Values needed for health bar
    public float maxHealth;
    public float currentHealth;
    public GameObject healthBarCanvas;
    public Slider healthBar;

	// Use this for initialization
	public void HealthBarInstantiate ()
    {
        // Turn off health bar canvas at the beginning
        healthBarCanvas.SetActive(false);

        // Set the healthBar Max value
        maxHealth = gameObject.GetComponent<EnemyManager>().maxHealth;
        healthBar.maxValue = maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {

        // Get current health and update the bar
        currentHealth = gameObject.GetComponent<EnemyManager>().CurrentLife;
        healthBar.value = currentHealth;

        // Turn on the HealthBar only if you damage the enemy
        if(maxHealth!=currentHealth)
        {
            healthBarCanvas.SetActive(true);
        }
	}
}
