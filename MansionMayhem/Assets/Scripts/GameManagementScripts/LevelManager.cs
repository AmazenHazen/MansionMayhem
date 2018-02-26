﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Level Variables
    public levelType levelObjective;

    // Requirement Variables
    public GameObject boss;
    public GameObject taskNPC;
    GameObject[] getListArray;
    public List<GameObject> levelRequirements;


    // Starting Variables
    public TextAsset levelObjectiveText;
    public string levelName;


    // Use this for initialization
    void Start()
    {
        //Find the 
        GameObject.Find("ObjectiveHeader").GetComponent<Text>().text = levelName;
        GameObject.Find("ObjectiveText").GetComponent<Text>().text = levelObjectiveText.text;


        // Sets the initial case for level
        switch (levelObjective)
        {
            case levelType.extermination:
                // Find all enemies (objects with tag enemy) and put them in the list.
                getListArray = GameObject.FindGameObjectsWithTag("enemy");

                for (int i = 0; i < getListArray.Length; i++)
                {
                    levelRequirements.Add(getListArray[i]);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckLevelCompletion();
    }

    void CheckLevelCompletion()
    {
        // Case: Extermination or task requires the requirement list to be empty or 0
        if (levelObjective == levelType.extermination)
        {
            if (levelRequirements.Count == 0)
            {
                //Advance the level
                AdvanceLevel();
            }
        }
        else if(levelObjective == levelType.task)
        {
            if(taskNPC.gameObject.GetComponent<NPC>())
            {
                if(taskNPC.gameObject.GetComponent<NPC>().CurrentQuestStatus == QuestStatus.Completed)
                {
                    AdvanceLevel();
                }
            }
        }
        else if (levelObjective == levelType.boss)
        {
            if (boss == null)
            {
                AdvanceLevel();
            }
        }
    }

    /// <summary>
    /// Is called when the player completes the level, automatically reloads the level selection screen
    /// </summary>
    void AdvanceLevel()
    {
        Debug.Log("Completed Level!");

        // Advance Level
        // Set highest level
        if (GameManager.instance)
        {
            // Increase the highest level
            if (GameManager.instance.currentLevel == GameManager.instance.highestLevel)
            {
                GameManager.instance.highestLevel++;
            }
        }
        // Sets the instance to win, HUI manager handles the rest
        GameManager.instance.currentGameState = GameState.CompleteLevel;

    }


    public void EnemyEliminated(GameObject enemy)
    {
        if(levelObjective == levelType.extermination)
        {
            levelRequirements.Remove(enemy);
        }
    }
}
