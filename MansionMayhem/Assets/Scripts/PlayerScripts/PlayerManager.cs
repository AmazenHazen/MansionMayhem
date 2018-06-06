using System.Collections;
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
    public bool canTravel;
    //private bool canMelee;
    private bool canShield;

    // Status Conditions
    int poisonCounter;
    bool applypoison;
    bool poisoned;

    // Weapon Variables
    private trinkets currentTrinket;
    public List<GameObject> playerGunPrefabs;
    private rangeWeapon currentRangeWeapon;
    public int weaponNum;
    public int portalNum;

    // Variables for the player's inventory
    public ItemType[] playerItems;

    //private int itemCount;


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
    public int PortalNum
    {
        get { return portalNum; }
        set { portalNum = value; }
    }

    #endregion

    #region Setting Initial Player Attributes
    // Use this for initialization
    void Start()
    {
        // Player Game Variables
        maxLife = GameManager.instance.healthTotal;
        currentLife = GameManager.instance.healthTotal;
        shieldLife = 1;
        invincibility = false;
        //canMelee = true;
        canShield = true;
        canTravel = true;
        applypoison = true; // Starts as true, turned to false only if poisoned
        poisoned = false;
        poisonCounter = 0;
        portalNum = 0;
        weaponNum = 0;


        // Inventory Set Up
        playerItems = new ItemType[6];
        for(int i = 0; i<playerItems.Length; i++)
        {
            playerItems[i] = ItemType.NoItem;
        }

        #region Set up Guns for the player for the level
        // Gun Set Up

        for (int i=0; i<GameManager.instance.currentGuns.Count; i++)
        {
            switch (GameManager.instance.currentGuns[i])
            {
                case rangeWeapon.aetherLightBow:
                    playerGunPrefabs.Add(GameObject.Find("AetherLightBow"));
                    break;
                case rangeWeapon.antiEctoPlasmator:
                    playerGunPrefabs.Add(GameObject.Find("AntiEctoPlasmator"));
                    break;
                case rangeWeapon.CelestialRepeater:
                    playerGunPrefabs.Add(GameObject.Find("CelestialRepeater"));
                    break;
                case rangeWeapon.cryoGun:
                    playerGunPrefabs.Add(GameObject.Find("CryoGun"));
                    break;
                case rangeWeapon.DarkEnergyRifle:
                    playerGunPrefabs.Add(GameObject.Find("DarkMatterRifle"));
                    break;
                case rangeWeapon.ElectronSeeker:
                    playerGunPrefabs.Add(GameObject.Find("ElectronSeeker"));
                    break;
                case rangeWeapon.flamethrower:
                    playerGunPrefabs.Add(GameObject.Find("Flamethrower"));
                    break;
                case rangeWeapon.hellfireshotgun:
                    playerGunPrefabs.Add(GameObject.Find("HellFireShotGun"));
                    break;
                case rangeWeapon.laserpistol:
                    playerGunPrefabs.Add(GameObject.Find("LaserGun"));
                    break;
                case rangeWeapon.PlasmaCannon:
                    playerGunPrefabs.Add(GameObject.Find("PlasmaPistol"));
                    break;
                case rangeWeapon.soundCannon:
                    playerGunPrefabs.Add(GameObject.Find("SoundCannon"));
                    break;
                case rangeWeapon.XenonPulser:
                    playerGunPrefabs.Add(GameObject.Find("XenonPulser"));
                    break;
                case rangeWeapon.AntimatterParticle:
                    playerGunPrefabs.Add(GameObject.Find("AntiMatterParticle"));
                    break;
                case rangeWeapon.Tempest:
                    playerGunPrefabs.Add(GameObject.Find("Tempest"));
                    break;
                case rangeWeapon.PreciousRevolver:
                    playerGunPrefabs.Add(GameObject.Find("PreciousMetalRevolver"));
                    break;
                default:
                    playerGunPrefabs.Add(null);
                    break;
            }
        }
        #endregion

        // If the gun doesn't exist increment and check if it's null
        while (playerGunPrefabs[weaponNum] == null)
        {
            weaponNum++;
            weaponNum %= 3;
        }
        currentRangeWeapon = playerGunPrefabs[weaponNum].GetComponent<GunScript>().gunType;

        // turn on the current gun sprite
        playerGunPrefabs[weaponNum].GetComponent<SpriteRenderer>().enabled = true;

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

        if (GameManager.instance.currentGameState != GameState.Paused)
        {
            Shoot();
        }
        //Melee();
        //Shield();
        if(currentLife<=0)
        {
            Death();
        }
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

                // Check to see if the player can travel first
                if (canTravel == true && collider.GetComponent<DoorScript>().requirements.Count==0 && collider.GetComponent<DoorScript>().monsterRequirements.Count == 0 && !GUIManager.bossFight)
                {
                    //Debug.Log("Travel");
                    collider.GetComponent<DoorScript>().Travel(gameObject);
                    // Activate just traveled method
                    JustTraveled();
                }

                // If not then check to see if they are using interaction to unlock the door
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Unlock the door
                    UseItem(collider.gameObject);

                    // Message the player of the requirements stil left of the door
                    if (collider.GetComponent<DoorScript>().requirements.Count > 0)
                    {
                        collider.GetComponent<DoorScript>().MessageRequirementsItem();
                    }
                    // Message the player of the requirements stil left of the door
                    else if (collider.GetComponent<DoorScript>().monsterRequirements.Count > 0)
                    {
                        collider.GetComponent<DoorScript>().MessageRequirementsMonster();
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
                    currentLife -= collider.GetComponent<EnemyManager>().damage;

                    // Heal the enemy if hit and a vampyric enemy
                    if (collider.GetComponent<EnemyManager>().vampyric == true)
                    {
                        // Call the vampyric heal method
                        collider.GetComponent<EnemyManager>().VampyricHeal(collider.GetComponent<EnemyManager>().damage);
                    }


                    // Poison the player if the enemy is poisonous
                    // The Enemy Poisons the player with the melee attack if poisonous
                    if (collider.GetComponent<EnemyManager>().isPoisonous == true)
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
                    currentLife -= collider.GetComponent<EnemyManager>().damage;

                    // Poison the player if the enemy is poisonous
                    // The Enemy Poisons the player with the melee attack if poisonous
                    if (collider.GetComponent<EnemyManager>().isPoisonous == true)
                    {
                        // Set poison to true and reset the counter
                        StartPoison();
                    }

                    StartInvincibility();
                }
                break;
            case "enemyweapon":
                if (invincibility == false)
                {

                    //Debug.Log("Enemy: " + collider.gameObject.GetComponent<EnemyManager>().monster);
                    currentLife -= collider.GetComponent<EnemyWeaponScript>().damage;

                    // Heal the enemy if hit and a vampyric enemy
                    if (collider.GetComponent<EnemyWeaponScript>().vampyric == true)
                    {
                        // Call the vampyric heal method
                        collider.GetComponent<EnemyWeaponScript>().owner.GetComponent<EnemyManager>().VampyricHeal(collider.GetComponent<EnemyWeaponScript>().damage);
                    }


                    // Poison the player if the enemy is poisonous
                    // The Enemy Poisons the player with the melee attack if poisonous
                    if (collider.GetComponent<EnemyWeaponScript>().isPoisonous == true)
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
                ItemType itemVarCopy = collider.GetComponent<ItemScript>().itemVar;
                bool pickedUp = false;
                GameObject itemCopy = collider.gameObject;

                switch (itemVarCopy)
                {
                    #region Health Pickups
                        // Heart
                    case ItemType.HeartPickup:
                        currentLife++;
                        if (currentLife > maxLife)
                        {
                            currentLife = maxLife;
                        }
                        pickedUp = true;
                        break;

                        // Health Potion
                    case ItemType.HealthPotionPickup:
                        currentLife += 3;
                        if(currentLife>maxLife)
                        {
                            currentLife = maxLife;
                        }
                        pickedUp = true;
                        break;

                        // Health Kit
                    case ItemType.HealthKit:
                        currentLife+= 5;
                        if (currentLife > maxLife)
                        {
                            currentLife = maxLife;
                        }
                        pickedUp = true;
                        break;

                    // Golden Heart
                    case ItemType.GoldenHeart:
                        currentLife += 10;
                        if (currentLife > maxLife)
                        {
                            currentLife = maxLife;
                        }
                        pickedUp = true;
                        break;
                    #endregion

                    #region Screw Pickups
                    case ItemType.NormalScrewPickup:
                        GameManager.instance.screws++;
                        pickedUp = true;
                        break;
                    case ItemType.RedScrewPickup:
                        GameManager.instance.screws += 5;
                        pickedUp = true;
                        break;
                    case ItemType.GoldenScrewPickup:
                        GameManager.instance.screws += 10;
                        pickedUp = true;
                        break;
                    case ItemType.ToolBox:
                        GameManager.instance.screws += 50;
                        pickedUp = true;
                        break;
                    case ItemType.Experience:
                        GameManager.instance.experience += 1;
                        pickedUp = true;
                        break;
                    case ItemType.Blueprint:
                        GameManager.instance.blueprints += 1;
                        pickedUp = true;
                        break;

                    #endregion

                    #region Level Pickups
                    // Keys and Quest Items go through the default item handler and are added to the player's inventory
                    default:
                        // Add the object to the Inventory
                        pickedUp = AddItem(collider.gameObject);
                        //Debug.Log("Made it past adding the item to GUI");
                        break;
                        #endregion
                }
                // At the end of an item collision destroy the gameobject
                if (pickedUp == true)
                {
                    Destroy(collider.gameObject);
                }

                break;
            #endregion

            #region NPC
            case "npc":
            case "interactableobject":
            case "ally":
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    collider.GetComponent<NPC>().TalkingBool = true;

                    //Debug.Log("Talking to " + collider.gameObject.GetComponent<NPC>().name);
                }
                break;
            #endregion

            #region chest
            case "chest":
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    collider.GetComponent<ChestScript>().OpenChest();
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
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Starts player's invincibility frames
    /// </summary>
    void StartInvincibility()
    {
        // Player Gains Invincibility for 3 seconds
        invincibility = true;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        Invoke("ResetInvincibility", 3);
    }
    #endregion

    #region Death Helper Method
    /// <summary>
    /// Calls death if the player dies
    /// </summary>
    private void Death()
    {
        GameManager.instance.currentGameState = GameState.Died;       
    }
    #endregion

    #region Bool Helper Methods
    /// <summary>
    /// Resets the player to not having invincibility
    /// </summary>
    public void ResetTravelBool()
    {
        canTravel = true;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Starts player's invincibility frames
    /// </summary>
    public void JustTraveled()
    {
        // Player Gains Invincibility for 3 seconds
        canTravel = false;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.red;
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
        if (playerGunPrefabs[weaponNum] != null)
        {
            playerGunPrefabs[weaponNum].GetComponent<GunScript>().PlayerFireWeapon();
        }
    }

    /// <summary>
    /// Switch Range Weapons Helper Method
    /// </summary>
    private void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GunScript currentGunScript = playerGunPrefabs[weaponNum].GetComponent<GunScript>();

            // For particle guns turn off the particles if you switch weapons
            if (currentGunScript.Particles)
            {
                currentGunScript.Particles.SetActive(false);
            }
            // For charging gun shooting
            if (currentGunScript.Charging)
            {
                currentGunScript.Charging = false;

                // Shoot the Charging bullet if you switch guns while charging the bullet (shoot as though you call Fire Weapon)
                currentGunScript.bulletCopy.GetComponent<BulletManager>().Speed = currentGunScript.bulletSpeed;
                currentGunScript.JustShot();
            }

            // turn off the current gun sprite
            playerGunPrefabs[weaponNum].GetComponent<SpriteRenderer>().enabled = false;

            // If the gun doesn't exist increment and check if it's null
            do
            {
                weaponNum++;
                weaponNum %= 3;
            } while (playerGunPrefabs[weaponNum] == null);


            // turn on the current gun sprite
            playerGunPrefabs[weaponNum].GetComponent<SpriteRenderer>().enabled = true;
            currentRangeWeapon = playerGunPrefabs[weaponNum].GetComponent<GunScript>().gunType;

            // Have a delay so the player can't bypass the normal shooring mechanic delay by switching weapons
            playerGunPrefabs[weaponNum].GetComponent<GunScript>().JustSwitchGuns();
        }
    }
    #endregion

    #region Melee Helper Methods
    /*
    /// <summary>
    /// Method that lets the player melee
    /// </summary>
    private void Melee()
    {
        if (Input.GetMouseButton(1) && canMelee == true)
        {
            // Activate the Melee Collider
            gameObject.transform.Find("MeleeAttack").gameObject.SetActive(true);

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
        gameObject.transform.Find("MeleeAttack").gameObject.SetActive(false);
        canMelee = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    */
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
            gameObject.transform.Find("Shield").gameObject.SetActive(true);
        }
        else
        {
            // Deactivate the Shield if not holding down E
            gameObject.transform.Find("Shield").gameObject.SetActive(false);
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
        gameObject.transform.Find("Shield").gameObject.SetActive(false);
        canShield = true;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
            //Debug.Log("Player is no longer poisoned");
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
        //Debug.Log("Poison Damages the player");
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

    #region Inventory Helper Methods
    public void UseItem(GameObject interactingObject)
    {
        #region Door
        // If the object is a door
        if (interactingObject.GetComponent<DoorScript>())
        {
            // Unlock the door
            for (int i = 0; i < playerItems.Length; i++)
            {
                foreach (ItemType requirement in interactingObject.GetComponent<DoorScript>().requirements)
                {
                    if (playerItems[i] == requirement)
                    {
                        // Debug Statment
                        //Debug.Log("Using Item: " + playerItems[i]);

                        // Remove the requirement throught the door helper method
                        interactingObject.GetComponent<DoorScript>().removeRequirement(playerItems[i]);

                        // Remove inventory graphic
                        GameObject.Find("HUDCanvas").GetComponent<GUIManager>().RemoveItemGUI(i);

                        // Remove the player's item
                        playerItems[i] = ItemType.NoItem;

                        // Break out of the loop
                        break;
                    }
                }
            }
        }
        #endregion

    }

    public bool AddItem(GameObject item)
    {
        //Debug.Log("Length" + playerItems.Length);


        // Loop through the inventory
        for (int i = 0; i < playerItems.Length; i++)
        {
            // If there is an empty spot add the item
            if (playerItems[i] == ItemType.NoItem)
            {
                // Set the inventory space to that item
                playerItems[i] = item.GetComponent<ItemScript>().itemVar;

                // Add graphic to inventory
                GameObject.Find("HUDCanvas").GetComponent<GUIManager>().AddItemGUI(item);

                //Debug.Log("Added" + i);
                //Debug.Log("Added Item to Inventory");
                // Return true
                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(GameObject item)
    {
        // Loop through the inventory
        for (int i = 0; i < playerItems.Length; i++)
        {
            // If you find what you are removing than remove it
            if (playerItems[i] == item.GetComponent<ItemScript>().itemVar)
            {
                // Set the inventory space to NoItem
                playerItems[i] = ItemType.NoItem;
                // Return true
                return true;
            }
        }
        return false;
    }
    #endregion
}