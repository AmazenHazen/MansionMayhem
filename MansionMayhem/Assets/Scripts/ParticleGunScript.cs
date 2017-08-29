using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGunScript : MonoBehaviour
{
    // Set owner of the gameObject
    public GameObject owner;
    public GameObject collidingPerson;
    public rangeWeapon particleGun;
    string ownerTag;
    public float damage;
    public float burnEffect;
    public int enemyCollisionCounter;

    void Start()
    {
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
        if ((collider.tag == "enemy" || collider.tag == "boss") && ownerTag == "player")
        {
            if (particleGun == rangeWeapon.cryoGun)
            {
                // Frost gun slows down enemy as well
                collider.gameObject.GetComponent<EnemyMovement>().beingSlowed = true;
                collider.gameObject.GetComponent<EnemyMovement>().currentSpeed -= .05f;
            }
            if ((particleGun == rangeWeapon.flamethrower) && damage<.05f)
            {
                burnEffect += .000005f;
                damage = damage + burnEffect;
            }

            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().CurrentLife -= damage;
        }
    }

    /// <summary>
    /// No longer colliding with an enemy unit
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.tag == "enemy" || collider.tag == "boss") && ownerTag == "player")
        {
            enemyCollisionCounter--;
        }
        if(enemyCollisionCounter == 0)
        {
            Debug.Log("LOL IT WOrked no more fire");
            damage = .025f;
            burnEffect = 0;
        }
    }
}
