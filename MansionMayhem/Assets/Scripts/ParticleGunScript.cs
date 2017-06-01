using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGunScript : MonoBehaviour
{
    // Set owner of the gameObject
    public GameObject owner;
    public GameObject collidingPerson;
    string ownerTag;
    float damage;

    void Start()
    {
        ownerTag = owner.tag;
        damage = .025f;
    }

    void Update()
    {
    }

    /// <summary>
    /// Collision with Enemy Unit
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        if ((collider.tag == "enemy" || collider.tag == "boss") && ownerTag == "player")
        {
            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().CurrentLife -= damage;

            // Frost gun slows down enemy as well
            collider.gameObject.GetComponent<EnemyMovement>().beingSlowed = true;
            collider.gameObject.GetComponent<EnemyMovement>().currentSpeed -= .05f;
        }

    }
}
