using UnityEngine;
using UnityEngine.Serialization;

public enum AtkType
{
    Rifle = 0,
    Laser = 1,
    MissileLauncher = 2,
    ShotGun = 3,

    Default,
}

public enum AmmunitionType
{
    // 默认没有类型
    None,
    // 产生爆炸效果的炸弹
    PostProcess_Expolde,
    // 产生激光判定的实体
    PostProcess_Laser,
    // 实际飞行的子弹/激光/导弹 
    Bullet,
    ThinLaser,
    ThickLaser,
}

public enum WeaponType
{
    Glock,
    Missile,
}

public enum SpecialAtkType
{
    None,
    Test,
}