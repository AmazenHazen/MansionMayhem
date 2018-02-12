using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadOutButtonScript : MonoBehaviour {

    // Var to see if it is true
    public bool selected;

    // weaponSlot number for weapons
    public int weaponSlot;
    public rangeWeapon buttonWeapon;

    public Equipment buttonEquipment;
    public trinkets buttontrinket;

	// Use this for initialization
	void Start ()
    {
        // Unlock the variable
        switch (buttonWeapon)
        {

            // Gun Unlocks
            // Laser Pistol
            case rangeWeapon.laserpistol:
                if (GameManager.instance.LaserPistolUnlock == true)
                {
                    gameObject.GetComponent<Button>().interactable = true;
                    gameObject.GetComponent<LoadOutButtonScript>().selected = false;
                }
                break;

            // Anti Ectoplasm Splatter Gun
            case Unlock.AntiEctoGun:
                if (GameManager.instance.LaserPistolUnlock == true)
                {
                    gameObject.GetComponent<Button>().interactable = true;
                    gameObject.GetComponent<LoadOutButtonScript>().selected = false;
                }
                break;
   
            // Plasma Pistol
            case Unlock.PlasmaPistol:
                if (GameManager.instance.PlasmaPistolUnlock == true)
                { unlockedBool = true; }
                break;


            // CryoGun
            case Unlock.CryoGun:
                if (GameManager.instance.CryoGunUnlock == true)
                { unlockedBool = true; }
                break;

            // Flamethrower
            case Unlock.FlameThrower:
                if (GameManager.instance.FlameThrowerUnlock == true)
                { unlockedBool = true; }
                break;


            // Hellfire Shotgun
            case Unlock.HellFireShotgun:
                if (GameManager.instance.HellFireShotgunUnlock == true)
                { unlockedBool = true; }
                break;


            // Sound Cannon
            case Unlock.SoundCannon:
                if (GameManager.instance.SoundCannonUnlock == true)
                { unlockedBool = true; }
                break;


            // Dark Energy Sniper
            case Unlock.DarkEnergySniper:
                if (GameManager.instance.DarkEnergySniperUnlock == true)
                { unlockedBool = true; }
                break;



            // Electron Cannon
            case Unlock.ElectronPulseCannon:
                if (GameManager.instance.ElectronPulseCannonUnlock == true)
                { unlockedBool = true; }
                break;


            // Aetherlight Bow
            case Unlock.AetherlightBow:
                if (GameManager.instance.AetherlightBowUnlock == true)
                { unlockedBool = true; }
                break;



            case Unlock.CelestialRepeater:
                if (GameManager.instance.CelestialRepeaterUnlock == true)
                { unlockedBool = true; }
                break;

        }
	
}
