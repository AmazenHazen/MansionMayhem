using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMovement : CharacterMovement
{
    #region Attributes
    // Private reference to the player
    private GameObject player;

    private const float MAX_SPEED = 6.0f;
    private const float MIN_SPEED = .25f;

    // Bool to see if the Ally is following the player or not
    [SerializeField] private bool followingPlayer;

    [Header("Seperation Forces")]
    // Variables for seperation force
    public float seperationBubble; // basic force is 1.0
    public float seperationForce; // basic seperation force is 1.5 for enemies, 4 for allies
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
    /// <summary>
    /// Seperation Method that returns a steering force to move an enemy away from another enemy if too close.
    /// </summary>
    /// <returns></returns>
    public Vector3 Seperation()
    {
        // Create a new steering force
        Vector3 steeringForce = Vector3.zero;

        // Find nearest neighbor
        foreach (GameObject ally in LevelManager.allies)
        {
            if ((transform.position - ally.transform.position).magnitude < seperationBubble)
            {
                if ((transform.position - ally.transform.position).magnitude != 0)
                {
                    // Step 1: Find Desired Velocity
                    // This is the vector pointing from my target to my myself
                    Vector3 desiredVelocity = position - ally.transform.position;

                    // Step 2: Scale Desired to maximum speed
                    //         so I move as fast as possible
                    desiredVelocity.Normalize();
                    desiredVelocity *= seperationForce;

                    // Step 3: Calculate your Steering Force
                    steeringForce = desiredVelocity - velocity;
                }
            }

        }
        return steeringForce;
    }
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
