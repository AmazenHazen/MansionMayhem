using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkBenchItem : MonoBehaviour
{
    [SerializeField]
    private int cost;
    public Text costText;

    // To hold other button variables to check to make the buttons interactable or not
    public GameObject gunUnlock;
    public List<GameObject> upgradeVars;


    // Var to see if it is true
    public bool unlockedBool;

    [SerializeField]
    private Unlock unlockVar;

    Color boughtColor = new Color(.55f, .85f, .245f);


    public void Start()
    {
        #region Switch setting buttons to unlocked or locked depending on the save variable
        // Unlock the variable
        switch (unlockVar)
        {
            case Unlock.heartIncrease:
                if(GameManager.instance.healthTotal> GameManager.instance.MAX_HEALTH)
                { unlockedBool = true;}
                break;

            case Unlock.equipmentIncrease:
                if (GameManager.instance.healthTotal > GameManager.instance.MAX_EQUIPMENT)
                { unlockedBool = true; }
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
                break;
            case Unlock.LaserPistolUpgrade3:
                if(GameManager.instance.LaserPistolUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.AntiEctoGunUpgrade3:
                if (GameManager.instance.AntiEctoGunUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.PlasmaPistolUpgrade3:
                if (GameManager.instance.PlasmaPistolUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.CryoGunUpgrade3:
                if (GameManager.instance.CryoGunUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.FlameThrowerUpgrade3:
                if (GameManager.instance.FlameThrowerUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.HellFireShotgunUpgrade3:
                if (GameManager.instance.HellFireShotgunUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.SoundCannonUpgrade3:
                if (GameManager.instance.SoundCannonUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.DarkEnergySniperUpgrade3:
                if (GameManager.instance.DarkEnergySniperUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.ElectronPulseCannonUpgrade3:
                if (GameManager.instance.ElectronPulseCannonUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.AetherlightBowUpgrade3:
                if (GameManager.instance.AetherlightBowUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.CelestialRepeaterUpgrade3:
                if (GameManager.instance.CelestialRepeaterUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.XenonPulserUpgrade3:
                if (GameManager.instance.XenonPulserUpgrade3Unlock == true)
                { unlockedBool = true; }
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
                break;
            case Unlock.AntiMatterParticleUpgrade3:
                if (GameManager.instance.AntiMatterParticleUpgrade3Unlock== true)
                { unlockedBool = true; }
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
        if (unlockVar == Unlock.heartIncrease && (GameManager.instance.healthTotal < GameManager.instance.MAX_HEALTH))
        {
            cost = (GameManager.instance.healthTotal - 3) * 200;
        }
        else if (unlockVar == Unlock.equipmentIncrease && (GameManager.instance.equipmentTotal < GameManager.instance.MAX_EQUIPMENT))
        {
            cost = GameManager.instance.equipmentTotal * 500;
        }
        #endregion


        // Set the Cost text
        costText.text = "Cost: " + cost + " screws";

        if ((unlockVar == Unlock.heartIncrease && GameManager.instance.healthTotal >= GameManager.instance.MAX_HEALTH) || (unlockVar == Unlock.equipmentIncrease && (GameManager.instance.equipmentTotal >= GameManager.instance.MAX_EQUIPMENT)))
        {
            costText.text = "Purchased";
        }


        Debug.Log(gameObject + "Start");

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
        if (unlockedBool && gameObject.GetComponent<Button>().interactable)
        {
            // Set the color to show it is bought
            gameObject.GetComponent<Image>().color = boughtColor;
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
}
