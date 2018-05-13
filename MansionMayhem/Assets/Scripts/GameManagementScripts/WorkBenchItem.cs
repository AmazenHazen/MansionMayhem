using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkBenchItem : MonoBehaviour
{
    // Constants for health and equipement
    const int MAX_HEALTH = 30;
    const int MAX_EQUIPMENT = 3;

    // Cost and text
    [SerializeField]
    private int cost;     // Cost of the item to unlock
    public Text costText; // "Click to unlock" or "Unlocked"

    // To hold other button variables to check to make the buttons interactable or not
    public GameObject gunUnlock;        // If an upgraded gun it points to the gun needed to unlock
    public List<GameObject> upgradeVars;// If unlocking a gun it points to the 3 unlockable upgrades

    // Var to see if it is true
    public bool unlockedBool;           // Checks to see if the gun is unlocked from the save file
    private bool lockedForDemo;         // Specific temporary bool for if the item can be unlocked for the demo

    [SerializeField]
    private Unlock unlockVar;           // Variable that is unlocked

    Color boughtColor = new Color(.55f, .85f, .245f);   // color that the button changes to once unlocked


    public void Start()
    {
        lockedForDemo = false;

        // Set up the onClick Event for every button
        GetComponent<Button>().onClick.AddListener(PurchaseItem);

        #region Switch setting buttons to unlocked or locked depending on the save variable
        // Unlock the variable
        switch (unlockVar)
        {
            case Unlock.heartIncrease:
                if(GameManager.instance.healthTotal> MAX_HEALTH)
                { unlockedBool = true;}
                break;

            case Unlock.equipmentIncrease:
                if (GameManager.instance.healthTotal > MAX_EQUIPMENT)
                { unlockedBool = true; }
                lockedForDemo = true;
                break;

            // Gun Unlocks
            // Laser Pistol
            case Unlock.LaserPistol:
                if (GameManager.instance.LaserPistolUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.LaserPistolUpgrade1:
                if(GameManager.instance.LaserPistolUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.LaserPistolUpgrade2:
                if(GameManager.instance.LaserPistolUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.LaserPistolUpgrade3:
                if(GameManager.instance.LaserPistolUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;

            // Anti Ectoplasm Splatter Gun
            case Unlock.AntiEctoGun:
                if (GameManager.instance.AntiEctoGunUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.AntiEctoGunUpgrade1:
                if (GameManager.instance.AntiEctoGunUpgrade1Unlock== true)
                { unlockedBool = true; }
                break;
            case Unlock.AntiEctoGunUpgrade2:
                if (GameManager.instance.AntiEctoGunUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.AntiEctoGunUpgrade3:
                if (GameManager.instance.AntiEctoGunUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;

            // Plasma Pistol
            case Unlock.PlasmaPistol:
                if (GameManager.instance.PlasmaPistolUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.PlasmaPistolUpgrade1:
                if (GameManager.instance.PlasmaPistolUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.PlasmaPistolUpgrade2:
                if (GameManager.instance.PlasmaPistolUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.PlasmaPistolUpgrade3:
                if (GameManager.instance.PlasmaPistolUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;

            // CryoGun
            case Unlock.CryoGun:
                if(GameManager.instance.CryoGunUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.CryoGunUpgrade1:
                if (GameManager.instance.CryoGunUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.CryoGunUpgrade2:
                if (GameManager.instance.CryoGunUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.CryoGunUpgrade3:
                if (GameManager.instance.CryoGunUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            // Flamethrower
            case Unlock.FlameThrower:
                    if(GameManager.instance.FlameThrowerUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.FlameThrowerUpgrade1:
                if (GameManager.instance.FlameThrowerUpgrade1Unlock == true)

                { unlockedBool = true; }
                break;
            case Unlock.FlameThrowerUpgrade2:
                if (GameManager.instance.FlameThrowerUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.FlameThrowerUpgrade3:
                if (GameManager.instance.FlameThrowerUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;

            // Hellfire Shotgun
            case Unlock.HellFireShotgun:
                if (GameManager.instance.HellFireShotgunUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.HellFireShotgunUpgrade1:
                if (GameManager.instance.HellFireShotgunUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.HellFireShotgunUpgrade2:
                if (GameManager.instance.HellFireShotgunUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.HellFireShotgunUpgrade3:
                if (GameManager.instance.HellFireShotgunUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;

            // Sound Cannon
            case Unlock.SoundCannon:
                if (GameManager.instance.SoundCannonUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.SoundCannonUpgrade1:
                if (GameManager.instance.SoundCannonUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.SoundCannonUpgrade2:
                if (GameManager.instance.SoundCannonUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.SoundCannonUpgrade3:
                if (GameManager.instance.SoundCannonUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;

            // Dark Energy Sniper
            case Unlock.DarkEnergySniper:
                if (GameManager.instance.DarkEnergySniperUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.DarkEnergySniperUpgrade1:
                if (GameManager.instance.DarkEnergySniperUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.DarkEnergySniperUpgrade2:
                if (GameManager.instance.DarkEnergySniperUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.DarkEnergySniperUpgrade3:
                if (GameManager.instance.DarkEnergySniperUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;


            // Electron Cannon
            case Unlock.ElectronPulseCannon:
                if (GameManager.instance.ElectronPulseCannonUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.ElectronPulseCannonUpgrade1:
                if (GameManager.instance.ElectronPulseCannonUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.ElectronPulseCannonUpgrade2:
                if (GameManager.instance.ElectronPulseCannonUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.ElectronPulseCannonUpgrade3:
                if (GameManager.instance.ElectronPulseCannonUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;

            // Aetherlight Bow
            case Unlock.AetherlightBow:
                if (GameManager.instance.AetherlightBowUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.AetherlightBowUpgrade1:
                if (GameManager.instance.AetherlightBowUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.AetherlightBowUpgrade2:
                if (GameManager.instance.AetherlightBowUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.AetherlightBowUpgrade3:
                if (GameManager.instance.AetherlightBowUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;


            case Unlock.CelestialRepeater:
                if (GameManager.instance.CelestialRepeaterUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.CelestialRepeaterUpgrade1:
                if (GameManager.instance.CelestialRepeaterUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.CelestialRepeaterUpgrade2:
                if (GameManager.instance.CelestialRepeaterUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.CelestialRepeaterUpgrade3:
                if (GameManager.instance.CelestialRepeaterUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;

            case Unlock.XenonPulser:
                if (GameManager.instance.XenonPulserUnlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.XenonPulserUpgrade1:
                if (GameManager.instance.XenonPulserUpgrade1Unlock== true)
                { unlockedBool = true; }
                break;
            case Unlock.XenonPulserUpgrade2:
                if (GameManager.instance.XenonPulserUpgrade2Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.XenonPulserUpgrade3:
                if (GameManager.instance.XenonPulserUpgrade3Unlock == true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;

            case Unlock.AntiMatterParticle:
                if (GameManager.instance.AntiMatterParticleUnlock== true)
                { unlockedBool = true; }
                break;
            case Unlock.AntiMatterParticleUpgrade1:
                if (GameManager.instance.AntiMatterParticleUpgrade1Unlock == true)
                { unlockedBool = true; }
                break;
            case Unlock.AntiMatterParticleUpgrade2:
                if (GameManager.instance.AntiMatterParticleUpgrade2Unlock== true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;
            case Unlock.AntiMatterParticleUpgrade3:
                if (GameManager.instance.AntiMatterParticleUpgrade3Unlock== true)
                { unlockedBool = true; }
                // this is only for the demo to lock unlock 2 &3 for guns
                lockedForDemo = true;
                break;



            // Trinket Unlocks
            case Unlock.AntiMatterDevice:
                { unlockedBool = true; }
                break;
            case Unlock.PortalDevice:
                { unlockedBool = true; }
                break;
            case Unlock.NanoBotHealingSwarm:
                { unlockedBool = true; }
                break;
            case Unlock.Drone:
                { unlockedBool = true; }
                break;
            case Unlock.HologramClone:
                { unlockedBool = true; }
                break;
            case Unlock.Shield:
                { unlockedBool = true; }
                break;
            case Unlock.BootRockets:
                { unlockedBool = true; }
                break;

            // Equipment Unlocks
            case Unlock.FireResistantUnderArmor:
                { unlockedBool = true; }
                break;
            case Unlock.HeatedCoatLining:
                { unlockedBool = true; }
                break;
            case Unlock.FrictionBoots:
                { unlockedBool = true; }
                break;
            case Unlock.AntidotePatch:
                { unlockedBool = true; }
                break;
            case Unlock.ScrewMagnet:
                { unlockedBool = true; }
                break;
            case Unlock.RoboticHeart:
                { unlockedBool = true; }
                break;
            case Unlock.DragonscaleArmor:
                { unlockedBool = true; }
                break;
            case Unlock.GooRepellingTreatment:
                { unlockedBool = true; }
                break;

        }
        #endregion

        #region Setting Dynamically Changing cost variables
        // Set the cost for dynamic costing unlocks
        if (unlockVar == Unlock.heartIncrease && (GameManager.instance.healthTotal < MAX_HEALTH))
        {
            cost = (GameManager.instance.healthTotal - 3) * 200;
        }
        else if (unlockVar == Unlock.equipmentIncrease && (GameManager.instance.equipmentTotal < MAX_EQUIPMENT))
        {
            cost = GameManager.instance.equipmentTotal * 500;
        }
        else if(unlockVar == Unlock.heartIncrease || unlockVar == Unlock.equipmentIncrease)
        {
            unlockedBool = true;
        }
        #endregion


        // Set the Cost text
        costText.text = "Cost: " + cost + " screws";

        if ((unlockVar == Unlock.heartIncrease && GameManager.instance.healthTotal >= MAX_HEALTH) || (unlockVar == Unlock.equipmentIncrease && (GameManager.instance.equipmentTotal >= MAX_EQUIPMENT)))
        {
            costText.text = "Purchased";
        }


        //Debug.Log(gameObject + "Start");

        // check if the gun unlock is unlocked before allowing purchasing of the upgrades

        // This is if the gun has been unlocked associated with the upgrades
        if (upgradeVars.Count>0 && unlockedBool == true)
        {
            for (int i = 0; i < upgradeVars.Count; i++)
            {
                upgradeVars[i].GetComponent<Button>().interactable = true;
            }
        }


        // Set the button as unlocked if the unlockedBool is true
        if (unlockedBool)
        {
            costText.text = "Purchased";

            // Set the color to show it is bought
            gameObject.GetComponent<Image>().color = boughtColor;
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    private void Update()
    {
        if ((unlockedBool||lockedForDemo) && gameObject.GetComponent<Button>().interactable)
        {
            // Set the color to show it is bought
            if (!lockedForDemo)
            {
                gameObject.GetComponent<Image>().color = boughtColor;
            }
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public int Cost
    {
        get { return cost; }
        set { cost = value; }

    }

    public Unlock UnlockVar
    {
        get { return unlockVar; }
    }

    public void PurchaseItem()
    {
        Debug.Log("You purchased an item!");
        Unlock unlockItem = unlockVar;

        if (GameManager.instance.screws < cost)
        {
            //Debug.Log("You do not have enough to build this item");
            return;
        }
        else
        {
            // Subtract the amount from the player's screws
            GameManager.instance.screws -= cost;

            #region Switch for unlocking variables
            // Unlock the variable
            switch (unlockItem)
            {
                case Unlock.heartIncrease:
                    GameManager.instance.healthTotal++;

                    // Increase max health
                    break;

                case Unlock.equipmentIncrease:
                    GameManager.instance.equipmentTotal++;

                    // Increase equipment
                    break;

                // Gun Unlocks
                // Laser Pistol
                case Unlock.LaserPistol:
                    GameManager.instance.LaserPistolUnlock = true;
                    break;
                case Unlock.LaserPistolUpgrade1:
                    GameManager.instance.LaserPistolUpgrade1Unlock = true;
                    break;
                case Unlock.LaserPistolUpgrade2:
                    GameManager.instance.LaserPistolUpgrade2Unlock = true;
                    break;
                case Unlock.LaserPistolUpgrade3:
                    GameManager.instance.LaserPistolUpgrade3Unlock = true;
                    break;

                // Anti Ectoplasm Splatter Gun
                case Unlock.AntiEctoGun:
                    GameManager.instance.AntiEctoGunUnlock = true;
                    break;
                case Unlock.AntiEctoGunUpgrade1:
                    GameManager.instance.AntiEctoGunUpgrade1Unlock = true;
                    break;
                case Unlock.AntiEctoGunUpgrade2:
                    GameManager.instance.AntiEctoGunUpgrade2Unlock = true;
                    break;
                case Unlock.AntiEctoGunUpgrade3:
                    GameManager.instance.AntiEctoGunUpgrade3Unlock = true;
                    break;

                // Plasma Pistol
                case Unlock.PlasmaPistol:
                    GameManager.instance.PlasmaPistolUnlock = true;
                    break;
                case Unlock.PlasmaPistolUpgrade1:
                    GameManager.instance.PlasmaPistolUpgrade1Unlock = true;
                    break;
                case Unlock.PlasmaPistolUpgrade2:
                    GameManager.instance.PlasmaPistolUpgrade2Unlock = true;
                    break;
                case Unlock.PlasmaPistolUpgrade3:
                    GameManager.instance.PlasmaPistolUpgrade3Unlock = true;
                    break;

                // CryoGun
                case Unlock.CryoGun:
                    GameManager.instance.CryoGunUnlock = true;
                    break;
                case Unlock.CryoGunUpgrade1:
                    GameManager.instance.CryoGunUpgrade1Unlock = true;
                    break;
                case Unlock.CryoGunUpgrade2:
                    GameManager.instance.CryoGunUpgrade2Unlock = true;

                    break;
                case Unlock.CryoGunUpgrade3:
                    GameManager.instance.CryoGunUpgrade3Unlock = true;

                    break;
                // Flamethrower
                case Unlock.FlameThrower:
                    GameManager.instance.FlameThrowerUnlock = true;
                    break;
                case Unlock.FlameThrowerUpgrade1:
                    GameManager.instance.FlameThrowerUpgrade1Unlock = true;
                    break;
                case Unlock.FlameThrowerUpgrade2:
                    GameManager.instance.FlameThrowerUpgrade2Unlock = true;
                    break;
                case Unlock.FlameThrowerUpgrade3:
                    GameManager.instance.FlameThrowerUpgrade3Unlock = true;
                    break;

                // Hellfire Shotgun
                case Unlock.HellFireShotgun:
                    GameManager.instance.HellFireShotgunUnlock = true;
                    break;
                case Unlock.HellFireShotgunUpgrade1:
                    GameManager.instance.HellFireShotgunUpgrade1Unlock = true;
                    break;
                case Unlock.HellFireShotgunUpgrade2:
                    GameManager.instance.HellFireShotgunUpgrade2Unlock = true;
                    break;
                case Unlock.HellFireShotgunUpgrade3:
                    GameManager.instance.HellFireShotgunUpgrade3Unlock = true;
                    break;

                // Sound Cannon
                case Unlock.SoundCannon:
                    GameManager.instance.SoundCannonUnlock = true;
                    break;
                case Unlock.SoundCannonUpgrade1:
                    GameManager.instance.SoundCannonUpgrade1Unlock = true;
                    break;
                case Unlock.SoundCannonUpgrade2:
                    GameManager.instance.SoundCannonUpgrade2Unlock = true;
                    break;
                case Unlock.SoundCannonUpgrade3:
                    GameManager.instance.SoundCannonUpgrade3Unlock = true;
                    break;

                // Dark Energy Sniper
                case Unlock.DarkEnergySniper:
                    GameManager.instance.DarkEnergySniperUnlock = true;
                    break;
                case Unlock.DarkEnergySniperUpgrade1:
                    GameManager.instance.DarkEnergySniperUpgrade1Unlock = true;
                    break;
                case Unlock.DarkEnergySniperUpgrade2:
                    GameManager.instance.DarkEnergySniperUpgrade2Unlock = true;
                    break;
                case Unlock.DarkEnergySniperUpgrade3:
                    GameManager.instance.DarkEnergySniperUpgrade3Unlock = true;
                    break;


                // Electron Cannon
                case Unlock.ElectronPulseCannon:
                    GameManager.instance.ElectronPulseCannonUnlock = true;
                    break;
                case Unlock.ElectronPulseCannonUpgrade1:
                    GameManager.instance.ElectronPulseCannonUpgrade1Unlock = true;
                    break;
                case Unlock.ElectronPulseCannonUpgrade2:
                    GameManager.instance.ElectronPulseCannonUpgrade2Unlock = true;
                    break;
                case Unlock.ElectronPulseCannonUpgrade3:
                    GameManager.instance.ElectronPulseCannonUpgrade3Unlock = true;
                    break;

                // Aetherlight Bow
                case Unlock.AetherlightBow:
                    GameManager.instance.AetherlightBowUnlock = true;
                    break;
                case Unlock.AetherlightBowUpgrade1:
                    GameManager.instance.AetherlightBowUpgrade1Unlock = true;
                    break;
                case Unlock.AetherlightBowUpgrade2:
                    GameManager.instance.AetherlightBowUpgrade2Unlock = true;
                    break;
                case Unlock.AetherlightBowUpgrade3:
                    GameManager.instance.AetherlightBowUpgrade3Unlock = true;
                    break;


                case Unlock.CelestialRepeater:
                    GameManager.instance.CelestialRepeaterUnlock = true;
                    break;
                case Unlock.CelestialRepeaterUpgrade1:
                    GameManager.instance.CelestialRepeaterUpgrade1Unlock = true;
                    break;
                case Unlock.CelestialRepeaterUpgrade2:
                    GameManager.instance.CelestialRepeaterUpgrade2Unlock = true;
                    break;
                case Unlock.CelestialRepeaterUpgrade3:
                    GameManager.instance.CelestialRepeaterUpgrade3Unlock = true;
                    break;

                case Unlock.XenonPulser:
                    GameManager.instance.XenonPulserUnlock = true;
                    break;
                case Unlock.XenonPulserUpgrade1:
                    GameManager.instance.XenonPulserUpgrade1Unlock = true;
                    break;
                case Unlock.XenonPulserUpgrade2:
                    GameManager.instance.XenonPulserUpgrade2Unlock = true;
                    break;
                case Unlock.XenonPulserUpgrade3:
                    GameManager.instance.XenonPulserUpgrade3Unlock = true;
                    break;

                case Unlock.AntiMatterParticle:
                    GameManager.instance.AntiMatterParticleUnlock = true;
                    break;
                case Unlock.AntiMatterParticleUpgrade1:
                    GameManager.instance.AntiMatterParticleUpgrade1Unlock = true;

                    break;
                case Unlock.AntiMatterParticleUpgrade2:
                    GameManager.instance.AntiMatterParticleUpgrade2Unlock = true;
                    break;
                case Unlock.AntiMatterParticleUpgrade3:
                    GameManager.instance.AntiMatterParticleUpgrade3Unlock = true;
                    break;

                // Trinket Unlocks
                case Unlock.AntiMatterDevice:
                    break;
                case Unlock.PortalDevice:
                    break;
                case Unlock.NanoBotHealingSwarm:
                    break;
                case Unlock.Drone:
                    break;
                case Unlock.HologramClone:
                    break;
                case Unlock.Shield:
                    break;
                case Unlock.BootRockets:
                    break;

                // Equipment Unlocks
                case Unlock.FireResistantUnderArmor:
                    break;
                case Unlock.HeatedCoatLining:
                    break;
                case Unlock.FrictionBoots:
                    break;
                case Unlock.AntidotePatch:
                    break;
                case Unlock.ScrewMagnet:
                    break;
                case Unlock.RoboticHeart:
                    break;
                case Unlock.DragonscaleArmor:
                    break;
                case Unlock.GooRepellingTreatment:
                    break;

            }
            #endregion

            #region Unlock Upgrade Varaibles
            if (upgradeVars.Count > 0)
            {
                for (int i = 0; i < upgradeVars.Count; i++)
                {
                    upgradeVars[i].GetComponent<Button>().interactable = true;
                }
            }

            #endregion

            #region Dynamic unlocks
            // IF the unlock is a heart increase and healthtotal<maxhealth or the same with equipement, then keep the button interactable
            if (unlockItem == Unlock.heartIncrease && (GameManager.instance.healthTotal < MAX_HEALTH))
            {
                // Set a temporary cost variable
                int tempCost = (GameManager.instance.healthTotal - 3) * 200;

                cost = tempCost;
                costText.text = "Cost: " + tempCost + " screws";
            }
            else if (unlockItem == Unlock.equipmentIncrease && (GameManager.instance.equipmentTotal < MAX_EQUIPMENT))
            {
                // Set a temporary cost variable
                int tempCost = GameManager.instance.equipmentTotal * 500;

                // Set the button cost and text to reflect it
                Cost = tempCost;
                costText.text = "Cost: " + tempCost + " screws";

                // Set the Cost text
                costText.text = "Cost: " + tempCost + " screws";
            }
            else
            {
                // Set the color to show it is bought
                GetComponent<Image>().color = boughtColor;

                // Make the button uninteractable
                GetComponent<Button>().interactable = false;
                costText.text = "Purchased";

                // Set the unlockedBool to true
                unlockedBool = true;
            }
            #endregion


        }

    }
}
