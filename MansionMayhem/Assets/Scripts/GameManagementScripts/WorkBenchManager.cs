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
       if(GameManager.screws<buyingButton.GetComponent<WorkBenchItem>().Cost)
        {
            Debug.Log("You do not have enough to build this item");
            return;
        } 
       else
        {
            // Unlock the variable
            switch(buyingButton.GetComponent<WorkBenchItem>().UnlockVar)
            {
                case Unlock.heartIncrease:

                    break;

                case Unlock.equipmentIncrease:
                    break;

                // Gun Unlocks
                // Laser Pistol
                case Unlock.LaserPistol:
                    break;
                case Unlock.LaserPistolUpgrade1:
                    break;
                case Unlock.LaserPistolUpgrade2:
                    break;
                case Unlock.LaserPistolUpgrade3:
                    break;

                // Anti Ectoplasm Splatter Gun
                case Unlock.AntiEctoGun:
                    break;
                case Unlock.AntiEctoGunUpgrade1:
                    break;
                case Unlock.AntiEctoGunUpgrade2:
                    break;
                case Unlock.AntiEctoGunUpgrade3:
                    break;

                // Plasma Pistol
                case Unlock.PlasmaPistol:
                    break;
                case Unlock.PlasmaPistolUpgrade1:
                    break;
                case Unlock.PlasmaPistolUpgrade2:
                    break;
                case Unlock.PlasmaPistolUpgrade3:
                    break;

                // CryoGun
                case Unlock.CryoGun:
                    break;
                case Unlock.CryoGunUpgrade1:
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

            // Subtract the amount from the player's screws
            GameManager.screws= GameManager.screws - buyingButton.GetComponent<WorkBenchItem>().Cost;
        }

    }

}
