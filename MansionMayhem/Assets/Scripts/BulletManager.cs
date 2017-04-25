﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    #region Attributes
    // Attributes
    //speed
    private float speed;
    private float damage;
    public bulletTypes bulletType;

    // Speed Variables
    private Vector3 velocity;
    private Vector3 direction;
    public GameObject owner;
    #endregion

    #region BulletStartMethod
    // Use this for initialization
    void BulletStart()
    {
        // Set bullet speed depending on the bullet
        #region Player Bullets
        if (owner.tag == "player")
        {
            direction = owner.GetComponent<PlayerMovement>().ReturnDirection();


            switch (bulletType)
            {
                case bulletTypes.aetherlight:
                    speed = 6f + velocity.magnitude;
                    damage = 3;
                    return;
                case bulletTypes.antiEctoPlasm:
                    speed = 4f + velocity.magnitude;
                    damage = 2;
                    return;
                case bulletTypes.ice:
                    speed = 5f + velocity.magnitude;
                    damage = 1;
                    return;
                case bulletTypes.hellFire:
                    speed = 5f + velocity.magnitude;
                    damage = 1;
                    return;
            }
        }
        #endregion

        #region Enemy Bullets
        else if (owner.tag == "enemy")
        {
            direction = owner.GetComponent<EnemyMovement>().ReturnDirection();

            switch (bulletType)
            {
                case bulletTypes.ectoPlasm:
                    speed = 5f + velocity.magnitude;
                    damage = 1;
                    return;
                case bulletTypes.hellFire:
                    speed = 5f + velocity.magnitude;
                    damage = 1;
                    return;
            }
        }
        #endregion

    }
    #endregion

    #region Bullet Management Helper Methods
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        velocity = direction * speed * Time.deltaTime;
        transform.position += velocity;
    }

    public void bulletSetUp(GameObject shooter)
    {
        owner = shooter;
        BulletStart();
    }
    #endregion

    #region CheckOnScreenMethod
    /// <summary>
    /// CheckOnScreen Method removes the bullet if it goes out of view.
    /// </summary>
    void CheckOnScreen()
    {
        // Takes the camera and creates viewport position variable
        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);


        // If the Bullet goes off the map
        if (viewportPosition.x > 1.05 || viewportPosition.x < -0.05 || viewportPosition.y > 1.07 || viewportPosition.y < -0.07)
        {
            GameObject.Find("Player").GetComponent<PlayerManager>().playerBullets.Remove(this.gameObject);
            Destroy(this.gameObject);
            GameObject.Find("Player").GetComponent<PlayerManager>().BulletCount--;
        }

    }
    #endregion

    #region CollisionDetection
    /// <summary>
    /// Player Collision Handled Here. This includes any objects that is effected by the player colliding with it.
    /// This includes: Coins, Health, Ammo, Enemies, Walls, and Furniture.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        // If the bullet Runs into a wall
        #region Wall Collision
        if (collider.tag == "leftwall" || collider.tag == "rightwall" || collider.tag == "topwall" || collider.tag == "bottomwall")
        {
            // Delete the player bullet
            Debug.Log("Wall!");
            GameObject.Find("Player").GetComponent<PlayerManager>().playerBullets.Remove(this.gameObject);
            Destroy(this.gameObject);
            GameObject.Find("Player").GetComponent<PlayerManager>().BulletCount--;
        }
        #endregion

        #region Enemy Collision with playerBullet
        // If bullet runs into an enemy
        else if (collider.tag == "enemy" && owner.tag == "player")
        {
            Debug.Log("Bullet Hit Enemy: " + collider.gameObject.GetComponent<EnemyManager>().Monster);

            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().Life -= damage;

            // Delete the player bullet
            owner.GetComponent<PlayerManager>().playerBullets.Remove(gameObject);
            Destroy(gameObject);
            owner.GetComponent<PlayerManager>().BulletCount--;

        }
        #endregion

        #region Player Collision with enemyBullet
        else if (collider.tag == "player" && owner.tag == "enemy")
        {
            Debug.Log("Bullet Hit Player");

            // Damage Enemy
            collider.gameObject.GetComponent<PlayerManager>().Life -= damage;

            // Delete the bullet
            owner.GetComponent<EnemyManager>().enemyBullets.Remove(gameObject);
            Destroy(gameObject);
            owner.GetComponent<EnemyManager>().BulletCount--;
        }
        #endregion
    }
    #endregion
}

