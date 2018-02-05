using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelSelectGUIManager : MonoBehaviour {

    #region Attributes
    private GameObject player;

    // Variables for escape screen
    public GameObject escapeScreen;
    public GameObject instructionsScreen;
    public GameObject workbenchScreen;
    public GameObject loadoutScreen;


    public Text scoreText;
    public Text levelText;

    #endregion

    // Use this for initialization
    void Start ()
    {
        // Get reference to the player
        player = GameObject.FindGameObjectWithTag("player");

        escapeScreen.SetActive(false);
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
        TextUpdate();
        EscapeScreenManagement();	
	}

    #region Text Management
    void TextUpdate()
    {
        // Get variables needed for the HUD Text
        scoreText.text = "Screws: " + GameManager.instance.screws;
        levelText.text = "Level: " + player.GetComponent<PlayerMapScript>().Destination.GetComponent<LevelLocation>().name;
    }
    #endregion

    #region Escape Screen Management
    public void EscapeScreenManagement()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) && (escapeScreen.activeSelf == true || loadoutScreen.activeSelf == true || workbenchScreen.activeSelf == true) && (GameManager.instance.currentGameState == GameState.MainMenu || GameManager.instance.currentGameState == GameState.Paused)))
        {
            ContinueGame();
        }
        else if ((Input.GetKeyDown(KeyCode.Escape)) && (escapeScreen.activeSelf == false && loadoutScreen.activeSelf == false && workbenchScreen.activeSelf == false) && (GameManager.instance.currentGameState == GameState.MainMenu))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        escapeScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void ContinueGame()
    {
        escapeScreen.SetActive(false);
        instructionsScreen.SetActive(false);
        workbenchScreen.SetActive(false);
        loadoutScreen.SetActive(false);
        Time.timeScale = 1;
    }
    #endregion

}
