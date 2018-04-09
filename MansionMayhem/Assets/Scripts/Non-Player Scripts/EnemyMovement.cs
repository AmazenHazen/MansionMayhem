using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    #region Additional Movement Variables
    // Variables for enemy targeting
    public GameObject player;
    private bool readyToMove;
    private bool resettingMovement;

    // Attributes for CalcSteeringForces Method
    public float maxForce;


    // Current enemy phase
    int phase;
    #endregion

    #region Start Method
    public override void Start()
    {
        readyToMove = true;
        resettingMovement = false;
        currentSpeed = maxSpeed;
        player = GameObject.FindGameObjectWithTag("player");
    }
    #endregion

    #region Update Method
    protected override void Update()
    {
        base.Update();
        phase = gameObject.GetComponent<EnemyManager>().Phase;

    }
    #endregion

    #region Enemy Movement Method
    // Call the necessary Forces on the enemy
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
                    ultimateForce += pursue(player);
                    break;
                case enemyType.redTermis:
                    ultimateForce += pursue(player);
                    break;
                case enemyType.tarantula:
                    // No movement
                    break;
                case enemyType.wolfSpider:     
                    // Jumping Movement
                    if (readyToMove)
                    {
                        ultimateForce += seek(player.transform.position)*50;
                        readyToMove = false;
                        resettingMovement = true;
                    }
                    else if(resettingMovement)
                    {
                        resettingMovement = false;
                        Invoke("ResetMoveBool", 1);
                    }
                    break;
                case enemyType.silkSpinnerSpider:
                    ultimateForce += pursue(player);
                    break;
                case enemyType.fatalCrimson:
                    if (readyToMove)
                    {
                        ultimateForce += seek(player.transform.position) * 100;
                        readyToMove = false;
                        resettingMovement = true;
                    }
                    else if (resettingMovement)
                    {
                        resettingMovement = false;
                        Invoke("ResetMoveBool", 1);
                    }
                    break;

                case enemyType.spiderQueen:
                    ultimateForce += pursue(player);
                    break;

                #endregion

                #region ghosts
                // Ghosts
                case enemyType.basicGhost:
                    ultimateForce += seek(player.transform.position);
                    break;


                case enemyType.ghostknight:

                    break;

                case enemyType.banshee:
                    // Seek
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.ghosthead:
                    // Seek
                    ultimateForce += seek(player.transform.position);
                    break;


                case enemyType.wraith:
                    ultimateForce += pursue(player);

                    // Jumping Movement
                    if ((player.transform.position - transform.position).magnitude < 3f)
                    {
                        //maxSpeed = 5.5f;
                        if (readyToMove)
                        {
                            ultimateForce += pursue(player) * 50;
                            readyToMove = false;
                            resettingMovement = true;
                        }
                        else if (resettingMovement)
                        {
                            resettingMovement = false;
                            Invoke("ResetMoveBool", 5);

                        }
                    }
                    break;

                case enemyType.bansheeMistress:

                    break;

                case enemyType.prisonLeader:
                    // First Phase: Stationary summoning ghost heads
                    // Do nothing here for movement

                    // Second Phase: Seek Player and throwing weights
                    if (phase == 1)
                    {
                        // Seek
                        ultimateForce += seek(player.transform.position);
                    }
                    if (phase == 2)
                    {
                        // Seek
                        ultimateForce += pursue(player);
                    }

                    // Third Phase he gets faster
                    if (phase == 2 && maxSpeed!=7)
                    {
                        mass = 1.75f;
                        maxSpeed = 7;
                    }


                    break;
                #endregion

                #region demons
                // Demons
                case enemyType.imp:

                    // Seek
                    ultimateForce += seek(player.transform.position);
                        break;
                case enemyType.darkimp:
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

                case enemyType.demonLord:

                    break;
                case enemyType.cerberus:

                    break;

                #endregion

                #region zombies
                case enemyType.crawlingHand:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.crawlingZombie:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.basicZombie:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.stalkerZombie:
                    ultimateForce += seek(player.transform.position);

                    // Jumping Movement
                    if ((player.transform.position - transform.position).magnitude < 1.75f)
                    {
                        //maxSpeed = 5.5f;
                        if (readyToMove)
                        {
                            ultimateForce += seek(player.transform.position) * 75;
                            readyToMove = false;
                            resettingMovement = true;
                        }
                        else if (resettingMovement)
                        {
                            resettingMovement = false;
                            Invoke("ResetMoveBool", 3);
                        }
                    }
                    break;
                case enemyType.runnerZombie:
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.gasZombie:
                    ultimateForce += pursue(player);

                    // Jumping Movement
                    if ((player.transform.position - transform.position).magnitude < 1.75f)
                    {
                        //maxSpeed = 5.5f;
                        if (readyToMove)
                        {
                            ultimateForce += seek(player.transform.position) * 50;
                            readyToMove = false;
                            resettingMovement = true;
                        }
                        else if (resettingMovement)
                        {
                            resettingMovement = false;
                            Invoke("ResetMoveBool", 3);

                        }
                    }
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
                case enemyType.zombiehordeLeader:
                    ultimateForce += pursue(player);
                    break;




                #endregion

                #region skeletons
                case enemyType.basicSkeleton:
                case enemyType.skeleHand:
                case enemyType.eliteSkeleArcher:
                case enemyType.eliteSkeleMage:
                case enemyType.eliteSkeleWarrior:
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.archerSkeleton:

                    break;

                case enemyType.warriorSkeleton:
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.mageSkeleton:

                    break;

                case enemyType.giantSkeleton:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.skeletonDragon:

                    break;

                case enemyType.necromancer:

                    break;

                #endregion

                #region Muck
                case enemyType.oilSlime:
                case enemyType.bloodSlime:
                case enemyType.phlegmSlime:
                case enemyType.mucusSlime:
                    // Jumping Movement
                    if (readyToMove)
                    {
                        ultimateForce += seek(player.transform.position) * 50;
                        readyToMove = false;
                        resettingMovement = true;
                    }
                    else if (resettingMovement)
                    {
                        resettingMovement = false;
                        Invoke("ResetMoveBool", 1);
                    }
                    break;

                case enemyType.mucusMuck:
                case enemyType.phlegmMuck:
                case enemyType.bloodMuck:
                case enemyType.oilMuck:
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.ectoplasmMuck:
                    ultimateForce += seek(player.transform.position);
                    break;


                case enemyType.purpleSludgeMuck:
                    ultimateForce += seek(player.transform.position);
                    break;

                #endregion

                #region shadows
                case enemyType.shadeKnight:

                    break;

                case enemyType.shadow:

                    break;
                case enemyType.shadowBehemoth:

                    break;
                #endregion

                #region Elementals
                case enemyType.infernalElemental:

                    break;

                case enemyType.blackFireElemental:

                    break;

                case enemyType.acidicElemental:

                    break;

                case enemyType.pyreLord:

                    break;

                #endregion

                #region Beasts
                case enemyType.shadowBeast:

                    break;
                case enemyType.flameBeast:

                    break;
                case enemyType.boneBeast:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.bloodBeast:

                    break;

                #endregion

                #region bats
                case enemyType.basicBat:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.giantBat:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.bloodBat:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.giantBloodBat:
                    ultimateForce += seek(player.transform.position);
                    break;

                #endregion

                #region other
                case enemyType.gargoyle:

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

    #region Advanced Movement Methods
    /// <summary>
    /// A delay for a movement method.
    /// Helps for rapid movement monsters
    /// </summary>
    void ResetMoveBool()
    {
        //Debug.Log("Reset");
        readyToMove = true;
        resettingMovement = false;
    }
    #endregion

    #region Revert Speed Method for Enemies
    /// <summary>
    /// Returns Speed to Max Speed
    /// </summary>
    protected override void RevertSpeed()
    {
        // Reset speed if you are slowed
        if (currentSpeed < maxSpeed && beingSlowed == false)
        {
            currentSpeed += .05f;
        }

        //Reset speed if on slippery surface
        if (currentSpeed > maxSpeed && beingSped == false)
        {
            currentSpeed -= .05f;
        }

        // Don't allow speed to be negative or 0
        if (currentSpeed < .25f)
        {
            currentSpeed = .25f;
        }

        // Don't allow speed to be too high
        if (currentSpeed > 6f)
        {
            currentSpeed = 6f;
        }
    }
    #endregion
}
