using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{

    #region Attributes
    // Basic Gun attributes
    public rangeWeapon gunType;
    public bulletOwners gunOwner;
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
    Coroutine sound;

    // Reset Bullet attributes
    GameObject bulletCopy;
    public bool canShoot;
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
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        // Basic Gun variables at script activation
        charging = false;
        canShoot = true;
        canBurst = true;

        switch (gunType)
        {
            case rangeWeapon.flamethrower:
            case rangeWeapon.cryoGun:
                particles = gameObject.transform.GetChild(0).gameObject;
                break;
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        BulletManagement();
        BlobManagement();
	}

    public void FireWeapon()
    {
        if (canShoot && Input.GetMouseButton(0))
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
                case rangeWeapon.soundCannon:
                    if (canBurst)
                    {
                        sound = StartCoroutine(SoundCannonShoot(4, .05f));
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
                    particles.SetActive(false);
                    break;
            }

            // if the gun was charging a bullet, fire that bullet!
            if (charging == true)
            {
                // for charging bullets
                charging = false;
                bulletCopy.GetComponent<BulletManager>().BulletStart(gameObject);
                bulletCount++;
                JustShot();
            }
        }

        
    }

    /// <summary>
    /// Launches a bullet
    /// </summary>
    void ShootBullet()
    {
        Debug.Log("Firing Bullet" + bulletPrefab);

        // Shoot the bullet
        bulletCopy = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        playerBullets.Add(bulletCopy);

        // Call Special start method for bullets
        bulletCopy.GetComponent<BulletManager>().BulletStart(gameObject);

        bulletCount++;

        JustShot();
    }

    /// <summary>
    /// Charges a bullet
    /// </summary>
    void ChargeBullet()
    {
        // Shoot the bullet
        if (!charging)
        {
            bulletCopy = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
            playerBullets.Add(bulletCopy);
            charging = true;
        }
        else
        {
            // Keep the bullet with the player and scale the bullet up
            bulletCopy.transform.position = transform.position + (.5f* transform.up);
            bulletCopy.transform.rotation = transform.rotation;
            if (plasmaSizeVar < 5)
            {
                plasmaSizeVar += 1f * Time.deltaTime;
            }
            bulletCopy.transform.localScale = new Vector3(plasmaSizeVar, plasmaSizeVar, transform.localScale.z);
        }
    }


    // Helper method for shooting bullets for sound Cannon
    IEnumerator SoundCannonShoot(int bulletPrefab, float delayTime)
    {
        for (int i = 0; i < 3; i++)
        {
            if (gunType == rangeWeapon.soundCannon)
            {
                ShootBullet();
                yield return new WaitForSeconds(delayTime);
                //Debug.Log("Burst");
            }
        }
        Invoke("ResetBurst", .5f);
    }

    #region ResetShooting Methods
    /// <summary>
    /// Keeps the player from spamming the shoot button
    /// </summary>
    void JustShot()
    {
        // Player Can't shoot for .5 seconds
        //Debug.Log("We Just Shot!");
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

    /// <summary>
    /// Resets Player's canShoot Bool
    /// </summary>
    void ResetBurst()
    {
        canBurst = true;
    }

    #endregion

    #region Bullet and Blob Management
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
