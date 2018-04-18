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
    Play,
    Paused,
    Died,
    CompleteLevel,
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

#region Weapons
/// <summary>
/// Enum that contains all levels available
/// </summary>
public enum rangeWeapon
{
    // Range Weapons
    None = -1,

    // Default Weapon
    laserpistol = 0,

    // Unlockable Weapons
    antiEctoPlasmator = 1,  // Ghost Splatter Weapon
    aetherLightBow = 2, // Demon Slaying Bow rumored to be from the gods
    cryoGun = 3, // FreezeRay
    hellfireshotgun = 4,    // Shotgun
    flamethrower = 5,   // Flamethrower
    soundCannon = 6,    // Three Round Burst Sound Cannon
    ElectronSeeker = 7, // Gun with seeking bullets
    CelestialRepeater = 8,
    PlasmaCannon = 9,
    DarkEnergyRifle = 10,
    XenonPulser = 11,
    AntimatterParticle = 12,
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
    electron,
    ElectronBall,
    PortalShot,
    Portal,
    Plasma,
    Weight,
    Magic,
    CelestialCrystal,
    DarkEnergy,
    Xenon,
    blood,
    mucus,
    phlegm,
}

public enum trinkets
{
    None = -1,
    antiMatterBomb =1,
    portalDevice=2,
    nanoHealingSwarm=3,
    drone=4,
    hologramClone=5,
}


public enum bulletOwners
{
    player,
    enemy,
}
#endregion

#region Equipement
/// <summary>
/// Enum that contains all levels available
/// </summary>
public enum Equipment
{
    None = -1,
    FireResistantArmor =0,
    HeatedCoatLining=1,
    FrictionBoots=2,
    AntidotePatch=3,
    ScrewMagnet=4,
    BootRockets=5,
    RoboticHeart=6,
    DragonScaleArmor=7,
    AntiGooTreatment=8,
    ShadowVeilRing=9,
}
#endregion

#region UnlockVariable Enum
public enum Unlock
{
    // Heart Unlocks
    heartIncrease = 0,
    equipmentIncrease = 1,

    // Gun Unlocks
    // Laser Pistol
    LaserPistol = 5,
    LaserPistolUpgrade1 = 6,
    LaserPistolUpgrade2 = 7,
    LaserPistolUpgrade3 = 8,

    // Anti Ectoplasm Splatter Gun
    AntiEctoGun = 10,
    AntiEctoGunUpgrade1 = 11,
    AntiEctoGunUpgrade2 = 12,
    AntiEctoGunUpgrade3 = 13,

    // Plasma Pistol
    PlasmaPistol = 15,
    PlasmaPistolUpgrade1 = 16,
    PlasmaPistolUpgrade2 = 17,
    PlasmaPistolUpgrade3 = 18,

    // CryoGun
    CryoGun = 20,
    CryoGunUpgrade1 = 21,
    CryoGunUpgrade2 = 22,
    CryoGunUpgrade3 = 23,

    // Flamethrower
    FlameThrower = 25,
    FlameThrowerUpgrade1 = 26,
    FlameThrowerUpgrade2 = 27,
    FlameThrowerUpgrade3 = 28,

    // Hellfire Shotgun
    HellFireShotgun = 30,
    HellFireShotgunUpgrade1 = 31,
    HellFireShotgunUpgrade2 = 32,
    HellFireShotgunUpgrade3 = 33,
    
    // Sound Cannon
    SoundCannon = 35,
    SoundCannonUpgrade1 = 36,
    SoundCannonUpgrade2 = 37,
    SoundCannonUpgrade3 = 38,

    // Dark Energy Sniper
    DarkEnergySniper = 40,
    DarkEnergySniperUpgrade1 = 41,
    DarkEnergySniperUpgrade2 = 42,
    DarkEnergySniperUpgrade3 = 43,

    // Electron Cannon
    ElectronPulseCannon = 45,
    ElectronPulseCannonUpgrade1 = 46,
    ElectronPulseCannonUpgrade2 = 47,
    ElectronPulseCannonUpgrade3 = 48,

