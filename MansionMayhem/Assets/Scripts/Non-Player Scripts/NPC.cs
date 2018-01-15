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
    private bool setUpTalking;
    private bool talkingBool;
    private bool optionBool;

    // Scrolling Autotyping variables
    private bool isTyping = false;
    private bool cancelTyping = false;

    private float typeSpeed = 0.0f;

    // Quest Variables
    //QuestStatus currentQuestStatus;
    #endregion

    #region NPC properties
    public bool TalkingBool
    {
        get { return talkingBool; }
        set { talkingBool = value; }
    }
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
        Options = new List<GameObject>();
        for(int i =0; i<5; i++)
        {
            Options.Add(dialogBox.transform.FindChild("Options").GetChild(i).gameObject);
        }

        currentLine = 0;
        setUpTalking = true;

        // Creates an Array for responses
        Responses = new ResponseType[5];

        // Check if the NPC has a text file
        TextFileSetUp(initialTextFile);


        // Start the quest status of the NPC to not started
        //currentQuestStatus = QuestStatus.NotStarted;

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
            Debug.Log("Dialog Scrolling");

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
        // Set up the dialog
        DialogSetUp();

        // Sets the text box to the first/current line of dialog
        //dialogText.GetComponent<Text>().text = textLines[currentLine];

        if (dialogBox.activeSelf == false)
        {
            // Activate the dialog box
            dialogBox.SetActive(true);

            // Start the scrolling text 
            Debug.Log("Starting Dialog");
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
                if (currentLine == endAtLine)
                {
                    Debug.Log("Exit Dialog");

                    //end the dialogue if at the end
                    endDialogue();
                }
                // Otherwise start scrolling the text
                else
                {
                    Debug.Log("Starting Dialog Scrolling if not at the end of the text");
                    
                    // Check to see if the line is already printed out
                    if(dialogText.GetComponent<Text>().text != textLines[currentLine])
                    { 
                        // Otherwise start typing it out
                        StartCoroutine(TextScroll(textLines[currentLine]));
                    }
                }
            }
            // If the text box is currently printing the text then cancel the scrolling
            else if (isTyping && !cancelTyping)
            {
                Debug.Log("Cancel Typing");

                cancelTyping = true;
            }
        }

        // Check for dialog options
        // Dialog Options designated with a * at the beginning of the line
        CheckForOptions();
        CheckCommand();

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
        string secondayCommand = "";
        int i = 0;

        if (textLines[currentLine][0] == '[')
        {
            // Loops through
            // Saves the text between the square brackets
            for (i = 1; i < fullOptionText.Length; i++)
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

            // Secondary command set to object
            if(commandText == "GiveItem")
            {
                // Loops through
                // Saves the text between the square brackets
                for (i++; i < fullOptionText.Length; i++)
                {
                    if (fullOptionText[i] == ')')
                    {
                        endOfCommand = true;
                    }
                    else if (endOfCommand != true)
                    {
                        secondayCommand += fullOptionText[i];
                    }
                }
            }
        }
        Debug.Log("Command: " + commandText + secondayCommand);
        


        switch (commandText)
        {
            case "StartQuest":
                //currentQuestStatus = QuestStatus.Started;
                TextFileSetUp(startedQuestTextFile);
                currentLine = 0;
                endDialogue();
                break;

            case "GiveItem":
                //currentQuestStatus = QuestStatus.Started;
                Debug.Log("Gave player: " + secondayCommand);
                break;

            case "CompleteQuest":
                //currentQuestStatus = QuestStatus.Completed;
                break;

            case "Exit":
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
        if (responseChosen == ResponseType.SayYes)
        {
            playerChoices += "+";

            for (int i = currentLine; i < endAtLine; i++)
            {
                if (textLines[i][0] == '+')
                {
                    Debug.Log("Chose Yes!");
                    EndOptions();
                    currentLine = ++i;
                    StartCoroutine(TextScroll(textLines[currentLine]));
                    return;
                }
            }
        }

        if (responseChosen == ResponseType.SayNo)
        {
            playerChoices += "-";
            for (int i = currentLine; i < endAtLine; i++)
            {
                if (textLines[i][0] == '-')
                {
                    Debug.Log("Chose No!");
                    EndOptions();
                    currentLine = ++i;
                    StartCoroutine(TextScroll(textLines[currentLine]));
                    return;
                }
            }
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
                        case "SayYes":
                            Responses[i] = ResponseType.SayYes;
                            Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayYes;
                            break;
                        case "SayNo":
                            Responses[i] = ResponseType.SayNo;
                            Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayNo;
                            break;
                        case "SayNothing":
                            Responses[i] = ResponseType.SayNothing;
                            Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayNothing;
                            break;
                    }


                    // Set the text on the button
                    Options[i].transform.GetChild(0).GetComponent<Text>().text = buttonText;

                    // Activate the buttons
                    if (!isTyping)
                    {
                        Options[i].SetActive(true);
                    }
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
        GameManager.currentGameState = GameState.Play;
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
