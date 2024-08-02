using UnityEngine;
using UnityEngine.Serialization;

public enum AtkType
{
    ShotGun = 0,
    Laser = 1,
    MissileLauncher = 2,
    Default,
}

public enum AmmunitionType
{
    // 产生爆炸效果的炸弹
    PostProcess_Expolde = 0,
    
    // 实际飞行的子弹/激光/导弹 
    Bullet,
    None,
}

public enum WeaponType
{
    Glock,
    Missile,
}

public enum SpecialAtkType
{
    None,
}





