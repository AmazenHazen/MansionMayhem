﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMapScript : MonoBehaviour
{
    #region attibutes
    public List<GameObject> locations;
    private GameObject destination;
    public int currentLocationIndex;
    private int locationIndexMax;
    public GameObject MenuGUIManager;
    #endregion

    #region Properties
    public GameObject Destination
    {
        get { return destination; }
    }
    #endregion


    // Use this for initialization
    void Start()
    {
        // Start the initial location at the highest current level location
        // Currently start them at the initial location
        destination = locations[0];
        currentLocationIndex = 0;
        locationIndexMax = locations.Count-1; // Total Location indexes currently
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


        // Check to see if any other screen is open
        if (MenuGUIManager.GetComponent<LevelSelectGUIManager>().escapeScreen.activeSelf == false && MenuGUIManager.GetComponent<LevelSelectGUIManager>().instructionsScreen.activeSelf == false
            && MenuGUIManager.GetComponent<LevelSelectGUIManager>().loadoutScreen.activeSelf == false && MenuGUIManager.GetComponent<LevelSelectGUIManager>().workbenchScreen.activeSelf == false)
        {
            // If the user hits a left command
            // Go to the previous location
            if (Input.GetKeyDown(KeyCode.A))
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
            if (Input.GetKeyDown(KeyCode.D))
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


            // If the user hits the enter or space button
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (destination.GetComponent<LevelLocation>().unlocked == true)
                {
                    // Set the GameManager current level for the GUI
                    GameManager.instance.currentLevel = destination.GetComponent<LevelLocation>().level;

                    // Set the current gamestate to play
                    GameManager.instance.currentGameState = GameState.Play;

                    // Set the current level to the level
                    GameManager.instance.currentLevel = currentLocationIndex;

                    // Enter the level
                    SceneManager.LoadScene(currentLocationIndex + 2);


                    
                }
            }
        }
    }
}
