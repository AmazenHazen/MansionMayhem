using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMovement : CharacterMovement
{
    #region Attributes
    // Private reference to the player
    private GameObject player;

    // Attributes for CalcSteeringForces Method
    public float maxForce;

    // Bool to see if the Ally is following the player or not
    [SerializeField] private bool followingPlayer;
    #endregion

    #region Properties
    public bool FollowingPlayer
    {
        get { return followingPlayer; }
        set { followingPlayer = value; }
    }
    #endregion

    public override void Start()
    {
        // Find the player game object
        player = GameObject.FindGameObjectWithTag("player");
    }


    #region Update Method
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    #endregion

    #region Movement Methods
    #region CalcSteerForce
    // Call the necessary Forces on the player
    protected override void CalcSteeringForces()
    {
        // Create a new ultimate force that is zeroed out
        Vector3 ultimateForce = Vector3.zero;

        // Rotate the facing of the Ally if the player is close enough or if an enemy is close enough
        if ((player.transform.position - transform.position).magnitude < awareDistance
            || (FindClosestEnemy().transform.position - transform.position).magnitude < awareDistance)
        {
            Rotate();
        }

        // Seek the followpoint if there is a big distance between the player and the follower
        if (followingPlayer && (player.transform.position - transform.position).magnitude >.25f)
        {
            ultimateForce += seek(player.transform.position - player.transform.up);
            ultimateForce += Seperation();
        }

        //Debug.Log("Before Clamp: " + ultimateForce);
        // Clamp the ultimate force by the maximum force
        Vector3.ClampMagnitude(ultimateForce, maxForce);

        // Ensure that the enemies do not move in the z-axis
        ultimateForce.z = 0;

        //Debug.Log("After Clamp: " + ultimateForce);
        ApplyForce(ultimateForce);
    }
    #endregion

    #region Ally Rotate
    /// <summary>
    /// Rotates the player based on the direction its facing
    /// </summary>
    protected override void Rotate()
    {

        // Set the target position to zero so the ally doesn't rotate if conditions are not met
        Vector3 targetPosition = Vector3.zero;

        // Change the rotation depending on if the ally if following the player or not
        if (followingPlayer && (FindClosestEnemy().transform.position - transform.position).magnitude < awareDistance)
        {
            targetPosition = FindClosestEnemy().transform.position;
        }
        else
        {
            targetPosition = player.transform.position;
        }

        Vector3 dir = targetPosition - transform.position;
        angleOfRotation = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
    }
    #endregion

    #region Revert Speed Method for Allies
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

    #region otherHelperMethods
    /// <summary>
    /// Finds the closest enemy and returns it to whatever calls this method
    /// </summary>
    /// <returns>The Closest Enemy</returns>
    public GameObject FindClosestEnemy()
    {
        // Find the closest enemy
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        float currentDistance = Mathf.Infinity;

        // Loop through all of the enemies in the scene
        foreach (GameObject enemy in LevelManager.enemies)
        {
            currentDistance = (enemy.transform.position - transform.position).magnitude;

            if (currentDistance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = currentDistance;
            }
        }

        return closestEnemy;
    }
    #endregion
}
