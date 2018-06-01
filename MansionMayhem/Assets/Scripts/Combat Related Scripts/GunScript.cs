using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    #region Attributes

    // constant that keeps you from spamming guns right after switching weapons
    const float GUNSWITCHDELAY = 0.3f;
    
    // Basic Gun attributes
    public rangeWeapon gunType;
    public Owner gunOwner;
    public GameObject bulletPrefab;
    public GameObject blobPrefab;
    public float bulletDamage;      // Player's gun's damage is determined by upgrades/unlocks
    public float bulletSpeed;
    public float timeBetweenShots;


    // Bullet Tracking
    [HideInInspector] public List<GameObject> poolBullets;
    [HideInInspector] public List<GameObject> poolBlobs;
    public int maxBullets;
    public int maxBlobs;
    private int blobCount;

    // Special gun tracking attributes
    GameObject particles;
    public bool poisonous;
    private float chargingSizeVar = 1.0f;
    public float maxChargingSizeVar; //5 for PlasmaPistol
    private bool charging;

    // Reset Bullet attributes
    public GameObject bulletCopy;
    private bool canShoot;
    private bool finishSwitch;
    private bool canBurst;

    // Enemy Guns
    public int numberOfBullets;     // number of bullets shot out of a gun when shot

    // Bullet Attributes
    // These will be based in the save file cause they can be upgraded but for now will be public attributes
    //public float bulletDamage;
    //public float bulletSpeed;

    #endregion

    #region Properties
    public GameObject Particles
    {
        get { return particles; }
    }
    public bool Charging
    {
        get { return charging; }
        set { charging = value; }
    }
    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        // Basic Gun variables at script activation
        charging = false;
        canShoot = true;
        canBurst = true;
        finishSwitch = true;

        // Create a bullet pool for the player/enemy
        poolBullets = new List<GameObject>();
        for(int i=0; i<maxBullets; i++)
        {
            poolBullets.Add(Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject);
            poolBullets[i].GetComponent<BulletManager>().owner = gameObject;
            poolBullets[i].GetComponent<BulletManager>().ownerType = gunOwner;
            poolBullets[i].SetActive(false);
        }

        // Create a blob pool for the player/enemy
        poolBlobs = new List<GameObject>();
        for (int i = 0; i < maxBlobs; i++)
        {
            poolBlobs.Add(Instantiate(blobPrefab, transform.position, transform.rotation) as GameObject);
            poolBlobs[i].SetActive(false);
        }

        // To get rid of the first blob if hitting the max
        blobCount = maxBlobs-1;

        switch (gunType)
        {
            case rangeWeapon.flamethrower:
            case rangeWeapon.cryoGun:
            case rangeWeapon.AntimatterParticle:
                particles = gameObject.transform.GetChild(0).gameObject;
                break;
        }

    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update ()
    {
        //BulletManagement();
        //BlobManagement();
	}
    #endregion

    #region Player Fire Weapon
    /// <summary>
    /// Fires the weapon depending on what weapon is currently equipped or being used
    /// </summary>
    public void PlayerFireWeapon()
    {
        if (Input.GetMouseButton(0) && canShoot && finishSwitch)
        {
                switch (gunType)
                {
                    // Special because there are a number of shots shot out
                    case rangeWeapon.hellfireshotgun:
                        ShootShotgunBullet();
                        break;

                    // Special because the courotine for the 3 round burst starts here
                    case rangeWeapon.XenonPulser:
                        if (canBurst)
                        {
                            StartCoroutine(XenonShoot(4, .05f));
                            canBurst = false;
                        }
                        break;

                    // Special because you can charge the weapon up
                    case rangeWeapon.PlasmaCannon:
                        ChargeBullet();
                        break;

                    // Turn on the particle systems for the flamethrower and the frost gun
                    case rangeWeapon.cryoGun:
                    case rangeWeapon.flamethrower:
                    case rangeWeapon.AntimatterParticle:
                        particles.SetActive(true);
                        break;
                    default:
                        ShootBullet();
                        break;
               
            }
        }
        // If not holding down the mouse button/shooting
        else
        {
            switch (gunType)
            {
                // Set the particle guns off
                case rangeWeapon.flamethrower:
                case rangeWeapon.cryoGun:
                case rangeWeapon.AntimatterParticle:
                    particles.SetActive(false);
                    break;
            }

            // if the gun was charging a bullet, fire that bullet!
            if (charging == true)
            {
                // Shoot the Charging bullet if not hitting the mouse button
                charging = false;
                JustShot();

                // Set the speed to shoot the bullet, also make the current copy not the currentChargingBullet
                bulletCopy.GetComponent<BulletManager>().speed = bulletSpeed;
                bulletCopy.GetComponent<BulletManager>().CurrentCharingBullet = false;
            }
        }
    }
    #endregion

    #region Enemy Fire Weapon
    /// <summary>
    /// Fires the weapon depending on what weapon is currently equipped or being used
    /// </summary>
    public void EnemyFireWeapon()
    {
        if (canShoot && finishSwitch)
        {
            switch (gunType)
            {
                // Special because there are a number of shots shot out
                case rangeWeapon.EnemyShotGun:
                    ShootShotgunBullet();
                    break;
                case rangeWeapon.EnemyAllDirectionsGun:
                    ShootAllWays();
                    break;
                case rangeWeapon.EnemyChargeGun:
                    if (!charging)
                    {
                        chargingSizeVar = 1;
                    }
                    ChargeBullet();
                    // if the bullet reaches it's potential size
                    if (chargingSizeVar>=maxChargingSizeVar)
                    {
                        // Shoot the Charging bullet if not hitting the mouse button
                        charging = false;
                        JustShot();

                        // Set the speed to shoot the bullet
                        bulletCopy.GetComponent<BulletManager>().speed = bulletSpeed;
                        bulletCopy.GetComponent<BulletManager>().CurrentCharingBullet = false;
                    }
                    break;
                default:
                    ShootBullet();
                    break;
            }
        }
    }
    #endregion

    #region Shooting Bullet method
    /// <summary>
    /// Launches a bullet (method to shoot a single shot)
    /// </summary>
    void ShootBullet()
    {
        //Debug.Log("Firing Bullet" + bulletPrefab);

        // Shoot the bullet
        foreach(GameObject bullet in poolBullets)
        {
            if(!bullet.activeSelf)
            {
                bulletCopy = bullet;
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.SetActive(true);
                break;
            }
        }

        JustShot();
    }
    #endregion

    #region Plopping Blob method
    /// <summary>
    /// Launches a bullet (method to shoot a single shot)
    /// </summary>
    public void BulletPlopBlob(GameObject bullet)
    {
        //Debug.Log("Firing Bullet" + bulletPrefab);
        // Increase the blob count number
        blobCount++;
        blobCount %= maxBlobs;
        if(poolBlobs[blobCount].activeSelf)
        {
            poolBlobs[blobCount].SetActive(false);
        }

        // Plop the blob
        foreach (GameObject blob in poolBlobs)
        {
            if (!blob.activeSelf)
            {
                blob.transform.position = bullet.transform.position;
                blob.transform.rotation = bullet.transform.rotation;
                blob.SetActive(true);
                blob.GetComponent<BlobScript>().BlobStart(gameObject);
                break;
            }
        }

    }
    #endregion

    #region Shooting Shotgun Method
    /// <summary>
    /// Launches multiple bullets with randomized spray
    /// </summary>
    void ShootShotgunBullet()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            // Shoot the bullet
            foreach (GameObject bullet in poolBullets)
            {
                if (!bullet.activeSelf)
                {
                    bulletCopy = bullet;
                    bullet.transform.position = transform.position;
                    bullet.transform.rotation = transform.rotation;
                    bullet.SetActive(true);
                    break;
                }
            }

            // Determining Shotgun pellets rotation
            float rotationAngle = transform.rotation.z;  // Gets current Angle

            // Determine a random Angle
            rotationAngle += Random.Range(-20, 20);

            // Spread of the bullets
            bulletCopy.transform.Rotate(0, 0, rotationAngle);
        }
        JustShot();
    }
    #endregion

    #region Shooting All Directions Method
    void ShootAllWays()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            // Shoot the bullet
            foreach (GameObject bullet in poolBullets)
            {
                if (!bullet.activeSelf)
                {
                    bulletCopy = bullet;
                    bullet.transform.position = transform.position;
                    bullet.transform.rotation = transform.rotation;
                    bullet.SetActive(true);
                    break;
                }
            }

            // Determining Shotgun pellets rotation
            float rotationAngle = transform.rotation.z;  // Gets current Angle

            // Determine the angle the bullet shoots out
            rotationAngle += i * (360.0f / numberOfBullets);

            // Spread of the bullets
            bulletCopy.transform.Rotate(0, 0, rotationAngle);
        }
        JustShot();
    }
    #endregion

    #region Charge Bullet Method - Used for Charging bullets
    /// <summary>
    /// Charges a bullet
    /// </summary>
    void ChargeBullet()
    {
        // Start Charging the bullet
        if (!charging)
        {
            Debug.Log("Attempting to start to charge bullet");
            //Start charging the bullet
            foreach (GameObject bullet in poolBullets)
            {
                if (!bullet.activeSelf)
                {
                    // Scales the bullet
                    charging = true;
                    chargingSizeVar = 1;
                    bulletCopy = bullet;
                    // resets all initial values for the bullet
                    bullet.transform.localScale = new Vector3(chargingSizeVar, chargingSizeVar, transform.localScale.z);
                    bullet.GetComponent<BulletManager>().owner = gameObject;
                    bullet.GetComponent<BulletManager>().ownerType = gunOwner;
                    bullet.GetComponent<BulletManager>().StartPos = transform.position + (.5f * transform.up);        
                    bullet.transform.position = transform.position + (.5f * transform.up);
                    bullet.transform.rotation = transform.rotation;
                    bullet.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            // Keep the bullet with the player and scale the bullet up
            // Call Special start method for bullets
            if (bulletCopy != null)
            {
                bulletCopy.transform.position = transform.position + (.5f * transform.up);

                // Set the start pos of the bullet while charging so it doesn't get destroyed if not shooting between 0-20 Unity units
                bulletCopy.GetComponent<BulletManager>().StartPos = transform.position + (.5f * transform.up);

                // Rotate the bullet with the player
                bulletCopy.transform.rotation = transform.rotation;

                // Keeps the bullet from being too big
                if (chargingSizeVar < maxChargingSizeVar)
                {
                    chargingSizeVar += 1.1f * Time.deltaTime;
                }

                // Scales the bullet
                bulletCopy.transform.localScale = new Vector3(chargingSizeVar, chargingSizeVar, transform.localScale.z);
            }
        }
    }
    #endregion

    #region Shooting Helper Method for burst guns
    // Helper method for shooting bullets for Xenon Pulser
    IEnumerator XenonShoot(int bulletPrefab, float delayTime)
    {
        for (int i = 0; i < 3; i++)
        {
            if (gunType == rangeWeapon.XenonPulser)
            {
                ShootBullet();
                yield return new WaitForSeconds(delayTime);
                //Debug.Log("Burst");
            }
        }
        Invoke("ResetBurst", .5f);
    }
    #endregion

    #region ResetShooting Methods
    /// <summary>
    /// Keeps the player from spamming the shoot button
    /// </summary>
    public void JustShot()
    {
        // Player Can't shoot for .5 seconds
        //Debug.Log("We Just Shot!");
        canShoot = false;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        Invoke("ResetShooting", timeBetweenShots);
    }

    /// <summary>
    /// Keeps the player from spamming the shoot button after switching weapons
    /// </summary>
    public void JustSwitchGuns()
    {
        // Player Can't shoot for .5 seconds
        //Debug.Log("We Just Switched Weapons!");
        finishSwitch = false;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        Invoke("ResetGunSwitch", GUNSWITCHDELAY);
    }

    /// <summary>
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetShooting()
    {
        canShoot = true;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Resets Player's finishSwitch bool
    /// </summary>
    void ResetGunSwitch()
    {
        finishSwitch = true;
    }

    /// <summary>
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetBurst()
    {
        canBurst = true;
    }

    #endregion

    #region Bullet and Blob Management Methods
    ///// <summary>
    ///// Handles the management of current bullets shot out
    ///// </summary>
    //void BulletManagement()
    //{
    //    if (poolBullets.Count > maxBullets)
    //    {
    //        GameObject playerBulletCopy = poolBullets[0];
    //        poolBullets.Remove(playerBulletCopy);
    //        Destroy(playerBulletCopy);
    //    }
    //}

    ///// <summary>
    ///// Handles the management of current blobs
    ///// </summary>
    //void BlobManagement()
    //{
    //    if (blobCount > maxBlobs)
    //    {
    //        GameObject playerBlobCopy = playerBlobs[0];
    //        blobCount--;
    //        playerBlobs.Remove(playerBlobCopy);
    //        Destroy(playerBlobCopy);
    //    }
    //}
    #endregion

    #region ParticleWeapons

    #endregion
}
