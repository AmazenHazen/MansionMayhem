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
            if (linkedDoor.name == "topdoor")
            {
                player.transform.position = linkedDoor.transform.position + new Vector3(0, -.5f, 0);
            }
            if (linkedDoor.name == "bottomdoor")
            {
                player.transform.position = linkedDoor.transform.position + new Vector3(0, .5f, 0);
            }
            if (linkedDoor.name == "leftdoor")
            {
                player.transform.position = linkedDoor.transform.position + new Vector3(.5f, 0, 0);
            }
            if (linkedDoor.name == "rightdoor")
            {
                player.transform.position = linkedDoor.transform.position + new Vector3(-.5f, 0, 0);
            }
        }
    }

    #endregion
}
