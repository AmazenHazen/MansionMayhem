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
    #endregion

    #region Update Method
    // Update for Player
    protected override void Update()
    {
        base.Update();
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

    #region Player Movement Method
    // Player Input is handled here
    public Vector3 playerMovementInput()
    {
        Vector3 playerForce = Vector3.zero;

        // Player Movement Code
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerForce += new Vector3(0, 5, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerForce += new Vector3(-5, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerForce += new Vector3(5, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerForce += new Vector3(0, -5, 0);
        }

        // Step 3: Scale Desired to maximum speed
        //         so I move as fast as possible
        playerForce.Normalize();
        playerForce *= maxSpeed;

        return playerForce;
    }
    #endregion
}
