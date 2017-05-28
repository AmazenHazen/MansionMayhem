using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactScript : MonoBehaviour
{

    public List<GameObject> requirements;
    public List<GameObject> spawns;
    public bool canActivate;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    { 


	}

    public void Activate()
    {
        // Check to see if requirements are completed
        if (requirements.Count == 0)
        {
            // Activates the list of GameObjects
            for (int i = 0; i < spawns.Count; i++)
            {
                spawns[i].SetActive(true);
            }
        }

    }
}
