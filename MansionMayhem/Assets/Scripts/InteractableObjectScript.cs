using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectScript : MonoBehaviour
{
    // Attributes
    // Requirements for the interactableObject (if the object has any requirements)
    public List<ItemType> requirements;

    // Holds Items the interactable Object has for the users
    public List<ItemType> containsItems;

    // Spawns (if it has any)
    public List<GameObject> spawns;

    #region Start
    // Use this for initialization
    void Start()
    {
        // Set enemies to false at the beginning
        for (int i = 0; i < spawns.Count; i++)
        {
            spawns[i].SetActive(false);
        }
    }
    #endregion

    public void SpawnEnemies()
    {
        // Check to see if requirements are completed
        if (requirements.Count == 0)
        {
            // Activates the list of GameObjects
            for (int i = 0; i < spawns.Count; i++)
            {
                spawns[i].SetActive(true);
            }
        }

    }

    public void removeRequirement(ItemType item)
    {
        requirements.Remove(item);
    }
}
