using System.Collections;
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
    public GameObject dialogBox;
    public Text dialogText;

    public int currentLine;
    public int endAtLine;

    public bool talkingBool;

    #endregion

    #region Start Method
    // Use this for initialization
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");

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


    public void TalkingTo()
    {
        // Sets the text box to the first/current line of dialog
        dialogText.text = textLines[currentLine];

        if(dialogBox.activeSelf == false)
        {
            dialogBox.SetActive(true);
        }
        // Advance the text if the player hits Enter or Space
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            currentLine++;
        }

        // Don't let the user go past the endline
        if(currentLine>endAtLine)
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

            // Set the dialog to the beginning
            currentLine = 0;

        }

    }

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
}
