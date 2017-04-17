using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // Room Script Attributes
    // Keeps track of doors
    public GameObject topDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject bottomDoor;
    public GameObject stairs;

    // Keeps track of doors
    public bool topDoorBool;
    public bool leftDoorBool;
    public bool rightDoorBool;
    public bool bottomDoorBool;
    public bool stairsBool;

    // Keeps track of rooms that are connected
    public int topRoom;
    public int bottomRoom;
    public int leftRoom;
    public int rightRoom;

    void Start()
    {

    }

}
