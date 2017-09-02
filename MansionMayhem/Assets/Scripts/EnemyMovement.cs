using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    #region Additional Movement Variables
    // Variables for enemy targeting
    public GameObject player;

    // Attributes for CalcSteeringForces Method
    public float maxForce;
    #endregion

    #region Start Method
    public override void Start()
    {
        currentSpeed = maxSpeed;
        player = GameObject.FindGameObjectWithTag("player");
    }
    #endregion

    #region Update Method
    protected override void Update()
    {
        base.Update();
    }
    #endregion

    #region Enemy Movement Method
    // Call the necessary Forces on the player
    protected override void CalcSteeringForces()
    {
        // Create a new ultimate force that is zeroed out
        Vector3 ultimateForce = Vector3.zero;

        // Apply forces to the enemy
        if ((player.transform.position - transform.position).magnitude < gameObject.GetComponent<EnemyManager>().seekDistance)
        {
            // Have the enemy face the player
            Rotate();

            // Basic Enemy Movement
            switch (gameObject.GetComponent<EnemyManager>().monster)
            {
        
                #region spiders
                case enemyType.smallSpider:
                    // Seek
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.blackWidow:

                    break;
                case enemyType.redTermis:

                    break;
                case enemyType.tarantula:

                    break;
                case enemyType.wolfSpider:

                    break;
                case enemyType.silkSpinnerSpider:
                    // flee
                    ultimateForce += flee(player.transform.position);
                    break;


                #endregion

                #region ghosts
                // Ghosts
                case enemyType.basicGhost:

                    break;


                case enemyType.ghostknight:

                    break;

                case enemyType.banshee:
                    // Seek
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.ghosthead:

                    break;


                case enemyType.wraith:

                    break;
                #endregion

                #region demons
                // Demons
                case enemyType.imp:

                    // Seek
                    ultimateForce += seek(player.transform.position);
                        break;

                case enemyType.boneDemon:

                    break;

                case enemyType.corruptedDemon:

                    break;

                case enemyType.infernalDemon:

                    break;

                case enemyType.shadowDemon:

                    break;

                case enemyType.slasherDemon:

                    break;

                case enemyType.spikeDemon:

                    break;

                case enemyType.hellhound:

                    break;

                case enemyType.fury:

                    break;

                #endregion

                #region zombies
                case enemyType.crawlingHand:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.crawlerZombie:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.basicZombie:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.stalkerZombie:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.gasZombie:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.spitterZombie:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.fatZombie:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.tankZombie:
                    ultimateForce += seek(player.transform.position);
                    break;



                #endregion

                #region skeletons
                case enemyType.charredSkeleton:

                    break;

                case enemyType.archerSkeleton:

                    break;

                case enemyType.knightSkeleton:

                    break;

                case enemyType.mageSkeleton:

                    break;

                case enemyType.giantSkeleton:

                    break;


                #endregion

                #region Muck
                case enemyType.blackMuck:

                    break;

                case enemyType.ectoplasmMuck:
                    ultimateForce += seek(player.transform.position);
                    break;


                case enemyType.acidicMuck:
                    ultimateForce += seek(player.transform.position);
                    break;

                #endregion

                #region shadows
                case enemyType.shadeKnight:

                    break;

                case enemyType.shadow:

                    break;

                case enemyType.shadowBeast:

                    break;
                #endregion

                #region Elementals
                case enemyType.infernalElemental:

                    break;

                case enemyType.blackFireElemental:

                    break;

                case enemyType.acidicElemental:

                    break;
                #endregion

                #region other
                case enemyType.gargoyle:

                    break;
                case enemyType.possessedArmor:

                    break;

                #endregion

                #region Bosses
                case enemyType.giantGhast:

                    break;

                case enemyType.bansheeMistress:

                    break;

                case enemyType.demonLord:

                    break;

                case enemyType.cerberus:

                    break;

                case enemyType.lilith:

                    break;

                case enemyType.skeletonDragon:

                    break;

                case enemyType.necromancer:

                    break;


                case enemyType.zombiehordeLeader:
                    ultimateForce += pursue(player);
                    break;


                case enemyType.grimReaper:

                    break;


                case enemyType.shadowBehemoth:

                    break;


                case enemyType.spiderQueen:

                    break;


                case enemyType.pyreLord:

                    break;

                case enemyType.dreor:
                    break;


                    #endregion

                    #region default monster
                    default:

                    break;

                #endregion

            }

        }
        else
        {
            //ultimateForce = wander();
        }
        // Apply Decelleration using ApplyFriction Force
        //ultimateForce += ApplyFriction(3.0f);

        //Debug.Log("Before Clamp: " + ultimateForce);
        // Clamp the ultimate force by the maximum force
        Vector3.ClampMagnitude(ultimateForce, maxForce);

        // Ensure that the enemies do not move in the z-axis
        ultimateForce.z = 0;

        //Debug.Log("After Clamp: " + ultimateForce);
        ApplyForce(ultimateForce);
    }
    #endregion

    #region Enemy Rotate
    /// <summary>
    /// Rotates the player based on the direction its facing
    /// </summary>
    protected override void Rotate()
    {
        Vector3 targetPosition = player.transform.position;
        Vector3 dir = targetPosition - this.transform.position;
        angleOfRotation = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg)-90;
    }
    #endregion

}
