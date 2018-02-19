using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjectScript : MonoBehaviour
{
    // Attributes
    // Requirements for the interactableObject (if the object has any requirements)
    public List<ItemType> requirements;
    public GameObject player;

    // Holds Items the interactable Object has for the users
    public List<ItemType> containsItems;

    // Spawns (if it has any)
    public List<GameObject> spawns;

    // Reference to the GUI
    private GameObject dialogBox;
    private GameObject dialogText;
    private bool interactBool;

    // Text for the interactable object.
    public string interactingString;


    // Scrolling Autotyping variables
    private bool isTyping = false;
    private bool cancelTyping = false;
    private float typeSpeed = 0.0f;

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
        dialogText = dialogBox.transform.Find("DialogText").gameObject;
        interactBool = false;

        // Set enemies to false at the beginning
        for (int i = 0; i < spawns.Count; i++)
        {
            spawns[i].SetActive(false);
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



    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        dialogText.GetComponent<Text>().text = "";
        isTyping = true;
        cancelTyping = false;

        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            Debug.Log("Dialog Scrolling");

            dialogText.GetComponent<Text>().text += lineOfText[letter];
            letter++;

            yield return new WaitForSeconds(typeSpeed);
        }
        dialogText.GetComponent<Text>().text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }

    public void InteractText()
    {
        // Sets the text box to the first/current line of dialog
        //dialogText.GetComponent<Text>().text = textLines[currentLine];

        if (dialogBox.activeSelf == false)
        {
            // Activate the dialog box
            dialogBox.SetActive(true);

            // Start the scrolling text 
            Debug.Log("Starting Dialog");

            if (CheckRequirements(player.GetComponent<PlayerManager>().playerItems))
            {
                StartCoroutine(TextScroll(interactingString));
            }
            else
            {
                StartCoroutine(TextScroll("You don't have what you need for this."));
            }
        }

        // Advance the text if the player hits Enter or Space
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            // if space and the text isn't scrolling, advance a line
            if (!isTyping)
            {

                Debug.Log("Exit Dialog");

                //end the dialogue if at the end
                endDialogue();

            }
            // If the text box is currently printing the text then cancel the scrolling
            else if (isTyping && !cancelTyping)
            {
                Debug.Log("Cancel Typing");

                cancelTyping = true;
            }

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
        GameManager.instance.currentGameState = GameState.Play;
        GUIManager.usingOtherInterface = false;
        Time.timeScale = 1;

        // Set the Talking to bool to false
        interactBool = false;

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
