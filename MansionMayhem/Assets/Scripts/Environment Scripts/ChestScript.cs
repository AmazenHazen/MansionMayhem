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
    private bool interactBool;


    public bool InteractBool
    {
        get { return interactBool; }
        set { interactBool = value; }
    }



    // Use this for initialization
    void Start()
    {        
        // set it as default that you aren't interacting with this door
        interactBool = false;

        alreadyOpened = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && alreadyOpened == true)
        {
            //Debug.Log("Turn off dialog");
            // if space and the text isn't scrolling, end dialog
            if (!GUIManager.isTyping && interactBool)
            {
                interactBool = false;
                GUIManager.TurnOffDialogBox();
            }
        }
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

                int screwIncrease = 0;

                // Spawn an item: Currency, heart, heart potion, or bonus
                int randomItemRoll = Random.Range(0, 100);

                // 30% chance of 75 screws
                if (randomItemRoll < 25)
                {
                    //Debug.Log("Chest Contained a 75 Screws");
                    screwIncrease = 75;
                }
                // 25% chance of 100 screws
                else if (randomItemRoll >= 25 && randomItemRoll < 50)
                {
                    //Debug.Log("Chest Contained a 100 Screws");
                    screwIncrease = 100;

                }
                // 20% chance of 125 screws
                else if (randomItemRoll >= 50 && randomItemRoll < 70)
                {
                    //Debug.Log("Chest Contained a 125 Screws");
                    screwIncrease = 125;

                }
                // 15% of 150
                else if (randomItemRoll >= 70 && randomItemRoll < 85)
                {
                    //Debug.Log("Chest Contained a 150 Screws");
                    screwIncrease = 150;

                }
                // 10% of 175
                else if (randomItemRoll >= 85 && randomItemRoll < 95)
                {
                    //Debug.Log("Chest Contained a 175 Screws");
                    screwIncrease = 175;

                }
                // 5% chance of 200
                else if (randomItemRoll >= 95 && randomItemRoll < 100)
                {
                    //Debug.Log("Chest Contained a 200 Screws");
                    screwIncrease = 200;
                }

                GameManager.instance.screws += screwIncrease;


                // switch the sprite to an open chest
                gameObject.GetComponent<SpriteRenderer>().sprite = OpenChestSprite;

                ChestMessage(screwIncrease);
            }
            else
            {
                //Debug.Log("Give Quest Item");
            }
        }
    }


    public void ChestMessage(int screwUp)
    {
        //Debug.Log("Chest Message");
        // Interacting with this chest
        interactBool = true;

        string chestString = "You receive " + screwUp + " screws from the chest!";
        GUIManager.TurnOnDialogBox();
        StartCoroutine(GUIManager.TextScroll(chestString));
    }



}
