using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    #region Attributes
    // Attributes
    //speed
    private float speed;
    private float damage;
    public bulletTypes bulletType;
    public bulletOwners bulletOwner;

    // Speed Variables
    private Vector3 velocity;
    private Vector3 direction;
    GameObject owner;
    #endregion

    // Use this for initialization
    void Start()
    {
        #region bullet Direction
        // Set the bullet Direction
        if (bulletOwner == bulletOwners.player)
        {
            owner = GameObject.Find("Player");
        }

        if (bulletOwner == bulletOwners.enemy)
        {
            // Set the Direction based on the position of the player
            owner = GameObject.Find("Player");
        }

        owner.GetComponent<PlayerMovement>().ReturnDirection();
        Vector3 userVelocity = owner.GetComponent<PlayerMovement>().velocity;
        #endregion

        #region Set Attributes of the bullet
        // Set bullet speed depending on the bullet
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
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        velocity = direction * speed * Time.deltaTime;
        transform.position += velocity;
    }

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

    #region CollisionDetection
    /// <summary>
    /// Player Collision Handled Here. This includes any objects that is effected by the player colliding with it.
    /// This includes: Coins, Health, Ammo, Enemies, Walls, and Furniture.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
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
        // If bullet runs into wall or enemy
        else if (collider.tag == "enemy" && bulletOwner == bulletOwners.player)
        {
            Debug.Log("Bullet Hit Enemy: " + collider.gameObject.GetComponent<EnemyManager>().Monster);

            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().Life -= damage;

            // Delete the player bullet
            GameObject.Find("Player").GetComponent<PlayerManager>().playerBullets.Remove(this.gameObject);
            Destroy(this.gameObject);
            GameObject.Find("Player").GetComponent<PlayerManager>().BulletCount--;

        }
        #endregion

        #region Player Collision with enemyBullet
        else if (collider.tag == "player" && bulletOwner == bulletOwners.enemy)
        {
            Debug.Log("Bullet Hit Player");

            // Damage Enemy
            collider.gameObject.GetComponent<EnemyManager>().Life -= damage;

            //
        }
        #endregion
    }
    #endregion
}

