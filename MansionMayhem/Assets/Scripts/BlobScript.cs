using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;

public class BlobScript : MonoBehaviour
{

    // Mananagement Variables
    private float damage;
    public bulletTypes blobComposite;
    public GameObject owner;
    public string ownerTag;
    bool isPoisonous;
    public bool ownerAlive;
    public bool slowsPlayer;
    public bool slippy;
    public int portalNum;
    public GameObject otherPortal;

    // Use this for initialization
    public void BlobStart(GameObject shooter)
    {
        // Set the tag to a copy
        owner = shooter;
        ownerTag = owner.tag;
        ownerAlive = true;

        if (blobComposite == bulletTypes.antiEctoPlasm)
        {
            damage = .003f;
            isPoisonous = false;
        }

        //Enemy Blob
        if (blobComposite == bulletTypes.ectoPlasm)
        {
            damage = .0001f;
            isPoisonous = true;
        }

        if (blobComposite == bulletTypes.web)
        {
            slowsPlayer = true;
        }

        if (blobComposite == bulletTypes.blackSlime)
        {
            slippy = true;
        }
        if(blobComposite == bulletTypes.Portal)
        {
            int tempPortalNum = owner.GetComponent<PlayerManager>().PortalNum;
            portalNum = tempPortalNum;
            tempPortalNum++;
            tempPortalNum %= 2;
            Debug.Log(tempPortalNum);
            owner.GetComponent<PlayerManager>().PortalNum = tempPortalNum;
        }

    }
	
	// Update is called once per frame
	void Update()
    {
        if(blobComposite == bulletTypes.ectoPlasm)
        {
            if (owner == false)
            {
                //Debug.Log("Owner = false");
                StartCoroutine(deleteBlob());
            }
        }
	}


    #region CollisionDetection
    /// <summary>
    /// Player Collision Handled Here. This includes any objects that is effected by the player colliding with it.
    /// This includes: Screws, Health, Ammo, Enemies, Walls, and Furniture.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        #region Enemy Collision with playerBlob
        // If an enemy runs ovr the blob
        if ((collider.tag == "enemy" || collider.tag == "boss") && ownerTag == "player")
        {
            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().CurrentLife -= damage;

        }
        #endregion

        #region Player Collision with enemyBlob
        else if (collider.tag == "player" && (ownerTag == "enemy" || ownerTag == "boss"))
        {
            //Debug.Log("Blob Hit Player");

            // Damage Player
            collider.gameObject.GetComponent<PlayerManager>().CurrentLife -= damage;

            // Apply Poison to player
            if (isPoisonous)
            {
                collider.gameObject.GetComponent<PlayerManager>().StartPoison();
            }

            if (slowsPlayer)
            {
                // Webs slows down player
                collider.gameObject.GetComponent<PlayerMovement>().BeingSlowed = true;
                collider.gameObject.GetComponent<PlayerMovement>().CurrentSpeed -= .025f;
            }

            if (slippy)
            {
                collider.gameObject.GetComponent<PlayerMovement>().BeingSped = true;
                collider.gameObject.GetComponent<PlayerMovement>().CurrentSpeed += .025f;
            }
        }
        #endregion

        #region Player Collision with playerblob
        else if (collider.tag == "player" && (ownerTag == "player"))
        {
            if (blobComposite == bulletTypes.Portal)
            {
                // Find the other portal
                GameObject[] portalArray = GameObject.FindGameObjectsWithTag("portal");

                foreach (GameObject portal in portalArray)
                {
                    //Debug.Log("First Portal Num: " + portal.GetComponent<BlobScript>().portalNum);
                    //Debug.Log("Second Portal Num: " + portalNum);


                    if (portal.GetComponent<BlobScript>().portalNum == ((portalNum + 1) % 2))
                    {
                        // Assign the other portal
                        otherPortal = portal;
                        // Teleport the player to the other portal
                        if (owner.GetComponent<PlayerManager>().canTravel)
                        {
                            owner.transform.position = otherPortal.transform.position;
                            owner.GetComponent<PlayerManager>().JustTraveled();
                        }
                    }
                }

            }
        }
        #endregion

    }

    #endregion


    #region Delete blob Method
    IEnumerator deleteBlob()
    {
        //Debug.Log("Waiting");
        yield return new WaitForSeconds(1f);
        //Debug.Log("Destory");
        Destroy(gameObject);
    }
    #endregion
}
