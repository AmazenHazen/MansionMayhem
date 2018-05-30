using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GUIManager : MonoBehaviour
{

    #region Attributes
    private GameObject player;

    // Health Management
    int colorIndex;
    private float health;
    private List<Color> HealthColors;
    public List<GameObject> HeartContainers;
    public List<GameObject> FullHearts;
    public List<GameObject> HalfHearts;

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
    public List<GameObject> instructionPages;
    public List<GameObject> instructionButtons;
    public int instructionsPage;

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
    #endregion

    #region Start
    // Start is called when the GUI is initialized
    void Start()
    {
        player = GameObject.Find("Player");

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
        HealthColors.Add(new Color32(186, 0, 0, 255));
        HealthColors.Add(new Color32(255, 215, 0, 255));

        // hide containers if not unlocked
        if (GameManager.instance.healthTotal<15)
        {
            for (int i = GameManager.instance.healthTotal; i < 15; i++)
            {
                HeartContainers[i].SetActive(false);
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
        instructionsPage = 0;

        // set boss fight to false
        bossFight = false;
    }

    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        BossHealthManagement();
        HealthManagement();
        TextUpdate();
        EscapeScreenManagement();
        if(GameManager.instance.currentGameState == GameState.Died)
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
        health = player.GetComponent<PlayerManager>().CurrentLife;

        // Color determination
        colorIndex = (int)((health / 15));

        HalfHeartManagement();
        FullHeartManagement();
    }

    #region Heart Activation
    /// <summary>
    /// Activates the hearts seen on screen
    /// </summary>
    void HalfHeartManagement()
    {
        // Set up hearts initially
        for (int i = 1; i < 16; i++)
        {
            //Deactivate all Half Hearts
            HalfHearts[i-1].SetActive(false);
        }

        // Activate halfHearts if you have a decimal health
        if (health % 1.0f == .5)
        {
            // Color the half heart correctly
            HalfHearts[(int)(((health - 0.5f) % 15.0f))].GetComponent<Image>().color = HealthColors[colorIndex+1];
            HalfHearts[(int)(((health-0.5f)%15.0f))].SetActive(true);
        }
    }
    #endregion
    
    #region Full Heart Managment
    /// <summary>
    /// Colors the hearts correctly based on how much health you have
    /// </summary>
    void FullHeartManagement()
    {
        int colorCount;
        colorCount = (int)(health % 15);

        // Sets color of hearts to prev color
        for (int i = 0; i < 15; i++)
        {
            FullHearts[i].GetComponent<Image>().color = HealthColors[colorIndex];
        }

        // Sets color of rest of hearts to current color
        for (int i = 0; i < colorCount; i++)
        {
            FullHearts[i].GetComponent<Image>().color = HealthColors[colorIndex + 1];
        }

    }
    #endregion

    #endregion

    #region Text Management
    void TextUpdate()
    {
        scoreText.text = "Screws: " + GameManager.instance.screws;

        // Get variables needed for the HUD Text
        if (GameManager.DebugMode)
        {
            // Create the text for weapons
            currentRangeWeapon = player.GetComponent<PlayerManager>().CurrentRangeWeapon;
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
        }
        else
        {
            levelText.text = "";
            rangeWeaponText.text = "";
            experienceText.text = "";
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
            bossCurrentHealth = boss.GetComponent<EnemyManager>().CurrentLife;
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
        instructionPages[0].SetActive(true);
        instructionPages[1].SetActive(false);
        instructionButtons[0].SetActive(false);
        instructionButtons[1].SetActive(true);
        instructionsPage = 0;

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

    #region instructions management

    /// <summary>
    /// Handling clicking next and previous buttons for the insturction screen
    /// </summary>
    /// <param name="forward"></param>
    public void pageTurn(bool forward)
    {
        // turn off current page
        for(int i=0; i< instructionPages.Count; i++)
        {
            if(instructionPages[instructionsPage])
            {
                instructionPages[instructionsPage].SetActive(false);
            }
        }
        if(forward)
        {
            instructionsPage++;
        }
        else
        {
            instructionsPage--;
        }

        instructionPages[instructionsPage].SetActive(true);


        // Turn off or on the next page button if needed
        if (instructionsPage < instructionButtons.Count-1)
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
