﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GUIManager : MonoBehaviour
{

    #region Attributes
    private PlayerManager player;

    // Health Management
    int colorIndex;
    private float health;
    private List<Color> HealthColors;
    public GameObject HealthUI;
    private List<Slider> HeartContainers;
    private List<Image> HeartContainersFill;
    private List<Image> HeartContainersBackground;

    private float stanima;
    public GameObject StanimaUI;
    private List<Slider> StanimaContainers;
    private List<Image> StanimaContainersFill;

    public GameObject SoulStoneCollectibleIndicator;

    // Inventory Management
    public GameObject InventoryPanel;
    public GameObject[] InventoryItems;
    bool minimized;

    // Boss Health Management
    public GameObject boss;
    public GameObject bossHealthCanvas;
    public Slider bossHealthBar; 
    public float bossMaxHealth;
    public float bossCurrentHealth;
    public static bool bossFight;
    public Text bossText;


    // Variables for changing Text
    public Text rangeWeaponText;
    rangeWeapon currentRangeWeapon;
    public Text scoreText;
    public Text levelText;
    public Text experienceText;

    // Variables for escape screen
    public GameObject escapeScreen;

    // Variables for the level Objective Screen
    public GameObject objectiveScreen;

    // Variables for Instructions Screen
    public GameObject instructionsScreen;

    // Variables for Death Screen
    public GameObject deathScreen;

    // Variables for Win Screen
    public GameObject winScreen;

    // Variables for talking to NPC/Workbench
    public static bool usingOtherInterface;

    // Reference to the GUI
    public static GameObject dialogBox;
    public static GameObject dialogText;
    public static List<GameObject> Options;

    // Scrolling Autotyping variables
    public static bool isTyping = false;
    public static bool cancelTyping = false;
    private static float typeSpeed = 0.01f;

    public static bool talkingBool;
    public static bool optionBool;

    // FPS variables
    public Text FPSText;
    float deltaTime = 0.0f;
    #endregion

    #region Start
    // Start is called when the GUI is initialized
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();

        // Fix for inventory if the worldCamera is not set manually.
        if (gameObject.GetComponent<Canvas>().worldCamera == null)
        {
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            gameObject.GetComponent<Canvas>().planeDistance = 100;
            gameObject.GetComponent<Canvas>().sortingLayerName = "UI";
            gameObject.GetComponent<Canvas>().sortingOrder = 2;

        }

        // Health Management
        HealthColors = new List<Color>();
        HealthColors.Add(new Color32(0, 0, 0, 255));
        HealthColors.Add(new Color32(255, 0, 0, 255));
        HealthColors.Add(new Color32(255, 215, 0, 255));

        HeartContainers = new List<Slider>();
        HeartContainersFill = new List<Image>();
        HeartContainersBackground = new List<Image>();
        for(int i=0; i<15; i++)
        {
            HeartContainers.Add(HealthUI.transform.GetChild(i).GetComponent<Slider>());
            HeartContainersBackground.Add(HeartContainers[i].transform.GetChild(0).GetComponent<Image>());
            HeartContainersFill.Add(HeartContainers[i].transform.GetChild(1).GetComponent<Image>());
        }

        // hide containers if not unlocked
        if (GameManager.instance.healthTotal<15)
        {
            for (int i = GameManager.instance.healthTotal; i < 15; i++)
            {
                HeartContainers[i].gameObject.SetActive(false);
            }
        }

        StanimaContainers = new List<Slider>();
        StanimaContainersFill = new List<Image>();
        for (int i = 0; i < 5; i++)
        {
            StanimaContainers.Add(StanimaUI.transform.GetChild(i).GetComponent<Slider>());
            StanimaContainersFill.Add(StanimaContainers[i].transform.GetChild(1).GetComponent<Image>());
        }

        // hide containers if not unlocked
        if (GameManager.instance.stanimaTotal < 5)
        {
            for (int i = GameManager.instance.stanimaTotal; i < 5; i++)
            {
                StanimaContainers[i].gameObject.SetActive(false);
            }
        }

        /*
        HealthColors.Add(new Color(186, 0, 186));
        HealthColors.Add(new Color(186, 186, 186));
        HealthColors.Add(new Color(186, 186, 186));
        */

        // Inventory Management
        usingOtherInterface = false;

        PauseGame();
        minimized = false;
        ManageInventoryMenu();

        // Reference to the GUI
        // Get the dialog boxes for dialog
        dialogBox = GameObject.Find("DialogBox");
        dialogText = dialogBox.transform.Find("DialogText").gameObject;

        // Get the options for option dialog
        Options = new List<GameObject>();
        optionBool = false;
        for (int i = 0; i < 5; i++)
        {
            Options.Add(dialogBox.transform.Find("Options").GetChild(i).gameObject);
        }

        // turn off the dialog box if it is on
        if (GameObject.Find("DialogBox"))
        {
            GameObject.Find("DialogBox").SetActive(false);
        }

        // turn off the death screen if active
        if (deathScreen.activeSelf)
        {
            deathScreen.SetActive(false);
        }

        // Turn off the win screen if active
        if (winScreen.activeSelf)
        {
            winScreen.SetActive(false);
        }

        // instruction page variables
        instructionsScreen.GetComponent<InstructionsManagement>().instructionsPage = 0;

        // set boss fight to false
        bossFight = false;

        Debug.Log("Current Level: " + GameManager.instance.currentLevel);
        if(!GameManager.instance.soulStones[GameManager.instance.currentLevel])
        {
            SoulStoneCollectibleIndicator.GetComponent<Image>().color = Color.black;
        }
    }

    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        HealthManagement();
        StanimaManagement();
        SoulStoneCollectibleManagement();
        BossHealthManagement();
        TextUpdate();
        EscapeScreenManagement();

        // To display FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        

        if (GameManager.instance.currentGameState == GameState.Died)
        {
            DeathManagement();
        }
        if (GameManager.instance.currentGameState == GameState.CompleteLevel)
        {
            WinManagement();
        }
    }
    #endregion

    #region Health Management
    void HealthManagement()
    {
        // Health Management
        health = player.CurrentHealth;

        HeartManagement();

    }

    /// <summary>
    /// Colors the hearts correctly based on how much health you have
    /// </summary>
    void HeartManagement()
    {
        int heartIndex = (int)(health % 15);

        // Color determination
        colorIndex = (int)((health / 15));

        // Sets color of hearts to prev color
        for (int i = 0; i < 15; i++)
        {
            // Set the background color
            HeartContainersBackground[i].color = HealthColors[colorIndex];
            // Unfills the hearts to see the background
            HeartContainers[i].GetComponent<Slider>().value = 0;
        }

        if (health > 0)
        {
            // Sets color of rest of hearts to current color
            for (int i = 0; i < heartIndex; i++)
            {
                // Set the fill to the current heart color
                HeartContainersFill[i].color = HealthColors[colorIndex + 1];
                // Fills the hearts
                HeartContainers[i].value = 1.0f;
            }

            // Set the fill value
            HeartContainers[heartIndex].value = health - Mathf.Floor(health);
        }
        
    }
    #endregion

    #region Stanima Management
    void StanimaManagement()
    {
        // Stanima Management
        stanima = player.CurrentStanima;

        StanimaCircleManagement();
    }

    /// <summary>
    /// Manage the stanima containers
    /// </summary>
    void StanimaCircleManagement()
    {
        int stanimaIndex = (int)(stanima % 5);

        // Sets Stanima circles to unfilled
        for (int i = 0; i < 5; i++)
        {
            // Unfills the stanima gauges before this one
            StanimaContainers[i].GetComponent<Slider>().value = 0;


            // Set the fill to whether or not the player is worn out
            if (player.WornOut)
            {
                StanimaContainersFill[i].color = Color.gray;
            }
            else
            {
                StanimaContainersFill[i].color = Color.green;
            }
        }
        if (stanima > 0)
        {
            // Sets the Stanima circles to full
            for (int i = 0; i < stanimaIndex; i++)
            {
                // Fills the hearts
                StanimaContainers[i].value = 1.0f;
            }

            // Set the fill value
            StanimaContainers[stanimaIndex].value = stanima - Mathf.Floor(stanima);
        }
    }
    #endregion


    #region Collectible Management
    /// <summary>
    /// Manages the SoulStone Collectible Icon
    /// </summary>
    void SoulStoneCollectibleManagement()
    {
        if (GameManager.instance.soulStones[GameManager.instance.currentLevel])
        {
            if (SoulStoneCollectibleIndicator.GetComponent<Image>().color == Color.black)
            {
                SoulStoneCollectibleIndicator.GetComponent<Image>().color = Color.white;
            }
        }
    }
    #endregion

    #region Text Management
    void TextUpdate()
    {
        scoreText.text = "x " + GameManager.instance.screws;

        // Get variables needed for the HUD Text
        if (GameManager.DebugMode)
        {
            // Create the text for weapons
            currentRangeWeapon = player.CurrentRangeWeapon;
            string weaponString;

            switch (currentRangeWeapon)
            {
                case rangeWeapon.laserpistol:
                    weaponString = "Laser Pistol";
                    break;
                case rangeWeapon.aetherLightBow:
                    weaponString = "AetherLight Bow";
                    break;
                case rangeWeapon.antiEctoPlasmator:
                    weaponString = "Anti-Ectoplasm Splatter Gun";
                    break;
                case rangeWeapon.AntimatterParticle:
                    weaponString = "Anti-Matter Particle Emitter";
                    break;
                case rangeWeapon.CelestialRepeater:
                    weaponString = "Celestial Repeater";
                    break;
                case rangeWeapon.cryoGun:
                    weaponString = "Frostweaver";
                    break;
                case rangeWeapon.DarkEnergyRifle:
                    weaponString = "Dark Energy Rifle";
                    break;
                case rangeWeapon.ElectronSeeker:
                    weaponString = "Electron Pulser";
                    break;
                case rangeWeapon.flamethrower:
                    weaponString = "Flamethrower";
                    break;
                case rangeWeapon.hellfireshotgun:
                    weaponString = "Hellfire Shotgun";
                    break;
                case rangeWeapon.PlasmaCannon:
                    weaponString = "Plasma Charger";
                    break;
                case rangeWeapon.soundCannon:
                    weaponString = "Sound Cannon";
                    break;
                case rangeWeapon.XenonPulser:
                    weaponString = "Xenon Pulser";
                    break;
                default:
                    weaponString = "No Weapon";
                    break;
            }
            rangeWeaponText.text = "Current Weapon: " + weaponString;


            // Print out the level number
            levelText.text = "Level: " + (GameManager.instance.currentLevel + 1);

            // Print out Experience Number
            experienceText.text = "Experience: " + (GameManager.instance.experience);

            // Print out the FPS
            FPSText.text = "FPS: " + 1.0f/deltaTime;
        }
        else
        {
            levelText.text = "";
            rangeWeaponText.text = "";
            experienceText.text = "";
            FPSText.text = "";

        }

        if (bossFight && boss!=null)
        {
            bossText.text = boss.name + ": ";
        }
    }
    #endregion
    
    #region Boss HealthBar Management
    public void BossHealthSetUp(GameObject bossVar)
    {
        boss = bossVar;
        // Set the healthBar Max value
        bossMaxHealth = boss.GetComponent<EnemyManager>().maxHealth;
        bossHealthBar.maxValue = bossMaxHealth;

        // Turn on the boss health bar
        bossHealthCanvas.gameObject.SetActive(true);
    }

    void BossHealthManagement()
    {
        if (boss != null && boss.activeSelf == true && bossFight == true)
        {
            //Debug.Log("in boss health update");
            bossHealthCanvas.gameObject.SetActive(true);

            // Get current health and update the bar
            bossCurrentHealth = boss.GetComponent<EnemyManager>().CurrentHealth;
            bossHealthBar.value = bossCurrentHealth;
        }
        else
        {
            bossHealthCanvas.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Escape Screen Management
    public void EscapeScreenManagement()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) && escapeScreen.activeSelf == true) && GameManager.instance.currentGameState == GameState.Paused)
        {
            ContinueGame();
        }
        else if((Input.GetKeyDown(KeyCode.Space) && objectiveScreen.activeSelf == true) && GameManager.instance.currentGameState == GameState.Paused)
        {
            ContinueGame();
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && escapeScreen.activeSelf == false) && GameManager.instance.currentGameState != GameState.Paused)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        escapeScreen.SetActive(true);
        GameManager.instance.currentGameState = GameState.Paused;
        Time.timeScale = 0;
    }
    public void ContinueGame()
    {
        escapeScreen.SetActive(false);

        // Instructions Pages
        instructionsScreen.SetActive(false);
        instructionsScreen.GetComponent<InstructionsManagement>().instructionPages[0].SetActive(true);
        instructionsScreen.GetComponent<InstructionsManagement>().instructionPages[1].SetActive(false);
        instructionsScreen.GetComponent<InstructionsManagement>().instructionButtons[0].SetActive(false);
        instructionsScreen.GetComponent<InstructionsManagement>().instructionButtons[1].SetActive(true);
        instructionsScreen.GetComponent<InstructionsManagement>().instructionsPage = 0;

        objectiveScreen.SetActive(false);
        GameManager.instance.currentGameState = GameState.Play;
        Time.timeScale = 1;
    }
    #endregion

    #region Inventory Management
    public bool AddItemGUI(GameObject item)
    {
        // Loop through the inventory
        for (int i = 0; i < InventoryItems.Length; i++)
        {
            // If there is an empty spot add the item
            if (InventoryItems[i].GetComponent<SpriteRenderer>().sprite == null)
            {
                // Set the inventory space to that item
                InventoryItems[i].GetComponent<SpriteRenderer>().sprite = item.GetComponent<SpriteRenderer>().sprite;
                InventoryItems[i].GetComponent<SpriteRenderer>().transform.localScale = item.gameObject.transform.localScale * 200;

                // Debug
                //Debug.Log("Item Added to GUI " + item);

                // Return true
                return true;
            }
        }

        return false;
    }
    public void RemoveItemGUI(int inventoryLocation)
    {
        // Set the inventory space to that item
        InventoryItems[inventoryLocation].GetComponent<SpriteRenderer>().sprite = null;
    }
    public void ManageInventoryMenu()
    {
        if (minimized)
        {
            minimized = false;
            InventoryPanel.GetComponent<RectTransform>().Translate(new Vector3(0, 4.75f, 0));
        }
        else
        {
            minimized = true;
            InventoryPanel.GetComponent<RectTransform>().Translate(new Vector3(0, -4.75f, 0));
        }
    }
    #endregion

    #region Debugging section
    /*
    void OnGUI()
    {
        GUI.Label(new Rect(100, 10, 400, 50), "Health: " + health);
    }
    */
    #endregion

    #region Death Management
    private void DeathManagement()
    {
        bossFight = false;

        if(!deathScreen.activeSelf)
        {
            deathScreen.SetActive(true);
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Same as Exit Level method in "Load On Click" Script
            GameManager.instance.currentGameState = GameState.MainMenu;

            // Check to make sure the game manager exists
            if (GameManager.instance != null)
            {
                // First Save Game
                GameManager.instance.Save();
            }

            // Exit Level
            SceneManager.LoadScene(1);
        }
    }
    #endregion

    #region WinManagement
    private void WinManagement()
    {
        bossFight = false;

        if (!deathScreen.activeSelf)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Increase screw count 
            GameManager.instance.screws += 150;

            // Same as Exit Level method in "Load On Click" Script
            GameManager.instance.currentGameState = GameState.MainMenu;

            // Check to make sure the game manager exists
            if (GameManager.instance != null)
            {
                // First Save Game
                GameManager.instance.Save();
            }

            // Exit Level
            SceneManager.LoadScene(1);
        }
    }
    #endregion

    #region dialogue Management
    /// <summary>
    /// Method to turn on the dialog box (also freezes game time)
    /// </summary>
    public static void TurnOnDialogBox()
    {
        // Pause the gameplay
        // Set pauseGame to true
        GameManager.instance.currentGameState = GameState.Paused;
        GUIManager.usingOtherInterface = true;
        Time.timeScale = 0;

        // Activate the dialog box
        GUIManager.dialogBox.SetActive(true);
    }

    /// <summary>
    /// Method to turn off the dialog box (also unfreezes game time)
    /// </summary>
    public static void TurnOffDialogBox()
    {

        // Return the Game World to normal time
        // Pause the gameplay
        // Set pauseGame to true
        GameManager.instance.currentGameState = GameState.Play;
        GUIManager.usingOtherInterface = false;
        Time.timeScale = 1;

        // Set the dialog box off
        GUIManager.dialogBox.SetActive(false);
    }

    /// <summary>
    /// Method that allows for text scrolling
    /// </summary>
    /// <param name="lineOfText"></param>
    /// <returns></returns>
    public static IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        dialogText.GetComponent<Text>().text = "";
        isTyping = true;
        cancelTyping = false;

        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            //Debug.Log("Dialog Scrolling");

            dialogText.GetComponent<Text>().text += lineOfText[letter];
            letter++;

            //Debug.Log("Wait for seconds: " + typeSpeed);
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        dialogText.GetComponent<Text>().text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }


    #region End Dialogue helping method
    /// <summary>
    /// Helper method to end text
    /// </summary>
    public static void endDialogue()
    {
        // Set the Talking to bool to false
        talkingBool = false;

        //Turn off options for next time you talk to NPCs
        EndOptions();

        TurnOffDialogBox();
    }
    #endregion

    #region Turn off Option Buttons helper method
    /// <summary>
    /// 
    /// </summary>
    public static void EndOptions()
    {
        //Turn off options for new text
        for (int i = 0; i < 5; i++)
        {
            optionBool = false;

            // Change the option buttons to default and to no NPC
            Options[i].transform.GetComponent<DialogOptionScript>().lineJumpNumber = 0;
            Options[i].transform.GetComponent<DialogOptionScript>().currentResponseType = ResponseType.SayNothing;
            Options[i].transform.GetComponent<DialogOptionScript>().currentNPC = null;

            // DeActivate the buttons
            Options[i].SetActive(false);
        }
    }
    #endregion
    #endregion
}
