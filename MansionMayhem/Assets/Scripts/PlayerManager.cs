using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region PlayerAttributes
    // Player's Attributes
    private float life;
    private int coins;
    // Ammo for Ghost Gun
    private int antiEctoplasm;
    // Ammo for Demon killing weapons
    private int aetherLight;
    private bool invincibility;
    private bool canTravel;
    private bool canShoot;

    // Weapon Variables
    private rangeWeapon currentRangeWeapon;
    private meleeWeapon currentMeleeWeapon;
    private trinkets currentTrinket;
    public List<GameObject> playerBullets;
    public List<GameObject> playerBulletPrefabs;
    private int bulletCount;
    #endregion

    #region PlayerProperties
    public float Life
    {
        get { return life; }
        set { life = value; }
    }
    public int Coins
    {
        get { return coins; }
    }
    public int AntiEctoPlasm
    {
        get { return antiEctoplasm; }
    }
    public int AetherLight
    {
        get { return aetherLight; }
    }
    public int BulletCount
    {
        get { return bulletCount; }
        set { bulletCount = value; }
    }
    #endregion

    #region Setting Initial Player Attributes
    // Use this for initialization
    void Start()
    {
        // Player Game Variables
        life = 3;
        coins = 0;
        antiEctoplasm = 0;
        aetherLight = 0;
        invincibility = false;
        canShoot = true;
        canTravel = true;
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
        WeaponSwitch();
        Shoot();
    }
    #endregion

    #region CollisionDetection
    /// <summary>
    /// Player Collision Handled Here. This includes any objects that is effected by the player colliding with it.
    /// This includes: Coins, Health, Ammo, Enemies, Walls, and Furniture.
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
                    
                }

                // Activate just traveled method
                JustTraveled();
                break;

            #endregion

            #region Enemy Collision
            // Enemies
            case "enemy":
                if (invincibility == false)
                {
                    Debug.Log("Enemy: " + collider.gameObject.GetComponent<EnemyManager>().Monster);
                    life -= collider.gameObject.GetComponent<EnemyManager>().Damage;
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
                    #region Ammo/Health/Coin Pickups
                    case itemType.aetherLightAmmo:
                        aetherLight++;
                        break;
                    case itemType.antiEctoplasmAmmo:
                        antiEctoplasm++;
                        break;
                    case itemType.heartPickup:
                        life++;
                        break;
                    case itemType.healthPotionPickup:
                        life += 3;
                        break;
                    case itemType.coinPickup:
                        coins++;
                        break;
                    #endregion
                }

                // At the end of an item collision destroy the gameobject
                Destroy(collider.gameObject);
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
                case rangeWeapon.aetherLightBow:
                    bulletCopy = Instantiate(playerBulletPrefabs[0], transform.position, transform.rotation) as GameObject;
                    playerBullets.Add(bulletCopy);
                    bulletCopy.GetComponent<BulletManager>().bulletSetUp(gameObject);
                    bulletCount++;
                    break;
                case rangeWeapon.antiEctoPlasmator:
                    bulletCopy = Instantiate(playerBulletPrefabs[1], transform.position, transform.rotation) as GameObject;
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
    /// Switch Weapons Helper Method
    /// </summary>
    private void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.C))
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

    /// <summary>
    /// Keeps the player from spamming the shoot button
    /// </summary>
    void JustShot()
    {
        // Player Gains Invincibility for 3 seconds
        canShoot = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        Invoke("ResetShooting", .7f);

    }

    /// <summary>
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetShooting()
    {
        canShoot = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion
}
