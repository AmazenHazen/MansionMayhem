using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLocation : MonoBehaviour
{

    // Properties
    public int level;
    private bool unlocked;
    int locationIndex;

    // Use this for initialization
    void Start()
    {
        // Lock the levels the player has not yet unlocked
        if ((GameObject.Find("GameHandler").GetComponent<GameManager>().HighestLevel + 1) < level)
        {
            unlocked = false;
        }
    }

}
