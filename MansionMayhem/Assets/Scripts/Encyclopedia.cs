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
    antiEctoPlasm, // Spirit Killing Whip
    ice, // Ice
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
#endregion

#region itemTypes
// Enums for ItemTypes
public enum itemType
{
    #region Ammo
    // Player Ammo Items
    coinPickup,
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

#region Animation
// Player Enums
/// <summary>
/// Direction of the Player
/// </summary>
public enum AnimState
{
    Idle,
    Walk,
    Hurt,

    // More Specific Animation Enums for items
    Secondary,
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
