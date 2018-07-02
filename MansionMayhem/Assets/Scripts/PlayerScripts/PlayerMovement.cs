using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is used for player input. This takes the input from the player and uses it for the Character's input on the screen
/// </summary>
public class PlayerMovement : CharacterMovement
{
    #region Additional Movement Variables
    // Other Attributes for CalcSteeringForces Method
    public float maxForce;
    private PlayerManager playerManager;
    #endregion

    #region Start
    void Start()
    {
        maxSpeed = 3;
        minSpeed = .75f;
        playerManager = GetComponent<PlayerManager>();
    }
    #endregion

    #region Update Method
    // Update for Player
    protected override void Update()
    {
        if (GameManager.instance.currentGameState != GameState.Paused)
        {
            Rotate();
        }
        base.Update();
    }
    #endregion

    #region Player Movement Method
    // Player Input is handled here
    public Vector3 playerMovementInput()
    {
        Vector3 playerForce = Vector3.zero;

        // Player Movement Code
        if (Input.GetKey(KeyCode.W))
        {
            playerForce += new Vector3(0, 5, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerForce += new Vector3(-5, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerForce += new Vector3(5, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerForce += new Vector3(0, -5, 0);
        }

        // Sprinting
        if((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && !playerManager.IsSprinting && !playerManager.WornOut)
        {
            StartSprint();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) && !playerManager.WornOut)
        {
            EndSprint();
        }


        // Step 3: Scale Desired to maximum speed
        //         so I move as fast as possible
        playerForce.Normalize();
        playerForce *= maxSpeed;

        return playerForce;
    }
    #endregion

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
        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        //direction = ultimateForce.normalized;

        //Debug.Log("After Clamp: " + ultimateForce);
        ApplyForce(ultimateForce);
    }
    #endregion

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
        angleOfRotation = Mathf.Atan2(looksPos.y, looksPos.x) * Mathf.Rad2Deg-90;
    }
    #endregion

    #region Sprinting Movement Helper Methods
    public void StartSprint()
    {
        if (!playerManager.IsSprinting)
        {
            playerManager.IsSprinting = true;
            currentSpeed++;
            maxSpeed++;
            minSpeed++;
        }
    }

    public void EndSprint()
    {
        if (playerManager.IsSprinting)
        {
            playerManager.IsSprinting = false;
            currentSpeed--;
            maxSpeed--;
            minSpeed--;
        }
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
        if (currentSpeed < minSpeed)
        {
            currentSpeed = .75f;
        }

        // Don't allow speed to be too high
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = 6f;
        }
    }
    #endregion


}
