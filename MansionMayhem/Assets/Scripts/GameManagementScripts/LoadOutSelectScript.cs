using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOutSelectScript : MonoBehaviour
{
    // To be global and saved attributes
    int totalEquipementNum;

    // Attributes
    Color selected = new Color(33, 88, 55);

    // Weapon Variables
    // Holds a reference to all the buttons in the weapons menu
    public List<GameObject> oneButtons;
    public List<GameObject> twoButtons;
    public List<GameObject> threeButtons;

    // Trinket Variables
    public List<GameObject> trinketList;

    // Equipment Variables
    int equipedNum;
    public List<GameObject> equipmentOnButtons;
    public List<GameObject> equipmentOffButtons;


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Manages what buttons are/can be pressed:
    /// This includes equipement, trinkets, and weapons used
    /// </summary>
    void buttonManagement()
    {

    }

    public void armorSelect(GameObject buttonChosen)
    {
        if (equipedNum >= totalEquipementNum)
        {
            // Send message to player that too many numbers who have been selected
        }
        else
        {
            // Turn on the button
            buttonChosen.GetComponent<Image>().color = selected;
            // Set button Bool to true

            // Increase the number of equiped items
            equipedNum++;
        }
        
    }

    /// <summary>
    /// Turns on the button and shows what weapon is selected.
    /// </summary>
    /// <param name="weaponSlot"></param>
    /// <param name="buttonChosen"></param>
    public void weaponSelect(int weaponSlot, GameObject buttonChosen)
    {
        // Turn on the button
        buttonChosen.GetComponent<Image>().color = selected;
        // Set button Bool to true

        // turn off other weapon buttons
        switch(weaponSlot)
        {
            // Cases depending on the weaponSlot selected
            case 1:
                for(int i=0; i<oneButtons.Count; i++)
                {
                    // find the other buttons selected and turn them off
                    if(buttonChosen == oneButtons[i] /*&& The button is turned on*/)
                    {
                        buttonChosen.GetComponent<Image>().color = Color.white;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < twoButtons.Count; i++)
                {
                    // find the other buttons selected and turn them off
                    if (buttonChosen == twoButtons[i] /*&& The button is turned on*/)
                    {
                        buttonChosen.GetComponent<Image>().color = Color.white;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < threeButtons.Count; i++)
                {
                    // find the other buttons selected and turn them off
                    if (buttonChosen == threeButtons[i] /*&& The button is turned on*/)
                    {
                        buttonChosen.GetComponent<Image>().color = Color.white;
                    }
                }
                break;
        }
    }
}
