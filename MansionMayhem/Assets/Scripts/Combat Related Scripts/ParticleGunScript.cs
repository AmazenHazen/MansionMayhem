using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGunScript : MonoBehaviour
{
    // Set owner of the gameObject
    private GameObject owner;
    public GameObject collidingPerson;
    public rangeWeapon particleGun;
    Owner ownerType;
    public float damage;
    public float burnEffect;
    public int enemyCollisionCounter;

    void Start()
    {
        #region assigning ownership
        owner = transform.parent.gameObject;

        // Check to see if a gun shot out the bullets and if it has an owner associated to it
        if (owner.GetComponent<GunScript>())
        {
            ownerType = owner.GetComponent<GunScript>().gunOwner;
        }

        // Otherwise assign the ownership based on the tag of the parent
        else
        {
            if (owner.tag == "player")
            {
                ownerType = Owner.Player;
            }
            else
            {
                ownerType = Owner.Enemy;
            }
        }
        #endregion


        if (particleGun == rangeWeapon.flamethrower)
        {
            if (GameManager.instance.unlockableBuyables[16]) { damage = .032f; }
            else { damage = .025f; }
        }
        if (particleGun == rangeWeapon.cryoGun)
        {
            if (GameManager.instance.unlockableBuyables[21]) { damage = .032f; }
            else { damage = .025f; }
        }
        if (particleGun == rangeWeapon.AntimatterParticle)
        {
            if (GameManager.instance.unlockableBuyables[66]) { damage = .042f; }
            else { damage = .035f; }
        }

    }

    void Update()
    {

    }

    #region Collision
    /// <summary>
    /// Starting to collide with an enemy unit
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.tag == "enemy" || collider.tag == "boss") && ownerType == Owner.Player)
        {
            enemyCollisionCounter++;
        }
    }
    /// <summary>
    /// No longer colliding with an enemy unit
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit2D(Collider2D collider)
    {
        if (particleGun == rangeWeapon.flamethrower)
        {
            if ((collider.tag == "enemy" || collider.tag == "boss") && ownerType == Owner.Player)
            {
                enemyCollisionCounter--;
            }
            if (enemyCollisionCounter == 0)
            {
                //Debug.Log("LOL IT WOrked no more fire");
                damage = .025f;
                burnEffect = 0;
            }
        }
    }

    /// <summary>
    /// Collision with Enemy Unit
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        #region Enemy Collision with player Particle Gun
        // Player particle System Collides with Enemy
        if ((collider.tag == "enemy" || collider.tag == "boss") && ownerType == Owner.Player)
        {
            if (particleGun == rangeWeapon.cryoGun && collider.gameObject.GetComponent<EnemyMovement>().CurrentSpeed > 1.5f)
            {
                // Frost gun slows down enemy as well
                collider.gameObject.GetComponent<EnemyMovement>().BeingSlowed = true;
                collider.gameObject.GetComponent<EnemyMovement>().CurrentSpeed -= .05f;
            }
            if ((particleGun == rangeWeapon.flamethrower) && damage<.05f)
            {
                burnEffect += .00005f;
                damage = damage + burnEffect;
            }

            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().CurrentLife -= damage;
        }
        #endregion

        #region Player or Enemy Particle Gun collides with breakable object
        if (collider.tag == "breakable" && (ownerType == Owner.Player || ownerType == Owner.Enemy))
        {
            // Spawn an object
            collider.GetComponent<BreakableObject>().SpawnInsides();

            // Delete the breakable
            Destroy(collider.gameObject);
        }
        #endregion
    }

    #endregion
}
