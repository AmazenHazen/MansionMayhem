using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponScript : MonoBehaviour
{
    public EnemyWeapon enemyWeapon;
    public GameObject owner;                   // Owner of the weapon
    public float damage;                       // Damage caused when the player is hit by the monster (collision)
    public bool isPoisonous;                   // Determines if the enemy can poison the player
    public bool vampyric;                      // Determines if the enemy heals when hitting the player
    public float speed;                        // Speed at which weapon moves (if it is spinning of being moved.

    // private variables tracking it's original condition
    private Quaternion initialRot;


    void Start()
    {
        initialRot = transform.rotation; 
    }

    void Update()
    {
        switch (enemyWeapon)
        {
            case EnemyWeapon.rightchain:
                transform.Rotate(0, 0, speed);
                break;
            case EnemyWeapon.leftchain:
                transform.Rotate(0, 0, -speed);
                break;
        }


        // If the object is set inactive, set the weapon back to it's original state
        if(gameObject.activeSelf == false)
        {
            transform.rotation = initialRot;
        }
    }

}
