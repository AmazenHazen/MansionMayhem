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

    // Variable to keep track of all the enemies in the level
    public int levelNumber;
    public GameObject[] taskNPC;
    GameObject[] getEnemyArray;
    GameObject[] getAllyArray;
    GameObject[] getBossArray;
    public static List<GameObject> enemies;
    public static List<GameObject> allies;


    // For completing the level
    public int[] levelUnlockOnCompletion;


    // Starting Variables
    GameObject LevelObjectiveGUI;
    public TextAsset levelObjectiveText;
    public string levelName;


    // Use this for initialization
    void Start()
    {
        GameManager.instance.currentLevel = levelNumber;

        //Find the Objective Text for the intro message
        LevelObjectiveGUI = GameObject.Find("HUDCanvas").transform.Find("LevelObjective").gameObject;
        LevelObjectiveGUI.transform.Find("ObjectiveHeader").GetComponent<Text>().text = levelName;
        LevelObjectiveGUI.transform.Find("ObjectiveText").GetComponent<Text>().text = levelObjectiveText.text;
        LevelObjectiveGUI.SetActive(true);

        // Create a list to hold all enemies for the level (used for seperation force in Character Movement Scripts and Elimination levels)
        getEnemyArray = GameObject.FindGameObjectsWithTag("enemy");
        getAllyArray = GameObject.FindGameObjectsWithTag("ally");
        getBossArray = GameObject.FindGameObjectsWithTag("boss");

        enemies = new List<GameObject>();
        allies = new List<GameObject>();

        for (int j = 0; j < getEnemyArray.Length; j++)
        {
            //Debug.Log(getEnemyArray[j]);
            enemies.Add(getEnemyArray[j]);
        }
        for (int j = 0; j < getBossArray.Length; j++)
        {
            enemies.Add(getBossArray[j]);
        }
        for (int j = 0; j < getAllyArray.Length; j++)
        {
            //Debug.Log(getAllyArray[j]);
            allies.Add(getAllyArray[j]);
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
                    if (enemies.Count == 0)
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


    /// <summary>
    /// Method to allow enemies to be removed from the big enemy array
    /// </summary>
    /// <param name="enemy"></param>
    public void EnemyEliminated(GameObject enemy)
    {
         enemies.Remove(enemy);
    }
}
