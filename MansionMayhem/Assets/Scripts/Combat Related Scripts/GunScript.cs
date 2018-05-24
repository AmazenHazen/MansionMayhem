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
    public float timeBetweenShots;
    

    // Bullet Tracking
    public List<GameObject> playerBullets;
    public List<GameObject> playerBlobs;
    public int maxBullets;
    public int maxBlobs;
    private int blobCount;
    private int bulletCount;

    // Special gun tracking attributes
    GameObject particles;
    public bool poisonous;
    private float plasmaSizeVar = 1.0f;
    private bool charging;

    // Reset Bullet attributes
    public GameObject bulletCopy;
    public bool canShoot;
    public bool finishSwitch;
    private bool canBurst;

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
    public int BlobCount
    {
        get { return blobCount; }
        set { blobCount = value; }
    }
    public bool Charging
    {
        get { return charging; }
        set { charging = value; }
    }
    public int BulletCount
    {
        get { return bulletCount; }
        set { bulletCount = value; }
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
        BulletManagement();
        BlobManagement();
	}
    #endregion

    #region Fire Weapon
    /// <summary>
    /// Fires the weapon depending on what weapon is currently equipped or being used
    /// </summary>
    public void FireWeapon()
    {
        if (canShoot && finishSwitch && Input.GetMouseButton(0))
        {
            switch (gunType)
            {
                // Special because there are a number of shots shot out
                case rangeWeapon.hellfireshotgun:
                    for (int i = 0; i < 4; i++)
                    {
                        ShootBullet();
                    }
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
                    if (!charging)
                    {
                        plasmaSizeVar = 1;
                    }
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
                bulletCopy.GetComponent<BulletManager>().BulletStart(gameObject);
                bulletCount++;
                JustShot();
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
        bulletCopy = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        playerBullets.Add(bulletCopy);

        // Call Special start method for bullets
        bulletCopy.GetComponent<BulletManager>().BulletStart(gameObject);

        bulletCount++;

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
            //Start charging the bullet
            bulletCopy = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
            bulletCopy.GetComponent<BulletManager>().StartPos = transform.position + (.5f * transform.up);
            bulletCopy.GetComponent<BulletManager>().owner = gameObject;
            bulletCopy.GetComponent<BulletManager>().ownerType = Owner.Player;
            playerBullets.Add(bulletCopy);
            charging = true;
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
                if (plasmaSizeVar < 5)
                {
                    plasmaSizeVar += 1.1f * Time.deltaTime;
                }

                // Scales the bullet
                bulletCopy.transform.localScale = new Vector3(plasmaSizeVar, plasmaSizeVar, transform.localScale.z);
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
    /// <summary>
    /// Handles the management of current bullets shot out
    /// </summary>
    void BulletManagement()
    {
        if (playerBullets.Count > maxBullets)
        {
            GameObject playerBulletCopy = playerBullets[0];
            bulletCount--;
            playerBullets.Remove(playerBulletCopy);
            Destroy(playerBulletCopy);
        }
    }

    /// <summary>
    /// Handles the management of current blobs
    /// </summary>
    void BlobManagement()
    {
        if (blobCount > maxBlobs)
        {
            GameObject playerBlobCopy = playerBlobs[0];
            blobCount--;
            playerBlobs.Remove(playerBlobCopy);
            Destroy(playerBlobCopy);
        }
    }
    #endregion

    #region ParticleWeapons

    #endregion
}
