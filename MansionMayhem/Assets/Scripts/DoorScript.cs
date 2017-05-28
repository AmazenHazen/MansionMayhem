using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    #region Attributes
    public GameObject linkedDoor;
    public List<GameObject> requirements;

    public void Travel(GameObject player)
    {
        if (requirements.Count == 0)
        {
            player.transform.position = linkedDoor.transform.position;
        }
    }

    #endregion
}
