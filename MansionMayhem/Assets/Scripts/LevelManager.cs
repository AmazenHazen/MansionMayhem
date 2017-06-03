﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Level Variables
    public levelType levelObjective;

    // Requirement Variables
    public GameObject boss;
    GameObject[] getListArray;
    public List<GameObject> levelRequirements;

    // Use this for initialization
    void Start()
    {
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
            case levelType.task:
                // Find all enemies (objects with tag enemy) and put them in the list.
                getListArray = GameObject.FindGameObjectsWithTag("task");

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
        if (levelObjective == levelType.extermination || levelObjective == levelType.task)
        {
            if (levelRequirements.Count == 0)
            {
                //Advance the level
                AdvanceLevel();
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

    void AdvanceLevel()
    {
        Debug.Log("Completed Level!");

        // Advance Level
        GameManager.currentLevel++;
        Application.LoadLevel(GameManager.currentLevel);
    }


    public void EnemyEliminated(GameObject enemy)
    {
        if(levelObjective == levelType.extermination)
        {
            levelRequirements.Remove(enemy);
        }
    }
}
