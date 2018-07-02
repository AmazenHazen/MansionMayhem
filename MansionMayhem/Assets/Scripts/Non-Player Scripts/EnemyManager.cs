using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Enemy Attributes
    // Attributes Set by editor
    const float INITIAL_SHOOT_DELAY = 1f;

    // Gameobject reference to the player
    private GameObject player;
    private GameObject levelManager;

    // Enemy Behavior variables - All of these are set in the inspector
    public enemyType monster;                   // The specific monster creature name
    public enemyClass primarymonsterType;      // The primary type of monster (demon, ghost, spider)
    public enemyClass secondarymonsterType;    // The secondary type of monster
    public bool hasBullets;                    // Determines if the enemy has bullets
    public bool hasAbility;                    // Determines if the enemy has an ability (shoot a web/leave slime behind it)
    public bool isPoisonous;                   // Determines if the enemy can poison the player
    public bool vampyric;                      // Determines if the enemy heals when hitting the player
    public bool boss;                           // Rewards experience directly if defeated
    public float maxHealth;                    // The amount of health the enemy spawns with
    private int experience;                    // The amount of experience rewarded atthe death of an enemy
    public float damage;                       // Damage caused when the player is hit by the monster (collision)
    public List<GameObject> enemyAbilityPrefabs;// Prefab of Ability being used (Webs, Slime, Etc.)
    public List<GameObject> enemyWeapons;       // Additional Weapons of the enemy
    public List<abilityType> abilityTypes;      // Determines the ability of the enemy
    public List<float> timeBetweenAbilities;    // Determines how long to wait between abilities
    public List<int> abilityRestrictionNumber;  // The number ascociated with the ability to determine how many ability object there can be in the scene
    public List<GameObject> enemyObjects;       // Holds additional objects that work with the enemy.
    private float seekDistance;                 // The distance at which an enemy can sense where you are - takes this from awareDistance from EnemyMovement Class
    

    // Basic Monster Attributes
    protected float currentHealth;                  // The current health the enemy has

    // Attack Variables - all set in the Prefab Instance
    protected bool invincibility;                 // Gives Enemy brief invincibility when hit by a melee attack
    protected bool hitByMeleeBool;                 // Determines if the enemy has been hit by a melee attack by the player

    // Ability Management
    private List<bool> canUseAbility;
    private List<int> abilityCount;       // Works has a count of the number of abilities are out for a specific ability (goes with the enemy ability prefab)
    [HideInInspector] public List<GameObject> enemyAbilityObjects;    // a list keeping track of all of the abilities out for a specific ability
    [HideInInspector] public List<GameObject> enemyBlobs;
    private GameObject parent;
    private int parentAbilityNum;

    // Bullet Management
    private int currentGunIndex;
    private bool initialBulletDelay;
    private List<GameObject> enemyGuns;

    // Boss Management
    public int phase;
    private float totalTime;
    #endregion

    #region Enemy Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    public bool HitByMeleeBool
    {
        get { return hitByMeleeBool; }
    }
    public GameObject Parent
    {
        set { parent = value; }
    }
    public int ParentAbilityNum
    {
        set { parentAbilityNum = value; }
    }
    public int Phase
    {
        get { return phase; }
        set { phase = value; }
    }
    #endregion

    #region Initialization for Enemy
    // Use this for initialization
    void Start()
    {
        // Set the player reference
        player = GameObject.FindGameObjectWithTag("player");
        levelManager = GameObject.Find("LevelManager");

        // Sets up the enemy's awareness distance in this class
        seekDistance = GetComponent<EnemyMovement>().awareDistance;

        // Sets up whether and enemy can shoot bullets or not
        currentGunIndex = 0; // Set the current Gun index to 0
        // Create a list full of the enemies guns
        enemyGuns = new List<GameObject>();
        if(hasBullets)
        {
            foreach(Transform child in transform)
            {
                if(child.GetComponent<GunScript>())
                {
                    enemyGuns.Add(child.gameObject);
                    //Debug.Log(child.gameObject);
                }
            }
        }

        // Adds a list of for tracking active abilities in the level and if you can use it
        abilityCount = new List<int>();
        canUseAbility = new List<bool>();
        for(int i = 0; i<enemyAbilityPrefabs.Count; i++)
        {
            canUseAbility.Add(true);
            abilityCount.Add(0);
        }

        hitByMeleeBool = false; // Set true so enemy can get hit by melee
        enemyAbilityObjects = new List<GameObject>();
        enemyBlobs = new List<GameObject>();

        // Sets up Enemy's HealthBar
        currentHealth = maxHealth;
        GetComponent<HealthBar>().HealthBarInstantiate();

        // start the enmy phase on 0
        phase = 0;

        // set initial bullet delay to true
        initialBulletDelay = true;

        // Determining Experience
        experience = Mathf.CeilToInt(maxHealth + damage); // initial calculation
    }
    #endregion

    #region Update for Enemy
    void Update()
    {
        // Check for death first
        Death();

        if ((player.transform.position - transform.position).magnitude < seekDistance)
        {
            // update deltaTime next
            totalTime += Time.deltaTime;
            if(totalTime>INITIAL_SHOOT_DELAY)
            {
                initialBulletDelay = false;
            }
        }

        #region Special Movement and shoot methods (For bosses mostly)
        switch (monster)
        {
            case enemyType.prisonLeader:
                if (phase == 0)
                {
                    hasBullets = false;
                }

                // Forwarding the phase depending on the Prisoner Leader's health
                if ((currentHealth / maxHealth) < .666f && phase == 0)
                {
                    phase++;
                    hasBullets = true;
                    timeBetweenAbilities[0] = Mathf.Infinity;
                }
                if ((currentHealth / maxHealth) < .333f && phase == 1)
                {
                    phase++;
                    for(int i=0; i<enemyWeapons.Count; i++)
                    {
                        enemyWeapons[i].GetComponent<EnemyWeaponScript>().speed = 3.75f;
                    }
                }

                break;
            case enemyType.necromancer:
                if (phase == 0)
                {
                    hasAbility = false;
                }

                // Forwarding the phase depending on time
                if (totalTime > 8f)
                {
                    // Increment the phase and reset the timer
                    phase++;
                    phase %= 3;
                    totalTime = 0;

                    switch (phase)
                    {
                        case 0:
                            //Debug.Log("Phase 1");
                            enemyGuns[currentGunIndex].GetComponent<GunScript>().timeBetweenShots = 1f;
                            hasAbility = false;
                            for (int i = 0; i < 4; i++)
                            {
                                enemyObjects[i].SetActive(true);
                            }
                            break;
                        case 1:
                            hasAbility = false;
                            //Debug.Log("Phase 2");
                            enemyGuns[currentGunIndex].GetComponent<GunScript>().timeBetweenShots = .3f;
                            break;
                        case 2:
                            //Debug.Log("Phase 3");
                            // Reset Bullets
                            enemyGuns[currentGunIndex].GetComponent<GunScript>().timeBetweenShots = 3f;
                            for (int i = 4; i < 7; i++)
                            {
                                enemyObjects[i].GetComponent<Spawner>().SpawnEnemy();
                            }
                            break;
                    }
                }
                break;

            case enemyType.dreorsProxy:
                // Forwarding the phase depending on time
                if ((currentHealth / maxHealth) < .5f && phase == 0)
                {
                    phase++;
                    for (int i = 0; i < enemyWeapons.Count; i++)
                    {
                        enemyWeapons[i].SetActive(true);
                    }
                    enemyGuns[currentGunIndex].GetComponent<GunScript>().timeBetweenShots = .5f;
                    abilityRestrictionNumber[0] = 80;
                }
                break;

            case enemyType.skeletonDragon:
                if (phase == 0)
                {
                    hasAbility = false;
                }

                // Forwarding the phase depending on time
                if (totalTime > 8f)
                {
                    // Increment the phase and reset the timer
                    phase++;
                    phase %= 3;
                    totalTime = 0;

                    switch (phase)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                    }
                }

                break;


            default:

                break;
        }
        #endregion

        // Enemy Shooting allows any enemy to shoot when possible
        if (hasBullets == true && initialBulletDelay==false && (player.transform.position - transform.position).magnitude < seekDistance)
        {
            Shoot();
        }

        // Enemy Abilities - allows any enemy to use ability when possible
        for (int i = 0; i < enemyAbilityPrefabs.Count; i++)
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
        

    }
    #endregion

    #region Enemy Helper Methods

    #region Death Related Function
    /// <summary>
    /// Checks if the Enemy should be dead
    /// </summary>
    void Death()
    {
        if(currentHealth <= 0)
        {
            // Tell the parent of it that it died
            if(parent!= null)
            {
                parent.GetComponent<EnemyManager>().abilityCount[parentAbilityNum]--;
                parent.GetComponent<EnemyManager>().enemyAbilityObjects.Remove(gameObject);
            }
 
            
            // Tell the abilities that the owner is dead
            foreach (GameObject ability in enemyAbilityObjects)
            {
                if (ability.GetComponent<BlobScript>() != null)
                {
                    ability.GetComponent<BlobScript>().ownerAlive = false;
                }

                // Kil enemy minions if the owner dies.
                if (ability.GetComponent<EnemyManager>() != null)
                {
                    ability.GetComponent<EnemyManager>().Death();
                }
            }

            // Reward player with experience
            DropRewards();

            // Destroy Enemys
            levelManager.GetComponent<LevelManager>().EnemyEliminated(gameObject);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Rewards player with experience at the death of the enemy
    /// </summary>
    void DropRewards()
    {
        bool experienceDropped = false;

        // Special Case: The enemy is a boss, reward the player directly
        if(boss)
        {
            GameManager.instance.experience += experience;
            experienceDropped = true;
        }

        // Special Case: Last enemy in an elimination level
        foreach(levelType objective in levelManager.GetComponent<LevelManager>().levelObjective)
        {
            if(objective == levelType.extermination && LevelManager.enemies.Count == 1)
            {
                GameManager.instance.experience += experience;
                experienceDropped = true;
            }
        }

        // If the other experience dropped isn't true
        if (!experienceDropped)
        {
            for (int i = 0; i < experience; i++)
            {
                Instantiate(GameManager.experienceOrb, new Vector3(transform.position.x + Random.Range(-transform.localScale.x / 3, transform.localScale.x / 3), transform.position.y + Random.Range(-transform.localScale.y / 3, transform.localScale.y / 3)), transform.rotation);
            }
        }
    }
    #endregion

    #region Shoot Methods
    /// <summary>
    /// Shooting Method
    /// </summary>
    void Shoot()
    {
        for (int i = 0; i < enemyGuns.Count; i++)
        {
            if (enemyGuns[i] != null)
            {
                enemyGuns[i].GetComponent<GunScript>().EnemyFireWeapon();
            }
        }
    }
    #endregion

    #region Ability Methods
    #region Abilities for Enemies
    /// <summary>
    /// Shooting Method
    /// </summary>
    protected void Ability(abilityType ability, int abilityIndex)
    {
        GameObject abilityObject;

        switch(ability)
        {
            case abilityType.blobs:
                // Instantiate the blob
                //Debug.Log("Dropping a doozy");
                abilityObject = Instantiate(enemyAbilityPrefabs[abilityIndex], transform.position, transform.rotation);

                // Add it to a list of blobs/Abilities
                enemyAbilityObjects.Add(abilityObject);
                // Increase the number of ability objects
                abilityCount[abilityIndex]++;


                // Call the special initialization
                abilityObject.GetComponent<BlobScript>().BlobStart(gameObject);
                break;

            case abilityType.babies:
                //Debug.Log("Giving Birth!");
                if (!(abilityRestrictionNumber[abilityIndex] == abilityCount[abilityIndex]))
                {

                    abilityObject = Instantiate(enemyAbilityPrefabs[abilityIndex], transform.position, transform.rotation);
                    abilityObject.GetComponent<EnemyManager>().Parent = gameObject;
                    abilityObject.GetComponent<EnemyManager>().ParentAbilityNum = abilityIndex;

                    // Add it to a list of babies
                    enemyAbilityObjects.Add(abilityObject);

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
        for (int i = 0; i < enemyAbilityPrefabs.Count; i++)
        {
            if (abilityCount[abilityIndex] > abilityRestrictionNumber[abilityIndex])
            {
                if (abilityTypes[abilityIndex] == abilityType.blobs)
                {
                    //Debug.Log("In ability Management");
                    GameObject playerAbilityCopy = enemyAbilityObjects[0];

                    enemyAbilityObjects.Remove(playerAbilityCopy);
                    Destroy(playerAbilityCopy);
                    abilityCount[abilityIndex]--;
                }
                /* Don't need this, the restriction is done before spawning a new baby
                if (abilityTypes[abilityIndex] == abilityType.babies)
                {
                    //Debug.Log("In ability Management");
                    GameObject playerAbilityCopy = enemyAbilityObjects[abilityRestrictionNumber[abilityIndex]];

                    enemyAbilityObjects.Remove(playerAbilityCopy);
                    Destroy(playerAbilityCopy);
                    abilityCount[abilityIndex]--;
                }
                */
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
        currentHealth += damageFloat;
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
