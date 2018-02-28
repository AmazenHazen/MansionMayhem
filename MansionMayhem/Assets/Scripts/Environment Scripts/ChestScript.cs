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
                //Debug.Log("You have opened PANDORAS BOXXX");

                // Spawn an item: Currency, heart, heart potion, or bonus
                int randomItemRoll = Random.Range(0, 100);

                // 30% chance of 75 screws
                if (randomItemRoll < 25)
                {
                    //Debug.Log("Chest Contained a 75 Screws");
                    GameManager.instance.screws += 75;
                }
                // 25% chance of 100 screws
                else if (randomItemRoll >= 25 && randomItemRoll < 50)
                {
                    //Debug.Log("Chest Contained a 100 Screws");
                    GameManager.instance.screws += 100;

                }
                // 20% chance of 125 screws
                else if (randomItemRoll >= 50 && randomItemRoll < 70)
                {
                    //Debug.Log("Chest Contained a 125 Screws");
                    GameManager.instance.screws += 125;

                }
                // 15% of 150
                else if (randomItemRoll >= 70 && randomItemRoll < 85)
                {
                    //Debug.Log("Chest Contained a 150 Screws");
                    GameManager.instance.screws += 150;

                }
                // 10% of 175
                else if (randomItemRoll >= 90 && randomItemRoll < 100)
                {
                    //Debug.Log("Chest Contained a 175 Screws");
                    GameManager.instance.screws += 175;

                }
                // 5% chance of 200
                else if (randomItemRoll >= 95 && randomItemRoll < 100)
                {
                    //Debug.Log("Chest Contained a 200 Screws");
                    GameManager.instance.screws += 200;
                }


                // switch the sprite to an open chest
                gameObject.GetComponent<SpriteRenderer>().sprite = OpenChestSprite;
            }

            else
            {
                //Debug.Log("Give Quest Item");
            }

        }


    }
}
