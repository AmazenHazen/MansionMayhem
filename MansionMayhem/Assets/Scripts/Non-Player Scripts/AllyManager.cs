using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The AllyManager class inherits from NPC so that you can talk to Allies or interact with them still
/// </summary>
public class AllyManager : NPC
{
    #region Ally Attributes
    // Attributes (very similar to enemy Manager)
    // Attributes Set by editor
    const float INITIAL_SHOOT_DELAY = 1f;

    // Ally Behavior variables - Set in the Editor
    public bool hasBullets;                    // Determines if the ally has bullets
    public bool hasAbility;                    // Determines if the ally has an ability (shoot a web/leave slime behind it)
    public bool vampyric;                      // Determines if the ally heals when hitting the player
    public float maxHealth;                    // The amount of health the ally spawns with
    public float rangeDamage;                   // Damage caused when the player is hit by the monster (collision)
    public float timeBetweenShots;              // time between bullets
    public List<GameObject> allyBulletPrefabs; // Prefabs of Bullets shot
    public List<GameObject> allyAbilityPrefabs;// Prefab of Ability being used (Webs, Slime, Etc.)
    public List<GameObject> allyWeapons;       // Additional Weapons of the ally
    public List<abilityType> abilityTypes;      // Determines the ability of the enemy
    public List<float> timeBetweenAbilities;    // Determines how long to wait between abilities
    public List<int> abilityRestrictionNumber;  // The number ascociated with the ability to determine how many ability object there can be in the scene
    public List<GameObject> allyObjects;        // Holds additional objects that work with the ally.
    public float seekDistance;                 // The distance at which an ally can sense enemies - takes this from awareDistance from AllyMovement Class

    // Basic Monster Attributes
    protected float currentLife;                  // The current health the ally has

    // Attack Variables - all set in the Prefab Instance
    protected bool invincibility;                 // Gives Enemy brief invincibility when hit by a melee attack
    protected bool hitByMeleeBool;                 // Determines if the enemy has been hit by a melee attack by the player

    // Ability Management
    protected List<bool> canUseAbility;
    protected List<int> abilityCount;       // Works has a count of the number of abilities are out for a specific ability (goes with the enemy ability prefab)
    [HideInInspector] public List<GameObject> allyBullets;           // a list keeping track of all of the current bullets on the screen
    [HideInInspector] public List<GameObject> allyAbilityObjects;    // a list keeping track of all of the abilities out for a specific ability
    [HideInInspector] public List<GameObject> allyBlobs;

    // Bullet Management
    protected bool canShoot;
    private float totalTime;
    private bool initialBulletDelay;
    #endregion

    #region Ally Properties
    public float CurrentLife
    {
        get { return currentLife; }
        set { currentLife = value; }
    }
    public bool HitByMeleeBool
    {
        get { return hitByMeleeBool; }
    }
    #endregion

    #region Initialization for Ally
    // Use this for initialization
    protected override void Start ()
    {

        // Sets up the enemy's awareness distance in this class
        seekDistance = GetComponent<AllyMovement>().awareDistance;

        // Sets up whether and enemy can shoot bullets or not
        canShoot = true; // Set to true if player gets within distance of the enemy

        // Adds a list of for tracking active abilities in the level and if you can use it
        abilityCount = new List<int>();
        canUseAbility = new List<bool>();
        for (int i = 0; i < allyAbilityPrefabs.Count; i++)
        {
            canUseAbility.Add(true);
            abilityCount.Add(0);
        }

        hitByMeleeBool = false; // Set true so all can get hit by melee
        allyBullets = new List<GameObject>();
        allyAbilityObjects = new List<GameObject>();
        allyBlobs = new List<GameObject>();

        // Sets up Ally's HealthBar
        currentLife = maxHealth;
        GetComponent<HealthBar>().HealthBarInstantiate();


        // set initial bullet delay to true
        initialBulletDelay = true;

        // Call the NPC Manager start
        base.Start();
    }
    #endregion

    #region Update Section
    // Update is called once per frame
    protected override void Update()
    {
        // Check for death first
        Death();

        if ((player.transform.position - transform.position).magnitude < seekDistance)
        {
            // update deltaTime next
            totalTime += Time.deltaTime;
            if (totalTime > INITIAL_SHOOT_DELAY)
            {
                initialBulletDelay = false;
            }
        }

        // Enemy Shooting allows any enemy to shoot when possible
        if (hasBullets == true && canShoot == true && initialBulletDelay == false && (player.transform.position - transform.position).magnitude < seekDistance)
        {
            Shoot();
        }

        // Enemy Abilities - allows any enemy to use ability when possible
        for (int i = 0; i < allyAbilityPrefabs.Count; i++)
        {
            if (hasAbility == true)
            {
                if (canUseAbility[i] == true && ((player.transform.position - transform.position).magnitude < seekDistance))
                {
                    // Use the ability
                    Ability(abilityTypes[i], i);

                    // Reset abilities if used
                    JustAbilitied(i);
                }

                // Call ability Managment for enemies with abilities
                abilityManagement(i);
            }
        }

        base.Update();
    }
    #endregion

