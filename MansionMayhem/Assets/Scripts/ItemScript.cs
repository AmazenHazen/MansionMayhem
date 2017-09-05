using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    // Item Type
    public itemType itemVar;
    public GameObject objectOwner;


    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0,360));
    }

    void Update()
    {
        transform.Rotate(0, 0, 2);
    }
}
