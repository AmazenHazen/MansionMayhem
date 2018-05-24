using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    #region Attributes
    public GameObject linkedDoor;
    public List<ItemType> requirements;
    public List<GameObject> monsterRequirements;
    public GameObject lockOnDoor;
    public bool bossDoor;
    public GameObject boss;

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
            if (requirements.Count <= 0 && monsterRequirements.Count<=0)
            {
                // Hide the lock!
                lockOnDoor.SetActive(false);
            }
        }

        // If the door is the boss door, set the boss to be the same as the Level Manager
        if(bossDoor)
        {
            GameObject[] bosses = GameObject.Find("LevelManager").GetComponent<LevelManager>().boss;
            for (int i=0; i<bosses.Length; i++)
            {
                boss = bosses[i] ?? boss;
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

        CheckMonsterRequirement();
        CheckLockOnDoor();

        
    }
    #endregion

    public void CheckMonsterRequirement()
    {
        for(int i=0; i<monsterRequirements.Count;i++)
        {
            if(monsterRequirements[i] == null)
            {
                monsterRequirements.Remove(null);
            }
        }
    }

    public void CheckLockOnDoor()
    {
        if (requirements.Count <= 0 && monsterRequirements.Count<=0)
        {
            if (linkedDoor.name != "upstairs" && (linkedDoor.name != "downstairs"))
            {
                // Hide the lock!
                lockOnDoor.SetActive(false);
            }
        }
    }

    public void removeRequirement(ItemType item)
    {
        requirements.Remove(item);
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
            GameObject.Find("HUDCanvas").GetComponent<GUIManager>().BossHealthSetUp(boss);
            GUIManager.bossFight = true;
            
        }

    }

    public void MessageRequirementsItem()
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


    public void MessageRequirementsMonster()
    {
        // Interacting with this door
        interactBool = true;

        string requirementString = "You need to kill these monsters in order to unlock the door: ";
        foreach (GameObject monster in monsterRequirements)
        {
            requirementString += monster.name + " ";
        }
        requirementString += ".";

        GUIManager.TurnOnDialogBox();

        StartCoroutine(GUIManager.TextScroll(requirementString));
    }


    #endregion
}
