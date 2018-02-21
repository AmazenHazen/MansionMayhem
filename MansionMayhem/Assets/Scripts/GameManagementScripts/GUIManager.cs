using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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


    // Variables for changing Text
    public Text rangeWeaponText;
    rangeWeapon currentRangeWeapon;
    public Text scoreText;
    public Text levelText;

    // Variables for escape screen
    public GameObject escapeScreen;

    // Variables for the level Objective Screen
    public GameObject objectiveScreen;

    // Variables for Instructions Screen
    public GameObject instructionsScreen;


    // Variables for talking to NPC/Workbench
    public static bool usingOtherInterface;
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

        if (GameObject.Find("LevelManager").GetComponent<LevelManager>().levelObjective == levelType.boss)
        {
            boss = GameObject.Find("LevelManager").GetComponent<LevelManager>().boss;
            // Set the healthBar Max value
            bossMaxHealth = boss.GetComponent<EnemyManager>().maxHealth;
            bossHealthBar.maxValue = bossMaxHealth;
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

        // turn off the dialog box if it is on
        if (GameObject.Find("DialogBox"))
        {
            GameObject.Find("DialogBox").SetActive(false);
        }

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
        // Get variables needed for the HUD Text
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
        scoreText.text = "Screws: " + GameManager.instance.screws;
        levelText.text = "Level: " + GameManager.instance.currentLevel;
    }
    #endregion
    
    #region Boss HealthBar Management
    void BossHealthManagement()
    {
        if (boss != null && boss.activeSelf == true)
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
        instructionsScreen.SetActive(false);
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
                Debug.Log("Item Added to GUI " + item);

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
    void OnGUI()
    {
        GUI.Label(new Rect(100, 10, 400, 50), "Health: " + health);
    }
    #endregion
}
