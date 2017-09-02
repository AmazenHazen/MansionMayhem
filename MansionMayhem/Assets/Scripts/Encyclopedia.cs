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
    // Spiders
    smallSpider,                    //art done
    blackWidow, // Venom            //art done
    redTermis,  // Venom            //art done
    fatalCrimson,   // Venom        //art done
    noxiousLongleg, // Venom        //art done
    tarantula,  // Huge             //art done
    wolfSpider, // Jumps            //art done
    silkSpinnerSpider, // Webs      //art done

    // Ghosts   // Can move through objects
    basicGhost, 
    ghostknight,
    ghosthead,
    banshee,
    wraith,

    // Zombie
    crawlingHand,       // art done
    basicZombie,        // art done
    crawlerZombie,      // art done
    gasZombie,          // art done
    spitterZombie,      // art done
    fatZombie,          // art done
    stalkerZombie,      // art done
    tankZombie,         // art done

    // Skeletons
    flameSkull,
    archerSkeleton,
    knightSkeleton,
    charredSkeleton,
    mageSkeleton,
    giantSkeleton,

    // Demons
    imp, // art done
    boneDemon,  // art done
    shadowDemon,    // art done
    spikeDemon,     // art done
    slasherDemon,
    corruptedDemon,    // art done
    infernalDemon,      // art done
    frostDemon,         // art done
    hellhound,
    fury,

    // Shadow
    shadeKnight,
    shadow,             // art done
    shadowBeast,

    // Elementals
    infernalElemental,   //Burn
    blackFireElemental,  //Burn
    acidicElemental,     //Venom

    // Mucks
    blackMuck,          // art done
    ectoplasmMuck,      // art done
    acidicMuck,         // art done

    // Other
    gargoyle,           // art done
    possessedArmor,
    // Muscle Monster   // art done

    // Boss
    giantGhast,         //Ghost Boss
    bansheeMistress,    //Ghost Boss
    demonLord,          //Demon Boss    art done
    cerberus,           //Demon Boss
    lilith,             //Demon Boss
    skeletonDragon,     //Skeleton Boss 
    necromancer,        //Skeleton Boss 
    zombiehordeLeader,  //Zombie Boss   
    grimReaper,         //Shadow Boss
    shadowBehemoth,     //Shadow Boss
    spiderQueen,        //Spider Boss   art done
    pyreLord,           //Elemental Boss
    dreor,              //None
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
}
#endregion

#region
public enum abilityType
{
    webs,
    blobs,
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
public enum roomType
{
    Entrance,
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
    UndergroundLake,
    Elevator,
}
#endregion
