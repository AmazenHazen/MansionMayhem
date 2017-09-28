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
    public GameObject firstaidKit;
    public GameObject healthPotion;
    public GameObject goldenHeart;


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
            screwCount = Random.Range(25, 35);
        }

                // Spawn the screws (and different types as well
        for (int i=0; i<screwCount; i++)
        {
            screwRNG = Random.Range(0, 100);

            if (screwRNG < 80)
            {
                Instantiate(normalScrew, new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
            if (screwRNG >= 80 && screwRNG <95)
            { 
                Instantiate(redScrew, new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
            if (screwRNG >95)
            {
                Instantiate(goldScrew, new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
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
                Instantiate(heart, new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
            if (GameManager.currentLevel > 5 && GameManager.currentLevel <=15)
            {
                Instantiate(healthPotion, new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
            if (GameManager.currentLevel > 15 && GameManager.currentLevel <= 25)
            {
                Instantiate(firstaidKit, new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
            if (GameManager.currentLevel > 25)
            {
                Instantiate(goldenHeart, new Vector3(transform.position.x + Random.Range(-width, width), transform.position.y + Random.Range(-length, length), gameObject.transform.position.z), transform.rotation);
            }
        }
        #endregion

        #region Door Linking
        // Link Doors for the Room
        if (topRoom!=null)
        {
            gameObject.transform.FindChild("topdoor").gameObject.SetActive(true);
            gameObject.transform.FindChild("topdoor").GetComponent<DoorScript>().linkedDoor = topRoom.transform.FindChild("bottomdoor").gameObject;
        }
        if (bottomRoom != null)
        {
            gameObject.transform.FindChild("bottomdoor").gameObject.SetActive(true);
            gameObject.transform.FindChild("bottomdoor").GetComponent<DoorScript>().linkedDoor = bottomRoom.transform.FindChild("topdoor").gameObject;
        }
        if (leftRoom != null)
        {
            gameObject.transform.FindChild("leftdoor").gameObject.SetActive(true);
            gameObject.transform.FindChild("leftdoor").GetComponent<DoorScript>().linkedDoor = leftRoom.transform.FindChild("rightdoor").gameObject;
        }

        if (rightRoom != null)
        {
            gameObject.transform.FindChild("rightdoor").gameObject.SetActive(true);
            gameObject.transform.FindChild("rightdoor").GetComponent<DoorScript>().linkedDoor = rightRoom.transform.FindChild("leftdoor").gameObject;
        }

        if (upStairsRoom != null)
        {
            gameObject.transform.FindChild("upstairs").gameObject.SetActive(true);
            gameObject.transform.FindChild("upstairs").GetComponent<DoorScript>().linkedDoor = upStairsRoom.transform.FindChild("downstairs").gameObject;
        }

        if (downStairsRoom != null)
        {
            gameObject.transform.FindChild("downstairs").gameObject.SetActive(true);
            gameObject.transform.FindChild("downstairs").GetComponent<DoorScript>().linkedDoor = downStairsRoom.transform.FindChild("upstairs").gameObject;
        }

        #endregion
    }
}
