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
    // Static instance of the GameManager to allows it to be accessed from any script
    public static GameManager instance = null;



    // Level Variables
    public static int currentLevel;
    public static GameState currentGameState;
    

    // ~~~~~~~Variables to be saved and loaded~~~~~~~~
    public int highestLevel; // Highest level/Progress Variable
    public static int screws; // Currency Variable
    //Unlockable variables
    private bool blobGunUnlock;
    private bool hellFireShotgunUnlock;
    private bool cryoGunUnlock;
    private bool aetherLightBowUnlock;
    private bool soundCannonUnlock;



    // Internal GameState Variables
    public bool inGame;
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

        // Start the currentGameState to MainMenu
        currentGameState = GameState.MainMenu;

        // Load the save file if starting the game up
        Load();

        // Game State Variables
        inGame = false;
    }
    #endregion

    #region GameUpdate
    // Update is called once per frame
    void Update ()
    {
        Debug.Log("GameState: " + currentGameState);
    }
    #endregion

    #region GUI
    /*
    /// <summary>
    /// Overloaded OnGUI method:
    /// Puts these thing on the display:
    /// Lives
    /// Score
    /// Level
    /// Boss Health (if bossfight is occuring)
    /// </summary>
    void OnGUI()
    {
        // Color
        GUI.color = Color.gray;
        // Font Size
        GUI.skin.box.fontSize = 20;

        // IN-GAME GUI, THE GUI YOU SEE WHEN YOU NORMALLY PLAY IN GAME
        if(inGame)
        {
            // Score FOR GUI
            GUI.Label(new Rect(10, 30, 400, 50), "Screws: " + screws);
        
            if(GUI.Button(new Rect(10, 300, 100, 30), "Save"))
            {
                Save();
            }
        }
    }
    */
    #endregion


    #region Save and Load Methods
    // This will work for everything but web
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MansionMayhem.dat");

        PlayerData data = new PlayerData();

        // Puts the Variables that need to be saved into the data Class
        data.screws = screws;
        data.highestLevel = highestLevel;
        data.aetherLightBowUnlock = aetherLightBowUnlock;
        data.blobGunUnlock = blobGunUnlock;
        data.cryoGunUnlock = cryoGunUnlock;
        data.hellFireShotgunUnlock = hellFireShotgunUnlock;
        data.soundCannonUnlock = soundCannonUnlock;



        // Serialize the data
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        // Check to see if a save file already exists
        if (File.Exists(Application.persistentDataPath + "/MansionMayhem.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MansionMayhem.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // Set variables based on the save file
            screws = data.screws;
            highestLevel = data.highestLevel;
            aetherLightBowUnlock = data.aetherLightBowUnlock;
            blobGunUnlock = data.blobGunUnlock;
            cryoGunUnlock = data.cryoGunUnlock;
            hellFireShotgunUnlock = data.hellFireShotgunUnlock;
            soundCannonUnlock = data.soundCannonUnlock;
        }
        else
        {
            // Variables that are not saved are set to original value otherwise
            screws = 0;
            highestLevel = 0;
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
    // All saved data here
    public int highestLevel;
    public int screws;
    //Unlockable variables
    public bool blobGunUnlock;
    public bool hellFireShotgunUnlock;
    public bool cryoGunUnlock;
    public bool aetherLightBowUnlock;
    public bool soundCannonUnlock;

}
#endregion

