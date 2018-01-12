using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour {

    // Attributes
    public Sprite OpenChestSprite;    // Sprite for the chest after being opened
    public bool alreadyOpened;

    // If chest is used for a quest
    public bool questChest; // Put items in this chest if 
    public GameObject item;

    // Use this for initialization
    void Start()
    {
        alreadyOpened = false;
    }

    public void OpenChest()
    {
        // Check to see if the chest is open
        if (alreadyOpened == false)
        {
            if (questChest == false)
            {
                alreadyOpened = true;
                Debug.Log("You have opened PANDORAS BOXXX");

                // Spawn an item: Currency, heart, heart potion, or bonus
                int randomItemRoll = Random.Range(0, 100);

                // 30% chance of 10 screws
                if (randomItemRoll < 25)
                {
                    Debug.Log("Chest Contained a 10 Screws");
                }
                // 20% chance of 20 screws
                else if (randomItemRoll >= 25 && randomItemRoll < 50)
                {
                    Debug.Log("Chest Contained a 20 Screws");

                }
                // 15% chance of 40 screws
                else if (randomItemRoll >= 50 && randomItemRoll < 70)
                {
                    Debug.Log("Chest Contained a 40 Screws");
                }
                // 10% chance of high health object
                else if (randomItemRoll >= 70 && randomItemRoll < 90)
                {
                    Debug.Log("Chest Contained a 50 Screws");
                }
                // 5% of medicine
                else if (randomItemRoll >= 90 && randomItemRoll < 95)
                {
                    Debug.Log("Chest Contained a 75 Screws");
                }
                // 5% chance of ultimate health object
                else if (randomItemRoll >= 95 && randomItemRoll < 100)
                {
                    Debug.Log("Chest Contained a 100 Screws");
                }


                // switch the sprite to an open chest
                gameObject.GetComponent<SpriteRenderer>().sprite = OpenChestSprite;
            }

            else
            {
                Debug.Log("Give Quest Item");
            }

        }


    }
}
