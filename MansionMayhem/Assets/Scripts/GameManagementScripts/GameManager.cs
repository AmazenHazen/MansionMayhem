using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Headers for Save Files
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameManager : MonoBehaviour
{
    #region Variables
    // Contstant Variables
    const int TOTAL_LEVELS = 54;

    // Static instance of the GameManager to allows it to be accessed from any script
    public static GameManager instance = null;

    // Common Items to be static and accessible by other scripts
    public static List<GameObject> screwItems;
    public static List<GameObject> recoveryItems;
    public static GameObject experienceOrb;

    public static bool DebugMode;

    // Level Variables
    public int currentLevel;
    public GameState currentGameState;


    // ~~~~~~~Variables to be saved and loaded~~~~~~~~
    #region SaveVariables
    // Game Currency and progress variables
    public int highestLevel; // Highest level/Progress Variable
    public bool[] unlockedLevels;
    public bool[] soulStones;
    public int screws; // Main Currency Variable
    public int experience; // Secondary Currency Variable
    public int blueprints; // Tertiary Currency
    public int healthTotal; // Hearts Unlocked
    public int staminaTotal; // Stamina Unlocked
    public List<rangeWeapon> currentGuns; // The current weapons the player has equiped
    public bool[] unlockableBuyables; // Saves all of the data regarding unlocked guns and upgrades
    #endregion

    #endregion

    #region Properties
    public int HighestLevel
    {
        get { return highestLevel; }
        set { highestLevel = value; }
    }
    #endregion

    #region initialization
    // Use this for initialization
    void Start ()
    {
        #region Singleton Start Logic
        // Check to see if a GameManager already exists
        if (instance == null)
        {
            // Set the instance to this script
            instance = this;
        }
        // Otherwise delete this gameObject
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        // Keep this instance from being destroyed
        DontDestroyOnLoad(gameObject);
        #endregion

        // Set some static game objects
        experienceOrb = Resources.Load<GameObject>("Prefabs/Items/Pickups/ExperienceOrb");


        // Screw = 1 screw, red screw = 5 screws, golden screw = 10 screws, toolbox = 50 screws
        screwItems = new List<GameObject>(3);
        screwItems.Add(Resources.Load<GameObject>("Prefabs/Items/Pickups/Screw"));
        screwItems.Add(Resources.Load<GameObject>("Prefabs/Items/Pickups/Red Screw"));
        screwItems.Add(Resources.Load<GameObject>("Prefabs/Items/Pickups/Golden Screw"));

        // Heart = 1 heart, potion = 3, healthkit = 5, goldenheart = 10, toolbox = 50
        recoveryItems = new List<GameObject>(4);
        recoveryItems.Add(Resources.Load<GameObject>("Prefabs/Items/Pickups/Heart"));
        recoveryItems.Add(Resources.Load<GameObject>("Prefabs/Items/Pickups/Health Potion"));
        recoveryItems.Add(Resources.Load<GameObject>("Prefabs/Items/Pickups/FirstAidPack"));
        recoveryItems.Add(Resources.Load<GameObject>("Prefabs/Items/Pickups/goldenHeart"));


        // Start the currentGameState to MainMenu
        if (GameObject.Find("Player"))
        {
            currentGameState = GameState.Play;
        }
        else
        {
            currentGameState = GameState.MainMenu;
        }


        currentGuns = new List<rangeWeapon>(3);
        for (int i = 0; i < 3; i++)
        {
            currentGuns.Add(rangeWeapon.None);
        }

        // Load the save file if starting the game up
        Load();
    }
    #endregion

    #region GameUpdate
    // Update is called once per frame
    void Update ()
    {
        // Check if the Debug Command is used
        if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (DebugMode) { DebugMode = false; }
            else { DebugMode = true; }
        }
    }
    #endregion

    #region Save and Load Methods
    // This will work for everything but web
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MansionMayhemTest.dat");

        PlayerData data = new PlayerData();

        // Puts the Variables that need to be saved into the data Class
        data.screws = screws;
        data.experience = experience;
        data.blueprints = blueprints;
        data.highestLevel = highestLevel;
        data.unlockedLevels = unlockedLevels;
        data.healthTotal = healthTotal;
        data.staminaTotal = staminaTotal;
        data.unlockableBuyables = unlockableBuyables;
        data.soulStones = soulStones;

        // Only load the list if it is correct
        data.currentGuns = currentGuns;

        // Serialize the data
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        // Check to see if a save file already exists
        if (File.Exists(Application.persistentDataPath + "/MansionMayhemTest.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MansionMayhemTest.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // Set variables based on the save file
            screws = data.screws;
            experience = data.experience;
            blueprints = data.blueprints;
            highestLevel = data.highestLevel;
            unlockedLevels = data.unlockedLevels;
            healthTotal = data.healthTotal;
            staminaTotal = data.staminaTotal;
            currentGuns = data.currentGuns;
            unlockableBuyables = data.unlockableBuyables;
            soulStones = data.soulStones;
        }
        else
        {
            // Variables that are not saved are set to original value otherwise
            // Giving players 2000 screws and some weapons unlocked for the demo
            screws = 0;//2000;
            experience = 0;
            blueprints = 0;
            highestLevel = 0;


            soulStones = new bool[TOTAL_LEVELS];
            // Set all the SoulStone Collectibles to false
            for (int i = 0; i < TOTAL_LEVELS; i++)
            {
                soulStones[i] = false;
            }

            unlockedLevels = new bool[TOTAL_LEVELS];

            for (int i = 0; i < TOTAL_LEVELS; i++)
            {
                unlockedLevels[i] = false;
                unlockedLevels[i] = true;       // used to unlock and test all levels
            }
            
            // Set default unlocked levels
            // Unlock the first three levels (Optional Tutorial Area, Spider Mansion, Ghost Knight)
            unlockedLevels[0] = true;   // Castor's Laboratory (optional tutorial)
            unlockedLevels[1] = true;   // Optional Path 1 - Spider Queen Mansion
            unlockedLevels[4] = true;   // Main Path - The Ghost Ruins


            healthTotal = 5;
            staminaTotal = 1;
            currentGuns[0] = rangeWeapon.laserpistol;
            currentGuns[1] = rangeWeapon.None;
            currentGuns[2] = rangeWeapon.None;


            #region Unlockable Variables
            unlockableBuyables = new bool[100];
            unlockableBuyables[0] = true;
            for(int i=1;i<100;i++)
            {
                unlockableBuyables[i] = false;
                unlockableBuyables[i] = true;       // Used for testing all guns
            }





            #endregion
            healthTotal = 8;    // used for testing purposes
            staminaTotal = 2;   // used for testing purposes

        }
    }

    #endregion
}


#region Data Container for saving
/// <summary>
/// Class for Saving
/// Just a class that is a "DATA Container" that allows writing the data to a save file
/// </summary>
[Serializable]
class PlayerData
{
    // Game Currency and progress variables
    public int highestLevel; // Highest level/Progress Variable
    public bool[] unlockedLevels;
    public int screws; // Currency Variable
    public int experience; // Secondary Currency Variable
    public int blueprints; // Tertiary Variable
    public int healthTotal; // Hearts Unlocked
    public int staminaTotal; // Stamina Circles Unlocked
    public int equipmentTotal; // Equipment Slots unlocked
    public List<rangeWeapon> currentGuns; // Current weapons the player has equipped
    public bool[] unlockableBuyables;
    public bool[] soulStones;
}
#endregion

