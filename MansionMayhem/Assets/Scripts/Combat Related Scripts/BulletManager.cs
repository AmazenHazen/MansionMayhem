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
    public GameObject owner;        // Owner refers to the gun shooting it (in most cases)
    private GunScript ownerGunScript;
    public Owner ownerType;         // Either player, Enemy, or Ally

    // Additional Variables for special weapons/bullets
    private Vector3 startPos;
    public GameObject secondaryBullet;

    // Variable for portal gunbullet
    private int portalNum;

    // Variable for bouncing bullets
    GameObject lastBounceWall;
    public LayerMask collisionMask;

    // Variables for RayCast Collision
    Vector3 previousPos;

    // variable held to make sure a bullet is deleted after a certain amount of time
    private float totalTime;
    private float currentDistance;

    // variable for plasma pistol
    bool currentChargingBullet;
    #endregion

    #region Properties
    public Vector3 StartPos
    {
        get { return startPos; }
        set { startPos = value; }
    }
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public bool CurrentCharingBullet
    {
        get { return currentChargingBullet; }
        set { currentChargingBullet = value; }
    }
    #endregion

    #region BulletStartMethod
    // Use this for initialization
    /// <summary>
    /// Pass in the shooter parameter (passes in the gun that shoots the bullet)
    /// The gun that shoots it should have an owner assigned to it, use that to 
    /// determine bullet behavior
    /// </summary>
    /// <param name="shooter"></param>
    void Start()
    {
        // Variables for any instantiated bullets
        currentChargingBullet = true;
        startPos = transform.position;
        previousPos = transform.position;
        canDamage = true;
        totalTime = 0;


        ownerGunScript = owner.GetComponent<GunScript>();

        if (ownerGunScript.bulletSpeed != 0)
        {
            speed = ownerGunScript.bulletSpeed;
        }

        // Set bullet speed depending on the bullet for the player
        #region Player Bullets
        if (ownerType == Owner.Player)
        {
            switch (bulletType)
            {
                case bulletTypes.aetherlight:
                    speed = 5f;
                    return;
                case bulletTypes.laser:
                    speed = 6f;
                    if (GameManager.instance.unlockableBuyables[1]) { damage = .3f; }
                    else { damage = .25f; }
                    return;
                case bulletTypes.antiEctoPlasm:
                    speed = 4f;
                    if (GameManager.instance.unlockableBuyables[6]){ damage = 2.0f; }
                    else { damage = 1.75f; }
                    return;
                case bulletTypes.Plasma:
                    speed = 0;
                    currentChargingBullet = true;
                    return;
                case bulletTypes.ice:
                    speed = 5f;
                    damage = 1;
                    return;
                case bulletTypes.hellFire:
                    speed = 5f;
                    if (GameManager.instance.unlockableBuyables[26]) { damage = .70f; }
                    else { damage = .55f; }
                    return;
                case bulletTypes.sound:
                    speed = 5f;
                    if (GameManager.instance.unlockableBuyables[31]) { damage = 1f; }
                    else { damage = 2f; }
                    return;
                case bulletTypes.DarkEnergy:
                    speed = 9f;
                    if (GameManager.instance.unlockableBuyables[36]) { damage = .25f; }
                    else { damage = .22f; }
                    return;
                case bulletTypes.Xenon:
                    speed = 5f;
                    if (GameManager.instance.unlockableBuyables[41]) { damage = .75f; }
                    else { damage = .90f; }
                    return;
                case bulletTypes.ElectronBall:
                    speed = 3f;
                    if (GameManager.instance.unlockableBuyables[46]) { damage = 1.25f; }
                    else { damage = 1.0f; }
                    return;
                case bulletTypes.electron:
                    speed = 3f;
                    if (GameManager.instance.unlockableBuyables[47]) { damage = .6f; }
                    else { damage = .4f; }
                    return;
                case bulletTypes.PreciousMetal:
                    speed = 5f;
                    if (GameManager.instance.unlockableBuyables[56]) { damage = .70f; }
                    else { damage = .55f; }
                    return;
                case bulletTypes.Tempest:
                    speed = 5f;
                    if (GameManager.instance.unlockableBuyables[61]) { damage = .70f; }
                    else { damage = .55f; }
                    return;
            }
        }
        #endregion

        // Get the damage from the enemy that shot the bullet (for the enemy)
        #region Enemy Bullets
        else if (ownerType == Owner.Enemy)
        {
            if (ownerGunScript.bulletDamage != 0)
            {
                damage = ownerGunScript.bulletDamage;
            }
            
            else
            {
                Debug.Log("Damage for gun not set.");
            }

            currentChargingBullet = false;

            switch (bulletType)
            {
                case bulletTypes.Weight:
                    speed = 10f;
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
        //increasing delta time
        totalTime += Time.deltaTime;
        currentDistance = (startPos - transform.position).magnitude;

        // If the enemy dies
        if (owner == null)
        {
            BulletDestroy();
        }

        // Check to make sure the bullet hasn't been on the screen too long
        if (totalTime > 15 && bulletType!=bulletTypes.Plasma)
        {
            BulletDestroy();
        }

        // Check to make sure the bullet didn't go too far or is moving too slow
        if (currentDistance > 20 || speed<0)
        {
            BulletDestroy();
        }

        switch(bulletType)
        {
            // Splatter the bullet if antiectoplasm
            case bulletTypes.antiEctoPlasm:
                if(currentDistance > 4)
                {
                    PlayerBlob();
                    BulletDestroy();
                }
                Move();
                break;
                
            // Increase speed for an aetherlight bullet
            case bulletTypes.aetherlight:
                speed += .5f;
                Move();
                break;
                
                    
            // For electrons seek the closest enemy
            case bulletTypes.electron:
                GameObject enemy = null;
                enemy = FindClosestEnemy();

                if ((transform.position - enemy.transform.position).magnitude < 4 && (currentDistance > 1f && enemy != null))
                {
                    SeekingBullet();
                }
                else
                {
                    Move();
                }
                break;

            // Weights slow down dramatically
            case bulletTypes.Weight:
                speed -= Mathf.Pow(.1f, .9f);
                Move();
                break;


            // The default is to move a bullet
            default:
                Move();
                break;
            
        }
        // Check for collisions
        RayCastCollisionDetection();
    }
    #endregion

    #region All Bullet Management Helper Methods

    #region Move Method
    /// <summary>
    /// Moves the bullet across the screen
    /// </summary>
    private void Move()
    {
        // Set previous pos
        previousPos = transform.position;

        velocity = transform.up * speed * Time.deltaTime;
        //Debug.Log(speed);
        transform.position += velocity;
    }
    #endregion

    #region PlayerBullet Special Helper Methods
    void PlayerBlob()
    {
        // Add Anti-Ectoplasm Blob
        ownerGunScript.BulletPlopBlob(gameObject);
    }

    /// <summary>
    /// Method that splatters a blob where a bullet lands
    /// </summary>
    void SplatterBlob()
    {
        // Check to make sure the enemy hasn't already been killed
        if (owner != null)
        {
            // Add Spider Web Blob
            //owner.GetComponent<GunScript>().playerBlobs.Add(blobCopy);
            //
            //blobCopy.GetComponent<BlobScript>().BlobStart(owner);
            ownerGunScript.BulletPlopBlob(gameObject);
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // First rotate the bullet to face the enemy

        // Step 1: Find Desired Velocity
        // This is the vector pointing from myself to my target
        Vector3 desiredVelocity = FindClosestEnemy().transform.position - transform.position;

        //rotate the bullet towards the enemy
        float angleOfRotation = (Mathf.Atan2(desiredVelocity.y, desiredVelocity.x) * Mathf.Rad2Deg) - 90;

        // Draw the vehicle at the correct rotation
        transform.rotation = Quaternion.Euler(0, 0, angleOfRotation);

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
            electronCopy.GetComponent<BulletManager>().owner = owner;
            electronCopy.GetComponent<BulletManager>().ownerType = ownerType;

            // Change the rotation for each bullet
            rotationAngle += 72;
            transform.Rotate(new Vector3(0,0, rotationAngle));
            electronCopy.transform.position += electronCopy.transform.up * .1f;

            // Call the special start method to spawn elections
            //electronCopy.GetComponent<BulletManager>().BulletStart(owner);
        }
    }

    void Bounce(GameObject wall)
    {

        // Will always hit on the top of the bullet (since it is going that direction)
        // We have to check if the wall is to the left or right of the bullets side
        // and then roatate it by 90 in the opposite direction

        // Good start reference: https://answers.unity.com/questions/782091/how-do-i-get-reflection-vector-from-a-2d-collision.html
        if (lastBounceWall != wall)
        {

            //Debug.Log("bounce!");
            //float dotProduct;
            // Check the position based on the local space of this object
            Ray ray = new Ray(transform.position, transform.up);
            //Debug.DrawRay(transform.position, transform.up*.5f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, .5f, 1<<10);


            if(hit.collider !=null)
            {
                
                // Calculate the reflecting vector
                Vector3 reflectDirection = Vector3.Reflect(ray.direction, hit.normal);
                float angle = Mathf.Atan2(reflectDirection.y, reflectDirection.x) * Mathf.Rad2Deg;


                //Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);
                //transform.rotation = targetRot;
                transform.eulerAngles = new Vector3(0, 0, angle);
                

                // The object is to the right
                lastBounceWall = wall;
                
            }

        }


    }

        #endregion

    #region Enemy Destroy Bullet Helper Methods

    /// <summary>
    /// Enemy Bullet Destory Method
    /// </summary>
    private void BulletDestroy()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region otherHelperMethods
    /// <summary>
    /// Finds the closest enemy and returns it to whatever calls this method
    /// </summary>
    /// <returns>The Closest Enemy</returns>
    public GameObject FindClosestEnemy()
    {
        // Find the closest enemy
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        float currentDistance = Mathf.Infinity;

        // Loop through all of the enemies in the scene
        foreach (GameObject enemy in LevelManager.enemies)
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
    /// Bullet collision Handled Here. This includes any objects that are effected by a bullet colliding with it.
    /// </summary>
    /// <param name="collider"></param>
    void ManageCollision(Collider2D collider)
    {
        // If the bullet Runs into a wall
        #region Wall Collision
        if (collider.tag == "wall" || collider.tag == "obstacle" || collider.tag == "interactableobject")
        {
            // Delete the player bullet
            //Debug.Log("Wall!");
            if (!collider.isTrigger)
            {

                // if the bullet is antiEctoplasm or portal shot also spawn a blob
                if (bulletType == bulletTypes.antiEctoPlasm || bulletType == bulletTypes.PortalShot)
                {
                    PlayerBlob();
                }
                if (bulletType == bulletTypes.splatterWeb)
                {
                    SplatterBlob();
                }
                if (bulletType == bulletTypes.ElectronBall)
                {
                    ElectronSpawn();
                }
                if (bulletType == bulletTypes.CelestialCrystal)
                {
                    Bounce(collider.gameObject);
                }

                if (!(bulletType == bulletTypes.Plasma && velocity.magnitude <= 0))
                {
                    //Debug.Log("Destroy Bullet");
                    // Delete the bullet reference in the Player Manager
                    BulletDestroy();
                }

                if (ownerType == Owner.Enemy)
                {
                    // Check to make sure the enemy hasn't already been killed
                    BulletDestroy();
                }
            }
        }
        #endregion

        #region Enemy Collision with playerBullet
        // If bullet runs into an enemy
        else if ((collider.tag == "enemy" || collider.tag == "boss") && ownerType == Owner.Player && canDamage == true)
        {
            if (bulletType != bulletTypes.DarkEnergy)
            {
                canDamage = false;
            }

            //Debug.Log("Bullet Hit Enemy: " + collider.gameObject.GetComponent<EnemyManager>().monster);
            if(bulletType == bulletTypes.aetherlight)
            {
                damage = speed * .2f;
                if (GameManager.instance.unlockableBuyables[51]) { damage = speed * .25f; }
                else { damage = speed * .2f; }

                if (damage > 4.0f) { damage = 4.5f; }
            }
            if (bulletType == bulletTypes.sound)
            {
                if(collider.GetComponent<EnemyMovement>().CurrentSpeed > 2)
                {
                    collider.GetComponent<EnemyMovement>().CurrentSpeed /= 2.5f;
                }
                else
                {
                    collider.GetComponent<EnemyMovement>().CurrentSpeed /= 4.0f;
                }
                //Debug.Log("Push Enemy Back");
            }

            //Debug.Log("Bullet Damaged enemy for: " + damage);
            // Damage Enemy
            if (bulletType == bulletTypes.Plasma)
            {
                if (GameManager.instance.unlockableBuyables[11]) { damage = .75f * Mathf.Pow(transform.localScale.x, 1.5f); }
                else { damage = .7f * Mathf.Pow(transform.localScale.x, 1.5f); }
                collider.GetComponent<EnemyManager>().CurrentHealth -= damage;


                if (ownerGunScript.Charging && currentChargingBullet)
                {
                    ownerGunScript.Charging = false;
                    ownerGunScript.JustShot();
                }
                
            }
            else
            {
                collider.GetComponent<EnemyManager>().CurrentHealth -= damage;
            }

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
            if (bulletType != bulletTypes.DarkEnergy)
            {
                BulletDestroy();
            }

        }
        #endregion

        #region Shield Collision with enemyBullet
        else if (collider.tag == "shield" && (ownerType == Owner.Enemy))
        {
            //Debug.Log("Bullet Hit Shield");

            // Damage Enemy
            collider.GetComponentInParent<PlayerManager>().ShieldHealth -= damage;

            // Kill the shield (and start it's reset time)
            if (collider.GetComponentInParent<PlayerManager>().ShieldHealth <= 0)
            {
                collider.GetComponentInParent<PlayerManager>().ShieldKilled();
            }

            // Delete the bullete
            BulletDestroy();
        }
        #endregion

        #region Player Collision with enemyBullet
        else if (collider.tag == "player" && (ownerType == Owner.Enemy) && !GameObject.Find("Shield"))
        {
            //Debug.Log("Bullet Hit Player");

            // Damage Player
            collider.GetComponent<PlayerManager>().CurrentHealth -= damage;

            // Apply Poison to player
            if (isPoisonous)
            {
                collider.GetComponent<PlayerManager>().StartPoison();
            }

            if (bulletType == bulletTypes.splatterWeb)
            {
                SplatterBlob();
            }

            BulletDestroy();
        }
        #endregion

        #region Player or Enemy Bullet Collision with a breakable object
        else if (collider.tag == "breakable" && (ownerType == Owner.Player || ownerType == Owner.Enemy))
        {
            // Spawn an object
            collider.GetComponent<BreakableObject>().SpawnInsides();

            // Delete the object
            Destroy(collider.gameObject);
            BulletDestroy();

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

            // If the bullet is a charging plasma weapon
            if (owner.GetComponent<GunScript>())
            {
                if (ownerGunScript.Charging && currentChargingBullet)
                {
                    ownerGunScript.Charging = false;
                    ownerGunScript.JustShot();
                }
            }


            // Check to make sure the enemy hasn't already been killed
            BulletDestroy();
        }

        else if (collider.tag == "DestructableEnemy" && (ownerType == Owner.Player))
        {
            // Delete the object
            collider.gameObject.SetActive(false);

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

            // If the bullet is a charging plasma weapon
            if (ownerGunScript)
            {
                if (ownerGunScript.Charging && currentChargingBullet)
                {
                    ownerGunScript.Charging = false;
                    ownerGunScript.JustShot();
                }
            }


            // Check to make sure the enemy hasn't already been killed
            if (ownerType == Owner.Player)
            {
                BulletDestroy();
            }
        }

            #endregion

        }
    #endregion

    /// <summary>
    /// Method to  determind if there was a collision based on a Raycast
    /// </summary>
    void RayCastCollisionDetection()
    {
        // Vector between last position and this position
        Vector3 differenceVector = transform.position - previousPos;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(previousPos, .1f, differenceVector.normalized, differenceVector.magnitude);

        for(int i=0; i <hits.Length; i++)
        {
            //Debug.Log("Hit");
            ManageCollision(hits[i].collider);
        }
    }

    /// <summary>
    /// On enable method
    /// </summary>
    void OnEnable()
    {
        //Debug.Log("On Enable");

        // Set the start position in bullet start (except for the charge bullet)
        startPos = transform.position;
        canDamage = true;
        currentChargingBullet = true;
        totalTime = 0;

        if (bulletType == bulletTypes.Plasma)
        {
            speed = 0;
        }
        if(bulletType == bulletTypes.aetherlight)
        {
            speed = 5f;
        }
        if(bulletType == bulletTypes.Weight)
        {
            speed = 10f;
        }
        if(bulletType == bulletTypes.PreciousMetal)
        {
            // Spawn the bullet with bronze silver or gold coloring
            switch(Random.Range(0, 3))
            {
                case 0:
                    GetComponent<SpriteRenderer>().color = new Color32(0x8C, 0x78, 0x53, 0xFF);
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().color = new Color32(0xE6, 0xE8, 0xFA, 0xFF);
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().color = new Color32(0xCF, 0xB5, 0x3B, 0xFF);
                    break;
            }

        }
    }
}

