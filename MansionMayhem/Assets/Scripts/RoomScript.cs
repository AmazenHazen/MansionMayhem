using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // Screw Attributes
    public GameObject normalScrew;
    public GameObject redScrew;
    public GameObject goldScrew;
    int screwCount;
    int screwRNG;

    // Health Spawn Attributes
    int healthRNG;
    public GameObject heart;
    public GameObject healthPotion;
    public GameObject fairyDust;
    public GameObject goldenHeart;


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

        #region Screw Spawning
        // Determine how many screws in the room
        screwCount = Random.Range(5, 15);
        
        // Spawn the screws (and different types as well
        for (int i=0; i<screwCount; i++)
        {
            screwRNG = Random.Range(0, 100);

            if (screwRNG < 80)
            {
                Instantiate(normalScrew, new Vector3(transform.position.x + Random.Range(-3.0f, 3.0f), transform.position.y + Random.Range(-2.0f, 2.0f), 0), transform.rotation);
            }
            if (screwRNG >= 80 && screwRNG <95)
            { 
                Instantiate(redScrew, new Vector3(transform.position.x + Random.Range(-3.0f, 3.0f), transform.position.y + Random.Range(-2.0f, 2.0f), 0), transform.rotation);
            }
            if (screwRNG >95)
            {
                Instantiate(goldScrew, new Vector3(transform.position.x + Random.Range(-3.0f, 3.0f), transform.position.y + Random.Range(-2.0f, 2.0f), 0), transform.rotation);
            }
        }
        #endregion

        #region Health Item Spawns
        // Determine if a health item spawns in the room
        healthRNG = Random.Range(0, 10);
        
        // If a health item does spawn in the room spawn it (based on current level)
        if(healthRNG > 8)
        {
            if(GameManager.currentLevel<=5)
            {
                Instantiate(heart, new Vector3(transform.position.x + Random.Range(-3.0f, 3.0f), transform.position.y + Random.Range(-2.0f, 2.0f), 0), transform.rotation);
            }
            if (GameManager.currentLevel > 5 && GameManager.currentLevel <=15)
            {
                Instantiate(healthPotion, new Vector3(transform.position.x + Random.Range(-3.0f, 3.0f), transform.position.y + Random.Range(-2.0f, 2.0f), 0), transform.rotation);
            }
            if (GameManager.currentLevel > 15 && GameManager.currentLevel <= 25)
            {
                Instantiate(fairyDust, new Vector3(transform.position.x + Random.Range(-3.0f, 3.0f), transform.position.y + Random.Range(-2.0f, 2.0f), 0), transform.rotation);
            }
            if (GameManager.currentLevel > 25)
            {
                Instantiate(goldenHeart, new Vector3(transform.position.x + Random.Range(-3.0f, 3.0f), transform.position.y + Random.Range(-2.0f, 2.0f), 0), transform.rotation);
            }
        }
        #endregion

        #region Door Linking
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
        #endregion
    }
}
