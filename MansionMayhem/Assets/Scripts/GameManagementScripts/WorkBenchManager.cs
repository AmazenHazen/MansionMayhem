using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkBenchManager : MonoBehaviour
{

    // Attributes
    public List<GameObject> upgradePanels;


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
       if(GameManager.instance.screws<buyingButton.GetComponent<WorkBenchItem>().Cost)
        {
            Debug.Log("You do not have enough to build this item");
            return;
        } 
       else
        {
            #region Switch for unlocking variables
            // Unlock the variable
            switch (buyingButton.GetComponent<WorkBenchItem>().UnlockVar)
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
                    break;
                case Unlock.CryoGunUpgrade3:
                    break;
                // Flamethrower
                case Unlock.FlameThrower:
                    break;
                case Unlock.FlameThrowerUpgrade1:
                    break;
                case Unlock.FlameThrowerUpgrade2:
                    break;
                case Unlock.FlameThrowerUpgrade3:
                    break;

                // Hellfire Shotgun
                case Unlock.HellFireShotgun:
                    break;
                case Unlock.HellFireShotgunUpgrade1:
                    break;
                case Unlock.HellFireShotgunUpgrade2:
                    break;
                case Unlock.HellFireShotgunUpgrade3:
                    break;

                // Sound Cannon
                case Unlock.SoundCannon:
                    break;
                case Unlock.SoundCannonUpgrade1:
                    break;
                case Unlock.SoundCannonUpgrade2:
                    break;
                case Unlock.SoundCannonUpgrade3:
                    break;

                // Dark Energy Sniper
                case Unlock.DarkEnergySniper:
                    break;
                case Unlock.DarkEnergySniperUpgrade1:
                    break;
                case Unlock.DarkEnergySniperUpgrade2:
                    break;
                case Unlock.DarkEnergySniperUpgrade3:
                    break;


                // Electron Cannon
                case Unlock.ElectronPulseCannon:
                    break;
                case Unlock.ElectronPulseCannonUpgrade1:
                    break;
                case Unlock.ElectronPulseCannonUpgrade2:
                    break;
                case Unlock.ElectronPulseCannonUpgrade3:
                    break;

                // Aetherlight Bow
                case Unlock.AetherlightBow:

                    break;
                case Unlock.AetherlightBowUpgrade1:
                    break;
                case Unlock.AetherlightBowUpgrade2:
                    break;
                case Unlock.AetherlightBowUpgrade3:
                    break;


                case Unlock.CelestialRepeater:
                    break;
                case Unlock.CelestialRepeaterUpgrade1:
                    break;
                case Unlock.CelestialRepeaterUpgrade2:
                    break;
                case Unlock.CelestialRepeaterUpgrade3:
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

            // Subtract the amount from the player's screws
            GameManager.instance.screws -= buyingButton.GetComponent<WorkBenchItem>().Cost;
        }

    }
}
