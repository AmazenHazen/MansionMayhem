using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public GameObject player;

    // Health Management
    int colorIndex;
    private float health;
    public List<Color> HealthColors;
    public List<GameObject> FullHearts;
    public List<GameObject> HalfHearts;

    // Boss Health Management
    public GameObject boss;
    public GameObject bossHealthCanvas;
    public Slider bossHealthBar; 
    public float bossMaxHealth;
    public float bossCurrentHealth;


    // Variables for changing Text
    public Text rangeWeaponText;
    public Text scoreText;
    public Text levelText;

    // Weapon Management
    rangeWeapon currentRangeWeapon;

    // Start is called when the GUI is initialized
    void Start()
    {
        if (GameObject.Find("LevelManager").GetComponent<LevelManager>().levelObjective == levelType.boss)
        {
            boss = GameObject.FindGameObjectWithTag("boss");
            // Set the healthBar Max value
            bossMaxHealth = boss.GetComponent<EnemyManager>().maxHealth;
            bossHealthBar.maxValue = bossMaxHealth;
        }

        // Health Management
        HealthColors.Add(new Color(0, 0, 0, 255));
        HealthColors.Add(new Color(186, 0, 0));
        HealthColors.Add(new Color(0, 186, 0));
        HealthColors.Add(new Color(186, 0, 186));
        HealthColors.Add(new Color(186, 186, 186));
        HealthColors.Add(new Color(186, 186, 186));
    }

    // Update is called once per frame
    void Update()
    {
        bossHealthManagement();
        HealthManagement();
        TextUpdate();
    }

    #region Health Management
    void HealthManagement()
    {
        // Health Management
        health = player.GetComponent<PlayerManager>().CurrentLife;

        // Color determination
        colorIndex = (int)((health / 5));

        HalfHeartManagement();
        FullHeartManagement();
    }

    #region Heart Activation
    /// <summary>
    /// Activates the hearts seen on screen
    /// </summary>
    void HalfHeartManagement()
    {
        // Set up hearts initially
        for (int i = 1; i < 6; i++)
        {
            //Deactivate all Half Hearts
            HalfHearts[i-1].SetActive(false);
        }

        // Activate halfHearts if you have a decimal health
        if (health % 1.0f == .5)
        {
            // Color the half heart correctly
            HalfHearts[(int)(((health - 0.5f) % 5.0f))].GetComponent<Image>().color = HealthColors[colorIndex+1];
            HalfHearts[(int)(((health-0.5f)%5.0f))].SetActive(true);
        }
    }
    #endregion
    
    #region Full Heart Managment
    /// <summary>
    /// Colors the hearts correctly based on how much health you have
    /// </summary>
    void FullHeartManagement()
    {
        int colorCount;
        colorCount = (int)(health % 5);

        // Sets color of hearts to prev color
        for (int i = 0; i < 5; i++)
        {
            FullHearts[i].GetComponent<Image>().color = HealthColors[colorIndex];
        }

        // Sets color of rest of hearts to current color
        for (int i = 0; i < colorCount; i++)
        {
            FullHearts[i].GetComponent<Image>().color = HealthColors[colorIndex + 1];
        }

    }
    #endregion

    #endregion

    #region Text Management
    void TextUpdate()
    {
        // Get variables needed for the HUD Text
        currentRangeWeapon = player.GetComponent<PlayerManager>().CurrentRangeWeapon;

        rangeWeaponText.text = "Current Weapon: " + currentRangeWeapon;
        scoreText.text = "Screws: " + GameManager.screws;
        levelText.text = "Level: " + GameManager.currentLevel;
    }
    #endregion

    #region Boss HealthBar Management
    void bossHealthManagement()
    {
        if (boss != null)
        {
            if (boss.activeSelf == true)
            {
                Debug.Log("in boss health update");
                bossHealthCanvas.gameObject.SetActive(true);

                // Get current health and update the bar
                bossCurrentHealth = boss.GetComponent<EnemyManager>().CurrentLife;
                bossHealthBar.value = bossCurrentHealth;
            }
            else
            {
                bossHealthCanvas.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    void OnGUI()
    {
        GUI.Label(new Rect(100, 10, 400, 50), "Health: " + health);
    }
}
