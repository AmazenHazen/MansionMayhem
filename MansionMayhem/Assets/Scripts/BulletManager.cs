using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    #region Attributes
    // Attributes
    //speed
    public float speed;
    public bool isPoisonous;
    private float damage;
    public bool canDamage; // Debug so only one enemy can be hit with a bullet
    public bulletTypes bulletType;

    // Speed Variables
    private Vector3 velocity;
    public Vector3 direction;
    public GameObject owner;
    public string ownerTag;

    // Additional Variables for special weapons/bullets
    private Vector3 startPos;
    public GameObject antiEctoPlasmBlob;
    public GameObject webBlob;


    // Variables for seeking bulletMethod
    private GameObject closestEnemy = null;
    private float closestDistance = Mathf.Infinity;
    #endregion

    #region BulletStartMethod
    // Use this for initialization
    public void BulletStart(GameObject shooter)
    {
        // Set can Damage to true
        canDamage = true;

        // Set the owner of the shooter and the position of the shot
        owner = shooter;
        startPos = transform.position;

        // Set the tag to a copy
        ownerTag = owner.tag;

        // Set bullet speed depending on the bullet for the player
        #region Player Bullets
        if (ownerTag == "player")
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
                    damage = 1;
                    return;
                case bulletTypes.hellFire:
                    speed = 5f;
                    damage = 1;
                    return;
                case bulletTypes.laser:
                    speed = 4f;
                    damage = 1;
                    return;
            }
        }
        #endregion

        // Get the damage from the enemy that shot the bullet (for the enemy)
        #region Enemy Bullets
        else if (ownerTag == "enemy")
        {
            damage = owner.GetComponent<EnemyManager>().rangeDamage;

            //Static for enemy bullets rn
            speed = 5f;
        }
        else if (ownerTag == "boss")
        {
            damage = owner.GetComponent<EnemyManager>().rangeDamage;

            //Static for enemy bullets rn
            speed = 5f;
        }
        #endregion

    }

    public void BulletShotgunStart(GameObject shooter)
    {
        // Set can Damage to true
        canDamage = true;

        // Set the owner of the shooter and the position of the shot
        owner = shooter;
        startPos = transform.position;

        // Set the tag to a copy
        ownerTag = owner.tag;

        speed = 5f;
        damage = .5f;

        // Determining Shotgun pellets rotation
        float rotationAngle=owner.transform.rotation.z;  // Gets current Angle

        // Determine a random Angle
        rotationAngle += Random.Range(-20, 20);

        // Spread of the bullets
        transform.Rotate(0, 0, rotationAngle);
    }
    #endregion

    #region bulletUpdate
    // Update is called once per frame
    void Update()
    {
        // Check to make sure the bullet didn't go too far
        if ((transform.position - startPos).magnitude > 20)
        {
            if (ownerTag == "player")
            {
                PlayerBulletDestroy();
            }
            if (ownerTag == "enemy" || ownerTag == "boss")
            {
                EnemyBulletDestroy();
            }
        }
        
        // For AntiEctoplasm gun check to see if the bullet got a certain distance, if so then splatter and destory this bullet
        if (bulletType == bulletTypes.antiEctoPlasm && ownerTag == "player" && (startPos - transform.position).magnitude > 4)
        {
            AntiEctoPlasmBlob();

            PlayerBulletDestroy();
        }

        if(bulletType == bulletTypes.electron && ownerTag == "player")
        {
            SeekingBullet();
        }
        Move();

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
        velocity = transform.up * speed * Time.deltaTime;
        transform.position += velocity;
    }
    #endregion

    #region PlayerBullet Special Helper Methods
    void AntiEctoPlasmBlob()
    {

        GameObject blobCopy = Instantiate(antiEctoPlasmBlob, transform.position, transform.rotation);

        // Add Anti-Ectoplasm Blob
        owner.GetComponent<PlayerManager>().playerBlobs.Add(blobCopy);
        owner.GetComponent<PlayerManager>().BlobCount++;

        blobCopy.GetComponent<BlobScript>().BlobStart(owner);

    }

    /// <summary>
    /// Method that splatters a blob where a bullet lands
    /// </summary>
    void SplatterBlob()
    {
        // First Instantiate a blob where the bullet is
        GameObject blobCopy = Instantiate(webBlob, transform.position, transform.rotation);

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
        // Find the closest enemy
        


        // Create an instance of the Rigidbody
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        // Step 1: Find Desired Velocity
        // This is the vector pointing from myself to my target
        Vector3 desiredVelocity = /*targetPos*/ - transform.position;

        // Step 2: Scale Desired to maximum speed
        //         so I move as fast as possible
        desiredVelocity.Normalize();
        desiredVelocity *= speed;

        // Step 3: Calculate your Steering Force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Step 4: Apply the steeringForce
        rb.AddForce(steeringForce);
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
            owner.GetComponent<EnemyManager>().enemyBullets.Remove(gameObject);
            owner.GetComponent<EnemyManager>().BulletCount--;
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
        owner.GetComponent<PlayerManager>().playerBullets.Remove(this.gameObject);
        owner.GetComponent<PlayerManager>().BulletCount--;
        Destroy(this.gameObject);
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

            // if the bullet is antiEctoplasm also spawn a blob
            if (bulletType == bulletTypes.antiEctoPlasm)
            {
                AntiEctoPlasmBlob();
            }
            if (bulletType == bulletTypes.splatterWeb)
            {
                SplatterBlob();
            }


            // Remove refernece to the bullet
            if (ownerTag == "player")
            {
                // Delete the bullet reference in the Player Manager
                GameObject.Find("Player").GetComponent<PlayerManager>().playerBullets.Remove(this.gameObject);
                GameObject.Find("Player").GetComponent<PlayerManager>().BulletCount--;
            }
            if(ownerTag == "enemy" || ownerTag == "boss")
            {
                // Check to make sure the enemy hasn't already been killed
                if (owner != null)
                {
                    // Delete the bullet reference in the Enemy Manager
                    owner.GetComponent<EnemyManager>().enemyBullets.Remove(gameObject);
                    owner.GetComponent<EnemyManager>().BulletCount--;
                }
            }
            Destroy(gameObject);
        }
        #endregion

        #region Enemy Collision with playerBullet
        // If bullet runs into an enemy
        else if ((collider.tag == "enemy" || collider.tag == "boss") && ownerTag == "player" && canDamage == true)
        {
            canDamage = false;
            Debug.Log("Bullet Hit Enemy: " + collider.gameObject.GetComponent<EnemyManager>().monster);

            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().CurrentLife -= damage;

            // if the bullet is antiEctoplasm also spawn a blob
            if (bulletType == bulletTypes.antiEctoPlasm)
            {
                AntiEctoPlasmBlob();
            }

            // Delete the player bullet
            PlayerBulletDestroy();


        }
        #endregion

        #region Shield Collision with enemyBullet
        else if (collider.tag == "shield" && (ownerTag == "enemy" || ownerTag == "boss"))
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
        else if (collider.tag == "player" && (ownerTag == "enemy" || ownerTag == "boss") && !GameObject.Find("Shield"))
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
        else if (collider.tag == "breakable" && (ownerTag == "player" || ownerTag == "enemy"))
        {
            // Spawn an object
            collider.GetComponent<BreakableObject>().SpawnInsides();

            // Delete the object
            Destroy(collider.gameObject);

            // if the bullet is antiEctoplasm also spawn a blob
            if (bulletType == bulletTypes.antiEctoPlasm)
            {
                AntiEctoPlasmBlob();
            }
            // If the bullet is also a splatter web spawn a web
            if (bulletType == bulletTypes.splatterWeb)
            {
                SplatterBlob();
            }

            // Check to make sure the enemy hasn't already been killed
            if (ownerTag == "player")
            {
                PlayerBulletDestroy();
            }
            if (ownerTag == "enemy" || ownerTag == "boss")
            {
                EnemyBulletDestroy();
            }
        }
        #endregion

        }
    #endregion
}

