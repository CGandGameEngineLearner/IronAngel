using System;
using System.Collections.Generic;
using UnityEngine;

public enum AmmunitionType
{
    Bullet = 0,
}

public enum WeaponType
{
    ShotGun = 0,
    Laser = 1,
    MissileLauncher = 2,
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon/WeaponConfig", order = 1)]
public class WeaponConfig : ScriptableObject
{
    // 武器类型
    public WeaponType weaponType;

    // 武器挂载子弹
    public AmmunitionConfig ammunitionConfig;

    // 射击间隔(等同于激光的判定间隔)
    public float interval;

    // 弹夹大小
    public float magSize;

    // 同时射击数量
    public int simShots;

    // 声音素材

    // 特效素材
}

// WeaponHandle: HP Sheild Mag Object
// AmmuntionHandle: Start Dir Object

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon/AmmunitionConfig", order = 1)]
public class AmmunitionConfig : ScriptableObject
{
    // 子弹类型
    public AmmunitionType ammunitionType;
    public float speed;
    public float lifeDistance;
}

// 由WeaponConfig可以得到AmmunitionConfig，通过注册武器GameObject可以得到索引表。武器子弹由武器得到索引关系
public class WeaponSystemCenter
{
    // // 武器表
    // private Dictionary<GameObject, WeaponConfig> m_WeaponConfigs = new();
    //
    // // 子弹表
    // private Dictionary<GameObject, AmmunitionHandle> m_AmmunitionConfigs = new();

    private WeaponUpdater m_WeaponUpdater = new();
    private AmmunitionUpdater m_AmmunitionUpdater = new();

    /// <summary>
    /// 更新子弹
    /// </summary>
    public void Update()
    {
        m_AmmunitionUpdater.Update();
    }

    // TODO:爆炸物可能同时影响到多个unit，内部已经处理ammunition重复加入的问题，这里只是需要拿到表
    // 但是之前就被注销掉的ammunition，后续就拿不到映射关系

    /// <summary>
    /// 伤害判定流程
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="ammunition"></param>
    public void JudgeWithAmmunition(GameObject unitAtker, GameObject unitToDamage, GameObject ammunition)
    {
        InternalUnRegisterAmmunition(ammunition);
    }

    public void FireWith(GameObject weapon, Vector2 startPoint, Vector2 dir)
    {
        if (!m_WeaponUpdater.HasWeapon(weapon)) throw new Exception("This weapon is not in WeaponUpdater");

        WeaponConfig weaponConfig = m_WeaponUpdater.GetWeaponHandle(weapon).weaponConfig;
        AmmunitionType ammunitionType = weaponConfig.ammunitionConfig.ammunitionType;
        // 注册子弹
        var ammunition = ObjectPoolManager.Instance.GetObject(weaponConfig.ammunitionConfig.ammunitionType.ToString());
        InternalRegisterAmmunition(ammunition, weaponConfig.ammunitionConfig, weaponConfig.weaponType, startPoint, dir);
    }

    public void RegisterWeapon(GameObject weapon, WeaponConfig weaponConfig)
    {
        m_WeaponUpdater.RegisterWeapon(weapon, weaponConfig);
    }

    public void UnRegisterWeapon(GameObject weapon)
    {
        m_WeaponUpdater.UnRegisterWeapon(weapon);
    }

    private void InternalUnRegisterAmmunition(GameObject ammunition)
    {
        m_AmmunitionUpdater.UnRegisterAmmunition(ammunition);
    }

    private void InternalRegisterAmmunition(GameObject ammunition, AmmunitionConfig ammunitionConfig,
        WeaponType weaponType, Vector2 startPoint,
        Vector2 dir)
    {
        m_AmmunitionUpdater.RegisterAmmunition(ammunition, ammunitionConfig, weaponType, startPoint, dir);
    }
}