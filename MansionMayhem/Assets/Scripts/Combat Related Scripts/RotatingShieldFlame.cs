using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShieldFlame : MonoBehaviour
{

    // Reference to owner and stuff
    public GameObject owner;
    Rigidbody2D rb;
    float angleOfRotation;

    // Combat variables
    public float damage;
    public float speed;
    public bulletOwners ownerType;


    // vector storing the vector for the shield's start position
    Vector3 startPos;

    // Axis to be rotated around
    Vector3 zAxis = new Vector3(0, 0, 1);

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;

        //Debug.Log(startPos);
        //owner = gameObject.transform.parent.gameObject;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rotate();
        move();
    }


    public void rotate()
    {
        if (owner != null)
        {
            Vector3 targetPosition = owner.transform.position;
            Vector3 dir = targetPosition - this.transform.position;

            angleOfRotation = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
            // Draw the vehicle at the correct rotation
            transform.rotation = Quaternion.Euler(0, 0, angleOfRotation);
        }

    }

    public void move()
    {
        if (owner != null)
        {
            transform.RotateAround(owner.transform.position, zAxis, speed * Time.deltaTime);
        }
    }

    void OnEnable()
    {
        if (startPos != Vector3.zero)
        {
            transform.position = startPos;
        }
    }

    /// <summary>
    /// When the player moves into the fire shield
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerStay2D(Collider2D collider)
    {
        //Debug.Log(collider.tag);

        //Debug.Log("Colliding");
        if(collider.tag == "player" && (ownerType == bulletOwners.enemy) && !GameObject.Find("Shield"))
        {
            //Debug.Log("Fire damage enemy");

            // Damage Player
            collider.gameObject.GetComponent<PlayerManager>().CurrentLife -= damage;
            gameObject.SetActive(false);
        }

    }
}
