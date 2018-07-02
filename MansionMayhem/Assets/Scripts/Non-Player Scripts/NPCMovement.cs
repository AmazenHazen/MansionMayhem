using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : CharacterMovement
{
    private GameObject player;

    private const float MAX_SPEED = 6.0f;
    private const float MIN_SPEED = .25f;

    public override void Start()
    {
        // Find the player game object
        player = GameObject.FindGameObjectWithTag("player");
    }

    #region Movement Methods
    #region CalcSteerForce
    // Call the necessary Forces on the NPC
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
    /// <summary>
    /// Method that rotates the NPC to look at/face the player
    /// </summary>
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
        if (currentSpeed < MAX_SPEED && beingSlowed == false)
        {
            currentSpeed += .05f;
        }

        //Reset speed if on slippery surface
        if (currentSpeed > MAX_SPEED && beingSped == false)
        {
            currentSpeed -= .05f;
        }

        // Don't allow speed to be negative or 0
        if (currentSpeed < MIN_SPEED)
        {
            currentSpeed = .25f;
        }

        // Don't allow speed to be too high
        if (currentSpeed > MAX_SPEED)
        {
            currentSpeed = 6f;
        }
    }
    #endregion
    #endregion
}
