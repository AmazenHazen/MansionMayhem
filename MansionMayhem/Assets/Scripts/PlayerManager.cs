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
    private float timeBetweenShots = .5f;
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
    private int maxBullets;
    private int maxBlobs;
    public int portalNum;
    GameObject FrostGun;
    GameObject Flamethrower;
    Coroutine sound;

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
        maxBullets = 4;
        maxBlobs = 3;
        portalNum = 0;

        // Initializing Lists
        playerBullets = new List<GameObject>();

        playerItems = new ItemType[6];
        for(int i = 0; i<playerItems.Length; i++)
        {
            playerItems[i] = ItemType.NoItem;
        }
        
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
        if (GameManager.currentGameState != GameState.Paused)
        {
            Shoot();
        }
        BlobManagement();
        BulletManagement();
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

                // Using interaction to unlock the door
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    // Unlock the door
                    UseItem(collider.gameObject);
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
                ItemType itemVarCopy = collider.gameObject.GetComponent<ItemScript>().itemVar;
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
                        GameManager.screws++;
                        pickedUp = true;
                        break;
                    case ItemType.RedScrewPickup:
                        GameManager.screws += 5;
                        pickedUp = true;
                        break;
                    case ItemType.GoldenScrewPickup:
                        GameManager.screws += 10;
                        pickedUp = true;
                        break;

                    #endregion

                    #region Level Pickups
                    // Keys and Quest Items go through the default item handler and are added to the player's inventory
                    default:
                        // Add the object to the Inventory
                        pickedUp = AddItem(collider.gameObject.GetComponent<ItemScript>().itemVar);

                        // Add the item graphic to the GUI
                        GameObject.Find("HUDCanvas").GetComponent<GUIManager>().AddItemGUI(itemCopy);
                        //Debug.Log("Made it past adding the item to GUI");
                        //collider.gameObject.GetComponent<ItemScript>().objectOwner.GetComponent<ArtifactScript>().requirements.Remove(collider.gameObject);
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

            #region artifact
            case "InteractableObject":
                if (Input.GetKeyDown(KeyCode.Space))
                {

                    // Debug Line
                    Debug.Log("Using Interactable Object:" + collider.gameObject);

                    collider.gameObject.GetComponent<ArtifactScript>().Activate();
                }

                break;
            #endregion

            #region interactableObject
            case "interactableobject":
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    // Pause the gameplay
                    // Set pauseGame to true
                    GameManager.currentGameState = GameState.Paused;
                    GUIManager.usingOtherInterface = true;
                    Time.timeScale = 0;

                    // Debug Line
                    Debug.Log("Using Interactable Object:" + collider.gameObject);

                    UseItem(collider.gameObject);

                    collider.gameObject.GetComponent<InteractableObjectScript>().InteractBool = true;
                }

                break;
            #endregion

            #region NPC
            case "npc":
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    // Pause the gameplay
                    // Set pauseGame to true
                    GameManager.currentGameState = GameState.Paused;
                    GUIManager.usingOtherInterface = true;
                    Time.timeScale = 0;

                    collider.gameObject.GetComponent<NPC>().TalkingBool = true;

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
                    GameManager.currentGameState = GameState.Paused;
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
    public void ResetTravelBool()
    {
        canTravel = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Starts player's invincibility frames
    /// </summary>
    public void JustTraveled()
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
                case rangeWeapon.ElectronSeeker:
                    ShootBullet(5);
                    break;
                case rangeWeapon.PortalGun:
                    ShootBullet(6);               
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

        // Call Special start method for bullets
        bulletCopy.GetComponent<BulletManager>().BulletStart(gameObject);

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
        Invoke("ResetShooting", timeBetweenShots);
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
            // Increment the current range weapon
            if (currentRangeWeapon != rangeWeapon.PortalGun)
            {
                currentRangeWeapon++;
            }
            else
            {
                currentRangeWeapon = 0;
            }

            // Stop the sound Courotine
            //StopCoroutine(sound);

            switch(currentRangeWeapon)
            {
                case rangeWeapon.aetherLightBow:
                    timeBetweenShots = .5f;
                    maxBullets = 3;
                    break;
                case rangeWeapon.antiEctoPlasmator:
                    timeBetweenShots = .5f;
                    maxBullets = 3;
                    maxBlobs = 3;
                    break;
                case rangeWeapon.hellfireshotgun:
                    timeBetweenShots = .5f;
                    maxBullets = 15;
                    break;
                case rangeWeapon.laserpistol:
                    timeBetweenShots = .5f;
                    maxBullets = 4;
                    break;
                case rangeWeapon.soundCannon:
                    timeBetweenShots = .5f;
                    maxBullets = 12;
                    break;
                case rangeWeapon.ElectronSeeker:
                    timeBetweenShots = 1.0f;
                    maxBullets = 4;
                    break;
                case rangeWeapon.PortalGun:
                    timeBetweenShots = .75f;
                    maxBullets = 3;
                    maxBlobs = 2;
                    break;
            }
        }
    }

    void BulletManagement()
    {
        if (playerBullets.Count > maxBullets)
        {
            GameObject playerBulletCopy = playerBullets[0];
            bulletCount--;
            playerBullets.Remove(playerBulletCopy);
            Destroy(playerBulletCopy);
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
    void BlobManagement()
    {
        if(blobCount>maxBlobs)
        {
            GameObject playerBlobCopy = playerBlobs[0];
            blobCount--;
            playerBlobs.Remove(playerBlobCopy);
            Destroy(playerBlobCopy);
        }
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
                        Debug.Log("Using Item: " + playerItems[i]);

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

        #region InteractableObject
        // If the object is an interactable Object
        if (interactingObject.GetComponent<InteractableObjectScript>())
        {
            // Unlock the door
            for (int i = 0; i < playerItems.Length; i++)
            {
                foreach (ItemType requirement in interactingObject.GetComponent<InteractableObjectScript>().requirements)
                {
                    if (playerItems[i] == requirement)
                    {
                        // Debug Statment
                        Debug.Log("Using Item: " + playerItems[i]);

                        // Remove the requirement throught the door helper method
                        interactingObject.GetComponent<InteractableObjectScript>().removeRequirement(playerItems[i]);

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

    public bool AddItem(ItemType item)
    {
        Debug.Log("Length" + playerItems.Length);
        // Loop through the inventory
        for (int i = 0; i < playerItems.Length; i++)
        {
            // If there is an empty spot add the item
            if (playerItems[i] == ItemType.NoItem)
            {
                // Set the inventory space to that item
                playerItems[i] = item;
                //Debug.Log("Added" + i);
                //Debug.Log("Added Item to Inventory");
                // Return true
                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(ItemType item)
    {
        // Loop through the inventory
        for (int i = 0; i < playerItems.Length; i++)
        {
            // If you find what you are removing than remove it
            if (playerItems[i] == item)
            {
                // Set the inventory space to null
                playerItems[i] = ItemType.NoItem;
                // Return true
                return true;
            }
        }
        return false;
    }
    #endregion
}