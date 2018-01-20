using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    #region Attributes
    // Attributes
    //speed
    public float speed;
    public bool isPoisonous;
    private float damage = .01f;
    private bool canDamage = true; // Debug so only one enemy can be hit with a bullet
    public bulletTypes bulletType;

    // Speed Variables
    private Vector3 velocity;
    public Vector3 direction;
    public GameObject owner;
    public bulletOwners ownerType;

    // Additional Variables for special weapons/bullets
    private Vector3 startPos;
    public GameObject secondaryBullet;

    // Variables for enemy seeking bullets
    public GameObject[] enemyArray;
    private GameObject[] bossArray;

    // Variable for portal gunbullet
    private int portalNum;
    #endregion

    #region BulletStartMethod
    // Use this for initialization
    public void BulletStart(GameObject shooter)
    {
        // Set can Damage to true
        canDamage = true;

        // Set the owner of the shooter and the position of the shot
        owner = shooter;

        // Check to see if a gun shot out the bullets and if it has an owner associated to it
        if(shooter.GetComponent<GunScript>())
        {
            ownerType = shooter.GetComponent<GunScript>().gunOwner;
        }

        // Otherwise assign the ownership based on the tag of the parent
        else
        {
            if(owner.tag == "player")
            {
                ownerType = bulletOwners.player;
            }
            else
            {
                ownerType = bulletOwners.enemy;
            }
        }


        // Set the start position in bullet start (except for the charge bullet)

        startPos = transform.position;



        // Set bullet speed depending on the bullet for the player
        #region Player Bullets
        if (ownerType == bulletOwners.player)
        {
            switch (bulletType)
            {
                case bulletTypes.aetherlight:
                    speed = 5f;
                    return;
                case bulletTypes.antiEctoPlasm:
                    speed = 4f;
                    damage = 2;
                    return;
                case bulletTypes.ice:
                    speed = 5f;
                    damage = 1;
                    return;
                case bulletTypes.sound:
                    speed = 5f;
                    damage = .75f;
                    return;
                case bulletTypes.laser:
                    speed = 4f;
                    damage = .1f;
                    return;
                case bulletTypes.ElectronBall:
                    speed = 3f;
                    damage = 1f;
                    return;
                case bulletTypes.electron:
                    speed = 3f;
                    damage = .4f;
                    return;
                case bulletTypes.PortalShot:
                    speed = 4f;
                    damage = .1f;
                    return;
                case bulletTypes.Plasma:
                    speed = 5f;
                    damage = .5f * Mathf.Pow(transform.localScale.x, 1.5f);
                    return;
                case bulletTypes.hellFire:
                    speed = 5f;
                    damage = .5f;

                    // Determining Shotgun pellets rotation
                    float rotationAngle = shooter.transform.rotation.z;  // Gets current Angle

                    // Determine a random Angle
                    rotationAngle += Random.Range(-20, 20);

                    // Spread of the bullets
                    transform.Rotate(0, 0, rotationAngle);
                    return;
            }
        }
        #endregion

        // Get the damage from the enemy that shot the bullet (for the enemy)
        #region Enemy Bullets
        else if (ownerType == bulletOwners.enemy)
        {
            damage = shooter.GetComponent<EnemyManager>().rangeDamage;

            switch (bulletType)
            {
                case bulletTypes.Weight:
                    speed = 12f;
                    return;
                default:
                    //Static for enemy bullets rn
                    speed = 5f;
                    return;
            }
        }
        #endregion

    }

    #endregion

    #region bulletUpdate
    // Update is called once per frame
    void Update()
    {
        // Check to make sure the bullet didn't go too far or is moving too slow
        if ((transform.position - startPos).magnitude > 20 || speed<0)
        {
            if (ownerType == bulletOwners.player)
            {
                Debug.Log("Too far");
                Debug.Log("start bullet pos: " + startPos);
                Debug.Log("current bullet pos: " + transform.position);
                Debug.Log("Magnitude: " + (transform.position - startPos).magnitude);


                PlayerBulletDestroy();
            }
            if (ownerType == bulletOwners.enemy)
            {
                EnemyBulletDestroy();
            }
        }

        
        // For AntiEctoplasm gun check to see if the bullet got a certain distance, if so then splatter and destory this bullet
        if (bulletType == bulletTypes.antiEctoPlasm && ownerType == bulletOwners.player && (startPos - transform.position).magnitude > 4)
        {
            PlayerBlob();
            PlayerBulletDestroy();
        }

        // For AntiEctoplasm gun check to see if the bullet got a certain distance, if so then splatter and destory this bullet

        if (bulletType == bulletTypes.PortalShot && ownerType == bulletOwners.player && (startPos - transform.position).magnitude > 5)
        {
            PlayerBlob();
            PlayerBulletDestroy();
        }

        // For electrons seek the closest enemy
        if (bulletType == bulletTypes.electron && ownerType == bulletOwners.player)
        {
            GameObject enemy = FindClosestEnemy();


            if ((transform.position - enemy.transform.position).magnitude < 4 && (startPos - transform.position).magnitude > 1f && enemy!=null)
            {
                SeekingBullet();
            }
            else
            {
                Move();
            }
        }
        else
        {
            Move();
        }
    }
    #endregion

    #region All Bullet Management Helper Methods

    #region Move Method
    /// <summary>
    /// Moves the bullet across the screen
    /// </summary>
    private void Move()
    {
        if(bulletType == bulletTypes.aetherlight)
        {
            speed+=.5f;
            damage = speed * .2f;
        }
        if (bulletType == bulletTypes.Weight)
        {
            speed -= Mathf.Pow(.1f, .9f);
        }
        velocity = transform.up * speed * Time.deltaTime;
        transform.position += velocity;
    }
    #endregion

    #region PlayerBullet Special Helper Methods
    void PlayerBlob()
    {
        GameObject blobCopy = Instantiate(secondaryBullet, transform.position, transform.rotation);

        // Add Anti-Ectoplasm Blob
        owner.GetComponent<GunScript>().playerBlobs.Add(blobCopy);
        owner.GetComponent<GunScript>().BlobCount++;

        blobCopy.GetComponent<BlobScript>().BlobStart(owner);

    }

    /// <summary>
    /// Method that splatters a blob where a bullet lands
    /// </summary>
    void SplatterBlob()
    {
        // First Instantiate a blob where the bullet is
        GameObject blobCopy = Instantiate(secondaryBullet, transform.position, transform.rotation);

        // Check to make sure the enemy hasn't already been killed
        if (owner != null)
        {
            // Add Spider Web Blob
            owner.GetComponent<EnemyManager>().enemyBlobs.Add(blobCopy);
            owner.GetComponent<EnemyManager>().BlobCount++;

            blobCopy.GetComponent<BlobScript>().BlobStart(owner);
        }
    }

    /// <summary>
    /// Seek a Target
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public void SeekingBullet()
    {
        // Create an instance of the Rigidbody
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        // Step 1: Find Desired Velocity
        // This is the vector pointing from myself to my target
        Vector3 desiredVelocity = FindClosestEnemy().transform.position - transform.position;

        // Step 2: Scale Desired to maximum speed
        //         so I move as fast as possible
        desiredVelocity.Normalize();
        desiredVelocity *= speed;

        // Step 3: Calculate your Steering Force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Step 4: Apply the steeringForce
        rb.AddForce(steeringForce);
    }

    private void ElectronSpawn()
    {
        
        // Spawn 5 elctrons
        for (int i = 0; i < 5; i++)
        {
            float rotationAngle = owner.transform.rotation.z;  // Gets current Angle

            // Instantiate an electron
            GameObject electronCopy = Instantiate(secondaryBullet, transform.position, transform.rotation);

            // Change the rotation for each bullet
            rotationAngle += 72;
            transform.Rotate(new Vector3(0,0, rotationAngle));

            // Call the special start method to spawn elections
            electronCopy.GetComponent<BulletManager>().BulletStart(owner);
        }
    }


    #endregion

    #region Enemy Destroy Bullet Helper Methods

    /// <summary>
    /// Enemy Bullet Destory Method
    /// </summary>
    private void EnemyBulletDestroy()
    {
        // Check to make sure the enemy hasn't already been killed
        if (owner != null)
        {
            // Delete the bullet reference in the Enemy Manager
            //owner.GetComponent<EnemyManager>().enemyBullets.Remove(gameObject);
            //owner.GetComponent<EnemyManager>().BulletCount--;
        }
        // Delete the bullet
        Destroy(gameObject);
    }

    /// <summary>
    /// Player Bullet Destory Method
    /// </summary>
    private void PlayerBulletDestroy()
    {
        // Remove and Destroy bullet
        //owner.GetComponent<PlayerManager>().playerBullets.Remove(this.gameObject);
        //owner.GetComponent<PlayerManager>().BulletCount--;
        Destroy(this.gameObject);
    }

    #endregion

    #region otherHelperMethods
    public GameObject FindClosestEnemy()
    {

        enemyArray = GameObject.FindGameObjectsWithTag("enemy");
        bossArray = GameObject.FindGameObjectsWithTag("boss");

        // Find the closest enemy
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        float currentDistance = Mathf.Infinity;

        // Loop through all of the enemies in the scene
        foreach (GameObject enemy in enemyArray)
        {
            currentDistance = (enemy.transform.position - transform.position).magnitude;

            if (currentDistance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = currentDistance;
            }
        }

        // Go through all of the boss's in the scene as well
        foreach (GameObject enemy in bossArray)
        {
            currentDistance = (enemy.transform.position - transform.position).magnitude;

            if (currentDistance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = currentDistance;
            }
        }

        return closestEnemy;
    }
    #endregion
    #endregion

    #region CollisionDetection
    /// <summary>
    /// Player Collision Handled Here. This includes any objects that is effected by the player colliding with it.
    /// This includes: Screws, Health, Ammo, Enemies, Walls, and Furniture.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        // If the bullet Runs into a wall
        #region Wall Collision
        if (collider.tag == "wall")
        {
            // Delete the player bullet
            //Debug.Log("Wall!");

            // if the bullet is antiEctoplasm or portal shot also spawn a blob
            if (bulletType == bulletTypes.antiEctoPlasm || bulletType == bulletTypes.PortalShot)
            {
                PlayerBlob();
            }
            if (bulletType == bulletTypes.splatterWeb)
            {
                SplatterBlob();
            }
            if(bulletType == bulletTypes.ElectronBall)
            {
                ElectronSpawn();
            }

            // Remove refernece to the bullet
            if ((ownerType == bulletOwners.player && bulletType!=bulletTypes.Plasma) || (bulletType==bulletTypes.Plasma && owner.GetComponent<GunScript>().Charging == false))
            {
                // Delete the bullet reference in the Player Manager
                PlayerBulletDestroy();
            }
            if (ownerType == bulletOwners.enemy)
            {
                // Check to make sure the enemy hasn't already been killed
                if (owner != null)
                {
                    // Delete the bullet reference in the Enemy Manager
                    owner.GetComponent<EnemyManager>().enemyBullets.Remove(gameObject);
                    owner.GetComponent<EnemyManager>().BulletCount--;
                }
                Destroy(gameObject);
            }
        }
        #endregion

        #region Enemy Collision with playerBullet
        // If bullet runs into an enemy
        else if ((collider.tag == "enemy" || collider.tag == "boss") && ownerType == bulletOwners.player && canDamage == true)
        {
            if (bulletType != bulletTypes.PortalShot)
            {
                canDamage = false;
            }

            Debug.Log("Bullet Hit Enemy: " + collider.gameObject.GetComponent<EnemyManager>().monster);

            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().CurrentLife -= damage;

            // if the bullet is antiEctoplasm or portal also spawn a blob
            if (bulletType == bulletTypes.antiEctoPlasm)
            {
                PlayerBlob();
            }
            if (bulletType == bulletTypes.ElectronBall)
            {
                ElectronSpawn();
            }

            // Delete the player bullet
            if (bulletType != bulletTypes.PortalShot)
            {
                PlayerBulletDestroy();
            }

        }
        #endregion

        #region Shield Collision with enemyBullet
        else if (collider.tag == "shield" && (ownerType == bulletOwners.enemy))
        {
            Debug.Log("Bullet Hit Shield");

            // Damage Enemy
            collider.GetComponentInParent<PlayerManager>().ShieldLife -= damage;

            // Kill the shield (and start it's reset time)
            if (collider.GetComponentInParent<PlayerManager>().ShieldLife <= 0)
            {
                collider.GetComponentInParent<PlayerManager>().ShieldKilled();
            }

            // Check to make sure the enemy hasn't already been killed
            if (owner != null)
            {
                // Delete the bullet reference in the Enemy Manager
                owner.GetComponent<EnemyManager>().enemyBullets.Remove(gameObject);
                owner.GetComponent<EnemyManager>().BulletCount--;
            }
            // Delete the bullete
            Destroy(gameObject);
        }
        #endregion

        #region Player Collision with enemyBullet
        else if (collider.tag == "player" && (ownerType == bulletOwners.enemy) && !GameObject.Find("Shield"))
        {
            Debug.Log("Bullet Hit Player");

            // Damage Player
            collider.gameObject.GetComponent<PlayerManager>().CurrentLife -= damage;

            // Apply Poison to player
            if (isPoisonous)
            {
                collider.gameObject.GetComponent<PlayerManager>().StartPoison();
            }

            if (bulletType == bulletTypes.splatterWeb)
            {
                SplatterBlob();
            }

            EnemyBulletDestroy();
        }
        #endregion

        #region Player or Enemy Bullet Collision with a breakable object
        else if (collider.tag == "breakable" && (ownerType == bulletOwners.player || ownerType == bulletOwners.enemy))
        {
            // Spawn an object
            collider.GetComponent<BreakableObject>().SpawnInsides();

            // Delete the object
            Destroy(collider.gameObject);

            // if the bullet is antiEctoplasm also spawn a blob
            if (bulletType == bulletTypes.antiEctoPlasm || bulletType == bulletTypes.PortalShot)
            {
                PlayerBlob();
            }
            // If the bullet is also a splatter web spawn a web
            if (bulletType == bulletTypes.splatterWeb)
            {
                SplatterBlob();
            }

            // Check to make sure the enemy hasn't already been killed
            if (ownerType == bulletOwners.player)
            {
                PlayerBulletDestroy();
            }
            if (ownerType == bulletOwners.enemy)
            {
                EnemyBulletDestroy();
            }
        }
        #endregion

    }
    #endregion
}

