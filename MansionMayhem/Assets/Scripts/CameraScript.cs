using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject player;
    Vector3 cameraLoc;

    #region Start Method
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }

    public void Update()
    {
        FollowPlayer();
    }
    #endregion

    #region Helper Methods
    private void FollowPlayer()
    {
        cameraLoc = player.transform.position;
        cameraLoc.z = player.transform.position.z-10;
        transform.position = cameraLoc;
    }
    #endregion
}
