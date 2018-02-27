using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjectScript : MonoBehaviour
{
    // Attributes
    // Requirements for the interactableObject (if the object has any requirements)
    private GameObject player;
    public InteractableObjectType interactableType;
    public List<ItemType> requirements;
    public QuestStatus currentQuestStatus;

    // Holds Items the interactable Object has for the users
    public List<GameObject> containsItems;
    bool containsItem = true;

    // Spawns (if it has any)
    public List<GameObject> spawns;

    // bool that makes sure the player is only interacting with this object
    private bool interactBool;

    // Text for the interactable object.
    public string interactingString;


    #region Interactables properties
    public bool InteractBool
    {
        get { return interactBool; }
        set { interactBool = value; }
    }
    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        // Get the dialog boxes for dialog
        interactBool = false;

        // Set enemies to false at the beginning
        for (int i = 0; i < spawns.Count; i++)
        {
            spawns[i].SetActive(false);
        }
        if(requirements.Count>0)
        {
            currentQuestStatus = QuestStatus.Started;
        }

        player = GameObject.FindGameObjectWithTag("player");
    }
    #endregion

    void Update()
    {
        if (interactBool)
        {
            InteractText();
        }
    }

    public void removeRequirement(ItemType item)
    {
        requirements.Remove(item);
    }



    public bool CheckRequirements(ItemType[] playerItems)
    {
        // int to check if the requirements are fulfilled
        int totalNumsFulfilled = 0;

        // Check to see if players have all the requirements
        for (int i = 0; i < playerItems.Length; i++)
        {
            foreach (ItemType requirement in requirements)
            {
                if (playerItems[i] == requirement)
                {
                    totalNumsFulfilled++;
                }
            }
        }

        if (totalNumsFulfilled == requirements.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Text that goes with interacting with the interactable object
    /// </summary>
    public void InteractText()
    {
        // Sets the text box to the first/current line of dialog
        //dialogText.GetComponent<Text>().text = textLines[currentLine];

        if (GUIManager.dialogBox.activeSelf == false)
        {
            GUIManager.TurnOnDialogBox();

            // Start the scrolling text 
            Debug.Log("Starting Dialog");


            // check if the interactable object has any requirements first
            if (CheckRequirements(player.GetComponent<PlayerManager>().playerItems) && interactableType == InteractableObjectType.Taker)
            {
                StartCoroutine(GUIManager.TextScroll(interactingString));
            }
            else if(interactableType == InteractableObjectType.Taker)
            {
                StartCoroutine(GUIManager.TextScroll("You don't have what you need for this."));
                return;
            }

            if (interactableType == InteractableObjectType.Giver && containsItems.Count>0)
            {
                StartCoroutine(GUIManager.TextScroll(interactingString));

                for (int i = 0; i < containsItems.Count; i++)
                {
                    player.GetComponent<PlayerManager>().AddItem(containsItems[i]);
                    containsItems.Remove(containsItems[i]);
                }
            }
            else if (interactableType == InteractableObjectType.Giver)
            {
                StartCoroutine(GUIManager.TextScroll("You already took what you wanted from this."));
                return;
            }
        }

        // Advance the text if the player hits Enter or Space
        else if ((Input.GetKeyDown(KeyCode.Space)))
        {
            // if space and the text isn't scrolling, advance a line
            if (!GUIManager.isTyping)
            {

                Debug.Log("Exit Dialog");

                //end the dialogue if at the end
                endDialogue();

            }
            // If the text box is currently printing the text then cancel the scrolling
            else if (GUIManager.isTyping && !GUIManager.cancelTyping)
            {
                Debug.Log("Cancel Typing");

                GUIManager.cancelTyping = true;
            }

        }
    }
    

    /// <summary>
    /// Helper method to end text
    /// </summary>
    public void endDialogue()
    {
        // Set the Talking to bool to false
        interactBool = false;

        // Return the Game World to normal time
        // Pause the gameplay
        // Set pauseGame to true
        GUIManager.TurnOffDialogBox();
    }

    #region Spawn Enemies
    /// <summary>
    /// Spawn enemies if it has it
    /// </summary>
    public void SpawnEnemies()
    {
        // Check to see if requirements are completed
        if (requirements.Count == 0)
        {
            // Activates the list of GameObjects
            for (int i = 0; i < spawns.Count; i++)
            {
                spawns[i].SetActive(true);
            }
        }

    }
    #endregion

}
