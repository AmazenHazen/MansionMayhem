using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Headers for Save Files
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    #region PlayerAttributes
    // Player's Attributes
    private float maxLife;
    private float currentLife;
    private float shieldMaxLife;
    private float shieldLife;
    private bool invincibility;
    private bool canTravel;
    private bool canShoot;
    private bool canMelee;
    private bool canShield;

    // Status Conditions
    int poisonCounter;
    bool applypoison;
    bool poisoned;

    // Weapon Variables
    private rangeWeapon currentRangeWeapon;
    private meleeWeapon currentMeleeWeapon;
    private trinkets currentTrinket;
    public List<GameObject> playerBullets;
    public List<GameObject> playerBulletPrefabs;
    private int bulletCount;
    #endregion

    #region PlayerProperties
    public float CurrentLife
    {
        get { return currentLife; }
        set { currentLife = value; }
    }
    public float ShieldLife
    {
        get { return shieldLife; }
        set { shieldLife = value; }
    }

    public int BulletCount
    {
        get { return bulletCount; }
        set { bulletCount = value; }
    }
    public bool Poisoned
    {
        set { poisoned = value; }
    }
    public int PoisonCounter
    {
        set { poisonCounter = value; }
    }

    #endregion

    #region Setting Initial Player Attributes
    // Use this for initialization
    void Start()
    {
        // Player Game Variables
        maxLife = 5;
        currentLife = 3;
        shieldLife = 1;
        invincibility = false;
        canShoot = true;
        canMelee = true;
        canShield = true;
        canTravel = true;
        applypoison = true; // Starts as true, turned to false only if poisoned
        poisoned = false;
        poisonCounter = 0;
        currentRangeWeapon = rangeWeapon.antiEctoPlasmator;
        currentMeleeWeapon = meleeWeapon.silverknife;

        // Initializing Lists
        playerBullets = new List<GameObject>();
    }
    #endregion

    #region Update for Player
    // Update is called once per frame
    void Update()
    {
        CheckStatusConditions();
        WeaponSwitch();
        Shoot();
        Melee();
        Shield();
    }
    #endregion

    #region CollisionDetection
    /// <summary>
    /// Player Collision Handled Here. This includes any objects that is effected by the player colliding with it.
    /// This includes: Screws, Health, Ammo, Enemies, Walls, and Furniture.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        // Get the tag associated with the collision
        switch (collider.tag)
        {
            #region Door

            // Obstacles
            case "door":
                Debug.Log("Door");

                // Move the player to the new room
                if(canTravel == true)
                {
                    Debug.Log("Travel");
                    gameObject.transform.position = collider.gameObject.GetComponent<DoorScript>().linkedDoor.transform.position;
                    // Activate just traveled method
                    JustTraveled();
                }

                break;

            #endregion

            #region Enemy Collision
            // Enemies
            case "enemy":
                if (invincibility == false)
                {

                    Debug.Log("Enemy: " + collider.gameObject.GetComponent<EnemyManager>().Monster);
                    currentLife -= collider.gameObject.GetComponent<EnemyManager>().Damage;

                    // Poison the player if the enemy is poisonous
                    // The Enemy Poisons the player with the melee attack if poisonous
                    if (collider.gameObject.GetComponent<EnemyManager>().IsPoisonous == true)
                    {
                        // Set poison to true and reset the counter
                        poisonCounter = 0;
                        poisoned = true;
                    }

                    StartInvincibility();
                }
                break;
            #endregion

            #region Items
            // Items
            case "item":
                // Debug Line
                Debug.Log("Item: " + collider.gameObject.GetComponent<ItemScript>().itemVar);


                // Make a copy of the type of item for determining what to do with it.
                itemType itemVarCopy = collider.gameObject.GetComponent<ItemScript>().itemVar;
                switch (itemVarCopy)
                {
                    #region Ammo/Health/Screws Pickups
                    case itemType.heartPickup:
                        currentLife++;
                        if (currentLife > maxLife)
                        {
                            currentLife = maxLife;
                        }
                        break;
                    case itemType.healthPotionPickup:
                        currentLife += 3;
                        if(currentLife>maxLife)
                        {
                            currentLife = maxLife;
                        }
                        break;
                    case itemType.normalScrewPickup:
                        GameObject.Find("GameHandler").GetComponent<GameManager>().Screws++;
                        break;
                    case itemType.redScrewPickup:
                        GameObject.Find("GameHandler").GetComponent<GameManager>().Screws += 5;
                        break;
                    case itemType.goldenScrewPickup:
                        GameObject.Find("GameHandler").GetComponent<GameManager>().Screws += 10;
                        break;
                        #endregion
                }

                // At the end of an item collision destroy the gameobject
                Destroy(collider.gameObject);
                break;
            #endregion

            #region workbench
            case "workbench":
                if (Input.GetKeyDown(KeyCode.Space))
                {

                    // Debug Line
                    Debug.Log("Using WorkBench:");
                }

                break;
            #endregion
        }

    }
    #endregion

    #region Invincibility Helper Methods
    /// <summary>
    /// Resets the player to not having invincibility
    /// </summary>
    void ResetInvincibility()
    {
        invincibility = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Starts player's invincibility frames
    /// </summary>
    void StartInvincibility()
    {
        // Player Gains Invincibility for 3 seconds
        invincibility = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        Invoke("ResetInvincibility", 3);
    }
    #endregion

    #region TravelBool Helper Methods
    /// <summary>
    /// Resets the player to not having invincibility
    /// </summary>
    void ResetTravelBool()
    {
        canTravel = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Starts player's invincibility frames
    /// </summary>
    void JustTraveled()
    {
        // Player Gains Invincibility for 3 seconds
        canTravel = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetTravelBool", 3);
        
    }
    #endregion

    #region Combat Helper Methods

    #region Shooting Helper Methods
    /// <summary>
    /// Method that shoots a bullet based on the player's weapon
    /// </summary>
    private void Shoot()
    {
        if (Input.GetMouseButton(0) && canShoot == true)
        {
            GameObject bulletCopy;

            switch (currentRangeWeapon)
            {
                case rangeWeapon.antiEctoPlasmator:
                    bulletCopy = Instantiate(playerBulletPrefabs[1], transform.position, transform.rotation) as GameObject;
                    playerBullets.Add(bulletCopy);
                    bulletCopy.GetComponent<BulletManager>().bulletSetUp(gameObject);
                    bulletCount++;
                    break;
                case rangeWeapon.aetherLightBow:
                    bulletCopy = Instantiate(playerBulletPrefabs[0], transform.position, transform.rotation) as GameObject;
                    playerBullets.Add(bulletCopy);
                    bulletCopy.GetComponent<BulletManager>().bulletSetUp(gameObject);
                    bulletCount++;
                    break;
                case rangeWeapon.cryoGun:
                    bulletCopy = Instantiate(playerBulletPrefabs[2], transform.position, transform.rotation) as GameObject;
                    playerBullets.Add(bulletCopy);
                    bulletCopy.GetComponent<BulletManager>().bulletSetUp(gameObject);
                    bulletCount++;
                    break;

            }
            JustShot();
            bulletCount++;

        }
    }

    /// <summary>
    /// Keeps the player from spamming the shoot button
    /// </summary>
    void JustShot()
    {
        // Player Gains Invincibility for 3 seconds
        canShoot = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        Invoke("ResetShooting", .5f);
    }

    /// <summary>
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetShooting()
    {
        canShoot = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }


    /// <summary>
    /// Switch Range Weapons Helper Method
    /// </summary>
    private void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(currentRangeWeapon == rangeWeapon.cryoGun)
            {
                currentRangeWeapon = 0;
            }
            else
            {
                currentRangeWeapon++;
            }
        }
    }
    #endregion

    #region Melee Helper Methods
    /// <summary>
    /// Method that lets the player melee
    /// </summary>
    private void Melee()
    {
        if (Input.GetMouseButton(1) && canMelee == true)
        {
            // Activate the Melee Collider
            gameObject.transform.FindChild("MeleeAttack").gameObject.SetActive(true);

            // Call the JustMeleed Method
            JustMeleed();
        }
    }

    /// <summary>
    /// Keeps the player from spamming the shoot button
    /// </summary>
    void JustMeleed()
    {
        // Player Gains Invincibility for 3 seconds
        canMelee = false;
        Invoke("ResetMelee", .5f);
    }

    /// <summary>
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetMelee()
    {
        // De-activates melee collider
        gameObject.transform.FindChild("MeleeAttack").gameObject.SetActive(false);
        canMelee = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion

    #region Shield Helper Method
    /// <summary>
    /// Method that shoots a bullet based on the player's weapon
    /// </summary>
    private void Shield()
    {
        if (Input.GetKey(KeyCode.E) && (canShield == true))
        {
            // Activate the Shield
            gameObject.transform.FindChild("Shield").gameObject.SetActive(true);
        }
        else
        {
            // Deactivate the Shield if not holding down E
            gameObject.transform.FindChild("Shield").gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Keeps the player from spamming the shoot button
    /// </summary>
    public void ShieldKilled()
    {
        // Player Gains Invincibility for 3 seconds
        canShield = false;
        Invoke("ResetShield", 20f);
    }

    /// <summary>
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetShield()
    {
        // De-activates melee collider
        gameObject.transform.FindChild("Shield").gameObject.SetActive(false);
        canShield = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion

    #region Poison Damage Helper Method
    /// <summary>
    /// Method to check all status conditions of the player
    /// </summary>
    void CheckStatusConditions()
    {
        // If the player is poisoned and has been damaged less than 4 times by it.
        if (poisoned == true && applypoison == true && poisonCounter<5)
        {
            // Apply Damage to player
            ApplyPoison();
        }
        else if(poisonCounter>5)
        {
            Debug.Log("Player is no longer poisoned");
            poisoned = false;
            applypoison = true;
            poisonCounter = 0;
        }
    }
    
    /// <summary>
    /// Apply Damage if poisoned
    /// </summary>
    void ApplyPoison()
    {
        Debug.Log("Poison Damages the player");
        currentLife--;
        poisonCounter++;
        applypoison = false;
        Invoke("ResetPoisonBool", 30f);
    }

    /// <summary>
    /// Resets when the player gets hit with more poison damage
    /// </summary>
    void ResetPoisonBool()
    {
        // Reactivates apply poison
        applypoison = true;
    }
    #endregion
    #endregion

    #region Save and Load Methods
    /*
    // This will work for everything but web
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MansionMayhem.dat");

        PlayerData data = new PlayerData();

        // Puts the Variables that need to be saved into the data Class
        data.screws = screws;


        // Serialize the data
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        // Check to see if a save file already exists
        if(File.Exists(Application.persistentDataPath + "/MansionMayhem.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MansionMayhem.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // Set variables based on the save file
            screws = data.screws;
        }
        else
        {
            // Variables that are not saved are set to original value otherwise
            screws = 0;

        }
    }
    */
    #endregion
}

#region Data Container for saving
/*
/// <summary>
/// Class for Saving
/// Just a class that is a "DATA Container" that allows writing the data to a save file
/// </summary>
[Serializable]
class PlayerData
{
    // All saved data here
    public int screws;

}
*/
#endregion