using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    void OnTriggerStay2D(Collider2D collider)
    {
        transform.parent.gameObject.GetComponent<PlayerManager>().playerCollisionMethod(collider);
    }
}
