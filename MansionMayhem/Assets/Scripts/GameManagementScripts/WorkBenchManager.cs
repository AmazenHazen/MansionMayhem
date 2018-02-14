using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkBenchManager : MonoBehaviour
{
    // Attributes
    public List<GameObject> upgradePanels; // holds the different panels for weapons, trinkets, and equipement

    Color boughtColor = new Color(.55f, .85f, .245f);


    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    /// <summary>
    /// Method for changing the selection for upgrades
    /// </summary>
    /// <param name="panelSelected"></param>
    public void SwitchSection(GameObject panelSelected)
    {
        // Hide the other Panels
        for(int i=0; i<upgradePanels.Count; i++)
        {
            // Loop through the panels and select the one that is passed in
            if(panelSelected == upgradePanels[i])
            {
                upgradePanels[i].SetActive(true);
            }
            else
            {
                upgradePanels[i].SetActive(false);
            }
        }


    }

    public void purchaseItem(GameObject buyingButton)
    {
       Unlock unlockItem = buyingButton.GetComponent<WorkBenchItem>().UnlockVar;

       if (GameManager.instance.screws < buyingButton.GetComponent<WorkBenchItem>().Cost)
        {
            Debug.Log("You do not have enough to build this item");
            return;
        } 
       else
        {
            // Subtract the amount from the player's screws
            GameManager.instance.screws -= buyingButton.GetComponent<WorkBenchItem>().Cost;

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
                    GameManager.instance.FlameThrowerUpgrade1Unlock= true;
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
            if(buyingButton.GetComponent<WorkBenchItem>().upgradeVars.Count>0)
            {
                for(int i=0; i< buyingButton.GetComponent<WorkBenchItem>().upgradeVars.Count; i++)
                {
                    buyingButton.GetComponent<WorkBenchItem>().upgradeVars[i].GetComponent<Button>().interactable = true;
                }
            }

            #endregion

            #region Dynamic unlocks
            // IF the unlock is a heart increase and healthtotal<maxhealth or the same with equipement, then keep the button interactable
            if (unlockItem == Unlock.heartIncrease && (GameManager.instance.healthTotal<GameManager.instance.MAX_HEALTH))
            {
                buyingButton.GetComponent<WorkBenchItem>().Cost = GameManager.instance.healthTotal * 200;

                // lock it if you max health
                if(GameManager.instance.healthTotal<=GameManager.instance.MAX_HEALTH)
                {
                    // Make the button uninteractable
                    buyingButton.GetComponent<Button>().interactable = false;
                }
            }   
            else if(unlockItem == Unlock.equipmentIncrease && (GameManager.instance.equipmentTotal < GameManager.instance.MAX_EQUIPMENT))
            {
                buyingButton.GetComponent<WorkBenchItem>().Cost = GameManager.instance.equipmentTotal * 500;

                // lock it if you max equipement
                if (GameManager.instance.equipmentTotal <= GameManager.instance.MAX_EQUIPMENT)
                {
                    // Make the button uninteractable
                    buyingButton.GetComponent<Button>().interactable = false;
                }
            }
            #endregion

            else
            {
                // Set the color to show it is bought
                buyingButton.GetComponent<Image>().color = boughtColor;

                // Make the button uninteractable
                buyingButton.GetComponent<Button>().interactable = false;

                // Set the unlockedBool to true
                buyingButton.GetComponent<WorkBenchItem>().unlockedBool = true; 
            }

        }

    }

}
