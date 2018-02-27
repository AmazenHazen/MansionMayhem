using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    #region Attributes
    public GameObject linkedDoor;
    public List<ItemType> requirements;
    public GameObject lockOnDoor;
    public bool bossDoor;

    // bool that makes sure the player is only interacting with this object
    private bool interactBool;
    #endregion


    public bool InteractBool
    {
        get { return interactBool; }
        set { interactBool = value; }
    }

    #region
    public void Start()
    {
        // set it as default that you aren't interacting with this door
        interactBool = false;

        // if the door has a lock for it
        if (linkedDoor.name != "upstairs" && (linkedDoor.name != "downstairs"))
        {
            lockOnDoor = transform.GetChild(0).gameObject;

            // Check if the door was locked or not
            if (requirements.Count <= 0)
            {
                // Hide the lock!
                lockOnDoor.SetActive(false);
            }

        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // if space and the text isn't scrolling, end dialog
            if (!GUIManager.isTyping && interactBool)
            {
                interactBool = false;
                GUIManager.TurnOffDialogBox();
            }
        }
    }
    #endregion

    public void removeRequirement(ItemType item)
    {
        requirements.Remove(item);

        if(requirements.Count <= 0)
        {
            // Hide the lock!
            lockOnDoor.SetActive(false);
        }
    }


    #region Travel Method
    public void Travel(GameObject player)
    {
            if (linkedDoor.name == "topdoor")
            {
                player.transform.position = new Vector2(linkedDoor.transform.position.x, linkedDoor.transform.position.y) + new Vector2(0, -.5f);
            }
            if (linkedDoor.name == "bottomdoor")
            {
                player.transform.position = new Vector2(linkedDoor.transform.position.x, linkedDoor.transform.position.y) + new Vector2(0, .5f);
            }
            if (linkedDoor.name == "leftdoor")
            {
                player.transform.position = new Vector2(linkedDoor.transform.position.x, linkedDoor.transform.position.y) + new Vector2(.5f, 0);
            }
            if (linkedDoor.name == "rightdoor")
            {
                player.transform.position = new Vector2(linkedDoor.transform.position.x, linkedDoor.transform.position.y) + new Vector2(-.5f, 0);
            }
            if (linkedDoor.name == "upstairs")
            {
                player.transform.position = new Vector2(linkedDoor.transform.position.x, linkedDoor.transform.position.y) + new Vector2(0, -.5f);
            }
            if (linkedDoor.name == "downstairs")
            {
                player.transform.position = new Vector2(linkedDoor.transform.position.x, linkedDoor.transform.position.y) + new Vector2(0, .5f);
            }

        if (bossDoor)
        {
            GUIManager.bossFight = true;
        }

    }

    public void MessageRequirements()
    {
        // Interacting with this door
        interactBool = true;

        string requirementString = "You need these items to unlock the door: ";
        foreach (ItemType requirement in requirements)
        {
            requirementString += requirement + " ";
        }
        requirementString += ".";

        GUIManager.TurnOnDialogBox();

        StartCoroutine(GUIManager.TextScroll(requirementString));
    }


    #endregion
}
