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
    public bool isPoisonous;                   // Determines if the enemy can poison the player
    public bool hasBullets;                    // Determines if the enemy has bullets
    public bool hasAbility;                    // Determines if the enemy has an ability (shoot a web/leave slime behind it)
    public float maxHealth;                    // The amount of health the enemy spawns with
    public float damage;                       // Damage caused when the player is hit by the monster (collision)
    public float rangeDamage;                       // Damage caused when the player is hit by the monster (collision)
    public float speedAttribute;               // How fast the enemy moves
    public float seekDistance;                 // The distance at which an enemy can sense where you are
    public float timeBetweenShots;
    public float timeBetweenAbility;
    public List<GameObject> enemyBulletPrefabs;
    public List<GameObject> enemyAbilityPrefabs;
    

    // Basic Monster Attributes
    private float currentLife;                  // The current health the enemy has
    
    // Attack Variables - all set in the Prefab Instance
    private bool invincibility;                 // Gives Enemy brief invincibility when hit by a melee attack
    private bool hitByMeleeBool;                 // Determines if the enemy has been hit by a melee attack by the player
    
    // Ability Management
    private bool canUseAbility;

    // Bullet Management
    public List<GameObject> enemyBullets;
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
    #endregion

    #region Initialization for Enemy
    // Use this for initialization
    void Start()
    {
        // Sets up whether and enemy can shoot bullets or not
        canShoot = true; // Set to true if player gets within distance of the enemy
        canUseAbility = true;
        hitByMeleeBool = false; // Set true so player can get hit by melee
        enemyBullets = new List<GameObject>();

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


        if(hasBullets == true && canShoot==true && (gameObject.GetComponent<EnemyMovement>().player.transform.position - transform.position).magnitude < seekDistance)
        {
            Shoot();
        }
        if (hasAbility == true && canUseAbility == true && (gameObject.GetComponent<EnemyMovement>().player.transform.position - transform.position).magnitude < seekDistance)
        {
            //Ability();
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
            // Spawn something where the monster died/Give EXP to player?

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
    /// <summary>
    /// Shooting Method
    /// </summary>
    void Ability()
    {
        GameObject abilityCopy;
        abilityCopy = Instantiate(enemyAbilityPrefabs[0], transform.position, transform.rotation) as GameObject;
        enemyBullets.Add(abilityCopy);
        //abilityCopy.GetComponent<AbilityManager>().abilitySetUp(gameObject);

        JustAbilitied();
    }

    /// <summary>
    /// Keeps the player from spamming the shoot button
    /// </summary>
    void JustAbilitied()
    {
        // Player Gains Invincibility for 3 seconds
        canUseAbility = false;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        Invoke("ResetAbility", timeBetweenAbility);

    }

    /// <summary>
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetAbility()
    {
        canShoot = true;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
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
