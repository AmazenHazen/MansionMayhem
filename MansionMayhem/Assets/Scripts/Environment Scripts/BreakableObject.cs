using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
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
            //Debug.Log("Don't go breaking my HEART!");

            // Spawn an item: Currency, heart, heart potion, or bonus
            int randomItemRoll = Random.Range(0, 100);

            // 25% chance of red screw
            if (randomItemRoll < 35)
            {
                //Debug.Log("Breakable Spawned a Red Screw");
                Instantiate(GameManager.screwItems[1], new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            // 25% chance of low health object
            else if (randomItemRoll >= 35 && randomItemRoll < 60)
            {
                //Debug.Log("Breakable Spawned a Low Health Object");
                Instantiate(GameManager.recoveryItems[0], new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);

            }
            // 20% chance of gold Screw
            else if (randomItemRoll >= 60 && randomItemRoll < 80)
            {
                //Debug.Log("Breakable Spawned a Gold Screw Object");
                Instantiate(GameManager.screwItems[2], new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            // 20% chance of high health object
            else if (randomItemRoll >= 80 && randomItemRoll < 90)
            {
                //Debug.Log("Breakable Spawned a High Health Object");
                Instantiate(GameManager.recoveryItems[1], new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            // 5% chance of toolbox
            else if (randomItemRoll >= 90 && randomItemRoll < 95)
            {
                //Debug.Log("Breakable Spawned a ToolBox");
                Instantiate(GameManager.screwItems[2], new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            // 5% chance of ultimate health object
            else if (randomItemRoll >= 95 && randomItemRoll < 100)
            {
                //Debug.Log("Breakable Spawned an Ultimate Health Object");
                Instantiate(GameManager.recoveryItems[3], new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
        }
    }
}
