﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool canShoot;
    private bool canBurst;
    private bool canMelee;
    private bool canShield;

    // Status Conditions
    int poisonCounter;
    bool applypoison;
    bool poisoned;

    // Weapon Variables
    private rangeWeapon currentRangeWeapon;
    private trinkets currentTrinket;
    public List<GameObject> playerBullets;
    public List<GameObject> playerBlobs;
    public List<GameObject> playerBulletPrefabs;
    private int bulletCount;
    private int blobCount;
    GameObject FrostGun;
    GameObject Flamethrower;
    bool frostGunAvailable;
    
    Coroutine sound;

    // Variables for the level
    public List<GameObject> playerItems;
    private int itemCount;


    // Unlockable abilities
    public bool magnet;
    public float magnetDistance;
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
    public int BlobCount
    {
        get { return blobCount; }
        set { blobCount = value; }
    }
    public bool Poisoned
    {
        get { return poisoned; }
        set { poisoned = value; }
    }
    public int PoisonCounter
    {
        set { poisonCounter = value; }
    }
    public rangeWeapon CurrentRangeWeapon
    {
        get { return currentRangeWeapon; }
    }

    #endregion

    #region Setting Initial Player Attributes
    // Use this for initialization
    void Start()
    {
        // Player Game Variables
        maxLife = 20;
        currentLife = 20f;
        shieldLife = 1;
        invincibility = false;
        canShoot = true;
        canBurst = true;
        canMelee = true;
        canShield = true;
        canTravel = true;
        applypoison = true; // Starts as true, turned to false only if poisoned
        poisoned = false;
        poisonCounter = 0;
        currentRangeWeapon = rangeWeapon.antiEctoPlasmator;
        FrostGun = transform.FindChild("FrostGun").gameObject;
        Flamethrower = transform.FindChild("Flamethrower").gameObject;

        // Initializing Lists
        playerBullets = new List<GameObject>();

        // Temp variables
        magnet = true;
        magnetDistance = 1.5f;
    }
    #endregion

    #region Update for Player
    // Update is called once per frame
    void Update()
    {
        CheckStatusConditions();
        WeaponSwitch();
        if (GUIManager.pausedGame == false)
        {
            Shoot();
        }
        blobManagement();
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
    public void playerCollisionMethod(Collider2D collider)
    {
        // Get the tag associated with the collision
        switch (collider.tag)
        {
            #region Door

            // Obstacles
            case "door":
                //Debug.Log("Door");
                

                // Travel first
                if (canTravel == true)
                {
                    Debug.Log("Travel");
                    collider.gameObject.GetComponent<DoorScript>().Travel(gameObject);
                    // Activate just traveled method
                    JustTraveled();
                }

                // Move the player to the new room
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    // Unlock the door
                    foreach(GameObject item in playerItems)
                    {
                        foreach(GameObject requirement in collider.gameObject.GetComponent<DoorScript>().requirements)
                        {
                            if(item == requirement)
                            {
                                // Remove the player's item
                                playerItems.Remove(item);

                                // Remove the requirement throught the door helper method
                                collider.gameObject.GetComponent<DoorScript>().removeRequirement(item);

                                // Debug Statment
                                Debug.Log("Using Item: " + item);                                                                                           
                            }
                        }
                    }
                }     
                break;

            #endregion

            #region Enemy Collision
            // Enemies
            case "enemy":
                if (invincibility == false)
                {

                    //Debug.Log("Enemy: " + collider.gameObject.GetComponent<EnemyManager>().monster);
                    currentLife -= collider.gameObject.GetComponent<EnemyManager>().damage;

                    // Heal the enemy if hit and a vampyric enemy
                    if (collider.gameObject.GetComponent<EnemyManager>().vampyric == true)
                    {
                        // Call the vampyric heal method
                        collider.gameObject.GetComponent<EnemyManager>().VampyricHeal();
                    }


                    // Poison the player if the enemy is poisonous
                    // The Enemy Poisons the player with the melee attack if poisonous
                    if (collider.gameObject.GetComponent<EnemyManager>().isPoisonous == true)
                    {
                        // Set poison to true and reset the counter
                        StartPoison();
                    }

                    StartInvincibility();
                }
                break;
            case "boss":
                if (invincibility == false)
                {

                    //Debug.Log("Enemy: " + collider.gameObject.GetComponent<EnemyManager>().monster);
                    currentLife -= collider.gameObject.GetComponent<EnemyManager>().damage;

                    // Poison the player if the enemy is poisonous
                    // The Enemy Poisons the player with the melee attack if poisonous
                    if (collider.gameObject.GetComponent<EnemyManager>().isPoisonous == true)
                    {
                        // Set poison to true and reset the counter
                        StartPoison();
                    }

                    StartInvincibility();
                }
                break;
            #endregion

            #region Items
            // Items
            case "item":
                // Debug Line
                //Debug.Log("Item: " + collider.gameObject.GetComponent<ItemScript>().itemVar);


                // Make a copy of the type of item for determining what to do with it.
                itemType itemVarCopy = collider.gameObject.GetComponent<ItemScript>().itemVar;
                switch (itemVarCopy)
                {
                    #region Health Pickups
                        // Heart
                    case itemType.heartPickup:
                        currentLife++;
                        if (currentLife > maxLife)
                        {
                            currentLife = maxLife;
                        }
                        break;

                        // Health Potion
                    case itemType.healthPotionPickup:
                        currentLife += 3;
                        if(currentLife>maxLife)
                        {
                            currentLife = maxLife;
                        }
                        break;

                        // Health Kit
                    case itemType.healthKit:
                        currentLife+= 5;
                        if (currentLife > maxLife)
                        {
                            currentLife = maxLife;
                        }
                        break;

                    // Golden Heart
                    case itemType.goldenHeart:
                        currentLife += 10;
                        if (currentLife > maxLife)
                        {
                            currentLife = maxLife;
                        }
                        break;
                    #endregion

                    #region Screw Pickups

                    case itemType.normalScrewPickup:
                        GameManager.screws++;
                        break;
                    case itemType.redScrewPickup:
                        GameManager.screws += 5;
                        break;
                    case itemType.goldenScrewPickup:
                        GameManager.screws += 10;
                        break;

                    #endregion

                    #region Level Pickups
                    case itemType.quest:
                        // Add the object to the List
                        playerItems.Add(collider.gameObject);
                        collider.gameObject.GetComponent<ItemScript>().objectOwner.GetComponent<ArtifactScript>().requirements.Remove(collider.gameObject);
                        break;
                    case itemType.key:
                        // Add the key to the List
                        playerItems.Add(collider.gameObject);
                        //collider.gameObject.GetComponent<ItemScript>().objectOwner.GetComponent<DoorScript>().requirements.Remove(collider.gameObject);
                        break;

                        #endregion
                }

                // At the end of an item collision destroy the gameobject
                Destroy(collider.gameObject);
                break;



            #endregion

            #region artifact
            case "artifact":
                if (Input.GetKeyDown(KeyCode.Space))
                {

                    // Debug Line
                    Debug.Log("Using Artifact:");

                    collider.gameObject.GetComponent<ArtifactScript>().Activate();
                }

                break;
                #endregion

            #region NPC
            case "npc":
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    // Pause the gameplay
                    // Set pauseGame to true
                    GUIManager.pausedGame = true;
                    GUIManager.usingOtherInterface = true;
                    Time.timeScale = 0;

                    collider.gameObject.GetComponent<NPC>().talkingBool = true;

                    Debug.Log("Talking to " + collider.gameObject.GetComponent<NPC>().name);
                }
                break;
            #endregion

            #region workbench
            case "workbench":
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    // Pause the gameplay
                    // Set pauseGame to true
                    GUIManager.pausedGame = true;
                    GUIManager.usingOtherInterface = true;
                    Time.timeScale = 0;

                    // Debug Line
                    Debug.Log("Using WorkBench:");
                }

                break;
            #endregion

            #region chest
            case "chest":
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    // Pause the gameplay
                    // Set pauseGame to true
                    /*
                    GUIManager.pausedGame = true;
                    GUIManager.usingOtherInterface = true;
                    Time.timeScale = 0;
                    */

                    collider.gameObject.GetComponent<ChestScript>().OpenChest();
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

    #region Bool Helper Methods
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
        Invoke("ResetTravelBool", 1.5f);
        
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
            switch (currentRangeWeapon)
            {
                case rangeWeapon.aetherLightBow:
                    ShootBullet(0);
                    break;
                case rangeWeapon.antiEctoPlasmator:
                    ShootBullet(1);
                    break;
                case rangeWeapon.laserpistol:
                    ShootBullet(2);
                    break;
                case rangeWeapon.hellfireshotgun:
                    for (int i = 0; i < 4; i++)
                    {
                        // Spread of the bullets
                        Quaternion pelletRotation = transform.rotation;
                        pelletRotation.x += Random.Range(-.05f, .05f);
                        pelletRotation.y += Random.Range(-.05f, .05f);

                        ShootBullet(3);
                    }
                    break;
                case rangeWeapon.soundCannon:
                    if (canBurst)
                    {
                        sound = StartCoroutine(SoundCannonShoot(4, .05f));
                        canBurst = false;
                    }
                    break;
                case rangeWeapon.cryoGun:
                    FrostGun.SetActive(true);
                    break;
                case rangeWeapon.flamethrower:
                    Flamethrower.SetActive(true);
                    break;
            }
        }
        else
        {
            Flamethrower.SetActive(false);
            FrostGun.SetActive(false);
        }
    }

    /// <summary>
    /// Launches a bullet
    /// </summary>
    void ShootBullet(int bulletPrefab)
    {
        Debug.Log("Firing Bullet" + bulletPrefab);
        Debug.Log(canShoot);
        GameObject bulletCopy;
        // Shoot the bullet
        bulletCopy = Instantiate(playerBulletPrefabs[bulletPrefab], transform.position, transform.rotation) as GameObject;
        playerBullets.Add(bulletCopy);

        // Special Start for shotgun
        if (currentRangeWeapon == rangeWeapon.hellfireshotgun)
        {
            bulletCopy.GetComponent<BulletManager>().BulletShotgunStart(gameObject);
        }
        else
        {
            bulletCopy.GetComponent<BulletManager>().BulletStart(gameObject);
        }
        bulletCount++;

        JustShot();

    }



    // Helper method for shooting bullets for sound Cannon
    IEnumerator SoundCannonShoot(int bulletPrefab, float delayTime)
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentRangeWeapon == rangeWeapon.soundCannon)
            {
                ShootBullet(4);
                yield return new WaitForSeconds(delayTime);
                //Debug.Log("Burst");
            }
        }
        Invoke("ResetBurst", .5f);
    }

    #region ResetShooting Methods
    /// <summary>
    /// Keeps the player from spamming the shoot button
    /// </summary>
    void JustShot()
    {
        // Player Can't shoot for .5 seconds
        //Debug.Log("We Just Shot!");
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
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetBurst()
    {
        canBurst = true;
    }
    #endregion


    /// <summary>
    /// Switch Range Weapons Helper Method
    /// </summary>
    private void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            // Stop the sound Courotine

            if (currentRangeWeapon == rangeWeapon.soundCannon)
            {
                //StopCoroutine(sound);
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
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) && (canShield == true))
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
    /// Starts the poison on the player/resets the variable
    /// </summary>
    public void StartPoison()
    {
        // Set poison to true and reset the counter
        poisonCounter = 0;
        poisoned = true;
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

    #region Blob Management
    void blobManagement()
    {
        if(blobCount>3)
        {
            GameObject playerBlobCopy = playerBlobs[0];
            blobCount--;
            playerBlobs.Remove(playerBlobCopy);
            Destroy(playerBlobCopy);
        }
    }
    #endregion

    #endregion
}