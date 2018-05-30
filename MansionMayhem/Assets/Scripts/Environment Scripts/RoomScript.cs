using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // Screw Attributes
    int screwCount;
    int screwRNG;

    // Health Spawn Attributes
    int healthRNG;

    // Keeps track of rooms that are connected
    public GameObject topRoom;
    public GameObject bottomRoom;
    public GameObject leftRoom;
    public GameObject rightRoom;
    public GameObject upStairsRoom;
    public GameObject downStairsRoom;

    // Dimensions (for spawning items)
    public float length;
    public float width;

    // Type of room
    public RoomType roomType;

    void Start()
    {

        #region Door Linking
        // Link Doors for the Room
        if (topRoom != null)
        {
            gameObject.transform.Find("topdoor").gameObject.SetActive(true);
            gameObject.transform.Find("topdoor").GetComponent<DoorScript>().linkedDoor = topRoom.transform.Find("bottomdoor").gameObject;
        }
        if (bottomRoom != null)
        {
            gameObject.transform.Find("bottomdoor").gameObject.SetActive(true);
            gameObject.transform.Find("bottomdoor").GetComponent<DoorScript>().linkedDoor = bottomRoom.transform.Find("topdoor").gameObject;
        }
        if (leftRoom != null)
        {
            gameObject.transform.Find("leftdoor").gameObject.SetActive(true);
            gameObject.transform.Find("leftdoor").GetComponent<DoorScript>().linkedDoor = leftRoom.transform.Find("rightdoor").gameObject;
        }

        if (rightRoom != null)
        {
            gameObject.transform.Find("rightdoor").gameObject.SetActive(true);
            gameObject.transform.Find("rightdoor").GetComponent<DoorScript>().linkedDoor = rightRoom.transform.Find("leftdoor").gameObject;
        }

        if (upStairsRoom != null)
        {
            gameObject.transform.Find("upstairs").gameObject.SetActive(true);
            gameObject.transform.Find("upstairs").GetComponent<DoorScript>().linkedDoor = upStairsRoom.transform.Find("downstairs").gameObject;
        }

        if (downStairsRoom != null)
        {
            gameObject.transform.Find("downstairs").gameObject.SetActive(true);
            gameObject.transform.Find("downstairs").GetComponent<DoorScript>().linkedDoor = downStairsRoom.transform.Find("upstairs").gameObject;
        }

        #endregion

        #region Screw Spawning
        // Determine how many screws in the room
        if (roomType == RoomType.small)
        {
            screwCount = Random.Range(5, 15);
        }
        else if (roomType == RoomType.medium)
        {
            screwCount = Random.Range(15, 20);
        }
        else if (roomType == RoomType.large)
        {
            screwCount = Random.Range(35, 50);
        }
        else if (roomType == RoomType.ExtremelyLarge)
        {
            screwCount = Random.Range(70, 100);
        }


        // Spawn the screws (and different types as well
        for (int i=0; i<screwCount; i++)
        {
            screwRNG = Random.Range(0, 100);

            if (screwRNG < 80)
            {
                Instantiate(GameManager.screwItems[0], new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), 0), transform.rotation);
            }
            if (screwRNG >= 80 && screwRNG <95)
            { 
                Instantiate(GameManager.screwItems[1], new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), 0), transform.rotation);
            }
            if (screwRNG >95)
            {
                Instantiate(GameManager.screwItems[2], new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), 0), transform.rotation);
            }
        }
        #endregion

        #region Health Item Spawns
        // Determine if a health item spawns in the room
        healthRNG = Random.Range(0, 10);
        
        // If a health item does spawn in the room spawn it (based on current level)
        if(healthRNG > 8)
        {
            if(GameManager.instance.currentLevel<=5)
            {
                Instantiate(GameManager.recoveryItems[0], new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
            if (GameManager.instance.currentLevel > 5 && GameManager.instance.currentLevel <= 15)
            {
                Instantiate(GameManager.recoveryItems[1], new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
            if (GameManager.instance.currentLevel > 15 && GameManager.instance.currentLevel <= 25)
            {
                Instantiate(GameManager.recoveryItems[2], new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
            if (GameManager.instance.currentLevel > 25)
            {
                Instantiate(GameManager.recoveryItems[3], new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
        }
        #endregion
    }
}
