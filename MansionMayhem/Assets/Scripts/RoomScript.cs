using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // Room Script Attributes

    // Keeps track of doors and if they exist for a room
    public bool topDoorBool;
    public bool leftDoorBool;
    public bool rightDoorBool;
    public bool bottomDoorBool;
    public bool stairsBool;

    // Keeps track of rooms that are connected
    public GameObject topRoom;
    public GameObject bottomRoom;
    public GameObject leftRoom;
    public GameObject rightRoom;
    public GameObject stairRoom;

    void Start()
    {
        // Link Doors for the Room
        if (gameObject.transform.FindChild("topdoor")!=null)
        {
            gameObject.transform.FindChild("topdoor").GetComponent<DoorScript>().linkedDoor = topRoom.transform.FindChild("bottomdoor").gameObject;
        }
        if (gameObject.transform.FindChild("leftdoor")!=null)
        {
            gameObject.transform.FindChild("leftdoor").GetComponent<DoorScript>().linkedDoor = leftRoom.transform.FindChild("rightdoor").gameObject;
        }
        if (gameObject.transform.FindChild("rightdoor") != null)
        {
            gameObject.transform.FindChild("rightdoor").GetComponent<DoorScript>().linkedDoor = rightRoom.transform.FindChild("leftdoor").gameObject;
        }
        if (gameObject.transform.FindChild("bottomdoor") != null)
        {
            gameObject.transform.FindChild("bottomdoor").GetComponent<DoorScript>().linkedDoor = bottomRoom.transform.FindChild("topdoor").gameObject;
        }
        if (gameObject.transform.FindChild("stairs") != null)
        {
            gameObject.transform.FindChild("stairs").GetComponent<DoorScript>().linkedDoor = stairRoom.transform.FindChild("stairs").gameObject;
        }
    }
}
