using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region GameState Enums
/// <summary>
/// Enum that contains all the GameStates available
/// </summary>
public enum GameState
{
    MainMenu,
    Instructions,
    Game,
    Paused,
    GameOver,
    LevelAdvance,
}
#endregion

#region Levels
/// <summary>
/// Enum that contains all levels available
/// </summary>
public enum levelType
{
    task,
    boss,
    extermination,
}
#endregion

#region PlayerWeapons
/// <summary>
/// Enum that contains all levels available
/// </summary>
public enum rangeWeapon
{
    // Range Weapons

    // Default Weapon
    laserpistol = 0,

    // Unlockable Weapons
    antiEctoPlasmator = 1,  // Ghost Splatter Weapon
    aetherLightBow = 2, // Demon Slaying Bow rumored to be from the gods
    cryoGun = 3, // FreezeRay
    hellfireshotgun = 4,    // Shotgun
    flamethrower = 5,   // Flamethrower
    soundCannon = 6,    // Three Round Burst Sound Cannon
}

/// <summary>
/// Enum that contains all levels available
/// </summary>
public enum meleeWeapon
{
    // Knife
    silverknife,

    // Whip
    hellWhip, // Spirit Killing Whip

    // Swords
    electricSword, // Basic Sword
    seraphBlade, // Demon Slaying Sword

}

public enum trinkets
{
    antiMatterBomb,
    blackHoleDevice,
    nanoHealingSwarm,
    drone,
}

public enum bulletTypes
{
    laser,
    aetherlight,
    antiEctoPlasm, 
    ice,
    hellFire,
    ectoPlasm,
    sound,
    web,
    splatterWeb,
    blackSlime,
}

public enum bulletOwners
{
    player,
    enemy,
}
#endregion

#region Enemy Enums
#region EnemyTypes
public enum enemyType
{
    // Spiders (0-20)
    smallSpider = 0,                    //art done
    blackWidow = 1, // Venom            //art done
    redTermis = 2,  // Venom            //art done
    fatalCrimson = 3,   // Venom        //art done
    noxiousLongleg = 4, // Venom        //art done
    tarantula = 5,  // Huge             //art done
    wolfSpider = 6, // Jumps            //art done
    silkSpinnerSpider = 7, // Webs      //art done
    spiderQueen = 10,        //Spider Boss   art done

    // Ghosts (30-50)
    // Can move through objects
    basicGhost = 30, 
    ghostknight = 31,
    ghosthead = 32,
    banshee = 33,
    wraith = 34,
    bansheeMistress = 35,    //Ghost Boss

    // Zombies (60-80)
    crawlingHand = 60,       // art done
    basicZombie = 61,        // art done
    crawlingZombie = 62,      // art done
    gasZombie = 63,          // art done
    stalkerZombie = 64,      // art done
    spitterZombie = 65,      // art done
    fatZombie = 66,          // art done
    tankZombie = 67,         // art done
    necroZombie = 68,
    elitezombie = 69,
    zombiehordeLeader = 70,  //Zombie Boss   
    runnerZombie = 71,

    // Skeletons (90 - 110)
    skeleHand = 90,
    flameSkull = 91,
    archerSkeleton = 92,
    warriorSkeleton = 93,
    charredSkeleton = 94,
    mageSkeleton = 95,
    giantSkeleton = 96,
    skeletonDragon = 97,     //Skeleton Boss 
    necromancer = 98,        //Skeleton Boss 
    skeleGiant = 99,
    knightSkeleton = 100,
    basicSkeleton = 101,

    // Demons (120-140)
    imp = 120, // art done
    frostimp = 121, // art done
    darkimp = 122, // art done
    boneDemon = 123,  // art done
    shadowDemon = 124,    // art done
    spikeDemon = 125,     // art done
    slasherDemon = 126,
    corruptedDemon = 127,    // art done
    infernalDemon = 128,      // art done
    frostDemon = 129,         // art done
    darkDemon = 130,         // art done
    hellhound = 131,
    fury = 132,
    gargoyle = 133,           // art done
    demonLord = 134,          //Demon Boss    art done
    cerberus = 135,           //Demon Boss

