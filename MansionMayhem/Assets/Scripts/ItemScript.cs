using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    // Item Type
    public ItemType itemVar;

    public GameObject player;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0,360));
        player = GameObject.FindGameObjectWithTag("player");
    }

    void Update()
    {

        transform.Rotate(0, 0, 2);

        // If the player has a magnet
        if (player.GetComponent<PlayerManager>().magnet && ((player.transform.position - gameObject.transform.position).magnitude < player.GetComponent<PlayerManager>().magnetDistance) && gameObject.GetComponent<Rigidbody2D>())
        {
            Debug.Log("Magnetizing");

            // Create an instance of the Rigidbody
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

            // Step 1: Find Desired Velocity
            // This is the vector pointing from my target to my myself
            Vector2 desiredVelocity = player.transform.position - gameObject.transform.position;

            // Step 2: Scale Desired to maximum speed
            //         so I move as fast as possible
            desiredVelocity.Normalize();
            desiredVelocity *= 2.5f;

            // Step 3: Calculate your Steering Force
            Vector3 steeringForce = desiredVelocity;

            // Move the screw towards the player
            rb.AddForce(steeringForce);
        }
    }


}
