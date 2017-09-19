using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : CharacterMovement
{
    #region Attributes
    // Attributes
    public string name;
    public GameObject player;
    public float awareDistance;

    #endregion

    #region Start Method
    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    #endregion

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
}
