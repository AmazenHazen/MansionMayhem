﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : CharacterMovement
{
    #region Attributes
    // Attributes
    public string charName;
    public GameObject player;
    public float awareDistance;

    // For Text Files
    public TextAsset textFile;
    public string[] textLines;

    // Reference to the GUI
    private GameObject dialogBox;
    private GameObject dialogText;

    // Variables to track dialogue options
    public List<GameObject> Options;
    public ResponseType[] Responses;
    public string playerChoices;
    public int optionNumber;            // Number of options for current question

    // Reference to the current line for dialog
    // reference to the last line for dialog
    public int currentLine;
    public int endAtLine;

    // Talking bools
    public bool setUpTalking;
    public bool talkingBool;
    public bool optionBool;

    #endregion

    #region Start Method
    // Use this for initialization
    public override void Start()
    {
        // Find the player game object
        player = GameObject.FindGameObjectWithTag("player");
        
        // Get the dialog boxes for dialog
        dialogBox = GameObject.Find("DialogBox");
        dialogText = dialogBox.transform.FindChild("DialogText").gameObject;

        // Get the options for option dialog
        for(int i =0; i<5; i++)
        {
            Options.Add(dialogBox.transform.FindChild("Options").GetChild(i).gameObject);
        }


        setUpTalking = true;

        // Creates an Array for responses
        Responses = new ResponseType[5];

        // Check if the NPC has a text file
        if(textFile != null)
        {
            // Create an array of text with the different lines of the text file
            textLines = textFile.text.Split('\n');
        }

        // If number of lines isn't specified then go through the text file
        if(endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

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

    #region Main Talking Method
    /// <summary>
    /// Talking to script that handles basic text
    /// </summary>
    public void TalkingTo()
    {
        // Set up the dialog
        DialogSetUp();

        // Sets the text box to the first/current line of dialog
        dialogText.GetComponent<Text>().text = textLines[currentLine];

        if(dialogBox.activeSelf == false)
        {
            dialogBox.SetActive(true);
        }
        // Advance the text if the player hits Enter or Space
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && (optionBool == false))
        {
            currentLine++;
        }

        // Check for dialog options
        // Dialog Options designated with a * at the beginning of the line
        CheckForOptions();
        CheckCommand();

        // Don't let the user go past the endline
        if(currentLine==endAtLine)
        {
            Debug.Log("Exit Dialog");

            //end the dialogue if at the end
            endDialogue();
        }

    }
    #endregion

    #region DialogSetUp
    public void DialogSetUp()
    {

        if (setUpTalking == true)
        {
            for (int i = 0; i < 5; i++)
            {
                Options[i].transform.GetComponent<DialogOptionScript>().currentNPC = gameObject;
            }
            setUpTalking = false;
        }
    }
    #endregion

    #region Special Commands
    public void CheckCommand()
    {
        bool endOfCommand = false;
        string fullOptionText = textLines[currentLine];
        string commandText = "";

        if (textLines[currentLine][0] == '[')
        {
            // Loops through
            // Saves the text between the square brackets
            for (int i = 1; i < fullOptionText.Length; i++)
            {
                if (fullOptionText[i] == ']')
                {
                    endOfCommand = true;
                }
                else if (endOfCommand != true)
                {
                    commandText += fullOptionText[i];
                }
            }
        }
        Debug.Log("Command: " + commandText);

        switch(commandText)
        {
            case "exit":
                endDialogue();
                break;
        }
    }
    #endregion

    #region Text Option Handling
    #region Choose Dialog Option
    /// <summary>
    /// Moves the current text to the Correct Response
    /// </summary>
    /// <param name="responseChosen"></param>
    public void ChooseDialogOption(ResponseType responseChosen)
    {
        if (responseChosen == ResponseType.sayYes)
        {
            playerChoices += "+";

            for (int i = currentLine; i < endAtLine; i++)
            {
                if (textLines[i][0] == '+')
                {
                    Debug.Log("Chose Yes!");
                    EndOptions();
                    currentLine = i;
                    return;
                }
            }
        }

        if (responseChosen == ResponseType.sayNo)
        {
            playerChoices += "-";
            for (int i = currentLine; i < endAtLine; i++)
            {
                if (textLines[i][0] == '-')
                {
                    Debug.Log("Chose No!");
                    EndOptions();
                    currentLine = i;
                    return;
                }
            }
        }
        if (responseChosen == ResponseType.sayNothing)
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
                    string fullOptionText = textLines[(currentLine + 2) + i];

                    // Bool if we are looking at a button text
                    bool buttonTextBool = true;
                    // String for the response
                    string buttonText = "";
                    // Bool to see if we are looking at response type
                    bool responseTextBool = false;
                    // String for the type of response
                    string responseOptionText = "";


                    // Loops through
                    // Puts the text before square brckets on the button
                    // and saves the text in the square brackets as a response type
                    for (int j = 0; j < fullOptionText.Length; j++)
                    {
                        // If you hit the last square bracket then the responseValue the button has has ended
                        if (fullOptionText[j] == ']')
                        {
                            responseTextBool = false;
                        }
                        // Record down what the Response type is if in the square brackets
                        if (responseTextBool == true)
                        {
                            responseOptionText += fullOptionText[j];
                        }

                        // If you hit the first square bracket then the buttonText is finished
                        if (fullOptionText[j] == '[')
                        {
                            buttonTextBool = false;
                            responseTextBool = true;
                        }
                        // If the buttonText bool is true record the text down
                        if (buttonTextBool == true)
                        {
                            buttonText += fullOptionText[j];
                        }

                    }

                    //Debug.Log("Button 1: " + buttonText + " Response Value: " + responseOptionText);

                    // Compares the response text to change it to a Response type
                    // Assign each ResponseType to a button.
                    switch (responseOptionText)
                    {
                        case "sayYes":
                            Responses[i] = ResponseType.sayYes;
                            Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.sayYes;
                            break;
                        case "sayNo":
                            Responses[i] = ResponseType.sayNo;
                            Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.sayNo;
                            break;
                        case "saySomething":
                            Responses[i] = ResponseType.sayNothing;
                            Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.sayNothing;
                            break;
                    }


                    // Set the text on the button
                    Options[i].transform.GetChild(0).GetComponent<Text>().text = buttonText;
                    // Activate the buttons
                    Options[i].SetActive(true);
                }
            }
        }
    }
    #endregion

    #region OptionEnd
    public void EndOptions()
    {
        optionBool = false;
        //Turn off options for new text
        for (int i = 0; i < 5; i++)
        {
            // Activate the buttons
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
        // Return the Game World to normal time
        // Pause the gameplay
        // Set pauseGame to true
        GUIManager.pausedGame = false;
        GUIManager.usingOtherInterface = false;
        Time.timeScale = 1;

        // Set the Talking to bool to false
        talkingBool = false;

        // Set the dialog box off
        dialogBox.SetActive(false);

        // Set playerChoices to null
        playerChoices = null;

        // Set the dialog to the beginning
        currentLine = 0;

        // Turn setUpTalking to false so that next time you talk to the NPC
        // It sets them as the current NPC
        setUpTalking = false;

        //Turn off options for next time you talk to NPCs
        EndOptions();
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
