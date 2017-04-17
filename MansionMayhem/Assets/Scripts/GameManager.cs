using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region Variables
    // HUD/GUI Variables
    public Texture2D heart;

    // Level Variables
    private int level;
    // Internal GameState Variables
    bool bossFight;


    // Variables for Map Creation
    // Room Dictionary
    Dictionary<string, GameObject> roomDictionary;
    // Floor Tiles
    List<GameObject> mainFloor;
    //GameObject mainFloor;
    public GameObject floorprefab;
    #endregion

    #region initialization
    // Use this for initialization
    void Start ()
    {
        // Game State Variables
        bossFight = false;
        level = 0;


        // Level Setup (for now)
        //player = Instantiate(playerprefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        mapIntantiaztion();

    }
    #endregion

    #region GameUpdate
    // Update is called once per frame
    void Update ()
    {
        //GameCollision();
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

        // Health FOR GUI
        // Drawing Health On screen
        GUI.Label(new Rect(10, 10, 400, 50), "Health: " + GameObject.Find("Player").GetComponent<PlayerManager>().Life);
        // Put hearts next to the label matching number of lifes you have
        for (int i = 0; i < GameObject.Find("Player").GetComponent<PlayerManager>().Life; i++)
        {
            //GUI.DrawTexture(new Rect(50 + 20 * i, 10, 20, 20), heart);
        }

        // Score FOR GUI
        GUI.Label(new Rect(10, 30, 400, 50), "Score: " + GameObject.Find("Player").GetComponent<PlayerManager>().Coins);

        // Ammo
        GUI.Label(new Rect(100, 30, 400, 50), "AetherLight: " + GameObject.Find("Player").GetComponent<PlayerManager>().AetherLight);
        GUI.Label(new Rect(100, 60, 400, 50), "AntiEctoplasm: " + GameObject.Find("Player").GetComponent<PlayerManager>().AntiEctoPlasm);

        // Level FOR GUI
        GUI.Label(new Rect(10, 60, 400, 50), "Level: " + level);

        if (bossFight == true)
        {
            GUI.Label(new Rect(600, 10, 400, 50), "Boss Health: " /*+ bossLife*/);
        }
    }
    #endregion

    #region Map Creation
    /// <summary>
    /// Map Creation
    /// </summary>
    public void mapIntantiaztion()
    {
        //Instantiate(floorprefab, new Vector2(0, 0), Quaternion.identity);
        // Floor Tiles


        // Spawn Room

        // Start Creating the Rooms

        mainFloor = new List<GameObject>();


        // Call the create Map method to create the map of the level
        /*
        for (float i = 0; i < 11; i++)
        {
            for (float j = 0; j < 7; j++)
            {
                Debug.Log("Floor Tile Create at: " + new Vector3(i / 1.57f, j / 1.57f, 0));
                mainFloor.Add(Instantiate(floorprefab, new Vector3(i / 1.57f, j / 1.57f, 0), Quaternion.identity) as GameObject);
            }
        }
        */
    }

    public void mapCreation()
    {
        /*
        roomDictionary = new Dictionary<string, List<string>>();
        roomDictionary.Add("FOYER", new List<string>());
        roomDictionary.Add("LIBRARY", new List<string>());
        roomDictionary.Add("KITCHEN", new List<string>());
        roomDictionary.Add("DINING ROOM", new List<string>());
        roomDictionary.Add("BEDROOM", new List<string>());
        roomDictionary.Add("BATHROOM", new List<string>());
        roomDictionary.Add("UPPERLANDING", new List<string>());
        */
    }


    private void levelSetUp()
    {

    }

    #endregion

}
