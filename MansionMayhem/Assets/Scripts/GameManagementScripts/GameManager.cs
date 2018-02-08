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
    public int MAX_EQUIPMENT = 3;
    public int MAX_HEALTH = 20;




    // Static instance of the GameManager to allows it to be accessed from any script
    public static GameManager instance = null;

    // Level Variables
    public int currentLevel;
    public GameState currentGameState;


    // ~~~~~~~Variables to be saved and loaded~~~~~~~~
    #region SaveVariables
    // Game Currency and progress variables
    public int highestLevel; // Highest level/Progress Variable
    public int screws; // Currency Variable
    public int healthTotal; // Hearts Unlocked
    public int equipmentTotal; // Equipment Slots unlocked
    public List<rangeWeapon> currentGuns; // The current weapons the player has equiped

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
    public bool AetherlightBowUnlock;
    public bool AetherlightBowUpgrade1Unlock;
    public bool AetherlightBowUpgrade2Unlock;
    public bool AetherlightBowUpgrade3Unlock;

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
        data.currentGuns = currentGuns;
        data.healthTotal = healthTotal;
        data.equipmentTotal = equipmentTotal;

        #region Unlockable Variables
        //Unlockable variables
        // Laser Pistol
        data.LaserPistolUnlock = LaserPistolUnlock;
        data.LaserPistolUpgrade1Unlock = LaserPistolUpgrade1Unlock;
        data.LaserPistolUpgrade2Unlock = LaserPistolUpgrade2Unlock;
        data.LaserPistolUpgrade3Unlock = LaserPistolUpgrade3Unlock;

        // Anti Ectoplasm Splatter Gun
        data.AntiEctoGunUnlock = AntiEctoGunUnlock;
        data.AntiEctoGunUpgrade1Unlock = AntiEctoGunUpgrade1Unlock;
        data.AntiEctoGunUpgrade2Unlock = AntiEctoGunUpgrade2Unlock;
        data.AntiEctoGunUpgrade3Unlock = AntiEctoGunUpgrade3Unlock;

        // Plasma Pistol
        data.PlasmaPistolUnlock = PlasmaPistolUnlock;
        data.PlasmaPistolUpgrade1Unlock = PlasmaPistolUpgrade1Unlock;
        data.PlasmaPistolUpgrade2Unlock = PlasmaPistolUpgrade2Unlock;
        data.PlasmaPistolUpgrade3Unlock = PlasmaPistolUpgrade3Unlock;

        // CryoGun
        data.CryoGunUnlock = CryoGunUnlock;
        data.CryoGunUpgrade1Unlock = CryoGunUpgrade1Unlock;
        data.CryoGunUpgrade2Unlock = CryoGunUpgrade2Unlock;
        data.CryoGunUpgrade3Unlock = CryoGunUpgrade3Unlock;

        // FlameThrower
        data.FlameThrowerUnlock = FlameThrowerUnlock;
        data.FlameThrowerUpgrade1Unlock = FlameThrowerUpgrade1Unlock;
        data.FlameThrowerUpgrade2Unlock = FlameThrowerUpgrade2Unlock;
        data.FlameThrowerUpgrade3Unlock = FlameThrowerUpgrade3Unlock;

        // Hellfire Shotgun
        data.HellFireShotgunUnlock = HellFireShotgunUnlock;
        data.HellFireShotgunUpgrade1Unlock = HellFireShotgunUpgrade1Unlock;
        data.HellFireShotgunUpgrade2Unlock = HellFireShotgunUpgrade2Unlock;
        data.HellFireShotgunUpgrade3Unlock = HellFireShotgunUpgrade3Unlock;

        // Sound Cannon
        data.SoundCannonUnlock = SoundCannonUnlock;
        data.SoundCannonUpgrade1Unlock = SoundCannonUpgrade1Unlock;
        data.SoundCannonUpgrade2Unlock = SoundCannonUpgrade2Unlock;
        data.SoundCannonUpgrade3Unlock = SoundCannonUpgrade3Unlock;

        // Dark Energy Sniper
        data.DarkEnergySniperUnlock = DarkEnergySniperUnlock;
        data.DarkEnergySniperUpgrade1Unlock = DarkEnergySniperUpgrade1Unlock;
        data.DarkEnergySniperUpgrade2Unlock = DarkEnergySniperUpgrade2Unlock;
        data.DarkEnergySniperUpgrade3Unlock = DarkEnergySniperUpgrade3Unlock;

        // Electron Cannon
        data.ElectronPulseCannonUnlock = ElectronPulseCannonUnlock;
        data.ElectronPulseCannonUpgrade1Unlock = ElectronPulseCannonUpgrade1Unlock;
        data.ElectronPulseCannonUpgrade2Unlock = ElectronPulseCannonUpgrade2Unlock;
        data.ElectronPulseCannonUpgrade3Unlock = ElectronPulseCannonUpgrade3Unlock;

            // Aetherlight Bow
        data.AetherlightBowUnlock = AetherlightBowUnlock;
        data.AetherlightBowUpgrade1Unlock = AetherlightBowUpgrade1Unlock;
        data.AetherlightBowUpgrade2Unlock = AetherlightBowUpgrade2Unlock;
        data.AetherlightBowUpgrade3Unlock = AetherlightBowUpgrade3Unlock;

        // Celestial Repeater
        data.CelestialRepeaterUnlock = CelestialRepeaterUnlock;
        data.CelestialRepeaterUpgrade1Unlock = CelestialRepeaterUpgrade1Unlock;
        data.CelestialRepeaterUpgrade2Unlock = CelestialRepeaterUpgrade2Unlock;
        data.CelestialRepeaterUpgrade3Unlock = CelestialRepeaterUpgrade3Unlock;

        // Trinket Unlocks
        data.AntiMatterDeviceUnlock = AntiMatterDeviceUnlock;
        data.PortalDeviceUnlock = PortalDeviceUnlock;
        data.NanoBotHealingSwarmUnlock = NanoBotHealingSwarmUnlock;
        data.DroneUnlock = DroneUnlock;
        data.HologramCloneUnlock = HologramCloneUnlock;
        data.ShieldUnlock = ShieldUnlock;
        data.BootRocketsUnlock = BootRocketsUnlock;

        // Equipment Unlocks
        data.FireResistantUnderArmorUnlock = FireResistantUnderArmorUnlock;
        data.HeatedCoatLiningUnlock = HeatedCoatLiningUnlock;
        data.FrictionBootsUnlock = FrictionBootsUnlock;
        data.AntidotePatchUnlock = AntidotePatchUnlock;
        data.ScrewMagnetUnlock = ScrewMagnetUnlock;
        data.RoboticHeartUnlock = RoboticHeartUnlock;
        data.DragonscaleArmorUnlock = DragonscaleArmorUnlock;
        data.GooRepellingTreatmentUnlock = GooRepellingTreatmentUnlock;
        #endregion



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
            healthTotal = data.healthTotal;
            equipmentTotal = data.equipmentTotal;
            currentGuns = data.currentGuns;

            #region Unlockable Variables
            //Unlockable variables
            // Laser Pistol
            LaserPistolUnlock = data.LaserPistolUnlock;
            LaserPistolUpgrade1Unlock = data.LaserPistolUpgrade1Unlock;
            LaserPistolUpgrade2Unlock = data.LaserPistolUpgrade2Unlock;
            LaserPistolUpgrade3Unlock = data.LaserPistolUpgrade3Unlock;

            // Anti Ectoplasm Splatter Gun
            AntiEctoGunUnlock = data.AntiEctoGunUnlock;
            AntiEctoGunUpgrade1Unlock = data.AntiEctoGunUpgrade1Unlock;
            AntiEctoGunUpgrade2Unlock = data.AntiEctoGunUpgrade2Unlock;
            AntiEctoGunUpgrade3Unlock = data.AntiEctoGunUpgrade3Unlock;

            // Plasma Pistol
            PlasmaPistolUnlock = data.PlasmaPistolUnlock;
            PlasmaPistolUpgrade1Unlock = data.PlasmaPistolUpgrade1Unlock;
            PlasmaPistolUpgrade2Unlock = data.PlasmaPistolUpgrade2Unlock;
            PlasmaPistolUpgrade3Unlock = data.PlasmaPistolUpgrade3Unlock;

            // CryoGun
            CryoGunUnlock = data.CryoGunUnlock;
            CryoGunUpgrade1Unlock = data.CryoGunUpgrade1Unlock;
            CryoGunUpgrade2Unlock = data.CryoGunUpgrade2Unlock;
            CryoGunUpgrade3Unlock = data.CryoGunUpgrade3Unlock;

            // FlameThrower
            FlameThrowerUnlock = data.FlameThrowerUnlock;
            FlameThrowerUpgrade1Unlock = data.FlameThrowerUpgrade1Unlock;
            FlameThrowerUpgrade2Unlock = data.FlameThrowerUpgrade2Unlock;
            FlameThrowerUpgrade3Unlock = data.FlameThrowerUpgrade3Unlock;

            // Hellfire Shotgun
            HellFireShotgunUnlock = data.HellFireShotgunUnlock;
            HellFireShotgunUpgrade1Unlock = data.HellFireShotgunUpgrade1Unlock;
            HellFireShotgunUpgrade2Unlock = data.HellFireShotgunUpgrade2Unlock;
            HellFireShotgunUpgrade3Unlock = data.HellFireShotgunUpgrade3Unlock;

            // Sound Cannon
            SoundCannonUnlock = data.SoundCannonUnlock;
            SoundCannonUpgrade1Unlock = data.SoundCannonUpgrade1Unlock;
            SoundCannonUpgrade2Unlock = data.SoundCannonUpgrade2Unlock;
            SoundCannonUpgrade3Unlock = data.SoundCannonUpgrade3Unlock;

            // Dark Energy Sniper
            DarkEnergySniperUnlock = data.DarkEnergySniperUnlock;
            DarkEnergySniperUpgrade1Unlock = data.DarkEnergySniperUpgrade1Unlock;
            DarkEnergySniperUpgrade2Unlock = data.DarkEnergySniperUpgrade2Unlock;
            DarkEnergySniperUpgrade3Unlock = data.DarkEnergySniperUpgrade3Unlock;

            // Electron Cannon
            ElectronPulseCannonUnlock = data.ElectronPulseCannonUnlock;
            ElectronPulseCannonUpgrade1Unlock = data.ElectronPulseCannonUpgrade1Unlock;
            ElectronPulseCannonUpgrade2Unlock = data.ElectronPulseCannonUpgrade2Unlock;
            ElectronPulseCannonUpgrade3Unlock = data.ElectronPulseCannonUpgrade3Unlock;

            // Aetherlight Bow
            AetherlightBowUnlock = data.AetherlightBowUnlock;
            AetherlightBowUpgrade1Unlock = data.AetherlightBowUpgrade1Unlock;
            AetherlightBowUpgrade2Unlock = data.AetherlightBowUpgrade2Unlock;
            AetherlightBowUpgrade3Unlock = data.AetherlightBowUpgrade3Unlock;

            // Celestial Repeater
            CelestialRepeaterUnlock = data.CelestialRepeaterUnlock;
            CelestialRepeaterUpgrade1Unlock = data.CelestialRepeaterUpgrade1Unlock;
            CelestialRepeaterUpgrade2Unlock = data.CelestialRepeaterUpgrade2Unlock;
            CelestialRepeaterUpgrade3Unlock = data.CelestialRepeaterUpgrade3Unlock;

            // Trinket Unlocks
            AntiMatterDeviceUnlock = data.AntiMatterDeviceUnlock;
            PortalDeviceUnlock = data.PortalDeviceUnlock;
            NanoBotHealingSwarmUnlock = data.NanoBotHealingSwarmUnlock;
            DroneUnlock = data.DroneUnlock;
            HologramCloneUnlock = data.HologramCloneUnlock;
            ShieldUnlock = data.ShieldUnlock;
            BootRocketsUnlock = data.BootRocketsUnlock;

            // Equipment Unlocks
            FireResistantUnderArmorUnlock = data.FireResistantUnderArmorUnlock;
            HeatedCoatLiningUnlock = data.HeatedCoatLiningUnlock;
            FrictionBootsUnlock = data.FrictionBootsUnlock;
            AntidotePatchUnlock = data.AntidotePatchUnlock;
            ScrewMagnetUnlock = data.ScrewMagnetUnlock;
            RoboticHeartUnlock = data.RoboticHeartUnlock;
            DragonscaleArmorUnlock = data.DragonscaleArmorUnlock;
            GooRepellingTreatmentUnlock = data.GooRepellingTreatmentUnlock;
            #endregion
        }
        else
        {
            // Variables that are not saved are set to original value otherwise
            screws = 0;
            highestLevel = 0;
            healthTotal = 5;
            equipmentTotal = 10;
            currentGuns = new List<rangeWeapon>(3);
            currentGuns[0] = rangeWeapon.laserpistol;
            currentGuns[1] = rangeWeapon.None;
            currentGuns[2] = rangeWeapon.None;


            #region Unlockable Variables
            //Unlockable variables

            // Laser Pistol
            LaserPistolUnlock = false;
            LaserPistolUpgrade1Unlock = false;
            LaserPistolUpgrade2Unlock = false;
            LaserPistolUpgrade3Unlock = false;

            // Anti Ectoplasm Splatter Gun
            AntiEctoGunUnlock = false;
            AntiEctoGunUpgrade1Unlock = false;
            AntiEctoGunUpgrade2Unlock = false;
            AntiEctoGunUpgrade3Unlock = false;

            // Plasma Pistol
            PlasmaPistolUnlock = false;
            PlasmaPistolUpgrade1Unlock = false;
            PlasmaPistolUpgrade2Unlock = false;
            PlasmaPistolUpgrade3Unlock = false;

            // CryoGun
            CryoGunUnlock = false;
            CryoGunUpgrade1Unlock = false;
            CryoGunUpgrade2Unlock = false;
            CryoGunUpgrade3Unlock = false;

            // FlameThrower
            FlameThrowerUnlock = false;
            FlameThrowerUpgrade1Unlock = false;
            FlameThrowerUpgrade2Unlock = false;
            FlameThrowerUpgrade3Unlock = false;

            // Hellfire Shotgun
            HellFireShotgunUnlock = false;
            HellFireShotgunUpgrade1Unlock = false;
            HellFireShotgunUpgrade2Unlock = false;
            HellFireShotgunUpgrade3Unlock = false;

            // Sound Cannon
            SoundCannonUnlock = false;
            SoundCannonUpgrade1Unlock = false;
            SoundCannonUpgrade2Unlock = false;
            SoundCannonUpgrade3Unlock = false;

            // Dark Energy Sniper
            DarkEnergySniperUnlock = false;
            DarkEnergySniperUpgrade1Unlock = false;
            DarkEnergySniperUpgrade2Unlock = false;
            DarkEnergySniperUpgrade3Unlock = false;

            // Electron Cannon
            ElectronPulseCannonUnlock = false;
            ElectronPulseCannonUpgrade1Unlock = false;
            ElectronPulseCannonUpgrade2Unlock = false;
            ElectronPulseCannonUpgrade3Unlock = false;

            // Aetherlight Bow
            AetherlightBowUnlock = false;
            AetherlightBowUpgrade1Unlock = false;
            AetherlightBowUpgrade2Unlock = false;
            AetherlightBowUpgrade3Unlock = false;

            // Celestial Repeater
            CelestialRepeaterUnlock = false;
            CelestialRepeaterUpgrade1Unlock = false;
            CelestialRepeaterUpgrade2Unlock = false;
            CelestialRepeaterUpgrade3Unlock = false;

            // Trinket Unlocks
            AntiMatterDeviceUnlock = false;
            PortalDeviceUnlock = false;
            NanoBotHealingSwarmUnlock = false;
            DroneUnlock = false;
            HologramCloneUnlock = false;
            ShieldUnlock = false;
            BootRocketsUnlock = false;

            // Equipment Unlocks
            FireResistantUnderArmorUnlock = false;
            HeatedCoatLiningUnlock = false;
            FrictionBootsUnlock = false;
            AntidotePatchUnlock = false;
            ScrewMagnetUnlock = false;
            RoboticHeartUnlock = false;
            DragonscaleArmorUnlock = false;
            GooRepellingTreatmentUnlock = false;
            #endregion

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
    public int screws; // Currency Variable
    public int healthTotal; // Hearts Unlocked
    public int equipmentTotal; // Equipment Slots unlocked
    public List<rangeWeapon> currentGuns; // Current weapons the player has equipped


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
    public bool AetherlightBowUnlock;
    public bool AetherlightBowUpgrade1Unlock;
    public bool AetherlightBowUpgrade2Unlock;
    public bool AetherlightBowUpgrade3Unlock;

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

