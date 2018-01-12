﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    #region Attributes
    public GameObject linkedDoor;
    public List<ItemType> requirements;
    public Sprite lockedSprite;
    private Sprite unlockedSprite;
    #endregion


    #region
    void Start()
    {
        // The door starts unlocked
        unlockedSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        // Check if the door was locked or not
        if (requirements.Count > 0)
        {
            // switch the sprite to an locked door
            gameObject.GetComponent<SpriteRenderer>().sprite = lockedSprite;

        }        
    }


    void Update()
    {


    }
    #endregion

    public void removeRequirement(ItemType item)
    {
        requirements.Remove(item);

        if(requirements.Count == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = unlockedSprite;
        }
    }


    #region Travel Method
    public void Travel(GameObject player)
    {
        if(requirements.Count == 0)
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
            if (linkedDoor.name == "upstairs")
            {
                player.transform.position = linkedDoor.transform.position + new Vector3(0, -.5f, 0);
            }
            if (linkedDoor.name == "downstairs")
            {
                player.transform.position = linkedDoor.transform.position + new Vector3(0, .5f, 0);
            }
        }

    }

    #endregion
}
