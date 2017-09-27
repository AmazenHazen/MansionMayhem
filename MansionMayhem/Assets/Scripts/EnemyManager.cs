using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    #region Enemy Attributes
    // Attributes Set by editor
    // Enemy Behavior variables
    public enemyType monster;                   // The specific monster creature name
    public enemyClass primarymonsterType;      // The primary type of monster (demon, ghost, spider)
    public enemyClass secondarymonsterType;    // The secondary type of monster
    public bool boss;                           // Bool determining if the enemy is a boss monster
    public bool hasBullets;                    // Determines if the enemy has bullets
    public bool hasAbility;                    // Determines if the enemy has an ability (shoot a web/leave slime behind it)
    public bool isPoisonous;                   // Determines if the enemy can poison the player
    public bool vampyric;                      // Determines if the enemy heals when hitting the player
    public float maxHealth;                    // The amount of health the enemy spawns with
    public float damage;                       // Damage caused when the player is hit by the monster (collision)
    public float seekDistance;                 // The distance at which an enemy can sense where you are
    public float rangeDamage;                       // Damage caused when the player is hit by the monster (collision)
    public float timeBetweenShots;
    public List<GameObject> enemyBulletPrefabs; // Prefabs of Bullets shot
    public List<GameObject> enemyAbilityPrefabs;// Prefab of Ability being used (Webs, Slime, Etc.)
    public List<abilityType> abilityTypes;      // Determines the ability of the enemy
    public List<float> timeBetweenAbilities;    // Determines how long to wait between abilities
    public List<int> abilityRestrictionNumber;  // The number ascociated with the ability to determine how many ability object there can be in the scene


    // Basic Monster Attributes
    private float currentLife;                  // The current health the enemy has
    
    // Attack Variables - all set in the Prefab Instance
    private bool invincibility;                 // Gives Enemy brief invincibility when hit by a melee attack
    private bool hitByMeleeBool;                 // Determines if the enemy has been hit by a melee attack by the player
    
    // Ability Management
    public List<bool> canUseAbility;
    public List<int> abilityCount;       // Works has a count of the number of abilities are out for a specific ability (goes with the enemy ability prefab)
    public List<GameObject> enemyBullets;           // a list keeping track of all of the current bullets on the screen
    public List<GameObject> enemyAbilityObjects;    // a list keeping track of all of the abilities out for a specific ability
    private GameObject parent;

    // Bullet Management
    public List<GameObject> enemyBlobs;
    private int blobCount;
    private bool canShoot;
    public int bulletCount;
    #endregion

    #region EnemyProperties
    public float CurrentLife
    {
        get { return currentLife; }
        set { currentLife = value; }
    }
    public int BulletCount
    {
        get { return bulletCount; }
        set { bulletCount = value; }
    }
    public bool HitByMeleeBool
    {
        get { return hitByMeleeBool; }
    }
    public int BlobCount
    {
        get { return blobCount; }
        set { blobCount = value; }
    }
    public GameObject Parent
    {
        set { parent = value; }
    }
    #endregion

    #region Initialization for Enemy
    // Use this for initialization
    void Start()
    {
        // Sets up whether and enemy can shoot bullets or not
        canShoot = true; // Set to true if player gets within distance of the enemy

        // Adds a list of true for each ability
        for(int i = 0; i<enemyAbilityPrefabs.Count; i++)
        {
            canUseAbility.Add(true);
            abilityCount.Add(0);
        }

        hitByMeleeBool = false; // Set true so player can get hit by melee
        enemyBullets = new List<GameObject>();
        enemyAbilityObjects = new List<GameObject>();

        // Sets up Player's HealthBar
        currentLife = maxHealth;
        gameObject.GetComponent<HealthBar>().HealthBarInstantiate();
    }
    #endregion

    #region Update for Enemy
    void Update()
    {
        // Check for death first
        death();

        // Enemy Shooting
        if(hasBullets == true && canShoot==true && (gameObject.GetComponent<EnemyMovement>().player.transform.position - transform.position).magnitude < seekDistance)
        {
            Shoot();
        }

        // Enemy Abilities
        for (int i = 0; i < enemyAbilityPrefabs.Count; i++)
        {
            if (hasAbility == true)
            {
                if (canUseAbility[i] == true && ((gameObject.GetComponent<EnemyMovement>().player.transform.position - transform.position).magnitude < seekDistance))
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

    #region Death
    /// <summary>
    /// Checks if the Enemy should be dead
    /// </summary>
    void death()
    {
        if(currentLife <= 0)
        {
            // Tell the parent of it that it died
            if(parent!= null)
            {
                parent.GetComponent<EnemyManager>().enemyAbilityObjects.Remove(gameObject);
            }
 
            
            // Tell the abilities that the owner is dead
            foreach (GameObject ability in enemyAbilityObjects)
            {
                if (ability.GetComponent<BlobScript>() != null)
                {
                    ability.GetComponent<BlobScript>().ownerAlive = false;
                }
            }


            // Destroy Enemys
            GameObject.Find("LevelManager").GetComponent<LevelManager>().EnemyEliminated(gameObject);
            Destroy(gameObject);
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
        bulletCopy = Instantiate(enemyBulletPrefabs[0], transform.position, transform.rotation) as GameObject;
        bulletCopy.GetComponent<BulletManager>().BulletStart(gameObject);
        enemyBullets.Add(bulletCopy);
        bulletCount++;

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
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion

    #region Ability Methods
    #region Abilities for Enemies
    /// <summary>
    /// Shooting Method
    /// </summary>
    void Ability(abilityType ability, int abilityIndex)
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
                abilityObject = Instantiate(enemyAbilityPrefabs[abilityIndex], transform.position, transform.rotation);
                abilityObject.GetComponent<EnemyManager>().Parent = gameObject;

                // Add it to a list of babies
                enemyAbilityObjects.Add(abilityObject);

                // Increase the number of ability objects
                abilityCount[abilityIndex]++;
                break;
        }
    }
    #endregion

    #region Ability Managment
    /// <summary>
    /// Keeps the player from spamming abilities
    /// </summary>
    /// <param name="abilityIndex"></param>

    void JustAbilitied(int abilityIndex)
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

    void abilityManagement(int abilityIndex)
    {
        // Managing the number of ability objects on screen
        for (int i = 0; i < enemyAbilityPrefabs.Count; i++)
        {
            if (abilityCount[abilityIndex] > abilityRestrictionNumber[abilityIndex])
            {
                //Debug.Log("In ability Management");
                GameObject playerAbilityCopy = enemyAbilityObjects[0];

                enemyAbilityObjects.Remove(playerAbilityCopy);
                Destroy(playerAbilityCopy);
                abilityCount[abilityIndex]--;
            }
        }
    }
    #endregion

    #region Vampyric Healing Method
    /// <summary>
    /// Heals the enemy if it hits the use with 
    /// </summary>
    public void VampyricHeal()
    {
        CurrentLife += damage;
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
