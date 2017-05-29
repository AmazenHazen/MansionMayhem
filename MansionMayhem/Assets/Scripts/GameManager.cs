﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Headers for Save Files
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameManager : MonoBehaviour
{
    #region Variables
    // Level Variables
    public static int currentLevel;
    private int highestLevel;

    // Currency Variables
    public static int screws;

    // Internal GameState Variables
    public bool inGame;
    #endregion

    #region initialization
    // Use this for initialization
    void Start ()
    {
        // Load the save file if starting the game up
        Load();

        // Game State Variables
        inGame = false;
    }
    #endregion

    #region GameUpdate
    // Update is called once per frame
    void Update ()
    {

    }
    #endregion

    #region GUI
    /// <summary>
    /// Overloaded OnGUI method:
    /// Puts these thing on the display:
    /// Lives
    /// Score
    /// Level
    /// Boss Health (if bossfight is occuring)
    /// </summary>
    void OnGUI()
    {
        // Color
        GUI.color = Color.gray;
        // Font Size
        GUI.skin.box.fontSize = 20;

        // IN-GAME GUI, THE GUI YOU SEE WHEN YOU NORMALLY PLAY IN GAME
        if(inGame)
        {
            // Score FOR GUI
            GUI.Label(new Rect(10, 30, 400, 50), "Screws: " + screws);
        
            if(GUI.Button(new Rect(10, 300, 100, 30), "Save"))
            {
                Save();
            }
        }
    }
    #endregion




    #region Save and Load Methods
    // This will work for everything but web
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MansionMayhem.dat");

        PlayerData data = new PlayerData();

        // Puts the Variables that need to be saved into the data Class
        data.screws = screws;


        // Serialize the data
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        // Check to see if a save file already exists
        if (File.Exists(Application.persistentDataPath + "/MansionMayhem.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MansionMayhem.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // Set variables based on the save file
            screws = data.screws;
            highestLevel = data.highestLevel;
        }
        else
        {
            // Variables that are not saved are set to original value otherwise
            screws = 0;
            highestLevel = 0;
        }
    }

    #endregion
}


#region Data Container for saving
/// <summary>
/// Class for Saving
/// Just a class that is a "DATA Container" that allows writing the data to a save file
/// </summary>
[Serializable]
class PlayerData
{
    // All saved data here
    public int highestLevel;
    public int screws;

}
#endregion

