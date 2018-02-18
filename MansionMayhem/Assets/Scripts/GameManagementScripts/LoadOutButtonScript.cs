using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadOutButtonScript : MonoBehaviour
{

    // Var to see if it is true
    public bool selected;
    public bool locked;

    // weaponSlot number for weapons
    public int weaponSlot;
    public rangeWeapon buttonWeapon;

    public Equipment buttonEquipment;
    public trinkets buttontrinket;

    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    Color selectedColor = new Color(.55f, .85f, .245f);

    // Use this for initialization
    void Start()
    {
        checkUnlocked();
    }


    public void checkUnlocked()
    {
        #region Checking if the guns/trinkets/equipement are unlocked
        // Unlock the variable
        switch (buttonWeapon)
        {
            // Gun Unlocks
            // Laser Pistol
            case rangeWeapon.laserpistol:
                if (!GameManager.instance.LaserPistolUnlock)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;

            // Anti Ectoplasm Splatter Gun
            case rangeWeapon.antiEctoPlasmator:
                if (GameManager.instance.AntiEctoGunUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;

            // Plasma Pistol
            case rangeWeapon.PlasmaCannon:
                if (GameManager.instance.PlasmaPistolUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;


            // CryoGun
            case rangeWeapon.cryoGun:
                if (GameManager.instance.CryoGunUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
        
                break;

            // Flamethrower
            case rangeWeapon.flamethrower:
                if (GameManager.instance.FlameThrowerUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;


            // Hellfire Shotgun
            case rangeWeapon.hellfireshotgun:
                if (GameManager.instance.HellFireShotgunUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;


            // Sound Cannon
            case rangeWeapon.soundCannon:
                if (GameManager.instance.SoundCannonUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;


            // Dark Energy Sniper
            case rangeWeapon.DarkEnergyRifle:
                if (GameManager.instance.DarkEnergySniperUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;



            // Electron Cannon
            case rangeWeapon.ElectronSeeker:
                if (GameManager.instance.ElectronPulseCannonUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;


            // Aetherlight Bow
            case rangeWeapon.aetherLightBow:
                if (GameManager.instance.AetherlightBowUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;



            case rangeWeapon.CelestialRepeater:
                if (GameManager.instance.CelestialRepeaterUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;

            case rangeWeapon.XenonPulser:
                if (GameManager.instance.XenonPulserUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;

            case rangeWeapon.AntimatterParticle:
                if (GameManager.instance.AntiMatterParticleUnlock != true)
                {
                    gameObject.GetComponent<Button>().interactable = false;

                }
                else
                {
                    gameObject.GetComponent<Button>().interactable = true;
                }
                break;

        #endregion
        }
        // turn off the button being selected
        selected = false;
    }
}
