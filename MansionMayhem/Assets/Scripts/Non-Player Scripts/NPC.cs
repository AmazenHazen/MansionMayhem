using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    #region Attributes
    // Attributes
    private string charName;
    protected GameObject player;

    // For Text Files
    [Header("Dialog Assets")]
    public TextAsset initialTextFile;
    public TextAsset startedQuestTextFile;
    public TextAsset completedQuestTextFile;
    private string[] textLines;

    // Variables to track dialogue options
    private ResponseType[] Responses;
    private int optionNumber;            // Number of options for current question

    // Reference to the current line for dialog
    // reference to the last line for dialog
    private int currentLine;
    private int endAtLine;

    // Talking bools
    private bool talkingBool;

    // Quest Variables
    private GameObject questIcon;
    private List<Sprite> QuestSprites; // for changing the overhead sprite of the NPC
    [Header("Lists for Commands")]
    public List<GameObject> items; // for items that the NPC will take from or accept from the player
    public List<GameObject> enemies;
    public List<GameObject> requirements; // requirements for completing a quest (could be items, interactive objects, or NPCs)
    public List<ItemType> itemRequirements;
    public List<GameObject> additionalObjectsNeeded;
    protected QuestStatus currentQuestStatus;
    #endregion

    #region NPC properties
    public bool TalkingBool
    {
        get { return talkingBool; }
        set { talkingBool = value; }
    }
    public QuestStatus CurrentQuestStatus
    {
        get { return currentQuestStatus; }
    }

    #endregion

    #region Start Method
    // Use this for initialization
    protected virtual void Start()
    {
        // Find the player game object
        player = GameObject.FindGameObjectWithTag("player");

        // Get the queest icon sprite
        if (transform.Find("Icon"))
        {
            questIcon = transform.Find("Icon").gameObject;
        }

        // Load in the Icons for the NPCS
        QuestSprites = new List<Sprite>(3);
        QuestSprites.Add(Resources.Load<Sprite>("Images/NPCs/Quest Icon"));
        QuestSprites.Add(Resources.Load<Sprite>("Images/NPCs/Started Icon"));
        QuestSprites.Add(Resources.Load<Sprite>("Images/NPCs/Completed Icon"));

        currentLine = 0;

        // Creates an Array for responses
        Responses = new ResponseType[5];

        // Check if the NPC has a text file
        TextFileSetUp(initialTextFile);


        // Start the quest status of the NPC to not started
        currentQuestStatus = QuestStatus.NotStarted;
        if (questIcon != null)
        {
            questIcon.GetComponent<SpriteRenderer>().sprite = QuestSprites[0];
        }
    }
    #endregion

    #region Update
    // Update is called once per frame
    protected virtual void Update()
    {
        if (talkingBool)
        {
            TalkingTo();
        }
    }
    #endregion

    #region Dialogue helper methods
    /// <summary>
    /// Helper method that takes checks whether there is a valid text file and set it as the current tect for the NPC
    /// </summary>
    /// <param name="textFile"></param>
    public void TextFileSetUp(TextAsset textFile)
    {
        // Check if the NPC has a text file
        if (textFile != null)
        {
            // Create an array of text with the different lines of the text file
            textLines = textFile.text.Split('\n');

            // Set endAtLine to the textLines length - 1
            endAtLine = textLines.Length - 1;
        }

    }

    #endregion

    #region Main Talking Method
    /// <summary>
    /// Talking to script that handles basic text
    /// </summary>
    public void TalkingTo()
    {
        // Sets the text box to the first/current line of dialog
        //dialogText.GetComponent<Text>().text = textLines[currentLine];

        if (GUIManager.dialogBox.activeSelf == false)
        {
            // Activate the dialog box
            GUIManager.TurnOnDialogBox();

            // Start the scrolling text 
            //Debug.Log("Starting Dialog");
            if (textLines[currentLine][0] != '[')
            {
                StartCoroutine(GUIManager.TextScroll(textLines[currentLine]));
            }
        }

        // Advance the text if the player hits Enter or Space
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // if space and the text isn't scrolling, advance a line
            if (!GUIManager.isTyping)
            {
                if (GUIManager.optionBool == false)
                {
                    currentLine++;
                }

                // Don't let the user go past the endline
                if (currentLine > endAtLine)
                {
                    //Debug.Log("Manually Exit Dialog");

                    //end the dialogue if at the end
                    endDialogue();
                }
                // Otherwise start scrolling the text
                else
                {
                    //Debug.Log("Starting Dialog Scrolling if not at the end of the text");

                    // Check to see if the line is already printed out or if the line is a command line (don't want to print out commands!)
                    if (GUIManager.dialogText.GetComponent<Text>().text != textLines[currentLine] && textLines[currentLine][0] != '[')
                    {
                        // Otherwise start typing it out
                        StartCoroutine(GUIManager.TextScroll(textLines[currentLine]));
                    }
                }
            }
            // If the text box is currently printing the text then cancel the scrolling
            else if (GUIManager.isTyping && !GUIManager.cancelTyping)
            {
                //Debug.Log("Cancel Typing");

                GUIManager.cancelTyping = true;
            }
        }

        // Check for dialog options
        // The command for determining the dialog options is the "*" character at the begginging of lines
        // Dialog Options designated with a [ at the beginning of the line (just like any command)
        CheckForOptions();


        // Commands are inside square brackets, so commands check if the first character is a "["
        if (textLines[currentLine][0] == '[')
        {
            CheckCommand();
        }
        

    }
    #endregion

    #region Special Commands
    public void CheckCommand()
    {
        #region Variables for checking a command
        bool endOfCommand = false;
        string fullOptionText = textLines[currentLine];
        string commandText = "";
        string secondaryCommandText = "";
        string tertiaryCommandText = "";
        int secondaryNum = -1; // can represent an item number to take from the NPC or give the NPC or a line number to jump to
        int tertiaryNum = -1; // can represent a line number to jump to
        GameObject otherObject;
        #endregion

        int i = 1;

        #region Primary Command
        // Loops through
        // Saves the text between the square brackets
        do
        {
            if (fullOptionText[i] == ']')
            {
                endOfCommand = true;
            }
            else if (endOfCommand != true)
            {
                commandText += fullOptionText[i];
            }

            i++;
        } while (endOfCommand == false);

        #endregion

        #region Secondary Command
        // getting the text for item management with an NPC or number for going to a different text line
        switch (commandText)
        {
            case "GiveItem":        // Second number is index of the item in items list
            case "RemoveItem":      // Second number is index of the item in items list
            case "SpawnEnemy":      // Second number is the index of the enemy in the enemies list
            case "StartQuestOtherObject":// Second number is the index of the additionalObjectsNeeded list
            case "SetObjectActive":     // Second number is the index of the additionalObjectsNeeded list
            case "GoToLine":        // Second number is line number the text will go to
            case "CheckRequirements": // Second number is the line to go to if the player fulfills the requirements
            case "Reward":              // Second number is the number of screws rewarded to the player
                // Increment by one for the beginning parenthesis
                i++;

                // Reset endofCommand to flase for the do/while loop
                endOfCommand = false;

                // loop through to get the full number
                do
                {
                    if (fullOptionText[i] == ')')
                    {
                        endOfCommand = true;
                    }
                    else if (endOfCommand != true)
                    {
                        secondaryCommandText += fullOptionText[i];
                    }

                    i++;
                } while (endOfCommand == false);


                // Loops through
                // Saves the text between the square brackets
                int.TryParse(secondaryCommandText, out secondaryNum);

                Console.WriteLine(secondaryNum);

                break;
        }
        #endregion

        #region Tertiary Command
        switch (commandText)
        {
            // Tertiary command set to object
            // used for conditional statements
            case "CheckRequirements":// Third number is the line the text document goes to if requirements are not met
            case "SpawnEnemy":      // Third number represents the line to go to if there are no enemies left to spawn
            // Increment by one for the beginning parenthesis
            i++;

            // Reset endofCommand to flase for the do/while loop
            endOfCommand = false;

            // loop through to get the full number
            do
            {
                if (fullOptionText[i] == ')')
                {
                    endOfCommand = true;
                }
                else if (endOfCommand != true)
                {
                    tertiaryCommandText += fullOptionText[i];
                }

                i++;
            } while (endOfCommand == false);

            // Loops through
            // Saves the text between the square brackets
            int.TryParse(tertiaryCommandText, out tertiaryNum);

            Console.WriteLine(tertiaryNum);
                break;
        }

        #endregion
        
        //Debug.Log("Command: " + commandText + " Secondary Command Number: " + secondaryNum + " Tertiary Command Number: " + tertiaryCommandText);
        #region Commands
        switch (commandText)
        {
            #region Item based commands (give/remove)
            case "GiveItem":
                // give player that item
                //Debug.Log("Gave player: " + items[secondaryNum]);

                // Add item to inventory
                player.GetComponent<PlayerManager>().AddItem(items[secondaryNum]);

                // Advance to the next text line
                currentLine++;
                break;

            case "RemoveItem":
                // give player that item
                //Debug.Log("Took from player: " + items[secondaryNum]);

                // Add item to inventory
                player.GetComponent<PlayerManager>().RemoveItem(items[secondaryNum]);

                // Advance to the next text line
                currentLine++;
                break;

            case "Reward":
                GameManager.instance.screws += secondaryNum;
                currentLine++;
                break;
            #endregion

            #region Dynamic Commands
            // This command checks all requirements including Items, Enemies, and NPCs
            // Items are checked in the inventory and only removed once all requirements are met
            // Enemies are checked if they are null (they are removed from the list and are deleted from the scene if dead)
            // NPCs are checked if they have a Complete Quest Status
            case "CheckRequirements":
                // give player that item
                //Debug.Log("Check Requirements");
                int completedRequirements = 0;

                for (int j=0; j<requirements.Count; j++)
                {
                    // For enemies (they would be deleted if they are defeated)
                    // This must be checked first so that a nullreferenece exception doesn't occur
                    if (requirements[j] != null)
                    {
                        // For NPCS (if their quest has been completed)
                        if (requirements[j].GetComponent<NPC>())
                        {
                            if (requirements[j].GetComponent<NPC>().currentQuestStatus == QuestStatus.Completed)
                            {
                                completedRequirements++;
                            }
                        }

                        // For Allies (if their quest has been completed)
                        if (requirements[j].GetComponent<AllyManager>())
                        {
                            if (requirements[j].GetComponent<AllyManager>().currentQuestStatus == QuestStatus.Completed)
                            {
                                completedRequirements++;
                            }
                        }
                    }
                    else
                    {
                        requirements.Remove(requirements[j]);
                        j--;
                    }
                }

                // Check For Items
                bool requirementfulfilled = true;
                requirementfulfilled = CheckInventory(player.GetComponent<PlayerManager>().playerItems);

                if (completedRequirements == requirements.Count && requirementfulfilled)
                {
                    //Debug.Log("Requirements fulfilled, moved to line " + secondaryNum);
                    // Only remove Item requirements if the full check has been completed
                    RemoveItemRequirements(player.GetComponent<PlayerManager>().playerItems);
                    currentLine = secondaryNum;
                }
                else
                {
                    //Debug.Log("Requirements not fulfilled, moved to line " + tertiaryNum);
                    currentLine = tertiaryNum;
                }

                //Debug.Log("Current Line: " + currentLine);
                break;
            #endregion

            #region Quest Commands
            case "ResetQuest":
                //Debug.Log("Command: " + commandText + secondayCommand);
                currentQuestStatus = QuestStatus.NotStarted;
                if (questIcon != null)
                {
                    questIcon.GetComponent<SpriteRenderer>().sprite = QuestSprites[1];
                }
                TextFileSetUp(startedQuestTextFile);
                currentLine = 0;
                endDialogue();
                break;
            case "StartQuest":
                //Debug.Log("Command: " + commandText + secondayCommand);
                currentQuestStatus = QuestStatus.Started;
                if (questIcon != null)
                {
                    questIcon.GetComponent<SpriteRenderer>().sprite = QuestSprites[1];
                }
                TextFileSetUp(startedQuestTextFile);
                currentLine = 0;
                endDialogue();
                break;
            case "StartQuestOtherObject":
                //Debug.Log("Command: " + commandText + secondayCommand);
                otherObject = additionalObjectsNeeded[secondaryNum];
                NPC otherNPCScript = otherObject.GetComponent<NPC>();
                otherNPCScript.currentQuestStatus = QuestStatus.Started;
                if (otherNPCScript.questIcon != null)
                {
                    otherNPCScript.questIcon.GetComponent<SpriteRenderer>().sprite = QuestSprites[1];
                }
                otherNPCScript.TextFileSetUp(otherNPCScript.startedQuestTextFile);
                otherNPCScript.currentLine = 0;
                currentLine++;
                break;

            case "CompleteQuest":
                currentQuestStatus = QuestStatus.Completed;
                //questIcon.GetComponent<SpriteRenderer>().sprite = QuestSprites[2];
                if (questIcon != null)
                {
                    questIcon.GetComponent<SpriteRenderer>().sprite = null;
                }
                TextFileSetUp(completedQuestTextFile);
                currentLine = 0;
                endDialogue();
                break;
            #endregion

            #region Spawning Commands
            case "SetObjectActive":
                if (additionalObjectsNeeded[secondaryNum] != null)
                {
                    additionalObjectsNeeded[secondaryNum].SetActive(true);
                    currentLine++;
                }
                else
                {
                    currentLine = tertiaryNum;
                }
                break;
            case "SpawnEnemy":
                if (enemies[secondaryNum] != null)
                {
                    enemies[secondaryNum].SetActive(true);
                    currentLine++;
                }
                else
                {
                    currentLine = tertiaryNum;
                }
                break;
            case "StartBossFight":
                // Start the boss fight
                enemies[0].SetActive(true);
                GameObject.Find("HUDCanvas").GetComponent<GUIManager>().BossHealthSetUp(enemies[0]);
                GUIManager.bossFight = true;
                currentLine++;
                break;
            #endregion

            #region Dialogue Specific Commands
            case "GoToLine":
                // Go to a specific text line
                currentLine = secondaryNum;
                break;

            case "Exit":
                endDialogue();
                break;
            #endregion

            #region Ally Commands
            case "StartFollowing":
                GetComponent<AllyMovement>().FollowingPlayer = true;
                currentLine++;
                break;

            case "StopFollowing":
                GetComponent<AllyMovement>().FollowingPlayer = false;
                currentLine++;
                break;
            #endregion

            case "DeleteSelf":
                // Go to a specific spot in the scene
                transform.position = new Vector3(10000, 10000, 0);
                currentLine++;
                break;

            #region Exiting Level
            case "ExitLevel":
                LoadOnClick.ReturnToLevelSelectScreen();
                currentLine++;
                break;
                #endregion
        }
        #endregion

        // Start the scrolling text (if the command isn't an Exit related command)
        if (!(commandText == "Exit" || commandText == "CompleteQuest" || commandText == "StartQuest" || commandText == "ExitLevel"))
        {
            StartCoroutine(GUIManager.TextScroll(textLines[currentLine]));
        }
    }
    #endregion

    #region Command Helper Methods
    /// <summary>
    /// Checks the player's inventory to see if the player fulfills all the NPC's item requirements
    /// May want to move this to the player manager script
    /// </summary>
    /// <param name="playerItems"></param>
    /// <returns></returns>
    public bool CheckInventory(ItemType[] playerItems)
    {
        // int to check if the requirements are fulfilled
        int totalNumsFulfilled = 0;

        // Check to see if players have all the requirements
        foreach (ItemType requirement in itemRequirements)
        {
            for (int i = 0; i < playerItems.Length; i++)
            {
                if (playerItems[i] == requirement)
                {
                    totalNumsFulfilled++;
                    break;
                }
            }
        }

        // if the player has all the requirements
        if (totalNumsFulfilled == itemRequirements.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// Method to remove all itemRequirements from the inventory
    /// </summary>
    /// <param name="playerItems"></param>
    public void RemoveItemRequirements(ItemType[] playerItems)
    {
        foreach (ItemType requirement in itemRequirements)
        {
            // Remove the items from the player's inventory
            for (int i = 0; i < playerItems.Length; i++)
            {
                if (playerItems[i] == requirement)
                {
                    //Debug.Log("Remove item requirement npc");
                    player.GetComponent<PlayerManager>().playerItems[i] = ItemType.NoItem;
                    GameObject.Find("HUDCanvas").GetComponent<GUIManager>().RemoveItemGUI(i);
                    break;
                }
            }
        }
    }
    #endregion

    #region Text Option Handling
    #region Choose Dialog Option
    /// <summary>
    /// Moves the current text to the Correct Response
    /// </summary>
    /// <param name="responseChosen"></param>
    public void ChooseDialogOption(ResponseType responseChosen, int lineJumpNumber)
    {
        if (responseChosen == ResponseType.SayYes)
        {
            //Debug.Log("Chose Yes!");
            GUIManager.EndOptions();
            currentLine = lineJumpNumber;
            if (textLines[currentLine][0] != '[')
            {
                StartCoroutine(GUIManager.TextScroll(textLines[currentLine]));
            }
            return;
        }

        if (responseChosen == ResponseType.SayNo)
        {

            //Debug.Log("Chose No!");
            GUIManager.EndOptions();
            currentLine = lineJumpNumber;
            if (textLines[currentLine][0] != '[')
            {
                StartCoroutine(GUIManager.TextScroll(textLines[currentLine]));
            }
            return;

        }

        if (responseChosen == ResponseType.SayNothing)
        {
            endDialogue();
        }

    }
    #endregion

    #region OptionSetup
    public void CheckForOptions()
    {
        if (currentLine + 1 < endAtLine)
        {
            if (textLines[currentLine + 1][0] == '*')
            {

                // Set the optionBool to true
                GUIManager.optionBool = true;

                // Find how many options the player has
                optionNumber = textLines[currentLine + 1][1] - 48;

                // Set the buttons
                for (int i = 0; i < optionNumber; i++)
                {
                    // Set the currentNPC if not already set
                    if (GUIManager.Options[i].transform.GetComponent<DialogOptionScript>().currentNPC == null)
                    {
                        //Debug.Log("set up options!");
                        GUIManager.Options[i].transform.GetComponent<DialogOptionScript>().currentNPC = gameObject;


                        string fullOptionText = textLines[(currentLine + 2) + i];

                        // String for the response
                        string buttonText = "";
                        // String for the type of response
                        string responseOptionText = "";
                        // bool to see if we have finished looking at the command/text
                        bool endOfCommand = false;
                        // int for the line number to jump to depending on the result of the option
                        string lineJumpStr = "";
                        int lineJump = 0;


                        int j = 0;

                        #region Text on Button (loop 1)
                        // First Loop: Text on button
                        do
                        {
                            // If you hit the first square bracket then the buttonText is finished
                            if (fullOptionText[j] == '[')
                            {
                                endOfCommand = true;
                            }
                            else if (endOfCommand != true)
                            {
                                // If the buttonText bool is true record the text down
                                buttonText += fullOptionText[j];
                            }
                            j++;
                        } while (endOfCommand == false);

                        
                        // reset the variable for the seond loop
                        endOfCommand = false;
                        #endregion

                        #region Response Type (loop 2)
                        // Loops through
                        // Puts the text before square brckets on the button
                        // and saves the text in the square brackets as a response type
                        do
                        {
                            // If you hit the last square bracket then the responseValue the button has has ended
                            if (fullOptionText[j] == ']')
                            {
                                endOfCommand = true;
                            }
                            else if (endOfCommand != true)
                            {
                                responseOptionText += fullOptionText[j];
                            }
                            // increment the index
                            j++;
                        } while (endOfCommand == false);

                        // reset the variable for the third loop
                        endOfCommand = false;
                        j++;
                        #endregion

                        #region LineJump Number Loop
                        if (responseOptionText != "SayNothing")
                        {
                            // loop through to get the number used to jump to after choosing an option
                            do
                            {
                                if (fullOptionText[j] == ')')
                                {
                                    endOfCommand = true;
                                }
                                else if (endOfCommand != true)
                                {
                                    lineJumpStr += fullOptionText[j];
                                }

                                j++;
                            } while (endOfCommand == false);
                        }

                        // convert the line jump number to an int
                        int.TryParse(lineJumpStr, out lineJump);
                        #endregion

                        // Debug Ling
                        //Debug.Log("Option: " + buttonText + " Response Option Text: " + responseOptionText + " Line jump String number: " + lineJumpStr + " Line jump number: " + lineJump);
                        
                        //Debug.Log("Button 1: " + buttonText + " Response Value: " + responseOptionText);

                        // Compares the response text to change it to a Response type
                        // Assign each ResponseType to a button.
                        switch (responseOptionText)
                        {
                            case "SayYes":
                                Responses[i] = ResponseType.SayYes;
                                GUIManager.Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayYes;
                                GUIManager.Options[i].transform.GetComponent<DialogOptionScript>().lineJumpNumber = lineJump;
                                break;
                            case "SayNo":
                                Responses[i] = ResponseType.SayNo;
                                GUIManager.Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayNo;
                                GUIManager.Options[i].transform.GetComponent<DialogOptionScript>().lineJumpNumber = lineJump;
                                break;
                            case "SayNothing":
                                Responses[i] = ResponseType.SayNothing;
                                GUIManager.Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayNothing;
                                break;
                        }


                        // Set the text on the button
                        GUIManager.Options[i].transform.GetChild(0).GetComponent<Text>().text = buttonText;
                    }


                    // Activate the buttons
                    if (!GUIManager.isTyping && !GUIManager.Options[i].activeSelf)
                    {
                        GUIManager.Options[i].SetActive(true);
                    }
                }
                
            }
        }
    }
    #endregion
    #endregion

    #region End Dialogue helping method
    /// <summary>
    /// Helper method to end text
    /// </summary>
    public void endDialogue()
    {
        // Set the dialog to the beginning
        currentLine = 0;

        //Turn off options for next time you talk to NPCs
        GUIManager.EndOptions();

        GUIManager.TurnOffDialogBox();

        // Set the Talking to bool to false
        talkingBool = false;
    }
    #endregion

}
