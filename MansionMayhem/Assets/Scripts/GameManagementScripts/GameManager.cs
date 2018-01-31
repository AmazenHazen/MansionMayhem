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

    // Internal GameState Variables
    public bool inGame;

    // ~~~~~~~Variables to be saved and loaded~~~~~~~~
    #region SaveVariables
    // Game Currency and progress variables
    public int highestLevel; // Highest level/Progress Variable
    public static int screws; // Currency Variable
    public int healthTotal; // Hearts Unlocked
    public int equipmentTotal; // Equipment Slots unlocked

    #region Unlockable Variables
    //Unlockable variables
    // Laser Pistol
    public bool LaserPistolUnlock;
    public bool LaserPistolUpgrade1Unlock;
    public bool LaserPistolUpgrade2Unlock;
    public bool LaserPistolUpgrade3Unlock;

    // Anti Ectoplasm Splatter Gun
    public bool AntiEctoGunUnlock;
    public bool AntiEctoGunUpgrade1Unlock;
    public bool AntiEctoGunUpgrade2Unlock;
    public bool AntiEctoGunUpgrade3Unlock;

    // Plasma Pistol
    public bool PlasmaPistolUnlock;
    public bool PlasmaPistolUpgrade1Unlock;
    public bool PlasmaPistolUpgrade2Unlock;
    public bool PlasmaPistolUpgrade3Unlock;

    // CryoGun
    public bool CryoGunUnlock;
    public bool CryoGunUpgrade1Unlock;
    public bool CryoGunUpgrade2Unlock;
    public bool CryoGunUpgrade3Unlock;

    // FlameThrower
    public bool FlameThrowerUnlock;
    public bool FlameThrowerUpgrade1Unlock;
    public bool FlameThrowerUpgrade2Unlock;
    public bool FlameThrowerUpgrade3Unlock;

    // Hellfire Shotgun
    public bool HellFireShotgunUnlock;        
    public bool HellFireShotgunUpgrade1Unlock;
    public bool HellFireShotgunUpgrade2Unlock;
    public bool HellFireShotgunUpgrade3Unlock;

    // Sound Cannon
    public bool SoundCannonUnlock;
    public bool SoundCannonUpgrade1Unlock;
    public bool SoundCannonUpgrade2Unlock;
    public bool SoundCannonUpgrade3Unlock;

    // Dark Energy Sniper
    public bool DarkEnergySniperUnlock;
    public bool DarkEnergySniperUpgrade1Unlock;
    public bool DarkEnergySniperUpgrade2Unlock;
    public bool DarkEnergySniperUpgrade3Unlock;

    // Electron Cannon
    public bool ElectronPulseCannonUnlock;
    public bool ElectronPulseCannonUpgrade1Unlock;
    public bool ElectronPulseCannonUpgrade2Unlock;
    public bool ElectronPulseCannonUpgrade3Unlock;

    // Aetherlight Bow
    public bool boolAetherlightBowUnlock;
    public bool boolAetherlightBowUpgrade1Unlock;
    public bool boolAetherlightBowUpgrade2Unlock;
    public bool boolAetherlightBowUpgrade3Unlock;

    // Celestial Repeater
    public bool CelestialRepeaterUnlock;
    public bool CelestialRepeaterUpgrade1Unlock;
    public bool CelestialRepeaterUpgrade2Unlock;
    public bool CelestialRepeaterUpgrade3Unlock;

    // Trinket Unlocks
    public bool AntiMatterDeviceUnlock;
    public bool PortalDeviceUnlock;
    public bool NanoBotHealingSwarmUnlock;
    public bool DroneUnlock;
    public bool HologramCloneUnlock;
    public bool ShieldUnlock;
    public bool BootRocketsUnlock;

    // Equipment Unlocks
    public bool FireResistantUnderArmorUnlock;
    public bool HeatedCoatLiningUnlock;
    public bool FrictionBootsUnlock;
    public bool AntidotePatchUnlock;
    public bool ScrewMagnetUnlock;
    public bool RoboticHeartUnlock;
    public bool DragonscaleArmorUnlock;
    public bool GooRepellingTreatmentUnlock;
    #endregion
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
    // Game Currency and progress variables
    public int highestLevel; // Highest level/Progress Variable
    public static int screws; // Currency Variable
    public int healthTotal; // Hearts Unlocked
    public int equipmentTotal; // Equipment Slots unlocked

    #region Unlockable Variables
    //Unlockable variables
    // Laser Pistol
    public bool LaserPistolUnlock;
    public bool LaserPistolUpgrade1Unlock;
    public bool LaserPistolUpgrade2Unlock;
    public bool LaserPistolUpgrade3Unlock;

    // Anti Ectoplasm Splatter Gun
    public bool AntiEctoGunUnlock;
    public bool AntiEctoGunUpgrade1Unlock;
    public bool AntiEctoGunUpgrade2Unlock;
    public bool AntiEctoGunUpgrade3Unlock;

    // Plasma Pistol
    public bool PlasmaPistolUnlock;
    public bool PlasmaPistolUpgrade1Unlock;
    public bool PlasmaPistolUpgrade2Unlock;
    public bool PlasmaPistolUpgrade3Unlock;

    // CryoGun
    public bool CryoGunUnlock;
    public bool CryoGunUpgrade1Unlock;
    public bool CryoGunUpgrade2Unlock;
    public bool CryoGunUpgrade3Unlock;

    // FlameThrower
    public bool FlameThrowerUnlock;
    public bool FlameThrowerUpgrade1Unlock;
    public bool FlameThrowerUpgrade2Unlock;
    public bool FlameThrowerUpgrade3Unlock;

    // Hellfire Shotgun
    public bool HellFireShotgunUnlock;
    public bool HellFireShotgunUpgrade1Unlock;
    public bool HellFireShotgunUpgrade2Unlock;
    public bool HellFireShotgunUpgrade3Unlock;

    // Sound Cannon
    public bool SoundCannonUnlock;
    public bool SoundCannonUpgrade1Unlock;
    public bool SoundCannonUpgrade2Unlock;
    public bool SoundCannonUpgrade3Unlock;

    // Dark Energy Sniper
    public bool DarkEnergySniperUnlock;
    public bool DarkEnergySniperUpgrade1Unlock;
    public bool DarkEnergySniperUpgrade2Unlock;
    public bool DarkEnergySniperUpgrade3Unlock;

    // Electron Cannon
    public bool ElectronPulseCannonUnlock;
    public bool ElectronPulseCannonUpgrade1Unlock;
    public bool ElectronPulseCannonUpgrade2Unlock;
    public bool ElectronPulseCannonUpgrade3Unlock;

    // Aetherlight Bow
    public bool boolAetherlightBowUnlock;
    public bool boolAetherlightBowUpgrade1Unlock;
    public bool boolAetherlightBowUpgrade2Unlock;
    public bool boolAetherlightBowUpgrade3Unlock;

    // Celestial Repeater
    public bool CelestialRepeaterUnlock;
    public bool CelestialRepeaterUpgrade1Unlock;
    public bool CelestialRepeaterUpgrade2Unlock;
    public bool CelestialRepeaterUpgrade3Unlock;

    // Trinket Unlocks
    public bool AntiMatterDeviceUnlock;
    public bool PortalDeviceUnlock;
    public bool NanoBotHealingSwarmUnlock;
    public bool DroneUnlock;
    public bool HologramCloneUnlock;
    public bool ShieldUnlock;
    public bool BootRocketsUnlock;

    // Equipment Unlocks
    public bool FireResistantUnderArmorUnlock;
    public bool HeatedCoatLiningUnlock;
    public bool FrictionBootsUnlock;
    public bool AntidotePatchUnlock;
    public bool ScrewMagnetUnlock;
    public bool RoboticHeartUnlock;
    public bool DragonscaleArmorUnlock;
    public bool GooRepellingTreatmentUnlock;
    #endregion
}
#endregion

