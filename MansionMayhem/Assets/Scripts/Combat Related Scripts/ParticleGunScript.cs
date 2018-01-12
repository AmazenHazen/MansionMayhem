using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGunScript : MonoBehaviour
{
    // Set owner of the gameObject
    private GameObject owner;
    public GameObject collidingPerson;
    public rangeWeapon particleGun;
    string ownerTag;
    public float damage;
    public float burnEffect;
    public int enemyCollisionCounter;

    void Start()
    {
        owner = transform.parent.gameObject;

        ownerTag = owner.tag;

        if (particleGun == rangeWeapon.flamethrower)
        {
            damage = .025f;
        }
        if (particleGun == rangeWeapon.cryoGun)
        {
            damage = .025f;
        }

    }

    void Update()
    {
    }

    /// <summary>
    /// Starting to collide with an enemy unit
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.tag == "enemy" || collider.tag == "boss") && ownerTag == "player")
        {
            enemyCollisionCounter++;
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
        if ((collider.tag == "enemy" || collider.tag == "boss") && ownerTag == "player")
        {
            if (particleGun == rangeWeapon.cryoGun && collider.gameObject.GetComponent<EnemyMovement>().CurrentSpeed > 1.5f)
            {
                // Frost gun slows down enemy as well
                collider.gameObject.GetComponent<EnemyMovement>().BeingSlowed = true;
                collider.gameObject.GetComponent<EnemyMovement>().CurrentSpeed -= .05f;
            }
            if ((particleGun == rangeWeapon.flamethrower) && damage<.05f)
            {
                burnEffect += .000005f;
                damage = damage + burnEffect;
            }

            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().CurrentLife -= damage;
        }
        #endregion

        #region Player or Enemy Particle Gun collides with breakable object
        if (collider.tag == "breakable" && (ownerTag == "player" || ownerTag == "enemy"))
        {
            // Spawn an object
            collider.GetComponent<BreakableObject>().SpawnInsides();

            // Delete the breakable
            Destroy(collider.gameObject);
        }
        #endregion
    }

    /// <summary>
    /// No longer colliding with an enemy unit
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit2D(Collider2D collider)
    {
        if (particleGun == rangeWeapon.flamethrower)
        {
            if ((collider.tag == "enemy" || collider.tag == "boss") && ownerTag == "player")
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
}
