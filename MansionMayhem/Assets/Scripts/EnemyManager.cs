using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    #region Enemy Attributes
    // Attributes
    public enemyType monster;
    private enemyClass monsterType;
    private float life;
    private float damage;
    private float speedAttribute;
    private bool invincibility;
    #endregion

    #region PlayerProperties
    public float Life
    {
        get { return life; }
        set { life = value; }
    }
    public float Damage
    {
        get { return damage; }
    }
    public float SpeedAttribute
    {
        get { return speedAttribute; }
    }
    public enemyType Monster
    {
        get { return monster; }
    }
    public enemyClass MonsterType
    {
        get { return monsterType; }
    }
    #endregion

    #region Initialization for Enemy
    // Use this for initialization
    void Start ()
    {
        // Sets the monster starting variables based on the type of monster you get
        switch (monster)
        {
            #region spiders
            case enemyType.smallSpider:
                life = 1;
                speedAttribute = 1;
                damage = .5f;
                monsterType = enemyClass.Spider;
                break;

            #endregion

            #region ghosts
            case enemyType.basicGhost:
                life = 2;
                speedAttribute = 1;
                damage = 1;
                monsterType = enemyClass.Ghost;
                break;
            #endregion

            #region demons
            case enemyType.imp:
                life = 3;
                speedAttribute = 2;
                damage = 1;
                monsterType = enemyClass.Demon;
                break;
            #endregion

            #region shades
            case enemyType.shadeKnight:
                life = 4;
                speedAttribute = .75f;
                damage = 1;
                monsterType = enemyClass.Shade;
                break;
            #endregion

            default:
                life = 1;
                speedAttribute = 1;
                damage = .5f;
                break;
        }

    }
    #endregion

    #region Update for Enemy
    void Update()
    {

    }
    #endregion
}