    // Shadow (150-170)
    shadeKnight = 150,
    shadow = 151,             // art done
    shadowBehemoth = 152,     //Shadow Boss


    // Mucks (180-200)
    blackMuck = 180,          // art done
    purpleSludgeMuck = 181,      // art done
    ectoplasmMuck = 182,         // art done

    // Elementals (210-220)
    infernalElemental = 210,   //Burn
    blackFireElemental = 211,  //Burn
    acidicElemental = 212,     //Venom
    pyreLord = 213,           //Elemental Boss
    darkElemental = 214,

    // Beasts (230-250)
    shadowBeast = 230,
    bloodBeast = 231,
    boneBeast = 232,
    flameBeast = 233,

    // Bats (260-280)
    basicBat = 260,
    giantBat = 261,
    bloodBat = 262,
    giantBloodBat = 263,


    // Boss
    /*
    giantGhast,         //Ghost Boss
    lilith,             //Demon Boss
    grimReaper,         //Shadow Boss
    dreor,              //None
    */

    // Other
    //possessedArmor ,
    // Muscle Monster   // art done

}
#endregion

#region Enemy Class
public enum enemyClass
{
    None,
    Spider,
    Ghost,
    Demon,
    Shade,
    Possesed,
    Skeleton,
    Zombie,
    Cthulhians,
    Elementals,
    Gargoyles,
    Werewolf,
    Muck,
    Shadow,
    Snake,
    Mummies,
    bats,
}
#endregion

#region abilities
public enum abilityType
{
    blobs,
    babies,
}
#endregion

#region movementType
public enum movementType
{ 
    stationary, // Does not move
    wander, // Wanders around the room
    seek,   // Goes towards the enemy player
    pursue, // Goes towards the enemy player and anticipates where it's moving toward
    flee,   // Run away from the player
    evade,  // Run away from teh player's future position
    dodging,    // Move in an unpredicted manor when the player shoots
    mimic,  // Moves when the player moves
    notLookingSeek, // Moves when the player isn't looking at it
}
#endregion

#region Boss
public enum BossEnum
{
    DemonLord,
    Dreor,
}
#endregion
#endregion


#region NPC Enums
public enum ResponseType
{
    SayNothing,
    SayYes,
    SayNo,
}
public enum QuestStatus
{
    NotStarted,
    Started,
    PartialCompletion,
    Completed,
}

#endregion

    #region itemTypes
    // Enums for ItemTypes
public enum itemType
{
    #region Ammo
    // Player Ammo Items
    normalScrewPickup,
    redScrewPickup,
    goldenScrewPickup,
    antiEctoplasmAmmo,
    aetherLightAmmo,
    heartPickup,
    healthPotionPickup,
    #endregion

    #region Weapons
    // Weapon
    aetherLightBow,
    antiEctoplasmGun,
    antiMatterGun,
    #endregion

    #region Quest Pieces
    // Quest Pieces
    quest,
    key,
    #endregion

    healthKit,
    goldenHeart,
}
#endregion

#region Direction
public enum direction
{
    Down = -2,
    Left = -1,
    Right = 1,
    Up = 2,
}
#endregion

#region RoomTypes
public enum RoomType
{
    entrance,
    elevator,
    boss,
    small,
    medium,
    large,
    /*
    CloakRoom,
    GrandFoyer,
    LivingRoom,
    Library,
    Kitchen,
    DiningRoom,
    Study,
    Observatory,
    BillardRoom,
    PowderRoom,
    BallRoom,
    Bedroom,
    HunterRoom,
    BoilerRoom,
    FurnaceRoom,
    UndergroundLake,*/
}
#endregion