    // Aetherlight Bow
    AetherlightBow = 50,
    AetherlightBowUpgrade1 = 51,
    AetherlightBowUpgrade2 = 52,
    AetherlightBowUpgrade3 = 53,

    // Celestial Repeater
    CelestialRepeater = 55,
    CelestialRepeaterUpgrade1 = 56,
    CelestialRepeaterUpgrade2 = 57,
    CelestialRepeaterUpgrade3 = 58,

    // Xenon Pulser
    XenonPulser = 60,
    XenonPulserUpgrade1 = 61,
    XenonPulserUpgrade2 = 62,
    XenonPulserUpgrade3 = 63,

    // Antimatter Particle Gun
    AntiMatterParticle = 65,
    AntiMatterParticleUpgrade1 = 66,
    AntiMatterParticleUpgrade2 = 67,
    AntiMatterParticleUpgrade3 = 68,

    // Trinket Unlocks
    AntiMatterDevice = 100,
    PortalDevice = 101,
    NanoBotHealingSwarm = 102,
    Drone = 103,
    HologramClone = 104,
    Shield = 105,
    BootRockets = 106,


    // Equipment Unlocks
    FireResistantUnderArmor = 200,
    HeatedCoatLining = 201,
    FrictionBoots = 202,
    AntidotePatch = 203,
    ScrewMagnet = 204,
    RoboticHeart = 205,
    DragonscaleArmor = 206,
    GooRepellingTreatment = 207,
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
    prisonerGhost = 36,
    prisonLeader = 37,

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
    eliteSkeleArcher = 102,
    eliteSkeleMage = 103,
    eliteSkeleWarrior = 104,

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
    oilSlime = 180,
    oilMuck = 181,          // art done
    oilOozoma = 182,
    mucusSlime = 183, 
    mucusMuck = 184,
    mucusOozoma = 185,
    phlegmSlime = 186,
    phlegmMuck = 187,
    phlegmOozoma = 188,
    bloodSlime = 189,
    bloodMuck = 190,
    bloodOozoma = 191,
    BeastoftheFourBiles = 192,
    purpleSludgeMuck = 195,      // art done
    ectoplasmMuck = 196,         // art done

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
    skeleton,
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

#region Enemy Weapons
public enum EnemyWeapon
{
    leftchain,
    rightchain,
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
public enum InteractableObjectType
{
    Chest,
    Giver, // gives the player an item
    Taker, // takes from the player
}

#endregion

#region itemTypes
    // Enums for ItemTypes
public enum ItemType
{
    NoItem = -1,

    #region Currency
    // Player Ammo Items
    NormalScrewPickup = 0,
    RedScrewPickup = 1,
    GoldenScrewPickup = 2,

    #endregion

    #region Health Pickups
    HeartPickup = 10,
    HealthPotionPickup = 11,
    HealthKit = 12,
    GoldenHeart = 13,
    #endregion

    #region Keys
    GoldKey = 20,
    SilverKey = 21,
    BronzeKey = 22,
    BoneKey = 23,
    DemonicKey = 24,
    EyeKey = 25,
    RedKey = 26,
    WebKey = 27,
    IcedKey = 28,
    FireKey = 29,
    BlueKey = 30,
    MoonKey = 31,
    #endregion

    #region Quest Items
    // Demon In Need Level
    TwistedDemonHorn = 50,
    VialOfBlood =51,
    RedChalk =52,
    DemonicTome =53,

    // Purgatory Prison
    DeathClutchJournal = 54,

    // Forest of Dripping Blood
    Bucket = 80,
    TreeTap = 81,
    BucketOfBloodSap = 82,

    Harpoon = 83,
    Lamprey = 84,
    LampreyTeeth = 85,

    RawSteak = 86,
    SleepingMedication = 87,
    RabidSleepingMonsterAnimal = 88,


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
public enum RoomType
{
    entrance,
    elevator,
    boss,
    small,
    medium,
    large,
    ExtremelyLarge,
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
