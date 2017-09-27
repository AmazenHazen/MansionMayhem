using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    // Screw Attributes
    public GameObject normalScrew;
    public GameObject redScrew;
    public GameObject goldScrew;

    // Health Spawn Attributes
    public GameObject heart;
    public GameObject healthPotion;
    public GameObject fairyDust;
    public GameObject goldenHeart;

    // Bug breaking attribute
    public bool alreadyBroken;

    void Start()
    {
        // Start with the breakable not broken
        alreadyBroken = false;
    }

    public void SpawnInsides()
    {
        // Check to see if the breakable is broken
        if (alreadyBroken == false)
        {
            alreadyBroken = true;
            Debug.Log("Don't go breaking my HEART!");

            // Spawn an item: Currency, heart, heart potion, or bonus
            int randomItemRoll = Random.Range(0, 100);

            // 25% chance of red screw
            if (randomItemRoll < 25)
            {
                Debug.Log("Breakable Spawned a Red Screw");
                Instantiate(redScrew, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            // 25% chance of low health object
            else if (randomItemRoll >= 25 && randomItemRoll < 50)
            {
                Debug.Log("Breakable Spawned a Low Health Object");
                Instantiate(heart, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);

            }
            // 20% chance of gold Screw
            else if (randomItemRoll >= 50 && randomItemRoll < 70)
            {
                Debug.Log("Breakable Spawned a Gold Screw Object");
                Instantiate(goldScrew, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            // 20% chance of high health object
            else if (randomItemRoll >= 70 && randomItemRoll < 90)
            {
                Debug.Log("Breakable Spawned a High Health Object");
                Instantiate(healthPotion, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            // 5% chance of toolbox
            else if (randomItemRoll >= 90 && randomItemRoll < 95)
            {
                Debug.Log("Breakable Spawned a ToolBox");
                Instantiate(goldScrew, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            // 5% chance of ultimate health object
            else if (randomItemRoll >= 95 && randomItemRoll < 100)
            {
                Debug.Log("Breakable Spawned an Ultimate Health Object");
                Instantiate(heart, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
        }
    }
}
