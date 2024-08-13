
using System;
using UnityEngine;

/// <summary>
/// 新加的敌人类型都往后面排 别在中间插入
/// </summary>
public enum EnemyType
{
    None,
    
    // Tank
    HowitzerTank,
    RailgunTank1,
    
    // Soldier
    NOWEAPON,
    Rifleman,
    Rocketman,
    
    // PowerSuit
    Suit_Assault_Shield,
    Suit_CLaser_Shield,
    Suit_Rocket_Shield,
    
    // Mecha
    EliteMecha_HLaserAndMissile,
    EliteMecha_HLaserAndMissile_Shield,
    EliteMecha_ShotgunAndVulcan_Shield,
    EliteMecha_SniperAndMissile_Shield,
    EliteMecha_VulcanAndMissile_Shield,
    VanillaMecha_AssaultAndShotgun_Shield,
    VanillaMecha_RocketAndCLaser_Shield,
    VanillaMecha_RocketAndSniper_Shield,
    
    // Turret
    HeavyTurret_HeavyHowitzer,
    HeavyTurret_RailGun,
    LightTurret_Assault,
    
    // Test
    AItest,
    OriginEdition,
    
    //Drone
    Drone,
    
    // 新增的都排在下面↓
    
    
    Count                // 用于计数
}

[Serializable]
public class EnemySetting
{
    [SerializeField]
    public EnemyType EnemyType = EnemyType.None;

    [SerializeField]
    public GameObject EnemyPrefab = null;
    
}
