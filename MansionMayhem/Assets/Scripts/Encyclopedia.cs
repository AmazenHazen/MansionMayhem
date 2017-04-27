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

#region LevelType
/// <summary>
/// Enum that contains all levels available
/// </summary>
public enum levelType
{
    Sacrifice,
    Boss,
    Collection,
}
#endregion

#region PlayerWeapons
/// <summary>
/// Enum that contains all levels available
/// </summary>
public enum rangeWeapon
{
    // Range Weapons
    // Ghost guns
    antiEctoPlasmator=0,
    // Bows
    aetherLightBow = 1, // Demon Slaying Bow rumored to be from the gods
    // Special
    cryoGun = 2, // FreezeRay
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
    knife,
    hellWhip, // Spirit Killing Whip
    electricSword, // 
    arcLightSword, // Demon Slaying Sword
}

public enum bulletTypes
{
    aetherlight,
    antiEctoPlasm, 
    ice, // Ice
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
    smallSpider,

    // Ghosts
    basicGhost,
    banshee,

    // Demons
    imp,

    // Shades
    shadeKnight,

    // Boss
    BansheeMistress,
}
#endregion

#region Enemy Class
public enum enemyClass
{
    Spider,
    Ghost,
    Demon,
    Shade,
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
