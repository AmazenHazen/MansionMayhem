using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMapScript : CharacterMovement
{
    #region attibutes
    // Other Attributes for CalcSteeringForces Method
    private const float MAXFORCE = 3.0f;

    private GameObject destination;
    public string destinationName;
    public int currentLocationIndex;
    public GameObject MenuGUIManager;
    #endregion

    #region Properties
    public GameObject Destination
    {
        get { return destination; }
    }
    #endregion

    #region Start
    // Use this for initialization
    public override void Start()
    {
        // Start the initial location at the highest current level location
        // Currently start them at the initial location
        currentLocationIndex = 0;
        destinationName = "";
    }
    #endregion

    // Update is called once per frame
    protected override void Update()
    {
        if (CheckUnpaused())
        {
            base.Update();
        }
    }

    #region Player's Calc Steering Forces Method
    // Call the necessary Forces on the player
    protected override void CalcSteeringForces()
    {
        // Create a new ultimate force that is zeroed out
        Vector3 ultimateForce = Vector3.zero;

        // Call player input
        ultimateForce += playerMovementInput();

        // Ensure that the player does not move in the z-axis
        ultimateForce.z = 0;

        //Debug.Log("Before Clamp: " + ultimateForce);
        // Clamp the ultimate force by the maximum force
        ultimateForce = Vector3.ClampMagnitude(ultimateForce, MAXFORCE);

        //direction = ultimateForce.normalized;

        //Debug.Log("After Clamp: " + ultimateForce);
        ApplyForce(ultimateForce);
    }
    #endregion

    /// <summary>
    /// Helper method to check if the menus are open at all
    /// </summary>
    /// <returns></returns>
    public bool CheckUnpaused()
    {
        if (MenuGUIManager.GetComponent<LevelSelectGUIManager>().escapeScreen.activeSelf == false && MenuGUIManager.GetComponent<LevelSelectGUIManager>().instructionsScreen.activeSelf == false
            && MenuGUIManager.GetComponent<LevelSelectGUIManager>().loadoutScreen.activeSelf == false && MenuGUIManager.GetComponent<LevelSelectGUIManager>().workbenchScreen.activeSelf == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    #region Methods that must be overrided for this script to work
    #region Player Rotation
    /// <summary>
    /// Rotates the player based on the direction its facing
    /// </summary>
    protected override void Rotate()
    {
        // Get the Mouse Position
        Vector3 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        // Get lookPos Vec
        Vector3 looksPos = Camera.main.ScreenToWorldPoint(mousePos);
        looksPos = looksPos - transform.position;

        // Get the angle between the points
        angleOfRotation = Mathf.Atan2(looksPos.y, looksPos.x) * Mathf.Rad2Deg - 90;
    }
    #endregion

    #region CurrentSpeed Helper Method
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
        if (currentSpeed < .75f)
        {
            currentSpeed = .75f;
        }

        // Don't allow speed to be too high
        if (currentSpeed > 6f)
        {
            currentSpeed = 6f;
        }
    }
    #endregion
    #endregion

    #region Selecting Levels Helper Method
    /// <summary>
    /// Select level
    /// </summary>
    private void SelectLevel()
    {
        if (CheckUnpaused())
        {
            // If the user hits the enter or space button
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (destination.GetComponent<LevelLocation>().unlocked == true)
                {

                    // Set the current gamestate to play
                    GameManager.instance.currentGameState = GameState.Play;

                    // Set the current level to the level
                    GameManager.instance.currentLevel = currentLocationIndex;

                    // Enter the level
                    SceneManager.LoadScene(currentLocationIndex + 2);
                }
            }
        }
    }
    #endregion
    /// <summary>
    /// Player Collision Handled Here. This is for hovering over different levels.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        //Debug.Log("Colliding with level: " + destination);
        // Get the tag associated with the collision
        switch (collider.tag)
        {
            case "level":
                //Debug.Log("Colliding with level: " + destination);
                destination = collider.gameObject;
                currentLocationIndex = collider.gameObject.GetComponent<LevelLocation>().level;
                destinationName = collider.gameObject.GetComponent<LevelLocation>().name;
                SelectLevel();
                break;
            default:
                //Debug.Log("Not colliding with level");
                break;

        }
    }

    /// <summary>
    /// When leaving colliding with a level
    /// </summary>
    void OnTriggerExit2D()
    {
        destinationName = "";
    }
}
