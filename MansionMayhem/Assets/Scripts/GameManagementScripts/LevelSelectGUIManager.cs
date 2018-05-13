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


    // Variables for Instructions Screen
    public List<GameObject> instructionPages;
    public List<GameObject> instructionButtons;
    public int instructionsPage;


    public Text scoreText;
    public Text levelText;

    #endregion

    // Use this for initialization
    void Start ()
    {
        // Fix for inventory if the worldCamera is not set manually.
        if (gameObject.GetComponent<Canvas>().worldCamera == null)
        {
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            gameObject.GetComponent<Canvas>().planeDistance = 100;
            gameObject.GetComponent<Canvas>().sortingLayerName = "UI";
            gameObject.GetComponent<Canvas>().sortingOrder = 2;

        }

        // Get reference to the player
        player = GameObject.FindGameObjectWithTag("player");



        escapeScreen.SetActive(false);
        Time.timeScale = 1;

        // Turn off menu screens if they are active
        if(loadoutScreen.activeSelf)
        {
            loadoutScreen.SetActive(false);
        }
        if(workbenchScreen.activeSelf)
        {
            workbenchScreen.SetActive(false);
        }


        // Set gamestate to menu just in case
        GameManager.instance.currentGameState = GameState.MainMenu;
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
        levelText.text = "Level: " + player.GetComponent<PlayerMapScript>().destinationName;
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

        // Instructions Pages
        instructionsScreen.SetActive(false);
        instructionPages[0].SetActive(true);
        instructionPages[1].SetActive(false);
        instructionButtons[0].SetActive(false);
        instructionButtons[1].SetActive(true);
        instructionsPage = 0;

        workbenchScreen.SetActive(false);
        loadoutScreen.SetActive(false);
        Time.timeScale = 1;
    }
    #endregion

    #region instructions management

    /// <summary>
    /// Handling clicking next and previous buttons for the insturction screen
    /// </summary>
    /// <param name="forward"></param>
    public void pageTurn(bool forward)
    {
        // turn off current page
        for (int i = 0; i < instructionPages.Count; i++)
        {
            if (instructionPages[instructionsPage])
            {
                instructionPages[instructionsPage].SetActive(false);
            }
        }
        if (forward)
        {
            instructionsPage++;
        }
        else
        {
            instructionsPage--;
        }

        instructionPages[instructionsPage].SetActive(true);


        // Turn off or on the next page button if needed
        if (instructionsPage < instructionButtons.Count - 1)
        {
            instructionButtons[1].SetActive(true);
        }
        else
        {
            instructionButtons[1].SetActive(false);
        }

        if (instructionsPage > 0)
        {
            instructionButtons[0].SetActive(true);
        }
        else
        {
            instructionButtons[0].SetActive(false);
        }

    }
    #endregion

}
