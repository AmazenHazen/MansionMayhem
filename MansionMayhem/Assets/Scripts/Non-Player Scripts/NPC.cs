using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : CharacterMovement
{
    #region Attributes
    // Attributes
    private string charName;
    public GameObject player;
    public float awareDistance;

    // For Text Files
    public TextAsset initialTextFile;
    public TextAsset startedQuestTextFile;
    public TextAsset completedQuestTextFile;
    private string[] textLines;

    // Variables to track dialogue options
    private ResponseType[] Responses;
    private string playerChoices;
    private int optionNumber;            // Number of options for current question

    // Reference to the current line for dialog
    // reference to the last line for dialog
    private int currentLine;
    private int endAtLine;

    // Talking bools
    private bool talkingBool;

    // Quest Variables
    private GameObject questIcon;
    public List<Sprite> QuestSprites; // for changing the overhead sprite of the NPC
    public List<GameObject> items; // for items that the NPC will take from or accept from the player
    public List<GameObject> enemies;
    public List<GameObject> otherStuff; // for anything else needing to be accessed.
    public List<ItemType> itemRequirements;
    public List<GameObject> requirements; // requirements for completing a quest (could be items, interactive objects, or NPCs)
    public QuestStatus currentQuestStatus;
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
    public override void Start()
    {
        // Find the player game object
        player = GameObject.FindGameObjectWithTag("player");

        // Get the queest icon sprite
        if (transform.Find("Icon"))
        {
            questIcon = transform.Find("Icon").gameObject;
        }

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

        base.Start();
    }
    #endregion

    #region Update
    // Update is called once per frame
    protected override void Update()
    {
        if (talkingBool)
        {
            TalkingTo();
        }

        base.Update();
    }
    #endregion

    #region dialogue helper methods
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
        }

        // Set endAtLine to the textLines length - 1
        endAtLine = textLines.Length - 1;

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
        if (commandText == "GiveItem" || commandText == "RemoveItem" || commandText == "GoToLine" || commandText == "CheckRequirements" || commandText == "CheckItemRequirements" || commandText == "Reward")
        {
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
        }
        #endregion

        #region Tertiary Command
        // Tertiary command set to object
        // used for conditional statements
        if (commandText == "CheckRequirements" || commandText == "CheckItemRequirements")
        {
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
        }

        #endregion
        
        //Debug.Log("Command: " + commandText + " Secondary Command Number: " + secondaryNum + " Tertiary Command Number: " + tertiaryCommandText);
        #region Commands
        switch (commandText)
        {
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


            case "CheckRequirements":
                // give player that item
                //Debug.Log("Check Requirements");
                int completedRequirements = 0;

                for(int j=0; j<requirements.Count; j++)
                {
                    if(requirements[j].GetComponent<NPC>())
                    {
                        if (requirements[j].GetComponent<NPC>().currentQuestStatus == QuestStatus.Completed)
                        {
                            completedRequirements++;
                        }
                    }
                }

                if (completedRequirements == requirements.Count)
                {
                    //Debug.Log("Requirements fulfilled, moved to line " + secondaryNum);
                    currentLine = secondaryNum;
                }
                else
                {
                    //Debug.Log("Requirements not fulfilled, moved to line " + tertiaryNum);
                    currentLine = tertiaryNum;
                }

                //Debug.Log("Current Line: " + currentLine);
                break;

            case "CheckItemRequirements":
                // give player that item
                //Debug.Log("Check Item Requirements");

                bool requirementfulfilled = CheckInventory(player.GetComponent<PlayerManager>().playerItems);


                if (requirementfulfilled)
                {
                    //Debug.Log("Requirements fulfilled, moved to line " + secondaryNum);
                    currentLine = secondaryNum;
                }
                else
                {
                    //Debug.Log("Requirements not fulfilled, moved to line " + tertiaryNum);
                    currentLine = tertiaryNum;
                }

                //Debug.Log("Current Line: " + currentLine);
                break;


            case "GoToLine":
                // Go to a specific text line
                currentLine = secondaryNum;
                break;

            case "DeleteSelf":
                // Go to a specific text line
                transform.position = new Vector3(10000, 10000, 0);
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

            case "StartBossFight":
                // Start the boss fight
                enemies[0].SetActive(true);
                GameObject.Find("HUDCanvas").GetComponent<GUIManager>().BossHealthSetUp(enemies[0]);
                GUIManager.bossFight = true;
                currentLine++;
                break;

            case "Exit":
                endDialogue();
                break;
        }
        #endregion

        // Start the scrolling text (if the command isn't an Exit related command)
        if (!(commandText == "Exit" || commandText == "CompleteQuest" || commandText == "StartQuest"))
        {
            StartCoroutine(GUIManager.TextScroll(textLines[currentLine]));
        }
    }
    #endregion

    #region commandHelperMethods
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
            return true;
        }
        else
        {
            return false;
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


        // Set playerChoices to null
        playerChoices = null;

        //Turn off options for next time you talk to NPCs
        GUIManager.EndOptions();

        GUIManager.TurnOffDialogBox();

        // Set the Talking to bool to false
        talkingBool = false;
    }
    #endregion

    #region Movement Methods
    #region CalcSteerForce
    // Call the necessary Forces on the player
    protected override void CalcSteeringForces()
    {
        // Rotate the facing of the NPC if the player is close enough
        if ((player.transform.position - transform.position).magnitude < awareDistance)
        {
            Rotate();
        }
    }
    #endregion

    #region Rotation Method
    protected override void Rotate()
    {
        Vector3 targetPosition = player.transform.position;
        Vector3 dir = targetPosition - this.transform.position;
        angleOfRotation = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
    }
    #endregion

    #region Revert Speed Method for NPCs
    /// <summary>
    /// Returns Speed to Max Speed
    /// </summary>
    protected override void RevertSpeed()
    {
        // Reset speed if you are slowed
        if (currentSpeed < maxSpeed && beingSlowed == false)
        {
            currentSpeed += .05f;
        }

        //Reset speed if on slippery surface
        if (currentSpeed > maxSpeed && beingSped == false)
        {
            currentSpeed -= .05f;
        }

        // Don't allow speed to be negative or 0
        if (currentSpeed < .25f)
        {
            currentSpeed = .25f;
        }

        // Don't allow speed to be too high
        if (currentSpeed > 6f)
        {
            currentSpeed = 6f;
        }
    }
    #endregion
    #endregion

}
