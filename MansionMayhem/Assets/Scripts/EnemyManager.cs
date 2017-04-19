using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    #region Enemy Attributes
    // Attributes
    public enemyType monster;
    private enemyClass monsterType;
    private float life;
    private float damage;
    private float speedAttribute;
    private float seekDistance;
    private bool invincibility;
    private bool hasBullets;
    private bool smartEnemyBool;


    // Bullet Management
    private bool canShoot;
    public List<GameObject> enemyBullets;
    public List<GameObject> enemyBulletPrefabs;
    private int bulletCount;
    #endregion

    #region EnemyProperties
    public float Life
    {
        get { return life; }
        set { life = value; }
    }
    public float Damage
    {
        get { return damage; }
    }
    public float SpeedAttribute
    {
        get { return speedAttribute; }
    }
    public float SeekDistance
    {
        get { return seekDistance; }
    }
    public enemyType Monster
    {
        get { return monster; }
    }
    public enemyClass MonsterType
    {
        get { return monsterType; }
    }
    public bool SmartEnemyBool
    {
        get { return smartEnemyBool; }
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
                life = 1;
                speedAttribute = 1;
                damage = .5f;
                seekDistance = 5;
                hasBullets = false;
                smartEnemyBool = false;
                monsterType = enemyClass.Spider;
                break;

            #endregion

            #region ghosts
            // Ghosts
            case enemyType.basicGhost:
                life = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 3;
                hasBullets = false;
                smartEnemyBool = false;
                monsterType = enemyClass.Ghost;
                break;

            case enemyType.banshee:
                life = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                smartEnemyBool = true;
                monsterType = enemyClass.Ghost;
                break;
            #endregion

            #region demons
            // Demons
            case enemyType.imp:
                life = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                smartEnemyBool = true;
                monsterType = enemyClass.Demon;
                break;
            #endregion

            #region shades
            case enemyType.shadeKnight:
                life = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                smartEnemyBool = true;
                monsterType = enemyClass.Shade;
                break;
            #endregion

            default:
                life = 1;
                speedAttribute = 1;
                damage = .5f;
                hasBullets = false;
                smartEnemyBool = false;
                break;
        }

        // Sets up whether and enemy can shoot bullets or not
        canShoot = true;
        enemyBullets = new List<GameObject>();
}
    #endregion

    #region Update for Enemy
    void Update()
    {
        death();
    }
    #endregion

    #region Enemy Helper Methods
    /// <summary>
    /// Checks if the Enemy should be dead
    /// </summary>
    void death()
    {
        if(life<=0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Shooting Method
    /// </summary>
    void Shoot()
    {
        enemyBullets.Add(Instantiate(enemyBulletPrefabs[0], transform.position, transform.rotation) as GameObject);
        bulletCount++;
    }

    void AdvanceShoot()
    {

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
