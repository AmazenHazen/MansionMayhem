using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjectScript : MonoBehaviour
{
    // Attributes
    // Requirements for the interactableObject (if the object has any requirements)
    public List<ItemType> requirements;

    // Holds Items the interactable Object has for the users
    public List<ItemType> containsItems;

    // Spawns (if it has any)
    public List<GameObject> spawns;

    // Reference to the GUI
    private GameObject dialogBox;
    private GameObject dialogText;
    private bool interactBool;
    private bool delayBool;

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
        dialogBox = GameObject.Find("DialogBox");
        dialogText = dialogBox.transform.FindChild("DialogText").gameObject;
        interactBool = false;
        delayBool = true;

        // Set enemies to false at the beginning
        for (int i = 0; i < spawns.Count; i++)
        {
            spawns[i].SetActive(false);
        }
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


    public void InteractText()
    {
        // Sets the text box to the first/current line of dialog
        dialogText.GetComponent<Text>().text = interactingString;


        if (dialogBox.activeSelf == false)
        {
            dialogBox.SetActive(true);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && delayBool==false)
        {
            Debug.Log("End Interaction");
            //end the dialogue if at the end
            endDialogue();
        }
    }




    /// <summary>
    /// Helper method to end text
    /// </summary>
    public void endDialogue()
    {
        // Return the Game World to normal time
        // Pause the gameplay
        // Set pauseGame to true
        GameManager.currentGameState = GameState.Play;
        GUIManager.usingOtherInterface = false;
        Time.timeScale = 1;

        // Set the Talking to bool to false
        interactBool = false;
        delayBool = false;

        // Set the dialog box off
        dialogBox.SetActive(false);
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
