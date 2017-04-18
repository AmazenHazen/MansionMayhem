using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{

    #region Movement Variables
    // Vectors for force-based movement
    public Vector3 position;
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float mass;
    public float maxSpeed;
    public float frictionVar;

    // Variables for wandering
    Vector3 futurePosition;
    public float wanderDistance;
    public float wandRadius;
    private float wandAngle;

    #endregion

    #region Start and Update for Movement
    // Use this for initialization
    public virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CalcSteeringForces();
        ApplyFriction(frictionVar);
        UpdatePosition();
        SetTransform();
    }
    #endregion

    #region Basic Movement Methods

    /// <summary>
    ///Abstract CalcSteeringForces to be used with children classes 
    /// </summary>
    protected abstract void CalcSteeringForces();

    /// <summary>
    /// UpdatePosition
    /// Calculate a new position for the vehicle based on incoming forces
    /// </summary>
    protected void UpdatePosition()
    {

        // Step 0: Set the position each loop based on the position of the vehicle
        position = gameObject.transform.position;

        // Step 1: Add accel to vel * time
        velocity += acceleration * Time.deltaTime;

        // keep z velocity zero to keep objects from changing on the z-axis
        velocity.z = 0;

        // Step 2: Change position based on Velocity
        position += velocity * Time.deltaTime;

        // Step 3: Derive a direction (won't do anything with it yet)
        direction = velocity.normalized;

        // Step 4: Zero out acceleration
        // (Start fresh with new force each frame)
        acceleration = Vector3.zero;
    }

    /// <summary>
    /// Applies a force to the vehicle
    /// </summary>
    /// <param name="force"></param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    /// <summary>
    /// Apply friction to a vehicle/Decelleration
    /// </summary>
    /// <param name="coeff"></param>
    protected void ApplyFriction(float coeff)
    {
        // Step 1: Get the negative velocity
        Vector3 friction = velocity * -1;

        //Step 2: Normalize it (friction is not dependent on velocity magnitude)
        friction.Normalize();

        // Step 3: Multiply by the coefficiant of friction
        friction = friction * coeff;

        // Step 4: Add to acceleration
        acceleration += friction;
    }



    /// <summary>
    /// Set the transform component to reflect the local position vector
    /// </summary>
    protected void SetTransform()
    {
        // Rotate this vehicle based on it's up vector
        // unity will rotate the game object to face the gameobject

        // for 3D
        //gameObject.transform.forward = direction;

        // For 2D
        gameObject.transform.position = position;
    }

    #endregion

    #region Enemy Movement Methods
    /// <summary>
    /// Seek a Target (Basic Enemy Movement)
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public Vector3 seek(Vector3 targetPos)
    {
        // Step 1: Find Desired Velocity
        // This is the vector pointing from myself to my target
        Vector3 desiredVelocity = targetPos - position;

        // Step 2: Scale Desired to maximum speed
        //         so I move as fast as possible
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // Step 3: Calculate your Steering Force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Step 4: Return the Steering force
        return steeringForce;
    }

    /// <summary>
    /// Flee a Target (Basic Cowardly Movement)
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public Vector3 flee(Vector3 targetPos)
    {
        // Step 1: Find Desired Velocity
        // This is the vector pointing from my target to my myself
        Vector3 desiredVelocity = position - targetPos;

        // Step 2: Scale Desired to maximum speed
        //         so I move as fast as possible
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // Step 3: Calculate your Steering Force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Step 4: Return the Steering force
        return steeringForce;
    }

    /// <summary>
    /// Pursuing Code Given a Target (Smart Enemy Movement)
    /// </summary>
    /// <param name="tar"></param>
    /// <returns></returns>
    public Vector3 pursue(GameObject tar)
    {
        // step 1: Find the Future Position of the target
        Vector3 futurePos = tar.transform.position + tar.transform.forward;

        // Step 1.5: If the target is too close scale the futurePos so you don't go the opposite way
        if ((position - tar.transform.position).magnitude < 1)
        {
            Vector3 scaledForward = tar.transform.forward;
            scaledForward.x = tar.transform.forward.x * .15f;
            scaledForward.y = tar.transform.forward.y * .15f;
            scaledForward.z = tar.transform.forward.z * .15f;
            futurePos = tar.transform.position + scaledForward;
        }
        else if ((position - tar.transform.position).magnitude < 2)
        {
            Vector3 scaledForward = tar.transform.forward;
            scaledForward.x = tar.transform.forward.x * .3f;
            scaledForward.y = tar.transform.forward.y * .3f;
            scaledForward.z = tar.transform.forward.z * .3f;
            futurePos = tar.transform.position + scaledForward;
        }

        // Step 1.5: If the target is far away, anticipate it's position farther away
        else if ((position - tar.transform.position).magnitude > 4)
        {
            Vector3 scaledForward = tar.transform.forward;
            scaledForward.x = tar.transform.forward.x * 2.5f;
            scaledForward.y = tar.transform.forward.y * 2.5f;
            scaledForward.z = tar.transform.forward.z * 2.5f;
            futurePos = tar.transform.position + scaledForward;
        }

        // Step 2: Find the desired velocity
        Vector3 desiredVelocity = futurePos - position;

        // Step 3: Scale Desired to maximum speed
        //         so I move as fast as possible
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // Step 4: Calculate your Steering Force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Step 5: Return the Steering force
        return steeringForce;
    }

    /// <summary>
    /// Evading Method Given a Target (Smart Enemy Movement - usually paired with another method)
    /// </summary>
    /// <param name="tar"></param>
    /// <returns></returns>
    public Vector3 evade(GameObject tar)
    {
        // step 1: Find the Future Position of the target
        Vector3 futurePos = tar.transform.position + tar.transform.forward;

        // Step 1.5: If the target is too close scale the futurePos so you aren't suicidal
        if ((position - tar.transform.position).magnitude < 2)
        {
            Vector3 scaledForward = tar.transform.forward;
            scaledForward.x = tar.transform.forward.x * .5f;
            scaledForward.y = tar.transform.forward.y * .5f;
            scaledForward.z = tar.transform.forward.z * .5f;
            futurePos = tar.transform.position + scaledForward;
        }

        // Step 2: Find the desired velocity
        Vector3 desiredVelocity = position - futurePos;


        // Step 3: Scale Desired to maximum speed
        //         so I move as fast as possible
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // Step 4: Calculate your Steering Force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Step 5: Return the Steering force
        return steeringForce;
    }

    /// <summary>
    /// Wander Method that returns a steering force based on a future position and wandering angle
    /// </summary>
    /// <returns></returns>
    public Vector3 wander()
    {
        // Step 1: Find the future position
        futurePosition = transform.position + transform.forward * wanderDistance;

        // Step 2: Find a displacement at the end of a vector (off of a circle)
        wandAngle = Random.Range(0, 2 * Mathf.PI);
        futurePosition = futurePosition + new Vector3(Mathf.Cos(wandAngle) * wandRadius, 0, Mathf.Sin(wandAngle) * wandRadius);

        // Step 3: Seek the new wandered position and add it to the ultimate force
        return seek(futurePosition);
    }

    #endregion

    #region Additional Movement Helping Methods
    /// <summary>
    /// Obstable Avoidance Code
    /// </summary>
    /// <param name="obst"></param>
    /// <param name="safeDistance"></param>
    /// <returns></returns>
    public Vector3 avoidObstacle(GameObject obst, float safeDistance)
    {
        // Variables within method
        Vector3 steer = new Vector3(0, 0, 0);

        // Create vecToCenter - a vector from the character to the center of the obstacle
        Vector3 vecToCenter = obst.transform.position - transform.position;

        // Find the distance to the obstacle
        float distance = vecToCenter.magnitude;

        // Return a zero vector if the obstacle is too far to concern us with
        if (distance > safeDistance)
        {
            return steer;
        }

        // Return Zero if the object is behind the vehicle (Dot product with forward results in a zero
        if (Vector3.Dot(vecToCenter, transform.forward) < 0)
        {
            return steer;
        }

        // Return Zero if the sum of the two radii is < the dot product of the vecToCenter and the agent's right and its
        float sumOfRadii;
        sumOfRadii = 1;

        if (sumOfRadii < Vector3.Dot(vecToCenter, obst.transform.right))
        {
            return steer;
        }

        // Use the dot product of the vector to obstacle center and the unit vector
        // to the right of the vehicle to find the projected distance between the centers
        // of the vehicle and obstacle

        // Compare this to the sum of the radii and return a zero vector if we can pass safely
        float dotProduct = Vector3.Dot(vecToCenter, transform.right);


        // If we get this far we are going to collide with something
        // Use the sign of the dot product between the vector
        Vector3 desiredVelocity;

        if (dotProduct > 0)
        {
            // Obstacle is on our right so turn left
            desiredVelocity = -transform.right * maxSpeed;
            Debug.DrawLine(transform.position, obst.transform.position, Color.yellow);
        }
        else
        {
            // Obstacle is on our left so turn right
            desiredVelocity = transform.right * maxSpeed;
            Debug.DrawLine(transform.position, obst.transform.position, Color.yellow);
        }


        // Compute the Steering force required to change current velocity and desired velocity
        steer = desiredVelocity - velocity;


        return steer;
    }

    /// <summary>
    /// A method that keeps the objects on the screen by bouncing them back
    /// </summary>
    void Bounce()
    {
        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
            velocity.x = -1 * velocity.x;

        }
        if (transform.position.x <= -11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
            velocity.x = -1 * velocity.x;
        }
        if (transform.position.y <= -5)
        {
            transform.position = new Vector3(transform.position.x, -5, 0);
            velocity.y = -1 * velocity.y;
        }
        if (transform.position.y >= 5)
        {
            transform.position = new Vector3(transform.position.x, 5, 0);
            velocity.y = -1 * velocity.y;
        }
    }

    /// <summary>
    /// Method to keep objects in bounds
    /// </summary>
    /// <returns></returns>
    public Vector3 bounds()
    {
        // Step 1: Find Desired Velocity
        // This is the vector pointing from myself back into the screen
        Vector3 desiredVelocity = new Vector3(0, 0, 0) - position;

        // Step 2: Scale Desired to maximum speed
        //         so I move as fast as possible
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // Step 3: Calculate your Steering Force
        Vector3 steeringForce = desiredVelocity - velocity;

        // Step 4: Return the Steering force
        return steeringForce;
    }

    /// <summary>
    /// Allows Other Scripts to access the velocity (the bullet specifically to set it's direction)
    /// </summary>
    public Vector3 ReturnDirection()
    {
        return direction;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        #region Walls
        switch (collider.tag)
        {
            // Obstacles

            // Left Wall
            case "leftwall":
                Debug.Log("LeftWall");
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;

                // set x velocity to 0
                velocity.x = 0;
                acceleration.x = 0;
                // Puts the player on the edge (does this by taking the position of the wall + [the value between the center and the outside of wall + the value between the center of the player and the outside of the player sprite + small amount to make the player model off the wall])
                transform.position = new Vector2(collider.gameObject.GetComponent<SpriteRenderer>().bounds.center.x + (collider.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x + .02f), transform.position.y);
                break;

            // Right Wall
            case "rightwall":
                Debug.Log("RightWall");
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;

                // set x velocity to 0
                velocity.x = 0;
                acceleration.x = 0;
                // Puts the player on the edge (does this by taking the position of the wall - [the value between the center and the outside of wall + the value between the center of the player and the outside of the player sprite + small amount to make the player model off the wall])
                transform.position = new Vector2(collider.gameObject.GetComponent<SpriteRenderer>().bounds.center.x - (collider.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x + gameObject.GetComponent<SpriteRenderer>().bounds.extents.x + .02f), transform.position.y);
                break;

            // Top Wall
            case "topwall":
                Debug.Log("TopWall");
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;

                // set x velocity to 0
                velocity.y = 0;
                acceleration.y = 0;
                // Puts the player on the edge (does this by taking the position of the wall - [the value between the center and the outside of wall + the value between the center of the player and the outside of the player sprite + small amount to make the player model off the wall])
                transform.position = new Vector2(transform.position.x, collider.gameObject.GetComponent<SpriteRenderer>().bounds.center.y - (collider.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y + .02f));
                break;

            // Bottom Wall
            case "bottomwall":
                Debug.Log("BottomWall");
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;

                // set y velocity to 0
                velocity.y = 0;
                acceleration.y = 0;
                // Puts the player on the edge (does this by taking the position of the wall + the value between the center and the outside of wall + the value between the center of the player and the outside of the player sprite + small amount to make the player model off the wall)
                transform.position = new Vector2(transform.position.x, collider.gameObject.GetComponent<SpriteRenderer>().bounds.center.y + (collider.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y + gameObject.GetComponent<SpriteRenderer>().bounds.extents.y + .02f));

                break;
        }
        #endregion
    }


    #endregion

}