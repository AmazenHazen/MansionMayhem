using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // Room Script Attributes

    // Keeps track of doors and if they exist for a room
    public bool topDoorBool;
    public bool bottomDoorBool;
    public bool leftDoorBool;
    public bool rightDoorBool;
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
        if (topDoorBool == true)
        {
            gameObject.transform.FindChild("topdoor").gameObject.SetActive(true);
            gameObject.transform.FindChild("topdoor").GetComponent<DoorScript>().linkedDoor = topRoom.transform.FindChild("bottomdoor").gameObject;
        }
        if (leftDoorBool == true)
        {
            gameObject.transform.FindChild("leftdoor").gameObject.SetActive(true);
            gameObject.transform.FindChild("leftdoor").GetComponent<DoorScript>().linkedDoor = leftRoom.transform.FindChild("rightdoor").gameObject;
        }
        if (rightDoorBool == true)
        {
            gameObject.transform.FindChild("rightdoor").gameObject.SetActive(true);
            gameObject.transform.FindChild("rightdoor").GetComponent<DoorScript>().linkedDoor = rightRoom.transform.FindChild("leftdoor").gameObject;
        }
        if (bottomDoorBool == true)
        {
            gameObject.transform.FindChild("bottomdoor").gameObject.SetActive(true);
            gameObject.transform.FindChild("bottomdoor").GetComponent<DoorScript>().linkedDoor = bottomRoom.transform.FindChild("topdoor").gameObject;
        }
        if (stairsBool == true)
        {
            gameObject.transform.FindChild("stairs").gameObject.SetActive(true);
            gameObject.transform.FindChild("stairs").GetComponent<DoorScript>().linkedDoor = stairRoom.transform.FindChild("stairs").gameObject;
        }
    }
}
