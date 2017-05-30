using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobScript : MonoBehaviour
{

    // Mananagement Variables
    private float damage;
    public bulletTypes blobComposite;
    public GameObject owner;
    public string ownerTag;
    bool isPoisonous;

    // Use this for initialization
    public void BlobStart(GameObject shooter, bulletTypes blobComp)
    {
        // Set the tag to a copy
        owner = shooter;
        ownerTag = owner.tag;

        blobComposite = blobComp;

        if(blobComposite == bulletTypes.antiEctoPlasm)
        {
            damage = .01f;
            isPoisonous = false;
        }

        /*Enemy Blob Timer Starts
        if(ownerTag == "enemy")
        {

        }
        */
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    #region CollisionDetection
    /// <summary>
    /// Player Collision Handled Here. This includes any objects that is effected by the player colliding with it.
    /// This includes: Screws, Health, Ammo, Enemies, Walls, and Furniture.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        #region Enemy Collision with playerBullet
        // If bullet runs into an enemy
        if ((collider.tag == "enemy" || collider.tag == "boss") && ownerTag == "player")
        {
            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().CurrentLife -= damage;

        }
        #endregion

        #region Player Collision with enemyBullet
        else if (collider.tag == "player" && (ownerTag == "enemy" || ownerTag == "boss"))
        {
            Debug.Log("Bullet Hit Player");

            // Damage Player
            collider.gameObject.GetComponent<PlayerManager>().CurrentLife -= damage;

            // Apply Poison to player
            if (isPoisonous)
            {
                collider.gameObject.GetComponent<PlayerManager>().StartPoison();
            }
        }
        #endregion

    }
    #endregion
}
