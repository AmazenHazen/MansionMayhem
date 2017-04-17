using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    #region Additional Movement Variables
    // Variables for enemy targeting
    public GameObject player;

    // Attributes for CalcSteeringForces Method
    public float maxForce;
    #endregion

    #region Start Method
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }
    #endregion

    #region Update Method
    protected override void Update()
    {
        base.Update();
    }
    #endregion

    #region Enemy Movement Method
    // Call the necessary Forces on the player
    protected override void CalcSteeringForces()
    {
        // Create a new ultimate force that is zeroed out
        Vector3 ultimateForce = Vector3.zero;

        // Apply forces to the player character
        ultimateForce += seek(player.transform.position);

        // Apply Decelleration using ApplyFriction Force
        //ultimateForce += ApplyFriction(3.0f);

        //Debug.Log("Before Clamp: " + ultimateForce);
        // Clamp the ultimate force by the maximum force
        Vector3.ClampMagnitude(ultimateForce, maxForce);

        // Ensure that the enemies do not move in the z-axis
        ultimateForce.z = 0;

        //Debug.Log("After Clamp: " + ultimateForce);
        ApplyForce(ultimateForce);
    }
    #endregion

}
