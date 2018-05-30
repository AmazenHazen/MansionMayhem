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
    public int screws; // Main Currency Variable
    public int experience; // Secondary Currency Variable
    public int blueprints; // Tertiary Currency
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

    // Xenon Pulser
    public bool XenonPulserUnlock;
    public bool XenonPulserUpgrade1Unlock;
    public bool XenonPulserUpgrade2Unlock;
    public bool XenonPulserUpgrade3Unlock;

    // Antimatter Particle Gun
    public bool AntiMatterParticleUnlock;
    public bool AntiMatterParticleUpgrade1Unlock;
    public bool AntiMatterParticleUpgrade2Unlock;
    public bool AntiMatterParticleUpgrade3Unlock;

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
        currentGameState = GameState.MainMenu;

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
        data.equipmentTotal = equipmentTotal;

        // Only load the list if it is correct
        data.currentGuns = currentGuns;

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

        // Xenon Pulser
        data.XenonPulserUnlock = XenonPulserUnlock;
        data.XenonPulserUpgrade1Unlock = XenonPulserUpgrade1Unlock;
        data.XenonPulserUpgrade2Unlock = XenonPulserUpgrade2Unlock;
        data.XenonPulserUpgrade3Unlock = XenonPulserUpgrade3Unlock;


        // AntimatterParticle Gun
        data.AntiMatterParticleUnlock = AntiMatterParticleUnlock;
        data.AntiMatterParticleUpgrade1Unlock = AntiMatterParticleUpgrade1Unlock;
        data.AntiMatterParticleUpgrade2Unlock = AntiMatterParticleUpgrade2Unlock;
        data.AntiMatterParticleUpgrade3Unlock = AntiMatterParticleUpgrade3Unlock;



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

            // Xenon Pulser
            XenonPulserUnlock = data.XenonPulserUnlock;
            XenonPulserUpgrade1Unlock = data.XenonPulserUpgrade1Unlock;
            XenonPulserUpgrade2Unlock = data.XenonPulserUpgrade2Unlock;
            XenonPulserUpgrade3Unlock = data.XenonPulserUpgrade3Unlock;

            // Antimatter particle
            AntiMatterParticleUnlock = data.AntiMatterParticleUnlock;
            AntiMatterParticleUpgrade1Unlock = data.AntiMatterParticleUpgrade1Unlock;
            AntiMatterParticleUpgrade2Unlock = data.AntiMatterParticleUpgrade2Unlock;
            AntiMatterParticleUpgrade3Unlock = data.AntiMatterParticleUpgrade3Unlock;

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
            // Giving players 2000 screws and some weapons unlocked for the demo
            screws = 2000;
            experience = 0;
            blueprints = 0;
            highestLevel = 0;

            // Unlock the first level
            unlockedLevels = new bool[TOTAL_LEVELS];
            unlockedLevels[0] = true;

            healthTotal = 5;
            equipmentTotal = 10;
            currentGuns[0] = rangeWeapon.laserpistol;
            currentGuns[1] = rangeWeapon.antiEctoPlasmator;
            currentGuns[2] = rangeWeapon.PlasmaCannon;


            #region Unlockable Variables
            //Unlockable variables

            // Laser Pistol
            LaserPistolUnlock = true;
            LaserPistolUpgrade1Unlock = false;
            LaserPistolUpgrade2Unlock = false;
            LaserPistolUpgrade3Unlock = false;

            // Anti Ectoplasm Splatter Gun
            AntiEctoGunUnlock = true;
            AntiEctoGunUpgrade1Unlock = false;
            AntiEctoGunUpgrade2Unlock = false;
            AntiEctoGunUpgrade3Unlock = false;

            // Plasma Pistol
            PlasmaPistolUnlock = true;
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

            // xenon pulser
            XenonPulserUnlock = false;
            XenonPulserUpgrade1Unlock = false;
            XenonPulserUpgrade2Unlock = false;
            XenonPulserUpgrade3Unlock = false;

            // antimatter particle
            AntiMatterParticleUnlock = false;
            AntiMatterParticleUpgrade1Unlock = false;
            AntiMatterParticleUpgrade2Unlock = false;
            AntiMatterParticleUpgrade3Unlock = false;

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
    public bool[] unlockedLevels;
    public int screws; // Currency Variable
    public int experience; // Secondary Currency Variable
    public int blueprints; // Tertiary Variable
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

    // Xenon Pulser
    public bool XenonPulserUnlock;
    public bool XenonPulserUpgrade1Unlock;
    public bool XenonPulserUpgrade2Unlock;
    public bool XenonPulserUpgrade3Unlock;

    // Antimatter Particle Gun
    public bool AntiMatterParticleUnlock;
    public bool AntiMatterParticleUpgrade1Unlock;
    public bool AntiMatterParticleUpgrade2Unlock;
    public bool AntiMatterParticleUpgrade3Unlock;

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

