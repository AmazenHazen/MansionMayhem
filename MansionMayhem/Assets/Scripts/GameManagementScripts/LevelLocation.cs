using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLocation : MonoBehaviour
{

    // Properties
    public string name;
    public int level;
    public bool unlocked;
    int locationIndex;

    // Use this for initialization
    void Start()
    {
        // Lock the levels the player has not yet unlocked
        if (GameManager.instance != null)
        {
            // Unlock the all the levels below the highest level and the next level
            if ((GameManager.instance.unlockedLevels[level]))
            {
                unlocked = true;
            }
            else
            {
                unlocked = false;
                gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                if(gameObject.GetComponent<ParticleSystem>())
                {
                    gameObject.GetComponent<ParticleSystem>().startColor = Color.black;
                }
            }
        }
        else
        {
            unlocked = true;
        }
    }

}
