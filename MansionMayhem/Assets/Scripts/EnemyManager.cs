using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    #region Enemy Attributes
    // Attributes
    public enemyType monster;
    private enemyClass monsterType;
    private float maxHealth;
    private float currentLife;
    private float damage;
    private float speedAttribute;
    private float seekDistance;
    private bool invincibility;
    private bool hasBullets;
    private movementType movement;


    // Bullet Management
    private bool canShoot;
    public float timeBetweenShots;
    public List<GameObject> enemyBullets;
    public List<GameObject> enemyBulletPrefabs;
    public int bulletCount;
    #endregion

    #region EnemyProperties
    public float CurrentLife
    {
        get { return currentLife; }
        set { currentLife = value; }
    }
    public float Damage
    {
        get { return damage; }
    }
    public int BulletCount
    {
        get { return bulletCount; }
        set { bulletCount = value; }
    }
    public float SpeedAttribute
    {
        get { return speedAttribute; }
    }
    public float SeekDistance
    {
        get { return seekDistance; }
    }
    public bool CanShoot
    {
        set { canShoot = value; }
    }
    public enemyType Monster
    {
        get { return monster; }
    }
    public enemyClass MonsterType
    {
        get { return monsterType; }
    }
    public movementType Movement
    {
        get { return movement; }
    }
    #endregion

    #region Initialization for Enemy
    // Use this for initialization
    void Start()
    {
        // Sets the monster starting variables based on the type of monster you get
        switch (monster)
        {
            #region spiders
            case enemyType.smallSpider:
                currentLife = 1;
                speedAttribute = 1;
                damage = .5f;
                seekDistance = 5;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.seek;
                monsterType = enemyClass.Spider;
                break;

            #endregion

            #region ghosts
            // Ghosts
            case enemyType.basicGhost:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 3;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.seek;
                monsterType = enemyClass.Ghost;
                break;

            case enemyType.banshee:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                monsterType = enemyClass.Ghost;
                break;
            #endregion

            #region demons
            // Demons
            case enemyType.imp:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                monsterType = enemyClass.Demon;
                break;
            #endregion

            #region shades
            case enemyType.shadeKnight:
                currentLife = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.pursue;
                monsterType = enemyClass.Shade;
                break;
            #endregion
                
            #region default monster
            default:
                currentLife = 1;
                speedAttribute = 1;
                damage = .5f;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.seek;
                break;

            #endregion
        }

        // Sets up whether and enemy can shoot bullets or not
        canShoot = true; // Set to true if player gets within distance of the enemy
        enemyBullets = new List<GameObject>();
}
    #endregion

    #region Update for Enemy
    void Update()
    {
        if(hasBullets == true && canShoot==true && (gameObject.GetComponent<EnemyMovement>().player.transform.position - transform.position).magnitude < seekDistance)
        {
            Shoot();
        }
        death();
    }
    #endregion

    #region Enemy Helper Methods
    /// <summary>
    /// Checks if the Enemy should be dead
    /// </summary>
    void death()
    {
        if(currentLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Shooting Method
    /// </summary>
    void Shoot()
    {
        GameObject bulletCopy;
        bulletCopy = Instantiate(enemyBulletPrefabs[0], transform.position, transform.rotation) as GameObject;
        enemyBullets.Add(bulletCopy);
        bulletCopy.GetComponent<BulletManager>().bulletSetUp(gameObject);
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
    #endregion

}
