using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Level Variables
    public levelType[] levelObjective;

    // Requirement Variables
    public GameObject[] boss;
    public GameObject[] taskNPC;
    GameObject[] getListArray;
    public List<GameObject> levelRequirements;

    // For completing the level
    public int[] levelUnlockOnCompletion;


    // Starting Variables
    public TextAsset levelObjectiveText;
    public string levelName;


    // Use this for initialization
    void Start()
    {
        //Find the 
        GameObject.Find("ObjectiveHeader").GetComponent<Text>().text = levelName;
        GameObject.Find("ObjectiveText").GetComponent<Text>().text = levelObjectiveText.text;

        for (int i = 0; i < levelObjective.Length; i++)
        {
            // Sets the initial case for level
            if (levelObjective[i] == levelType.extermination)
            {
                getListArray = GameObject.FindGameObjectsWithTag("enemy");

                for (int j = 0; j < getListArray.Length; j++)
                {
                    levelRequirements.Add(getListArray[j]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckLevelCompletion();
    }


    /// <summary>
    /// Method that checks whether the level has been completed
    /// </summary>
    void CheckLevelCompletion()
    {
        for (int i = 0; i < levelObjective.Length; i++)
        {
            switch (levelObjective[i])
            {
                // Case: Extermination or task requires the requirement list to be empty or 0
                case levelType.extermination:
                    if (levelRequirements.Count == 0)
                    {
                        //Advance the level
                        CompleteLevel(levelUnlockOnCompletion[i]);
                    }
                    break;


                // Case: Task/Quest is completed
                case levelType.task:
                    if (taskNPC[i].GetComponent<NPC>())
                    {
                        if (taskNPC[i].GetComponent<NPC>().CurrentQuestStatus == QuestStatus.Completed)
                        {
                            //Advance the level
                            CompleteLevel(levelUnlockOnCompletion[i]);
                        }
                    }
                    break;

                // Case: Boss is killed
                case levelType.boss:

                    if (boss[i] == null)
                    {
                        CompleteLevel(levelUnlockOnCompletion[i]);
                    }
                    break;

            }
        }
    }
    

    /// <summary>
    /// Is called when the player completes the level, automatically reloads the level selection screen
    /// </summary>
    void CompleteLevel(int unlockLevel)
    {
        //Debug.Log("Completed Level!");

        // Advance Level
        // Set highest level
        if (GameManager.instance)
        {
            // Increase the highest level
            if (GameManager.instance.unlockedLevels[unlockLevel]==false)
            {
                // Unlock the level in the gameManager instance
                GameManager.instance.unlockedLevels[unlockLevel] = true;

                // Set the highest level to the newest level reached
                if (unlockLevel < GameManager.instance.highestLevel)
                {
                    GameManager.instance.highestLevel = unlockLevel;
                }
            }
        }
        // Sets the instance to win, HUI manager handles the rest
        GameManager.instance.currentGameState = GameState.CompleteLevel;

    }


    public void EnemyEliminated(GameObject enemy)
    {
        for (int i = 0; i < levelObjective.Length; i++)
        {
            // Sets the initial case for level
            if (levelObjective[i] == levelType.extermination)
            {
                levelRequirements.Remove(enemy);
            }
        }
    }
}