    #region Enemy Helper Methods

    #region Death
    /// <summary>
    /// Checks if the Enemy should be dead
    /// </summary>
    void Death()
    {
        // if currentHealth<=0
        if (currentLife <= 0)
        { 
            // Tell the abilities that the owner is dead
            foreach (GameObject ability in allyAbilityObjects)
            {
                if (ability.GetComponent<BlobScript>() != null)
                {
                    ability.GetComponent<BlobScript>().ownerAlive = false;
                }

                // Kil enemy minions if the owner dies.
                if (ability.GetComponent<AllyManager>() != null)
                {
                    ability.GetComponent<AllyManager>().Death();
                }
            }


            // Return the Ally to their start position or end the level?
        }
    }
    #endregion

    #region Shoot Methods
    /// <summary>
    /// Shooting Method
    /// </summary>
    void Shoot()
    {
        GameObject bulletCopy;
        bulletCopy = Instantiate(allyBulletPrefabs[0], transform.position, transform.rotation) as GameObject;
        bulletCopy.GetComponent<BulletManager>().BulletStart(gameObject);
        allyBullets.Add(bulletCopy);

        JustShot();
    }

    /// <summary>
    /// Keeps the player from spamming the shoot button
    /// </summary>
    void JustShot()
    {
        // Player Gains Invincibility for 3 seconds
        canShoot = false;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        Invoke("ResetShooting", timeBetweenShots);

    }

    /// <summary>
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetShooting()
    {
        canShoot = true;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion

    #region Ability Methods
    #region Abilities for Allies
    /// <summary>
    /// Shooting Method
    /// </summary>
    protected void Ability(abilityType ability, int abilityIndex)
    {
        GameObject abilityObject;

        switch (ability)
        {
            case abilityType.blobs:
                // Instantiate the blob
                //Debug.Log("Dropping a doozy");
                abilityObject = Instantiate(allyAbilityPrefabs[abilityIndex], transform.position, transform.rotation);

                // Add it to a list of blobs/Abilities
                allyAbilityObjects.Add(abilityObject);
                // Increase the number of ability objects
                abilityCount[abilityIndex]++;


                // Call the special initialization
                abilityObject.GetComponent<BlobScript>().BlobStart(gameObject);
                break;

            case abilityType.babies:
                //Debug.Log("Giving Birth!");
                if (!(abilityRestrictionNumber[abilityIndex] == abilityCount[abilityIndex]))
                {

                    abilityObject = Instantiate(allyAbilityPrefabs[abilityIndex], transform.position, transform.rotation);
                    abilityObject.GetComponent<EnemyManager>().Parent = gameObject;
                    abilityObject.GetComponent<EnemyManager>().ParentAbilityNum = abilityIndex;

                    // Add it to a list of babies
                    allyAbilityObjects.Add(abilityObject);

                    // Increase the number of ability objects
                    abilityCount[abilityIndex]++;
                }
                break;
        }
    }
    #endregion

    #region Ability Managment
    /// <summary>
    /// Keeps the player from spamming abilities
    /// </summary>
    /// <param name="abilityIndex"></param>

    protected void JustAbilitied(int abilityIndex)
    {
        // Set the ability to use the ability to false
        canUseAbility[abilityIndex] = false;

        // Run the reset ability method
        StartCoroutine(ResetAbility(abilityIndex, timeBetweenAbilities[abilityIndex]));
    }

    /// <summary>
    /// Resets Player's ability Bool
    /// </summary>
    IEnumerator ResetAbility(int abilityIndex, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        canUseAbility[abilityIndex] = true;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    protected void abilityManagement(int abilityIndex)
    {
        // Managing the number of ability objects on screen
        for (int i = 0; i < allyAbilityPrefabs.Count; i++)
        {
            if (abilityCount[abilityIndex] > abilityRestrictionNumber[abilityIndex])
            {
                if (abilityTypes[abilityIndex] == abilityType.blobs)
                {
                    //Debug.Log("In ability Management");
                    GameObject playerAbilityCopy = allyAbilityObjects[0];

                    allyAbilityObjects.Remove(playerAbilityCopy);
                    Destroy(playerAbilityCopy);
                    abilityCount[abilityIndex]--;
                }
            }
        }
    }
    #endregion

    #region Vampyric Healing Method
    /// <summary>
    /// Heals the enemy if it hits the use with 
    /// </summary>
    public void VampyricHeal(float damageFloat)
    {
        currentLife += damageFloat;
    }
    #endregion
    #endregion

    #region HurtMethod
    public void HurtByMelee()
    {
        hitByMeleeBool = true;
        Invoke("ResetHurtByMelee", .2f);
    }

    void ResetHurtByMelee()
    {
        hitByMeleeBool = false;
    }
    #endregion

    #endregion
}
