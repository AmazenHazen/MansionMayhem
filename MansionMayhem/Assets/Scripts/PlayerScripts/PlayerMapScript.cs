using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMapScript : MonoBehaviour {

    public List<GameObject> locations;
    private GameObject destination;
    public int currentLocationIndex;
    private int locationIndexMax;

    // Use this for initialization
    void Start()
    {
        // Start the initial location at the highest current level location
        // Currently start them at the initial location
        destination = locations[0];
        currentLocationIndex = 0;
        locationIndexMax = 4; // Total Location indexes currently
    }

    // Update is called once per frame
    void Update()
    {
        playerMovementInput();
    }

    // Helper method to move player icon on screen
    public void playerMovementInput()
    {
        Vector3 playerForce = Vector3.zero;

        // If the user hits a left command
        // Go to the previous location
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {

            // Cannot go to a location below zero
            if (currentLocationIndex != 0)
            {
                // Set the destination as the previous location in the locations index
                destination = locations[currentLocationIndex - 1];

                // Move the player icon to the level icon
                gameObject.transform.position = destination.transform.position;

                // Decrease the level index
                currentLocationIndex--;
            }
        }

        // If the user hits a right command
        // Go to the next location
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentLocationIndex < locationIndexMax)
            {
                // Set the destination as the next location in the locations index
                destination = locations[currentLocationIndex + 1];

                // Move the player icon to the level icon
                gameObject.transform.position = destination.transform.position;

                // Increase the level index
                currentLocationIndex++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (destination.GetComponent<LevelLocation>().unlocked == true)
            {
                // Set the GameManager current level for the GUI
                GameManager.currentLevel = destination.GetComponent<LevelLocation>().level;

                // Enter the level
                SceneManager.LoadScene(currentLocationIndex + 2);
            }
        }
    }
}
