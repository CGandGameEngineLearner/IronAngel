using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum AtkType
{
    [Tooltip("步枪，全自动，半自动等等普通弹道武器")] Rifle = 0,

    [Tooltip("激光类武器")] Laser = 1,

    [Tooltip("追踪类武器(还是饼)")] MissileLauncher = 2,

    [Tooltip("霰弹枪")] ShotGun = 3,

    Default,
}

public enum AmmunitionType
{
    Start,

    // 实际飞行的子弹/激光/导弹 -----------------------
    // 突击炮子弹
    AssaultGunAmmunition,

    // 霰弹炮子弹
    ShotGunAmmunition,

    // 狙击炮子弹
    SniperCannonAmmunition,

    // 火神炮子弹
    VulcanCannonAmmunition,

    // 105电磁炮子弹
    _105RailGunAmmunition,

    // 战斗激光枪子弹
    CombatLaserGunAmmunition,

    // 重型激光枪子弹
    HeavyLaserCannonAmmunition,

    // 火箭筒子弹
    RocketLauncherAmmunition,

    // 导弹发射器子弹
    MissileLauncherAmmunition,

    // 重型榴弹炮子弹
    HeavyHowitzerAmmunition,
    
    // 骑士投矛
    KnightPilumAmmunition,
    
    // 火箭弹仓子弹
    RocketPodAmmunition,

    // 分隔符  --------------- 
    PostExplodeSplitter,

    // 产生爆炸效果的炸弹 ------------------
    None,

    // 火箭筒子弹产生的爆炸
    RocketLauncherPostExplode,

    // 导弹发射器子弹产生的爆炸
    MissileLauncherPostExplode,

    // 重型榴弹炮子弹产生的爆炸
    HeavyHowitzerPostExplode,
    
    // c4炸弹爆炸
    ExplosivePostExplode,
    
    // EMP爆炸
    EmpPostExplode,
    
    // 火箭发射仓爆炸
    RocketPodPostExplode,
    
    Count,
}

public enum WeaponType
{
    // 普通武器 ------------------------------
    // 
    // 选择没有武器
    None,
    
    // 突击炮
    AssaultGun,

    // 散弹枪
    ShotGun,

    // 狙击炮
    SniperCannon,

    // 火神炮
    VulcanCannon,

    // 105电磁炮
    _105RailGun,

    // 战斗激光枪
    CombatLaserGun,

    // 重型激光枪
    HeavyLaserCannon,

    // 火箭筒
    RocketLauncher,

    // 导弹发射器
    MissileLauncher,

    // 重型榴弹炮
    HeavyHowitzer,
    
    // -- 特殊武器
    SPExplosiveLuncher,
    SPKnightPilumLuncher,
    SPEMPLuncher,
    SPRocketPodLuncher,
}

public enum SpecialAtkType
{
    None,
    Armor,
    EnergyShield
}

public enum VfxType
{
    // 射击视觉特效类型 ----------------------
    None,
    AssaultGunFireVfx,
    ShotGunFireVfx,
    SniperCannonFireVfx,
    VulcanCannonFireVfx,
    _105RailGunFireVfx,
    CombatLaserGunFireVfx,
    HeavyLaserCannonFireVfx,
    RocketLauncherFireVfx,
    MissileLauncherFireVfx,
    HeavyHowitzerFireVfx,
    
    //
    AssaultAmmunitionHitVfx,
    ShotGunAmmunitionHitVfx,
    SniperCannonAmmunitionHitVfx,
    VulcanCannonAmmunitionHitVfx,
    _105RailGunAmmunitionHitVfx,
    CombatLaserGunAmmunitionHitVfx,
    HeavyLaserCannonAmmunitionHitVfx,
    RocketLauncherAmmunitionHitVfx,
    MissileLauncherAmmunitionHitVfx,
    HeavyHowitzerAmmunitionHitVfx,
    //
    Hole1,
    Hole2,
    Hole3,
    Scrap1,
    Scrap2,
    Scrap3,
    
    // 角色死亡特效
    MachaDied,
    HumanDied,
    
    // 
    EmpVfx,
}

public enum SfxType
{
    // 射击音效类型 --------------------------
    None,
    AssaultGunFireSfx,
    ShotGunFireSfx,
    SniperCannonFireSfx,
    VulcanCannonFireSfx,
    _105RailGunFireSfx,
    CombatLaserGunFireSfx,
    HeavyLaserCannonFireSfx,
    RocketLauncherFireSfx,
    MissileLauncherFireSfx,
    HeavyHowitzerFireSfx,
    // 
    AssaultAmmunitionHitSfx,
    ShotGunAmmunitionHitSfx,
    SniperCannonAmmunitionHitSfx,
    VulcanCannonAmmunitionHitSfx,
    _105RailGunAmmunitionHitSfx,
    CombatLaserGunAmmunitionHitSfx,
    HeavyLaserCannonAmmunitionHitSfx,
    RocketLauncherAmmunitionHitSfx,
    MissileLauncherAmmunitionHitSfx,
    HeavyHowitzerAmmunitionHitSfx,
    
    // add on
    ExplosiveHitSfx,
    KnightPilumHitSfx,
    EMPHitSfx,
    RocketPodHitSfx
}