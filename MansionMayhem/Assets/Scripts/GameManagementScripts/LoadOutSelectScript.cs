using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOutSelectScript : MonoBehaviour
{
    // To be global and saved attributes
    int totalEquipementNum;

    // Attributes
    Color selectedColor = new Color(55, 85, 245);

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
    /// Handles if you equip armor
    /// </summary>
    /// <param name="buttonChosen"></param>
    public void EquipArmor(GameObject buttonChosen)
    {
        // check to see if you already have too much equipped or already equipped
        if (equipedNum >= totalEquipementNum/* && already equipped*/)
        {
            // Send message to player that too many numbers who have been selected
        }
        else
        {
            // Turn on the button
            buttonChosen.GetComponent<Image>().color = selectedColor;

            // Set button Bool to true
            buttonChosen.GetComponent<LoadOutButtonScript>().selected = true;

            // Equip the item


            // Increase the number of equiped items
            equipedNum++;
        }
    }


    /// <summary>
    /// Handles if you equip armor
    /// </summary>
    /// <param name="buttonChosen"></param>
    public void UnequipArmor(GameObject buttonChosen)
    {

        // Turn off the button
        buttonChosen.GetComponent<Image>().color = Color.white;

        // Set button Bool to false
        buttonChosen.GetComponent<LoadOutButtonScript>().selected = false;

        // Unequip the armor


        // Increase the number of equiped items
        equipedNum--;
    }



    /// <summary>
    /// Turns on the button and shows what weapon is selected.
    /// </summary>
    /// <param name="weaponSlot"></param>
    /// <param name="buttonChosen"></param>
    public void weaponSelect(GameObject buttonChosen)
    {
        int weaponSlot = buttonChosen.GetComponent<LoadOutButtonScript>().weaponSlot;

        // Turn off the button
        buttonChosen.GetComponent<Button>().interactable = false;
        // Set button Bool to true
        buttonChosen.GetComponent<LoadOutButtonScript>().selected = true;


        // Put the gun selection on the GameManager
        GameManager.instance.currentGuns[weaponSlot] = buttonChosen.GetComponent<LoadOutButtonScript>().buttonWeapon;

        buttonChosen.GetComponent<Image>().color = selectedColor;


        // Turn off multiple instances of the same gun
        // This logic is correct
        for(int i =0; i<GameManager.instance.currentGuns.Count; i++)
        {
            if((GameManager.instance.currentGuns[i] == buttonChosen.GetComponent<LoadOutButtonScript>().buttonWeapon) && (weaponSlot!= (i)))
            {
                //Debug.Log("Turned off " + GameManager.instance.currentGuns[i] + " at weapon slot" + i);
                GameManager.instance.currentGuns[i] = rangeWeapon.None;

                // turn off the other buttons as well
                switch (weaponSlot)
                {
                    case 0:
                        for (int j = 0; j < oneButtons.Count; j++)
                        {
                            if (oneButtons[j].GetComponent<LoadOutButtonScript>().buttonWeapon == GameManager.instance.currentGuns[weaponSlot])
                            {
                                twoButtons[j].GetComponent<Button>().interactable = true;
                                twoButtons[j].GetComponent<LoadOutButtonScript>().selected = false;
                                threeButtons[j].GetComponent<Button>().interactable = true;
                                threeButtons[j].GetComponent<LoadOutButtonScript>().selected = false;
                            }
                        }
                        break;
                    case 1:
                        for (int j = 0; j < oneButtons.Count; j++)
                        {
                            if (oneButtons[j].GetComponent<LoadOutButtonScript>().buttonWeapon == GameManager.instance.currentGuns[weaponSlot])
                            {
                                oneButtons[j].GetComponent<Button>().interactable = true;
                                oneButtons[j].GetComponent<LoadOutButtonScript>().selected = false;
                                threeButtons[j].GetComponent<Button>().interactable = true;
                                threeButtons[j].GetComponent<LoadOutButtonScript>().selected = false;
                            }
                        }
                        break;
                    case 2:
                        for (int j = 0; j < oneButtons.Count; j++)
                        {
                            if (oneButtons[j].GetComponent<LoadOutButtonScript>().buttonWeapon == GameManager.instance.currentGuns[weaponSlot])
                            {
                                twoButtons[j].GetComponent<Button>().interactable = true;
                                twoButtons[j].GetComponent<LoadOutButtonScript>().selected = false;
                                oneButtons[j].GetComponent<Button>().interactable = true;
                                oneButtons[j].GetComponent<LoadOutButtonScript>().selected = false;
                            }
                        }
                        break;
                }
            }
        }

        // turn off other weapon buttons
        switch(weaponSlot)
        {
            // Cases depending on the weaponSlot selected
            case 0:
                for(int i=0; i<oneButtons.Count; i++)
                {
                    // find the other buttons selected and turn them on
                    if (oneButtons[i].GetComponent<LoadOutButtonScript>().buttonWeapon != GameManager.instance.currentGuns[weaponSlot])
                    {
                        Debug.Log("Reset Button:" + oneButtons[i].GetComponent<LoadOutButtonScript>().buttonWeapon);
                        oneButtons[i].GetComponent<Button>().interactable = true;
                        oneButtons[i].GetComponent<LoadOutButtonScript>().selected = false;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < twoButtons.Count; i++)
                {
                    // find the other buttons selected and turn them off
                    if (twoButtons[i].GetComponent<LoadOutButtonScript>().buttonWeapon != GameManager.instance.currentGuns[weaponSlot])
                    {
                        Debug.Log("Reset Button:" + oneButtons[i].GetComponent<LoadOutButtonScript>().buttonWeapon);
                        twoButtons[i].GetComponent<Button>().interactable = true;
                        twoButtons[i].GetComponent<LoadOutButtonScript>().selected = false;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < threeButtons.Count; i++)
                {
                    // find the other buttons selected and turn them off
                    if (threeButtons[i].GetComponent<LoadOutButtonScript>().buttonWeapon != GameManager.instance.currentGuns[weaponSlot])
                    {
                        Debug.Log("Reset " + i + " Button:" + oneButtons[i].GetComponent<LoadOutButtonScript>().buttonWeapon);
                        threeButtons[i].GetComponent<Button>().interactable = true;
                        threeButtons[i].GetComponent<LoadOutButtonScript>().selected = false;
                    }
                }
                break;
        }
    }
}
