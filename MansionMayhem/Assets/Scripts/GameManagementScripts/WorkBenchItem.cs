using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkBenchItem : MonoBehaviour
{
    // Constants for health and equipement
    const int MAX_HEALTH = 30;
    const int MAX_EQUIPMENT = 3;

    // Cost and text
    [SerializeField]
    private int cost;     // Cost of the item to unlock
    public Text costText; // "Click to unlock" or "Unlocked"

    // To hold other button variables to check to make the buttons interactable or not
    public GameObject gunUnlock;        // If an upgraded gun it points to the gun needed to unlock
    public List<GameObject> upgradeVars;// If unlocking a gun it points to the 3 unlockable upgrades

    // Var to see if it is true
    public bool unlockedBool;           // Checks to see if the gun is unlocked or all upgrades have been bought
    private bool lockedForDemo;         // Specific temporary bool for if the item can be unlocked for the demo

    public int unlockNumber;

    [SerializeField]
    private Unlock unlockVar;           // Variable that is unlocked

    Color boughtColor = new Color(.55f, .85f, .245f);   // color that the button changes to once unlocked


    public void Start()
    {
        lockedForDemo = false;

        // Set up the onClick Event for every button
        GetComponent<Button>().onClick.AddListener(PurchaseItem);

        #region Switch setting buttons to unlocked or locked depending on the save variable
        // Unlock the variable
        switch (unlockVar)
        {
            case Unlock.heartIncrease:
                if(GameManager.instance.healthTotal>MAX_HEALTH)
                { unlockedBool = false;}
                break;

            case Unlock.equipmentIncrease:
                if (GameManager.instance.healthTotal>MAX_EQUIPMENT)
                { unlockedBool = false; }
                lockedForDemo = true;
                break;

            // Gun Unlocks && trinkets are default case
            default:
                if (GameManager.instance.unlockableBuyables[unlockNumber])
                {
                    unlockedBool = true; 
                }
                else
                {
                    unlockedBool= false; 
                }
                break;

        }
        #endregion

        // Set the Cost text
        costText.text = "Cost: " + cost + " screws";

        #region Setting Dynamically Changing cost variables
        //Health
        
        // Set the cost for dynamic costing unlocks
        // Check to see if the button is for heart increase and if it is below the max amount of health a player can have
        if (unlockVar == Unlock.heartIncrease && GameManager.instance.healthTotal < MAX_HEALTH)
        {
            // Set the cost dynamically
            cost = (GameManager.instance.healthTotal - 3) * 200;

            // Set the Cost text
            costText.text = "Cost: " + cost + " screws";
        }
        /*else if (unlockVar == Unlock.equipmentIncrease && (GameManager.instance.equipmentTotal < MAX_EQUIPMENT))
        {
            cost = GameManager.instance.equipmentTotal * 500;
        }*/
        else if ((unlockVar == Unlock.heartIncrease && GameManager.instance.healthTotal >= MAX_HEALTH))
        {
            Debug.Log("Health locked cause health is" + GameManager.instance.healthTotal + "/" + MAX_HEALTH);
            //Debug.Log("Equipement locked cause is" + GameManager.instance.equipmentTotal + "/" + MAX_EQUIPMENT);
            costText.text = "Purchased";
        }

        #endregion



        //Debug.Log(gameObject + "Start");

        // check if the gun unlock is unlocked before allowing purchasing of the upgrades
        // This checks to see if the gun has been unlocked associated with the upgrades
        // This won't work due to start of the other purchaseable items also running at the same exact time**************** NEED TO FIND A SOLUTION FOR THIS
        if (upgradeVars.Count>0 && unlockedBool == true)
        {
            for (int i = 0; i < upgradeVars.Count; i++)
            {
                upgradeVars[i].GetComponent<Button>().interactable = true;
            }
        }

        // Set the button as unlocked if the unlockedBool is true
        if (unlockedBool)
        {
            costText.text = "Purchased";

            // Set the color to show it is bought
            gameObject.GetComponent<Image>().color = boughtColor;
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    private void Update()
    {
        if ((unlockedBool||lockedForDemo) && gameObject.GetComponent<Button>().interactable)
        {
            // Set the color to show it is bought
            if (!lockedForDemo)
            {
                gameObject.GetComponent<Image>().color = boughtColor;
            }
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public int Cost
    {
        get { return cost; }
        set { cost = value; }

    }

    public Unlock UnlockVar
    {
        get { return unlockVar; }
    }

    public void PurchaseItem()
    {
        Debug.Log("You purchased an item!");
        Unlock unlockItem = unlockVar;

        if (GameManager.instance.screws < cost)
        {
            //Debug.Log("You do not have enough to build this item");
            return;
        }
        else
        {
            // Subtract the amount from the player's screws
            GameManager.instance.screws -= cost;

            #region Switch for unlocking variables
            // Unlock the variable
            switch (unlockItem)
            {
                case Unlock.heartIncrease:
                    GameManager.instance.healthTotal++;

                    // Increase max health
                    break;
                    /*
                case Unlock.equipmentIncrease:
                    GameManager.instance.equipmentTotal++;

                    // Increase equipment
                    break;
                    */
                // Gun Unlocks & trinket unlocks
                default:
                    GameManager.instance.unlockableBuyables[unlockNumber] = true;
                    break;
            }
            #endregion

            #region Unlock Upgrade Varaibles
            if (upgradeVars.Count > 0)
            {
                for (int i = 0; i < upgradeVars.Count; i++)
                {
                    upgradeVars[i].GetComponent<Button>().interactable = true;
                }
            }

            #endregion

            #region Buttons reacting to purchased upgrade
            // IF the unlock is a heart increase and healthtotal<maxhealth or the same with equipement, then keep the button interactable
            if (unlockItem == Unlock.heartIncrease && (GameManager.instance.healthTotal < MAX_HEALTH))
            {
                // Set a temporary cost variable
                int tempCost = (GameManager.instance.healthTotal - 3) * 200;

                cost = tempCost;
                costText.text = "Cost: " + tempCost + " screws";
            }
            /*
            else if (unlockItem == Unlock.equipmentIncrease && (GameManager.instance.equipmentTotal < MAX_EQUIPMENT))
            {
                // Set a temporary cost variable
                int tempCost = GameManager.instance.equipmentTotal * 500;

                // Set the button cost and text to reflect it
                Cost = tempCost;
                costText.text = "Cost: " + tempCost + " screws";

                // Set the Cost text
                costText.text = "Cost: " + tempCost + " screws";
            }
            */
            else
            {
                // Set the color to show it is bought
                GetComponent<Image>().color = boughtColor;

                // Make the button uninteractable
                GetComponent<Button>().interactable = false;
                costText.text = "Purchased";

                // Set the unlockedBool to true
                unlockedBool = true;
            }
            #endregion


        }

    }
}
