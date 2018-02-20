﻿using System;
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

    // Reference to the GUI
    private GameObject dialogBox;
    private GameObject dialogText;

    // Variables to track dialogue options
    private List<GameObject> Options;
    private ResponseType[] Responses;
    private string playerChoices;
    private int optionNumber;            // Number of options for current question

    // Reference to the current line for dialog
    // reference to the last line for dialog
    private int currentLine;
    private int endAtLine;

    // Talking bools
    private bool talkingBool;
    private bool optionBool;

    // Scrolling Autotyping variables
    private bool isTyping = false;
    private bool cancelTyping = false;

    private float typeSpeed = 0.0f;

    // Quest Variables
    private GameObject questIcon;
    public List<Sprite> QuestSprites; // for changing the overhead sprite of the NPC
    public List<GameObject> items; // for items that the NPC will take from or accept from the player
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
        questIcon = transform.GetChild(0).gameObject;


        // Get the dialog boxes for dialog
        dialogBox = GameObject.Find("DialogBox");
        dialogText = dialogBox.transform.Find("DialogText").gameObject;


        // Get the options for option dialog
        Options = new List<GameObject>();
        for(int i =0; i<5; i++)
        {
            Options.Add(dialogBox.transform.Find("Options").GetChild(i).gameObject);
        }

        currentLine = 0;

        // Creates an Array for responses
        Responses = new ResponseType[5];

        // Check if the NPC has a text file
        TextFileSetUp(initialTextFile);


        // Start the quest status of the NPC to not started
        currentQuestStatus = QuestStatus.NotStarted;
        questIcon.GetComponent<SpriteRenderer>().sprite = QuestSprites[0];

        base.Start();
    }
    #endregion

    #region Update
    // Update is called once per frame
    protected override void Update()
    {
        if(talkingBool)
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

    private IEnumerator TextScroll (string lineOfText)
    {
        int letter = 0;
        dialogText.GetComponent<Text>().text = "";
        isTyping = true;
        cancelTyping = false;

        while(isTyping && !cancelTyping && (letter<lineOfText.Length - 1))
        {
            //Debug.Log("Dialog Scrolling");

            dialogText.GetComponent<Text>().text += lineOfText[letter];
            letter++;

            yield return new WaitForSeconds(typeSpeed);
        }
        dialogText.GetComponent<Text>().text = lineOfText;
        isTyping = false;
        cancelTyping = false;
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

            if (dialogBox.activeSelf == false)
            {
                // Activate the dialog box
                dialogBox.SetActive(true);

                // Start the scrolling text 
                //Debug.Log("Starting Dialog");
                StartCoroutine(TextScroll(textLines[currentLine]));
            }

            // Advance the text if the player hits Enter or Space
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                // if space and the text isn't scrolling, advance a line
                if (!isTyping)
                {
                    if (optionBool == false)
                    {
                        currentLine++;
                    }

                    // Don't let the user go past the endline
                    if (currentLine >= endAtLine)
                    {
                        Debug.Log("Manually Exit Dialog");

                        //end the dialogue if at the end
                        endDialogue();
                    }
                    // Otherwise start scrolling the text
                    else
                    {
                        //Debug.Log("Starting Dialog Scrolling if not at the end of the text");

                        // Check to see if the line is already printed out
                        if (dialogText.GetComponent<Text>().text != textLines[currentLine])
                        {
                            // Otherwise start typing it out
                            StartCoroutine(TextScroll(textLines[currentLine]));
                        }
                    }
                }
                // If the text box is currently printing the text then cancel the scrolling
                else if (isTyping && !cancelTyping)
                {
                    //Debug.Log("Cancel Typing");

                    cancelTyping = true;
                }
            }

        // Check for dialog options
        // Dialog Options designated with a * at the beginning of the line
        if (talkingBool)
        {
            CheckForOptions();

            if (textLines[currentLine][0] == '[')
            {
                CheckCommand();
            }
        }
            
    }
    #endregion
     
    #region Special Commands
    public void CheckCommand()
    {
        bool endOfCommand = false;
        string fullOptionText = textLines[currentLine];
        string commandText = "";
        string secondaryCommandText = "";
        string tertiaryCommandText = "";
        int secondaryNum = -1; // can represent an item number to take from the NPC or give the NPC or a line number to jump to
        int tertiaryNum = -1; // can represent a line number to jump to

        //string secondayCommand = "";
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
        if (commandText == "GiveItem" || commandText == "RemoveItem" || commandText == "GoToLine" || commandText == "CheckRequirements")
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
        if (commandText == "CheckRequirements")
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

        Debug.Log("Command: " + commandText + " Secondary Command Number: " + secondaryNum + " Tertiary Command Number: " + tertiaryCommandText);

        switch (commandText)
        {
            case "StartQuest":
                //Debug.Log("Command: " + commandText + secondayCommand);
                currentQuestStatus = QuestStatus.Started;
                questIcon.GetComponent<SpriteRenderer>().sprite = QuestSprites[1];
                TextFileSetUp(startedQuestTextFile);
                currentLine = 0;
                endDialogue();
                break;

            case "GiveItem":
                // give player that item
                Debug.Log("Gave player: " + items[secondaryNum]);

                // Add item to inventory
                player.GetComponent<PlayerManager>().AddItem(items[secondaryNum]);

                // Advance to the next text line
                currentLine++;
                break;

            case "RemoveItem":
                // give player that item
                Debug.Log("Took from player: " + items[secondaryNum]);

                // Add item to inventory
                player.GetComponent<PlayerManager>().RemoveItem(items[secondaryNum]);

                // Advance to the next text line
                currentLine++;
                break;

            case "CheckRequirements":
                // give player that item
                Debug.Log("Check Requirements");
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
                    if (requirements[j].GetComponent<InteractableObjectScript>())
                    {
                        if(requirements[j].GetComponent<InteractableObjectScript>().currentQuestStatus == QuestStatus.Completed)
                        {
                            completedRequirements++;
                        }
                    }
                }

                if (completedRequirements == requirements.Count)
                {
                    Debug.Log("Requirements fulfilled, moved to line " + secondaryNum);
                    currentLine = secondaryNum;
                }
                else
                {
                    Debug.Log("Requirements not fulfilled, moved to line " + tertiaryNum);
                    currentLine = tertiaryNum;
                }

                Debug.Log("Current Line: " + currentLine);
                break;

            case "GoToLine":
                // Go to a specific text line
                currentLine = secondaryNum;
                break;

            case "CompleteQuest":
                currentQuestStatus = QuestStatus.Completed;
                //questIcon.GetComponent<SpriteRenderer>().sprite = QuestSprites[2];
                questIcon.GetComponent<SpriteRenderer>().sprite = null;
                TextFileSetUp(completedQuestTextFile);
                currentLine = 0;
                endDialogue();
                break;

            case "Exit":
                endDialogue();
                break;
        }

        // Start the scrolling text 
        StartCoroutine(TextScroll(textLines[currentLine]));
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
            Debug.Log("Chose Yes!");
            EndOptions();
            currentLine = lineJumpNumber;
            StartCoroutine(TextScroll(textLines[currentLine]));
            return;
        }

        if (responseChosen == ResponseType.SayNo)
        {

            Debug.Log("Chose No!");
            EndOptions();
            currentLine = lineJumpNumber;
            StartCoroutine(TextScroll(textLines[currentLine]));
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
                optionBool = true;

                // Find how many options the player has
                optionNumber = textLines[currentLine + 1][1] - 48;

                // Set the buttons
                for (int i = 0; i < optionNumber; i++)
                {
                    // Set the currentNPC if not already set
                    if (Options[i].transform.GetComponent<DialogOptionScript>().currentNPC == null)
                    {
                        Debug.Log("set up options!");
                        Options[i].transform.GetComponent<DialogOptionScript>().currentNPC = gameObject;


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
                        Debug.Log("Option: " + buttonText + " Response Option Text: " + responseOptionText + " Line jump String number: " + lineJumpStr + " Line jump number: " + lineJump);
                        
                        //Debug.Log("Button 1: " + buttonText + " Response Value: " + responseOptionText);

                        // Compares the response text to change it to a Response type
                        // Assign each ResponseType to a button.
                        switch (responseOptionText)
                        {
                            case "SayYes":
                                Responses[i] = ResponseType.SayYes;
                                Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayYes;
                                Options[i].transform.GetComponent<DialogOptionScript>().lineJumpNumber = lineJump;
                                break;
                            case "SayNo":
                                Responses[i] = ResponseType.SayNo;
                                Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayNo;
                                Options[i].transform.GetComponent<DialogOptionScript>().lineJumpNumber = lineJump;
                                break;
                            case "SayNothing":
                                Responses[i] = ResponseType.SayNothing;
                                Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayNothing;
                                break;
                        }


                        // Set the text on the button
                        Options[i].transform.GetChild(0).GetComponent<Text>().text = buttonText;
                    }


                    // Activate the buttons
                    if (!isTyping && !Options[i].activeSelf)
                    {
                        Options[i].SetActive(true);
                    }
                }
                
            }
        }
    }
    #endregion

    #region Turn off Option Buttons helper method
    public void EndOptions()
    {
        optionBool = false;

        //Turn off options for new text
        for (int i = 0; i < 5; i++)
        {
            // Change the option buttons to default and to no NPC
            Options[i].transform.GetComponent<DialogOptionScript>().lineJumpNumber = 0;
            Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayNothing;
            Options[i].transform.GetComponent<DialogOptionScript>().currentNPC = null;

            // DeActivate the buttons
            Options[i].SetActive(false);
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
        // Set the Talking to bool to false
        talkingBool = false;

        // Set playerChoices to null
        playerChoices = null;

        //Turn off options for next time you talk to NPCs
        EndOptions();

        // Set the dialog box off
        dialogBox.SetActive(false);

        // Set the dialog to the beginning
        currentLine = 0;

        // Return the Game World to normal time
        // Pause the gameplay
        // Set pauseGame to true
        GameManager.instance.currentGameState = GameState.Play;
        GUIManager.usingOtherInterface = false;
        Time.timeScale = 1;
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
