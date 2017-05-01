using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    #region Enemy Attributes
    // Attributes
    public enemyType monster;
    private enemyClass primarymonsterType;
    private enemyClass secondarymonsterType;
    private float maxHealth;
    private float currentLife;
    private float damage;
    private float speedAttribute;
    private float seekDistance;
    private bool invincibility;
    private bool hasBullets;
    public bool hitByMeleeBool;
    public bool boss;
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
    public enemyClass PrimaryMonsterType
    {
        get { return primarymonsterType; }
    }
    public enemyClass SecondaryMonsterType
    {
        get { return secondarymonsterType; }
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
                primarymonsterType = enemyClass.Spider;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;
            case enemyType.blackWidow:
                currentLife = 1;
                speedAttribute = 1;
                damage = .5f;
                seekDistance = 5;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.seek;
                primarymonsterType = enemyClass.Spider;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;
            case enemyType.redTermis:
                currentLife = 1;
                speedAttribute = 1;
                damage = .5f;
                seekDistance = 5;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.seek;
                primarymonsterType = enemyClass.Spider;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;
            case enemyType.tarantula:
                currentLife = 1;
                speedAttribute = 1;
                damage = .5f;
                seekDistance = 5;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.seek;
                primarymonsterType = enemyClass.Spider;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;
            case enemyType.wolfSpider:
                currentLife = 1;
                speedAttribute = 1;
                damage = .5f;
                seekDistance = 5;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.seek;
                primarymonsterType = enemyClass.Spider;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;
            case enemyType.silkSpinnerSpider:
                currentLife = 1;
                speedAttribute = 1;
                damage = .5f;
                seekDistance = 5;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.seek;
                primarymonsterType = enemyClass.Spider;
                secondarymonsterType = enemyClass.None;
                boss = false;
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
                primarymonsterType = enemyClass.Ghost;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;


            case enemyType.ghostknight:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Ghost;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.banshee:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Ghost;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.ghosthead:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Ghost;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;


            case enemyType.wraith:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Ghost;
                secondarymonsterType = enemyClass.None;
                boss = false;
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
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.boneDemon:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.Skeleton;
                boss = false;
                break;

            case enemyType.corruptedDemon:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.infernalDemon:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.shadowDemon:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.Shadow;
                boss = false;
                break;

            case enemyType.slasherDemon:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.spikeDemon:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.hellhound:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.fury:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            #endregion

            #region zombies
            case enemyType.crawlingHand:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Zombie;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.spitterZombie:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Zombie;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.exploadingZombie:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Zombie;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.stalkerZombie:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Zombie;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.tankZombie:
                currentLife = 15;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Zombie;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            #endregion

            #region skeletons
            case enemyType.charredSkeleton:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Skeleton;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.archerSkeleton:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Skeleton;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.knightSkeleton:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Skeleton;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.mageSkeleton:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Skeleton;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.giantSkeleton:
                currentLife = 2;
                speedAttribute = 1;
                damage = 1;
                seekDistance = 10;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Skeleton;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;


            #endregion

            #region Muck
            case enemyType.blackMuck:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Muck;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.ectoplasmMuck:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Muck;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;


            case enemyType.acidicMuck:
                currentLife = 3;
                speedAttribute = 2;
                damage = 1;
                seekDistance = 7;
                hasBullets = true;
                timeBetweenShots = 2f;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Muck;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            #endregion

            #region shadows
            case enemyType.shadeKnight:
                currentLife = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Shadow;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.shadow:
                currentLife = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Shadow;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.shadowBeast:
                currentLife = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Shadow;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;
            #endregion

            #region Elementals
            case enemyType.infernalElemental:
                currentLife = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Elementals;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.blackFireElemental:
                currentLife = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Elementals;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            case enemyType.acidicElemental:
                currentLife = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.Elementals;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;
            #endregion

            #region other
            case enemyType.gargoyle:
                currentLife = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.None;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;
            case enemyType.possessedArmor:
                currentLife = 4;
                speedAttribute = .75f;
                damage = 1;
                seekDistance = 4;
                hasBullets = false;
                timeBetweenShots = 0;
                movement = movementType.pursue;
                primarymonsterType = enemyClass.None;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            #endregion

            #region Bosses
            case enemyType.giantGhast:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Ghost;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;

            case enemyType.bansheeMistress:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Ghost;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;

            case enemyType.demonLord:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;

            case enemyType.cerberus:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;

            case enemyType.lilith:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Demon;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;

            case enemyType.skeletonDragon:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Skeleton;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;

            case enemyType.necromancer:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Skeleton;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;


            case enemyType.zombiehordeLeader:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Zombie;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;


            case enemyType.grimReaper:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Shadow;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;


            case enemyType.shadowBehemoth:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Shadow;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;


            case enemyType.spiderQueen:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Spider;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;


            case enemyType.pyreLord:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.Elementals;
                secondarymonsterType = enemyClass.None;
                boss = true;
                break;

            case enemyType.dreor:
                currentLife = 20;
                speedAttribute = 1;
                damage = 3;
                seekDistance = 15;
                hasBullets = true;
                timeBetweenShots = 4f;
                movement = movementType.stationary;
                primarymonsterType = enemyClass.None;
                secondarymonsterType = enemyClass.None;
                boss = true;
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
                primarymonsterType = enemyClass.None;
                secondarymonsterType = enemyClass.None;
                boss = false;
                break;

            #endregion
        }

        // Sets up whether and enemy can shoot bullets or not
        canShoot = true; // Set to true if player gets within distance of the enemy
        hitByMeleeBool = false; // Set true so player can get hit by melee
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

    #region Death
    /// <summary>
    /// Checks if the Enemy should be dead
    /// </summary>
    void death()
    {
        if(currentLife <= 0)
        {
            // Spawn something where the monster died?

            // Destroy Enemys
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
