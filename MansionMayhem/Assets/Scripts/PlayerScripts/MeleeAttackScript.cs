using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackScript : MonoBehaviour
{
    // Attributes
    public float damage;
    GameObject owner;

    void Start()
    {
        owner = GameObject.FindGameObjectWithTag("player");
    }


    /// <summary>
    /// Player Collision Handled Here. This includes any objects that is effected by the player colliding with it.
    /// This includes: Screws, Health, Ammo, Enemies, Walls, and Furniture.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        #region Enemy Collision with melee attack
        // If bullet runs into an enemy
        if ((collider.tag == "enemy" || collider.tag == "boss") && owner.tag == "player")
        {
            if (collider.gameObject.GetComponent<EnemyManager>().HitByMeleeBool == false)
            {
                //Debug.Log("Melee Attack Hit Enemy: " + collider.gameObject.GetComponent<EnemyManager>().monster);

                // Damage Enemy
                collider.gameObject.GetComponent<EnemyManager>().CurrentLife -= damage;

                // Give Enemy a very very brief invinsibility so the melee doesn't damage the enemy multiple times
                collider.gameObject.GetComponent<EnemyManager>().HurtByMelee();
            }
        }
        #endregion
    }
}
