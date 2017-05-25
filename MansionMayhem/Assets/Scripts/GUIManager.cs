using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public GameObject player;

    // Health Management
    private float health;
    public List<GameObject> FullHearts;
    public List<GameObject> HalfHearts;

    int colorIndex;
    public List<Color> HealthColors;
    Color Healthy;
    Color Healthy2;
    Color Healthy3;
    Color Healthy4;

    void Start()
    {
        HealthColors.Add(new Color(186, 0, 0));
        HealthColors.Add(new Color(0, 186, 0));
        HealthColors.Add(new Color(186, 0, 186));
        HealthColors.Add(new Color(186, 186, 186));
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthManagement();
    }


    void HealthManagement()
    {
        // Color determination
        colorIndex = (int)((health / 5));

        // Health Management
        health = player.GetComponent<PlayerManager>().CurrentLife;
        ColorHearts();
        HeartActivation();
    }


    #region Heart Activation
    /// <summary>
    /// Activates the hearts seen on screen
    /// </summary>
    void HeartActivation()
    {
        // Set up hearts initially
        for (int i = 1; i < 6; i++)
        {
            // Activate All Full Hearts
            FullHearts[i-1].SetActive(true);
            //Deactivate all Half Hearts
            HalfHearts[i-1].SetActive(false);
        }

        // De-Activate Unused Full Hearts
        if(health < 5)
        {
            //runs if health is less than, 5 otherwise this for loop doesn't run)
            for (int i = (int)health; i < 5; i++)
            {
                FullHearts[i].SetActive(false);
            }

        }

        // Activate halfHearts if you have a decimal health
        if (health % 1.0f == .5)
        {
            // Color the half heart correctly
            HalfHearts[(int)(((health - 0.5f) % 5.0f))].GetComponent<Image>().color = HealthColors[colorIndex];
            HalfHearts[(int)(((health-0.5f)%5.0f))].SetActive(true);
        }
    }
    #endregion
    
    #region Heart Coloring
    /// <summary>
    /// Colors the hearts correctly based on how much health you have
    /// </summary>
    void ColorHearts()
    {
        int colorCount;
        colorCount = (int)(health % 5);


        #region All other hearts
        #region 1-5 Hearts
        // Determining Color of Hearts 1-5
        if (health <= 5)
        {
            for (int i = 0; i < 5; i++)
            {
                // Sets color of hearts
                FullHearts[i].GetComponent<Image>().color = HealthColors[0];
            }
        }
        #endregion
        else
        {

            // Sets color of hearts to prev color
            for (int i = 0; i < 5; i++)
            {
                FullHearts[i].GetComponent<Image>().color = HealthColors[colorIndex - 1];
            }
            // Sets color of rest of hearts to purple
            for (int i = 0; i < colorCount; i++)
            {
                FullHearts[i].GetComponent<Image>().color = HealthColors[colorIndex];
            }
        }

        #endregion
    }
    #endregion


    void OnGUI()
    {
        GUI.Label(new Rect(100, 10, 400, 50), "Health: " + health);
    }
}
